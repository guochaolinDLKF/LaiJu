//===================================================
//Author      : DRB
//CreateTime  ：1/16/2018 2:59:25 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.common {

public class OP_CLUB_ROOM_LIST { 

	public const int CODE = 99308; 

	private byte[] __flag = new byte[16]; 

	private List<OP_CLUB_ROOM> room = new List<OP_CLUB_ROOM>(); 

	public OP_CLUB_ROOM getRoom(int index) { 
			return this.room[index];
	} 
	
	public void addRoom(OP_CLUB_ROOM value) { 
			if(!this.hasRoom()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.room.Add(value);
	} 

	private int _clubId; 

	public int clubId { 
		set { 
			if(!this.hasClubId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._clubId = value;
		} 
		get { 
			return this._clubId;
		} 
	} 

	public static OP_CLUB_ROOM_LIST newBuilder() { 
		return new OP_CLUB_ROOM_LIST(); 
	} 

	public static OP_CLUB_ROOM_LIST decode(byte[] data) { 
		OP_CLUB_ROOM_LIST proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasRoom()) {
				int length = 0;
				for(int i=0, len=this.room.Count; i<len; i++) {
					length += this.room[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.room.Count * 4 + length + 2);
				bytes[0].putShort((short) this.room.Count);
				for(int i=0, len=this.room.Count; i<len; i++) {
					byte[] _byte = this.room[i].encode();
					bytes[0].putInt(_byte.Length);
					bytes[0].put(_byte);
				}
			total += bytes[0].limit();
		}

		if(this.hasClubId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.clubId);
			total += bytes[1].limit();
		}

	
		ByteBuffer buf = ByteBuffer.allocate(16 + total);
	
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
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.room.Add(OP_CLUB_ROOM.decode(bytes));
			}
		}

		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

	} 

	public int roomCount() {
		return this.room.Count;
	}

	public bool hasRoom() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasClubId() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<OP_CLUB_ROOM> getRoomList() {
		return this.room;
	}

}
}

