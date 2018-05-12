//===================================================
//Author      : DRB
//CreateTime  ：11/6/2017 2:21:32 PM
//Description ：
//===================================================
namespace guandan.proto {


public enum SEAT_STATUS {
		
		/// <summary>
		/// 空闲，新��?��?��?始等��? 
		/// </summary>
		SEAT_STATUS_IDLE,
		
		/// <summary>
		/// 准备，新��?��?��?始准��?
		/// </summary>
		SEAT_STATUS_READY,
		
		/// <summary>
		/// 出牌，自己操作出��? 
		/// </summary>
		SEAT_STATUS_OPERATE,
		
		/// <summary>
		/// 结束，自己的牌打��? 
		/// </summary>
		SEAT_STATUS_FINISH,
		
		/// <summary>
		/// 等待，牌��?中，等待别人出牌 
		/// </summary>
		SEAT_STATUS_WAIT
		
}


}

