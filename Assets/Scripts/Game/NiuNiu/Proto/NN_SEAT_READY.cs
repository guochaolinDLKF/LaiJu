//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:14 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class NN_SEAT_READY { 

	public const int CODE = 201021; 

	private byte[] __flag = new byte[1]; 

	private NN_SEAT _nn_seat; 

	public NN_SEAT nn_seat { 
		set { 
			if(!this.hasNnSeat()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._nn_seat = value;
		} 
		get { 
			return this._nn_seat;
		} 
	} 

	public static NN_SEAT_READY newBuilder() { 
		return new NN_SEAT_READY(); 
	} 

	public static NN_SEAT_READY decode(byte[] data) { 
		NN_SEAT_READY proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasNnSeat()) {
			byte[] _byte = this.nn_seat.encode();
			int len = _byte.Length;
			bytes[0] = ByteBuffer.allocate(4 + len);
			bytes[0].putInt(len);
			bytes[0].put(_byte);
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
		  
		if(this.hasNnSeat()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.nn_seat = NN_SEAT.decode(bytes);
		}

	} 

	public bool hasNnSeat() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

