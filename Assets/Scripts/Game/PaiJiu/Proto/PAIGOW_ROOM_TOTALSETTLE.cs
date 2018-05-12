//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:07 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_ROOM_TOTALSETTLE { 

	public const int CODE = 501021; 

	private byte[] __flag = new byte[1]; 

	private PAIGOW_ROOM _paigow_room; 

	public PAIGOW_ROOM paigow_room { 
		set { 
			if(!this.hasPaigowRoom()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._paigow_room = value;
		} 
		get { 
			return this._paigow_room;
		} 
	} 

	public static PAIGOW_ROOM_TOTALSETTLE newBuilder() { 
		return new PAIGOW_ROOM_TOTALSETTLE(); 
	} 

	public static PAIGOW_ROOM_TOTALSETTLE decode(byte[] data) { 
		PAIGOW_ROOM_TOTALSETTLE proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasPaigowRoom()) {
			byte[] _byte = this.paigow_room.encode();
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
		  
		if(this.hasPaigowRoom()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.paigow_room = PAIGOW_ROOM.decode(bytes);
		}

	} 

	public bool hasPaigowRoom() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

