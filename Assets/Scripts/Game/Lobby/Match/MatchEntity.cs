//===================================================
//Author      : DRB
//CreateTime  ：5/18/2017 6:23:43 PM
//Description ：比赛数据实体
//===================================================
using System.Collections.Generic;
using com.oegame.mahjong.protobuf;
using proto.mahjong;
using UnityEngine;


public class MatchEntity 
{
    /// <summary>
    /// 比赛状态
    /// </summary>
    public MatchStatus MatchStatus;

    /// <summary>
    /// 比赛场Id
    /// </summary>
    public int MatchId;

    /// <summary>
    /// 当前报名人数
    /// </summary>
    public int CurrentPlayerCount;

    /// <summary>
    /// 总人数
    /// </summary>
    public int MaxPlayerCount;

    /// <summary>
    /// 是否被淘汰
    /// </summary>
    public bool IsOut;

    /// <summary>
    /// 排名
    /// </summary>
    public int Rank;

    /// <summary>
    /// 等待的数量
    /// </summary>
    public int WaitCount;

    /// <summary>
    /// 晋级的数量
    /// </summary>
    public int RiseCount;

    /// <summary>
    /// 是否结束
    /// </summary>
    public bool IsOver;

    /// <summary>
    /// 结算数据
    /// </summary>
    public List<OP_MATCH_PLAYER_INFO> Ranking;

}
