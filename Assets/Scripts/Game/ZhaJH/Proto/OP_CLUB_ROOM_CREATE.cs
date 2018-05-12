//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:48 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class OP_CLUB_ROOM_CREATE { 

	public const int CODE = 99318; 

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

	private OP_CLUB_ROOM _room; 

	public OP_CLUB_ROOM room { 
		set { 
			if(!this.hasRoom()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._room = value;
		} 
		get { 
			return this._room;
		} 
	} 

	public static OP_CLUB_ROOM_CREATE newBuilder() { 
		return new OP_CLUB_ROOM_CREATE(); 
	} 

	public static OP_CLUB_ROOM_CREATE decode(byte[] data) { 
		OP_CLUB_ROOM_CREATE proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasClubId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.clubId);
			total += bytes[0].limit();
		}

		if(this.hasRoom()) {
			byte[] _byte = this.room.encode();
			int len = _byte.Length;
			bytes[1] = ByteBuffer.allocate(4 + len);
			bytes[1].putInt(len);
			bytes[1].put(_byte);
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
		  
		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

		if(this.hasRoom()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.room = OP_CLUB_ROOM.decode(bytes);
		}

	} 

	public bool hasClubId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasRoom() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

