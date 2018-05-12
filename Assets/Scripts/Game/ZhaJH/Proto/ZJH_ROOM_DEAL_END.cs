//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:38 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ZJH_ROOM_DEAL_END { 

	public const int CODE = 401049; 

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

	private int _round; 

	public int round { 
		set { 
			if(!this.hasRound()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._round = value;
		} 
		get { 
			return this._round;
		} 
	} 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	public static ZJH_ROOM_DEAL_END newBuilder() { 
		return new ZJH_ROOM_DEAL_END(); 
	} 

	public static ZJH_ROOM_DEAL_END decode(byte[] data) { 
		ZJH_ROOM_DEAL_END proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pos);
			total += bytes[0].limit();
		}

		if(this.hasRound()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.round);
			total += bytes[1].limit();
		}

		if(this.hasUnixtime()) {
			bytes[2] = ByteBuffer.allocate(8);
			bytes[2].putLong(this.unixtime);
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
		  
		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasRound()) {
			this.round = buf.getInt();
		}

		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasRound() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasUnixtime() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

