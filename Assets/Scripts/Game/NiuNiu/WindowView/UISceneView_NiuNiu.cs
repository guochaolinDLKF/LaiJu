//===================================================
//Author      : WZQ
//CreateTime  ：5/9/2017 12:15:04 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NiuNiu;
using UnityEngine.EventSystems;
using niuniu.proto;
/// <summary>
/// 控制所有玩家信息显示       （调用具体Iewm 的接口显示）
/// </summary>

namespace NiuNiu
{

    public class UISceneView_NiuNiu : UISceneViewBase
    {

        //所有玩家Item
        [SerializeField]
        private PlayerUIItem[] playersItem;
     

        //private int InitShowPokerSum = 3;//初始应该开牌数量

        //当前场景UI   抽象出来 只保存信息
        [SerializeField]
        private UISceneViewBase CurrentUIScene;

        [SerializeField]
        private int cycleBankerAni = 5;

        [SerializeField]
        private float BankerFlickerTime = 2;//庄闪烁时间
        [SerializeField]
        private float BankerFlickerIntervalTime = 0.04f;//庄闪烁间隔时间
        [SerializeField]
        private float PokerTypeAniIntervalTime = 0.5f;//顺序出现牌型间隔
        [SerializeField]
        private float popupUnitAni = 4f;//弹出小结算时间
        [SerializeField]
        private float delayPopupGameOverTime = 6f;//延迟弹出总结算时间
        //[SerializeField]
        //private float autoBankerDelayTime = 0f;//没牛下庄 延迟显示
        [SerializeField]
        private float delaySetGoldTime = 1f;//延迟更改金币显示时间

        [SerializeField]
        private float RobBankerAniSpace = 0.1f;//抢庄闪烁间隔时间

        private static UISceneView_NiuNiu instance;

        public static UISceneView_NiuNiu Instance { get { return instance; } }



        [SerializeField]
        private RectTransform m_EffectContainer;//动画挂载点

        [SerializeField]
        private Button m_ButtonMicroPhone;//录制语音Btn

        private bool isPlayDrawPokerAni = false;//是否在播放发牌动画

        [SerializeField]
        private Image[] m_BG;//背景桌面

        #region 
        #endregion

        #region  麻将中以加载预制体形式展示特效
        //public void PlayUIAnimation(int seatIndex, UIAnimationType type)
        //{
        //    m_Seats[seatIndex].PlayUIAnimation(type);
        //}

        public void PlayUIAnimation(UIAniType type)
        {
            string path = string.Format("download/{0}/prefab/uiprefab/uianimations/{1}.drb", ConstDefine.GAME_NAME, type.ToString().ToLower());
            AssetBundleManager.Instance.LoadOrDownload(path, type.ToString().ToLower(), (GameObject go) =>
            {
                go = Instantiate(go);
                go.SetParent(m_EffectContainer);
            });
        }
        #endregion

        #region  发送语音的按下抬起  
        private void OnBtnMouseDown(PointerEventData eventData)
        {


            if (eventData.selectedObject == m_ButtonMicroPhone.gameObject)
            {
                UIViewManager.Instance.OpenWindow(UIWindowType.Micro);
            }
        }

        private void OnBtnMouseUp(PointerEventData eventData)
        {
            Debug.Log("语音抬起");
            if (eventData.selectedObject == m_ButtonMicroPhone.gameObject)
            {
                if (eventData.pointerCurrentRaycast.gameObject == m_ButtonMicroPhone.gameObject)
                {
                    Debug.Log("鼠标从语音按钮上抬起");
                    SendNotification(ConstDefine_NiuNiu.ObKey_OnBtnMicroUp);
                }
                else
                {
                    Debug.Log("鼠标从语音按钮上移开");
                    SendNotification(ConstDefine_NiuNiu.ObKey_OnBtnMicroCancel);
                }
            }
        }
        #endregion

        protected override void OnBtnClick(GameObject go)
        {
            base.OnBtnClick(go);
            switch (go.name)
            {

                case ConstDefine_NiuNiu.ViewChat_BtnName:
                    //应该由控制器开启窗口
                    //UIViewManager.Instance.OpenWindow(UIWindowType.Chat);
                    ChatCtrl.Instance.OpenView(UIWindowType.Chat);
                    //ChatCtrl.Instance.OpenView(UIWindowType.Chat);

                    break;
                case NiuNiu.ConstDefine_NiuNiu.ViewShare_BtnName:
                    SendNotification(NiuNiu.ConstDefine_NiuNiu.ViewShare_BtnName);//微信邀请
                    break;

            }
        }





        protected override void OnAwake()
        {
            instance = this;            //暂时使用单例  完成更改结构后删除   统一使用事件
            base.OnAwake();

            UIViewManager.Instance.CurrentUIScene = UISceneView_NiuNiu.Instance.CurrentUIScene;

            EventTriggerListener.Get(m_ButtonMicroPhone.gameObject).onDown = OnBtnMouseDown;
            EventTriggerListener.Get(m_ButtonMicroPhone.gameObject).onUp = OnBtnMouseUp;

            ModelDispatcher.Instance.AddEventListener(ConstDefine_NiuNiu.ObKey_SetDeal, SetDeal);//设置发牌

            ModelDispatcher.Instance.AddEventListener(ConstDefine_NiuNiu.ObKey_SetShowPokersUI, SetShowPokersUI);//设置某玩家手牌
            //ModelDispatcher.Instance.AddEventListener("SetPokerPos", SetPokerPos);//判断是否有牛 改变位置
            ModelDispatcher.Instance.AddEventListener(ConstDefine_NiuNiu.ObKey_RoomOpenPokerSettle, RoomOpenPokerSettle);//小结算
            ModelDispatcher.Instance.AddEventListener(ConstDefine_NiuNiu.ObKey_SetNextGameUISceneView, NextGame);//允许开始下一局
            ModelDispatcher.Instance.AddEventListener(ConstDefine_NiuNiu.ObKey_SetGameOverUISceneView, GameOver);//游戏结束
            ModelDispatcher.Instance.AddEventListener(ConstDefine_NiuNiu.ObKey_SetRobBankerAni, SetRobBankerAni);//设置抢庄Ani

            //=====================================================================
            ModelDispatcher.Instance.AddEventListener(ConstDefine_NiuNiu.ObKey_SeatInfoChanged, OnSeatInfoChanged);//座位信息变更回调
            
            //ConstDefine_NiuNiu
            
            //=====================================================================



        }

        protected override void BeforeOnDestroy()
        {
            base.BeforeOnDestroy();

            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_NiuNiu.ObKey_SetDeal, SetDeal);
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_NiuNiu.ObKey_SetShowPokersUI, SetShowPokersUI);
            //ModelDispatcher.Instance.RemoveEventListener("SetPokerPos", SetPokerPos);
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_NiuNiu.ObKey_RoomOpenPokerSettle, RoomOpenPokerSettle);//小结算
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_NiuNiu.ObKey_SetNextGameUISceneView, NextGame);//允许开始下一局
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_NiuNiu.ObKey_SetGameOverUISceneView, GameOver);//游戏结束
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_NiuNiu.ObKey_SetRobBankerAni, SetRobBankerAni);//设置抢庄Ani

            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_NiuNiu.ObKey_SeatInfoChanged, OnSeatInfoChanged);//座位信息变更回调
        }






        protected override void OnStart()
        {
           

            //InitPlayersInfoUI();

            base.OnStart();

            ////设置房间初始玩家信息   应该是全部信息
            //SetEnterRoomUI(GameStateManager.Instance.oneselfReferenceSeatList);



        }

        //加载UIRoot后 展示房间全部信息
        public void InitEnterRoomUI(NiuNiu.Room currentRoom, NiuNiu.Seat playerSeat, int playerNumber)
        {

            Debug.Log("房间初始化--置空");

            for (int i = 0; i < playersItem.Length; i++)
            {
                playersItem[i].InfoItem();
            }

            Debug.Log(" //----------------------设置房间初始玩家信息-------------------------------");
            for (int i = 0; i < currentRoom.SeatList.Count; i++)
            {
                Debug.Log(currentRoom.SeatList[i].Pos);
            }


            Debug.Log(currentRoom.roomId);

            //跟换背景桌面

            Debug.Log(" //-------------------" + currentRoom.superModel);
            m_BG[0].gameObject.SetActive(currentRoom.superModel == Room.SuperModel.CommonRoom);
            m_BG[1].gameObject.SetActive(currentRoom.superModel == Room.SuperModel.PassionRoom);

            for (int i = 0; i < playersItem.Length; i++)
            {
                playersItem[i].superModel = currentRoom.superModel;
            }


            //设置房间初始玩家信息   应该是全部信息
            SetEnterRoomUI(currentRoom.SeatList,currentRoom.roomStatus);

        

            //初始化交互显影
            UIInteraction_NiuNiu.Instance.InitAll(currentRoom.roomModel,currentRoom.superModel); //(currentRoom, playerSeat, playerNumber);


            //初始化房间固有信息
            UIItemRoomInfo_NiuNiu.Instance.SetUI(RoomNiuNiuProxy.Instance.CurrentRoom.roomId, 0);
            UIItemRoomInfo_NiuNiu.Instance.SetRoomConfig(RoomNiuNiuProxy.Instance.CurrentRoom.Config);
            

            //根据此时状态 弹出对应窗口
            SetEnterRoomView(currentRoom, playerSeat);

           
            //JuYou.JuYouSceneCtrl

        }

     

        //=========================================================================================================
        //JuYou.UISceneJuYouView
        #region OnSeatInfoChanged 座位信息变更回调  由RoomPaiJiuProxy发送
        /// <summary>
        /// 座位信息变更回调
        /// </summary>
        /// <param name="obj"></param>
        private void OnSeatInfoChanged(TransferData data)
        {
            //Seat seat = data.GetValue<Seat>("Seat");//座位
            //bool isPlayer = data.GetValue<bool>("IsPlayer");//是否自己
            //NN_ENUM_ROOM_STATUS roomStatus = data.GetValue<NN_ENUM_ROOM_STATUS>("RoomStatus");//房间状态
            //Room currentRoom = data.GetValue<Room>("CurrentRoom");//当前房间
            //Seat BankerSeat = data.GetValue<Seat>("BankerSeat");//庄家座位
            //Seat ChooseBankerSeat = data.GetValue<Seat>("ChooseBankerSeat");//当前选庄座位

            //if (isPlayer)
            //{
                
            //    m_ButtonReady.gameObject.SetActive(!seat.isReady && seat.seatStatus == SEAT_STATUS.IDLE);
            //    m_ButtonShare.gameObject.SetActive(roomStatus == ROOM_STATUS.IDLE);

            //    //设置开始按钮
            //    SetStartBtn(currentRoom, seat);

            //    if (!SystemProxy.Instance.IsInstallWeChat)
            //    {
            //        m_ButtonShare.gameObject.SetActive(false);
            //    }
            //    //m_CancelAuto.gameObject.SetActive(seat.IsTrustee);


            //    //=================================设置下注按钮=================================================================================
            //    //                                                             是庄 自己未下注                                    不是庄 自己未下注 庄家已下注
            //    // m_Operater.NoticeJetton(roomStatus == ROOM_STATUS.POUR && (BankerSeat != null) && ((seat.IsBanker && seat.Pour <= 0) || ((seat.Pour <= 0) && (!seat.IsBanker) && BankerSeat.Pour > 0)), seat);
            //    m_Operater.NoticeJetton(roomStatus == ROOM_STATUS.GAME && seat.seatStatus == SEAT_STATUS.POUR, seat, currentRoom.baseScore);
            //    //RoomJuYouProxy

            //    //=================================设置选庄=================================================================================
            //    //选庄按钮
            //    // m_Operater.ChooseBanker(roomStatus == ROOM_STATUS.CHOOSEBANKER && currentRoom.ChooseBankerSeat != null && currentRoom.ChooseBankerSeat == seat);


            //}
        }
        #endregion
        //=====================================================================================================





        //---------------------------------

        #region
        #endregion



        #region
       /// <summary>
       /// 抢庄动画
       /// </summary>
       /// <param name="data"></param>
        public void SetRobBankerAni(TransferData data)
        {
          

            NiuNiu.Room CurrentRoom = data.GetValue<NiuNiu.Room>("CurrentRoom");
            //关闭
            for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
            {
                if (CurrentRoom.SeatList[i].PlayerId > 0)
                {

                    playersItem[CurrentRoom.SeatList[i].Index].SetChooseBankerHint(false);
                }

            }


            bool isOnOff = data.GetValue<bool>("isOnOff");
            if (isOnOff)
            {
                StartCoroutine("RobBankerAni", CurrentRoom);
            }
            else
            {
                StopCoroutine("RobBankerAni");
            }

        }

        IEnumerator RobBankerAni(Room CurrentRoom)
        {
           

            bool isAllNoRob = true;
            int HOGSum = 0;
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                if (CurrentRoom.SeatList[i].PlayerId > 0 && CurrentRoom.SeatList[i].isAlreadyHOG == 1)
                {
                    isAllNoRob = false;
                    HOGSum++;
                }
            }
#if !IS_WANGQUE
            if (HOGSum == 1)
            {
                for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
                {
                    if (CurrentRoom.SeatList[i].PlayerId > 0 && CurrentRoom.SeatList[i].isAlreadyHOG == 1)
                    {
                        playersItem[CurrentRoom.SeatList[i].Index].SetChooseBankerHint(true);
                        yield break;
                    }
                }
            }
#endif
            float timing = Time.time;

            while (true)
            {

                for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
                {
                    if (CurrentRoom.SeatList[i].PlayerId > 0)
                    {
                        if (CurrentRoom.SeatList[i].isAlreadyHOG == 1 || isAllNoRob)
                        {

                            for (int j = 0; j < CurrentRoom.SeatList.Count; ++j)
                            {
                                if (CurrentRoom.SeatList[j].PlayerId > 0)
                                {
                                    playersItem[CurrentRoom.SeatList[j].Index].SetChooseBankerHint(false);
                                }

                            }

                            
                            AudioEffectManager.Instance.Play(ConstDefine_NiuNiu.AuidoRobBankerDi_niuniu, Vector3.zero);//播放音效
                            playersItem[CurrentRoom.SeatList[i].Index].SetChooseBankerHint(true);

                            yield return new WaitForSeconds(RobBankerAniSpace);
                        }

                       
                    }

                }
                if (Time.time - timing > 1f)  yield break;

                yield return new WaitForSeconds(RobBankerAniSpace);
            }

        }




#endregion

#region 发牌  处理发牌显示
        /// <summary>
        /// 发牌  处理发牌显示
        /// </summary>
        /// <param name="data"></param>
        public void SetDeal(TransferData data)
        {
            isPlayDrawPokerAni = true;
            InitAllPokerPos();//还原所有手牌位置
            NiuNiu.Room CurrentRoom = data.GetValue<NiuNiu.Room>("Room");

            StartCoroutine("DealAni", CurrentRoom);
      
        }
#endregion


        IEnumerator DealAni(NiuNiu.Room CurrentRoom)
        {

            //-------牛头动画----
            const string prefabName = "TaurenAni";
            string prefabPath = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, prefabName);
            GameObject go = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(prefabPath, prefabName);
            go = UnityEngine.Object.Instantiate(go);
            go.SetParent(m_EffectContainer.transform);

            yield return new WaitForSeconds(1.6f);
            
          /*if(CurrentRoom.superModel == Room.SuperModel.CommonRoom) */ UIInteraction_NiuNiu.Instance.SwitchOpenPokerBtn(CurrentRoom);//开牌按钮
            for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
            {
                if (CurrentRoom.SeatList[i] != null && CurrentRoom.SeatList[i].PlayerId > 0)
                {

                    if (CurrentRoom.SeatList[i].PokerList[0].index != 0) SetDealAni(CurrentRoom.SeatList[i]);//设置发牌动画 

                }
            }


        }





#region  小结算管理  处理小结算一系列显示
        /// <summary>
        ///  小结算管理 处理小结算一系列显示
        /// </summary>
        /// <param name="seatList"></param>
        public void RoomOpenPokerSettle(TransferData data)
        {
            NiuNiu.Room CurrentRoom = data.GetValue<NiuNiu.Room>("CurrentRoom");
            UIInteraction_NiuNiu.Instance.SwitchOpenPokerBtn(CurrentRoom);//开牌按钮

            StartCoroutine("RoomOpenPokerSettleAni", CurrentRoom);
        }


        //公开牌型动画 
        IEnumerator RoomOpenPokerSettleAni(NiuNiu.Room room)//NN_ENUM_ROOM_STATUS roomStatus
        {
            List<NiuNiu.Seat> seatList = room.SeatList;
            NN_ENUM_ROOM_STATUS roomStatus = room.roomStatus;

            NiuNiu.Seat playerSeat = null;
            for (int i = 0; i < seatList.Count; i++)
            {
                if (seatList[i].Index == 0)
                {
                    playerSeat = seatList[i];
                    break;
                }
            }
             //yield return 0;

            for (int i = 0; i < seatList.Count; i++)
            {
                if (seatList[i] != null && seatList[i].PlayerId > 0 && seatList[i].PockeType != 0 && !seatList[i].IsBanker)
                {


                    RoomOpenPokerSettleAniPerform(seatList[i], roomStatus);
                }
                else
                {
                    continue;
                }

                yield return new WaitForSeconds(PokerTypeAniIntervalTime);

            }

           
            //显示庄的各种动画
            for (int i = 0; i < seatList.Count; i++)
            {
                if (seatList[i].IsBanker)
                {
                    //
                    RoomOpenPokerSettleAniPerform(seatList[i], roomStatus);
                    yield return new WaitForSeconds(PokerTypeAniIntervalTime);
                    break;
                }
            }

#if IS_WANGQUE
            //显示通杀 通赔
            PlayTongCompensateKill(seatList);
#endif

            //显示金币移动                        
            GoldFlowCtrl_NiuNiu.Instance.GoldFlowAni(seatList);


#if IS_CANGZHOU
            if (room.superModel == Room.SuperModel.CommonRoom)
            {
                //每局小结算
                yield return new WaitForSeconds(popupUnitAni);

                NiuNiuGameCtrl.Instance.OpenView(UIWindowType.UnitSettlement_NiuNiu);//管理器调用
            }
#endif

            //#if IS_WANGQUE
            //            if (room.superModel == Room.SuperModel.PassionRoom)
            //            {
            //                const string prefabName = "UIItemSeniorSettle_NiuNiu";
            //                string prefabPath = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, prefabName);
            //                GameObject go = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(prefabPath, prefabName);
            //                go = UnityEngine.Object.Instantiate(go);
            //                go.SetParent(m_EffectContainer.transform);
            //                 go.GetComponent<UIItemSeniorSettle_NiuNiu>().SetUI(playerSeat !=null ? playerSeat.Earnings:0);

            //            }
            //#endif
        }

        //具体设置
        void RoomOpenPokerSettleAniPerform(NiuNiu.Seat seat, NN_ENUM_ROOM_STATUS roomStatus)
        {

            UISceneView_NiuNiu.Instance.SetShowTypeUI(seat, seat.Index,  roomStatus);//显示牌型信息
            UISceneView_NiuNiu.Instance.PlayEarningsAni(seat, seat.Index);//动画显示本局收益信息  SetGold 
            Debug.Log("位置" + seat.Pos + "服务器传递收益为：" + seat.Earnings);

            StartCoroutine(delaySetGold(seat));//更新玩家积分 SetGold

            playersItem[seat.Index].SetAllPokerUI(seat.PokerList);//刷新牌

            if (!isPlayDrawPokerAni) playersItem[seat.Index].SetPokerPos(seat.PokerList);//计算是否移动牌位置

        }

        IEnumerator delaySetGold(NiuNiu.Seat seat)
        {
            yield return new WaitForSeconds(delaySetGoldTime);
            SetGold(seat, seat.Index);  
        }

        //通杀 通赔
        private void PlayTongCompensateKill(List<Seat> seatList)
        {
            int playerSum = 0;

            bool tongchi = true;
            bool tongpei = true;
            for (int i = 0; i < seatList.Count; i++)
            {
                if (seatList[i].PlayerId > 0)  ++playerSum;
                if (seatList[i].PlayerId > 0 && !seatList[i].IsBanker && seatList[i].Pour > 0 && seatList[i].Winner)
                    tongchi = false;
                if (seatList[i].PlayerId > 0 && !seatList[i].IsBanker && seatList[i].Pour > 0 && !seatList[i].Winner)
                    tongpei = false;
            }

            if (playerSum <= 2) return;

            if (tongchi || tongpei)
            {
                PlayUIAnimation(tongchi ? UIAniType.UIAnimation_TongSha: UIAniType.UIAnimation_TongPei );
                AudioEffectManager.Instance.Play(tongchi ? "tongchi" : "tongpei", Vector3.zero);//播放通杀声音
            }
           
        }

#endregion

#region
#endregion
#region 处理允许开始下一局  
        /// <summary>
        ///  处理允许开始下一局
        /// </summary>
        /// <param name="data"></param>
        public void NextGame(TransferData data)
        {
            NiuNiu.Room room = data.GetValue<NiuNiu.Room>("CurrentRoom");
            InitAllPokerPos();//还原所有手牌位置
            SetEnterRoomUI(room.SeatList,room.roomStatus);//刷新房间显示

        }
#endregion

#region 游戏结束  处理游戏结束一系列
        /// <summary>
        /// 游戏结束  处理游戏结束一系列
        /// </summary>
        public void GameOver(TransferData data)
        {
            NiuNiu.Room room = data.GetValue<NiuNiu.Room>("CurrentRoom");

            StartCoroutine("DelayedLoadRankList", room);


        }

        IEnumerator DelayedLoadRankList(NiuNiu.Room room)
        {
            yield return new WaitForSeconds(delayPopupGameOverTime);

            //调用
            //NetDispatcher.Instance.Dispatch(NN_ROOM_DISMISS_SUCCEED.CODE);//准备退出房间

            NiuNiuGameCtrl.Instance.OpenView(UIWindowType.RankList_NiuNiu);//准备退出房间


        }
#endregion




        //  初始化全部玩家 5张Poker位置
        public void InitAllPokerPos()
        {
          
            for (int i = 0; i < playersItem.Length; i++)
            {

                playersItem[i].InitPokerPos();
            }

        }

        /// <summary>
        /// 设置单个 玩家手牌UI (参数对应座位手牌信息  座位索引)
        /// </summary>
        /// <param name="seat"></param>
        /// <param name="pos"></param>
        public void SetShowPokersUI(TransferData data)//NiuNiu.Seat seat, int pos
        {
            if (isPlayDrawPokerAni) return;
            NiuNiu.Seat seat = data.GetValue<NiuNiu.Seat>("Seat");

            playersItem[seat.Index].SetAllPokerUI(seat.PokerList);//翻牌
            playersItem[seat.Index].SetPokerPos(seat.PokerList);//计算位置
        }

        //------------------------------------------------------------------------

        //设置发牌动画
        void SetDealAni(NiuNiu.Seat seat)
        {
            playersItem[seat.Index].PlayeDealAni(seat ,()=>{ isPlayDrawPokerAni = false; });


        }



        /// <summary>
        ///  显示牌型
        /// </summary>
        /// <param name="seat"></param>
        /// <param name="pos"></param>
        public void SetShowTypeUI(NiuNiu.Seat seat, int pos, NN_ENUM_ROOM_STATUS roomStatus)
        {

            playersItem[pos].SetPockeTypeUI(seat.PockeType, roomStatus);

        }

        /// <summary>
        ///  显示本局收益 动画
        /// </summary>
        /// <param name="seat"></param>
        /// <param name="pos"></param>
        public void PlayEarningsAni(NiuNiu.Seat seat, int pos)
        {

            playersItem[pos].PlayEarningsAni(seat.Earnings);

        }

        //设置玩家已有积分
        public void SetGold(NiuNiu.Seat seat, int pos)
        {
            playersItem[pos].SetGold(seat.Gold);

        }

      
        //更改对应玩家信息显示  
        //参数座位类     位置索引(转化后的位置)
        void SetItemInfoUI(NiuNiu.Seat seat, int pos, NN_ENUM_ROOM_STATUS roomStatus)
        {
            //打开这一Item
            ShowPlayerItem(pos);

            //显示该位置完整信息
            playersItem[pos].ShowWholeItem(seat, roomStatus);
            //Debug.Log("位置" + pos + "的牌型是" + seat.PockeType);

        }

        //开启对应玩家Item显示
        //参数 玩家类  位置索引
        void ShowPlayerItem(int pos)
        {
            playersItem[pos].OpenOrClosed(true);
        }


#region
#endregion



#region 刷新房间全部信息
        /// <summary>
        /// 进入房间 
        /// </summary>
        /// <param name="seatList"></param>
        public void SetEnterRoomUI(List<NiuNiu.Seat> seatList, NN_ENUM_ROOM_STATUS roomStatus)
        {
           
            for (int i = 0; i < seatList.Count; i++)
            {

                if (seatList[i] != null && (seatList[i].PlayerId > 0))
                {

                    SetItemInfoUI(seatList[i], seatList[i].Index,  roomStatus); //应改为更具信息 显示全部              
                    Debug.Log(string.Format("刷新房间全部信息:玩家位置{0}，玩家座位转化后索引{1}", seatList[i].Pos, seatList[i].Index));
                }

            }

        }
#endregion



        //------------------------------------------------------------------------

        //刷新窗口 显示房间状态对应窗口

        public void SetEnterRoomView(NiuNiu.Room currRoom, Seat seat)
        {

            //发送消息NiuNiuGameCtrl
            UIDispatcher.Instance.Dispatch("SetEnterRoomView_NiuNiu");




        }











    }

}



