//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:22 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class NN_ROOM_HOG { 

	public const int CODE = 201017; 

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

	private int _rob_zhuang; 

	public int rob_zhuang { 
		set { 
			if(!this.hasRobZhuang()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._rob_zhuang = value;
		} 
		get { 
			return this._rob_zhuang;
		} 
	} 

	public static NN_ROOM_HOG newBuilder() { 
		return new NN_ROOM_HOG(); 
	} 

	public static NN_ROOM_HOG decode(byte[] data) { 
		NN_ROOM_HOG proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

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

		if(this.hasRobZhuang()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.rob_zhuang);
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
		  
		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasRobZhuang()) {
			this.rob_zhuang = buf.getInt();
		}

	} 

	public bool hasUnixtime() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasRobZhuang() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

