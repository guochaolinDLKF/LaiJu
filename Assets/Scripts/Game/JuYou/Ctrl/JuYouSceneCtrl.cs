//===================================================
//Author      : WZQ
//CreateTime  ：8/9/2017 7:33:29 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.jy;
using UnityEngine.EventSystems;
namespace JuYou
{
    //#region
    //#endregion
    public class JuYouSceneCtrl : SceneCtrlBase
    {
        private JuYouSceneCtrl() { }

        private static JuYouSceneCtrl instance;

        public static JuYouSceneCtrl Instance{get{return instance;}}

        #region Variable
        private UISceneJuYouView m_UISceneJuYou3DView;//场景UI
        [SerializeField]
        private SeatCtrl_JuYou[] m_Seats;//座位(3d场景座位控制器)
       
        [SerializeField]
        private Transform m_EffectContainer;//特效挂载点

        public Camera HandPokerCamera;

        private List<MaJiangCtrl_JuYou> wallList = new List<MaJiangCtrl_JuYou>();//剩余牌墙 牌List
        private const float DEAL_ANIMATION_DURATION = 0.4f;//摸牌动画时间
        #endregion



        #region Override MonoBehaviour
        protected override void OnAwake()
        {
            base.OnAwake();
            instance = this;
            for (int i = 0; i < m_Seats.Length; ++i)
            {
                m_Seats[i].Init(RoomJuYouProxy.Instance.CurrentRoom.SeatList.Count, RoomJuYouProxy.Instance.CurrentRoom.roomStatus, RoomJuYouProxy.Instance.PlayerSeat.Pos);
            }

            //销毁加载界面
          if(DelegateDefine.Instance.OnSceneLoadComplete!=null)  DelegateDefine.Instance.OnSceneLoadComplete();


            UIDispatcher.Instance.AddEventListener(ConstDefine_JuYou.ObKey_SendRoomGoldChanged, SendRoomGoldChanged);//房间底注
            UIDispatcher.Instance.AddEventListener(ConstDefine_JuYou.ObKey_SendSeatGoldChanged, SendSeatGoldChanged);//座位gold

        }

        //游戏切入切出
        private void OnApplicationPause(bool isPause)
        {
            if (!isPause)
            {
                if (!NetWorkSocket.Instance.Connected(GameCtrl.Instance.SocketHandle))
                {
                    //if (RoomMaJiangProxy.Instance.CurrentRoom.Status != RoomEntity.RoomStatus.Replay)
                    //{
                    JuYouGameCtrl.Instance.RebuildRoom();
                    //}
                }
            }
        }

       

        protected override void OnStart()
        {
            base.OnStart();
            if (DelegateDefine.Instance.OnSceneLoadComplete != null)
            {
                DelegateDefine.Instance.OnSceneLoadComplete();
            }
            
            GameObject go = UIViewManager.Instance.LoadSceneUIFromAssetBundle(UIViewManager.SceneUIType.JuYou3D,
                () =>
                {

                    UIDispatcher.Instance.Dispatch(ConstDefine_JuYou.ObKey_SetEnterRoomView);

                    if (RoomJuYouProxy.Instance.CurrentRoom.roomStatus == ROOM_STATUS.GAME)
                        m_UISceneJuYou3DView.Begin(RoomJuYouProxy.Instance.CurrentRoom);

                }


                );



            m_UISceneJuYou3DView = go.GetComponent<UISceneJuYouView>();

            //初始 房间底注 座位gold 
            RoomJuYouProxy.Instance.SendRoomGoldChanged();
            for (int i = 0; i < RoomJuYouProxy.Instance.CurrentRoom.SeatList.Count; ++i)
            {
                if (RoomJuYouProxy.Instance.CurrentRoom.SeatList[i].PlayerId > 0)
                    RoomJuYouProxy.Instance.SendSeatGoldChanged(RoomJuYouProxy.Instance.CurrentRoom.SeatList[i]);
            }




            UIItemJuYouRoomInfo.Instance.SetUI(RoomJuYouProxy.Instance.CurrentRoom.roomId, 0);
            UIItemJuYouRoomInfo.Instance.SetRoomConfig(RoomJuYouProxy.Instance.CurrentRoom.Config);
            //摄像机
            //DRB.MahJong.CameraCtrl.Instance.SetPos(RoomPaiJiuProxy.Instance.PlayerSeat.Pos, RoomPaiJiuProxy.Instance.CurrentRoom.SeatList.Count);
            //CompassCtrl.Instance.SetNormal();               //-------------------------------------------<指南针>-----------------------------

            //设置房间UI   由模型层发消息
            RoomJuYouProxy.Instance.SendRoomInfoChangeNotify();

            //设置房间底分 座位gold




         if(RoomJuYouProxy.Instance.CurrentRoom.Unixtime != 0)   RoomJuYouProxy.Instance.SetCountDown(RoomJuYouProxy.Instance.CurrentRoom.Unixtime);


           MahJongManager_JuYou.Instance.Init(() =>
            {
                // 如果当前房间已经开始，重建房间信息
                ReBuildRoom(RoomJuYouProxy.Instance.CurrentRoom);

                //如果当前房间已经开始，重建房间信息
                //ReBuildRoom(RoomPaiJiuProxy.Instance.CurrentRoom);

            }
            );


            //设置BGM
            AudioBackGroundManager.Instance.Play("bgm_juyou");
        }

        protected override void BeforeOnDestroy()
        {
            base.BeforeOnDestroy();

            UIDispatcher.Instance.RemoveEventListener(ConstDefine_JuYou.ObKey_SendRoomGoldChanged, SendRoomGoldChanged);//房间底注
            UIDispatcher.Instance.RemoveEventListener(ConstDefine_JuYou.ObKey_SendSeatGoldChanged, SendSeatGoldChanged);//座位gold

            //MahJongManager_PaiJiu.Instance.Init(null);


            //销毁注册的事件
            //NetDispatcher.Instance.RemoveEventListener(OP_ROOM_FIGHT_PASS.CODE, OnServerReturnPass);//服务器返回过
            //NetDispatcher.Instance.RemoveEventListener(OP_ROOM_FIGHT_WAIT.CODE, OnServerReturnOperateWait);//服务器返回吃碰杠胡等待
        }
        #endregion



        //发送房间底注变更
        private void SendRoomGoldChanged(object[] obj)
        {
            RoomJuYouProxy.Instance.SendRoomGoldChanged();
        }

        //发送座位gold变更
        private void SendSeatGoldChanged(object[] obj)
        {
            try
            {
                SeatEntity seat = (SeatEntity) obj[0];
                RoomJuYouProxy.Instance.SendSeatGoldChanged(seat);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }




        #region Rebuild 重建房间
        /// <summary>
        /// 重建房间
        /// </summary>
        /// <param name="room"></param>
        private void ReBuildRoom(RoomEntity room)
        {

            if (RoomJuYouProxy.Instance.CurrentRoom.roomStatus == ROOM_STATUS.GAME )
            {
                Begin(room, false);
            }


            //====================确认当前游戏状态=====================


            RoomJuYouProxy.Instance.SendRoomInfoChangeNotify();
        }

        #endregion










        #region 开局相关 
        #region Begin 开局发牌等
        /// <summary>
        /// 开局
        /// </summary>
        /// <param name="room"></param>
        /// <param name="isPlayAnimation"></param>
        public void Begin(RoomEntity room, bool isPlayAnimation)
        {
            //CompassCtrl.Instance.SetNormal(); //<--------------指南针控制
            //UIItemTingTip.Instance.Close();
            //m_UISceneMaJiang3DView.Begin();       
            //m_UIScenePaiJiu3DView.               //设置开局的

            //====================初始化墙====================
                InitWall(isPlayAnimation);
              
            //===================摸牌=======================
            if (!isPlayAnimation)
            {
                for (int i = 0; i < room.SeatList.Count; i++)
                {
                    SeatEntity seat = room.SeatList[i];
                    //桌面上的牌
                    for (int j = 0; j < seat.TablePokerList.Count; j++)
                    {
                        MaJiangCtrl_JuYou majiang =  MahJongManager_JuYou.Instance.DrawMaJiang(seat.Index, seat.TablePokerList[j]);

                    //清空摸到手里的已出过的牌
                    if (seat.TablePokerList.Count > 0) m_Seats[seat.Index].OnSeatDiscardPoker();
                    //if (seat.TablePokerList.Count > 0) MahJongManager_JuYou.Instance.ClearDicTable(seat.Index);
                    }

                    List<MaJiangCtrl_JuYou> majiangs = new List<MaJiangCtrl_JuYou>();
                    //手里的牌
                    for (int j = 0; j < seat.PokerList.Count; ++j)
                    {
                        if (seat == RoomJuYouProxy.Instance.PlayerSeat) AppDebug.Log(string.Format("---PlayerSeat---------重连房间座位手牌{0}", seat.PokerList[j].ToChinese()));
                        MaJiangCtrl_JuYou majiang = null;

                        majiang = MahJongManager_JuYou.Instance.DrawMaJiang(seat.Index, seat.PokerList[j]);
                        majiangs.Add(majiang);

                    }
                    m_Seats[seat.Index].DrawPoker(majiangs,true);
                   
                }


            }
            else
            {
                //// 清空手牌
                // for (int i = 0; i < room.SeatList.Count; i++)
                // {
                //     SeatCtrl_JuYou seatCtrl = m_Seats[room.SeatList[i].Index];
                //     if (seatCtrl != null) seatCtrl.OnSeatDiscardPoker();
                // }

                //其他动画
                m_UISceneJuYou3DView.Begin(RoomJuYouProxy.Instance.CurrentRoom);

                //StartCoroutine(BeginAnimation(/*isReplay*/));
            }
        }
        #endregion

        private void InitWall(bool isPlayAnimation)
        {

            //清空牌墙
            //MahJongManager_JuYou.Instance.ClearWall();
            //根据数量加载  加载到挂载点
            wallList = MahJongManager_JuYou.Instance.Rebuild(RoomJuYouProxy.Instance.CurrentRoom.mahJongSum);


            WallCtrl_JuYou.Instance.InitWall(wallList, isPlayAnimation);
 



        }
        #endregion

        #region 发牌
        /// <summary>
        /// 发牌
        /// </summary>
        public void DealPoker( int seatPos,bool isAni)
        {
            SeatEntity seat = RoomJuYouProxy.Instance.GetSeatBySeatId(seatPos);
            if (seat == null) return;

            List<MaJiangCtrl_JuYou> majiangs = new List<MaJiangCtrl_JuYou>();

            List<MaJiangCtrl_JuYou> majiangsHand =  MahJongManager_JuYou.Instance.GetHand(seat.Index);
            //手里的牌
            for (int j = 0; j < seat.PokerList.Count; ++j)
            {
                if (majiangsHand == null || j >= majiangsHand.Count)
                {

                MaJiangCtrl_JuYou majiang = null;

                majiang = MahJongManager_JuYou.Instance.DrawMaJiang(seat.Index, seat.PokerList[j]);
                majiangs.Add(majiang);

                }

            }

            

            if (majiangs.Count > 0) m_Seats[seat.Index].DrawPoker(majiangs, false);

         
        }
        #endregion


        #region
        /// <summary>
        /// 某人下注
        /// </summary>
        public void BroadcastJetton(int seatPos)
        {
            SeatEntity seat = RoomJuYouProxy.Instance.GetSeatBySeatId(seatPos);
            if (seat == null) return;

            //兜 全兜
          AudioEffectManager.Instance.Play(string.Format(seat.Pour == RoomJuYouProxy.Instance.CurrentRoom.baseScore ? ConstDefine_JuYou.AudioQuanDou:ConstDefine_JuYou.AudioDou, seat.Gender), Vector3.zero);
            DealPoker(seat.Pos, true);

        }

        #endregion


        IEnumerator GetAloneSettle()
        {
            yield return new WaitForSeconds(3);
            //发送获取结算信息
            UIDispatcher.Instance.Dispatch(ConstDefine_JuYou.ObKey_AloneSettle);

        }





        #region 翻牌
        /// <summary>
        /// 翻牌
        /// </summary>
        /// <param name="seatIndex"></param>
        /// <param name="pokerIndexs"></param>
        public void SetHandPokerStatus(int pos, List<int> pokerIndexs)
        {
            SeatEntity seat = RoomJuYouProxy.Instance.GetSeatBySeatId(pos);
            if (seat == null) return;
            int seatIndex = seat.Index;
            //SeatCtrl_JuYou
            m_Seats[seatIndex].SetPokerStatus(pokerIndexs);
        }
        #endregion

        #region 玩家个人结算 GIVEUPPOKER
        /// <summary>
        /// 
        /// </summary>
        /// <param name="seatIndex"></param>
        /// <param name="pokerIndexs"></param>
        public void AloneSettle(int pos,bool isGiveup)
        {
            Debug.Log("玩家个人结算1");
            SeatEntity seat = RoomJuYouProxy.Instance.GetSeatBySeatId(pos);
            if (seat == null) return;
            int seatIndex = seat.Index;
            Debug.Log("玩家个人结算2");
            m_Seats[seatIndex].OnSeatDiscardPoker();
            if (isGiveup) AudioEffectManager.Instance.Play(string.Format(ConstDefine_JuYou.AudioBuDou,seat.Gender) , Vector3.zero);//不兜
            if (!isGiveup) m_UISceneJuYou3DView.AloneSettle(seat);
           
        }
        #endregion

        #region 重新洗牌 Shuffle
        /// <summary>
        /// 重新洗牌
        /// </summary>
        public void Shuffle()
        {
            InitWall(true);
        }
        #endregion



        #region
        /// <summary>
        /// 结算
        /// </summary>
        public void Settle()
        {
            //ui处理
            m_UISceneJuYou3DView.EverytimeSettle(RoomJuYouProxy.Instance.CurrentRoom);
            

        }
        #endregion

        #region
        /// <summary>
        /// 游戏结束
        /// </summary>
        public void GameOver()
        {
            m_UISceneJuYou3DView.GameOver();

        }
        #endregion


        //#region DrawPokerAnimation 开局动画
        ///// <summary>
        ///// 开局动画
        ///// </summary>
        ///// <param name="isReplay"></param>
        ///// <returns></returns>
        //private IEnumerator BeginAnimation(bool isReplay)
        //{
        //    yield return new WaitForSeconds(0.7f);
        //    //===================摇骰子=====================
        //    //yield return StartCoroutine(RollDice(RoomMaJiangProxy.Instance.CurrentRoom.FirstDice.diceA, RoomMaJiangProxy.Instance.CurrentRoom.FirstDice.diceB, RoomMaJiangProxy.Instance.CurrentRoom.SecondDice.diceA, RoomMaJiangProxy.Instance.CurrentRoom.SecondDice.diceB));
        //    //===================发牌=====================
        //    //yield return StartCoroutine(PlayDealAnimation());


        //    if (isReplay)
        //    {
        //        for (int i = 0; i < RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count; ++i)
        //        {
        //            MahJongManager.Instance.Sort(MahJongManager.Instance.GetHand(RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i].Pos), RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i].UniversalList);
        //            //GetSeatCtrlBySeatPos(RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i].Pos).HandSort();
        //        }
        //    }
        //}
        //#endregion

        private IEnumerable PlayDealAnimation()
        {
            SeatEntity seat = RoomJuYouProxy.Instance.CurrentRoom.SeatList[0];
            for (int i = 0; i < seat. PokerList.Count; i++)
            {
                MahJongManager_JuYou.Instance.DrawMaJiang(seat.Index, seat.PokerList[i]);
                yield return new WaitForSeconds(DEAL_ANIMATION_DURATION);
            }
             //List<MaJiangCtrl_JuYou>  handList=  MahJongManager_JuYou.Instance.GetHand(seat.Index);


            //int loopCount = RoomPaiJiuProxy.Instance.PlayerSeat.PokerList.Count / countPerTimes;   //===================根据手牌长度设定摸牌循环次数============count/countPerTimes=========

            //int firstGivePos = RoomPaiJiuProxy.Instance.CurrentRoom.currentDice.seatPos;
            ////按顺序发牌 
            //for (int i = 0; i < loopCount; ++i)
            //{
            //    for (int j = 0; j < RoomPaiJiuProxy.Instance.CurrentRoom.SeatList.Count; ++j)
            //    {
            //        int seatPos = (firstGivePos + j - 1) % RoomPaiJiuProxy.Instance.CurrentRoom.SeatList.Count;
            //        PaiJiu.Seat seat = RoomPaiJiuProxy.Instance.CurrentRoom.SeatList[seatPos];


            //        for (int k = 0; k < countPerTimes; ++k)
            //        {

            //            //发牌入手
            //            MaJiangCtrl_PaiJiu majiang = MahJongManager_PaiJiu.Instance.DrawMaJiang(seat.Pos, seat.PokerList[(i * countPerTimes) + k]);

            //            //显示手牌
            //            if (seat == RoomPaiJiuProxy.Instance.PlayerSeat)
            //            {
            //                m_UIScenePaiJiu3DView.DrawPoker(majiang);
            //            }
            //            else
            //            {
            //                GetSeatCtrlBySeatPos(seat.Pos).DrawPoker(majiang);
            //            }

            //        }

            //        yield return new WaitForSeconds(DEAL_ANIMATION_DURATION);
            //    }
            //}


        }

        public void NextGame()
        {
            Debug.Log("原清空桌面NextGame");

            MahJongManager_JuYou.Instance.DespawnAll();

        //刷新场景
        ////清空牌墙
        //MahJongManager_JuYou.Instance.ClearWall();
        ////丢弃手牌
        //for (int i = 0; i < m_Seats.Length; i++)
        //{
        //    m_Seats[i].OnSeatDiscardPoker();
        //}
        ////清空桌面牌
        //MahJongManager_JuYou.Instance.ClearDicTable();

        }


        protected override void OnPlayerClick()
        {
            base.OnPlayerClick();

            //如果点击的是UI 不做处理
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = HandPokerCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit[] hitArr = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("PlayerHand"));
            Debug.Log(hitArr.Length);
            if (hitArr.Length > 0)
            {
               
                MaJiangCtrl_JuYou ctrl = hitArr[0].collider.gameObject.GetComponent<MaJiangCtrl_JuYou>();
                AudioEffectManager.Instance.Play("dianpai", Vector3.zero, false);

                //for (int q = 0; q < RoomMaJiangProxy.Instance.PlayerSeat.HoldPoker.Count; ++q)
                //{
                //    if (MahJongHelper.HasPoker(ctrl.Poker, RoomMaJiangProxy.Instance.PlayerSeat.HoldPoker[q]))
                //    {
                //        List<MaJiangCtrl> hand = MahJongManager.Instance.GetHand(RoomMaJiangProxy.Instance.PlayerSeat.Pos);
                //        for (int i = 0; i < hand.Count; ++i)
                //        {
                //            if (MahJongHelper.ContainPoker(hand[i].Poker, RoomMaJiangProxy.Instance.PlayerSeat.HoldPoker[q]))
                //            {
                //                hand[i].Hold(true, false);
                //            }
                //        }
                //        return;
                //    }
                //}

               
              if(!ctrl.IsBeenPlayed)  JuYouGameCtrl.Instance.OnPokerClick(ctrl);
            }
        }

   

    }
}