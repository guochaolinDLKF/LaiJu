//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:37 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class OP_CLUB_ROOM_INFO_GET { 

	public const int CODE = 99310; 

	private byte[] __flag = new byte[1]; 

	private int _clubId; 

	public int clubId { 
		set { 
			if(!this.hasClubId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._clubId = value;
		} 
		get { 
			return this._clubId;
		} 
	} 

	private int _gameId; 

	public int gameId { 
		set { 
			if(!this.hasGameId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._gameId = value;
		} 
		get { 
			return this._gameId;
		} 
	} 

	private int _roomId; 

	public int roomId { 
		set { 
			if(!this.hasRoomId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._roomId = value;
		} 
		get { 
			return this._roomId;
		} 
	} 

	public static OP_CLUB_ROOM_INFO_GET newBuilder() { 
		return new OP_CLUB_ROOM_INFO_GET(); 
	} 

	public static OP_CLUB_ROOM_INFO_GET decode(byte[] data) { 
		OP_CLUB_ROOM_INFO_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasClubId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.clubId);
			total += bytes[0].limit();
		}

		if(this.hasGameId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.gameId);
			total += bytes[1].limit();
		}

		if(this.hasRoomId()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.roomId);
			total += bytes[2].limit();
		}

	
		ByteBuffer buf = ByteBuffer.allocate(1 + total);
	
		buf.put(this.__flag);
	
		for (int i = 0; i < bytes.Length; i++) {
			if (bytes[i] != null) {
			   buf.put(bytes[i].array());
		    }
		}
	
		return buf.array();

	}

	public void build(byte[] data) { 
		  
		ByteBuffer buf = ByteBuffer.wrap(data);
		  
		for (int i = 0; i < this.__flag.Length; i++) {
		    this.__flag[i] = buf.get();
		}
		  
		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

		if(this.hasGameId()) {
			this.gameId = buf.getInt();
		}

		if(this.hasRoomId()) {
			this.roomId = buf.getInt();
		}

	} 

	public bool hasClubId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasGameId() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasRoomId() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

