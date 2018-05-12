//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 5:30:37 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.sss {

public class OP_PLAYER_CONNECT { 

	public const int CODE = 99101; 

	private byte[] __flag = new byte[1]; 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	public static OP_PLAYER_CONNECT newBuilder() { 
		return new OP_PLAYER_CONNECT(); 
	} 

	public static OP_PLAYER_CONNECT decode(byte[] data) { 
		OP_PLAYER_CONNECT proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasUnixtime()) {
			bytes[0] = ByteBuffer.allocate(8);
			bytes[0].putLong(this.unixtime);
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
		  
		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
		}

	} 

	public bool hasUnixtime() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

