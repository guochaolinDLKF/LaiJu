//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:22 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_ROOM_GAMESTART { 

	public const int CODE = 501020; 

	private byte[] __flag = new byte[1]; 

	private int _loop; 

	public int loop { 
		set { 
			if(!this.hasLoop()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._loop = value;
		} 
		get { 
			return this._loop;
		} 
	} 

	public static PAIGOW_ROOM_GAMESTART newBuilder() { 
		return new PAIGOW_ROOM_GAMESTART(); 
	} 

	public static PAIGOW_ROOM_GAMESTART decode(byte[] data) { 
		PAIGOW_ROOM_GAMESTART proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasLoop()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.loop);
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
		  
		if(this.hasLoop()) {
			this.loop = buf.getInt();
		}

	} 

	public bool hasLoop() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

