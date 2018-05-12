//===================================================
//Author      : DRB
//CreateTime  ：5/15/2017 6:03:47 PM
//Description ：麻将AI
//===================================================
using System;
using System.Collections.Generic;
using DRB.MahJong;
using UnityEngine;

/// <summary>
/// 麻将游戏AI
/// </summary>
public class GameMahJongAI : IGameAI
{
    private float m_fAITimer;

#if DEBUG_MODE
    private const float AI_FRAME_SPACE = 0.1f;
#else
    private const float AI_FRAME_SPACE = 1f;
#endif

    #region Override IGameAI

    #region DoAI 执行AI
    /// <summary>
    /// 执行AI
    /// </summary>
    public void DoAI()
    {

        if (Time.realtimeSinceStartup - m_fAITimer > AI_FRAME_SPACE)
        {
            m_fAITimer = Time.realtimeSinceStartup;
            if (RoomMaJiangProxy.Instance.CurrentRoom == null) return;
            if (RoomMaJiangProxy.Instance.PlayerSeat == null) return;
            if (!RoomMaJiangProxy.Instance.PlayerSeat.IsTrustee) return;

            if (GlobalInit.Instance.AILevel == AILevel.Silly)
            {
                DoSillyAI();
            }
            else if (GlobalInit.Instance.AILevel == AILevel.Normal)
            {
                DoNormalAI();
            }
            else if (GlobalInit.Instance.AILevel == AILevel.Clever)
            {
                DoCleverAI();
            }
        }
    }
    #endregion

    #endregion

    #region DoNormalAI 普通AI
    /// <summary>
    /// 没脑子的AI
    /// </summary>
    private void DoNormalAI()
    {
        if (RoomMaJiangProxy.Instance.AskPokerGroup != null)
        {
            for (int i = 0; i < RoomMaJiangProxy.Instance.AskPokerGroup.Count; ++i)
            {
                if ((OperatorType)RoomMaJiangProxy.Instance.AskPokerGroup[i].CombinationType == OperatorType.Hu)
                {
                    MaJiangGameCtrl.Instance.ClientSendOperate(OperatorType.Hu, null);
                    return;
                }
            }
            MaJiangGameCtrl.Instance.ClientSendOperate(OperatorType.Pass, null);
        }
        if (RoomMaJiangProxy.Instance.CurrentState == MahjongGameState.DrawPoker || RoomMaJiangProxy.Instance.CurrentState == MahjongGameState.Operate)
        {
            if (RoomMaJiangProxy.Instance.CurrentRoom.CurrentOperator == RoomMaJiangProxy.Instance.PlayerSeat)
            {
                Poker poker = RoomMaJiangProxy.Instance.PlayerSeat.HitPoker;
                if (poker == null || MahJongHelper.CheckUniversal(poker, RoomMaJiangProxy.Instance.PlayerSeat.UniversalList))
                {
                    for (int i = 0; i < RoomMaJiangProxy.Instance.PlayerSeat.PokerList.Count; ++i)
                    {
                        poker = RoomMaJiangProxy.Instance.PlayerSeat.PokerList[i];
                        if (!MahJongHelper.CheckUniversal(poker, RoomMaJiangProxy.Instance.PlayerSeat.UniversalList))
                        {
                            break;
                        }
                    }
                }
                MaJiangGameCtrl.Instance.ClientSendPlayPoker(poker);
            }
        }
    }
    #endregion

    #region DoSillyAI 没有脑子的AI
    /// <summary>
    /// 没有脑子的AI
    /// </summary>
    private void DoSillyAI()
    {
        if (RoomMaJiangProxy.Instance.AskPokerGroup != null)
        {
            MaJiangGameCtrl.Instance.ClientSendOperate(OperatorType.Pass, null);
            return;
        }
        if (RoomMaJiangProxy.Instance.CurrentState == MahjongGameState.DrawPoker || RoomMaJiangProxy.Instance.CurrentState == MahjongGameState.Operate)
        {
            if (RoomMaJiangProxy.Instance.CurrentRoom.CurrentOperator == RoomMaJiangProxy.Instance.PlayerSeat)
            {
                Poker poker = RoomMaJiangProxy.Instance.PlayerSeat.HitPoker;
                if (poker == null || MahJongHelper.CheckUniversal(poker, RoomMaJiangProxy.Instance.PlayerSeat.UniversalList))
                {
                    for (int i = 0; i < RoomMaJiangProxy.Instance.PlayerSeat.PokerList.Count; ++i)
                    {
                        poker = RoomMaJiangProxy.Instance.PlayerSeat.PokerList[i];
                        if (!MahJongHelper.CheckUniversal(poker, RoomMaJiangProxy.Instance.PlayerSeat.UniversalList))
                        {
                            break;
                        }
                    }
                }
                MaJiangGameCtrl.Instance.ClientSendPlayPoker(poker);
            }
        }
    }
    #endregion

    #region DoCleverAI 聪明的AI
    /// <summary>
    /// 聪明的AI
    /// </summary>
    private void DoCleverAI()
    {
        if (RoomMaJiangProxy.Instance.AskPokerGroup != null && RoomMaJiangProxy.Instance.AskPokerGroup.Count > 0)
        {
            for (int i = 0; i < RoomMaJiangProxy.Instance.AskPokerGroup.Count; ++i)
            {
                if ((OperatorType)RoomMaJiangProxy.Instance.AskPokerGroup[i].CombinationType == OperatorType.Hu)
                {
                    MaJiangGameCtrl.Instance.ClientSendOperate(OperatorType.Hu, null);
                    return;
                }
            }
            Poker poker = null;
            List<Poker> pokers = new List<Poker>();
            if (RoomMaJiangProxy.Instance.AskPokerGroup[0].PokerList.Count > 0)
            {
                poker = RoomMaJiangProxy.Instance.AskPokerGroup[0].PokerList[0];
                pokers.Add(poker);
            }

            OperatorType type = (OperatorType)RoomMaJiangProxy.Instance.AskPokerGroup[0].CombinationType;
            if (type == OperatorType.Gang && RoomMaJiangProxy.Instance.AskPokerGroup[0].SubTypeId != 3)
            {
                pokers.Clear();
                List<List<Poker>> lstAnGang = RoomMaJiangProxy.Instance.GetAnGang();
                List<Poker> lstBuGang = RoomMaJiangProxy.Instance.GetBuGang();
                if (lstAnGang.Count > 0)
                {
                    pokers.AddRange(lstAnGang[0]);
                }
                else if (lstBuGang.Count > 0)
                {
                    pokers.AddRange(lstBuGang);
                }
            }

            if (type == OperatorType.Chi)
            {
                pokers = RoomMaJiangProxy.Instance.GetChi(pokers[0])[0];
            }
            else if (type == OperatorType.ChiTing)
            {
                pokers = RoomMaJiangProxy.Instance.GetChiTing(pokers[0])[0];
            }

            if (type == OperatorType.LiangXi || type == OperatorType.BuXi || type == OperatorType.ZhiDui)
            {
                type = OperatorType.Pass;
            }
            MaJiangGameCtrl.Instance.ClientSendOperate(type, pokers);
        }
        if ((RoomMaJiangProxy.Instance.CurrentRoom.Status != RoomEntity.RoomStatus.Ready && RoomMaJiangProxy.Instance.CurrentRoom.Status != RoomEntity.RoomStatus.Settle) && (RoomMaJiangProxy.Instance.CurrentState == MahjongGameState.DrawPoker || RoomMaJiangProxy.Instance.CurrentState == MahjongGameState.Operate))
        {
            if (RoomMaJiangProxy.Instance.CurrentRoom.CurrentOperator == RoomMaJiangProxy.Instance.PlayerSeat)
            {
                Poker poker = null;
                if (RoomMaJiangProxy.Instance.PlayerSeat.IsTing)
                {
                    poker = RoomMaJiangProxy.Instance.PlayerSeat.HitPoker;
                }
                else
                {

                    Dictionary<Poker, List<Poker>> ting = RoomMaJiangProxy.Instance.GetAllTing();
                    Poker wantPlayPoker = null;
                    if (ting != null && ting.Count > 0)
                    {

                        int count = 0;
                        foreach (var pair in ting)
                        {
                            if (pair.Value != null && pair.Value.Count > count)
                            {
                                count = pair.Value.Count;
                                wantPlayPoker = pair.Key;
                            }
                        }
                    }

                    if (wantPlayPoker != null)
                    {
                        MaJiangGameCtrl.Instance.ClientSendPlayPoker(wantPlayPoker);
                    }
                    else
                    {
                        List<Poker> hand = new List<Poker>(RoomMaJiangProxy.Instance.PlayerSeat.PokerList);
                        if (RoomMaJiangProxy.Instance.PlayerSeat.HitPoker != null)
                        {
                            hand.Add(RoomMaJiangProxy.Instance.PlayerSeat.HitPoker);
                        }

                        for (int i = 0; i < hand.Count; ++i)
                        {
                            bool isSingle = true;
                            for (int j = 0; j < hand.Count; ++j)
                            {
                                if (i == j) continue;

                                if (hand[i].color < 4 && hand[i].color == hand[j].color && Mathf.Abs(hand[i].size - hand[j].size) <= 1)
                                {
                                    isSingle = false;
                                }
                                else if (hand[i].color >= 4 && hand[i].color == hand[j].color && Mathf.Abs(hand[i].size - hand[j].size) <= 0)
                                {
                                    isSingle = false;
                                }
                            }
                            if (isSingle)
                            {
                                MaJiangGameCtrl.Instance.ClientSendPlayPoker(hand[i]);
                                return;
                            }
                        }
                        MaJiangGameCtrl.Instance.ClientSendPlayPoker(hand[UnityEngine.Random.Range(0, hand.Count)]);
                    }

                }
            }
        }
    }
    #endregion
}
