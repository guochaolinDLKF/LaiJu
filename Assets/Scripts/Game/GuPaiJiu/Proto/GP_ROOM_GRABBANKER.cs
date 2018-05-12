//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:19 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class GP_ROOM_GRABBANKER { 

	public const int CODE = 701025; 

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

	private int _diceFirst; 

	public int diceFirst { 
		set { 
			if(!this.hasDiceFirst()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._diceFirst = value;
		} 
		get { 
			return this._diceFirst;
		} 
	} 

	private int _diceSecond; 

	public int diceSecond { 
		set { 
			if(!this.hasDiceSecond()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._diceSecond = value;
		} 
		get { 
			return this._diceSecond;
		} 
	} 

	private int _isGrabBanker; 

	public int isGrabBanker { 
		set { 
			if(!this.hasIsGrabBanker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._isGrabBanker = value;
		} 
		get { 
			return this._isGrabBanker;
		} 
	} 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	public static GP_ROOM_GRABBANKER newBuilder() { 
		return new GP_ROOM_GRABBANKER(); 
	} 

	public static GP_ROOM_GRABBANKER decode(byte[] data) { 
		GP_ROOM_GRABBANKER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[5]; 

		int total = 0;
		if(this.hasPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pos);
			total += bytes[0].limit();
		}

		if(this.hasDiceFirst()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.diceFirst);
			total += bytes[1].limit();
		}

		if(this.hasDiceSecond()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.diceSecond);
			total += bytes[2].limit();
		}

		if(this.hasIsGrabBanker()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.isGrabBanker);
			total += bytes[3].limit();
		}

		if(this.hasUnixtime()) {
			bytes[4] = ByteBuffer.allocate(8);
			bytes[4].putLong(this.unixtime);
			total += bytes[4].limit();
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

		if(this.hasDiceFirst()) {
			this.diceFirst = buf.getInt();
		}

		if(this.hasDiceSecond()) {
			this.diceSecond = buf.getInt();
		}

		if(this.hasIsGrabBanker()) {
			this.isGrabBanker = buf.getInt();
		}

		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasDiceFirst() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasDiceSecond() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasIsGrabBanker() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasUnixtime() {
		return (this.__flag[0] & 16) != 0;
	}

}
}

