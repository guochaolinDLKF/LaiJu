//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:45 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class SEAT_COMMON { 

	public const int CODE = 4006; 

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

	private float _pour; 

	public float pour { 
		set { 
			if(!this.hasPour()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._pour = value;
		} 
		get { 
			return this._pour;
		} 
	} 

	private float _totalPour; 

	public float totalPour { 
		set { 
			if(!this.hasTotalPour()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._totalPour = value;
		} 
		get { 
			return this._totalPour;
		} 
	} 

	private bool _isAfk; 

	public bool isAfk { 
		set { 
			if(!this.hasIsAfk()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._isAfk = value;
		} 
		get { 
			return this._isAfk;
		} 
	} 

	public static SEAT_COMMON newBuilder() { 
		return new SEAT_COMMON(); 
	} 

	public static SEAT_COMMON decode(byte[] data) { 
		SEAT_COMMON proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[4]; 

		int total = 0;
		if(this.hasPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pos);
			total += bytes[0].limit();
		}

		if(this.hasPour()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putFloat(this.pour);
			total += bytes[1].limit();
		}

		if(this.hasTotalPour()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putFloat(this.totalPour);
			total += bytes[2].limit();
		}

		if(this.hasIsAfk()) {
			bytes[3] = ByteBuffer.allocate(1);
			if(this.isAfk) {
				bytes[3].put((byte) 1);
			}else{
				bytes[3].put((byte) 0);
			}
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
		  
		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasPour()) {
			this.pour = buf.getFloat();
		}

		if(this.hasTotalPour()) {
			this.totalPour = buf.getFloat();
		}

		if(this.hasIsAfk()) {
			if(buf.get() == 1) {
				this.isAfk = true;
			}else{
				this.isAfk = false;
			}
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPour() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasTotalPour() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasIsAfk() {
		return (this.__flag[0] & 8) != 0;
	}

}
}

