using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_RANDOM_DICE { 

	public const int CODE = 101024; 

	private byte[] __flag = new byte[16]; 

	private byte _diceA; 

	public byte diceA { 
		set { 
			if(!this.hasDiceA()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._diceA = value;
		} 
		get { 
			return this._diceA;
		} 
	} 

	private byte _diceB; 

	public byte diceB { 
		set { 
			if(!this.hasDiceB()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._diceB = value;
		} 
		get { 
			return this._diceB;
		} 
	} 

	private int _playerId; 

	public int playerId { 
		set { 
			if(!this.hasPlayerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._playerId = value;
		} 
		get { 
			return this._playerId;
		} 
	} 

	public static OP_ROOM_RANDOM_DICE newBuilder() { 
		return new OP_ROOM_RANDOM_DICE(); 
	} 

	public static OP_ROOM_RANDOM_DICE decode(byte[] data) { 
		OP_ROOM_RANDOM_DICE proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasDiceA()) {
			bytes[0] = ByteBuffer.allocate(1);
			bytes[0].put(this.diceA);
			total += bytes[0].limit();
		}

		if(this.hasDiceB()) {
			bytes[1] = ByteBuffer.allocate(1);
			bytes[1].put(this.diceB);
			total += bytes[1].limit();
		}

		if(this.hasPlayerId()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.playerId);
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
		  
		if(this.hasDiceA()) {
			this.diceA = buf.get();
		}

		if(this.hasDiceB()) {
			this.diceB = buf.get();
		}

		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

	} 

	public bool hasDiceA() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasDiceB() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

