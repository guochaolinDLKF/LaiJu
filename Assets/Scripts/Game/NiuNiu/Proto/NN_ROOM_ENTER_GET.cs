//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:27 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class NN_ROOM_ENTER_GET { 

	public const int CODE = 201002; 

	private byte[] __flag = new byte[1]; 

	private int _roomId; 

	public int roomId { 
		set { 
			if(!this.hasRoomId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._roomId = value;
		} 
		get { 
			return this._roomId;
		} 
	} 

	private int _clubId; 

	public int clubId { 
		set { 
			if(!this.hasClubId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._clubId = value;
		} 
		get { 
			return this._clubId;
		} 
	} 

	public static NN_ROOM_ENTER_GET newBuilder() { 
		return new NN_ROOM_ENTER_GET(); 
	} 

	public static NN_ROOM_ENTER_GET decode(byte[] data) { 
		NN_ROOM_ENTER_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasRoomId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.roomId);
			total += bytes[0].limit();
		}

		if(this.hasClubId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.clubId);
			total += bytes[1].limit();
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
		  
		if(this.hasRoomId()) {
			this.roomId = buf.getInt();
		}

		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

	} 

	public bool hasRoomId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasClubId() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

