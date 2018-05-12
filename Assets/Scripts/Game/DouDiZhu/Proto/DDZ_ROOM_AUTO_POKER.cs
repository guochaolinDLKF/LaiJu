using System.Collections.Generic;

namespace ddz.proto {

public class DDZ_ROOM_AUTO_POKER { 

	public const int CODE = 301011; 

	private byte[] __flag = new byte[16]; 

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

	private DDZ_POCKER _pocker; 

	public DDZ_POCKER pocker { 
		set { 
			if(!this.hasPocker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._pocker = value;
		} 
		get { 
			return this._pocker;
		} 
	} 

	public static DDZ_ROOM_AUTO_POKER newBuilder() { 
		return new DDZ_ROOM_AUTO_POKER(); 
	} 

	public static DDZ_ROOM_AUTO_POKER decode(byte[] data) { 
		DDZ_ROOM_AUTO_POKER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pos);
			total += bytes[0].limit();
		}

		if(this.hasPocker()) {
			byte[] _byte = this.pocker.encode();
			int len = _byte.Length;
			bytes[1] = ByteBuffer.allocate(4 + len);
			bytes[1].putInt(len);
			bytes[1].put(_byte);
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
		  
		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasPocker()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.pocker = DDZ_POCKER.decode(bytes);
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPocker() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

