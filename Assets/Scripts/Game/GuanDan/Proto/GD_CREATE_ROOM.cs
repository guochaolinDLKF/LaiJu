//===================================================
//Author      : DRB
//CreateTime  ：11/6/2017 2:21:39 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace guandan.proto {

public class GD_CREATE_ROOM { 

	public const int CODE = 801001; 

	private byte[] __flag = new byte[1]; 

	private ROOM_INFO _roominfo; 

	public ROOM_INFO roominfo { 
		set { 
			if(!this.hasRoominfo()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._roominfo = value;
		} 
		get { 
			return this._roominfo;
		} 
	} 

	public static GD_CREATE_ROOM newBuilder() { 
		return new GD_CREATE_ROOM(); 
	} 

	public static GD_CREATE_ROOM decode(byte[] data) { 
		GD_CREATE_ROOM proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasRoominfo()) {
			byte[] _byte = this.roominfo.encode();
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
		  
		if(this.hasRoominfo()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.roominfo = ROOM_INFO.decode(bytes);
		}

	} 

	public bool hasRoominfo() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

