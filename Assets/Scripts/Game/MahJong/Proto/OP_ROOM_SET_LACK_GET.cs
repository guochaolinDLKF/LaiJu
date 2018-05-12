using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_SET_LACK_GET { 

	public const int CODE = 101034; 

	private byte[] __flag = new byte[16]; 

	private int _color; 

	public int color { 
		set { 
			if(!this.hasColor()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._color = value;
		} 
		get { 
			return this._color;
		} 
	} 

	public static OP_ROOM_SET_LACK_GET newBuilder() { 
		return new OP_ROOM_SET_LACK_GET(); 
	} 

	public static OP_ROOM_SET_LACK_GET decode(byte[] data) { 
		OP_ROOM_SET_LACK_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasColor()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.color);
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
		  
		if(this.hasColor()) {
			this.color = buf.getInt();
		}

	} 

	public bool hasColor() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

