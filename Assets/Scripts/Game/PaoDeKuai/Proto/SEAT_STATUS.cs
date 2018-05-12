//===================================================
//Author      : DRB
//CreateTime  ：12/5/2017 12:01:57 PM
//Description ：
//===================================================
namespace proto.pdk {


public enum SEAT_STATUS {
		
		/// <summary>
		/// 空闲，新��?��?��?始等��? 
		/// </summary>
		SEAT_STATUS_IDLE,
		
		/// <summary>
		/// 准备��?
		/// </summary>
		ROOM_STATUS_READY,
		
		/// <summary>
		/// 出牌，自己操作出��? 
		/// </summary>
		SEAT_STATUS_OPERATE,
		
		/// <summary>
		/// 结束，自己的牌打��? 
		/// </summary>
		SEAT_STATUS_FINISH,
		
		/// <summary>
		/// 弃权，没有金币，放弃��? 
		/// </summary>
		SEAT_STATUS_WAIVER,
		
		/// <summary>
		/// 等待，牌��?中，等待别人出牌 
		/// </summary>
		SEAT_STATUS_WAIT
		
}


}

