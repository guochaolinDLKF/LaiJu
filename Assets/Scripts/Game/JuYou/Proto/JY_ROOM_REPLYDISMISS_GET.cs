using System.Collections.Generic;

namespace proto.jy {

public class JY_ROOM_REPLYDISMISS_GET { 

	public const int CODE = 601016; 

	private byte[] __flag = new byte[1]; 

	private bool _isDismiss; 

	public bool isDismiss { 
		set { 
			if(!this.hasIsDismiss()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._isDismiss = value;
		} 
		get { 
			return this._isDismiss;
		} 
	} 

	public static JY_ROOM_REPLYDISMISS_GET newBuilder() { 
		return new JY_ROOM_REPLYDISMISS_GET(); 
	} 

	public static JY_ROOM_REPLYDISMISS_GET decode(byte[] data) { 
		JY_ROOM_REPLYDISMISS_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasIsDismiss()) {
			bytes[0] = ByteBuffer.allocate(1);
			if(this.isDismiss) {
				bytes[0].put((byte) 1);
			}else{
				bytes[0].put((byte) 0);
			}
			total += bytes[0].limit();
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
		  
		if(this.hasIsDismiss()) {
			if(buf.get() == 1) {
				this.isDismiss = true;
			}else{
				this.isDismiss = false;
			}
		}

	} 

	public bool hasIsDismiss() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

