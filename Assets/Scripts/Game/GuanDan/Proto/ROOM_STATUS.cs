//===================================================
//Author      : DRB
//CreateTime  ：11/6/2017 2:21:41 PM
//Description ：
//===================================================
namespace guandan.proto {


public enum ROOM_STATUS {
		
		/// <summary>
		/// 准备��?
		/// </summary>
		ROOM_STATUS_READY,
		
		/// <summary>
		/// 进行��?
		/// </summary>
		ROOM_STATUS_BEGIN,
		
		/// <summary>
		/// 结算��?
		/// </summary>
		ROOM_STATUS_SETTLE,
		
		/// <summary>
		/// 发牌��? 
		/// </summary>
		ROOM_STATUS_DEAL,
		
		/// <summary>
		/// 申请解散
		/// </summary>
		ROOM_STATUS_DISMISS,
		
		/// <summary>
		/// 空闲
		/// </summary>
		ROOM_STATUS_IDLE
		
}


}

