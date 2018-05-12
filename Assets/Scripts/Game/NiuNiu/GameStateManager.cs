//===================================================
//Author      : WAQ
//CreateTime  ：5/9/2017 2:35:03 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using niuniu.proto;
using NiuNiu;
using System;
/// <summary>
///  进入房间后 游戏管理 
/// </summary>


namespace NiuNiu
{
    public class GameStateManager : Singleton<GameStateManager>
    {



        public const int ROOM_SEAT_COUNT = 6;                        //房间座位数量
        public const int ROOM_POKERLIST_COUNT = 5;


        public GameStateManager()
        {




            //向服务器发送消息-------------------------------------------------------------------------------
            NiuNiuEventDispatcher.Instance.AddEventListener("applyReadyNiuNiu", Ready);                                           //申请准备
            NiuNiuEventDispatcher.Instance.AddEventListener("applyForStartGameNiuNiu", applyForStartGame);                 //申请开始
            NiuNiuEventDispatcher.Instance.AddEventListener("AbdicateBankerNiuNiu", AbdicateBanker);                       //让庄
            NiuNiuEventDispatcher.Instance.AddEventListener("RobBankerNiuNiu", RobBankerNiuNiu);                           //抢庄



            NiuNiuEventDispatcher.Instance.AddEventListener("RoomOpenPokersNiuNiu", RoomOpenPokers);                       //开牌对比
            NiuNiuEventDispatcher.Instance.AddEventListener("NeedOpenPokerNiuNiu", NeedOpenPoker);                         //开单张牌
            NiuNiuEventDispatcher.Instance.AddEventListener("SendBetScoreNiuNiu", SendBetScore);                           //下注选分
            NiuNiuEventDispatcher.Instance.AddEventListener("SendLeaveRoomNiuNiu", SendLeaveRoom);                         //游戏结束离开房间

            NiuNiuEventDispatcher.Instance.AddEventListener("SendRubPokerNiuNiu", SendRubPoker);                         //搓牌
            
            //在此注册游戏内所有监听-------------------------------------------------------------------------------


            NetDispatcher.Instance.AddEventListener(NN_SEAT_READY.CODE, RadioReady);                   //服务器广播某人准备
            NetDispatcher.Instance.AddEventListener(NN_ROOM_HOG.CODE, StartRobBanker);                   //开始抢庄RobBankerWin 
            NetDispatcher.Instance.AddEventListener(NN_ROOM_HOG_SUCCEED.CODE, RobBankerWin);              //服务器广播某玩家抢庄成功
            NetDispatcher.Instance.AddEventListener(NN_ROOM_NO_NIUNIU.CODE, AutoBanker);                  //服务器广播没牛下庄
         
            NetDispatcher.Instance.AddEventListener(NN_ROOM_JETTON.CODE, StartPour);                      //开始本局下注
            NetDispatcher.Instance.AddEventListener(NN_ROOM_SB_JETTON.CODE, OneBetScore);                 //服务器广播某玩家下注某分
            NetDispatcher.Instance.AddEventListener(NN_ROOM_BEGIN.CODE, Deal);                            //发牌
            NetDispatcher.Instance.AddEventListener(NN_ROOM_DRAW.CODE, OpenAPoker);                       //服务器广播某玩家开某些牌
            NetDispatcher.Instance.AddEventListener(NN_ROOM_CHECK.CODE, RoomOpenPokerSettle);             //开牌结算
            NetDispatcher.Instance.AddEventListener(NN_ROOM_NEXT_GAME.CODE, NextGame);                    //开始下一局
            NetDispatcher.Instance.AddEventListener(NN_ROOM_GAME_OVER.CODE, NiuNiuGameOver);              //房间牌局结束

            NetDispatcher.Instance.AddEventListener(NN_ROOM_ROB_POKER.CODE, RubPoker);              //搓牌
            


        }

        void OnDisable()
        {
            NiuNiuEventDispatcher.Instance.RemoveEventListener("applyReadyNiuNiu", Ready);
            NiuNiuEventDispatcher.Instance.RemoveEventListener("applyForStartGameNiuNiu", applyForStartGame);
            NiuNiuEventDispatcher.Instance.RemoveEventListener("AbdicateBankerNiuNiu", AbdicateBanker);
            NiuNiuEventDispatcher.Instance.RemoveEventListener("RobBankerNiuNiu", RobBankerNiuNiu);
            NiuNiuEventDispatcher.Instance.RemoveEventListener("RoomOpenPokersNiuNiu", RoomOpenPokers);
            NiuNiuEventDispatcher.Instance.RemoveEventListener("NeedOpenPokerNiuNiu", NeedOpenPoker);
            NiuNiuEventDispatcher.Instance.RemoveEventListener("SendBetScoreNiuNiu", SendBetScore);
            NiuNiuEventDispatcher.Instance.RemoveEventListener("SendLeaveRoomNiuNiu", SendLeaveRoom);
            NiuNiuEventDispatcher.Instance.RemoveEventListener("SendRubPokerNiuNiu", SendRubPoker);

            NetDispatcher.Instance.RemoveEventListener(NN_SEAT_READY.CODE, RadioReady);
            NetDispatcher.Instance.RemoveEventListener(NN_ROOM_HOG.CODE, StartRobBanker);
            NetDispatcher.Instance.RemoveEventListener(NN_ROOM_HOG_SUCCEED.CODE, RobBankerWin);
            NetDispatcher.Instance.RemoveEventListener(NN_ROOM_NO_NIUNIU.CODE, AutoBanker);
            NetDispatcher.Instance.RemoveEventListener(NN_ROOM_JETTON.CODE, StartPour);
            NetDispatcher.Instance.RemoveEventListener(NN_ROOM_SB_JETTON.CODE, OneBetScore);
            NetDispatcher.Instance.RemoveEventListener(NN_ROOM_BEGIN.CODE, Deal);
            NetDispatcher.Instance.RemoveEventListener(NN_ROOM_DRAW.CODE, OpenAPoker);
            NetDispatcher.Instance.RemoveEventListener(NN_ROOM_CHECK.CODE, RoomOpenPokerSettle);
            NetDispatcher.Instance.RemoveEventListener(NN_ROOM_NEXT_GAME.CODE, NextGame);
            NetDispatcher.Instance.RemoveEventListener(NN_ROOM_GAME_OVER.CODE, NiuNiuGameOver);

            NetDispatcher.Instance.RemoveEventListener(NN_ROOM_ROB_POKER.CODE, RubPoker);              //搓牌

            Debug.Log("注销GameStateManager中的事件");

        }
        /// <summary>
        /// 初始化房间信息  构建房间
        /// </summary>
        /// <param name="pbRoom"></param>
        public void InitCurrentRoom(NN_ROOM_CREATE proto)
        {
            RoomNiuNiuProxy.Instance.InitCurrentRoom(proto);
           
        }

        #region StartRobBanker 服务器广播某人准备
        /// <summary>
        /// 服务器广播某人准备
        /// </summary>
        /// <param name="obj"></param>
        public void RadioReady(byte[] obj)
        {

            NN_SEAT_READY proto = NN_SEAT_READY.decode(obj);
            RoomNiuNiuProxy.Instance.Ready(proto);


        }
        #endregion




        #region StartRobBanker 服务器广播开始抢庄
        /// <summary>
        /// 服务器广播发牌
        /// </summary>
        /// <param name="obj"></param>
        public void StartRobBanker(byte[] obj)
        {
            NN_ROOM_HOG proto = NN_ROOM_HOG.decode(obj);
            RoomNiuNiuProxy.Instance.StartRobBanker(proto);


        }
        #endregion

        #region StartRobBanker 服务器广播抢庄结果

        public void RobBankerWin(byte[] obj)
        {


            //RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus = NN_ENUM_ROOM_STATUS.IDLE;
            NN_ROOM_HOG_SUCCEED proto = NN_ROOM_HOG_SUCCEED.decode(obj);
            RoomNiuNiuProxy.Instance.RobBankerWin(proto);

            Debug.Log("抢庄结果 pos: " + proto.pos);


        }

        public void AutoBanker(byte[] obj)
        {

            NN_ROOM_HOG_SUCCEED proto = NN_ROOM_HOG_SUCCEED.decode(obj);
            RoomNiuNiuProxy.Instance.RobBankerWin(proto, false);

            Debug.Log("没牛下庄结果 pos: " + proto.pos);

        }




        #endregion


        #region Deal 服务器广播发牌
        /// <summary>
        /// 服务器广播发牌
        /// </summary>
        /// <param name="obj"></param>
        public void Deal(byte[] obj)
        {
            Debug.Log("服务器广播发牌");
            NN_ROOM_BEGIN proto = NN_ROOM_BEGIN.decode(obj);
            RoomNiuNiuProxy.Instance.Deal(proto);

        }
        #endregion



        #region  EnterRoom 服务器广播开始下注消息
        /// <summary>
        ///   服务器广播所有玩家开始下注
        /// </summary>
        /// <param name="obj"></param>
        public void StartPour(byte[] obj)
        {

            Debug.Log("服务器广播开始下注");

            //关闭小结算
            NiuNiuGameCtrl.Instance.UISettleViewClose();

            NN_ROOM_JETTON proto = NN_ROOM_JETTON.decode(obj);
            RoomNiuNiuProxy.Instance.StartPour(proto);

            //显示设置
            ModelDispatcher.Instance.Dispatch("StartPour", null);
        }
        #endregion




        #region  EnterRoom 服务器广播某玩家下注某分         
        /// <summary>
        ///   服务器广播  单个玩家下注分数
        /// </summary>
        /// <param name="obj"></param>
        public void OneBetScore(byte[] obj)
        {


            NN_ROOM_SB_JETTON proto = NN_ROOM_SB_JETTON.decode(obj);
            Debug.Log(string.Format("有玩家下注 位置：{0} 分数：{1}", proto.pos, proto.pour));
            RoomNiuNiuProxy.Instance.OneBetScore(proto);




        }
        #endregion



        #region  EnterRoom 服务器广播玩家进入消息  (新玩家进入)
        /// <summary>
        /// 进入房间回调   服务器广播玩家进入消息
        /// </summary>
        /// <param name="obj"></param>
        public void EnterRoom(NN_ROOM_ENTER proto)
        {


            if (proto.playerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
            {
                return;
            }

            try
            {
                Debug.Log(string.Format("房间状态{0} 加入房间的玩家ID为：{1} Pos：{2}", NN_ENUM_ROOM_STATUS.IDLE.ToString(), proto.playerId, proto.pos));

                RoomNiuNiuProxy.Instance.EnterRoom(proto);

            }
            catch (Exception e)
            {
                Debug.Log(e);
            }



        }
        #endregion



        #region OnServerBroadcastLeave 
        /// <summary>
        /// 离开消息回调   服务器广播玩家离开消息
        /// </summary>
        /// <param name="obj"></param>
        public void LeaveRoom(NN_ROOM_LEAVE proto)
        {
            RoomNiuNiuProxy.Instance.ExitRoom(proto);
        }
        #endregion



        #region     OpenAPoker  服务器广播某玩家打开某些牌
        /// <summary>
        ///  服务器广播某玩家打开某些牌
        /// </summary>
        /// <param name="obj"></param>
        public void OpenAPoker(byte[] obj)
        {

            NN_ROOM_DRAW proto = NN_ROOM_DRAW.decode(obj);
            RoomNiuNiuProxy.Instance.OpenAPoker(proto);

            if(proto.hasPos()&&proto.pos == RoomNiuNiuProxy.Instance.PlayerSeat.Pos)
                NiuNiuGameCtrl.Instance.UIRubPokerViewClose();
        }
        #endregion

        #region     RubPoker  服务器广播搓牌
        /// <summary>
        ///  服务器广播搓牌
        /// </summary>
        /// <param name="obj"></param>
        public void RubPoker(byte[] obj)
        {
            NN_ROOM_ROB_POKER proto = proto = NN_ROOM_ROB_POKER.decode(obj);
            RoomNiuNiuProxy.Instance.RubPoker(proto);
            if (RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus == NN_ENUM_ROOM_STATUS.LOOKPOCKER ||
            (RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus == NN_ENUM_ROOM_STATUS.POUR && !RoomNiuNiuProxy.Instance.PlayerSeat.IsBanker && RoomNiuNiuProxy.Instance.PlayerSeat.Pour > 0))
            {
                NiuNiuGameCtrl.Instance.OpenView(UIWindowType.RubPoker_NiuNiu);
            }
        }
        #endregion
        




        #region 服务器广播开始下一局
        /// <summary>
        /// 服务器广播开始下一局
        /// </summary>
        /// <param name="obj"></param>
        public void NextGame(byte[] obj)
        {
            NN_ROOM_NEXT_GAME proto = NN_ROOM_NEXT_GAME.decode(obj);
            RoomNiuNiuProxy.Instance.NextGame(proto);//清空数据       
                         
        }
        #endregion


        #region NiuNiuGameOver   服务器广播8局结束
        /// <summary>
        ///  服务器广播8局结束
        /// </summary>
        /// <param name="obj"></param>
        public void NiuNiuGameOver(byte[] obj)
        {
            //if (RoomNiuNiuProxy.Instance.CurrentRoom.currentLoop != RoomNiuNiuProxy.Instance.CurrentRoom.maxLoop)
            //{
            //    Debug.Log("非正常的游戏结束");
            //    UIViewManager.Instance.ShowMessage("提示", "游戏已结束,欲知详情请查询战绩", MessageViewType.Ok,
            //        () => {
            //            SceneMgr.Instance.LoadScene(SceneType.Main);
            //        }
                    
            //        );
            //}
            Debug.Log("局数打完，游戏结束");
            RoomNiuNiuProxy.Instance.GameOver();

        }
        #endregion

        //--------------发送消息---（监听内容）-----------------------------------------------------------------------------
        #region 发送消息

        // 向服务器发送准备
        public void Ready(object[] obj)
        {

            if (RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus == NN_ENUM_ROOM_STATUS.IDLE)
            {
                Debug.Log("发送消息：准备");
                NetWorkSocket.Instance.Send(null, NN_SEAT_READY.CODE, GameCtrl.Instance.SocketHandle);

            }
        }

        /// <summary>
        /// 向服务器申请开始游戏  即房主点击开始
        /// </summary>
        public void applyForStartGame(object[] obj)
        {
            if (RoomNiuNiuProxy.Instance.playerNumber < 2)
            {
                UIViewManager.Instance.ShowMessage("提示", "房间人数不足", MessageViewType.Ok);
                return;
            }
          
            if (RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus == NN_ENUM_ROOM_STATUS.IDLE)
            {
                Debug.Log("发送消息：点击开始");
                NetWorkSocket.Instance.Send(null, NN_ROOM_BEGIN.CODE, GameCtrl.Instance.SocketHandle);

            }
        }
        //让庄
        public void AbdicateBanker(object[] obj)
        {

            if (RoomNiuNiuProxy.Instance.playerNumber < 2)
            {
                UIViewManager.Instance.ShowMessage("提示", "房间人数不足，不能让庄", MessageViewType.Ok);
                return;
            }

            //房间准备人数
            List<NiuNiu.Seat> seatList = RoomNiuNiuProxy.Instance.CurrentRoom.SeatList;
            for (int i = 0; i < seatList.Count; i++)
            {
                if (seatList[i].PlayerId > 0 && !seatList[i].IsReady)
                {
                    UIViewManager.Instance.ShowMessage("提示", "房间有人未准备，不能让庄", MessageViewType.Ok);
                    return;
                }
            }



            if (RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus != NN_ENUM_ROOM_STATUS.IDLE)
            {

                return;
            }

            if (RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus == NN_ENUM_ROOM_STATUS.IDLE)
            {
                Debug.Log("发送消息：让庄");
                NetWorkSocket.Instance.Send(null, NN_ROOM_HOG.CODE, GameCtrl.Instance.SocketHandle);
            }
        }


        //抢庄RobBankerNiuNiu
        public void RobBankerNiuNiu(object[] obj)
        {
           
            if (RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus == NN_ENUM_ROOM_STATUS.HOG)
            {
                NN_ROOM_HOG_GET proto = new NN_ROOM_HOG_GET();
                proto.rob_zhuang = (int)obj[0];
                NetWorkSocket.Instance.Send(proto.encode(), NN_ROOM_HOG.CODE, GameCtrl.Instance.SocketHandle);
            }
        }



        /// <summary>
        /// 向服务器申请 开牌对比
        /// </summary>
        public void RoomOpenPokers(object[] obj)
        {
            if (RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus == NN_ENUM_ROOM_STATUS.LOOKPOCKER)
            {
                //NetWorkSocket.Instance.Send(null, NN_ROOM_OPEN_POCKER);

            }
        }

        #region
        #endregion


        /// <summary>
        /// 向服务器申请 翻开一张
        /// </summary>
        /// <param name="seat"></param>
        public void NeedOpenPoker(object[] obj)
        {
            Debug.Log("点击了牌");
            if (RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus != NN_ENUM_ROOM_STATUS.LOOKPOCKER && RoomNiuNiuProxy.Instance.PlayerSeat.Pour <= 0)
            {
                Debug.Log("不是看牌阶段，无法翻开这张牌");
                return;
            }
            NN_ROOM_DRAW_GET proto = new NN_ROOM_DRAW_GET();
            List<NN_POKER> prPokerList = proto.getNnPokerList();
            NiuNiu.Seat PlayerSeat = RoomNiuNiuProxy.Instance.PlayerSeat;      
            for (int i = 0; i < obj.Length; i++)
            {
                int itemIndex = (int)obj[i];
                if (itemIndex > 0 && itemIndex <= ROOM_POKERLIST_COUNT)
                {
                    Debug.Log("牌状态：" + PlayerSeat.PokerList[itemIndex].status);

                    if (PlayerSeat.PokerList[itemIndex].status == 0)
                    {
                        NN_POKER nn_poker = new NN_POKER();
                        nn_poker.index = PlayerSeat.PokerList[itemIndex].index;
                        nn_poker.color = PlayerSeat.PokerList[itemIndex].color;
                        nn_poker.size = PlayerSeat.PokerList[itemIndex].size;
                        nn_poker.pokerStatus = (NN_ENUM_POKER_STATUS)PlayerSeat.PokerList[itemIndex].status;
                        proto.addNnPoker(nn_poker);
                        Debug.Log("要开的牌index是" + nn_poker.index);

                    }
                }
            }

            Debug.Log("发送的翻牌长度" + proto.getNnPokerList().Count);
            if (prPokerList.Count <= 0)
            {
                Debug.Log("没有要开的牌");
                return;
            }

            NetWorkSocket.Instance.Send(proto.encode(), NN_ROOM_DRAW.CODE, GameCtrl.Instance.SocketHandle);


        }

        /// <summary>
        ///  向服务器发送 搓牌
        /// </summary>
        /// <param name="obj"></param>
        public void SendRubPoker(object[] obj)
        {
            Seat seat = RoomNiuNiuProxy.Instance.PlayerSeat;
            if (seat.PokerList.Count != 5 || seat.PokerList[seat.PokerList.Count - 1].status != NN_ENUM_POKER_STATUS.POKER_STATUS_BACK)
                return;

            NN_ROOM_ROB_POKER_GET proto = new NN_ROOM_ROB_POKER_GET();       
            NetWorkSocket.Instance.Send(null, NN_ROOM_ROB_POKER_GET.CODE, GameCtrl.Instance.SocketHandle);

        }


        /// <summary>
        /// 向服务器发送 下注分选分
        /// </summary>
        /// <param name="betScore"></param>
        public void SendBetScore(object[] obj)
        {
            int pour = 0;
            if (obj.Length > 0)
            {
                pour = (int)obj[0];
            }

            NN_ROOM_SB_JETTON_GET proto = new NN_ROOM_SB_JETTON_GET();
            proto.pour = pour;


            NetWorkSocket.Instance.Send(proto.encode(), NN_ROOM_SB_JETTON.CODE, GameCtrl.Instance.SocketHandle);

        }

        /// <summary>
        /// 向服务器发送 离开房间 (表示 已收到游戏结束信号)
        /// </summary>
        /// <param name="obj"></param>
        public void SendLeaveRoom(object[] obj)
        {

            NetWorkSocket.Instance.Send(null, NN_ROOM_LEAVE.CODE, GameCtrl.Instance.SocketHandle);
        }
        #endregion



        //------------------------------------------------------------------------------------------------------

        #region 开牌比对 公开房间所有牌型  结算
        /// <summary>
        /// 开牌比对 公开房间所有牌型  结算
        /// </summary>
        /// <param name="obj"></param>
        public void RoomOpenPokerSettle(byte[] obj)
        {
            Debug.Log("开牌了");
            NiuNiuGameCtrl.Instance.UIRubPokerViewClose();

            NN_ROOM_CHECK proto = NN_ROOM_CHECK.decode(obj);
            NiuNiuGameCtrl.Instance.m_Result = proto;
            RoomNiuNiuProxy.Instance.RoomOpenPokerSettle(proto);

        }

        #endregion

    }
}