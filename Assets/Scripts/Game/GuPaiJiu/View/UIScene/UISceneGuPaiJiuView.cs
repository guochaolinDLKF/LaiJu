//===================================================
//Author      : DRB
//CreateTime  ：9/5/2017 10:43:49 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using proto.gp;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace GuPaiJiu
{
    public class UISceneGuPaiJiuView : UISceneViewBase
    {
        [SerializeField]
        private GuPaiJiuSeatCtrl[] m_Seats;
        [SerializeField]
        private Button m_ButtonReady;//准备
        [SerializeField]
        private Image m_btnAlreadyReady;//灰色准备
        [SerializeField]
        private Button m_ButtonShare;//微信邀请
        [SerializeField]
        private Button m_ButtonMicroPhone;//语音
        [SerializeField]
        private Button m_ButtonOpening;//开局
        [SerializeField]
        private GameObject m_CutPokerObj;//切牌或者不切   
     
        private ROOM_STATUS curRoomStatus;//切牌房间状态

        private Queue<int> CutPokerAniQueue = new Queue<int>(); //切牌队列


        const int countPerTimes = 2;//每次抓几张

        public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
        {
            Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
            dic.Add("OnSeatInfoChanged", OnSeatInfoChanged);//断线重连和创建房间使用
            dic.Add("OnSeatGameInfoChanged", OnSeatGameInfoChanged);//游戏中使用
            dic.Add(ConstantGuPaiJiu.ShuffleAnimation, ShuffleAnimation);//洗牌动画            
            dic.Add(ConstantGuPaiJiu.GroupEnd, GroupEnd);//组合牌结束
            dic.Add(ConstantGuPaiJiu.GroupEndJieSuan, GroupEndJieSuan);//结算的时候实例化别人的牌
            dic.Add(ConstantGuPaiJiu.PlayMusic, PlayMusic);//播放音乐
            dic.Add(ConstantGuPaiJiu.LoadSmallResult, LoadSmallResult);//每局小结算                           
            //dic.Add(ConstantGuPaiJiu.BigDealAniPoker, BigDealAniPoker);//发牌摇筛子前先移动四张牌
            dic.Add(ConstantGuPaiJiu.BigDealAni, BigDealAni);//发牌动画        
            return dic;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            EventTriggerListener.Get(m_ButtonMicroPhone.gameObject).onDown = OnBtnMouseDown;
            EventTriggerListener.Get(m_ButtonMicroPhone.gameObject).onUp = OnBtnMouseUp;
            if (!SystemProxy.Instance.IsInstallWeChat)
            {
                m_ButtonShare.gameObject.SetActive(false);
            }
            if (m_CutPokerObj != null) m_CutPokerObj.SetActive(false);

        }

        private void OnBtnMouseDown(PointerEventData eventData)
        {
            if (eventData.selectedObject == m_ButtonMicroPhone.gameObject)
            {
                UIViewManager.Instance.OpenWindow(UIWindowType.Micro);
            }
        }

        private void OnBtnMouseUp(PointerEventData eventData)
        {
            if (eventData.selectedObject == m_ButtonMicroPhone.gameObject)
            {
                if (eventData.pointerCurrentRaycast.gameObject == m_ButtonMicroPhone.gameObject)
                {
                    SendNotification("OnBtnMicroUp");
                }
                else
                {
                    Debug.Log("取消发送语音");
                    SendNotification("OnBtnMicroCancel");
                }
            }
        }

        protected override void OnBtnClick(GameObject go)
        {
            base.OnBtnClick(go);
            switch (go.name)
            {
                case ConstDefine.BtnSetting:
                    UIViewManager.Instance.OpenWindow(UIWindowType.Setting);
                    break;
                case ConstantGuPaiJiu.btnGuPaiJiuViewChat:
                    UIViewManager.Instance.OpenWindow(UIWindowType.Chat);
                    break;
                case ConstantGuPaiJiu.btnGuPaiJiuViewShare:
                    SendNotification(ConstantGuPaiJiu.btnGuPaiJiuViewShare);
                    break;
                case ConstantGuPaiJiu.btnGuPaiJiuViewReady:
                    SendNotification(ConstantGuPaiJiu.GuPaiJiuClientSendReady);
                    break;
                case ConstantGuPaiJiu.btnGuPaiJiuViewOpening:
                    SendNotification(ConstantGuPaiJiu.GuPaiJiuClientSendGameStart);
                    break;
            }
        }

        private void OnSeatInfoChanged(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("Seat");
            bool isPlayer = data.GetValue<bool>("IsPlayer");
            ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
            if (isPlayer)
            {
                SetBtn(seat, roomStatus);
            }
        }

        private void OnSeatGameInfoChanged(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("Seat");
            bool isPlayer = data.GetValue<bool>("IsPlayer");
            ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
            if (isPlayer)
            {
                SetBtn(seat, roomStatus);
            }
        }

        /// <summary>
        /// 设置按钮显隐
        /// </summary>
        /// <param name="seat"></param>
        /// <param name="roomStatus"></param>
        private void SetBtn(SeatEntity seat, ROOM_STATUS roomStatus)
        {
            m_ButtonReady.gameObject.SetActive(seat.seatStatus == SEAT_STATUS.IDLE);
            m_btnAlreadyReady.gameObject.SetActive(roomStatus == ROOM_STATUS.READY && seat.seatStatus == SEAT_STATUS.READY);
            m_ButtonOpening.gameObject.SetActive((roomStatus == ROOM_STATUS.READY || roomStatus == ROOM_STATUS.IDLE) && seat.IsBanker);
            m_ButtonShare.gameObject.SetActive(roomStatus == ROOM_STATUS.IDLE || roomStatus == ROOM_STATUS.READY);
            if (!SystemProxy.Instance.IsInstallWeChat)
            {
                m_ButtonShare.gameObject.SetActive(false);
            }
        }


        #region GetSeatCtrlBySeatPos 根据座位号获取座位控制器
        /// <summary>
        /// 根据座位号获取座位控制器
        /// </summary>
        /// <param name="seatPos"></param>
        /// <returns></returns>
        private GuPaiJiuSeatCtrl GetSeatCtrlBySeatPos(int Index)
        {
            for (int i = 0; i < m_Seats.Length; ++i)
            {
                if (m_Seats[i].SeatIndex == Index)
                {
                    return m_Seats[i];
                }
            }
            return null;
        }
        #endregion

        #region 洗牌动画
        /// <summary>
        /// 洗牌动画
        /// </summary>
        private void ShuffleAnimation(TransferData data)
        {
            Debug.Log("······················开始洗牌啦························");
            RoomEntity room = data.GetValue<RoomEntity>("Room");
            for (int i = 0; i < room.seatList.Count; i++)
            {
                if (room.seatList[i].IsBanker)
                {
                    GuPaiJiuSeatCtrl seatCtrl = GetSeatCtrlBySeatPos(room.seatList[i].Index);
                    Debug.Log(seatCtrl.SeatIndex + "                              洗牌Index");
                    //BankerSeat = room.seatList[i];
                    //if (wallList.Count != 0) wallList.Clear();
                    seatCtrl.ShuffleAnimation(room, (List<GameObject> listObj) =>
                     {
                         //this.wallList = listObj;
                     });
                    AudioEffectManager.Instance.Play("gp_xipai", Vector3.zero);
                }
            }
        }


        /// <summary>
        /// 发牌动画
        /// </summary>
        private void BigDealAni(TransferData data)
        {
            RoomEntity room = data.GetValue<RoomEntity>("Room");
            for (int i = 0; i < room.seatList.Count; i++)
            {
                if (room.seatList[i].IsBanker)
                {
                    GuPaiJiuSeatCtrl seatCtrl = GetSeatCtrlBySeatPos(room.seatList[i].Index);
                    StartCoroutine(DealAniTor(seatCtrl, room));
                }
            }

        }
        IEnumerator DealAniTor(GuPaiJiuSeatCtrl seatCtrl, RoomEntity room)
        {
            Debug.Log("```````````````````````````````````````开始发牌``````````````````````````````````````````````````````" + room.FirstGivePos);
            int loopCount = room.roomPlay == EnumPlay.BigPaiJiu ? 4 : 2;
            //int loopCount = 31;
            int firstGivePos = room.FirstGivePos;
            //按顺序发牌             
            for (int j = 0; j < room.seatList.Count; ++j)
            {
                int seatIndex = (firstGivePos + j - 1) % room.seatList.Count;
                SeatEntity seat = room.seatList[seatIndex];
                GuPaiJiuSeatCtrl currSeatCrtl = GetSeatCtrlBySeatPos(seat.Index);
                for (int i = 0; i < loopCount; i++)
                {
                    if (i < 2)
                    {
                        seatCtrl.DealTran.transform.GetChild(seatCtrl.DealTran.transform.childCount - i - 1).DOMove(currSeatCrtl.HandContainer[2].position, 0.5f);
                    }
                    else
                    {
                        seatCtrl.DealTran.transform.GetChild(seatCtrl.DealTran.transform.childCount - i - 1).DOMove(currSeatCrtl.HandContainer[2].position + new Vector3(39, 0, 0), 0.5f);
                    }

                }
                yield return new WaitForSeconds(0.2f);
                for (int k = 0; k < loopCount; k++)
                {
                    Destroy(seatCtrl.DealTran.transform.GetChild(seatCtrl.DealTran.transform.childCount - k - 1).gameObject);
                    LoadPoker(seat, currSeatCrtl.HandContainer[k], k);
                }

                AudioEffectManager.Instance.Play("gp_fapai", Vector3.zero);
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(0.1f);
            SendNotification(ConstantGuPaiJiu.GuPaiJiuClientEmptyReceive);
        }



        /// <summary>
        /// 发牌的时候加载空牌
        /// </summary>
        /// <param name="index"></param>
        /// <param name="tran"></param>
        private void LoadPoker(SeatEntity seat, Transform tran, int dun)
        {
            GuPaiJiuPrefabManager.Instance.LoadPoker(seat.Index, 0, (GameObject go) =>
            {
                go.transform.SetParent(tran);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = new Vector3(0.45f,0.45f,0.45f);
                go.AddComponent<EventTriggerListener>().onClick += (GameObject go1) =>
                {
                    GuPaiJiuGameCtrl.Instance.GuPaiJiuClisentSendDrawPoker(seat, dun / 2);
                };
            });
        }
        #endregion


        /// <summary>
        /// 组合牌结束
        /// </summary>
        /// <param name="data"></param>
        private void GroupEnd(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("Seat");
            ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
            bool IsPlayer = data.GetValue<bool>("IsPlayer");
            GuPaiJiuSeatCtrl currSeatCrtl = GetSeatCtrlBySeatPos(seat.Index);
            currSeatCrtl.SeatCtrlGroupEnd(seat, roomStatus);
        }

        /// <summary>
        /// 结算的时候加载别人的牌
        /// </summary>
        /// <param name="data"></param>
        private void GroupEndJieSuan(TransferData data)
        {           
            List<SeatEntity> seatList = data.GetValue<List<SeatEntity>>("SeatList");
            SeatEntity currentSeat = data.GetValue<SeatEntity>("PlayerSeat");
            RoomEntity room = data.GetValue<RoomEntity>("Room");
            int currentPos = currentSeat.Pos == 4 ? 1 : currentSeat.Pos + 1;
            StartCoroutine(GroupEndJieSuanPoker(currentPos,room, seatList));
            StartCoroutine(LoadSmallResult(seatList));
        }

        IEnumerator GroupEndJieSuanPoker(int currentPos, RoomEntity room, List<SeatEntity> seatList)
        {
            for (int j = 0; j < 4; ++j)
            {               
                int seatIndex = (currentPos + j - 1) % room.seatList.Count;
                SeatEntity seat = room.seatList[seatIndex];          
                GuPaiJiuSeatCtrl currSeatCrtl = GetSeatCtrlBySeatPos(seat.Index);
                currSeatCrtl.SetCtrlJieSuanPoker(seat,room.roomStatus);
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds(0.5f);
            RoomGuPaiJiuProxy.Instance.SetBetUI(seatList);                
        }




        /// <summary>
        /// 调用播放牌型音乐的方法
        /// </summary>
        /// <param name="data"></param>
        private void PlayMusic(TransferData data)
        {
            RoomEntity room = data.GetValue<RoomEntity>("Room");
            SeatEntity currentSeat = data.GetValue<SeatEntity>("PlayerSeat");
            StartCoroutine(PlayMusicTor(room, currentSeat));
        }
        IEnumerator PlayMusicTor(RoomEntity room, SeatEntity currentSeat)
        {
            List<string> MusiList = new List<string>();
            int currentPos = currentSeat.Pos;
            for (int j = 0; j < room.seatList.Count; ++j)
            {
                if (MusiList.Count != 0) MusiList.Clear();
                int seatIndex = (currentPos + j - 1) % room.seatList.Count;
                SeatEntity seat = room.seatList[seatIndex];
                if (seat.PlayerId == 0) continue;
                GuPaiJiuSeatCtrl currSeatCrtl = GetSeatCtrlBySeatPos(seat.Index);
                MusiList = currSeatCrtl.PlayMusic(seat);
                for (int i = 0; i < MusiList.Count; i++)
                {
                    string MusicName = string.Format("gp_{0}_{1}", MusiList[i], seat.Gender);
                    AudioEffectManager.Instance.Play(MusicName, Vector3.zero);
                    yield return new WaitForSeconds(0.5f);
                }
                yield return new WaitForSeconds(0.3f);
            }
        }

        /// <summary>
        /// 加载小结算
        /// </summary>
        /// <param name="data"></param>
        private void LoadSmallResult(TransferData data)
        {
            List<SeatEntity> seatList = data.GetValue<List<SeatEntity>>("SeatList");
            StartCoroutine(LoadSmallResult(seatList));
        }

        IEnumerator LoadSmallResult(List<SeatEntity> seatList)
        {
#if IS_BAODINGQIPAI
            yield return new WaitForSeconds(13);
#else
             yield return new WaitForSeconds(6);
#endif
            UIViewUtil.Instance.LoadWindowAsync(UIWindowType.GuPaiJiuSmallResult, (GameObject go) =>
            {
                go.GetComponent<UIGuPaiJiuSmallResultView>().SetUI(seatList);
            });
        }
    }  
    
}
