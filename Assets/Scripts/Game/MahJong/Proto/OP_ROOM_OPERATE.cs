using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_OPERATE { 

	public const int CODE = 101010; 

	private byte[] __flag = new byte[16]; 

	private int _index; 

	public int index { 
		set { 
			if(!this.hasIndex()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._index = value;
		} 
		get { 
			return this._index;
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

	private int _color; 

	public int color { 
		set { 
			if(!this.hasColor()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._size = value;
		} 
		get { 
			return this._size;
		} 
	} 

	private bool _isListen; 

	public bool isListen { 
		set { 
			if(!this.hasIsListen()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._isListen = value;
		} 
		get { 
			return this._isListen;
		} 
	} 

	public static OP_ROOM_OPERATE newBuilder() { 
		return new OP_ROOM_OPERATE(); 
	} 

	public static OP_ROOM_OPERATE decode(byte[] data) { 
		OP_ROOM_OPERATE proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[5]; 

		int total = 0;
		if(this.hasIndex()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.index);
			total += bytes[0].limit();
		}

		if(this.hasPlayerId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.playerId);
			total += bytes[1].limit();
		}

		if(this.hasColor()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.color);
			total += bytes[2].limit();
		}

		if(this.hasSize()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.size);
			total += bytes[3].limit();
		}

		if(this.hasIsListen()) {
			bytes[4] = ByteBuffer.allocate(1);
			if(this.isListen) {
				bytes[4].put((byte) 1);
			}else{
				bytes[4].put((byte) 0);
			}
			total += bytes[4].limit();
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
		  
		if(this.hasIndex()) {
			this.index = buf.getInt();
		}

		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

		if(this.hasColor()) {
			this.color = buf.getInt();
		}

		if(this.hasSize()) {
			this.size = buf.getInt();
		}

		if(this.hasIsListen()) {
			if(buf.get() == 1) {
				this.isListen = true;
			}else{
				this.isListen = false;
			}
		}

	} 

	public bool hasIndex() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasColor() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasSize() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasIsListen() {
		return (this.__flag[0] & 16) != 0;
	}

}
}

