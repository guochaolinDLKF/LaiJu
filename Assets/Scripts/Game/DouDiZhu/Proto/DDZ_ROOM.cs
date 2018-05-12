using System.Collections.Generic;

namespace ddz.proto {

public class DDZ_ROOM { 

	public const int CODE = 3001; 

	private byte[] __flag = new byte[16]; 

	private int _roomId; 

	public int roomId { 
		set { 
			if(!this.hasRoomId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._loop = value;
		} 
		get { 
			return this._loop;
		} 
	} 

	private int _baseScore; 

	public int baseScore { 
		set { 
			if(!this.hasBaseScore()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._baseScore = value;
		} 
		get { 
			return this._baseScore;
		} 
	} 

	private int _scores; 

	public int scores { 
		set { 
			if(!this.hasScores()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._scores = value;
		} 
		get { 
			return this._scores;
		} 
	} 

	private ROOM_STATUS _status; 

	public ROOM_STATUS status { 
		set { 
			if(!this.hasStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._status = value;
		} 
		get { 
			return this._status;
		} 
	} 

	private int _maxLoop; 

	public int maxLoop { 
		set { 
			if(!this.hasMaxLoop()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._maxLoop = value;
		} 
		get { 
			return this._maxLoop;
		} 
	} 

	private int _times; 

	public int times { 
		set { 
			if(!this.hasTimes()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._times = value;
		} 
		get { 
			return this._times;
		} 
	} 

	private int _dizhu; 

	public int dizhu { 
		set { 
			if(!this.hasDizhu()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this._dizhu = value;
		} 
		get { 
			return this._dizhu;
		} 
	} 

	private List<DDZ_SEAT> seatList = new List<DDZ_SEAT>(); 

	public DDZ_SEAT getSeatList(int index) { 
			return this.seatList[index];
	} 
	
	public void addSeatList(DDZ_SEAT value) { 
			if(!this.hasSeatList()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this.seatList.Add(value);
	} 

	private List<int> settingId = new List<int>(); 

	public int getSettingId(int index) { 
			return this.settingId[index];
	} 
	
	public void addSettingId(int value) { 
			if(!this.hasSettingId()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 2);
			}
			this.settingId.Add(value);
	} 

	private int _currentPlayPokerPos; 

	public int currentPlayPokerPos { 
		set { 
			if(!this.hasCurrentPlayPokerPos()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 4);
			}
			this._currentPlayPokerPos = value;
		} 
		get { 
			return this._currentPlayPokerPos;
		} 
	} 

	private List<DDZ_POCKER> basePokerList = new List<DDZ_POCKER>(); 

	public DDZ_POCKER getBasePokerList(int index) { 
			return this.basePokerList[index];
	} 
	
	public void addBasePokerList(DDZ_POCKER value) { 
			if(!this.hasBasePokerList()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 8);
			}
			this.basePokerList.Add(value);
	} 

	private int _ownerId; 

	public int ownerId { 
		set { 
			if(!this.hasOwnerId()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 16);
			}
			this._ownerId = value;
		} 
		get { 
			return this._ownerId;
		} 
	} 

	private int _currentQiangPlayerId; 

	public int currentQiangPlayerId { 
		set { 
			if(!this.hasCurrentQiangPlayerId()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 32);
			}
			this._currentQiangPlayerId = value;
		} 
		get { 
			return this._currentQiangPlayerId;
		} 
	} 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 64);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	public static DDZ_ROOM newBuilder() { 
		return new DDZ_ROOM(); 
	} 

	public static DDZ_ROOM decode(byte[] data) { 
		DDZ_ROOM proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[15]; 

		int total = 0;
		if(this.hasRoomId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.roomId);
			total += bytes[0].limit();
		}

		if(this.hasLoop()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.loop);
			total += bytes[1].limit();
		}

		if(this.hasBaseScore()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.baseScore);
			total += bytes[2].limit();
		}

		if(this.hasScores()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.scores);
			total += bytes[3].limit();
		}

		if(this.hasStatus()) {
			bytes[4] = ByteBuffer.allocate(1);
			bytes[4].put((byte) this.status);
			total += bytes[4].limit();
		}

		if(this.hasMaxLoop()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putInt(this.maxLoop);
			total += bytes[5].limit();
		}

		if(this.hasTimes()) {
			bytes[6] = ByteBuffer.allocate(4);
			bytes[6].putInt(this.times);
			total += bytes[6].limit();
		}

		if(this.hasDizhu()) {
			bytes[7] = ByteBuffer.allocate(4);
			bytes[7].putInt(this.dizhu);
			total += bytes[7].limit();
		}

		if(this.hasSeatList()) {
				int length = 0;
				for(int i=0, len=this.seatList.Count; i<len; i++) {
					length += this.seatList[i].encode().Length;
				}
				bytes[8] = ByteBuffer.allocate(this.seatList.Count * 4 + length + 2);
				bytes[8].putShort((short) this.seatList.Count);
				for(int i=0, len=this.seatList.Count; i<len; i++) {
					byte[] _byte = this.seatList[i].encode();
					bytes[8].putInt(_byte.Length);
					bytes[8].put(_byte);
				}
			total += bytes[8].limit();
		}

		if(this.hasSettingId()) {
			bytes[9] = ByteBuffer.allocate(this.settingId.Count * 4 + 2);
			bytes[9].putShort((short) this.settingId.Count);
			for(int i=0, len=this.settingId.Count; i<len; i++) {
				bytes[9].putInt(this.settingId[i]);
			}
			total += bytes[9].limit();
		}

		if(this.hasCurrentPlayPokerPos()) {
			bytes[10] = ByteBuffer.allocate(4);
			bytes[10].putInt(this.currentPlayPokerPos);
			total += bytes[10].limit();
		}

		if(this.hasBasePokerList()) {
				int length = 0;
				for(int i=0, len=this.basePokerList.Count; i<len; i++) {
					length += this.basePokerList[i].encode().Length;
				}
				bytes[11] = ByteBuffer.allocate(this.basePokerList.Count * 4 + length + 2);
				bytes[11].putShort((short) this.basePokerList.Count);
				for(int i=0, len=this.basePokerList.Count; i<len; i++) {
					byte[] _byte = this.basePokerList[i].encode();
					bytes[11].putInt(_byte.Length);
					bytes[11].put(_byte);
				}
			total += bytes[11].limit();
		}

		if(this.hasOwnerId()) {
			bytes[12] = ByteBuffer.allocate(4);
			bytes[12].putInt(this.ownerId);
			total += bytes[12].limit();
		}

		if(this.hasCurrentQiangPlayerId()) {
			bytes[13] = ByteBuffer.allocate(4);
			bytes[13].putInt(this.currentQiangPlayerId);
			total += bytes[13].limit();
		}

		if(this.hasUnixtime()) {
			bytes[14] = ByteBuffer.allocate(8);
			bytes[14].putLong(this.unixtime);
			total += bytes[14].limit();
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
		  
		if(this.hasRoomId()) {
			this.roomId = buf.getInt();
		}

		if(this.hasLoop()) {
			this.loop = buf.getInt();
		}

		if(this.hasBaseScore()) {
			this.baseScore = buf.getInt();
		}

		if(this.hasScores()) {
			this.scores = buf.getInt();
		}

		if(this.hasStatus()) {
			this.status = (ROOM_STATUS) buf.get();
		}

		if(this.hasMaxLoop()) {
			this.maxLoop = buf.getInt();
		}

		if(this.hasTimes()) {
			this.times = buf.getInt();
		}

		if(this.hasDizhu()) {
			this.dizhu = buf.getInt();
		}

		if(this.hasSeatList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.seatList.Add(DDZ_SEAT.decode(bytes));
			}
		}

		if(this.hasSettingId()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.settingId.Add(buf.getInt());
			}
		}

		if(this.hasCurrentPlayPokerPos()) {
			this.currentPlayPokerPos = buf.getInt();
		}

		if(this.hasBasePokerList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.basePokerList.Add(DDZ_POCKER.decode(bytes));
			}
		}

		if(this.hasOwnerId()) {
			this.ownerId = buf.getInt();
		}

		if(this.hasCurrentQiangPlayerId()) {
			this.currentQiangPlayerId = buf.getInt();
		}

		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
		}

	} 

	public bool hasRoomId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasLoop() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasBaseScore() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasScores() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasStatus() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasMaxLoop() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasTimes() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasDizhu() {
		return (this.__flag[0] & 128) != 0;
	}

	public int seatListCount() {
		return this.seatList.Count;
	}

	public bool hasSeatList() {
		return (this.__flag[1] & 1) != 0;
	}

	public int settingIdCount() {
		return this.settingId.Count;
	}

	public bool hasSettingId() {
		return (this.__flag[1] & 2) != 0;
	}

	public bool hasCurrentPlayPokerPos() {
		return (this.__flag[1] & 4) != 0;
	}

	public int basePokerListCount() {
		return this.basePokerList.Count;
	}

	public bool hasBasePokerList() {
		return (this.__flag[1] & 8) != 0;
	}

	public bool hasOwnerId() {
		return (this.__flag[1] & 16) != 0;
	}

	public bool hasCurrentQiangPlayerId() {
		return (this.__flag[1] & 32) != 0;
	}

	public bool hasUnixtime() {
		return (this.__flag[1] & 64) != 0;
	}

	public List<DDZ_SEAT> getSeatListList() {
		return this.seatList;
	}

	public List<int> getSettingIdList() {
		return this.settingId;
	}

	public List<DDZ_POCKER> getBasePokerListList() {
		return this.basePokerList;
	}

}
}

