//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:47 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ZJH_ROOM_READY { 

	public const int CODE = 401031; 

	private byte[] __flag = new byte[1]; 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	private ENUM_SEAT_STATUS _status; 

	public ENUM_SEAT_STATUS status { 
		set { 
			if(!this.hasStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._status = value;
		} 
		get { 
			return this._status;
		} 
	} 

	public static ZJH_ROOM_READY newBuilder() { 
		return new ZJH_ROOM_READY(); 
	} 

	public static ZJH_ROOM_READY decode(byte[] data) { 
		ZJH_ROOM_READY proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pos);
			total += bytes[0].limit();
		}

		if(this.hasStatus()) {
			bytes[1] = ByteBuffer.allocate(1);
			bytes[1].put((byte) this.status);
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
		  
		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasStatus()) {
			this.status = (ENUM_SEAT_STATUS) buf.get();
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasStatus() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

