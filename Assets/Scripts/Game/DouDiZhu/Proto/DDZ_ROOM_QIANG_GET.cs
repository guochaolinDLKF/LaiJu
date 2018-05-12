using System.Collections.Generic;

namespace ddz.proto {

public class DDZ_ROOM_QIANG_GET { 

	public const int CODE = 301003; 

	private byte[] __flag = new byte[16]; 

	private int _pour; 

	public int pour { 
		set { 
			if(!this.hasPour()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._pour = value;
		} 
		get { 
			return this._pour;
		} 
	} 

	public static DDZ_ROOM_QIANG_GET newBuilder() { 
		return new DDZ_ROOM_QIANG_GET(); 
	} 

	public static DDZ_ROOM_QIANG_GET decode(byte[] data) { 
		DDZ_ROOM_QIANG_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasPour()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pour);
			total += bytes[0].limit();
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
		  
		if(this.hasPour()) {
			this.pour = buf.getInt();
		}

	} 

	public bool hasPour() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

