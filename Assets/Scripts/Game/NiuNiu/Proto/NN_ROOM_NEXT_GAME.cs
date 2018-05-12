//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:17 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class NN_ROOM_NEXT_GAME { 

	public const int CODE = 201015; 

	private byte[] __flag = new byte[1]; 

	private long _beginTime; 

	public long beginTime { 
		set { 
			if(!this.hasBeginTime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._beginTime = value;
		} 
		get { 
			return this._beginTime;
		} 
	} 

	public static NN_ROOM_NEXT_GAME newBuilder() { 
		return new NN_ROOM_NEXT_GAME(); 
	} 

	public static NN_ROOM_NEXT_GAME decode(byte[] data) { 
		NN_ROOM_NEXT_GAME proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasBeginTime()) {
			bytes[0] = ByteBuffer.allocate(8);
			bytes[0].putLong(this.beginTime);
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
		  
		if(this.hasBeginTime()) {
			this.beginTime = buf.getLong();
		}

	} 

	public bool hasBeginTime() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

