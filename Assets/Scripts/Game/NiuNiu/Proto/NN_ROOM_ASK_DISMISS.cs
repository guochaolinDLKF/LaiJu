//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:32 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class NN_ROOM_ASK_DISMISS { 

	public const int CODE = 201008; 

	private byte[] __flag = new byte[1]; 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	private NN_ENUM_SEAT_DISSOLVE _dissolve; 

	public NN_ENUM_SEAT_DISSOLVE dissolve { 
		set { 
			if(!this.hasDissolve()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._dissolve = value;
		} 
		get { 
			return this._dissolve;
		} 
	} 

	private NN_ENUM_ROOM_STATUS _nn_room_status; 

	public NN_ENUM_ROOM_STATUS nn_room_status { 
		set { 
			if(!this.hasNnRoomStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._nn_room_status = value;
		} 
		get { 
			return this._nn_room_status;
		} 
	} 

	public static NN_ROOM_ASK_DISMISS newBuilder() { 
		return new NN_ROOM_ASK_DISMISS(); 
	} 

	public static NN_ROOM_ASK_DISMISS decode(byte[] data) { 
		NN_ROOM_ASK_DISMISS proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[4]; 

		int total = 0;
		if(this.hasUnixtime()) {
			bytes[0] = ByteBuffer.allocate(8);
			bytes[0].putLong(this.unixtime);
			total += bytes[0].limit();
		}

		if(this.hasPos()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.pos);
			total += bytes[1].limit();
		}

		if(this.hasDissolve()) {
			bytes[2] = ByteBuffer.allocate(1);
			bytes[2].put((byte) this.dissolve);
			total += bytes[2].limit();
		}

		if(this.hasNnRoomStatus()) {
			bytes[3] = ByteBuffer.allocate(1);
			bytes[3].put((byte) this.nn_room_status);
			total += bytes[3].limit();
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
		  
		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasDissolve()) {
			this.dissolve = (NN_ENUM_SEAT_DISSOLVE) buf.get();
		}

		if(this.hasNnRoomStatus()) {
			this.nn_room_status = (NN_ENUM_ROOM_STATUS) buf.get();
		}

	} 

	public bool hasUnixtime() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasDissolve() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasNnRoomStatus() {
		return (this.__flag[0] & 8) != 0;
	}

}
}

