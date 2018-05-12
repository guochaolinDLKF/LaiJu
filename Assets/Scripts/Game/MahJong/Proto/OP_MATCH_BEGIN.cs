using System.Collections.Generic;

namespace proto.mahjong {

public class OP_MATCH_BEGIN { 

	public const int CODE = 102004; 

	private byte[] __flag = new byte[16]; 

	private int _promoted; 

	public int promoted { 
		set { 
			if(!this.hasPromoted()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._promoted = value;
		} 
		get { 
			return this._promoted;
		} 
	} 

	public static OP_MATCH_BEGIN newBuilder() { 
		return new OP_MATCH_BEGIN(); 
	} 

	public static OP_MATCH_BEGIN decode(byte[] data) { 
		OP_MATCH_BEGIN proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasPromoted()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.promoted);
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
		  
		if(this.hasPromoted()) {
			this.promoted = buf.getInt();
		}

	} 

	public bool hasPromoted() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

