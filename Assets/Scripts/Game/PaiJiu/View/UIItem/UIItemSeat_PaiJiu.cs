//===================================================
//Author      : WZQ
//CreateTime  ：7/4/2017 8:18:51 PM
//Description ：管理UI Seat
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.paigow;
using UnityEngine.UI;
namespace PaiJiu
{
    public class UIItemSeat_PaiJiu : MonoBehaviour
    {

        [SerializeField]
        protected UIItemPlayerInfo_PaiJiu m_PlayerInfo;

        [SerializeField]
        protected GameObject m_Ready;//准备好了
        [SerializeField]
        private Image[] m_isRobBanker;//是否抢庄
        [SerializeField]
        protected Transform m_UIAnimationContainer;

        [SerializeField]
        protected Transform m_UIHintLight;//座位提示灯

        [SerializeField]
        protected int m_nSeatIndex = -1;
        [SerializeField]
        protected Transform m_UIEarningsContainer;//飘分挂载点
        private UIItemEarnings_PaiJiu m_Earnings = null;//飘分预制体
        [SerializeField]
        protected  Image m_HandPokersType;//手牌牌型
#if IS_ZHANGJIAKOU
        [SerializeField]
        protected Image m_DoubleImage;//x2 闲家翻倍
#endif
        [SerializeField]
        private Text m_Pour;//下注分数
        private bool isUIHintLight=false;//提示灯是否开启
        private float HintLightInterval=0.2f;//提示灯闪烁间隔
        private void Awake()
        {
            ModelDispatcher.Instance.AddEventListener(ConstDefine_PaiJiu.ObKey_SeatInfoChanged, OnSeatInfoChanged);
            ModelDispatcher.Instance.AddEventListener(ConstDefine_PaiJiu.ObKey_SetRobBanker, SetRobBanker);//设置是否抢庄显影
            gameObject.SetActive(false);
            m_UIHintLight.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_PaiJiu.ObKey_SeatInfoChanged, OnSeatInfoChanged);
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_PaiJiu.ObKey_SetRobBanker, SetRobBanker);
        }

        private void OnSeatInfoChanged(TransferData data)
        {
            PaiJiu.Seat seat = data.GetValue<PaiJiu.Seat>("Seat");
            ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
            Room CurrentRoom = data.GetValue<Room>("CurrentRoom");//当前房间
            PaiJiu.Seat BankerSeat = data.GetValue<PaiJiu.Seat>("BankerSeat");//庄家座位
            PaiJiu.Seat ChooseBankerSeat = data.GetValue<PaiJiu.Seat>("ChooseBankerSeat");//当前选庄座位

            bool IsPlayer = data.GetValue<bool>("IsPlayer");

            if (m_nSeatIndex != seat.Index) return;

            SetSeatInfo(seat);
            SetOtherShow(seat, CurrentRoom, BankerSeat, ChooseBankerSeat, IsPlayer);
        }


        #region  SetOtherShow  设置座位效果
        private void SetOtherShow(PaiJiu.Seat seat, Room CurrentRoom, PaiJiu.Seat BankerSeat, PaiJiu.Seat ChooseBankerSeat, bool IsPlayer)
        {
            ROOM_STATUS roomStatus = CurrentRoom.roomStatus;
            //牌型
            SetHandPokersType(seat, roomStatus == ROOM_STATUS.SETTLE && seat.PokerList.Count >= 2);
            //准备好了
            m_Ready.gameObject.SetActive(seat.seatStatus == SEAT_STATUS.SEAT_STATUS_READY && roomStatus == ROOM_STATUS.READY);
            //已下注
            m_Pour.transform.parent.gameObject.SetActive(seat.Pour > 0);
            m_Pour.SafeSetText(seat.Pour.ToString());
            //=================================设置提示=================================================================================
            bool onOffUIHintLight = false;
            if (roomStatus == ROOM_STATUS.CHOOSEBANKER && ChooseBankerSeat == seat) onOffUIHintLight = true;
            if (roomStatus == ROOM_STATUS.POUR && (BankerSeat != null) && ((seat.IsBanker && seat.Pour <= 0) || ((seat.Pour <= 0) && (!seat.IsBanker) && BankerSeat.Pour > 0))) onOffUIHintLight = true;

            //提示/*HintLight*/ 在选庄和下注均有提示
           if(gameObject.activeInHierarchy) SetHint(onOffUIHintLight);
#if IS_ZHANGJIAKOU
            //if (seat.PlayerId > 0 && !gameObject.activeSelf)
            //设置是否抢庄标识  只开不关
            if (CurrentRoom.roomModel == ROOM_MODEL.ROOM_MODEL_GRAB) SetRobBanker(seat, roomStatus);
#endif

        }
        #endregion

        #region  SetSeatInfo  设置座位基本信息
        public void SetSeatInfo(PaiJiu.Seat seat)
        {

            bool isShow = gameObject.activeSelf;

                if (seat.PlayerId == 0)
                { 
                    gameObject.SetActive(false);
                }
                else
                { 
                    gameObject.SetActive(true);
                }

            //=================设置基本信息===================================

            //设置基本信息
            if (seat.PlayerId > 0 && !isShow)
            {
                //刚开启该座位时
                m_PlayerInfo.SetAllUI(seat);
            }
            else
            {
                m_PlayerInfo.SetUI(seat);
            }
            

        }
        #endregion

        #region SetRobBanker 设置是否抢庄
        /// <summary>
        /// 设置是否抢庄
        /// </summary>
        /// <param name="isBanker"></param>
        public void SetRobBanker(Seat seat, ROOM_STATUS roomStatus)
        {
            //if (roomStatus != ROOM_STATUS.GRABBANKER) return;
            if (seat.isGrabBanker == 1) m_isRobBanker[0].gameObject.SetActive(true);//抢
            if (seat.isGrabBanker == 2) m_isRobBanker[1].gameObject.SetActive(true);//不抢
            if (roomStatus == ROOM_STATUS.GRABBANKER) m_isRobBanker[0].transform.parent.gameObject.SetActive(true);


            //m_isRobBanker[0].gameObject.SetActive(seat.isGrabBanker == 1);//抢
            //m_isRobBanker[1].gameObject.SetActive(seat.isGrabBanker == 2);//不抢
            //m_isRobBanker[0].transform.parent.gameObject.SetActive(roomStatus == ROOM_STATUS.GRABBANKER);
        }


        public void SetRobBanker(TransferData data)
        {
            if (!gameObject.activeSelf) return;
            //关闭显示
             m_isRobBanker[0].gameObject.SetActive(false);//抢
             m_isRobBanker[1].gameObject.SetActive(false);//不抢
             m_isRobBanker[0].transform.parent.gameObject.SetActive(false);

            //Seat seat = data.GetValue<Seat>("seat");
            //if (seat.Index != m_nSeatIndex) return;

            //if (seat.IsBanker == m_Banker.gameObject.activeSelf) return;

            //Debug.Log("------roomStatus == ROOM_STATUS.GRABBANKER-------" + roomStatus);
            //m_isRobBanker[0].gameObject.SetActive(seat.isGrabBanker == 1);//抢
            //m_isRobBanker[1].gameObject.SetActive(seat.isGrabBanker == 2);//不抢
            //m_isRobBanker[0].transform.parent.gameObject.SetActive(roomStatus == ROOM_STATUS.GRABBANKER);

        }

        #endregion

        #region SetSettle 设置结算效果
        /// <summary>
        /// 设置结算效果
        /// </summary>
        public void SetSettle(Seat seat)
        {
            SetScoresGoUp(seat.Earnings);
            SetHandPokersType(seat, true);
            //SetDoubleImage(seat, true, bankerSeat);
        }
        #endregion

        #region SetScoresGoUp 设置结算飘分效果
        /// <summary>
        /// 飘分
        /// </summary>
        public void SetScoresGoUp(int earnings)
        {
            if (m_Earnings==null)
            {
                //加载飘分
                string strPath = string.Format(ConstDefine_PaiJiu.UIItemsPath, ConstDefine.GAME_NAME, ConstDefine_PaiJiu.UIItemNameEarnings);
                GameObject go = Pool_PaiJiu.Instance.GetObjectFromPool(ConstDefine_PaiJiu.UIItemNameEarnings, strPath);
                

                AppDebug.Log(go == null);
               
                go.SetParent(m_UIEarningsContainer,true);
                m_Earnings = go.GetComponent<UIItemEarnings_PaiJiu>();
                m_Earnings.InitAni();
            }

            m_Earnings.SetUI(earnings);
          
            AppDebug.Log(string.Format("座位{0}飘分", m_nSeatIndex));
        }
        #endregion

        #region SetHandPokersType 设置手牌类型
        /// <summary>
        /// 设置手牌类型
        /// </summary>
        public void SetHandPokersType(Seat seat,bool isShow)
        {
            bool isShowactiveSelf = m_HandPokersType.gameObject.activeSelf;
            if (isShowactiveSelf != isShow)
            {
                m_HandPokersType.gameObject.SetActive(isShow );
#if IS_ZHANGJIAKOU
              if(m_DoubleImage!=null)  m_DoubleImage.gameObject.SetActive(false);
#endif

                if (isShow)
                {
                    int typeData = 0;

                    //m_HandPokersType.gameObject.SetActive(true);
                    if (seat.PokerList.Count >= 2)
                    {
                        if (seat.PokerList[0].type == seat.PokerList[1].type && seat.PokerList[0].size == seat.PokerList[1].size)
                        {
                            typeData = 10;
#if IS_ZHANGJIAKOU
 
                            if (m_DoubleImage != null && !seat.IsBanker && seat.PokerList[0].size >= 4&& seat.Earnings > 0) m_DoubleImage.gameObject.SetActive(true);
#endif

                        }
                        else
                        {
                            for (int i = 0; i < seat.PokerList.Count; i++)
                            {
                                typeData += seat.PokerList[i].size;

                            }

                            typeData = typeData % 10;

                        }
                    }
                    else
                    {
                        return;
                    }


                    string spritePath = string.Format(ConstDefine_PaiJiu.UISpritePath, ConstDefine.GAME_NAME);
                    Sprite sprite = AssetBundleManager.Instance.LoadSprite(spritePath, ("handpokerstype" + typeData));
                    m_HandPokersType.sprite = sprite;
                    //播放声音
                    string pokerTypeAudio = string.Format(ConstDefine_PaiJiu.PokerType_paijiu,typeData);
//#if IS_PAIJIU
//#else
//                    string pokerTypeAudio = seat.Gender + "_" + string.Format(ConstDefine_PaiJiu.PokerType_paijiu, typeData);
//#endif

                    AudioEffectManager.Instance.Play(pokerTypeAudio, Vector3.zero);
                    //NiuNiu.NiuNiuWindWordAni
                    TweenAni_PaiJiu.PokerTypeAni(m_HandPokersType.transform);



                }
            }
            
        }
        #endregion

        //#region SetHandPokersType 设置X2标识显示
        //public void SetDoubleImage(Seat seat, bool isShow,Seat bankerSeat)
        //{


        //}
        //#endregion

        #region SetHint  设置提示效果
        /// <summary>
        /// 设置该选庄 （开启提示 选择是否坐庄）
        /// </summary>
        public void SetHint(bool OnOff)
        {
            if (OnOff == isUIHintLight) return;
            m_UIHintLight.gameObject.SetActive(OnOff);
            isUIHintLight = OnOff;
            if (OnOff)
            {
                StartCoroutine("SetUIHintLight");
            }
            else
            {
                StopCoroutine("SetUIHintLight");

            }
           
        }

        IEnumerator SetUIHintLight()
        {

            while (isUIHintLight)
            {
                m_UIHintLight.Rotate(new Vector3(0, 0, 90));
                yield return new WaitForSeconds(HintLightInterval) ;
            }
       
        }
#endregion
    }
}