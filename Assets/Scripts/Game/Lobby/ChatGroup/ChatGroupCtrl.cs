//===================================================
//Author      : DRB
//CreateTime  ：9/23/2017 1:39:24 PM
//Description ：群友会控制器
//===================================================
using System;
using System.Collections.Generic;
using proto.common;
using UnityEngine;


/// <summary>
/// 群友会控制器
/// </summary>
public class ChatGroupCtrl : SystemCtrlBase<ChatGroupCtrl>, ISystemCtrl
{
    #region Members
    private UIChatGroupView m_UIChatGroupView;//群友会窗口

    private UIChatRoomDetailView m_UIChatRoomDetailView;//群友会房间详情窗口

    private UIChatGroupApplyView m_UIChatGroupApplyView;//群友会申请列表窗口

    private UIChatGroupCardsView m_UIChatGroupCardsView;//群友会设置房卡窗口


    private int m_SocketHandle;

    private int m_CurrentSelectGroupId;

    private int m_CurrentSelectRoomId;

    private int m_CurrentSelectMemberId;
    #endregion

    #region Constructor
    public ChatGroupCtrl()
    {
        NetDispatcher.Instance.AddEventListener(OP_CLUB_CREATE.CODE, OnServerReturnCreateGroup);//服务器返回创建群消息
        NetDispatcher.Instance.AddEventListener(OP_CLUB_APPLY.CODE, OnServerReturnJoinGroup);//服务器返回加入群消息
        NetDispatcher.Instance.AddEventListener(OP_CLUB_ENTER.CODE, OnServerBroadcastJoinGroup);//服务器广播加入群消息
        NetDispatcher.Instance.AddEventListener(OP_CLUB_DISMISS.CODE, OnServerBroadcastDisbandGroup);//服务器广播解散群消息
        NetDispatcher.Instance.AddEventListener(OP_CLUB_LEAVE.CODE, OnServerBroadcastLeaveGroup);//服务器广播离开群消息
        NetDispatcher.Instance.AddEventListener(OP_CLUB_KICK.CODE, OnServerBroadcastnPleaseLeaveGroup);//服务器广播踢人消息
        NetDispatcher.Instance.AddEventListener(OP_CLUB_LIST.CODE, OnServerReturnGroupList);//服务器返回群列表消息
        NetDispatcher.Instance.AddEventListener(OP_CLUB_PLAYER_LIST.CODE, OnServerReturnMemberList);//服务器返回成员列表消息
        NetDispatcher.Instance.AddEventListener(OP_CLUB.CODE, OnServerReturnGroupInfo);//服务器返回群信息消息
        NetDispatcher.Instance.AddEventListener(OP_CLUB_ROOM_LIST.CODE, OnServerReturnRoomList);//服务器返回房间列表消息
        NetDispatcher.Instance.AddEventListener(OP_CLUB_ROOM.CODE, OnServerReturnRoomInfo);//服务器返回房间信息消息
        NetDispatcher.Instance.AddEventListener(OP_CLUB_APPLY.CODE, OnServerReturnApply);//服务器返回申请进群消息
        NetDispatcher.Instance.AddEventListener(OP_CLUB_APPLY_LIST.CODE, OnServerReturnApplyList);//服务器返回申请进群消息
        NetDispatcher.Instance.AddEventListener(OP_CLUB_AGREE.CODE, OnServerReturnAgreeApply);//服务器返回同意申请消息
        NetDispatcher.Instance.AddEventListener(OP_CLUB_REJECT.CODE, OnServerReturnRefuseApply);//服务器返回拒绝申请消息
        NetDispatcher.Instance.AddEventListener(OP_CLUB_MSG.CODE, OnServerReturnMessage);//服务器返回聊天消息
        NetDispatcher.Instance.AddEventListener(OP_CLUB_ROOM_ENTER.CODE, OnServerBroadcastEnterRoom);//服务器广播玩家进入房间
        NetDispatcher.Instance.AddEventListener(OP_CLUB_ROOM_DISMISS.CODE, OnServerBroadcastDisbandRoom);//服务器广播房间解散
        NetDispatcher.Instance.AddEventListener(OP_CLUB_ROOM_LEAVE.CODE, OnServerBroadcastLeaveRoom);//服务器广播房间解散
        NetDispatcher.Instance.AddEventListener(OP_CLUB_ROOM_CREATE.CODE, OnServerBroadcastCreateRoom);//服务器广播创建房间
        NetDispatcher.Instance.AddEventListener(OP_CLUB_ROOM_KICK.CODE, OnServerBroadcastRoomKick);//服务器广播房间踢人
        NetDispatcher.Instance.AddEventListener(OP_CLUB_ROOM_STATUS.CODE, OnServerBroadcastRoomStatus);//服务器广播房间状态
        NetDispatcher.Instance.AddEventListener(OP_CLUB_PLAYER_STATUS.CODE, OnServerBroadcastPlayerStatus);//服务器广播玩家状态
        NetDispatcher.Instance.AddEventListener(OP_CLUB_MSG_LIST.CODE, OnServerReturnMessageList);//服务器广播聊天列表
        NetDispatcher.Instance.AddEventListener(OP_CLUB_RECORD.CODE, OnServerReturnRecord);//服务器广播战绩
        NetDispatcher.Instance.AddEventListener(OP_CLUB_RECORD_LIST.CODE, OnServerReturnRecordList);//服务器广播战绩列表
        NetDispatcher.Instance.AddEventListener(OP_CLUB_SET_PLAYER.CODE, OnServerBroadcastIdentityChanged);//服务器广播身份变更
        NetDispatcher.Instance.AddEventListener(OP_CLUB_SET_CARD.CODE, OnServerBroadcastSetCards);//服务器广播房卡数量变更
        NetDispatcher.Instance.AddEventListener(OP_CLUB_ROOM_CLEAR.CODE, OnServerBroadcastClearRoom);//服务器广播清空房间消息
    }
    #endregion

    #region DicNotificationInterests UI事件注册
    /// <summary>
    /// UI事件注册
    /// </summary>
    /// <returns></returns>
    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        dic.Add("OnChatGroupClick", OnChatGroupClick);//群点击
        dic.Add("OnChatMemberClick", OnChatMemberClick);//成员点击
        dic.Add("btnChatGroupViewCreatGroup", OnBtnChatGroupViewCreatGroup);//创建群按钮
        dic.Add("btnChatGroupViewJoinGroup", OnBtnChatGroupViewJoinGroup);//加入群按钮
        dic.Add("btnChatGroupViewCreatRoom", OnBtnChatGroupViewCreatRoom);//创建房间按钮
        dic.Add("OnChatRoomDetailClick", OnChatRoomDetailClick);//房间详情按钮
        dic.Add("btnChatRoomDetailJoin", OnBtnChatRoomDetailJoin);//房间详情界面加入按钮
        dic.Add("OnChatRoomPlayerClick", OnChatRoomPlayerClick);//房间玩家点击
        dic.Add("btnChatGroupViewApply", OnBtnChatGroupViewApply);//申请按钮
        dic.Add("OnChatGroupAgreeApplyClick", OnChatGroupAgreeApplyClick);//同意申请按钮
        dic.Add("OnChatGroupRefuseApplyClick", OnChatGroupRefuseApplyClick);//拒绝申请按钮
        dic.Add("btnChatGroupViewDisband", OnBtnChatGroupViewDisband);//解散按钮
        dic.Add("btnChatGroupViewSendMessage", OnBtnChatGroupViewSendMessage);//发送消息按钮
        dic.Add("OnMessageScrollViewTop", OnMessageScrollViewTop);//消息滚动视图拉到最上
        dic.Add("btnChatGroupViewAppoint", OnAppointClick);//任命管理员按钮
        dic.Add("btnChatGroupViewDimission", OnDimissionClick);//卸任管理员按钮
        dic.Add("btnChatGroupViewKick", OnKickClick);//踢人按钮
        dic.Add("btnChatGroupViewSettingCards", OnSettingCardClick);//设置房卡按钮
        dic.Add("btnChatGroupViewDeposit", OnDepositClick);//存钱按钮
        dic.Add("btnChatGroupViewEnchashment", OnEnchashmentClick);//提现按钮
        dic.Add("btnChatGroupViewExit", OnExitClick);//退群点击
        dic.Add("OnChatRoomInviteClick", OnChatRoomInviteClick);//邀请按钮点击
        dic.Add("btnChatRoomDetailDisband", OnDisbandRoomClick);//解散房间按钮点击
        return dic;
    }
    #endregion

    #region Override ISystemCtrl
    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.ChatGroup:
                OpenChatGroupView();
                break;
        }
    }

    #region OpenChatGroupView 打开群友会窗口
    /// <summary>
    /// 打开群友会窗口
    /// </summary>
    private void OpenChatGroupView()
    {
        m_UIChatGroupView = UIViewUtil.Instance.LoadWindow(UIWindowType.ChatGroup).GetComponent<UIChatGroupView>();
        if (!NetWorkSocket.Instance.Connected(m_SocketHandle))
        {
            ConnectChatServer();
        }
        else
        {
            m_UIChatGroupView.ShowGroupList(ChatGroupProxy.Instance.AllGroup);
        }
    }
    #endregion

    #region OpenChatRoomDetailView 打开群友会房间详情窗口
    /// <summary>
    /// 打开群友会房间详情窗口
    /// </summary>
    private void OpenChatRoomDetailView(ChatGroupEntity group, RoomEntityBase room)
    {
        if (group == null) return;
        if (room == null) return;
        m_UIChatRoomDetailView = UIViewUtil.Instance.LoadWindow(UIWindowType.ChatRoomDetail).GetComponent<UIChatRoomDetailView>();
        cfg_gameEntity game = cfg_gameDBModel.Instance.Get(room.gameId);
        string gameName = string.Empty;
        if (game != null)
        {
            gameName = game.GameName;
        }
        bool isManager = false;
        PlayerEntity self = ChatGroupProxy.Instance.GetMember(group.id, AccountProxy.Instance.CurrentAccountEntity.passportId);
        if (self != null)
        {
            isManager = self.isManager;
        }
        m_UIChatRoomDetailView.SetRoomInfo(group.id, room.roomId, gameName, room.players.Count, room.currentLoop, room.maxLoop, self.isManager);
        m_UIChatRoomDetailView.SetRoomPlayer(room.players);
        Dictionary<string, string> settingDic = new Dictionary<string, string>();
        string strSetting = string.Empty;
        for (int i = 0; i < room.Config.Count; ++i)
        {
            if (!settingDic.ContainsKey(room.Config[i].label))
            {
                settingDic[room.Config[i].label] = room.Config[i].name;
            }
            else
            {
                settingDic[room.Config[i].label] += " " + room.Config[i].name;
            }
        }
        foreach (var pair in settingDic)
        {
            strSetting += pair.Key + ":";
            strSetting += pair.Value + "\r\n";
        }
        m_UIChatRoomDetailView.SetRoomSetting(strSetting);
    }
    #endregion

    #region OpenChatGroupApplyView 打开群友会申请窗口
    /// <summary>
    /// 打开群友会申请窗口
    /// </summary>
    private void OpenChatGroupApplyView(int groupId)
    {
        m_UIChatGroupApplyView = UIViewUtil.Instance.LoadWindow(UIWindowType.ChatGroupApply).GetComponent<UIChatGroupApplyView>();
        m_UIChatGroupApplyView.SetUI(groupId);
    }
    #endregion

    #region OpenChatGroupCardsView 打开群友会设置房卡窗口
    /// <summary>
    /// 打开群友会设置房卡窗口
    /// </summary>
    private void OpenChatGroupCardsView(int groupId)
    {
        m_UIChatGroupCardsView = UIViewUtil.Instance.LoadWindow(UIWindowType.ChatGroupCards).GetComponent<UIChatGroupCardsView>();

        ChatGroupEntity group = ChatGroupProxy.Instance.GetGroup(groupId);
        if (group == null) return;
        TransferData data = new TransferData();
        data.SetValue("GroupId", groupId);
        data.SetValue("PlayerCardCount", AccountProxy.Instance.CurrentAccountEntity.cards);
        data.SetValue("GroupCardCount", group.cards);
        m_UIChatGroupCardsView.SetUI(data);
    }
    #endregion
    #endregion

    #region 按钮点击

    #region OnBtnChatGroupViewCreatGroup 创建群按钮点击
    /// <summary>
    /// 创建群按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnChatGroupViewCreatGroup(object[] obj)
    {
        string groupName = obj[0].ToString();
        int headIndex = (int)obj[1];

        ClientSendCreateGroup(groupName, headIndex);
    }
    #endregion

    #region OnBtnChatGroupViewJoinGroup 加入群按钮点击
    /// <summary>
    /// 加入群按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnChatGroupViewJoinGroup(object[] obj)
    {
        int groupId = (int)obj[0];
        if (groupId <= 0)
        {
            ShowTip("请输入有效的群号");
            return;
        }

        ClientSendJoinGroup(groupId);
    }
    #endregion

    #region OnBtnChatGroupViewDisband 解散群按钮点击
    /// <summary>
    /// 解散群按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnChatGroupViewDisband(object[] obj)
    {
        int groupId = (int)obj[0];
        if (ChatGroupProxy.Instance.GetGroup(groupId) == null) return;
        m_CurrentSelectGroupId = groupId;
        ShowMessage("提示", "是否解散该群", MessageViewType.OkAndCancel, DisbandGroup);
    }
    #endregion

    #region DisbandGroup 确认解散
    /// <summary>
    /// 确认解散
    /// </summary>
    private void DisbandGroup()
    {
        ClientSendDisbandGroup(m_CurrentSelectGroupId);
    }
    #endregion

    #region OnBtnChatGroupViewCreatRoom 创建房间按钮点击
    /// <summary>
    /// 创建房间按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnChatGroupViewCreatRoom(object[] obj)
    {
        int groupId = (int)obj[0];
#if IS_WANGQUE
        GameCtrl.Instance.OpenCreateRoomView(0, groupId);
#else
        GameCtrl.Instance.OpenCreateRoomView(cfg_gameDBModel.Instance.GetList()[0].id, groupId);
#endif
    }
    #endregion

    #region OnBtnChatGroupViewApply 申请按钮点击
    /// <summary>
    /// 申请按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnChatGroupViewApply(object[] obj)
    {
        int groupId = (int)obj[0];
        ChatGroupEntity group = ChatGroupProxy.Instance.GetGroup(groupId);
        if (group == null) return;
        //OpenChatGroupApplyView(groupId);
        //if (!group.hasNewApply)
            //ChatGroupProxy.Instance.RefreshApply(groupId);
        //if (group.hasNewApply)
            ClientRequestApplyList(groupId);
    }
    #endregion

    #region OnChatGroupAgreeApplyClick 同意申请按钮点击
    /// <summary> 
    /// 同意申请按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnChatGroupAgreeApplyClick(object[] obj)
    {
        int groupId = (int)obj[0];
        int playerId = (int)obj[1];
        ClientSendAgreeApply(groupId, playerId);
    }
    #endregion

    #region OnChatGroupRefuseApplyClick 拒绝申请按钮点击
    /// <summary>
    /// 拒绝申请按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnChatGroupRefuseApplyClick(object[] obj)
    {
        int groupId = (int)obj[0];
        int playerId = (int)obj[1];
        ClientSendRefuseApply(groupId, playerId);
    }
    #endregion

    #region OnBtnChatGroupViewSendMessage 发送消息按钮点击
    /// <summary>
    /// 发送消息按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnChatGroupViewSendMessage(object[] obj)
    {
        int groupId = (int)obj[0];
        string message = obj[1].ToString();

        if (string.IsNullOrEmpty(message)) return;

        ClientSendChatMessage(groupId, message);
    }
    #endregion

    #region OnChatGroupClick 聊天群点击
    /// <summary>
    /// 聊天群点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnChatGroupClick(object[] obj)
    {
        int groupId = (int)obj[0];
        ChatGroupEntity group = ChatGroupProxy.Instance.GetGroup(groupId);
        if (group == null) return;
        //ClientRequestApplyList(groupId);
        if (!group.isRequested)
        {
            group.isRequested = true;
            ClientSendRequestMemberList(groupId);
            ClientSendRequestRoomList(groupId);
            ClientSendRequestChatMessageList(groupId);
            ClientSendRequestRecordList(groupId);
        }
        TransferData data = new TransferData();
        data.SetValue("GroupId", group.id);
        data.SetValue("IsOwner", group.isOwner);
        data.SetValue("HasNewApply", group.hasNewApply);
        data.SetValue("GroupName", group.name);
        data.SetValue("MemberCount", group.currMemberCount);
        data.SetValue("MaxMemberCount", group.maxMemberCount);
        data.SetValue("OwnerId", group.ownerId);
        data.SetValue("CardCount", group.cards);
        data.SetValue("AvatarIndex", group.avatarIndex);
        data.SetValue("WaitingRoomCount", group.WaitingRoomCount);
        data.SetValue("GamingRoomCount", group.GamingRoomCount);
        data.SetValue("PlayerIsManager", group.playerEntity==null?false: group.playerEntity.isManager);
        data.SetValue("GroupAvatar", group.avatar);
        List<TransferData> lstRoom = new List<TransferData>();
        List<TransferData> lstMember = new List<TransferData>();
        List<TransferData> lstMessage = new List<TransferData>();
        List<TransferData> lstRecord = new List<TransferData>();
        for (int i = 0; i < group.rooms.Count; ++i)
        {
            RoomEntityBase roomEntity = group.rooms[i];
            TransferData roomData = new TransferData();
            roomData.SetValue("RoomId", roomEntity.roomId);
            roomData.SetValue("GameName", roomEntity.GameName);
            roomData.SetValue("RoomStatus", roomEntity.roomStatus);
            List<TransferData> lstPlayer = new List<TransferData>();
            for (int j = 0; j < roomEntity.players.Count; ++j)
            {
                PlayerEntity playerEntity = roomEntity.players[j];
                TransferData playerData = new TransferData();
                if (playerEntity == null)
                {
                    playerData.SetValue("PlayerId", 0);
                    playerData.SetValue("Avatar", string.Empty);
                }
                else
                {
                    playerData.SetValue("PlayerId", playerEntity.id);
                    playerData.SetValue("Avatar", playerEntity.avatar);
                }
                lstPlayer.Add(playerData);
            }
            roomData.SetValue("Player", lstPlayer);
            lstRoom.Add(roomData);
        }
        for (int i = 0; i < group.members.Count; ++i)
        {
            PlayerEntity playerEntity = group.members[i];
            TransferData memberData = new TransferData();
            memberData.SetValue("PlayerId", playerEntity.id);
            memberData.SetValue("Nickname", playerEntity.nickname);
            memberData.SetValue("IsOnline", playerEntity.online > 0);
            memberData.SetValue("Avatar", playerEntity.avatar);
            memberData.SetValue("IsOwner", playerEntity.isOwner);
            memberData.SetValue("IsManager", playerEntity.isManager);
            lstMember.Add(memberData);
        }
        for (int i = 0; i < group.messageList.Count; ++i)
        {
            MessageEntity messageEntity = group.messageList[i];
            TransferData messageData = new TransferData();
            messageData.SetValue("Avatar", messageEntity.sendPlayer.avatar);
            messageData.SetValue("Message", messageEntity.message);
            messageData.SetValue("IsPlayer", messageEntity.isPlayer);
            lstMessage.Add(messageData);
        }
        for (int i = group.recordList.Count - 1; i >= 0; --i)
        {
            RecordEntity recordEntity = group.recordList[i];
            TransferData recordData = new TransferData();
            recordData.SetValue("RoomId", recordEntity.roomId);
            recordData.SetValue("BattleId", recordEntity.battleId);
            recordData.SetValue("DateTime", recordEntity.time);
            recordData.SetValue("GameName", recordEntity.gameName);
            List<TransferData> lstPlayer = new List<TransferData>();
            for (int j = 0; j < recordEntity.players.Count; ++j)
            {
                RecordPlayerEntity playerEntity = recordEntity.players[j];
                TransferData playerData = new TransferData();
                playerData.SetValue("Avatar", playerEntity.avatar);
                playerData.SetValue("Gold", playerEntity.gold);
                playerData.SetValue("Nickname", playerEntity.nickname);
                playerData.SetValue("PlayerId", playerEntity.id);
                lstPlayer.Add(playerData);
            }
            recordData.SetValue("Player", lstPlayer);
            lstRecord.Add(recordData);
        }
        data.SetValue("Room", lstRoom);
        data.SetValue("Member", lstMember);
        data.SetValue("Message", lstMessage);
        data.SetValue("Record", lstRecord);
        m_UIChatGroupView.ShowGroup(data);
    }
    #endregion

    #region OnChatMemberClick 聊天群成员点击
    /// <summary>
    /// 聊天群成员点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnChatMemberClick(object[] obj)
    {
        int groupId = (int)obj[0];
        int playerId = (int)obj[1];
        ChatGroupEntity group = ChatGroupProxy.Instance.GetGroup(groupId);
        if (group == null) return;
        PlayerEntity player = ChatGroupProxy.Instance.GetMember(groupId, playerId);
        if (player == null) return;

        PlayerEntity self = ChatGroupProxy.Instance.GetMember(groupId, AccountProxy.Instance.CurrentAccountEntity.passportId);

        if (player.id == AccountProxy.Instance.CurrentAccountEntity.passportId) return;

        bool canKick = false;
        if (self.isOwner)
        {
            canKick = true;
        }
        if (self.isManager && !player.isManager)
        {
            canKick = true;
        }


        m_UIChatGroupView.ShowMemberOption(playerId, !player.isManager && self.isOwner, player.isManager && self.isOwner, canKick);
    }
    #endregion

    #region OnAppointClick 任命管理员按钮点击
    /// <summary>
    /// 任命管理员按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnAppointClick(object[] obj)
    {
        int groupId = (int)obj[0];
        int playerId = (int)obj[1];
        ClientSendAppointManager(groupId, playerId);
    }
    #endregion

    #region OnDimissionClick 卸任管理员按钮点击
    /// <summary>
    /// 卸任管理员按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnDimissionClick(object[] obj)
    {
        int groupId = (int)obj[0];
        int playerId = (int)obj[1];
        ClientSendDimissionManager(groupId, playerId);
    }
    #endregion

    #region OnSettingCardClick 设置房卡按钮点击
    /// <summary>
    /// 设置房卡按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnSettingCardClick(object[] obj)
    {
        int groupId = (int)obj[0];
        OpenChatGroupCardsView(groupId);
    }
    #endregion

    #region OnDepositClick 存钱按钮点击
    /// <summary>
    /// 存钱按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnDepositClick(object[] obj)
    {
        int groupId = (int)obj[0];
        int cards = (int)obj[1];
        if (cards > AccountProxy.Instance.CurrentAccountEntity.cards)
        {
            ShowTip("您的房卡数量不足");
            return;
        }
        if (cards <= 0)
        {
            ShowTip("请输入房卡数量");
            return;
        }

        ClientSendSetCards(groupId, cards);
    }
    #endregion

    #region OnEnchashmentClick 提现按钮点击
    /// <summary>
    /// 提现按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnEnchashmentClick(object[] obj)
    {
        int groupId = (int)obj[0];
        int cards = (int)obj[1];
        ChatGroupEntity group = ChatGroupProxy.Instance.GetGroup(groupId);
        if (group == null) return;

        if (cards > group.cards)
        {
            ShowTip("俱乐部房卡数量不足");
            return;
        }
        if (cards <= 0)
        {
            ShowTip("请输入房卡数量");
            return;
        }

        ClientSendSetCards(groupId, ~cards + 1);
    }
    #endregion

    #region OnKickClick 踢人按钮点击
    /// <summary>
    /// 踢人按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnKickClick(object[] obj)
    {
        int groupId = (int)obj[0];
        int playerId = (int)obj[1];
        m_CurrentSelectGroupId = groupId;
        m_CurrentSelectMemberId = playerId;
        PlayerEntity player = ChatGroupProxy.Instance.GetMember(groupId, playerId);
        if (player == null) return;
        ShowMessage("提示", string.Format("是否从该群踢掉{0}", player.nickname), MessageViewType.OkAndCancel, KickMember);
    }
    #endregion

    #region KickMember 确认踢人
    /// <summary>
    /// 确认踢人
    /// </summary>
    private void KickMember()
    {
        ClientSendPleaseLeave(m_CurrentSelectGroupId, m_CurrentSelectMemberId);
    }
    #endregion

    #region OnChatRoomPlayerClick 聊天群房间玩家点击
    /// <summary>
    /// 聊天群房间玩家点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnChatRoomPlayerClick(object[] obj)
    {
        int groupId = (int)obj[0];
        int roomId = (int)obj[1];
        int playerId = (int)obj[2];
        ChatGroupEntity group = ChatGroupProxy.Instance.GetGroup(groupId);
        if (group == null) return;
        RoomEntityBase room = ChatGroupProxy.Instance.GetRoom(groupId, roomId);
        if (room == null) return;
        if (playerId > 0)
        {
            PlayerEntity player = ChatGroupProxy.Instance.GetMember(groupId, playerId);
            if (player == null) return;
            if (ChatGroupProxy.Instance.CheckOwner(groupId, AccountProxy.Instance.CurrentAccountEntity.passportId))
            {
                m_CurrentSelectGroupId = groupId;
                m_CurrentSelectRoomId = roomId;
                m_CurrentSelectMemberId = playerId;
                ShowMessage("提示", string.Format("是否踢掉{0}{1}", player.nickname, room.roomStatus == RoomEntityBase.RoomStatus.Gaming ? ",该房间会解散" : ""), MessageViewType.OkAndCancel, RoomKickPlayer);
            }
            else
            {
                OpenChatRoomDetailView(group, room);
            }
        }
        else
        {
            OpenChatRoomDetailView(group, room);
        }
    }
    #endregion

    #region RoomKickPlayer 确认房间踢人
    /// <summary>
    /// 确认房间踢人
    /// </summary>
    private void RoomKickPlayer()
    {
        RoomEntityBase room = ChatGroupProxy.Instance.GetRoom(m_CurrentSelectGroupId, m_CurrentSelectRoomId);
        if (room == null) return;
        ClientSendRoomKick(m_CurrentSelectGroupId, m_CurrentSelectRoomId, m_CurrentSelectMemberId, room.gameId);
    }
    #endregion

    #region OnChatRoomDetailClick 聊天群房间详情按钮点击
    /// <summary>
    /// 聊天群房间详情按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnChatRoomDetailClick(object[] obj)
    {
        int groupId = (int)obj[0];
        int roomId = (int)obj[1];
        RoomEntityBase room = ChatGroupProxy.Instance.GetRoom(groupId, roomId);
        if (room == null) return;
        ChatGroupEntity group = ChatGroupProxy.Instance.GetGroup(groupId);
        if (group == null) return;
        OpenChatRoomDetailView(ChatGroupProxy.Instance.GetGroup(groupId), room);
    }
    #endregion

    #region OnChatRoomInviteClick 聊天群房间邀请按钮点击
    /// <summary>
    /// 聊天群房间邀请按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnChatRoomInviteClick(object[] obj)
    {
        int groupId = (int)obj[0];
        int roomId = (int)obj[1];
        RoomEntityBase room = ChatGroupProxy.Instance.GetRoom(groupId, roomId);
        if (room == null) return;
        int[] cfgId = new int[room.Config.Count];
        for (int i = 0; i < room.Config.Count; ++i)
        {
            cfgId[i] = room.Config[i].id;
        }
        ShareCtrl.Instance.InviteFriend(room.roomId, room.gameId, cfgId, groupId);
    }
    #endregion

    #region OnBtnChatRoomDetailJoin 聊天群房间详情界面加入按钮点击
    /// <summary>
    /// 聊天群房间详情界面加入按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnChatRoomDetailJoin(object[] obj)
    {
        int groupId = (int)obj[0];
        int roomId = (int)obj[1];
        RoomEntityBase room = ChatGroupProxy.Instance.GetRoom(groupId, roomId);
        if (room == null) return;
        GameCtrl.Instance.JoinRoom(room.gameId, room.roomId);
    }
    #endregion

    #region OnMessageScrollViewTop 消息滚动视图拉到最上
    /// <summary>
    /// 消息滚动视图拉到最上
    /// </summary>
    /// <param name="obj"></param>
    private void OnMessageScrollViewTop(object[] obj)
    {
        int groupId = (int)obj[0];
        ClientSendRequestChatMessageList(groupId);
    }
    #endregion

    #region OnExitClick 退群按钮点击
    /// <summary>
    /// 退群按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnExitClick(object[] obj)
    {
        int groupId = (int)obj[0];
        ChatGroupEntity group = ChatGroupProxy.Instance.GetGroup(groupId);
        if (group == null) return;
        m_CurrentSelectGroupId = groupId;
        ShowMessage("提示", "是否退出群" + group.name, MessageViewType.OkAndCancel, ExitGroup, null);
    }
    #endregion

    #region OnDisbandRoomClick 解散房间按钮点击
    /// <summary>
    /// 解散房间按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnDisbandRoomClick(object[] obj)
    {
        int groupId = (int)obj[0];
        int roomId = (int)obj[1];

        RoomEntityBase room = ChatGroupProxy.Instance.GetRoom(groupId, roomId);
        m_UIChatRoomDetailView.Close();
        if (room == null)
        {
            ShowTip("房间已解散");
            return;
        }

        m_CurrentSelectGroupId = groupId;
        m_CurrentSelectRoomId = roomId;
        ShowMessage("提示", "是否解散该房间", MessageViewType.OkAndCancel, DisbandRoom, null);

    }
    #endregion

    #region DisbandRoom 确认解散房间
    /// <summary>
    /// 确认解散房间
    /// </summary>
    private void DisbandRoom()
    {
        RoomEntityBase room = ChatGroupProxy.Instance.GetRoom(m_CurrentSelectGroupId, m_CurrentSelectRoomId);
        if (room == null) return;
        ClientSendDisbandRoom(m_CurrentSelectGroupId, m_CurrentSelectRoomId, room.gameId);
    }
    #endregion

    #region ExitGroup 确认退群
    /// <summary>
    /// 确认退群
    /// </summary>
    private void ExitGroup()
    {
        ClientSendExitGroup(m_CurrentSelectGroupId);
    }
    #endregion

    #region ConnectChatServer 连接聊天服务器
    /// <summary>
    /// 连接聊天服务器
    /// </summary>
    private void ConnectChatServer()
    {
        GameEntity entity = GameProxy.Instance.GetChatServer();
        if (entity == null) return;
        m_SocketHandle = NetWorkSocket.Instance.BeginConnect(entity.ipaddr, entity.port, OnConnectedCallBack);
    }
    #endregion

    #region OnConnectedCallBack 连接聊天服务器回调
    /// <summary>
    /// 连接聊天服务器回调
    /// </summary>
    /// <param name="isSuccess"></param>
    private void OnConnectedCallBack(bool isSuccess)
    {
        if (!isSuccess)
        {
            ShowMessage("提示", "俱乐部连接失败,请重新尝试", MessageViewType.OkAndCancel, ConnectChatServer, Close);
            return;
        }

        NetWorkSocket.Instance.GetSocket(m_SocketHandle).OnDisConnect = OnDisConnect;
        NetWorkSocket.Instance.GetSocket(m_SocketHandle).OnReconnect = OnReconnect;

        ClientSendRequestGroupList();
    }
    #endregion

    #region Close 关闭聊天窗口
    /// <summary>
    /// 关闭聊天窗口
    /// </summary>
    private void Close()
    {
        if (m_UIChatGroupView != null)
        {
            m_UIChatGroupView.Close();
        }
    }
    #endregion

    #endregion

    #region 客户端发送消息

    #region ClientSendCreateGroup 客户端发送创建群消息
    /// <summary>
    /// 客户端发送创建群消息
    /// </summary>
    private void ClientSendCreateGroup(string groupName, int headIndex)
    {
        OP_CLUB_CREATE_GET proto = new OP_CLUB_CREATE_GET();
        proto.name = groupName;
        proto.avatar = headIndex.ToString();

        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_CREATE_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendJoinGroup 客户端发送加入群消息
    /// <summary>
    /// 客户端发送加入群消息
    /// </summary>
    private void ClientSendJoinGroup(int groupId)
    {
        OP_CLUB_APPLY_GET proto = new OP_CLUB_APPLY_GET();
        proto.clubId = groupId;

        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_APPLY_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendExitGroup 客户端发送退出群消息
    /// <summary>
    /// 客户端发送退出群消息
    /// </summary>
    /// <param name="groupId"></param>
    private void ClientSendExitGroup(int groupId)
    {
        OP_CLUB_LEAVE_GET proto = new OP_CLUB_LEAVE_GET();
        proto.clubId = groupId;

        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_LEAVE_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendDisbandGroup 客户端发送解散群消息
    /// <summary>
    /// 客户端发送解散群消息
    /// </summary>
    private void ClientSendDisbandGroup(int groupId)
    {
        OP_CLUB_DISMISS_GET proto = new OP_CLUB_DISMISS_GET();
        proto.clubId = groupId;

        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_DISMISS_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendRequestGroupList 客户端发送获取群列表消息
    /// <summary>
    /// 客户端发送获取群列表消息
    /// </summary>
    private void ClientSendRequestGroupList()
    {
        NetWorkSocket.Instance.Send(null, OP_CLUB_LIST_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendRequestMemberList 客户端发送获取群成员列表消息
    /// <summary>
    /// 客户端发送获取群成员列表消息
    /// </summary>
    private void ClientSendRequestMemberList(int groupId)
    {
        OP_CLUB_PLAYER_LIST_GET proto = new OP_CLUB_PLAYER_LIST_GET();
        proto.clubId = groupId;
        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_PLAYER_LIST_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendRequestGroupInfo 客户端发送获取群信息消息
    /// <summary>
    /// 客户端发送获取群信息消息
    /// </summary>
    private void ClientSendRequestGroupInfo(int groupId)
    {
        OP_CLUB_GET proto = new OP_CLUB_GET();
        proto.clubId = groupId;
        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendRequestRoomList 客户端发送获取房间列表消息
    /// <summary>
    /// 客户端发送获取房间列表消息
    /// </summary>
    private void ClientSendRequestRoomList(int groupId)
    {
        OP_CLUB_ROOM_LIST_GET proto = new OP_CLUB_ROOM_LIST_GET();
        proto.clubId = groupId;
        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_ROOM_LIST_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendPleaseLeave 客户端发送踢人消息
    /// <summary>
    /// 客户端发送踢人消息
    /// </summary>
    private void ClientSendPleaseLeave(int groupId, int playerId)
    {
        OP_CLUB_KICK_GET proto = new OP_CLUB_KICK_GET();
        proto.clubId = groupId;
        proto.playerId = playerId;
        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_KICK_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientRequestApplyList 客户端发送请求申请列表消息
    /// <summary>
    /// 客户端发送请求申请列表消息
    /// </summary>
    /// <param name="groupId"></param>
    private void ClientRequestApplyList(int groupId)
    {
        OP_CLUB_APPLY_LIST_GET proto = new OP_CLUB_APPLY_LIST_GET();
        proto.clubId = groupId;
        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_APPLY_LIST_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendAgreeApply 客户端发送同意申请
    /// <summary>
    /// 客户端发送同意申请消息
    /// </summary>
    private void ClientSendAgreeApply(int clubId, int playerId)
    {
        OP_CLUB_AGREE_GET proto = new OP_CLUB_AGREE_GET();
        proto.clubId = clubId;
        proto.playerId = playerId;
        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_AGREE_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendRefuseApply 客户端发送拒绝申请消息
    /// <summary>
    /// 客户端发送拒绝申请消息
    /// </summary>
    /// <param name="clubId"></param>
    /// <param name="playerId"></param>
    private void ClientSendRefuseApply(int clubId, int playerId)
    {
        OP_CLUB_REJECT_GET proto = new OP_CLUB_REJECT_GET();
        proto.clubId = clubId;
        proto.playerId = playerId;
        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_REJECT_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendRoomKick 客户端发送房间踢人消息
    /// <summary>
    /// 客户端发送房间踢人消息
    /// </summary>
    private void ClientSendRoomKick(int groupId, int roomId, int playerId, int gameId)
    {
        OP_CLUB_ROOM_KICK_GET proto = new OP_CLUB_ROOM_KICK_GET();
        proto.clubId = groupId;
        proto.roomId = roomId;
        proto.playerId = playerId;
        proto.gameId = gameId;
        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_ROOM_KICK_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendChatMessage 客户端发送聊天消息
    /// <summary>
    /// 客户端发送聊天消息
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="message"></param>
    private void ClientSendChatMessage(int groupId, string message)
    {
        OP_CLUB_MSG_GET proto = new OP_CLUB_MSG_GET();
        proto.clubId = groupId;
        proto.content = message.ToBytes();
        proto.typeId = ENUM_PLAYER_MESSAGE.STRING;
        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_MSG_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendRequestChatMessageList 客户端发送请求聊天列表消息
    /// <summary>
    /// 客户端发送请求聊天列表消息
    /// </summary>
    /// <param name="groupId"></param>
    private void ClientSendRequestChatMessageList(int groupId)
    {
        Debug.Log("请求消息列表");
        OP_CLUB_MSG_LIST_GET proto = new OP_CLUB_MSG_LIST_GET();
        proto.clubId = groupId;
        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_MSG_LIST_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendRequestRecordList 客户端发送请求战绩列表
    /// <summary>
    /// 客户端发送请求战绩列表
    /// </summary>
    private void ClientSendRequestRecordList(int groupId)
    {
        OP_CLUB_RECORD_LIST_GET proto = new OP_CLUB_RECORD_LIST_GET();
        proto.clubId = groupId;

        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_RECORD_LIST_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendAppointManager 客户端发送任命管理员
    /// <summary>
    /// 客户端发送任命管理员
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="playerId"></param>
    private void ClientSendAppointManager(int groupId, int playerId)
    {
        OP_CLUB_SET_PLAYER_GET proto = new OP_CLUB_SET_PLAYER_GET();
        proto.clubId = groupId;
        proto.playerId = playerId;
        proto.identity = ENUM_PLAYER_IDENTITY.ADMIN;

        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_SET_PLAYER_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendDimissionManager 客户端发送卸任管理员
    /// <summary>
    /// 客户端发送卸任管理员
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="playerId"></param>
    private void ClientSendDimissionManager(int groupId, int playerId)
    {
        OP_CLUB_SET_PLAYER_GET proto = new OP_CLUB_SET_PLAYER_GET();
        proto.clubId = groupId;
        proto.playerId = playerId;
        proto.identity = ENUM_PLAYER_IDENTITY.MEMBER;

        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_SET_PLAYER_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendRechargeCards 客户端发送充值房卡
    /// <summary>
    /// 客户端发送充值房卡
    /// </summary>
    /// <param name="groupId"></param>
    private void ClientSendSetCards(int groupId, int cards)
    {
        OP_CLUB_SET_CARD_GET proto = new OP_CLUB_SET_CARD_GET();
        proto.clubId = groupId;
        proto.cards = cards;

        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_SET_CARD_GET.CODE, m_SocketHandle);
    }
    #endregion

    #region ClientSendDisbandRoom 客户端发送解散房间消息
    /// <summary>
    /// 客户端发送解散房间消息
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="roomId"></param>
    private void ClientSendDisbandRoom(int groupId, int roomId, int gameId)
    {
        OP_CLUB_ROOM_DISMISS_GET proto = new OP_CLUB_ROOM_DISMISS_GET();
        proto.clubId = groupId;
        proto.roomId = roomId;
        proto.gameId = gameId;

        NetWorkSocket.Instance.Send(proto.encode(), OP_CLUB_ROOM_DISMISS_GET.CODE, m_SocketHandle);
    }
    #endregion

    #endregion

    #region 服务器返回消息

    #region OnServerReturnCreateGroup 服务器返回创建群消息
    /// <summary>
    /// 服务器返回创建群消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnCreateGroup(byte[] obj)
    {
        OP_CLUB_CREATE proto = OP_CLUB_CREATE.decode(obj);
        if (proto.clubId > 0)
        {
            ShowTip("创建成功");
        }
    }
    #endregion

    #region OnServerReturnJoinGroup 服务器返回加入群消息
    /// <summary>
    /// 服务器返回加入群消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnJoinGroup(byte[] obj)
    {
        OP_CLUB_APPLY proto = OP_CLUB_APPLY.decode(obj);
    }
    #endregion

    #region OnServerBroadcastJoinGroup 服务器广播加入群消息
    /// <summary>
    /// 服务器广播加入群消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastJoinGroup(byte[] obj)
    {
        OP_CLUB_ENTER proto = OP_CLUB_ENTER.decode(obj);
        PlayerEntity entity = new PlayerEntity()
        {
            id = proto.playerId,
            nickname = proto.nickname,
            avatar = proto.avatar,
            gameId = proto.gameId,
            gender = proto.gender,
            ipaddr = proto.ipaddr,
            roomId = proto.roomId,
            online = proto.online,
            latitude = proto.latitude,
            longitude = proto.longitude,
            isManager = proto.identity == ENUM_PLAYER_IDENTITY.ADMIN || proto.identity == ENUM_PLAYER_IDENTITY.HOLDER
        };
        Debug.Log(proto.identity);
        ChatGroupProxy.Instance.AddMember(proto.clubId, entity, proto.playerId == AccountProxy.Instance.CurrentAccountEntity.passportId);
    }
    #endregion

    #region OnServerBroadcastDisbandGroup 服务器广播解散群消息
    /// <summary>
    /// 服务器广播解散群消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastDisbandGroup(byte[] obj)
    {
        OP_CLUB_DISMISS proto = OP_CLUB_DISMISS.decode(obj);
        ShowTip(proto.name + "已解散");
        ChatGroupProxy.Instance.RemoveGroup(proto.clubId);
    }
    #endregion

    #region OnServerBroadcastLeaveGroup 服务器广播离开群消息
    /// <summary>
    /// 服务器广播离开群消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastLeaveGroup(byte[] obj)
    {
        OP_CLUB_LEAVE proto = OP_CLUB_LEAVE.decode(obj);
        if (proto.playerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
        {
            ChatGroupProxy.Instance.RemoveGroup(proto.clubId);
        }
        else
        {
            ChatGroupProxy.Instance.RemoveMember(proto.clubId, proto.playerId);
        }
    }
    #endregion

    #region OnServerBroadcastnPleaseLeaveGroup 服务器广播踢人消息
    /// <summary>
    /// 服务器广播踢人消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastnPleaseLeaveGroup(byte[] obj)
    {
        OP_CLUB_KICK proto = OP_CLUB_KICK.decode(obj);
        if (proto.playerId == AccountProxy.Instance.CurrentAccountEntity.passportId)//被踢的是自己
        {
            ChatGroupEntity group = ChatGroupProxy.Instance.GetGroup(proto.clubId);
            if (group != null)
            {
                ShowTip("您被从" + group.name + "群移除");
                ChatGroupProxy.Instance.RemoveGroup(proto.clubId);
            }

        }
        else
        {
            ChatGroupProxy.Instance.RemoveMember(proto.clubId, proto.playerId);
        }
    }
    #endregion

    #region OnServerReturnGroupList 服务器返回群列表消息
    /// <summary>
    /// 服务器返回群列表消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnGroupList(byte[] obj)
    {
        OP_CLUB_LIST proto = OP_CLUB_LIST.decode(obj);

        ChatGroupProxy.Instance.ClearGroup();
        for (int i = 0; i < proto.clubCount(); ++i)
        {
            OP_CLUB op_club = proto.getClub(i);
            bool isOwner = false;
            if (op_club.ownerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
            {
                isOwner = true;
            }
            Debug.Log(op_club.playerCount);
            ChatGroupEntity group = new ChatGroupEntity(op_club.clubId, op_club.name, op_club.announce, op_club.ownerId, op_club.playerCount, op_club.playerTotal, op_club.avatar.ToInt(), isOwner, op_club.roomCount, op_club.cards,op_club.ownerAvatar);
            ChatGroupProxy.Instance.AddGroup(group);
        }
    }
    #endregion

    #region OnServerReturnMemberList 服务器返回成员列表消息
    /// <summary>
    /// 服务器返回成员列表消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnMemberList(byte[] obj)
    {
        OP_CLUB_PLAYER_LIST proto = OP_CLUB_PLAYER_LIST.decode(obj);
        ChatGroupProxy.Instance.ClearMember(proto.clubId);

        Debug.Log("群人数" + proto.playerCount().ToString());
        List<PlayerEntity> playerList = new List<PlayerEntity>();

        for (int i = 0; i < proto.playerCount(); ++i)
        {
            OP_CLUB_PLAYER op_player = proto.getPlayer(i);
            PlayerEntity player = new PlayerEntity()
            {
                id = op_player.playerId,
                nickname = op_player.nickname,
                gender = op_player.gender,
                avatar = op_player.avatar,
                ipaddr = op_player.ipaddr,
                gameId = op_player.gameId,
                roomId = op_player.roomId,
                online = op_player.online,
                latitude = op_player.latitude,
                longitude = op_player.longitude,
                isManager = op_player.identity == ENUM_PLAYER_IDENTITY.ADMIN || op_player.identity == ENUM_PLAYER_IDENTITY.HOLDER,
            };

            playerList.Add(player);
            if (player.id == AccountProxy.Instance.CurrentAccountEntity.passportId)
            {
                ChatGroupEntity groupEntity= ChatGroupProxy.Instance.GetGroup(proto.clubId);
                if (groupEntity != null)  groupEntity.playerEntity = player;
            }

        }

        for (int i = 0; i < playerList.Count; ++i)
        {
            ChatGroupProxy.Instance.AddMember(proto.clubId, playerList[i], playerList[i].id == AccountProxy.Instance.CurrentAccountEntity.passportId);
        }


    }
    #endregion

    #region OnServerReturnRoomList 服务器返回房间列表消息
    /// <summary>
    /// 服务器返回房间列表消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnRoomList(byte[] obj)
    {
        OP_CLUB_ROOM_LIST proto = OP_CLUB_ROOM_LIST.decode(obj);
        Debug.Log("房间数量" + proto.roomCount().ToString());
        ChatGroupProxy.Instance.ClearRoom(proto.clubId);
        for (int i = 0; i < proto.roomCount(); ++i)
        {
            OP_CLUB_ROOM op_room = proto.getRoom(i);
            Debug.Log("房间Id" + op_room.roomId);
            RoomEntityBase entity = new RoomEntityBase()
            {
                roomId = op_room.roomId,
                maxLoop = op_room.maxLoop,
                currentLoop = op_room.loop,
                roomStatus = (RoomEntityBase.RoomStatus)op_room.status,
                gameId = op_room.gameId
            };
            for (int j = 0; j < op_room.settingIdCount(); ++j)
            {
                cfg_settingEntity setting = cfg_settingDBModel.Instance.Get(op_room.getSettingId(j));
                if (setting != null)
                {
                    entity.Config.Add(setting);
                }
            }
            List<int> lstPlayerId = new List<int>();
            Debug.Log("房间人数" + op_room.playerCount());
            for (int j = 0; j < op_room.playerCount(); ++j)
            {
                OP_CLUB_PLAYER op_player = op_room.getPlayer(j);
                lstPlayerId.Add(op_player.playerId);
            }
            ChatGroupProxy.Instance.AddRoom(proto.clubId, entity, lstPlayerId);
        }
    }
    #endregion

    #region OnServerReturnApply 服务器返回申请进群消息
    /// <summary>
    /// 服务器返回申请进群消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnApply(byte[] obj)
    {
        OP_CLUB_APPLY proto = OP_CLUB_APPLY.decode(obj);
        Debug.LogWarning("有人申请进群");
        if (proto.isSucceed)
        {
            ChatGroupEntity group = ChatGroupProxy.Instance.GetGroup(proto.clubId);
            if (group != null)
            {
                ChatGroupProxy.Instance.SetNewApply(proto.clubId, true);
            }
            else
            {
                ShowTip("申请成功");
            }
        }
    }
    #endregion

    #region OnServerReturnApplyList 服务器返回申请列表
    /// <summary>
    /// 服务器返回申请列表
    /// </summary>
    /// <param name="data"></param>
    private void OnServerReturnApplyList(byte[] data)
    {
        OP_CLUB_APPLY_LIST proto = OP_CLUB_APPLY_LIST.decode(data);

        ChatGroupProxy.Instance.ClearApply(proto.clubId);

        Debug.Log("申请列表长度" + proto.playerCount().ToString());
        for (int i = 0; i < proto.playerCount(); ++i)
        {
            OP_CLUB_PLAYER op_player = proto.getPlayer(i);
            PlayerEntity player = new PlayerEntity();
            player.id = op_player.playerId;
            player.avatar = op_player.avatar;
            player.gender = op_player.gender;
            player.online = op_player.online;
            player.nickname = op_player.nickname;

            ChatGroupProxy.Instance.AddApply(proto.clubId, player);
        }
    }
    #endregion

    #region OnServerReturnAgreeApply 服务器返回同意申请消息
    /// <summary>
    /// 服务器返回同意申请消息
    /// </summary>
    private void OnServerReturnAgreeApply(byte[] bytes)
    {
        OP_CLUB_AGREE proto = OP_CLUB_AGREE.decode(bytes);
        if (!proto.isSucceed) return;

        if (proto.playerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
        {
            ClientSendRequestGroupInfo(proto.clubId);
            ShowTip("进群成功");
        }
        else
        {
            Debug.Log(proto.clubId);
            Debug.Log(proto.playerId);
            ChatGroupProxy.Instance.RemoveApply(proto.clubId, proto.playerId);

            ShowTip("添加成功");
        }

    }
    #endregion

    #region OnServerReturnRefuseApply 服务器返回拒绝申请消息
    /// <summary>
    /// 服务器返回拒绝申请消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnRefuseApply(byte[] obj)
    {
        OP_CLUB_REJECT proto = OP_CLUB_REJECT.decode(obj);
        if (!proto.isSucceed) return;
        Debug.Log(proto.clubId);
        Debug.Log(proto.playerId);

        if (proto.playerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
        {
            ShowTip("申请被拒");
        }
        else
        {
            ChatGroupProxy.Instance.RemoveApply(proto.clubId, proto.playerId);
            ShowTip("拒绝成功");
        }
    }
    #endregion

    #region OnServerReturnRoomInfo 服务器返回房间信息消息
    /// <summary>
    /// 服务器返回房间信息消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnRoomInfo(byte[] obj)
    {
        OP_CLUB_ROOM proto = OP_CLUB_ROOM.decode(obj);
        RoomEntityBase room = new RoomEntityBase()
        {
            roomId = proto.roomId,
            gameId = proto.gameId,
            currentLoop = proto.loop,
            maxLoop = proto.maxLoop,
            roomStatus = (RoomEntityBase.RoomStatus)proto.status,
        };
        Debug.Log(proto.settingIdCount());
        for (int j = 0; j < proto.settingIdCount(); ++j)
        {
            cfg_settingEntity setting = cfg_settingDBModel.Instance.Get(proto.getSettingId(j));
            if (setting != null)
            {
                room.Config.Add(setting);
            }
        }
        List<int> playerIds = new List<int>();
        for (int i = 0; i < proto.playerCount(); ++i)
        {
            playerIds.Add(proto.getPlayer(i).playerId);
        }

        ChatGroupProxy.Instance.AddRoom(proto.clubId, room, playerIds);
    }
    #endregion

    #region OnServerReturnGroupInfo 服务器返回群信息消息
    /// <summary>
    /// 服务器返回群信息消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnGroupInfo(byte[] obj)
    {
        OP_CLUB proto = OP_CLUB.decode(obj);
        Debug.Log(proto.clubId);
        bool isOwner = false;
        if (proto.ownerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
        {
            isOwner = true;
        }

        ChatGroupEntity entity = new ChatGroupEntity(proto.clubId, proto.name, proto.announce, proto.ownerId, proto.playerCount, proto.playerTotal, proto.avatar.ToInt(), isOwner, proto.roomCount, proto.cards,proto.ownerAvatar);
        ChatGroupProxy.Instance.AddGroup(entity);
    }
    #endregion

    #region OnServerBroadcastCreateRoom 服务器广播创建房间消息
    /// <summary>
    /// 服务器广播创建房间消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastCreateRoom(byte[] obj)
    {
        OP_CLUB_ROOM_CREATE proto = OP_CLUB_ROOM_CREATE.decode(obj);

        RoomEntityBase room = new RoomEntityBase()
        {
            roomId = proto.room.roomId,
            roomStatus = (RoomEntityBase.RoomStatus)proto.room.status,
            currentLoop = proto.room.loop,
            gameId = proto.room.gameId,
            maxLoop = proto.room.maxLoop,
        };
        for (int j = 0; j < proto.room.settingIdCount(); ++j)
        {
            cfg_settingEntity setting = cfg_settingDBModel.Instance.Get(proto.room.getSettingId(j));
            if (setting != null)
            {
                room.Config.Add(setting);
            }
        }
        List<int> players = new List<int>();
        Debug.Log("房间人数" + proto.room.playerCount());
        for (int i = 0; i < proto.room.playerCount(); ++i)
        {
            Debug.Log("玩家id" + proto.room.getPlayer(i).playerId);
            players.Add(proto.room.getPlayer(i).playerId);
        }

        ChatGroupProxy.Instance.AddRoom(proto.clubId, room, players);
    }
    #endregion

    #region OnServerBroadcastEnterRoom 服务器广播进入房间
    /// <summary>
    /// 服务器广播进入房间
    /// </summary>
    private void OnServerBroadcastEnterRoom(byte[] obj)
    {
        OP_CLUB_ROOM_ENTER proto = OP_CLUB_ROOM_ENTER.decode(obj);
        int groupId = proto.clubId;
        int roomId = proto.roomId;
        int playerId = proto.playerId;
        ChatGroupProxy.Instance.EnterRoom(groupId, roomId, playerId);
    }
    #endregion

    #region OnServerBroadcastLeaveRoom 服务器广播离开房间
    /// <summary>
    /// 服务器广播离开房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastLeaveRoom(byte[] obj)
    {
        OP_CLUB_ROOM_LEAVE proto = OP_CLUB_ROOM_LEAVE.decode(obj);
        int groupId = proto.clubId;
        int roomId = proto.roomId;
        int playerId = proto.playerId;
        ChatGroupProxy.Instance.LeaveRoom(groupId, roomId, playerId);
    }
    #endregion

    #region OnServerBroadcastDisbandRoom 服务器广播解散房间
    /// <summary>
    /// 服务器广播解散房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastDisbandRoom(byte[] obj)
    {
        OP_CLUB_ROOM_DISMISS proto = OP_CLUB_ROOM_DISMISS.decode(obj);
        int groupId = proto.clubId;
        int roomId = proto.roomId;
        ChatGroupEntity group = ChatGroupProxy.Instance.GetGroup(groupId);
        if (group == null) return;
        ChatGroupProxy.Instance.RemoveRoom(groupId, roomId);
    }
    #endregion

    #region OnServerBroadcastRoomKick 服务器广播房间踢人
    /// <summary>
    /// 服务器广播房间踢人
    /// </summary>
    private void OnServerBroadcastRoomKick(byte[] bytes)
    {
        OP_CLUB_ROOM_KICK proto = OP_CLUB_ROOM_KICK.decode(bytes);

        if (proto.playerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
        {
            ShowTip("您被踢出房间");
        }
        else
        {
            ShowTip("踢人成功");
        }
    }
    #endregion

    #region OnServerBroadcastRoomStatus 服务器广播房间状态
    /// <summary>
    /// 服务器广播房间状态
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastRoomStatus(byte[] obj)
    {
        OP_CLUB_ROOM_STATUS proto = OP_CLUB_ROOM_STATUS.decode(obj);


        ChatGroupProxy.Instance.UpdateRoom(proto.clubId, proto.roomId, proto.loop, (RoomEntityBase.RoomStatus)proto.status);
    }
    #endregion

    #region OnServerBroadcastPlayerStatus 服务器广播玩家状态
    /// <summary>
    /// 服务器广播玩家状态
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastPlayerStatus(byte[] obj)
    {
        OP_CLUB_PLAYER_STATUS proto = OP_CLUB_PLAYER_STATUS.decode(obj);
        Debug.Log(proto.playerId.ToString() + (proto.online > 0 ? "上线" : "下线"));
        ChatGroupProxy.Instance.SetMemberStatus(proto.clubId, proto.playerId, proto.online);
    }
    #endregion

    #region OnServerReturnMessage 服务器返回聊天消息
    /// <summary>
    /// 服务器返回聊天消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnMessage(byte[] obj)
    {
        OP_CLUB_MSG proto = OP_CLUB_MSG.decode(obj);
        ChatGroupProxy.Instance.AddMessage(proto.clubId, proto.playerId, new MessageEntity(proto.content, (ChatType)proto.typeId, proto.playerId == AccountProxy.Instance.CurrentAccountEntity.passportId), true);
    }
    #endregion

    #region OnServerReturnMessageList 服务器返回聊天列表消息
    /// <summary>
    /// 服务器返回聊天列表消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnMessageList(byte[] obj)
    {
        OP_CLUB_MSG_LIST proto = OP_CLUB_MSG_LIST.decode(obj);
        Debug.Log("消息数量" + proto.msgCount().ToString());
        for (int i = 0; i < proto.msgCount(); ++i)
        {
            OP_CLUB_MSG op_msg = proto.getMsg(i);
            ChatGroupProxy.Instance.AddMessage(op_msg.clubId, op_msg.playerId, new MessageEntity(op_msg.content, (ChatType)op_msg.typeId, op_msg.playerId == AccountProxy.Instance.CurrentAccountEntity.passportId), false);
        }
    }
    #endregion

    #region OnServerReturnRecord 服务器返回战绩
    /// <summary>
    /// 服务器返回战绩
    /// </summary>
    /// <param name="bytes"></param>
    private void OnServerReturnRecord(byte[] bytes)
    {
        OP_CLUB_RECORD proto = OP_CLUB_RECORD.decode(bytes);


        RecordEntity recordEntity = new RecordEntity();
        recordEntity.gameId = proto.gameId;
        recordEntity.gameName = cfg_gameDBModel.Instance.Get(proto.gameId).GameName;
        recordEntity.roomId = proto.roomId;
        recordEntity.time = TimeUtil.GetCSharpTime(proto.beginTime * 1000).ToString("yyyy-MM-dd HH:mm:ss");
        Debug.Log("战绩时间戳：" + proto.beginTime.ToString());
        Debug.Log("战绩时间：" + recordEntity.time);
        recordEntity.players = new List<RecordPlayerEntity>();
        for (int j = 0; j < proto.playerCount(); ++j)
        {
            RecordPlayerEntity playerEntity = new RecordPlayerEntity();
            OP_CLUB_RECORD_PLAYER op_player = proto.getPlayer(j);
            playerEntity.id = op_player.playerId;
            playerEntity.nickname = op_player.nickname;
            playerEntity.gold = op_player.gold;
            playerEntity.avatar = op_player.avatar;
            recordEntity.players.Add(playerEntity);
        }
        recordEntity.players.Sort();

        ChatGroupProxy.Instance.AddRecord(proto.clubId, recordEntity);
    }
    #endregion

    #region OnServerReturnRecordList 服务器返回战绩列表
    /// <summary>
    /// 服务器返回战绩列表
    /// </summary>
    /// <param name="bytes"></param>
    private void OnServerReturnRecordList(byte[] bytes)
    {
        OP_CLUB_RECORD_LIST proto = OP_CLUB_RECORD_LIST.decode(bytes);
        Debug.Log("战绩数量:" + proto.recordCount().ToString());
        for (int i = proto.recordCount() - 1; i >= 0; --i)
        {
            OP_CLUB_RECORD op_record = proto.getRecord(i);
            RecordEntity recordEntity = new RecordEntity();
            recordEntity.gameId = op_record.gameId;
            recordEntity.gameName = cfg_gameDBModel.Instance.Get(op_record.gameId).GameName;
            recordEntity.roomId = op_record.roomId;
            recordEntity.time = TimeUtil.GetCSharpTime(op_record.beginTime * 1000).ToString("yyyy-MM-dd HH:mm:ss");
            Debug.Log("战绩时间戳：" + op_record.beginTime.ToString());
            Debug.Log("战绩时间：" + recordEntity.time);
            recordEntity.players = new List<RecordPlayerEntity>();
            Debug.Log("玩家数量:" + op_record.playerCount());
            for (int j = 0; j < op_record.playerCount(); ++j)
            {
                OP_CLUB_RECORD_PLAYER op_player = op_record.getPlayer(j);
                RecordPlayerEntity playerEntity = new RecordPlayerEntity();
                playerEntity.id = op_player.playerId;
                playerEntity.nickname = op_player.nickname;
                playerEntity.gold = op_player.gold;
                playerEntity.avatar = op_player.avatar;
                recordEntity.players.Add(playerEntity);
                if (i == 2)
                {
                    Debug.Log(playerEntity.gold);
                }
            }
            recordEntity.players.Sort();

            ChatGroupProxy.Instance.AddRecord(proto.clubId, recordEntity);
        }
    }
    #endregion

    #region OnServerBroadcastIdentityChanged 服务器广播身份变更
    /// <summary>
    /// 服务器广播身份变更
    /// </summary>
    /// <param name="bytes"></param>
    private void OnServerBroadcastIdentityChanged(byte[] bytes)
    {
        OP_CLUB_SET_PLAYER proto = OP_CLUB_SET_PLAYER.decode(bytes);
        Debug.Log(proto.playerId + "身份变更为" + proto.identity.ToString());
        ChatGroupProxy.Instance.SetMemberIdentity
            (
            proto.clubId,
            proto.playerId,
            proto.identity == ENUM_PLAYER_IDENTITY.ADMIN,
            proto.playerId == AccountProxy.Instance.CurrentAccountEntity.passportId
            );
    }
    #endregion

    #region OnServerBroadcastSetCards 服务器广播设置房卡
    /// <summary>
    /// 服务器广播设置房卡
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastSetCards(byte[] obj)
    {
        OP_CLUB_SET_CARD proto = OP_CLUB_SET_CARD.decode(obj);
        ChatGroupProxy.Instance.SetGroupCards(proto.clubId, proto.cards);
        if (proto.playerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
        {
            AccountProxy.Instance.SetCards(AccountProxy.Instance.CurrentAccountEntity.cards - proto.cards);
        }
    }
    #endregion

    #region OnServerBroadcastClearRoom 服务器广播清空房间
    /// <summary>
    /// 服务器广播清空房间
    /// </summary>
    /// <param name="bytes"></param>
    private void OnServerBroadcastClearRoom(byte[] bytes)
    {
        OP_CLUB_ROOM_CLEAR proto = OP_CLUB_ROOM_CLEAR.decode(bytes);
        Debug.Log("清空所有游戏ID为" + proto.gameId.ToString() + "的房间");
        ChatGroupProxy.Instance.RemoveRoom(proto.gameId);
    }
    #endregion

    #endregion

    #region OnReconnect 重新连接
    /// <summary>
    /// 重新连接
    /// </summary>
    private void OnReconnect()
    {
        ClientSendRequestGroupList();
    }
    #endregion

    #region OnDisConnect 断开连接
    /// <summary>
    /// 断开连接
    /// </summary>
    /// <param name="isActive"></param>
    private void OnDisConnect(bool isActive)
    {
        if (!isActive)
        {
            ShowMessage("提示","俱乐部连接断开",MessageViewType.OkAndCancel,ConnectChatServer, Close);
        }
    }
    #endregion
}