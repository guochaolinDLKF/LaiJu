//===================================================
//Author      : DRB
//CreateTime  ：9/26/2017 3:10:35 PM
//Description ：聊天群数据实体
//===================================================
using System.Collections.Generic;

/// <summary>
/// 聊天群数据实体
/// </summary>
public class ChatGroupEntity 
{
    /// <summary>
    /// 群Id
    /// </summary>
    public int id;
    /// <summary>
    /// 群名称
    /// </summary>
    public string name;
    /// <summary>
    /// 群公告
    /// </summary>
    public string announce;
    /// <summary>
    /// 群主Id
    /// </summary>
    public int ownerId;
    /// <summary>
    /// 群主房卡数量
    /// </summary>
    public int cards;
    /// <summary>
    /// 群成员列表
    /// </summary>
    public List<PlayerEntity> members;
    /// <summary>
    /// 群房间列表
    /// </summary>
    public List<RoomEntityBase> rooms;
    /// <summary>
    /// 当前成员数量
    /// </summary>
    public int currMemberCount;
    /// <summary>
    /// 最大成员数量
    /// </summary>
    public int maxMemberCount;

    public bool hasNewApply;

    public List<PlayerEntity> ApplyList;

    public int avatarIndex;
    /// <summary>
    /// 玩家是否是群主
    /// </summary>
    public bool isOwner;
    /// <summary>
    /// 玩家自己
    /// </summary>
    public PlayerEntity playerEntity;
    /// <summary>
    /// 房间数量
    /// </summary>
    public int roomCount;

    /// <summary>
    /// 等待中房间数量
    /// </summary>
    public int WaitingRoomCount
    {
        get
        {
            int count = 0;
            for (int i = 0; i < rooms.Count; ++i)
            {
                if (rooms[i].roomStatus == RoomEntityBase.RoomStatus.Waiting)
                {
                    ++count;
                }
            }
            return count;
        }
    }
    /// <summary>
    /// 游戏中房间数量
    /// </summary>
    public int GamingRoomCount
    {
        get
        {
            int count = 0;
            for (int i = 0; i < rooms.Count; ++i)
            {
                if (rooms[i].roomStatus == RoomEntityBase.RoomStatus.Gaming)
                {
                    ++count;
                }
            }
            return count;
        }
    }
    /// <summary>
    /// 消息列表
    /// </summary>
    public List<MessageEntity> messageList;
    /// <summary>
    /// 未读数量
    /// </summary>
    public int unreadCount;
    /// <summary>
    /// 战绩列表
    /// </summary>
    public List<RecordEntity> recordList;

    public bool isRequested;
    /// <summary>
    /// 群头像
    /// </summary>
    public string avatar;

    public ChatGroupEntity(int id,string name,string announce, int ownerId, int currMemberCount, int maxMemberCount, int avatarIndex, bool isOwner,int roomCount,int cards,string avatar)
    {
        this.id = id;
        this.name = name;
        this.announce = announce;
        this.ownerId = ownerId;
        this.currMemberCount = currMemberCount;
        this.maxMemberCount = maxMemberCount;
        this.avatarIndex = avatarIndex;
        this.isOwner = isOwner;
        this.roomCount = roomCount;
        this.cards = cards;
        members = new List<PlayerEntity>();
        rooms = new List<RoomEntityBase>();
        ApplyList = new List<PlayerEntity>();
        messageList = new List<MessageEntity>();
        recordList = new List<RecordEntity>();
        this.avatar = avatar;
        AppDebug.Log("ChatGroupAvatar:" + avatar);
    }
}
