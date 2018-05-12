//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 4:20:27 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShiSanZhang;
using proto.sss;

public class ShiSanZhangGameCtrl : SystemCtrlBase<ShiSanZhangGameCtrl>, IGameCtrl, ISystemCtrl
{


    public Queue<IGameCommand> CommandQueue = new Queue<IGameCommand>();

    public List<Deck> TipDui;
    public List<Deck> TipLiangDui;
    public List<Deck> TipSanTiao;
    public List<Deck> TipShunZi;
    public List<Deck> TipTongHua;
    public List<Deck> TipTieZhi;
    public List<Deck> TipTongHuaShun;
    public List<Deck> TipHuLu;
    public int TipIndexDui;
    public int TipIndexLiangDui;
    public int TipIndexSanTiao;
    public int TipIndexShunZi;
    public int TipIndexTongHua;
    public int TipIndexTieZhi;
    public int TipIndexTongHuaShun;
    public int TipIndexHuLu;



    #region 创建房间接口实现
    /// <summary>
    /// 创建房间接口
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="settingIds"></param>
    public void CreateRoom(int groupId, List<int> settingIds)
    {
        ClientSendCreateRoom(groupId, settingIds);
    }
    #endregion

    #region 加入房间接口实现
    /// <summary>
    /// 加入房间接口
    /// </summary>
    /// <param name="roomId"></param>
    public void JoinRoom(int roomId)
    {
        ClientSendJoinRoom(roomId);
    }
    #endregion

    #region 重建房间接口实现
    /// <summary>
    /// 重建房间
    /// </summary>
    public void RebuildRoom()
    {
        ClientSendRebuild();
    }
    #endregion

    #region 离开接口实现
    /// <summary>
    /// 离开的接口
    /// </summary>
    public void QuitRoom()
    {
        if (RoomShiSanZhangProxy.Instance.CurrentRoom.currentLoop == 0)
        {
            UIViewManager.Instance.ShowMessage("提示", "是否离开房间", MessageViewType.OkAndCancel, ClientSendLeaveRoom);
        }
    }
    #endregion

    #region 解散接口实现
    /// <summary>
    /// 解散房间的接口
    /// </summary>
    public void DisbandRoom()
    {
        ShowMessage("提示", "是否解散房间", MessageViewType.OkAndCancel, ClientSendApplyDisbandRoom);
    }
    #endregion

    #region 获得房间实体的接口实现
    /// <summary>
    /// 获得房间实体的接口
    /// </summary>
    /// <returns></returns>
    public RoomEntityBase GetRoomEntity()
    {
        return RoomShiSanZhangProxy.Instance.CurrentRoom;
    }
    #endregion

    #region 聊天的接口实现
    /// <summary>
    /// 聊天的接口
    /// </summary>
    /// <param name="type"></param>
    /// <param name="playerId"></param>
    /// <param name="message"></param>
    /// <param name="audioName"></param>
    /// <param name="toPlayerId"></param>
    public void OnReceiveMessage(ChatType type, int playerId, string message, string audioName, int toPlayerId)
    {
        Debug.Log(toPlayerId + "                            TOID");
        SeatEntity seat = RoomShiSanZhangProxy.Instance.GetSeatByPlayerId(playerId);
        if (type == ChatType.Expression)//表情
        {
            if (!string.IsNullOrEmpty(audioName))
            {
                AudioEffectManager.Instance.Play(string.Format("{0}", audioName), Vector3.zero, false);
            }
            UIItemChat.Instance.ShowExpression(seat.Index, message);
        }
        else if (type == ChatType.InteractiveExpression)//互动表情
        {
            SeatEntity toSeat = RoomShiSanZhangProxy.Instance.GetSeatByPlayerId(toPlayerId);

            UIItemChat.Instance.ShowInteractiveExpression(seat.Index, message, toSeat.Index, audioName);
        }
        else if (type == ChatType.MicroPhone)//语音
        {
            UIItemChat.Instance.ShowMicroPhone(seat.Index);
        }
        else if (type == ChatType.Message)//文字
        {
            if (!string.IsNullOrEmpty(audioName))
            {
                AudioEffectManager.Instance.Play(string.Format("{0}_{1}", seat.Gender, audioName), Vector3.zero, false);
            }
            UIItemChat.Instance.ShowMessage(seat.Index, message);
        }
    }
    #endregion

    #region 打开窗口接口实现
    /// <summary>
    /// 打开窗口的接口
    /// </summary>
    /// <param name="type"></param>
    public void OpenView(UIWindowType type)
    {
        throw new NotImplementedException();
    }
    #endregion






    #region DicNotificationInterests 注册UI事件
    /// <summary>
    /// 注册UI事件
    /// </summary>
    /// <returns></returns>
    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        dic.Add("OnShiSanZhangBtnMicroUp", OnBtnMicroUp);//发送语音
        dic.Add("OnShiSanZhangBtnMicroCancel", OnBtnMicroCancel);//取消语音
        dic.Add(ShiSanZhangConstant.btnShiSanZhangViewReady, ShiSanZhangSendReady);//客户端发送准备
        dic.Add(ShiSanZhangConstant.btnShiSanZhangViewStartGame, ShiSanZhangSendStartGame);//客户端发送开始游戏
        dic.Add(ShiSanZhangConstant.btnShiSanZhangViewShare, OnBtnShiSanZhangViewShareClick);//微信邀请和分享
        dic.Add(ShiSanZhangConstant.OnShiSanZhangTipPlayPoker,OnTipPoker);//提示
        dic.Add(ShiSanZhangConstant.OnShiSanZhangRemoveHandPoker,OnShiSanZhangRemoveHandPoker);//移除手牌
        dic.Add(ShiSanZhangConstant.OnShiSanZhangAddHandPoker, OnShiSanZhangAddHandPoker);//添加到手牌        
        return dic;
    }
    #endregion


    public ShiSanZhangGameCtrl()
    {
        NetDispatcher.Instance.AddEventListener(SSS_CREATE_ROOM.CODE, OnServerBroadcastCreateRoom);//创建房间    
        NetDispatcher.Instance.AddEventListener(SSS_READY.CODE, OnServerBroadcastShiSanZhangReady);//准备  
        NetDispatcher.Instance.AddEventListener(SSS_ENTRY.CODE, OnServerBroadcastShiSanZhangEntry);//进入房间
        NetDispatcher.Instance.AddEventListener(SSS_BEGIN.CODE, OnServerBroadcastShiSanZhangBegin);//开始游戏
        NetDispatcher.Instance.AddEventListener(SSS_NOTICE_GROUP.CODE,OnServerBroadcastShiSanZhangGroup);//开始组合牌
        NetDispatcher.Instance.AddEventListener(SSS_PLAY.CODE, OnServerBroadcastShiSanZhangPlay);//出牌      

    }

    public override void Dispose()
    {
        base.Dispose();
        NetDispatcher.Instance.RemoveEventListener(SSS_CREATE_ROOM.CODE, OnServerBroadcastCreateRoom);//创建房间    
        NetDispatcher.Instance.RemoveEventListener(SSS_READY.CODE, OnServerBroadcastShiSanZhangReady);//准备  
        NetDispatcher.Instance.RemoveEventListener(SSS_ENTRY.CODE, OnServerBroadcastShiSanZhangEntry);//进入房间
        NetDispatcher.Instance.RemoveEventListener(SSS_BEGIN.CODE, OnServerBroadcastShiSanZhangBegin);//开始游戏
        NetDispatcher.Instance.RemoveEventListener(SSS_NOTICE_GROUP.CODE, OnServerBroadcastShiSanZhangGroup);//开始组合牌
        NetDispatcher.Instance.RemoveEventListener(SSS_PLAY.CODE, OnServerBroadcastShiSanZhangPlay);//出牌    


        //NetDispatcher.Instance.AddEventListener(DDZ_ROOM_CREATE.CODE, OnServerReturnCreateRoom);//创建房间     
    }


    #region 确认 取消发送语音
    //取消
    private void OnBtnMicroCancel(object[] obj)
    {
        ChatCtrl.Instance.CancelMicro();
    }
    //确认
    private void OnBtnMicroUp(object[] obj)
    {

        ChatCtrl.Instance.SendMicro();
    }
    #endregion

    #region 客户端发送创建房间
    /// <summary>
    /// 客户端发送创建房间消息
    /// </summary>
    private void ClientSendCreateRoom(int groupId, List<int> settingIds)
    {
        Debug.Log("创建房间。。。。。。。。。。。。。。");
        SSS_CREATE_ROOM_GET proto = new SSS_CREATE_ROOM_GET();
        for (int i = 0; i < settingIds.Count; ++i)
        {            
            proto.addSettingId(settingIds[i]);
        }
        proto.clubId = groupId;
        NetWorkSocket.Instance.Send(proto.encode(), SSS_CREATE_ROOM.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region 客户端发送加入房间
    /// <summary>
    /// 客户端发送加入房间
    /// </summary>
    private void ClientSendJoinRoom(int roomId)
    {
        SSS_ENTRY_GET proto = new SSS_ENTRY_GET();
        proto.roomId = roomId;
        NetWorkSocket.Instance.Send(proto.encode(), SSS_ENTRY.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region 客户端发送重建房间
    /// <summary>
    /// 客户端发送重建房间
    /// </summary>
    private void ClientSendRebuild()
    {
        NetWorkSocket.Instance.Send(null, SSS_ONLINE.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region 客户端发送准备
    /// <summary>
    /// 客户端发送准备
    /// </summary>
    /// <param name="obj"></param>
    private void ShiSanZhangSendReady(object[] obj)
    {
        NetWorkSocket.Instance.Send(null, SSS_READY.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region 客户端发送开始游戏
    /// <summary>
    /// 客户端发送开始游戏
    /// </summary>
    /// <param name="obj"></param>
    private void ShiSanZhangSendStartGame(object[] obj)
    {
        NetWorkSocket.Instance.Send(null, SSS_BEGIN.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region 客户端发送发牌结束
    public void ClientSendNoticeGroup()
    {
        NetWorkSocket.Instance.Send(null, SSS_NOTICE_GROUP.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region 客户端发出牌
    /// <summary>
    /// 客户端发送配牌
    /// </summary>
    public void PlayPoker()
    {
        bool isAsk= DeckRules.AskIsPlayPoker(RoomShiSanZhangProxy.Instance.PlayerSeat.firstPokerList,RoomShiSanZhangProxy.Instance.PlayerSeat.middlePokerList);
        bool isAsk1 = DeckRules.AskIsPlayPoker(RoomShiSanZhangProxy.Instance.PlayerSeat.middlePokerList, RoomShiSanZhangProxy.Instance.PlayerSeat.endPokerList);
        Debug.Log(isAsk+"       "+isAsk1+"                 检测的牌型");
        if (isAsk && isAsk1)
        {
            SSS_PLAY_GET proto = new SSS_PLAY_GET();
            proto.seatInfo = new SEAT_INFO();
            for (int i = 0; i < RoomShiSanZhangProxy.Instance.PlayerSeat.firstPokerList.Count; i++)
            {
                FIRST_POKER_INFO firstPoker = new FIRST_POKER_INFO();
                firstPoker.size = RoomShiSanZhangProxy.Instance.PlayerSeat.firstPokerList[i].Size;
                firstPoker.color = RoomShiSanZhangProxy.Instance.PlayerSeat.firstPokerList[i].Color;
                firstPoker.index = RoomShiSanZhangProxy.Instance.PlayerSeat.firstPokerList[i].Index;
                proto.seatInfo.addFirstPokerInfo(firstPoker);
            }
            for (int i = 0; i < RoomShiSanZhangProxy.Instance.PlayerSeat.middlePokerList.Count; i++)
            {
                SECOND_POKER_INFO twoPoker = new SECOND_POKER_INFO();
                twoPoker.size = RoomShiSanZhangProxy.Instance.PlayerSeat.middlePokerList[i].Size;
                twoPoker.color = RoomShiSanZhangProxy.Instance.PlayerSeat.middlePokerList[i].Color;
                twoPoker.index = RoomShiSanZhangProxy.Instance.PlayerSeat.middlePokerList[i].Index;
                proto.seatInfo.addSecondPokerInfo(twoPoker);
            }
            for (int i = 0; i < RoomShiSanZhangProxy.Instance.PlayerSeat.endPokerList.Count; i++)
            {
                THIRD_POKER_INFO threePoker = new THIRD_POKER_INFO();
                threePoker.size = RoomShiSanZhangProxy.Instance.PlayerSeat.endPokerList[i].Size;
                threePoker.color = RoomShiSanZhangProxy.Instance.PlayerSeat.endPokerList[i].Color;
                threePoker.index = RoomShiSanZhangProxy.Instance.PlayerSeat.endPokerList[i].Index;
                proto.seatInfo.addThirdPokerInfo(threePoker);
            }
            NetWorkSocket.Instance.Send(proto.encode(), SSS_PLAY.CODE, GameCtrl.Instance.SocketHandle);
        }
        else
        {
            UITipPokerShiSanZhangView.Instance.FalseHints();
        }
    }
    #endregion

    #region 客户端发送离开
    /// <summary>
    /// 客户端发送离开的消息
    /// </summary>
    public void ClientSendLeaveRoom()
    {
        // NetWorkSocket.Instance.Send(null, GP_ROOM_LEAVE.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region 客户端发送解散
    /// <summary>
    /// 客户端发送请求解散房间
    /// </summary>
    private void ClientSendApplyDisbandRoom()
    {
        //NetWorkSocket.Instance.Send(null, GP_ROOM_APPLYDISMISS.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion






    #region 服务器广播创建房间
    /// <summary>
    /// 创建房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastCreateRoom(byte[] obj)
    {
        SSS_CREATE_ROOM proto = SSS_CREATE_ROOM.decode(obj);
        CommandQueue.Clear();
        UIViewManager.Instance.CloseWait();
        RoomShiSanZhangProxy.Instance.InitRoom(proto);
        //IGameCommand command = new CreateRoomCommand(room);
        //CommandQueue.Enqueue(command);
        SceneMgr.Instance.LoadScene(SceneType.ShiSanZhang);
    }
    #endregion

    #region 服务器广播准备
    /// <summary>
    /// 服务器广播准备
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastShiSanZhangReady(byte[] obj)
    {
        SSS_READY proto = SSS_READY.decode(obj);
        IGameCommand command = new ReadyCommand(proto.pos);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region 服务器广播进入房间
    /// <summary>
    /// 服务器广播进入房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastShiSanZhangEntry(byte[] obj)
    {
        SSS_ENTRY proto = SSS_ENTRY.decode(obj);
        IGameCommand command = new EnterRoomCommand(proto.playerId, proto.gold, proto.avatar, proto.gender, proto.nickname, proto.pos);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region 服务器广播开始游戏
    /// <summary>
    /// 服务器广播开始有洗
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastShiSanZhangBegin(byte[] obj)
    {
        SSS_BEGIN proto = SSS_BEGIN.decode(obj);           
        IGameCommand command = new StartGameCommand(proto.getDealPokerList());
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region 服务器广播组合牌
    private void OnServerBroadcastShiSanZhangGroup(byte[] obj)
    {
        IGameCommand command = new GroupPokerCommand();
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region 服务器广播出牌
    /// <summary>
    /// 服务器广播出牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastShiSanZhangPlay(byte[] obj)
    {
        SSS_PLAY proto = SSS_PLAY.decode(obj);
        IGameCommand command = new PlayPokerCommand(proto.pos);
        CommandQueue.Enqueue(command);
    }
    #endregion




    #region  微信分享按钮点击
    /// <summary>
    /// 分享按钮点击及微信邀请
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnShiSanZhangViewShareClick(object[] obj)
    {
        Debug.Log("           打开微信邀请                ");
        ShareCtrl.Instance.ShareURL(ShareType.InGame);
    }
    #endregion

    #region 提示
    /// <summary>
    /// 提示
    /// </summary>
    /// <param name="obj"></param>
    private void OnTipPoker(object[] obj)
    {
        if (RoomShiSanZhangProxy.Instance.PlayerSeat == null) return;
        DeckType deckType = (DeckType)obj[0];
        if (deckType==DeckType.DUI_ZI)
        {
            if (TipDui == null || TipDui.Count == 0)
            {
                TipDui = DeckRules.GetAllDuiZi(RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList);
                TipIndexDui = 0;
                if (TipDui == null || TipDui.Count == 0) return;
            }
            UITipPokerShiSanZhangView.Instance.SelectPoker(TipDui[TipIndexDui].pokers);
            ++TipIndexDui;
            if (TipIndexDui == TipDui.Count) TipIndexDui = 0;
        }
        else if(deckType==DeckType.LIANG_DUI)
        {
            if (TipLiangDui == null || TipLiangDui.Count == 0)
            {
                TipLiangDui = DeckRules.GetAllLiangDui(RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList);
                TipIndexLiangDui = 0;
                if (TipLiangDui == null || TipLiangDui.Count == 0) return;
            }
            UITipPokerShiSanZhangView.Instance.SelectPoker(TipLiangDui[TipIndexLiangDui].pokers);
            ++TipIndexLiangDui;
            if (TipIndexLiangDui == TipLiangDui.Count) TipIndexLiangDui = 0;
        }
        else if (deckType == DeckType.SAN_TIAO)
        {
            if (TipSanTiao == null || TipSanTiao.Count == 0)
            {
                TipSanTiao = DeckRules.GetAllSanTiao(RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList);
                TipIndexSanTiao = 0;
                if (TipSanTiao == null || TipSanTiao.Count == 0) return;
            }
            UITipPokerShiSanZhangView.Instance.SelectPoker(TipSanTiao[TipIndexSanTiao].pokers);
            ++TipIndexSanTiao;
            if (TipIndexSanTiao == TipSanTiao.Count) TipIndexSanTiao = 0;
            UITipPokerShiSanZhangView.Instance.TipKuang();
        }
        else if (deckType == DeckType.SHUN_ZI)
        {
            if (TipShunZi == null || TipShunZi.Count == 0)
            {
                TipShunZi = DeckRules.GetAllShunZi(RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList);
                TipIndexShunZi = 0;
                if (TipShunZi == null || TipShunZi.Count == 0) return;
            }
            UITipPokerShiSanZhangView.Instance.SelectPoker(TipShunZi[TipIndexShunZi].pokers);
            ++TipIndexShunZi;
            if (TipIndexShunZi == TipShunZi.Count) TipIndexShunZi = 0;
            UITipPokerShiSanZhangView.Instance.TipKuang();
        }
        else if (deckType == DeckType.TONG_HUA)
        {
            if (TipTongHua == null || TipTongHua.Count == 0)
            {
                TipTongHua = DeckRules.GetAllTongHua(RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList);
                TipIndexTongHua = 0;
                if (TipTongHua == null || TipTongHua.Count == 0) return;
            }
            UITipPokerShiSanZhangView.Instance.SelectPoker(TipTongHua[TipIndexTongHua].pokers);
            ++TipIndexTongHua;
            if (TipIndexTongHua == TipTongHua.Count) TipIndexTongHua = 0;
            UITipPokerShiSanZhangView.Instance.TipKuang();
        }
        else if (deckType == DeckType.TIE_ZHI)
        {
            if (TipTieZhi == null || TipTieZhi.Count == 0)
            {
                TipTieZhi = DeckRules.GetAllTieZhi(RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList);
                TipIndexTieZhi = 0;
                if (TipTieZhi == null || TipTieZhi.Count == 0) return;
            }
            UITipPokerShiSanZhangView.Instance.SelectPoker(TipTieZhi[TipIndexTieZhi].pokers);
            ++TipIndexTieZhi;
            if (TipIndexTieZhi == TipTieZhi.Count) TipIndexTieZhi = 0;
        }
        else if (deckType == DeckType.HU_LU)
        {
            if (TipHuLu == null || TipHuLu.Count == 0)
            {
                TipHuLu = DeckRules.GetAllHuLu(RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList);
                TipIndexHuLu = 0;
                if (TipHuLu == null || TipHuLu.Count == 0) return;
            }
            UITipPokerShiSanZhangView.Instance.SelectPoker(TipHuLu[TipIndexHuLu].pokers);
            ++TipIndexHuLu;
            if (TipIndexHuLu == TipHuLu.Count) TipIndexHuLu = 0;
            UITipPokerShiSanZhangView.Instance.TipKuang();
        }
        else if (deckType == DeckType.TONG_HUA_SHUN)
        {
            if (TipTongHuaShun == null || TipTongHuaShun.Count == 0)
            {
                TipTongHuaShun = DeckRules.GetAllTongHuaShun(RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList);
                TipIndexTongHuaShun = 0;
                if (TipTongHuaShun == null || TipTongHuaShun.Count == 0) return;
            }
            UITipPokerShiSanZhangView.Instance.SelectPoker(TipTongHuaShun[TipIndexTongHuaShun].pokers);
            ++TipIndexTongHuaShun;
            if (TipIndexTongHuaShun == TipTongHuaShun.Count) TipIndexTongHuaShun = 0;
            UITipPokerShiSanZhangView.Instance.TipKuang();
        }
    }
    #endregion

    #region 移除手牌
    /// <summary>
    /// 移除手牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnShiSanZhangRemoveHandPoker(object[] obj)
    {
        if (RoomShiSanZhangProxy.Instance.PlayerSeat == null) return;
        Poker poker = obj[0] as Poker;
        LevelType levelType = (LevelType)obj[1];
        RoomShiSanZhangProxy.Instance.RemoveHandPokerProxy(poker,levelType);
    }
    #endregion

    #region 增加手牌
    /// <summary>
    /// 增加手牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnShiSanZhangAddHandPoker(object[] obj)
    {
        if (RoomShiSanZhangProxy.Instance.PlayerSeat == null) return;
        Poker poker = obj[0] as Poker;
        LevelType levelType = (LevelType)obj[1];
        RoomShiSanZhangProxy.Instance.AddHandPokerProxy(poker, levelType);
    }
    #endregion




}
