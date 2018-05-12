//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:53 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class OP_SYS_HEART { 

	public const int CODE = 99201; 

	private byte[] __flag = new byte[1]; 

	private long _svr_time; 

	public long svr_time { 
		set { 
			if(!this.hasSvrTime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._svr_time = value;
		} 
		get { 
			return this._svr_time;
		} 
	} 

	private long _cli_time; 

	public long cli_time { 
		set { 
			if(!this.hasCliTime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._cli_time = value;
		} 
		get { 
			return this._cli_time;
		} 
	} 

	public static OP_SYS_HEART newBuilder() { 
		return new OP_SYS_HEART(); 
	} 

	public static OP_SYS_HEART decode(byte[] data) { 
		OP_SYS_HEART proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasSvrTime()) {
			bytes[0] = ByteBuffer.allocate(8);
			bytes[0].putLong(this.svr_time);
			total += bytes[0].limit();
		}

		if(this.hasCliTime()) {
			bytes[1] = ByteBuffer.allocate(8);
			bytes[1].putLong(this.cli_time);
			total += bytes[1].limit();
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
		  
		if(this.hasSvrTime()) {
			this.svr_time = buf.getLong();
		}

		if(this.hasCliTime()) {
			this.cli_time = buf.getLong();
		}

	} 

	public bool hasSvrTime() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasCliTime() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

