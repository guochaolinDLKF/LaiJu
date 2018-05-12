using System.Collections.Generic;

namespace proto.jy {

public class JY_ROOM_JETTON_GET { 

	public const int CODE = 601006; 

	private byte[] __flag = new byte[1]; 

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

	private bool _isAllBag; 

	public bool isAllBag { 
		set { 
			if(!this.hasIsAllBag()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._isAllBag = value;
		} 
		get { 
			return this._isAllBag;
		} 
	} 

	public static JY_ROOM_JETTON_GET newBuilder() { 
		return new JY_ROOM_JETTON_GET(); 
	} 

	public static JY_ROOM_JETTON_GET decode(byte[] data) { 
		JY_ROOM_JETTON_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasPour()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pour);
			total += bytes[0].limit();
		}

		if(this.hasIsAllBag()) {
			bytes[1] = ByteBuffer.allocate(1);
			if(this.isAllBag) {
				bytes[1].put((byte) 1);
			}else{
				bytes[1].put((byte) 0);
			}
			total += bytes[1].limit();
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
		  
		if(this.hasPour()) {
			this.pour = buf.getInt();
		}

		if(this.hasIsAllBag()) {
			if(buf.get() == 1) {
				this.isAllBag = true;
			}else{
				this.isAllBag = false;
			}
		}

	} 

	public bool hasPour() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasIsAllBag() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

