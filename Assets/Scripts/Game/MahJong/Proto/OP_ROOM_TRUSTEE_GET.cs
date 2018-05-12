using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_TRUSTEE_GET { 

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

	public static OP_ROOM_TRUSTEE_GET newBuilder() { 
		return new OP_ROOM_TRUSTEE_GET(); 
	} 

	public static OP_ROOM_TRUSTEE_GET decode(byte[] data) { 
		OP_ROOM_TRUSTEE_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

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

	} 

	public bool hasIsTrustee() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

