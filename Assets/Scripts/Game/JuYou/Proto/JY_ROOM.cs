using System.Collections.Generic;

namespace proto.jy {

public class JY_ROOM { 

	public const int CODE = 6003; 

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

	private ROOM_STATUS _status; 

	public ROOM_STATUS status { 
		set { 
			if(!this.hasStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._status = value;
		} 
		get { 
			return this._status;
		} 
	} 

	private List<JY_SEAT> seatList = new List<JY_SEAT>(); 

	public JY_SEAT getSeatList(int index) { 
			return this.seatList[index];
	} 
	
	public void addSeatList(JY_SEAT value) { 
			if(!this.hasSeatList()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this.seatList.Add(value);
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

	private int _baseScore; 

	public int baseScore { 
		set { 
			if(!this.hasBaseScore()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._baseScore = value;
		} 
		get { 
			return this._baseScore;
		} 
	} 

	private bool _isBomb; 

	public bool isBomb { 
		set { 
			if(!this.hasIsBomb()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this._isBomb = value;
		} 
		get { 
			return this._isBomb;
		} 
	} 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	public static JY_ROOM newBuilder() { 
		return new JY_ROOM(); 
	} 

	public static JY_ROOM decode(byte[] data) { 
		JY_ROOM proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[9]; 

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

		if(this.hasStatus()) {
			bytes[3] = ByteBuffer.allocate(1);
			bytes[3].put((byte) this.status);
			total += bytes[3].limit();
		}

		if(this.hasSeatList()) {
				int length = 0;
				for(int i=0, len=this.seatList.Count; i<len; i++) {
					length += this.seatList[i].encode().Length;
				}
				bytes[4] = ByteBuffer.allocate(this.seatList.Count * 4 + length + 2);
				bytes[4].putShort((short) this.seatList.Count);
				for(int i=0, len=this.seatList.Count; i<len; i++) {
					byte[] _byte = this.seatList[i].encode();
					bytes[4].putInt(_byte.Length);
					bytes[4].put(_byte);
				}
			total += bytes[4].limit();
		}

		if(this.hasMaxLoop()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putInt(this.maxLoop);
			total += bytes[5].limit();
		}

		if(this.hasBaseScore()) {
			bytes[6] = ByteBuffer.allocate(4);
			bytes[6].putInt(this.baseScore);
			total += bytes[6].limit();
		}

		if(this.hasIsBomb()) {
			bytes[7] = ByteBuffer.allocate(1);
			if(this.isBomb) {
				bytes[7].put((byte) 1);
			}else{
				bytes[7].put((byte) 0);
			}
			total += bytes[7].limit();
		}

		if(this.hasUnixtime()) {
			bytes[8] = ByteBuffer.allocate(8);
			bytes[8].putLong(this.unixtime);
			total += bytes[8].limit();
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

		if(this.hasStatus()) {
			this.status = (ROOM_STATUS) buf.get();
		}

		if(this.hasSeatList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.seatList.Add(JY_SEAT.decode(bytes));
			}
		}

		if(this.hasMaxLoop()) {
			this.maxLoop = buf.getInt();
		}

		if(this.hasBaseScore()) {
			this.baseScore = buf.getInt();
		}

		if(this.hasIsBomb()) {
			if(buf.get() == 1) {
				this.isBomb = true;
			}else{
				this.isBomb = false;
			}
		}

		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
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

	public bool hasStatus() {
		return (this.__flag[0] & 8) != 0;
	}

	public int seatListCount() {
		return this.seatList.Count;
	}

	public bool hasSeatList() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasMaxLoop() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasBaseScore() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasIsBomb() {
		return (this.__flag[0] & 128) != 0;
	}

	public bool hasUnixtime() {
		return (this.__flag[1] & 1) != 0;
	}

	public List<int> getSettingIdList() {
		return this.settingId;
	}

	public List<JY_SEAT> getSeatListList() {
		return this.seatList;
	}

}
}

