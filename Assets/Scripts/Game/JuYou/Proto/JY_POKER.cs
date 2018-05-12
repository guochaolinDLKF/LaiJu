using System.Collections.Generic;

namespace proto.jy {

public class JY_POKER { 

	public const int CODE = 6001; 

	private byte[] __flag = new byte[1]; 

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

	private int _type; 

	public int type { 
		set { 
			if(!this.hasType()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._type = value;
		} 
		get { 
			return this._type;
		} 
	} 

	public static JY_POKER newBuilder() { 
		return new JY_POKER(); 
	} 

	public static JY_POKER decode(byte[] data) { 
		JY_POKER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasIndex()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.index);
			total += bytes[0].limit();
		}

		if(this.hasSize()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.size);
			total += bytes[1].limit();
		}

		if(this.hasType()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.type);
			total += bytes[2].limit();
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
		  
		if(this.hasIndex()) {
			this.index = buf.getInt();
		}

		if(this.hasSize()) {
			this.size = buf.getInt();
		}

		if(this.hasType()) {
			this.type = buf.getInt();
		}

	} 

	public bool hasIndex() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasSize() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasType() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

