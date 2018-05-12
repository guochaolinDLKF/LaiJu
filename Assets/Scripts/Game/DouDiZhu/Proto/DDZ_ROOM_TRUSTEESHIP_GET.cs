using System.Collections.Generic;

namespace ddz.proto {

public class DDZ_ROOM_TRUSTEESHIP_GET { 

	public const int CODE = 301013; 

	private byte[] __flag = new byte[16]; 

	private bool _isTrusteeship; 

	public bool isTrusteeship { 
		set { 
			if(!this.hasIsTrusteeship()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._isTrusteeship = value;
		} 
		get { 
			return this._isTrusteeship;
		} 
	} 

	public static DDZ_ROOM_TRUSTEESHIP_GET newBuilder() { 
		return new DDZ_ROOM_TRUSTEESHIP_GET(); 
	} 

	public static DDZ_ROOM_TRUSTEESHIP_GET decode(byte[] data) { 
		DDZ_ROOM_TRUSTEESHIP_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasIsTrusteeship()) {
			bytes[0] = ByteBuffer.allocate(1);
			if(this.isTrusteeship) {
				bytes[0].put((byte) 1);
			}else{
				bytes[0].put((byte) 0);
			}
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
		  
		if(this.hasIsTrusteeship()) {
			if(buf.get() == 1) {
				this.isTrusteeship = true;
			}else{
				this.isTrusteeship = false;
			}
		}

	} 

	public bool hasIsTrusteeship() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

