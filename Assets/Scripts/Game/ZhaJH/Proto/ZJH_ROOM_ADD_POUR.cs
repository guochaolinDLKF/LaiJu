//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:53 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ZJH_ROOM_ADD_POUR { 

	public const int CODE = 401044; 

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

	private ENUM_POKER_STATUS _pokerstatus; 

	public ENUM_POKER_STATUS pokerstatus { 
		set { 
			if(!this.hasPokerstatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._pokerstatus = value;
		} 
		get { 
			return this._pokerstatus;
		} 
	} 

	private int _round; 

	public int round { 
		set { 
			if(!this.hasRound()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._round = value;
		} 
		get { 
			return this._round;
		} 
	} 

	private float _pour; 

	public float pour { 
		set { 
			if(!this.hasPour()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._totalPour = value;
		} 
		get { 
			return this._totalPour;
		} 
	} 

	public static ZJH_ROOM_ADD_POUR newBuilder() { 
		return new ZJH_ROOM_ADD_POUR(); 
	} 

	public static ZJH_ROOM_ADD_POUR decode(byte[] data) { 
		ZJH_ROOM_ADD_POUR proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[6]; 

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

		if(this.hasPokerstatus()) {
			bytes[2] = ByteBuffer.allocate(1);
			bytes[2].put((byte) this.pokerstatus);
			total += bytes[2].limit();
		}

		if(this.hasRound()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.round);
			total += bytes[3].limit();
		}

		if(this.hasPour()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putFloat(this.pour);
			total += bytes[4].limit();
		}

		if(this.hasTotalPour()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putFloat(this.totalPour);
			total += bytes[5].limit();
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

		if(this.hasPokerstatus()) {
			this.pokerstatus = (ENUM_POKER_STATUS) buf.get();
		}

		if(this.hasRound()) {
			this.round = buf.getInt();
		}

		if(this.hasPour()) {
			this.pour = buf.getFloat();
		}

		if(this.hasTotalPour()) {
			this.totalPour = buf.getFloat();
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasPokerstatus() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasRound() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasPour() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasTotalPour() {
		return (this.__flag[0] & 32) != 0;
	}

}
}

