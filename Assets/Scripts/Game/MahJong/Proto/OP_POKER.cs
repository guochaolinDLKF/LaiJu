using System.Collections.Generic;

namespace proto.mahjong {

public class OP_POKER { 

	public const int CODE = 1001; 

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

	private int _size; 

	public int size { 
		set { 
			if(!this.hasSize()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._size = value;
		} 
		get { 
			return this._size;
		} 
	} 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	public static OP_POKER newBuilder() { 
		return new OP_POKER(); 
	} 

	public static OP_POKER decode(byte[] data) { 
		OP_POKER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[4]; 

		int total = 0;
		if(this.hasIndex()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.index);
			total += bytes[0].limit();
		}

		if(this.hasColor()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.color);
			total += bytes[1].limit();
		}

		if(this.hasSize()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.size);
			total += bytes[2].limit();
		}

		if(this.hasPos()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.pos);
			total += bytes[3].limit();
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

		if(this.hasColor()) {
			this.color = buf.getInt();
		}

		if(this.hasSize()) {
			this.size = buf.getInt();
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

	} 

	public bool hasIndex() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasColor() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasSize() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 8) != 0;
	}

}
}

