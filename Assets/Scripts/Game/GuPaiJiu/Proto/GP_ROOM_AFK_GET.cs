//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:22 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class GP_ROOM_AFK_GET { 

	public const int CODE = 701023; 

	private byte[] __flag = new byte[1]; 

	private bool _isAfk; 

	public bool isAfk { 
		set { 
			if(!this.hasIsAfk()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._isAfk = value;
		} 
		get { 
			return this._isAfk;
		} 
	} 

	public static GP_ROOM_AFK_GET newBuilder() { 
		return new GP_ROOM_AFK_GET(); 
	} 

	public static GP_ROOM_AFK_GET decode(byte[] data) { 
		GP_ROOM_AFK_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasIsAfk()) {
			bytes[0] = ByteBuffer.allocate(1);
			if(this.isAfk) {
				bytes[0].put((byte) 1);
			}else{
				bytes[0].put((byte) 0);
			}
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
		  
		if(this.hasIsAfk()) {
			if(buf.get() == 1) {
				this.isAfk = true;
			}else{
				this.isAfk = false;
			}
		}

	} 

	public bool hasIsAfk() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

