using System.Collections.Generic;

namespace proto.jy {

public class JY_ROOM_ONLINE { 

	public const int CODE = 601021; 

	private byte[] __flag = new byte[1]; 

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

	private bool _isOnLine; 

	public bool isOnLine { 
		set { 
			if(!this.hasIsOnLine()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._isOnLine = value;
		} 
		get { 
			return this._isOnLine;
		} 
	} 

	public static JY_ROOM_ONLINE newBuilder() { 
		return new JY_ROOM_ONLINE(); 
	} 

	public static JY_ROOM_ONLINE decode(byte[] data) { 
		JY_ROOM_ONLINE proto = newBuilder();
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

		if(this.hasIsOnLine()) {
			bytes[1] = ByteBuffer.allocate(1);
			if(this.isOnLine) {
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
		  
		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasIsOnLine()) {
			if(buf.get() == 1) {
				this.isOnLine = true;
			}else{
				this.isOnLine = false;
			}
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasIsOnLine() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

