//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:56 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ZJH_ROOM_RECREATE { 

	public const int CODE = 401033; 

	private byte[] __flag = new byte[1]; 

	private ROOM _zjh_room; 

	public ROOM zjh_room { 
		set { 
			if(!this.hasZjhRoom()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._zjh_room = value;
		} 
		get { 
			return this._zjh_room;
		} 
	} 

	private string _nickname; 

	public string nickname { 
		set { 
			if(!this.hasNickname()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._nickname = value;
		} 
		get { 
			return this._nickname;
		} 
	} 

	public static ZJH_ROOM_RECREATE newBuilder() { 
		return new ZJH_ROOM_RECREATE(); 
	} 

	public static ZJH_ROOM_RECREATE decode(byte[] data) { 
		ZJH_ROOM_RECREATE proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasZjhRoom()) {
			byte[] _byte = this.zjh_room.encode();
			int len = _byte.Length;
			bytes[0] = ByteBuffer.allocate(4 + len);
			bytes[0].putInt(len);
			bytes[0].put(_byte);
			total += bytes[0].limit();
		}

		if(this.hasNickname()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.nickname);
			    short len = (short) _byte.Length;
			    bytes[1] = ByteBuffer.allocate(2 + len);
			    bytes[1].putShort(len);
				bytes[1].put(_byte);
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
		  
		if(this.hasZjhRoom()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.zjh_room = ROOM.decode(bytes);
		}

		if(this.hasNickname()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.nickname = System.Text.Encoding.UTF8.GetString(bytes);
		}

	} 

	public bool hasZjhRoom() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasNickname() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

