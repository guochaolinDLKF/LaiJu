//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:36 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ZJH_ROOM_REPLY_DISMISS_GET { 

	public const int CODE = 401035; 

	private byte[] __flag = new byte[1]; 

	private ENUM_ROOMRESULT _zjh_enum_roomresult; 

	public ENUM_ROOMRESULT zjh_enum_roomresult { 
		set { 
			if(!this.hasZjhEnumRoomresult()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._zjh_enum_roomresult = value;
		} 
		get { 
			return this._zjh_enum_roomresult;
		} 
	} 

	public static ZJH_ROOM_REPLY_DISMISS_GET newBuilder() { 
		return new ZJH_ROOM_REPLY_DISMISS_GET(); 
	} 

	public static ZJH_ROOM_REPLY_DISMISS_GET decode(byte[] data) { 
		ZJH_ROOM_REPLY_DISMISS_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasZjhEnumRoomresult()) {
			bytes[0] = ByteBuffer.allocate(1);
			bytes[0].put((byte) this.zjh_enum_roomresult);
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
		  
		if(this.hasZjhEnumRoomresult()) {
			this.zjh_enum_roomresult = (ENUM_ROOMRESULT) buf.get();
		}

	} 

	public bool hasZjhEnumRoomresult() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

