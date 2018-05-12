//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 5:30:43 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.sss {

public class OP_CLUB_ROOM_CLEAR { 

	public const int CODE = 99332; 

	private byte[] __flag = new byte[1]; 

	private int _gameId; 

	public int gameId { 
		set { 
			if(!this.hasGameId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._gameId = value;
		} 
		get { 
			return this._gameId;
		} 
	} 

	public static OP_CLUB_ROOM_CLEAR newBuilder() { 
		return new OP_CLUB_ROOM_CLEAR(); 
	} 

	public static OP_CLUB_ROOM_CLEAR decode(byte[] data) { 
		OP_CLUB_ROOM_CLEAR proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasGameId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.gameId);
			total += bytes[0].limit();
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
		  
		if(this.hasGameId()) {
			this.gameId = buf.getInt();
		}

	} 

	public bool hasGameId() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

