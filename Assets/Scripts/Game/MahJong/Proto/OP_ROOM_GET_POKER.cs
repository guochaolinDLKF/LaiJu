using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_GET_POKER { 

	public const int CODE = 101009; 

	private byte[] __flag = new byte[16]; 

	private bool _isLast; 

	public bool isLast { 
		set { 
			if(!this.hasIsLast()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._isLast = value;
		} 
		get { 
			return this._isLast;
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

	private long _countdown; 

	public long countdown { 
		set { 
			if(!this.hasCountdown()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._countdown = value;
		} 
		get { 
			return this._countdown;
		} 
	} 

	private int _index; 

	public int index { 
		set { 
			if(!this.hasIndex()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._index = value;
		} 
		get { 
			return this._index;
		} 
	} 

	private int _color; 

	public int color { 
		set { 
			if(!this.hasColor()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._color = value;
		} 
		get { 
			return this._color;
		} 
	} 

	private int _size; 

	public int size { 
		set { 
			if(!this.hasSize()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._size = value;
		} 
		get { 
			return this._size;
		} 
	} 

	private bool _isBuhua; 

	public bool isBuhua { 
		set { 
			if(!this.hasIsBuhua()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._isBuhua = value;
		} 
		get { 
			return this._isBuhua;
		} 
	} 

	public static OP_ROOM_GET_POKER newBuilder() { 
		return new OP_ROOM_GET_POKER(); 
	} 

	public static OP_ROOM_GET_POKER decode(byte[] data) { 
		OP_ROOM_GET_POKER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[7]; 

		int total = 0;
		if(this.hasIsLast()) {
			bytes[0] = ByteBuffer.allocate(1);
			if(this.isLast) {
				bytes[0].put((byte) 1);
			}else{
				bytes[0].put((byte) 0);
			}
			total += bytes[0].limit();
		}

		if(this.hasPlayerId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.playerId);
			total += bytes[1].limit();
		}

		if(this.hasCountdown()) {
			bytes[2] = ByteBuffer.allocate(8);
			bytes[2].putLong(this.countdown);
			total += bytes[2].limit();
		}

		if(this.hasIndex()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.index);
			total += bytes[3].limit();
		}

		if(this.hasColor()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.color);
			total += bytes[4].limit();
		}

		if(this.hasSize()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putInt(this.size);
			total += bytes[5].limit();
		}

		if(this.hasIsBuhua()) {
			bytes[6] = ByteBuffer.allocate(1);
			if(this.isBuhua) {
				bytes[6].put((byte) 1);
			}else{
				bytes[6].put((byte) 0);
			}
			total += bytes[6].limit();
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
		  
		if(this.hasIsLast()) {
			if(buf.get() == 1) {
				this.isLast = true;
			}else{
				this.isLast = false;
			}
		}

		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

		if(this.hasCountdown()) {
			this.countdown = buf.getLong();
		}

		if(this.hasIndex()) {
			this.index = buf.getInt();
		}

		if(this.hasColor()) {
			this.color = buf.getInt();
		}

		if(this.hasSize()) {
			this.size = buf.getInt();
		}

		if(this.hasIsBuhua()) {
			if(buf.get() == 1) {
				this.isBuhua = true;
			}else{
				this.isBuhua = false;
			}
		}

	} 

	public bool hasIsLast() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasCountdown() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasIndex() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasColor() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasSize() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasIsBuhua() {
		return (this.__flag[0] & 64) != 0;
	}

}
}

