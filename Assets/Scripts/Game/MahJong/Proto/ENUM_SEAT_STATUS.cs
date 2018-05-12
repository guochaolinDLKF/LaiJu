namespace proto.mahjong {


public enum ENUM_SEAT_STATUS {
		
		/// <summary>
		/// 空闲，新一局开始等待 
		/// </summary>
		SEAT_STATUS_IDLE,
		
		/// <summary>
		/// 准备，新一局开始准备
		/// </summary>
		SEAT_STATUS_READY,
		
		/// <summary>
		/// 出牌，自己操作出牌 
		/// </summary>
		SEAT_STATUS_OPERATE,
		
		/// <summary>
		/// 结束，自己的牌打完 
		/// </summary>
		SEAT_STATUS_FINISH,
		
		/// <summary>
		/// 弃权，没有金币，放弃了 
		/// </summary>
		SEAT_STATUS_WAIVER,
		
		/// <summary>
		/// 等待，牌局中，等待别人出牌 
		/// </summary>
		SEAT_STATUS_WAIT,
		
		/// <summary>
		/// 是否吃，碰，杠，胡，自摸 
		/// </summary>
		SEAT_STATUS_FIGHT
		
}


}

