//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:10 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class GP_ROOM_BEGIN { 

	public const int CODE = 701010; 

	private byte[] __flag = new byte[1]; 

	private GP_ROOM _room; 

	public GP_ROOM room { 
		set { 
			if(!this.hasRoom()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._room = value;
		} 
		get { 
			return this._room;
		} 
	} 

	public static GP_ROOM_BEGIN newBuilder() { 
		return new GP_ROOM_BEGIN(); 
	} 

	public static GP_ROOM_BEGIN decode(byte[] data) { 
		GP_ROOM_BEGIN proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasRoom()) {
			byte[] _byte = this.room.encode();
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
		  
		if(this.hasRoom()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.room = GP_ROOM.decode(bytes);
		}

	} 

	public bool hasRoom() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

