//===================================================
//Author      : DRB
//CreateTime  ：9/26/2017 3:10:14 PM
//Description ：聊天群数据代理类
//===================================================
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 聊天群数据代理类
/// </summary>
public class ChatGroupProxy : ProxyBase<ChatGroupProxy>
{
    #region Const
    public const string ON_GROUP_INFO_CHANGED = "OnGroupInfoChanged";
    public const string ON_ROOM_INFO_CHANGED = "OnGroupRoomInfoChanged";
    public const string ON_PLAYER_INFO_CHANGED = "OnPlayerInfoChanged";
    public const string ON_ADD_GROUP = "OnChatGroupAddGroup";
    public const string ON_REMOVE_GROUP = "OnChatGroupRemoveGroup";
    public const string ON_ADD_MEMBER = "OnChatGroupAddMember";
    public const string ON_REMOVE_MEMBER = "OnChatGroupRemoveMember";
    public const string ON_ADD_ROOM = "OnChatGroupAddRoom";
    public const string ON_REMOVE_ROOM = "OnChatGroupRemoveRoom";
    public const string ON_ENTER_ROOM = "OnChatGroupEnterRoom";
    public const string ON_LEAVE_ROOM = "OnChatGroupLeaveRoom";
    public const string ON_GROUP_NEW_APPLY = "OnChatGroupNewApply";
    public const string ON_GROUP_ADD_APPLY = "OnChatGroupAddApply";
    public const string ON_GROUP_REMOVE_APPLY = "OnChatGroupRemoveApply";
    public const string ON_GROUP_REFRESH_APPLY = "OnChatGroupRefreshApply";
    public const string ON_ADD_MESSAGE = "OnChatGroupAddMessage";
    public const string ON_ADD_RECORD = "OnChatGroupAddRecord";
    public const string ON_APPOINT_MANAGER = "OnChatGroupAppointManager";
    public const string ON_DIMISSION_MANAGER = "OnChatGroupDimissionManager";
    public const string ON_REMOVE_RECORD = "OnChatGroupRemoveRecord";
    #endregion

    /// <summary>
    /// 所有群
    /// </summary>
    public List<ChatGroupEntity> AllGroup = new List<ChatGroupEntity>();

    #region GetGroup 获取群数据实体
    /// <summary>
    /// 获取群数据实体
    /// </summary>
    /// <param name="id">群Id</param>
    /// <returns>群数据实体</returns>
    public ChatGroupEntity GetGroup(int id)
    {
        for (int i = 0; i < AllGroup.Count; ++i)
        {
            if (AllGroup[i].id == id)
            {
                return AllGroup[i];
            }
        }
        return null;
    }
    #endregion

    #region GetMemberList 获取群成员列表
    /// <summary>
    /// 获取群成员列表
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public List<PlayerEntity> GetMemberList(int groupId)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return null;
        return group.members;
    }
    #endregion

    #region GetMember 获取成员
    /// <summary>
    /// 获取成员
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="playerId"></param>
    /// <returns></returns>
    public PlayerEntity GetMember(int groupId, int playerId)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return null;
        for (int i = 0; i < group.members.Count; ++i)
        {
            if (group.members[i].id == playerId)
            {
                return group.members[i];
            }
        }
        return null;
    }
    #endregion

    #region GetRoom 获取房间
    /// <summary>
    /// 获取房间
    /// </summary>
    /// <param name="groupId">群Id</param>
    /// <param name="roomId">房间Id</param>
    /// <returns></returns>
    public RoomEntityBase GetRoom(int groupId, int roomId)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return null;
        for (int i = 0; i < group.rooms.Count; ++i)
        {
            if (group.rooms[i].roomId == roomId)
            {
                return group.rooms[i];
            }
        }
        return null;
    }
    #endregion

    #region SetGroupCards 设置群房卡数量
    /// <summary>
    /// 设置群房卡数量
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="cards"></param>
    public void SetGroupCards(int groupId, int cards)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;
        group.cards += cards;

        SendGroupInfoChangedNotification(group);
    }
    #endregion

    #region CheckOwner 检测是否是群主
    /// <summary>
    /// 检测是否是群主
    /// </summary>
    /// <param name="groupId">群Id</param>
    /// <param name="playerId">玩家Id</param>
    /// <returns></returns>
    public bool CheckOwner(int groupId, int playerId)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return false;
        return group.ownerId == playerId;
    }
    #endregion

    #region AddGroup 添加群
    /// <summary>
    /// 添加群
    /// </summary>
    /// <param name="entity">群数据实体</param>
    public void AddGroup(ChatGroupEntity entity)
    {
        if (GetGroup(entity.id) != null)//有这个群的信息了就不加了
        {
            AppDebug.LogWarning("这个群存在了");
            return;
        }
        AllGroup.Add(entity);

        TransferData data = new TransferData();
        data.SetValue("ChatGroup", entity);
        SendNotification(ON_ADD_GROUP, data);
    }
    #endregion

    #region RemoveGroup 移除群
    /// <summary>
    /// 移除群
    /// </summary>
    /// <param name="groupId">群Id</param>
    public void RemoveGroup(int groupId)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;
        AllGroup.Remove(group);

        TransferData data = new TransferData();
        data.SetValue("ChatGroup", group);
        SendNotification(ON_REMOVE_GROUP, data);
    }
    #endregion

    #region ClearGroup 清空群
    /// <summary>
    /// 清空群
    /// </summary>
    public void ClearGroup()
    {
        while (AllGroup.Count > 0)
        {
            RemoveGroup(AllGroup[0].id);
        }
    }
    #endregion

    #region ClearMember 清空群成员
    /// <summary>
    /// 清空群成员
    /// </summary>
    /// <param name="groupId">群Id</param>
    public void ClearMember(int groupId)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;

        while (group.members.Count > 0)
        {
            RemoveMember(groupId, group.members[0].id);
        }
    }
    #endregion

    #region SetMemberStatus 设置成员状态
    /// <summary>
    /// 设置成员状态
    /// </summary>
    public void SetMemberStatus(int groupId, int playerId, long onLine)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;
        PlayerEntity player = GetMember(groupId, playerId);
        if (player == null) return;
        player.online = onLine;

        //group.members.Sort();
        TransferData data = new TransferData();
        data.SetValue("GroupEntity", group);
        data.SetValue("PlayerEntity", player);
        data.SetValue("OneselfEntity", group.playerEntity);
        data.SetValue("Index", group.members.IndexOf(player));
        SendNotification(ON_PLAYER_INFO_CHANGED, data);
    }
    #endregion

    #region SetMemberIdentity 设置成员身份
    /// <summary>
    /// 设置成员身份
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="playerId"></param>
    /// <param name="isManager"></param>
    public void SetMemberIdentity(int groupId, int playerId, bool isManager, bool isPlayer)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;
        PlayerEntity player = GetMember(groupId, playerId);
        if (player == null) return;
        player.isManager = isManager;

        group.members.Sort();
        TransferData data = new TransferData();
        data.SetValue("GroupEntity", group);
        data.SetValue("PlayerEntity", player);
        data.SetValue("OneselfEntity", group.playerEntity);
        data.SetValue("IsPlayer", isPlayer);
        data.SetValue("Index", group.members.IndexOf(player));
        SendNotification(ON_PLAYER_INFO_CHANGED, data);
    }
    #endregion

    #region AddMember 添加群成员
    /// <summary>
    /// 添加群成员
    /// </summary>
    /// <param name="groupId">群Id</param>
    /// <param name="entity">玩家数据实体</param>
    public void AddMember(int groupId, PlayerEntity entity, bool isPlayer)
    {
        if (entity == null) return;
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;
        if (GetMember(groupId, entity.id) != null)
        {
            AppDebug.LogWarning("这个玩家存在了");
            return;
        }
        AppDebug.Log("玩家" + entity.nickname + "加入群" + group.name);
        if (group.ownerId == entity.id)
        {
            entity.isOwner = true;
        }
        if (!group.isRequested)
        {
            ++group.currMemberCount;
        }
        else
        {
            group.members.Add(entity);
            group.currMemberCount = group.members.Count;
        }

        group.members.Sort();

        if (isPlayer)
        {
            group.playerEntity = entity;
        }

        TransferData data = new TransferData();
        data.SetValue("GroupEntity", group);
        data.SetValue("PlayerEntity", entity);
        data.SetValue("IsPlayer", isPlayer);
        data.SetValue("Index", group.members.IndexOf(entity));
        data.SetValue("OneselfEntity", group.playerEntity);
        SendNotification(ON_ADD_MEMBER, data);

        SendGroupInfoChangedNotification(group);
    }
    #endregion

    #region RemoveMember 移除群成员
    /// <summary>
    /// 移除群成员
    /// </summary>
    /// <param name="groupId">群Id</param>
    /// <param name="playerId">玩家Id</param>
    public void RemoveMember(int groupId, int playerId)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;

        if (!group.isRequested)
        {
            --group.currMemberCount;
        }
        else
        {
            bool isExists = false;
            for (int i = 0; i < group.members.Count; ++i)
            {
                if (group.members[i].id == playerId)
                {
                    isExists = true;
                    group.members.RemoveAt(i);
                    group.currMemberCount = group.members.Count;
                    break;
                }
            }
            if (!isExists)
            {
                AppDebug.LogWarning("要移除的玩家不存在");
                return;
            }

            TransferData data = new TransferData();
            data.SetValue("GroupId", groupId);
            data.SetValue("PlayerCount", group.currMemberCount);
            data.SetValue("PlayerId", playerId);
            SendNotification(ON_REMOVE_MEMBER, data);
        }





        SendGroupInfoChangedNotification(group);
    }
    #endregion

    #region ClearRoom 清空房间
    /// <summary>
    /// 清空房间
    /// </summary>
    /// <param name="groupId">群Id</param>
    public void ClearRoom(int groupId)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;
        while (group.rooms.Count > 0)
        {
            RemoveRoom(group.id, group.rooms[0].roomId);
        }
    }
    #endregion

    #region UpdateRoom 更新房间信息
    /// <summary>
    /// 更新房间信息
    /// </summary>
    public void UpdateRoom(int groupId, int roomId, int loop, RoomEntityBase.RoomStatus status)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;
        RoomEntityBase room = GetRoom(groupId, roomId);
        if (room == null) return;
        room.currentLoop = loop;
        room.roomStatus = status;

        TransferData data = new TransferData();
        data.SetValue("GroupEntity", group);
        data.SetValue("RoomEntity", room);
        SendNotification(ON_ROOM_INFO_CHANGED, data);
        SendGroupInfoChangedNotification(group);
    }
    #endregion

    #region AddRoom 添加房间
    /// <summary>
    /// 添加房间
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="entity"></param>
    public void AddRoom(int groupId, RoomEntityBase entity, List<int> lstPlayerId)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;
        if (entity == null) return;
        if (GetRoom(groupId, entity.gameId) != null)
        {
            AppDebug.LogWarning("房间存在了");
            return;
        }
        AppDebug.Log("有一个新的房间");
        if (lstPlayerId != null)
        {
            for (int i = 0; i < lstPlayerId.Count; ++i)
            {
                bool isExists = false;
                for (int j = 0; j < group.members.Count; ++j)
                {
                    if (lstPlayerId[i] == group.members[j].id)
                    {
                        entity.players.Add(group.members[j]);
                        isExists = true;
                        break;
                    }
                }
                if (!isExists)
                {
                    entity.players.Add(null);
                }
            }
        }
        group.rooms.Add(entity);
        group.roomCount = group.rooms.Count;

        TransferData data = new TransferData();
        data.SetValue("GroupEntity", group);
        data.SetValue("RoomEntity", entity);
        SendNotification(ON_ADD_ROOM, data);

        SendGroupInfoChangedNotification(group);
    }
    #endregion

    #region RemoveRoom 移除房间
    /// <summary>
    /// 移除房间
    /// </summary>
    /// <param name="groupId">群Id</param>
    /// <param name="roomId">房间Id</param>
    public void RemoveRoom(int groupId, int roomId)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;
        for (int i = 0; i < group.rooms.Count; ++i)
        {
            if (group.rooms[i].roomId == roomId)
            {
                group.rooms.RemoveAt(i);
                group.roomCount = group.rooms.Count;
                break;
            }
        }

        TransferData data = new TransferData();
        data.SetValue("GroupId", groupId);
        data.SetValue("RoomId", roomId);
        SendNotification(ON_REMOVE_ROOM, data);

        SendGroupInfoChangedNotification(group);
    }
    #endregion

    #region RemoveRoom 移除房间
    /// <summary>
    /// 移除房间
    /// </summary>
    /// <param name="gameId"></param>
    public void RemoveRoom(int gameId)
    {
        for (int i = 0; i < AllGroup.Count; ++i)
        {
            for (int j = AllGroup[i].rooms.Count - 1; j >= 0; --j)
            {
                if (AllGroup[i].rooms[j].gameId == gameId)
                {
                    RemoveRoom(AllGroup[i].id, AllGroup[i].rooms[j].roomId);
                }
            }
        }
    }
    #endregion

    #region EnterRoom 进入房间
    /// <summary>
    /// 进入房间
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="roomId"></param>
    /// <param name="playerId"></param>
    public void EnterRoom(int groupId, int roomId, int playerId)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;
        RoomEntityBase room = GetRoom(groupId, roomId);
        if (room == null) return;
        PlayerEntity player = GetMember(groupId, playerId);
        if (player == null) return;
        Debug.Log("玩家" + player.nickname + "进入房间" + room.roomId);
        for (int i = 0; i < room.players.Count; ++i)
        {
            if (room.players[i] == null)
            {
                room.players[i] = player;
                break;
            }
        }

        TransferData data = new TransferData();
        data.SetValue("GroupEntity", group);
        data.SetValue("RoomId", roomId);
        data.SetValue("PlayerEntity", player);
        SendNotification(ON_ENTER_ROOM, data);
    }
    #endregion

    #region LeaveRoom 离开房间
    /// <summary>
    /// 离开房间
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="roomId"></param>
    /// <param name="playerId"></param>
    public void LeaveRoom(int groupId, int roomId, int playerId)
    {
        RoomEntityBase room = GetRoom(groupId, roomId);
        if (room == null) return;
        PlayerEntity player = GetMember(groupId, playerId);
        if (player == null) return;
        Debug.Log("玩家" + playerId.ToString() + "离开房间" + roomId.ToString());
        for (int i = 0; i < room.players.Count; ++i)
        {
            if (room.players[i] == null) continue;

            if (room.players[i].id == playerId)
            {
                room.players[i] = null;
                break;
            }
        }

        TransferData data = new TransferData();
        data.SetValue("GroupId", groupId);
        data.SetValue("RoomId", roomId);
        data.SetValue("PlayerId", playerId);
        SendNotification(ON_LEAVE_ROOM, data);
    }
    #endregion

    #region ApplyGroup 申请加入群
    /// <summary>
    /// 申请加入群
    /// </summary>
    public void SetNewApply(int groupId, bool hasNewApply)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;

        group.hasNewApply = hasNewApply;

        TransferData data = new TransferData();
        data.SetValue("GroupId", groupId);
        data.SetValue("HasNewApply", group.hasNewApply);
        SendNotification(ON_GROUP_NEW_APPLY, data);
    }
    #endregion

    #region ClearApply 清空申请人
    /// <summary>
    /// 清空申请人
    /// </summary>
    public void ClearApply(int groupId)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;
        while (group.ApplyList.Count > 0)
        {
            RemoveApply(groupId, group.ApplyList[0].id);
        }
    }
    #endregion

    #region AddApply 添加申请人
    /// <summary>
    /// 添加申请人
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="entity"></param>
    public void AddApply(int groupId, PlayerEntity entity)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;
        group.hasNewApply = false;
        for (int i = 0; i < group.ApplyList.Count; ++i)
        {
            if (group.ApplyList[i].id == entity.id) return;
        }
        group.ApplyList.Add(entity);

        TransferData data = new TransferData();
        data.SetValue("GroupId", groupId);
        data.SetValue("PlayerEntity", entity);
        SendNotification(ON_GROUP_ADD_APPLY, data);
    }
    #endregion

    #region RemoveApply 移除申请
    /// <summary>
    /// 移除申请
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="playerId"></param>
    public void RemoveApply(int groupId, int playerId)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;

        for (int i = 0; i < group.ApplyList.Count; ++i)
        {
            if (group.ApplyList[i].id == playerId)
            {
                group.ApplyList.RemoveAt(i);
                break;
            }
        }
        group.hasNewApply = group.ApplyList.Count > 0;
        TransferData data = new TransferData();
        data.SetValue("GroupId", groupId);
        data.SetValue("PlayerId", playerId);
        data.SetValue("HasNewApply", group.hasNewApply);
        SendNotification(ON_GROUP_REMOVE_APPLY, data);
    }
    #endregion

    #region RefreshApply 刷新申请
    /// <summary>
    /// 刷新申请
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="playerId"></param>
    public void RefreshApply(int groupId)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;
        Debug.Log("---------group.ApplyList-----------------" + group.ApplyList.Count);
        TransferData data = new TransferData();
        data.SetValue("GroupId", groupId);
        data.SetValue("ApplyList", group.ApplyList);
        SendNotification(ON_GROUP_REFRESH_APPLY, data);
    }
    #endregion

    #region AddMessage 添加聊天消息
    /// <summary>
    /// 添加聊天消息
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="playerId"></param>
    /// <param name="message"></param>
    public void AddMessage(int groupId, int playerId, MessageEntity message, bool isLast)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;
        PlayerEntity player = GetMember(groupId, playerId);
        if (player == null) return;

        if (!group.isRequested) return;

        message.sendPlayer = player;
        if (isLast)
        {
            group.messageList.Add(message);
        }
        else
        {
            group.messageList.Insert(0, message);
        }

        ++group.unreadCount;

        TransferData data = new TransferData();
        data.SetValue("GroupEntity", group);
        data.SetValue("Message", message);
        data.SetValue("Index", isLast ? group.messageList.Count - 1 : 0);

        SendNotification(ON_ADD_MESSAGE, data);
    }
    #endregion

    #region AddRecord 添加战绩
    /// <summary>
    /// 添加战绩
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="record"></param>
    public void AddRecord(int groupId, RecordEntity record)
    {
        if (record == null) return;
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;

        if (!group.isRequested) return;

        for (int i = 0; i < group.recordList.Count; ++i)
        {
            if (group.recordList[i].battleId == record.battleId)
            {
                RemoveRecord(groupId, record.battleId);
                break;
            }
        }

        group.recordList.Add(record);

        TransferData data = new TransferData();
        data.SetValue("GroupId", group.id);
        data.SetValue("BattleId", record.battleId);
        data.SetValue<int>("RoomId", record.roomId);
        data.SetValue<string>("DateTime", record.time);
        data.SetValue<string>("GameName", record.gameName);
        List<TransferData> lstPlayer = new List<TransferData>();
        for (int i = 0; i < record.players.Count; ++i)
        {
            RecordPlayerEntity playerEntity = record.players[i];
            TransferData playerData = new TransferData();
            playerData.SetValue("PlayerId", playerEntity.id);
            playerData.SetValue("Avatar", playerEntity.avatar);
            playerData.SetValue("Gold", playerEntity.gold);
            playerData.SetValue("Nickname", playerEntity.nickname);
            lstPlayer.Add(playerData);
        }
        data.SetValue<List<TransferData>>("Player", lstPlayer);

        SendNotification(ON_ADD_RECORD, data);
    }
    #endregion

    public void RemoveRecord(int groupId, int battleId)
    {
        ChatGroupEntity group = GetGroup(groupId);
        if (group == null) return;

        for (int i = 0; i < group.recordList.Count; ++i)
        {
            if (group.recordList[i].battleId == battleId)
            {
                group.recordList.RemoveAt(i);
                break;
            }
        }

        TransferData data = new TransferData();
        data.SetValue("GroupId", groupId);
        data.SetValue("BattleId", battleId);

        SendNotification(ON_REMOVE_RECORD, data);
    }

    #region isManager 是否是管理员
    /// <summary>
    /// 是否是管理员
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="playerId"></param>
    /// <returns></returns>
    public bool isManager(int groupId, int playerId)
    {
        PlayerEntity player = GetMember(groupId, playerId);
        return player.isManager;
    }
    #endregion

    #region SendGroupInfoChangedNotification 发送群信息变更消息
    /// <summary>
    /// 发送群信息变更消息
    /// </summary>
    /// <param name="group"></param>
    private void SendGroupInfoChangedNotification(ChatGroupEntity group)
    {
        if (group == null) return;

        TransferData data = new TransferData();
        data.SetValue("GroupEntity", group);
        SendNotification(ON_GROUP_INFO_CHANGED, data);
    }
    #endregion

}
