//===================================================
//Author      : DRB
//CreateTime  ：11/6/2017 2:21:50 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace guandan.proto {

public class ROOM_INFO { 

	public const int CODE = 817; 

	private byte[] __flag = new byte[1]; 

	private int _settingId; 

	public int settingId { 
		set { 
			if(!this.hasSettingId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._settingId = value;
		} 
		get { 
			return this._settingId;
		} 
	} 

	private int _roomId; 

	public int roomId { 
		set { 
			if(!this.hasRoomId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._roomId = value;
		} 
		get { 
			return this._roomId;
		} 
	} 

	private int _loop; 

	public int loop { 
		set { 
			if(!this.hasLoop()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._loop = value;
		} 
		get { 
			return this._loop;
		} 
	} 

	private int _maxLoop; 

	public int maxLoop { 
		set { 
			if(!this.hasMaxLoop()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._maxLoop = value;
		} 
		get { 
			return this._maxLoop;
		} 
	} 

	private int _ownerId; 

	public int ownerId { 
		set { 
			if(!this.hasOwnerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._ownerId = value;
		} 
		get { 
			return this._ownerId;
		} 
	} 

	private List<SEAT_INFO> seatinfo = new List<SEAT_INFO>(); 

	public SEAT_INFO getSeatinfo(int index) { 
			return this.seatinfo[index];
	} 
	
	public void addSeatinfo(SEAT_INFO value) { 
			if(!this.hasSeatinfo()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this.seatinfo.Add(value);
	} 

	private ROOM_STATUS _room_status; 

	public ROOM_STATUS room_status { 
		set { 
			if(!this.hasRoomStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._room_status = value;
		} 
		get { 
			return this._room_status;
		} 
	} 

	public static ROOM_INFO newBuilder() { 
		return new ROOM_INFO(); 
	} 

	public static ROOM_INFO decode(byte[] data) { 
		ROOM_INFO proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[7]; 

		int total = 0;
		if(this.hasSettingId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.settingId);
			total += bytes[0].limit();
		}

		if(this.hasRoomId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.roomId);
			total += bytes[1].limit();
		}

		if(this.hasLoop()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.loop);
			total += bytes[2].limit();
		}

		if(this.hasMaxLoop()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.maxLoop);
			total += bytes[3].limit();
		}

		if(this.hasOwnerId()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.ownerId);
			total += bytes[4].limit();
		}

		if(this.hasSeatinfo()) {
				int length = 0;
				for(int i=0, len=this.seatinfo.Count; i<len; i++) {
					length += this.seatinfo[i].encode().Length;
				}
				bytes[5] = ByteBuffer.allocate(this.seatinfo.Count * 4 + length + 2);
				bytes[5].putShort((short) this.seatinfo.Count);
				for(int i=0, len=this.seatinfo.Count; i<len; i++) {
					byte[] _byte = this.seatinfo[i].encode();
					bytes[5].putInt(_byte.Length);
					bytes[5].put(_byte);
				}
			total += bytes[5].limit();
		}

		if(this.hasRoomStatus()) {
			bytes[6] = ByteBuffer.allocate(1);
			bytes[6].put((byte) this.room_status);
			total += bytes[6].limit();
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
		  
		if(this.hasSettingId()) {
			this.settingId = buf.getInt();
		}

		if(this.hasRoomId()) {
			this.roomId = buf.getInt();
		}

		if(this.hasLoop()) {
			this.loop = buf.getInt();
		}

		if(this.hasMaxLoop()) {
			this.maxLoop = buf.getInt();
		}

		if(this.hasOwnerId()) {
			this.ownerId = buf.getInt();
		}

		if(this.hasSeatinfo()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.seatinfo.Add(SEAT_INFO.decode(bytes));
			}
		}

		if(this.hasRoomStatus()) {
			this.room_status = (ROOM_STATUS) buf.get();
		}

	} 

	public bool hasSettingId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasRoomId() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasLoop() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasMaxLoop() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasOwnerId() {
		return (this.__flag[0] & 16) != 0;
	}

	public int seatinfoCount() {
		return this.seatinfo.Count;
	}

	public bool hasSeatinfo() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasRoomStatus() {
		return (this.__flag[0] & 64) != 0;
	}

	public List<SEAT_INFO> getSeatinfoList() {
		return this.seatinfo;
	}

}
}

