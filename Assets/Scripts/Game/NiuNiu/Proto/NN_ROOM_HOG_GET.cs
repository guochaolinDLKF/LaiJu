//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:26 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class NN_ROOM_HOG_GET { 

	public const int CODE = 201017; 

	private byte[] __flag = new byte[1]; 

	private int _rob_zhuang; 

	public int rob_zhuang { 
		set { 
			if(!this.hasRobZhuang()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._rob_zhuang = value;
		} 
		get { 
			return this._rob_zhuang;
		} 
	} 

	public static NN_ROOM_HOG_GET newBuilder() { 
		return new NN_ROOM_HOG_GET(); 
	} 

	public static NN_ROOM_HOG_GET decode(byte[] data) { 
		NN_ROOM_HOG_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasRobZhuang()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.rob_zhuang);
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
		  
		if(this.hasRobZhuang()) {
			this.rob_zhuang = buf.getInt();
		}

	} 

	public bool hasRobZhuang() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

