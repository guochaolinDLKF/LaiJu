using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_LUCK_POKER { 

	public const int CODE = 101020; 

	private byte[] __flag = new byte[16]; 

	private int _color; 

	public int color { 
		set { 
			if(!this.hasColor()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._size = value;
		} 
		get { 
			return this._size;
		} 
	} 

	private int _index; 

	public int index { 
		set { 
			if(!this.hasIndex()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._index = value;
		} 
		get { 
			return this._index;
		} 
	} 

	public static OP_ROOM_LUCK_POKER newBuilder() { 
		return new OP_ROOM_LUCK_POKER(); 
	} 

	public static OP_ROOM_LUCK_POKER decode(byte[] data) { 
		OP_ROOM_LUCK_POKER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasColor()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.color);
			total += bytes[0].limit();
		}

		if(this.hasSize()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.size);
			total += bytes[1].limit();
		}

		if(this.hasIndex()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.index);
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
		  
		if(this.hasColor()) {
			this.color = buf.getInt();
		}

		if(this.hasSize()) {
			this.size = buf.getInt();
		}

		if(this.hasIndex()) {
			this.index = buf.getInt();
		}

	} 

	public bool hasColor() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasSize() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasIndex() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

