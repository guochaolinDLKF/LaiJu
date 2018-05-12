using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_TRUSTEE { 

	public const int CODE = 101019; 

	private byte[] __flag = new byte[16]; 

	private bool _isTrustee; 

	public bool isTrustee { 
		set { 
			if(!this.hasIsTrustee()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._isTrustee = value;
		} 
		get { 
			return this._isTrustee;
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

	public static OP_ROOM_TRUSTEE newBuilder() { 
		return new OP_ROOM_TRUSTEE(); 
	} 

	public static OP_ROOM_TRUSTEE decode(byte[] data) { 
		OP_ROOM_TRUSTEE proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasIsTrustee()) {
			bytes[0] = ByteBuffer.allocate(1);
			if(this.isTrustee) {
				bytes[0].put((byte) 1);
			}else{
				bytes[0].put((byte) 0);
			}
			total += bytes[0].limit();
		}

		if(this.hasPos()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.pos);
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
		  
		if(this.hasIsTrustee()) {
			if(buf.get() == 1) {
				this.isTrustee = true;
			}else{
				this.isTrustee = false;
			}
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

	} 

	public bool hasIsTrustee() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

