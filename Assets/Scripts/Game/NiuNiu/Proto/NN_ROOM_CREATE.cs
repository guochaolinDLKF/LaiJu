//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:30 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class NN_ROOM_CREATE { 

	public const int CODE = 201001; 

	private byte[] __flag = new byte[1]; 

	private NN_ROOM _nn_room; 

	public NN_ROOM nn_room { 
		set { 
			if(!this.hasNnRoom()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._nn_room = value;
		} 
		get { 
			return this._nn_room;
		} 
	} 

	public static NN_ROOM_CREATE newBuilder() { 
		return new NN_ROOM_CREATE(); 
	} 

	public static NN_ROOM_CREATE decode(byte[] data) { 
		NN_ROOM_CREATE proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasNnRoom()) {
			byte[] _byte = this.nn_room.encode();
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
		  
		if(this.hasNnRoom()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.nn_room = NN_ROOM.decode(bytes);
		}

	} 

	public bool hasNnRoom() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

