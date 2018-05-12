//===================================================
//Author      : WZQ
//CreateTime  ：5/9/2017 11:34:26 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;

using niuniu.proto;
/// <summary>
/// 控制交互显影
/// </summary>

namespace NiuNiu
{
    public class UIInteraction_NiuNiu : MonoBehaviour
    {
        private static UIInteraction_NiuNiu instance;

        public static UIInteraction_NiuNiu Instance { get { return instance; } }


        [SerializeField]
        private RectTransform _BetPanel;                           //下注Panel                   //暂时停用

        [SerializeField]
        private RectTransform _SelectScore;                       //选分父物体
        [SerializeField]
        private RectTransform[] _BtnSelectScores;                   //选分按钮

        [SerializeField]
        private RectTransform _BankerBtnParent;                   //庄家开始按钮+让庄按钮 父物体
        [SerializeField]
        private RectTransform _NoBankerBtnParent;                 //非庄家抢庄按钮+不抢按钮 父物体

        [SerializeField]
        private RectTransform _ReadyBtn;                          //准备按钮
        [SerializeField]
        private RectTransform _ViewShareClickBtn;                 //微信邀请
        [SerializeField]
        private RectTransform _Countdown;                         //倒计时

        [SerializeField]
        private RectTransform _OpenPokerBtn;                      //开牌按钮

        [SerializeField]
        private RectTransform _RubPokerBtn;                      //搓牌按钮

        public bool countdownNoOff = false;                       //是否开启倒计时

        [SerializeField]
        private float countdownTimeMax = 15f;                    //设置倒计时最大读秒

        private float countdownTime;                             //计时器剩余时间


        [SerializeField]
        private Image _CountdownImage;                           //倒计时显示 Image

                                       

        [SerializeField]
        private Text _Hint;                                      //提示文字

        private float countdownUIAdjust=0.1f;                    //倒计时UI调整

        [SerializeField]
        private GameObject startBtnMask;                        //开始游戏按钮遮罩

       

       

        void Awake()
        {
            instance = this;
            //ModelDispatcher.Instance.AddEventListener("SetRoomInteraction", SetRoomInteraction);//设置交互显影

            ModelDispatcher.Instance.AddEventListener(ConstDefine_NiuNiu.ObKey_EnableAllowStartBtn, EnableAllowStartBtn);//设置开始按钮遮罩
            ModelDispatcher.Instance.AddEventListener(ConstDefine_NiuNiu.ObKey_SetHint, SetHint);//设置提示条

            ModelDispatcher.Instance.AddEventListener(ConstDefine_NiuNiu.ObKey_SeatInfoChanged, OnSeatInfoChanged);//座位信息变更回调
            ModelDispatcher.Instance.AddEventListener(ConstDefine_NiuNiu.ObKey_OnCountDownUpdate, OnCountDownUpdate);//设置倒计时

        }
     
        void OnDestroy()
        {
            //ModelDispatcher.Instance.RemoveEventListener("SetRoomInteraction", SetRoomInteraction);//设置交互显影

            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_NiuNiu.ObKey_EnableAllowStartBtn, EnableAllowStartBtn);//设置开始按钮遮罩
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_NiuNiu.ObKey_SetHint, SetHint);//设置提示条

            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_NiuNiu.ObKey_SeatInfoChanged, OnSeatInfoChanged);//座位信息变更回调
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_NiuNiu.ObKey_OnCountDownUpdate, OnCountDownUpdate);
        }


        #region OnSeatInfoChanged 座位信息变更回调  由RoomPaiJiuProxy发送
        /// <summary>
        /// 座位信息变更回调
        /// </summary>
        /// <param name="obj"></param>
        private void OnSeatInfoChanged(TransferData data)
        {
            Seat seat = data.GetValue<Seat>("Seat");//座位
            bool isPlayer = data.GetValue<bool>("IsPlayer");//是否自己
            NN_ENUM_ROOM_STATUS roomStatus = data.GetValue<NN_ENUM_ROOM_STATUS>("RoomStatus");//房间状态
            Room currentRoom = data.GetValue<Room>("CurrentRoom");//当前房间
            Seat BankerSeat = data.GetValue<Seat>("BankerSeat");//庄家座位
            Seat ChooseBankerSeat = data.GetValue<Seat>("ChooseBankerSeat");//当前选庄座位
            int playerNumber = data.GetValue<int>("playerNumber");//当前房间玩家人数
            if (isPlayer)
            {

                //m_ButtonReady.gameObject.SetActive(!seat.isReady && seat.seatStatus == SEAT_STATUS.IDLE);
                //m_ButtonShare.gameObject.SetActive(roomStatus == ROOM_STATUS.IDLE);

                ////设置开始按钮
                //SetStartBtn(currentRoom, seat);

                //if (!SystemProxy.Instance.IsInstallWeChat)
                //{
                //    m_ButtonShare.gameObject.SetActive(false);
                //}
                ////m_CancelAuto.gameObject.SetActive(seat.IsTrustee);


                ////=================================设置下注按钮=================================================================================
                ////                                                             是庄 自己未下注                                    不是庄 自己未下注 庄家已下注
                //// m_Operater.NoticeJetton(roomStatus == ROOM_STATUS.POUR && (BankerSeat != null) && ((seat.IsBanker && seat.Pour <= 0) || ((seat.Pour <= 0) && (!seat.IsBanker) && BankerSeat.Pour > 0)), seat);
                //m_Operater.NoticeJetton(roomStatus == ROOM_STATUS.GAME && seat.seatStatus == SEAT_STATUS.POUR, seat, currentRoom.baseScore);
                ////RoomJuYouProxy

                ////=================================设置选庄=================================================================================
                ////选庄按钮
                //// m_Operater.ChooseBanker(roomStatus == ROOM_STATUS.CHOOSEBANKER && currentRoom.ChooseBankerSeat != null && currentRoom.ChooseBankerSeat == seat);


                _SelectScore.gameObject.SetActive(roomStatus == NN_ENUM_ROOM_STATUS.POUR && !seat.IsBanker && seat.Pour == 0 && (currentRoom.superModel == Room.SuperModel.CommonRoom || currentRoom.superModel== Room.SuperModel.PassionRoom && seat.PokerList[0].index > 0)); //下注项


                SwitchOpenPokerBtn(currentRoom);//设置开牌按钮显影
                SwitchHint(roomStatus, seat); //横条提示
                SwitchDecisionBnaker(seat, currentRoom, playerNumber);//确认庄阶段按钮
                                                                     
                //_ViewShareClickBtn.gameObject.SetActive(roomStatus ==  NN_ENUM_ROOM_STATUS.IDLE);//微信邀请 
                     
                //if (!SystemProxy.Instance.IsInstallWeChat)
                //{
                //    _ViewShareClickBtn.gameObject.SetActive(false);
                //}
                SwitchViewShare(roomStatus, seat,currentRoom.currentLoop);
                //准备
                _ReadyBtn.gameObject.SetActive(currentRoom.currentLoop == 0 && currentRoom.roomStatus == NN_ENUM_ROOM_STATUS.IDLE && !seat.IsBanker && !seat.IsReady);  
               
                SwitchEnableAllowStartBtn(currentRoom, playerNumber);//设置开始游戏按钮遮罩

            }
        }
        #endregion


        void Update()
        {
            if (countdownNoOff)
            {
                countdownTime -= Time.deltaTime;

                SetCountdownUI((int)Mathf.Clamp(countdownTime+countdownUIAdjust, 0, countdownTimeMax));
                if (countdownTime <= 0)
                {

                    // 倒数完毕  
                    InitCountdownUI();

                }

            }

        


        }



        //-------------对对外接口-------------------------------------------------------------------


        #region 对外直接设置提示条（sceneView）
        void SetHint(TransferData data)
        {
            bool currActiveSelf = _Hint.rectTransform.parent.gameObject.activeSelf;
            bool onOff = data.GetValue<bool>("OnOff");
            string hintText = data.GetValue<string>("HintText");

            if (currActiveSelf != onOff)
            {
                _Hint.rectTransform.parent.gameObject.SetActive(onOff);
            }

            if (hintText != null && onOff)
            {
                _Hint.text = hintText;
            }

        }

        #endregion





        #region 设置提示条

        /// <summary>
        ///   设置提示
        /// </summary>
        /// <param name="onOff"></param>
        /// <param name="hintIndex"></param>
        void SwitchHint( NN_ENUM_ROOM_STATUS status,NiuNiu.Seat PlayerSeat)
        {
            bool currActiveSelf = _Hint.rectTransform.parent.gameObject.activeSelf;
           
           

            switch (status)
            {
                case NN_ENUM_ROOM_STATUS.IDLE:
     
                  _Hint.rectTransform.parent.gameObject.SetActive(PlayerSeat.IsReady&& !PlayerSeat.IsBanker);
                 _Hint.text = NiuNiu.ConstDefine_NiuNiu.RoomStateHint_Wait;
                    break;
                
                case NN_ENUM_ROOM_STATUS.BEGIN:
                    _Hint.rectTransform.parent.gameObject.SetActive(false);
                    break;
                case NN_ENUM_ROOM_STATUS.RSETTLE:
                    //_Hint.text = NiuNiu.ConstDefine_NiuNiu.RoomStateHint_SweepTable;
                    //_Hint.rectTransform.parent.gameObject.SetActive(true);
                    _Hint.rectTransform.parent.gameObject.SetActive(false);
                    break;
                case NN_ENUM_ROOM_STATUS.DEAL:
                    //_Hint.text = NiuNiu.ConstDefine_NiuNiu.RoomStateHint_Wait;
                     _Hint.rectTransform.parent.gameObject.SetActive(false);
                    break;
                case NN_ENUM_ROOM_STATUS.POUR:
                    _Hint.text = NiuNiu.ConstDefine_NiuNiu.RoomStateHint_Pour;
                    _Hint.rectTransform.parent.gameObject.SetActive(PlayerSeat.IsBanker);
                    break;
                case NN_ENUM_ROOM_STATUS.LOOKPOCKER:
                    _Hint.text = NiuNiu.ConstDefine_NiuNiu.RoomStateHint_LoopPoker;
                    _Hint.rectTransform.parent.gameObject.SetActive(true);
                    break;
                case NN_ENUM_ROOM_STATUS.HOG:
                    //_Hint.text = NiuNiu.ConstDefine_NiuNiu.RoomStateHint_HOG;
                    _Hint.rectTransform.parent.gameObject.SetActive(false);
                    break;

                case NN_ENUM_ROOM_STATUS.GAMEOVER:
                    break;
                default:
                    break;
            }

          

          

        }
        #endregion


        #region 设置开始游戏按钮遮罩
        //设置开始游戏按钮遮罩
        private void   EnableAllowStartBtn(TransferData data)
        {
            bool onOff = data.GetValue<bool>("OnOff");
            Debug.Log("设置开始游戏按钮遮罩" + onOff);
            Debug.Log(startBtnMask);
            if (startBtnMask != null) startBtnMask.gameObject.SetActive(onOff); 
        }

        private void SwitchEnableAllowStartBtn(NiuNiu.Room room, int playerNumber)
        {
            Debug.Log("设置开始游戏按钮遮罩 Loop："+ room.currentLoop);

            if (room.currentLoop != 0 )
            {
 
                if (startBtnMask != null) startBtnMask.gameObject.SetActive(false);
            }
            else
            {
                for (int i = 0; i <room.SeatList.Count; i++)
                {
                    if (room.SeatList[i].PlayerId > 0 && !room.SeatList[i].IsReady)
                    {
                        if (startBtnMask != null) startBtnMask.gameObject.SetActive(true);
                        return;
                    }

                }
                if (startBtnMask != null) startBtnMask.gameObject.SetActive(false);
            }


        }
        #endregion

        //开关庄家开始游戏按钮+让庄按钮
        public void SwitchBankerBtnParentBtn(bool onOff)
        {

            if (_BankerBtnParent.gameObject.activeSelf != onOff)
            {
                _BankerBtnParent.gameObject.SetActive(onOff);

                if (onOff)
                {
                    NiuNiuWindWordAni.PopUp(_BankerBtnParent);
                }
            }


        }
        //开关非庄家抢庄按钮+不抢按钮
        public void SwitchNoBankerBtnParentBtn(bool onOff)
        {
            if (_NoBankerBtnParent.gameObject.activeSelf != onOff)
            {

                _NoBankerBtnParent.gameObject.SetActive(onOff);
                if (onOff)
                {
                    NiuNiuWindWordAni.PopUp(_NoBankerBtnParent);
                }


            }


        }

        #region
        #endregion

        #region 处理开关 换庄系列(换庄 抢庄等)
        /// <summary>
        /// 处理开关 换庄系列(换庄 抢庄等)
        /// </summary>
        /// <param name="seat"></param>
        /// <param name="room"></param>
        public void SwitchDecisionBnaker(NiuNiu.Seat seat, NiuNiu.Room room, int playerNumber )
        {

            NN_ENUM_ROOM_STATUS roomStatus = room.roomStatus;

            bool currActiveSelfBankerBtn = _BankerBtnParent.gameObject.activeSelf;
            bool currActiveSelfNoBankerBtn = _NoBankerBtnParent.gameObject.activeSelf;


            Debug.Log(string.Format("处理开关换庄按钮 房间状态：{0},自己ID：{1},庄：{2}", roomStatus.ToString(), seat.PlayerId, seat.IsBanker));

            if (roomStatus == NN_ENUM_ROOM_STATUS.IDLE || roomStatus == NN_ENUM_ROOM_STATUS.DISSOLVE)
            {
              
                //庄
                if (seat.IsBanker && playerNumber >= 2)
                {                
                    if (currActiveSelfBankerBtn == false)
                    {
                        _BankerBtnParent.gameObject.SetActive(true);
                        NiuNiuWindWordAni.PopUp(_BankerBtnParent);
                    }
                }
                else
                {
                 
                    _BankerBtnParent.gameObject.SetActive(false);
                }
               
                //非庄
                //if (!seat.IsBanker && currActiveSelfBankerBtn == true)
                //{
                    _NoBankerBtnParent.gameObject.SetActive(false);   
                //}

            }
            else if (roomStatus == NN_ENUM_ROOM_STATUS.HOG)
            {
                _BankerBtnParent.gameObject.SetActive(false);
                if (room.roomModel == Room.RoomModel.robBanker)
                {
                    _NoBankerBtnParent.gameObject.SetActive(seat.isAlreadyHOG == 0 && seat.PokerList[0].index >0);

                    if ( seat.isAlreadyHOG == 0 && seat.PokerList[0].index > 0 && currActiveSelfNoBankerBtn == false)  NiuNiuWindWordAni.PopUp(_NoBankerBtnParent);
                  
                }
                else if(room.roomModel == Room.RoomModel.AbdicateBanker)
                {
                    _NoBankerBtnParent.gameObject.SetActive( !seat.IsBanker && seat.isAlreadyHOG == 0 );
                    if (!seat.IsBanker && seat.isAlreadyHOG == 0 && currActiveSelfNoBankerBtn == false) NiuNiuWindWordAni.PopUp(_NoBankerBtnParent);

                }

            }
            else
            {
                if (currActiveSelfBankerBtn)
                {
                    _BankerBtnParent.gameObject.SetActive(false);
                }
                if (currActiveSelfNoBankerBtn)
                {
                    _NoBankerBtnParent.gameObject.SetActive(false);
                }
                return;
            }


        }
        #endregion


        //对外直接设置准备按钮
        public void CloseReadyBtn()
        {
            if (_ReadyBtn.gameObject.activeSelf != false)
            {
                _ReadyBtn.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 显影 准备按钮
        /// </summary>
        /// <param name="room"></param>
        /// <param name="PlayerSeat"></param>
       void SwitchReadyBtn(NiuNiu.Room room, NiuNiu.Seat PlayerSeat)
        {
            if ( room.currentLoop==0 && room.roomStatus == NN_ENUM_ROOM_STATUS.IDLE && !PlayerSeat.IsBanker && !PlayerSeat.IsReady)
            {
                if (_ReadyBtn.gameObject.activeSelf == false)
                {
                    _ReadyBtn.gameObject.SetActive(true);
                }
            }
            else
            {
                if (_ReadyBtn.gameObject.activeSelf ==true)
                {
                    _ReadyBtn.gameObject.SetActive(false);
                }
            }





        }


        //开关微信邀请好友
        void SwitchViewShare(NN_ENUM_ROOM_STATUS roomStatus, NiuNiu.Seat PlayerSeat,int loop)
        {

            if ( !SystemProxy.Instance.IsInstallWeChat ||  ! SystemProxy.Instance.IsOpenWXLogin)
            {
                _ViewShareClickBtn.gameObject.SetActive(false);
                return;
            }

#if IS_CANGZHOU
            if (loop > 0)
            {
                _ViewShareClickBtn.gameObject.SetActive(false);
                return;
            }
#endif

            if (roomStatus == NN_ENUM_ROOM_STATUS.IDLE && PlayerSeat.IsBanker)
            {
                if ( !_ViewShareClickBtn.gameObject.activeSelf)
                {
                    _ViewShareClickBtn.gameObject.SetActive(true);
                    NiuNiuWindWordAni.PopUp(_ViewShareClickBtn);
                }
               
            }
            else
            {
                if (_ViewShareClickBtn.gameObject.activeSelf)
                {
                    _ViewShareClickBtn.gameObject.SetActive(false);
                }
            }

        }



#region 开关 选择下注分数显影
        /// <summary>
        ///  //开关选择下注分数
        /// </summary>
        /// <param name="seat"></param>
        /// <param name="roomStatus"></param>
        public void SwitchSelectScore(NiuNiu.Seat playerSeat , NN_ENUM_ROOM_STATUS roomStatus)
        {
            if (roomStatus!= NN_ENUM_ROOM_STATUS.POUR)
            {
                _SelectScore.gameObject.SetActive(false);
                return;
            }

            if (!playerSeat.IsBanker&& playerSeat.Pour == 0)
            {
                _SelectScore.gameObject.SetActive(true);
            }
            else 
            {
                _SelectScore.gameObject.SetActive(false);
            }

        }
#endregion

#region  设置房间倒计时
        /// <summary>
        ///  //开关房间倒计时
        /// </summary>
        /// <param name="room"></param>
        void OnCountDownUpdate(TransferData data)
        {
            NiuNiu.Room room = data.GetValue<Room>("CurrentRoom");
            if (room.roomStatus== NN_ENUM_ROOM_STATUS .POUR|| room.roomStatus ==NN_ENUM_ROOM_STATUS.LOOKPOCKER  || room.roomStatus == NN_ENUM_ROOM_STATUS.HOG || room.roomStatus == NN_ENUM_ROOM_STATUS.IDLE)
            {
                
                    //计算倒计时剩余时间
                    TalculateRemainingTime(room.serverTime);

                    _Countdown.gameObject.SetActive(true);
                    countdownNoOff = true;

                    SetCountdownUI((int)countdownTime);
            }
            else
            {
                if (_Countdown.gameObject.activeSelf == true)
                {
                    //初始倒计时
                    InitCountdownUI();

                }

            }

        }
#endregion

#region 开关开牌按钮  （现为开自己牌）
        /// <summary>
        /// 开关开牌按钮  （现为开自己牌）
        /// </summary>
        /// <param name="roomStatus"></param>
        public void SwitchOpenPokerBtn(Room room)
        {
            Seat player = null;
            for (int i = 0; i < room.SeatList.Count; ++i)
            {
                if (room.SeatList[i].PlayerId > 0 && room.SeatList[i].Index == 0)
                {
                    player = room.SeatList[i];
                    break;
                }
            }

            //自己牌是否全开
            bool isPokerAllShow = true;
            for (int i = 0; i < player.PokerList.Count; i++)
            {
                if (player.PokerList[i].index != 0 && player.PokerList[i].status != NN_ENUM_POKER_STATUS.POKER_STATUS_UPWARD) isPokerAllShow = false;
            }
            
            if ( !isPokerAllShow && (room.roomStatus == NN_ENUM_ROOM_STATUS.LOOKPOCKER || ( room.superModel ==  Room.SuperModel.PassionRoom&& room.roomStatus == NN_ENUM_ROOM_STATUS.POUR && player != null&& player.Pour > 0) ))
            {
                if (_OpenPokerBtn.gameObject.activeSelf == false)
                {
                    _OpenPokerBtn.gameObject.SetActive(true);
                    NiuNiuWindWordAni.PopUp(_OpenPokerBtn);

                    if (_RubPokerBtn != null && room.superModel== Room.SuperModel.PassionRoom)
                    {
                        _RubPokerBtn.gameObject.SetActive(true);
                        NiuNiuWindWordAni.PopUp(_RubPokerBtn);
                    }
                    return;
                }

            }
            else
            {
                _OpenPokerBtn.gameObject.SetActive(false);
                if (_RubPokerBtn != null) _RubPokerBtn.gameObject.SetActive(false);
            }

        }

#endregion




        //计算倒计时剩余时间
         void TalculateRemainingTime(long time)
        {
            
            if (time > 0)
            {
                long currTime = TimeUtil.GetTimestamp();

                if (countdownTime < 0)
                {
                    countdownTime = countdownTimeMax;
                }

                countdownTime = Mathf.Clamp( (time * 1000 - GlobalInit.Instance.CurrentServerTime) * 0.001f/*(   (time-currTime) + GlobalInit.Instance.TimeDistance * 0.001f)*/ , 0, countdownTimeMax);

                //Debug.Log(string.Format("服务器发送时间:{0},当前时间: {1},时间差:{2},倒计时剩余时间:{3}", time, currTime, GlobalInit.Instance.TimeDistance, countdownTime));
                //Debug.Log(string.Format("服务器发送时间:{0},当前时间: {1},服务器当前时间:{2},倒计时剩余时间:{3}", time, currTime, GlobalInit.Instance.CurrentServerTime, countdownTime));
            }


          



        }


        //---------------------------------------------------------------------------------------------

        //在更具数据显示房间所有信息前 初始房间显示（清理房间）
        public void InitAll(NiuNiu.Room.RoomModel roomModel, Room.SuperModel superModel)
        {
            InitCountdownUI();
            _SelectScore.gameObject.SetActive(false);

            //_StartBtn.GetComponent<Button>().interactable = false;
            _BankerBtnParent.gameObject.SetActive(false);
            _NoBankerBtnParent.gameObject.SetActive(false);
            //_OpenPokerBtn.GetComponent<Button>().interactable = false;
            _OpenPokerBtn.gameObject.SetActive(false);

            _Hint.rectTransform.parent.gameObject.SetActive(false);

            //_GameOverPanel.gameObject.SetActive(false);
            _ViewShareClickBtn.gameObject.SetActive(false);
            InitRoomModel(roomModel);

            _ReadyBtn.gameObject.SetActive(false);

            if (superModel == Room.SuperModel.PassionRoom)
            {
                for (int i = 0; i < _BtnSelectScores.Length; i++)
                {

#if IS_WANGQUE
                    _BtnSelectScores[i].gameObject.SetActive(string.Equals(_BtnSelectScores[i].gameObject.name, "1") || string.Equals(_BtnSelectScores[i].gameObject.name, "2") || string.Equals(_BtnSelectScores[i].gameObject.name, "3"));
#elif IS_CANGZHOU
                  _BtnSelectScores[i].gameObject.SetActive(string.Equals(_BtnSelectScores[i].gameObject.name, "5") || string.Equals(_BtnSelectScores[i].gameObject.name, "8")  || string.Equals(_BtnSelectScores[i].gameObject.name, "10"));
#endif
                }

            }




        }

        void InitRoomModel(NiuNiu.Room.RoomModel roomModel)
        {

            if (roomModel == NiuNiu.Room.RoomModel.AutoBanker || roomModel == NiuNiu.Room.RoomModel.robBanker ||
                roomModel == NiuNiu.Room.RoomModel.EveryTime || roomModel == NiuNiu.Room.RoomModel.WinnerByBanker
                )
            {
                foreach (Transform item in _BankerBtnParent)
                {
                    if(string.Equals(item.name, ConstDefine_NiuNiu.AbdicateBanker_BtnName)) item.gameObject.SetActive(false);
                    //if (item.name==ConstDefine_NiuNiu.AbdicateBanker_BtnName) item.gameObject.SetActive(false);
                }
            }
           


        }


        //初始化倒计时UI
        void InitCountdownUI()
        {
            countdownTime = countdownTimeMax;


            SetCountdownUI((int)countdownTimeMax);


            countdownNoOff = false;
            _Countdown.gameObject.SetActive(false);

        }





        //设置倒计时图片显示
        void SetCountdownUI(int timeSpriet)
        {
           
            if (_CountdownImage != null)
            {
                string tensStrName = "img_daoshu" + timeSpriet;
                string pathK = string.Format("download/{0}/source/uisource/niuniu.drb", ConstDefine.GAME_NAME);
                Sprite currSprite = null;
                currSprite = AssetBundleManager.Instance.LoadSprite(pathK, tensStrName);
                if (currSprite != null)
                {
                    _CountdownImage.sprite = currSprite;
                }

            }


        }

        //==============================更改============================================================





    }
}