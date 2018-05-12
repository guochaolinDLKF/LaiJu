//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 5:30:44 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.sss {

public class ROOM_INFO { 

	public const int CODE = 1; 

	private byte[] __flag = new byte[2]; 

	private List<int> settingId = new List<int>(); 

	public int getSettingId(int index) { 
			return this.settingId[index];
	} 
	
	public void addSettingId(int value) { 
			if(!this.hasSettingId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.settingId.Add(value);
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

	private long _dismissTime; 

	public long dismissTime { 
		set { 
			if(!this.hasDismissTime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._dismissTime = value;
		} 
		get { 
			return this._dismissTime;
		} 
	} 

	private long _dismissMaxTime; 

	public long dismissMaxTime { 
		set { 
			if(!this.hasDismissMaxTime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._dismissMaxTime = value;
		} 
		get { 
			return this._dismissMaxTime;
		} 
	} 

	private int _ownerId; 

	public int ownerId { 
		set { 
			if(!this.hasOwnerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._ownerId = value;
		} 
		get { 
			return this._ownerId;
		} 
	} 

	private List<SEAT_INFO> seatInfo = new List<SEAT_INFO>(); 

	public SEAT_INFO getSeatInfo(int index) { 
			return this.seatInfo[index];
	} 
	
	public void addSeatInfo(SEAT_INFO value) { 
			if(!this.hasSeatInfo()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this.seatInfo.Add(value);
	} 

	private int _clubId; 

	public int clubId { 
		set { 
			if(!this.hasClubId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this._clubId = value;
		} 
		get { 
			return this._clubId;
		} 
	} 

	private int _maxLoop; 

	public int maxLoop { 
		set { 
			if(!this.hasMaxLoop()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this._maxLoop = value;
		} 
		get { 
			return this._maxLoop;
		} 
	} 

	private int _baseScore; 

	public int baseScore { 
		set { 
			if(!this.hasBaseScore()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 2);
			}
			this._baseScore = value;
		} 
		get { 
			return this._baseScore;
		} 
	} 

	private ROOM_STATUS _roomStatus; 

	public ROOM_STATUS roomStatus { 
		set { 
			if(!this.hasRoomStatus()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 4);
			}
			this._roomStatus = value;
		} 
		get { 
			return this._roomStatus;
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

		ByteBuffer[] bytes = new ByteBuffer[11]; 

		int total = 0;
		if(this.hasSettingId()) {
			bytes[0] = ByteBuffer.allocate(this.settingId.Count * 4 + 2);
			bytes[0].putShort((short) this.settingId.Count);
			for(int i=0, len=this.settingId.Count; i<len; i++) {
				bytes[0].putInt(this.settingId[i]);
			}
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

		if(this.hasDismissTime()) {
			bytes[3] = ByteBuffer.allocate(8);
			bytes[3].putLong(this.dismissTime);
			total += bytes[3].limit();
		}

		if(this.hasDismissMaxTime()) {
			bytes[4] = ByteBuffer.allocate(8);
			bytes[4].putLong(this.dismissMaxTime);
			total += bytes[4].limit();
		}

		if(this.hasOwnerId()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putInt(this.ownerId);
			total += bytes[5].limit();
		}

		if(this.hasSeatInfo()) {
				int length = 0;
				for(int i=0, len=this.seatInfo.Count; i<len; i++) {
					length += this.seatInfo[i].encode().Length;
				}
				bytes[6] = ByteBuffer.allocate(this.seatInfo.Count * 4 + length + 2);
				bytes[6].putShort((short) this.seatInfo.Count);
				for(int i=0, len=this.seatInfo.Count; i<len; i++) {
					byte[] _byte = this.seatInfo[i].encode();
					bytes[6].putInt(_byte.Length);
					bytes[6].put(_byte);
				}
			total += bytes[6].limit();
		}

		if(this.hasClubId()) {
			bytes[7] = ByteBuffer.allocate(4);
			bytes[7].putInt(this.clubId);
			total += bytes[7].limit();
		}

		if(this.hasMaxLoop()) {
			bytes[8] = ByteBuffer.allocate(4);
			bytes[8].putInt(this.maxLoop);
			total += bytes[8].limit();
		}

		if(this.hasBaseScore()) {
			bytes[9] = ByteBuffer.allocate(4);
			bytes[9].putInt(this.baseScore);
			total += bytes[9].limit();
		}

		if(this.hasRoomStatus()) {
			bytes[10] = ByteBuffer.allocate(1);
			bytes[10].put((byte) this.roomStatus);
			total += bytes[10].limit();
		}

	
		ByteBuffer buf = ByteBuffer.allocate(2 + total);
	
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
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.settingId.Add(buf.getInt());
			}
		}

		if(this.hasRoomId()) {
			this.roomId = buf.getInt();
		}

		if(this.hasLoop()) {
			this.loop = buf.getInt();
		}

		if(this.hasDismissTime()) {
			this.dismissTime = buf.getLong();
		}

		if(this.hasDismissMaxTime()) {
			this.dismissMaxTime = buf.getLong();
		}

		if(this.hasOwnerId()) {
			this.ownerId = buf.getInt();
		}

		if(this.hasSeatInfo()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.seatInfo.Add(SEAT_INFO.decode(bytes));
			}
		}

		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

		if(this.hasMaxLoop()) {
			this.maxLoop = buf.getInt();
		}

		if(this.hasBaseScore()) {
			this.baseScore = buf.getInt();
		}

		if(this.hasRoomStatus()) {
			this.roomStatus = (ROOM_STATUS) buf.get();
		}

	} 

	public int settingIdCount() {
		return this.settingId.Count;
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

	public bool hasDismissTime() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasDismissMaxTime() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasOwnerId() {
		return (this.__flag[0] & 32) != 0;
	}

	public int seatInfoCount() {
		return this.seatInfo.Count;
	}

	public bool hasSeatInfo() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasClubId() {
		return (this.__flag[0] & 128) != 0;
	}

	public bool hasMaxLoop() {
		return (this.__flag[1] & 1) != 0;
	}

	public bool hasBaseScore() {
		return (this.__flag[1] & 2) != 0;
	}

	public bool hasRoomStatus() {
		return (this.__flag[1] & 4) != 0;
	}

	public List<int> getSettingIdList() {
		return this.settingId;
	}

	public List<SEAT_INFO> getSeatInfoList() {
		return this.seatInfo;
	}

}
}

