//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:43 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ZJH_ROOM_CHANGE_VILLAGE { 

	public const int CODE = 401053; 

	private byte[] __flag = new byte[1]; 

	private SEAT _zjh_seat; 

	public SEAT zjh_seat { 
		set { 
			if(!this.hasZjhSeat()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._zjh_seat = value;
		} 
		get { 
			return this._zjh_seat;
		} 
	} 

	public static ZJH_ROOM_CHANGE_VILLAGE newBuilder() { 
		return new ZJH_ROOM_CHANGE_VILLAGE(); 
	} 

	public static ZJH_ROOM_CHANGE_VILLAGE decode(byte[] data) { 
		ZJH_ROOM_CHANGE_VILLAGE proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasZjhSeat()) {
			byte[] _byte = this.zjh_seat.encode();
			int len = _byte.Length;
			bytes[0] = ByteBuffer.allocate(4 + len);
			bytes[0].putInt(len);
			bytes[0].put(_byte);
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
		  
		if(this.hasZjhSeat()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.zjh_seat = SEAT.decode(bytes);
		}

	} 

	public bool hasZjhSeat() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

