//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:17 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class NN_ROOM_SB_JETTON_GET { 

	public const int CODE = 201020; 

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

	public static NN_ROOM_SB_JETTON_GET newBuilder() { 
		return new NN_ROOM_SB_JETTON_GET(); 
	} 

	public static NN_ROOM_SB_JETTON_GET decode(byte[] data) { 
		NN_ROOM_SB_JETTON_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasPour()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pour);
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
		  
		if(this.hasPour()) {
			this.pour = buf.getInt();
		}

	} 

	public bool hasPour() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

