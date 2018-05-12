//===================================================
//Author      : DRB
//CreateTime  ：7/6/2017 7:23:51 PM
//Description ：吃碰杠胡命令
//===================================================
using System.Collections.Generic;
using UnityEngine;

namespace DRB.MahJong
{
    public class OperateCommand : IGameCommand
    {
        private int m_PlayerId;

        private OperatorType m_OperatorType;

        private int m_SubTypeId;

        private List<Poker> m_PokerList;

        private long m_CountDown;

        public OperateCommand(int playerId, OperatorType type, int subTypeId, List<Poker> pokerList, long countDown)
        {
            m_PlayerId = playerId;
            m_OperatorType = type;
            m_SubTypeId = subTypeId;
            m_PokerList = pokerList;
            m_CountDown = countDown;
        }

        public void Execute()
        {
            Debug.Log("===========================================执行吃碰杠胡命令");

            RoomMaJiangProxy.Instance.AskPokerGroup = null;

            switch (m_OperatorType)
            {
                case OperatorType.Pass:
                    if (RoomMaJiangProxy.Instance.PlayerSeat.HitPoker != null && RoomMaJiangProxy.Instance.PlayerSeat.HitPoker.color > 0)
                    {
                        RoomMaJiangProxy.Instance.SetCurrentOperator(RoomMaJiangProxy.Instance.PlayerSeat.Pos);
                    }
                    UIItemOperator.Instance.Close();
#if !IS_LEPING
                    List<Poker> ting = RoomMaJiangProxy.Instance.CheckTingByHand(false);
                    UIItemTingTip.Instance.Show(null, ting);
#endif
                    return;
                case OperatorType.ZhiDui:
                    RoomMaJiangProxy.Instance.ZhiDui(m_PlayerId, m_PokerList);
                    break;
                case OperatorType.Hu:
                    RoomMaJiangProxy.Instance.Hu(m_PlayerId, m_SubTypeId, m_PokerList);
                    break;
                case OperatorType.MingTi:
                    RoomMaJiangProxy.Instance.MingTi(m_PlayerId, m_PokerList);
                    break;
                default:
                    RoomMaJiangProxy.Instance.OperatePoker(m_OperatorType, m_PlayerId, m_SubTypeId, m_PokerList);
                    break;
            }

            RoomMaJiangProxy.Instance.SetCountDown(m_CountDown, m_PlayerId == AccountProxy.Instance.CurrentAccountEntity.passportId);
        }

        public void Revoke()
        {

        }
    }
}
