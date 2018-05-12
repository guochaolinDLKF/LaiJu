//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:14 PM
//Description ：
//===================================================
namespace proto.gp {


public enum SEAT_STATUS {
		
		/// <summary>
		/// 空闲��?
		/// </summary>
		IDLE,
		
		/// <summary>
		/// 准备��?
		/// </summary>
		READY,
		
		/// <summary>
		/// 下注��?
		/// </summary>
		POUR,
		
		/// <summary>
		/// 等待发牌��?
		/// </summary>
		WAITDEAL,
		
		/// <summary>
		/// 组合��?
		/// </summary>
		GROUP,
		
		/// <summary>
		/// 结算��?
		/// </summary>
		SETTLE,
		
		/// <summary>
		/// 组合完成
		/// </summary>
		GROUPDONE,
		
		/// <summary>
		/// 空牌状�??
		/// </summary>
		EMPTYPOKER
		
}


}

