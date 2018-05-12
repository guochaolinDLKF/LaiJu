//===================================================
//Author      : WZQ
//CreateTime  ：8/9/2017 7:45:22 PM
//Description ：聚友UIRoot
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using proto.jy;
using System;
namespace JuYou
{
    public class UISceneJuYouView : UISceneViewBase
    {
        #region Variable
        [SerializeField]
        private UIItemSeat_JuYou[] m_Seats;
        [SerializeField]
        private UIItemOperater_JuYou m_Operater;//Operater项

        [SerializeField]
        private Grid3D m_HandConatiner;//手牌挂载点
        [SerializeField]
        private Transform m_DrawContainer;//摸牌： 第三张牌

        [SerializeField]
        private GameObject[] m_Animations;
        [SerializeField]
        private Button m_ButtonMicroPhone;//语音按钮
        [SerializeField]
        private Button m_ButtonShare;//微信分享(邀请好友)
        [SerializeField]
        private Button m_ButtonReady;//准备按钮
        [SerializeField]
        private Button m_ButtonStart;//开始按钮

        [SerializeField]
        private Button m_ButtonChat;//文字聊天
        [SerializeField]
        private Button m_ButtonSetting;//菜单按钮
        [SerializeField]
        private Transform m_EffectContainer;//ui动画挂载点

        [SerializeField]
        private Transform[] m_seatHeadPos;//座位头像点
        //[SerializeField]
        //private Transform m_GoldContainer;//注池挂载点点

        [SerializeField]
        private GoldManager m_goldManager;

        [SerializeField]
        private Text m_TextBaseScore;//房间底注

        //private Tweener m_HandTween; //手动画

        //private UIItemHandPoker m_CurrentPoker;//当前操作的牌

        //[SerializeField]
        //private float PokerTypeAniIntervalTime = 0.5f;//顺序开牌间隔
        //[SerializeField]
        //private float delaySetGoldTime = 1f;//延迟更改金币text显示时间
        //[SerializeField]
        //private float popupUnitAni = 4f;//延迟弹出小结算时间


        //private List<UIItemHandPoker_PaiJiu> m_HandList = new List<UIItemHandPoker_PaiJiu>();//UI手牌列表



        #endregion


        #region MonoBehaviour
        protected override void OnAwake()
        {
            base.OnAwake();

            EventTriggerListener.Get(m_ButtonMicroPhone.gameObject).onDown = OnBtnMouseDown;
            EventTriggerListener.Get(m_ButtonMicroPhone.gameObject).onUp = OnBtnMouseUp;
            for (int i = 0; i < m_Animations.Length; ++i)
            {
                m_Animations[i].gameObject.SetActive(false);
            }


            ModelDispatcher.Instance.AddEventListener(ConstDefine_JuYou.ObKey_SeatInfoChanged, OnSeatInfoChanged);//座位信息变更回调
            ModelDispatcher.Instance.AddEventListener(ConstDefine_JuYou.ObKey_RoomGoldChanged, OnRoomGoldChanged);//房间底注变更

            m_ButtonShare.gameObject.SetActive(RoomJuYouProxy.Instance.CurrentRoom.matchId <= 0);
            m_ButtonReady.gameObject.SetActive(RoomJuYouProxy.Instance.CurrentRoom.matchId <= 0);
            m_ButtonStart.gameObject.SetActive(false);
            if (!SystemProxy.Instance.IsInstallWeChat || !SystemProxy.Instance.IsOpenWXLogin)
            {
                m_ButtonShare.gameObject.SetActive(false);
            }

            //需要调用 Operater_PaiJiu初始化

            

        }

        protected override void BeforeOnDestroy()
        {
            base.BeforeOnDestroy();
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_JuYou.ObKey_SeatInfoChanged, OnSeatInfoChanged);
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_JuYou.ObKey_RoomGoldChanged, OnRoomGoldChanged);//房间底注变更
        }

        #endregion


        #region //语音按钮抬起按下
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
        #endregion


        #region OnSeatInfoChanged 座位信息变更回调  由RoomPaiJiuProxy发送
        /// <summary>
        /// 座位信息变更回调
        /// </summary>
        /// <param name="obj"></param>
        private void OnSeatInfoChanged(TransferData data)
        {
            SeatEntity seat = data.GetValue< SeatEntity>("Seat");//座位
            bool isPlayer = data.GetValue<bool>("IsPlayer");//是否自己
            ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");//房间状态
            RoomEntity currentRoom = data.GetValue<RoomEntity>("CurrentRoom");//当前房间
            SeatEntity BankerSeat = data.GetValue<SeatEntity>("BankerSeat");//庄家座位
            SeatEntity ChooseBankerSeat = data.GetValue<SeatEntity>("ChooseBankerSeat");//当前选庄座位

            if (isPlayer)
            {
                Debug.Log("自己座位准备状态：" + seat.seatStatus.ToString());
                m_ButtonReady.gameObject.SetActive(!seat.isReady && seat.seatStatus == SEAT_STATUS.IDLE );
                m_ButtonShare.gameObject.SetActive(roomStatus == ROOM_STATUS.IDLE);

                //设置开始按钮
                SetStartBtn(currentRoom,seat);
              
                if (!SystemProxy.Instance.IsInstallWeChat)
                {
                    m_ButtonShare.gameObject.SetActive(false);
                }
                //m_CancelAuto.gameObject.SetActive(seat.IsTrustee);


                //=================================设置下注按钮=================================================================================
                //                                                             是庄 自己未下注                                    不是庄 自己未下注 庄家已下注
                // m_Operater.NoticeJetton(roomStatus == ROOM_STATUS.POUR && (BankerSeat != null) && ((seat.IsBanker && seat.Pour <= 0) || ((seat.Pour <= 0) && (!seat.IsBanker) && BankerSeat.Pour > 0)), seat);
                m_Operater.NoticeJetton(roomStatus == ROOM_STATUS.GAME && seat.seatStatus == SEAT_STATUS.POUR,seat,currentRoom.baseScore);
                //RoomJuYouProxy

                //=================================设置选庄=================================================================================
                //选庄按钮
               // m_Operater.ChooseBanker(roomStatus == ROOM_STATUS.CHOOSEBANKER && currentRoom.ChooseBankerSeat != null && currentRoom.ChooseBankerSeat == seat);
               

            }
        }
        #endregion

        protected override void OnBtnClick(GameObject go)
        {
            base.OnBtnClick(go);
            switch (go.name)
            {
                case ConstDefine_JuYou.BtnSetting:
                    UIViewManager.Instance.OpenWindow(UIWindowType.Setting);
                    break;
                case ConstDefine_JuYou.BtnViewChat:
                    UIViewManager.Instance.OpenWindow(UIWindowType.Chat);
                    break;
                case ConstDefine_JuYou.BtnViewShare:
                    SendNotification(ConstDefine_JuYou.BtnViewShare);
                    break;
                case ConstDefine_JuYou.BtnViewReady:
                    SendNotification(ConstDefine_JuYou.BtnViewReady);
                    break;
                case ConstDefine_JuYou.BtnViewStart:
                    SendNotification(ConstDefine_JuYou.BtnViewStart);
                    break;
                    //btnJuYouViewStart
            }
        }


        /// <summary>
        /// 设置开始按钮
        /// </summary>
        /// <param name="currentRoom"></param>
        /// <param name="seat"></param>
        private void SetStartBtn(RoomEntity currentRoom,SeatEntity seat)
        {
            int isReadySum = 0;
            if (seat.IsBanker)
            {
                for (int i = 0; i < currentRoom.SeatList.Count; i++)
                {
                    if (currentRoom.SeatList[i] != null && currentRoom.SeatList[i].PlayerId > 0)
                    {
                        if (currentRoom.SeatList[i].isReady) ++isReadySum;
                    }
                }
            }

            m_ButtonStart.gameObject.SetActive( currentRoom.roomStatus == ROOM_STATUS.IDLE && seat.IsBanker && isReadySum >= 2);


        }


        /// <summary>
        /// 设置房间第注 由初始及动画事件变更
        /// </summary>
        /// <param name="data"></param>
        private void OnRoomGoldChanged(TransferData data)
        {
            int baseScore = data.GetValue<int>("BaseScore");
            m_TextBaseScore.transform.parent.gameObject.SetActive(baseScore > 0);
            m_TextBaseScore.SafeSetText(baseScore.ToString());


        }

       




        #region UI开局
        /// <summary>
        /// UI开局
        /// </summary>
        public void Begin(RoomEntity room)
        {
            m_goldManager.PutBaseScore(room);

        }
        #endregion



        #region 个人结算
        /// <summary>
        /// 个人结算
        /// </summary>
        public void AloneSettle(SeatEntity seat)
        {
            m_goldManager.AloneSettleGoldMove(seat);

            m_Seats[seat.Index].AloneSettle(seat.Earnings);
            //Vector3 startPos = seat.Earnings >= 0 ? m_GoldContainer.position: m_seatHeadPos[seat.Index].position;
            //Vector3 endPos = seat.Earnings >= 0 ?  m_seatHeadPos[seat.Index].position : m_GoldContainer.position ;
            //PlayUIAnimation(ConstDefine_JuYou.UIAniGoldMove_JuYou, startPos,endPos);
        }
       #endregion



        #region  小结算UI管理  处理小结算一系列显示
        /// <summary>
        /// 每次结算
        /// </summary>
        public void EverytimeSettle(RoomEntity room)
        {

            for (int i = 0; i < room.SeatList.Count; i++)
            {
                if (room.SeatList[i].PlayerId > 0)  m_Seats[room.SeatList[i].Index].AloneSettle(room.SeatList[i].Earnings);//飘分
            }

            PlayUIAnimation( room.isBaoGuo? ConstDefine_JuYou.UIAniBaoGuo_JuYou : ConstDefine_JuYou.UIAniZhaGuo_JuYou); //加载爆锅炸锅
            m_goldManager.BaoZhaGuo(room);
            StartCoroutine(SettleInfo());
        }


       private  IEnumerator SettleInfo()
        {
            yield return new WaitForSeconds(3f);
            //加载小结算
            UIDispatcher.Instance.Dispatch(ConstDefine_JuYou.ObKey_OpenViewJuYou, new object[] { UIWindowType.Settle });
        }






        #endregion


        #region 游戏结束
        /// <summary>
        /// 游戏结束
        /// </summary>
        public void GameOver()
        {
            //加载总排行
            UIDispatcher.Instance.Dispatch(ConstDefine_JuYou.ObKey_OpenViewJuYou, new object[] { UIWindowType.Ranking });
            //StartCoroutine(GameOverInfo());
        }

        private IEnumerator GameOverInfo()
        {
            yield return new WaitForSeconds(8f);
            //加载总排行
            UIDispatcher.Instance.Dispatch(ConstDefine_JuYou.ObKey_OpenViewJuYou, new object[] { UIWindowType.Ranking });
        }

        #endregion

        #region PlayUIAnimation 播放UI动画
        /// <summary>
        /// 播放UI动画
        /// </summary>
        /// <param name="type"></param>
        public void PlayUIAnimation(string type)//UIAnimationType
        {
            string path = string.Format("download/{0}/prefab/uiprefab/uianimations/{1}.drb", ConstDefine.GAME_NAME, type.ToLower());
            AssetBundleManager.Instance.LoadOrDownload(path, type.ToString().ToLower(), (GameObject go) =>
            {
                if (go != null)
                {
                    go = Instantiate(go);
                    go.SetParent(m_EffectContainer);
                }
            });
        }
        #endregion


        #region PlayUIAnimation 播放金币移动动画
        /// <summary>
        /// 播放UI动画
        /// </summary>
        /// <param name="type"></param>
        public void PlayUIAnimation(string type,Vector3 startPos,Vector3 endPos,Action OnComplete = null)//UIAnimationType
        {
            //string path = string.Format("download/{0}/prefab/uiprefab/uianimations/{1}.drb", ConstDefine.GAME_NAME, type.ToLower());
            //AssetBundleManager.Instance.LoadOrDownload(path, type.ToString().ToLower(), (GameObject go) =>
            //{
            //    if (go != null)
            //    {
            //        go = Instantiate(go);
            //        go.SetParent(m_EffectContainer);
            //        GoldMoveAni ani = go.GetComponent<GoldMoveAni>();
            //        if (ani != null) ani.PlayAni(startPos, endPos, OnComplete);
            //    }
            //});
        }
        #endregion


    }
}