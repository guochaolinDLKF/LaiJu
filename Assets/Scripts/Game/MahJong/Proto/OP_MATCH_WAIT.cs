using System.Collections.Generic;

namespace proto.mahjong {

public class OP_MATCH_WAIT { 

	public const int CODE = 102007; 

	private byte[] __flag = new byte[16]; 

	private int _waitCount; 

	public int waitCount { 
		set { 
			if(!this.hasWaitCount()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._waitCount = value;
		} 
		get { 
			return this._waitCount;
		} 
	} 

	public static OP_MATCH_WAIT newBuilder() { 
		return new OP_MATCH_WAIT(); 
	} 

	public static OP_MATCH_WAIT decode(byte[] data) { 
		OP_MATCH_WAIT proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasWaitCount()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.waitCount);
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
		  
		if(this.hasWaitCount()) {
			this.waitCount = buf.getInt();
		}

	} 

	public bool hasWaitCount() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

