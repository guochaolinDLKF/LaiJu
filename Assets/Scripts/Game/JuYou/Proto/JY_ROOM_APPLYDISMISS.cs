using System.Collections.Generic;

namespace proto.jy {

public class JY_ROOM_APPLYDISMISS { 

	public const int CODE = 601015; 

	private byte[] __flag = new byte[1]; 

	private bool _isSucceed; 

	public bool isSucceed { 
		set { 
			if(!this.hasIsSucceed()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._isSucceed = value;
		} 
		get { 
			return this._isSucceed;
		} 
	} 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	private ROOM_STATUS _status; 

	public ROOM_STATUS status { 
		set { 
			if(!this.hasStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._status = value;
		} 
		get { 
			return this._status;
		} 
	} 

	public static JY_ROOM_APPLYDISMISS newBuilder() { 
		return new JY_ROOM_APPLYDISMISS(); 
	} 

	public static JY_ROOM_APPLYDISMISS decode(byte[] data) { 
		JY_ROOM_APPLYDISMISS proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasIsSucceed()) {
			bytes[0] = ByteBuffer.allocate(1);
			if(this.isSucceed) {
				bytes[0].put((byte) 1);
			}else{
				bytes[0].put((byte) 0);
			}
			total += bytes[0].limit();
		}

		if(this.hasPos()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.pos);
			total += bytes[1].limit();
		}

		if(this.hasStatus()) {
			bytes[2] = ByteBuffer.allocate(1);
			bytes[2].put((byte) this.status);
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
		  
		if(this.hasIsSucceed()) {
			if(buf.get() == 1) {
				this.isSucceed = true;
			}else{
				this.isSucceed = false;
			}
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasStatus()) {
			this.status = (ROOM_STATUS) buf.get();
		}

	} 

	public bool hasIsSucceed() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasStatus() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

