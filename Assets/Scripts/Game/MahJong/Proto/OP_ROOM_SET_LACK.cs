using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_SET_LACK { 

	public const int CODE = 101034; 

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

	private int _color; 

	public int color { 
		set { 
			if(!this.hasColor()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._color = value;
		} 
		get { 
			return this._color;
		} 
	} 

	public static OP_ROOM_SET_LACK newBuilder() { 
		return new OP_ROOM_SET_LACK(); 
	} 

	public static OP_ROOM_SET_LACK decode(byte[] data) { 
		OP_ROOM_SET_LACK proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasPlayerId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.playerId);
			total += bytes[0].limit();
		}

		if(this.hasColor()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.color);
			total += bytes[1].limit();
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

		if(this.hasColor()) {
			this.color = buf.getInt();
		}

	} 

	public bool hasPlayerId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasColor() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

