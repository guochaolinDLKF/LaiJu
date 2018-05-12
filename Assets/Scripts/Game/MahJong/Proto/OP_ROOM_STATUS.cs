using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_STATUS { 

	public const int CODE = 101027; 

	private byte[] __flag = new byte[16]; 

	private ENUM_ROOM_STATUS _status; 

	public ENUM_ROOM_STATUS status { 
		set { 
			if(!this.hasStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._status = value;
		} 
		get { 
			return this._status;
		} 
	} 

	public static OP_ROOM_STATUS newBuilder() { 
		return new OP_ROOM_STATUS(); 
	} 

	public static OP_ROOM_STATUS decode(byte[] data) { 
		OP_ROOM_STATUS proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasStatus()) {
			bytes[0] = ByteBuffer.allocate(1);
			bytes[0].put((byte) this.status);
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
		  
		if(this.hasStatus()) {
			this.status = (ENUM_ROOM_STATUS) buf.get();
		}

	} 

	public bool hasStatus() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

