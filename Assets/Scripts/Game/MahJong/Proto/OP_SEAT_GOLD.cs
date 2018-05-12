using System.Collections.Generic;

namespace proto.mahjong {

public class OP_SEAT_GOLD { 

	public const int CODE = 1008; 

	private byte[] __flag = new byte[16]; 

	private int _playerId; 

	public int playerId { 
		set { 
			if(!this.hasPlayerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._playerId = value;
		} 
		get { 
			return this._playerId;
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

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	public static OP_SEAT_GOLD newBuilder() { 
		return new OP_SEAT_GOLD(); 
	} 

	public static OP_SEAT_GOLD decode(byte[] data) { 
		OP_SEAT_GOLD proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasPlayerId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.playerId);
			total += bytes[0].limit();
		}

		if(this.hasGold()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.gold);
			total += bytes[1].limit();
		}

		if(this.hasPos()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.pos);
			total += bytes[2].limit();
		}

	
		ByteBuffer buf = ByteBuffer.allocate(16 + total);
	
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
		  
		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

		if(this.hasGold()) {
			this.gold = buf.getInt();
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

	} 

	public bool hasPlayerId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasGold() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

