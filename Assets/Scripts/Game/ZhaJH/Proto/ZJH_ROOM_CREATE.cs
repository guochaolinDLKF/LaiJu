//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:46 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ZJH_ROOM_CREATE { 

	public const int CODE = 401028; 

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

	private List<PLAYER> player = new List<PLAYER>(); 

	public PLAYER getPlayer(int index) { 
			return this.player[index];
	} 
	
	public void addPlayer(PLAYER value) { 
			if(!this.hasPlayer()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this.player.Add(value);
	} 

	public static ZJH_ROOM_CREATE newBuilder() { 
		return new ZJH_ROOM_CREATE(); 
	} 

	public static ZJH_ROOM_CREATE decode(byte[] data) { 
		ZJH_ROOM_CREATE proto = newBuilder();
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

		if(this.hasPlayer()) {
				int length = 0;
				for(int i=0, len=this.player.Count; i<len; i++) {
					length += this.player[i].encode().Length;
				}
				bytes[1] = ByteBuffer.allocate(this.player.Count * 4 + length + 2);
				bytes[1].putShort((short) this.player.Count);
				for(int i=0, len=this.player.Count; i<len; i++) {
					byte[] _byte = this.player[i].encode();
					bytes[1].putInt(_byte.Length);
					bytes[1].put(_byte);
				}
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

		if(this.hasPlayer()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.player.Add(PLAYER.decode(bytes));
			}
		}

	} 

	public bool hasZjhRoom() {
		return (this.__flag[0] & 1) != 0;
	}

	public int playerCount() {
		return this.player.Count;
	}

	public bool hasPlayer() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<PLAYER> getPlayerList() {
		return this.player;
	}

}
}

