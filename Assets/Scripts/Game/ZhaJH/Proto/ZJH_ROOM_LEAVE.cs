//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:32 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ZJH_ROOM_LEAVE { 

	public const int CODE = 401030; 

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

	private int _playerId; 

	public int playerId { 
		set { 
			if(!this.hasPlayerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._playerId = value;
		} 
		get { 
			return this._playerId;
		} 
	} 

	private int _maxLoop; 

	public int maxLoop { 
		set { 
			if(!this.hasMaxLoop()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._maxLoop = value;
		} 
		get { 
			return this._maxLoop;
		} 
	} 

	private float _overGold; 

	public float overGold { 
		set { 
			if(!this.hasOverGold()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._overGold = value;
		} 
		get { 
			return this._overGold;
		} 
	} 

	public static ZJH_ROOM_LEAVE newBuilder() { 
		return new ZJH_ROOM_LEAVE(); 
	} 

	public static ZJH_ROOM_LEAVE decode(byte[] data) { 
		ZJH_ROOM_LEAVE proto = newBuilder();
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

		if(this.hasPlayerId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.playerId);
			total += bytes[1].limit();
		}

		if(this.hasMaxLoop()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.maxLoop);
			total += bytes[2].limit();
		}

		if(this.hasOverGold()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putFloat(this.overGold);
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

		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

		if(this.hasMaxLoop()) {
			this.maxLoop = buf.getInt();
		}

		if(this.hasOverGold()) {
			this.overGold = buf.getFloat();
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasMaxLoop() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasOverGold() {
		return (this.__flag[0] & 8) != 0;
	}

}
}

