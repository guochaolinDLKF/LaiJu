//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 5:30:27 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.sss {

public class SSS_CREATE_ROOM { 

	public const int CODE = 1011001; 

	private byte[] __flag = new byte[1]; 

	private ROOM_INFO _roomInfo; 

	public ROOM_INFO roomInfo { 
		set { 
			if(!this.hasRoomInfo()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._roomInfo = value;
		} 
		get { 
			return this._roomInfo;
		} 
	} 

	public static SSS_CREATE_ROOM newBuilder() { 
		return new SSS_CREATE_ROOM(); 
	} 

	public static SSS_CREATE_ROOM decode(byte[] data) { 
		SSS_CREATE_ROOM proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasRoomInfo()) {
			byte[] _byte = this.roomInfo.encode();
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
		  
		if(this.hasRoomInfo()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.roomInfo = ROOM_INFO.decode(bytes);
		}

	} 

	public bool hasRoomInfo() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

