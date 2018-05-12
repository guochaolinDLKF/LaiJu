//===================================================
//Author      : DRB
//CreateTime  ：7/14/2017 11:16:51 AM
//Description ：座位数据实体基类
//===================================================
using UnityEngine;


public enum DisbandState
{
    /// <summary>
    /// 申请
    /// </summary>
    Apply,
    /// <summary>
    /// 同意
    /// </summary>
    Agree,
    /// <summary>
    /// 拒绝
    /// </summary>
    Refuse,
    /// <summary>
    /// 未响应
    /// </summary>
    Wait,
}

public class SeatEntityBase 
{
    /// <summary>
    /// 玩家编号
    /// </summary>
    public int PlayerId;
    /// <summary>
    /// 是否是玩家
    /// </summary>
    public bool IsPlayer;
    /// <summary>
    /// 位置
    /// </summary>
    public int Pos;
    /// <summary>
    /// 索引
    /// </summary>
    public int Index;
    /// <summary>
    /// 是否庄家
    /// </summary>
    public bool IsBanker;
    /// <summary>
    /// 玩家金币
    /// </summary>
    public int Gold;
    /// <summary>
    /// 玩家昵称
    /// </summary>
    public string Nickname;
    /// <summary>
    /// 性别
    /// </summary>
    public int Gender;
    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar;
    /// <summary>
    /// IP地址
    /// </summary>
    public string IP;
    /// <summary>
    /// 纬度
    /// </summary>
    public float Latitude;
    /// <summary>
    /// 经度
    /// </summary>
    public float Longitude;
    /// <summary>
    /// 焦点是否在当前应用
    /// </summary>
    public bool IsFocus = true;
    /// <summary>
    /// 托管
    /// </summary>
    public bool IsTrustee;
    /// <summary>
    /// 解散房间状态
    /// </summary>
    public DisbandState DisbandState = DisbandState.Wait;

}
