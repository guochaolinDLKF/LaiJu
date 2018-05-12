//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:27 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class NN_ROOM_ROB_POKER { 

	public const int CODE = 201023; 

	private byte[] __flag = new byte[1]; 

	private NN_POKER _nn_poker; 

	public NN_POKER nn_poker { 
		set { 
			if(!this.hasNnPoker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._nn_poker = value;
		} 
		get { 
			return this._nn_poker;
		} 
	} 

	public static NN_ROOM_ROB_POKER newBuilder() { 
		return new NN_ROOM_ROB_POKER(); 
	} 

	public static NN_ROOM_ROB_POKER decode(byte[] data) { 
		NN_ROOM_ROB_POKER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasNnPoker()) {
			byte[] _byte = this.nn_poker.encode();
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
		  
		if(this.hasNnPoker()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.nn_poker = NN_POKER.decode(bytes);
		}

	} 

	public bool hasNnPoker() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

