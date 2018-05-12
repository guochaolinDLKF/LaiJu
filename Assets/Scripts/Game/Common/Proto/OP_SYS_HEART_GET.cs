//===================================================
//Author      : DRB
//CreateTime  ：1/16/2018 2:59:26 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.common {

public class OP_SYS_HEART_GET { 

	public const int CODE = 99201; 

	private byte[] __flag = new byte[16]; 

	private long _cli_time; 

	public long cli_time { 
		set { 
			if(!this.hasCliTime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._cli_time = value;
		} 
		get { 
			return this._cli_time;
		} 
	} 

	public static OP_SYS_HEART_GET newBuilder() { 
		return new OP_SYS_HEART_GET(); 
	} 

	public static OP_SYS_HEART_GET decode(byte[] data) { 
		OP_SYS_HEART_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasCliTime()) {
			bytes[0] = ByteBuffer.allocate(8);
			bytes[0].putLong(this.cli_time);
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
		  
		if(this.hasCliTime()) {
			this.cli_time = buf.getLong();
		}

	} 

	public bool hasCliTime() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

