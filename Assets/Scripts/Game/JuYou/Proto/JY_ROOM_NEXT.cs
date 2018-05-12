using System.Collections.Generic;

namespace proto.jy {

public class JY_ROOM_NEXT { 

	public const int CODE = 601007; 

	private byte[] __flag = new byte[1]; 

	private int _bankerPos; 

	public int bankerPos { 
		set { 
			if(!this.hasBankerPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._bankerPos = value;
		} 
		get { 
			return this._bankerPos;
		} 
	} 

	public static JY_ROOM_NEXT newBuilder() { 
		return new JY_ROOM_NEXT(); 
	} 

	public static JY_ROOM_NEXT decode(byte[] data) { 
		JY_ROOM_NEXT proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasBankerPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.bankerPos);
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
		  
		if(this.hasBankerPos()) {
			this.bankerPos = buf.getInt();
		}

	} 

	public bool hasBankerPos() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

