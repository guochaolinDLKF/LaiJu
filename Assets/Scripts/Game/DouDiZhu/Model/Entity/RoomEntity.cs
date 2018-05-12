//===================================================
//Author      : DRB
//CreateTime  ：7/25/2017 2:47:35 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace DRB.DouDiZhu  {

    public class RoomEntity : RoomEntityBase
    {
        public new enum RoomStatus
        {
            Idle,
            Bet,
            Gaming,
        }

        /// <summary>
        /// 房间状态
        /// </summary>
        public new RoomStatus roomStatus;
        /// <summary>
        /// 房间座位
        /// </summary>
        public List<SeatEntity> SeatList;
        /// <summary>
        /// 倍数
        /// </summary>
        public int Times;
        /// <summary>
        /// 底分
        /// </summary>
        public int baseScore;
        /// <summary>
        /// 座位数
        /// </summary>
        public int SeatCount;
        /// <summary>
        /// 底牌
        /// </summary>
        public List<Poker> basePoker = new List<Poker>();
        /// <summary>
        /// 房主
        /// </summary>
        public int OwnerID;
        /// <summary>
        /// 地主ID
        /// </summary>
        public int currentQiangPlayerId;
        /// <summary>
        /// 玩家座位数据
        /// </summary>
        public SeatEntity PlayerSeat;
    }
}
