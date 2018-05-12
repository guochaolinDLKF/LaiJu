//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:20 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class NN_ROOM_ANSWER_TO_DISMISS_GET { 

	public const int CODE = 201009; 

	private byte[] __flag = new byte[1]; 

	private NN_ENUM_SEAT_DISSOLVE _dissolve; 

	public NN_ENUM_SEAT_DISSOLVE dissolve { 
		set { 
			if(!this.hasDissolve()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._dissolve = value;
		} 
		get { 
			return this._dissolve;
		} 
	} 

	public static NN_ROOM_ANSWER_TO_DISMISS_GET newBuilder() { 
		return new NN_ROOM_ANSWER_TO_DISMISS_GET(); 
	} 

	public static NN_ROOM_ANSWER_TO_DISMISS_GET decode(byte[] data) { 
		NN_ROOM_ANSWER_TO_DISMISS_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasDissolve()) {
			bytes[0] = ByteBuffer.allocate(1);
			bytes[0].put((byte) this.dissolve);
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
		  
		if(this.hasDissolve()) {
			this.dissolve = (NN_ENUM_SEAT_DISSOLVE) buf.get();
		}

	} 

	public bool hasDissolve() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

