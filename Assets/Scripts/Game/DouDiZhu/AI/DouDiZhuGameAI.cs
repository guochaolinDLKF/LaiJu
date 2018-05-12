//===================================================
//Author      : DRB
//CreateTime  ：12/6/2017 4:40:37 PM
//Description ：斗地主AI
//===================================================

using UnityEngine;

namespace DRB.DouDiZhu
{
    public class DouDiZhuGameAI : IGameAI
    {
        private float m_fAITimer;

#if DEBUG_MODE
        private const float AI_FRAME_SPACE = 0.4f;
#else
        private const float AI_FRAME_SPACE = 1f;
#endif


        public void DoAI()
        {
            if (Time.realtimeSinceStartup - m_fAITimer > AI_FRAME_SPACE)
            {
                m_fAITimer = Time.realtimeSinceStartup;
                if (RoomProxy.Instance.CurrentRoom == null) return;
                if (RoomProxy.Instance.CurrentRoom.PlayerSeat == null) return;
                if (!RoomProxy.Instance.CurrentRoom.PlayerSeat.IsTrustee) return;
                DoSillyAI();
                //if (GlobalInit.Instance.MahjongConfig.AILevel == AILevel.Normal)
                //{
                //    DoSillyAI();
                //}
                //else if (GlobalInit.Instance.MahjongConfig.AILevel == AILevel.Clever)
                //{
                //    DoNormalAI();
                //}
            }
        }

        #region DoSillyAI 没脑子的AI
        /// <summary>
        /// 没脑子的AI
        /// </summary>
        private void DoSillyAI()
        {
            if (RoomProxy.Instance.CurrentRoom.PlayerSeat.status == SeatEntity.SeatStatus.PlayPoker)
            {
                DouDiZhuGameCtrl.Instance.Trustee();
            }
        }
        #endregion

        #region DoNormalAI 稍微有点脑子的AI
        /// <summary>
        /// 稍微有点脑子的AI
        /// </summary>
        private void DoNormalAI()
        {
            if (RoomProxy.Instance.CurrentRoom.PlayerSeat.status == SeatEntity.SeatStatus.PlayPoker)
            {
                DouDiZhuGameCtrl.Instance.NormalAITrustee();
            }
        }
        #endregion
    }
}
