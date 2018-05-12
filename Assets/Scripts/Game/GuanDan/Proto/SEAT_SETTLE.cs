//===================================================
//Author      : DRB
//CreateTime  ：11/6/2017 2:21:57 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace guandan.proto {

public class SEAT_SETTLE { 

	public const int CODE = 820; 

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

	private int _gold; 

	public int gold { 
		set { 
			if(!this.hasGold()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._gold = value;
		} 
		get { 
			return this._gold;
		} 
	} 

	private int _ranking; 

	public int ranking { 
		set { 
			if(!this.hasRanking()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._ranking = value;
		} 
		get { 
			return this._ranking;
		} 
	} 

	public static SEAT_SETTLE newBuilder() { 
		return new SEAT_SETTLE(); 
	} 

	public static SEAT_SETTLE decode(byte[] data) { 
		SEAT_SETTLE proto = newBuilder();
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

		if(this.hasGold()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.gold);
			total += bytes[1].limit();
		}

		if(this.hasRanking()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.ranking);
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

		if(this.hasGold()) {
			this.gold = buf.getInt();
		}

		if(this.hasRanking()) {
			this.ranking = buf.getInt();
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasGold() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasRanking() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

