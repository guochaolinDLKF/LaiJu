//===================================================
//Author      : DRB
//CreateTime  ：5/4/2017 1:25:39 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using com.oegame.mahjong.protobuf;
using proto.mahjong;
using UnityEngine;


public class MatchProxy : ProxyBase<MatchProxy> 
{
    public MatchEntity CurrentMatch;

    



    public MatchProxy() : base()
    {
        CurrentMatch = new MatchEntity();
    }

    /// <summary>
    /// 报名
    /// </summary>
    /// <param name="matchId"></param>
    public void Apply(int matchId)
    {
        CurrentMatch.MatchStatus = MatchStatus.Apply;
        CurrentMatch.MatchId = matchId;
        CurrentMatch.CurrentPlayerCount = 0;
        CurrentMatch.MaxPlayerCount = 0;
        CurrentMatch.IsOut = false;
        CurrentMatch.Rank = 0;
        CurrentMatch.WaitCount = 0;
        CurrentMatch.RiseCount = 0;
        CurrentMatch.IsOver = false;
        CurrentMatch.Ranking = null;
    }

    /// <summary>
    /// 取消报名
    /// </summary>
    public void Bowout()
    {
        CurrentMatch.MatchStatus = MatchStatus.None;
        CurrentMatch.MatchId = 0;
    }

    /// <summary>
    /// 开始比赛
    /// </summary>
    public void BeginMatch()
    {
        CurrentMatch.MatchStatus = MatchStatus.Begin;
        CurrentMatch.IsOut = false;
        CurrentMatch.Rank = 0;
        CurrentMatch.WaitCount = 0;
        CurrentMatch.RiseCount = 0;
        CurrentMatch.IsOver = false;
        CurrentMatch.Ranking = null;
    }

    #region UpdateEnterInfo 更新报名信息
    /// <summary>
    /// 更新报名信息
    /// </summary>
    /// <param name="currentPlayerCount"></param>
    /// <param name="maxPlayerCount"></param>
    public void UpdateEnterInfo(int currentPlayerCount,int maxPlayerCount)
    {
        CurrentMatch.CurrentPlayerCount = currentPlayerCount;
        CurrentMatch.MaxPlayerCount = maxPlayerCount;

        TransferData data = new TransferData();
        data.SetValue("CurrentPlayerCount",CurrentMatch.CurrentPlayerCount);
        data.SetValue("MaxPlayerCount", CurrentMatch.MaxPlayerCount);
        SendNotification(NotificationDefine.ON_APPLY_INFO_CHANGED, data);
    }
    #endregion

    #region Settle 结算
    /// <summary>
    /// 结算
    /// </summary>
    /// <param name="isOut"></param>
    /// <param name="rank"></param>
    public void Settle(bool isOut,int rank)
    {
        CurrentMatch.MatchStatus = MatchStatus.Settle;
        CurrentMatch.IsOut = isOut;
        CurrentMatch.Rank = rank;
    }
    #endregion

    #region UpdateRiseCount 更新晋级数量
    /// <summary>
    /// 更新晋级数量
    /// </summary>
    /// <param name="riseCount"></param>
    public void UpdateRiseCount(int riseCount)
    {
        CurrentMatch.RiseCount = riseCount;
    }
    #endregion

    #region UpdateWaitInfo 更新等待数量
    /// <summary>
    /// 更新等待数量
    /// </summary>
    public void UpdateWaitInfo(int waitCount)
    {
        CurrentMatch.WaitCount = waitCount;

        TransferData data = new TransferData();
        data.SetValue("WaitCount",CurrentMatch.WaitCount);
        SendNotification(NotificationDefine.ON_WAIT_COUNT_CHANGED, data);
    }
    #endregion

    #region Result 比赛结束
    /// <summary>
    /// 比赛结束
    /// </summary>
    /// <param name="ranking"></param>
    public void Result(List<OP_MATCH_PLAYER_INFO> ranking)
    {
        CurrentMatch.IsOver = true;
        CurrentMatch.MatchStatus = MatchStatus.None;
        CurrentMatch.Ranking = ranking;
    }
    #endregion
}
