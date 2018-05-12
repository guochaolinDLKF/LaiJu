//===================================================
//Author      : DRB
//CreateTime  ：6/9/2017 11:00:56 AM
//Description ：玩家数据实体
//===================================================
using System;
using UnityEngine;

/// <summary>
/// 玩家数据实体
/// </summary>
public class PlayerEntity : IComparable<PlayerEntity>
{
    /// <summary>
    /// 玩家Id
    /// </summary>
    public int id;
    /// <summary>
    /// 昵称
    /// </summary>
    public string nickname;
    /// <summary>
    /// 头像
    /// </summary>
    public string avatar;
    /// <summary>
    /// 性别
    /// </summary>
    public int gender;
    /// <summary>
    /// 所在游戏Id
    /// </summary>
    public int gameId;
    /// <summary>
    /// 所在房间Id
    /// </summary>
    public int roomId;
    /// <summary>
    /// IP地址
    /// </summary>
    public string ipaddr;
    /// <summary>
    /// 经度
    /// </summary>
    public float longitude;
    /// <summary>
    /// 纬度
    /// </summary>
    public float latitude;
    /// <summary>
    /// 在线
    /// </summary>
    public long online;
    /// <summary>
    /// 是否是群主
    /// </summary>
    public bool isOwner;
    /// <summary>
    /// 是否是管理员
    /// </summary>
    public bool isManager;

    public int CompareTo(PlayerEntity other)
    {
        int ret = 0;
        if (other == null) return ret;
        if (id == other.id) return ret;
        else if (isOwner) ret = -1;
        else if (other.isOwner) ret = 1;
        else if (isManager) ret = -1;
        else if (other.isManager) ret = 1;
        else if (online > 0) ret = -1;
        else if (other.online > 0) ret = 1;
        return ret;
    }
}
