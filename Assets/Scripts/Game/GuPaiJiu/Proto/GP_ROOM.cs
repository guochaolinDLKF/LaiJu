//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:24 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class GP_ROOM { 

	public const int CODE = 7003; 

	private byte[] __flag = new byte[3]; 

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

	private int _firstDice; 

	public int firstDice { 
		set { 
			if(!this.hasFirstDice()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._firstDice = value;
		} 
		get { 
			return this._firstDice;
		} 
	} 

	private int _secondDice; 

	public int secondDice { 
		set { 
			if(!this.hasSecondDice()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._secondDice = value;
		} 
		get { 
			return this._secondDice;
		} 
	} 

	private ROOM_STATUS _status; 

	public ROOM_STATUS status { 
		set { 
			if(!this.hasStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._status = value;
		} 
		get { 
			return this._status;
		} 
	} 

	private List<GP_SEAT> seatList = new List<GP_SEAT>(); 

	public GP_SEAT getSeatList(int index) { 
			return this.seatList[index];
	} 
	
	public void addSeatList(GP_SEAT value) { 
			if(!this.hasSeatList()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this.seatList.Add(value);
	} 

	private int _firstGivePos; 

	public int firstGivePos { 
		set { 
			if(!this.hasFirstGivePos()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this._firstGivePos = value;
		} 
		get { 
			return this._firstGivePos;
		} 
	} 

	private int _totalPokerNum; 

	public int totalPokerNum { 
		set { 
			if(!this.hasTotalPokerNum()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 2);
			}
			this._totalPokerNum = value;
		} 
		get { 
			return this._totalPokerNum;
		} 
	} 

	private int _remainPokerNum; 

	public int remainPokerNum { 
		set { 
			if(!this.hasRemainPokerNum()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 4);
			}
			this._remainPokerNum = value;
		} 
		get { 
			return this._remainPokerNum;
		} 
	} 

	private int _panBase; 

	public int panBase { 
		set { 
			if(!this.hasPanBase()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 8);
			}
			this._panBase = value;
		} 
		get { 
			return this._panBase;
		} 
	} 

	private bool _isAddPanBase; 

	public bool isAddPanBase { 
		set { 
			if(!this.hasIsAddPanBase()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 16);
			}
			this._isAddPanBase = value;
		} 
		get { 
			return this._isAddPanBase;
		} 
	} 

	private long _groupPokerTime; 

	public long groupPokerTime { 
		set { 
			if(!this.hasGroupPokerTime()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 32);
			}
			this._groupPokerTime = value;
		} 
		get { 
			return this._groupPokerTime;
		} 
	} 

	private List<GP_POKER> historyPokerList = new List<GP_POKER>(); 

	public GP_POKER getHistoryPokerList(int index) { 
			return this.historyPokerList[index];
	} 
	
	public void addHistoryPokerList(GP_POKER value) { 
			if(!this.hasHistoryPokerList()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 64);
			}
			this.historyPokerList.Add(value);
	} 

	private bool _loopEnd; 

	public bool loopEnd { 
		set { 
			if(!this.hasLoopEnd()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 128);
			}
			this._loopEnd = value;
		} 
		get { 
			return this._loopEnd;
		} 
	} 

	private int _dealTime; 

	public int dealTime { 
		set { 
			if(!this.hasDealTime()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 1);
			}
			this._dealTime = value;
		} 
		get { 
			return this._dealTime;
		} 
	} 

	private int _ownerId; 

	public int ownerId { 
		set { 
			if(!this.hasOwnerId()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 2);
			}
			this._ownerId = value;
		} 
		get { 
			return this._ownerId;
		} 
	} 

	private string _ownerNickName; 

	public string ownerNickName { 
		set { 
			if(!this.hasOwnerNickName()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 4);
			}
			this._ownerNickName = value;
		} 
		get { 
			return this._ownerNickName;
		} 
	} 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 8);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	public static GP_ROOM newBuilder() { 
		return new GP_ROOM(); 
	} 

	public static GP_ROOM decode(byte[] data) { 
		GP_ROOM proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[20]; 

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

		if(this.hasMaxLoop()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.maxLoop);
			total += bytes[3].limit();
		}

		if(this.hasFirstDice()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.firstDice);
			total += bytes[4].limit();
		}

		if(this.hasSecondDice()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putInt(this.secondDice);
			total += bytes[5].limit();
		}

		if(this.hasStatus()) {
			bytes[6] = ByteBuffer.allocate(1);
			bytes[6].put((byte) this.status);
			total += bytes[6].limit();
		}

		if(this.hasSeatList()) {
				int length = 0;
				for(int i=0, len=this.seatList.Count; i<len; i++) {
					length += this.seatList[i].encode().Length;
				}
				bytes[7] = ByteBuffer.allocate(this.seatList.Count * 4 + length + 2);
				bytes[7].putShort((short) this.seatList.Count);
				for(int i=0, len=this.seatList.Count; i<len; i++) {
					byte[] _byte = this.seatList[i].encode();
					bytes[7].putInt(_byte.Length);
					bytes[7].put(_byte);
				}
			total += bytes[7].limit();
		}

		if(this.hasFirstGivePos()) {
			bytes[8] = ByteBuffer.allocate(4);
			bytes[8].putInt(this.firstGivePos);
			total += bytes[8].limit();
		}

		if(this.hasTotalPokerNum()) {
			bytes[9] = ByteBuffer.allocate(4);
			bytes[9].putInt(this.totalPokerNum);
			total += bytes[9].limit();
		}

		if(this.hasRemainPokerNum()) {
			bytes[10] = ByteBuffer.allocate(4);
			bytes[10].putInt(this.remainPokerNum);
			total += bytes[10].limit();
		}

		if(this.hasPanBase()) {
			bytes[11] = ByteBuffer.allocate(4);
			bytes[11].putInt(this.panBase);
			total += bytes[11].limit();
		}

		if(this.hasIsAddPanBase()) {
			bytes[12] = ByteBuffer.allocate(1);
			if(this.isAddPanBase) {
				bytes[12].put((byte) 1);
			}else{
				bytes[12].put((byte) 0);
			}
			total += bytes[12].limit();
		}

		if(this.hasGroupPokerTime()) {
			bytes[13] = ByteBuffer.allocate(8);
			bytes[13].putLong(this.groupPokerTime);
			total += bytes[13].limit();
		}

		if(this.hasHistoryPokerList()) {
				int length = 0;
				for(int i=0, len=this.historyPokerList.Count; i<len; i++) {
					length += this.historyPokerList[i].encode().Length;
				}
				bytes[14] = ByteBuffer.allocate(this.historyPokerList.Count * 4 + length + 2);
				bytes[14].putShort((short) this.historyPokerList.Count);
				for(int i=0, len=this.historyPokerList.Count; i<len; i++) {
					byte[] _byte = this.historyPokerList[i].encode();
					bytes[14].putInt(_byte.Length);
					bytes[14].put(_byte);
				}
			total += bytes[14].limit();
		}

		if(this.hasLoopEnd()) {
			bytes[15] = ByteBuffer.allocate(1);
			if(this.loopEnd) {
				bytes[15].put((byte) 1);
			}else{
				bytes[15].put((byte) 0);
			}
			total += bytes[15].limit();
		}

		if(this.hasDealTime()) {
			bytes[16] = ByteBuffer.allocate(4);
			bytes[16].putInt(this.dealTime);
			total += bytes[16].limit();
		}

		if(this.hasOwnerId()) {
			bytes[17] = ByteBuffer.allocate(4);
			bytes[17].putInt(this.ownerId);
			total += bytes[17].limit();
		}

		if(this.hasOwnerNickName()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.ownerNickName);
			    short len = (short) _byte.Length;
			    bytes[18] = ByteBuffer.allocate(2 + len);
			    bytes[18].putShort(len);
				bytes[18].put(_byte);
			total += bytes[18].limit();
		}

		if(this.hasUnixtime()) {
			bytes[19] = ByteBuffer.allocate(8);
			bytes[19].putLong(this.unixtime);
			total += bytes[19].limit();
		}

	
		ByteBuffer buf = ByteBuffer.allocate(3 + total);
	
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

		if(this.hasMaxLoop()) {
			this.maxLoop = buf.getInt();
		}

		if(this.hasFirstDice()) {
			this.firstDice = buf.getInt();
		}

		if(this.hasSecondDice()) {
			this.secondDice = buf.getInt();
		}

		if(this.hasStatus()) {
			this.status = (ROOM_STATUS) buf.get();
		}

		if(this.hasSeatList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.seatList.Add(GP_SEAT.decode(bytes));
			}
		}

		if(this.hasFirstGivePos()) {
			this.firstGivePos = buf.getInt();
		}

		if(this.hasTotalPokerNum()) {
			this.totalPokerNum = buf.getInt();
		}

		if(this.hasRemainPokerNum()) {
			this.remainPokerNum = buf.getInt();
		}

		if(this.hasPanBase()) {
			this.panBase = buf.getInt();
		}

		if(this.hasIsAddPanBase()) {
			if(buf.get() == 1) {
				this.isAddPanBase = true;
			}else{
				this.isAddPanBase = false;
			}
		}

		if(this.hasGroupPokerTime()) {
			this.groupPokerTime = buf.getLong();
		}

		if(this.hasHistoryPokerList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.historyPokerList.Add(GP_POKER.decode(bytes));
			}
		}

		if(this.hasLoopEnd()) {
			if(buf.get() == 1) {
				this.loopEnd = true;
			}else{
				this.loopEnd = false;
			}
		}

		if(this.hasDealTime()) {
			this.dealTime = buf.getInt();
		}

		if(this.hasOwnerId()) {
			this.ownerId = buf.getInt();
		}

		if(this.hasOwnerNickName()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.ownerNickName = System.Text.Encoding.UTF8.GetString(bytes);
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

	public bool hasMaxLoop() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasFirstDice() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasSecondDice() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasStatus() {
		return (this.__flag[0] & 64) != 0;
	}

	public int seatListCount() {
		return this.seatList.Count;
	}

	public bool hasSeatList() {
		return (this.__flag[0] & 128) != 0;
	}

	public bool hasFirstGivePos() {
		return (this.__flag[1] & 1) != 0;
	}

	public bool hasTotalPokerNum() {
		return (this.__flag[1] & 2) != 0;
	}

	public bool hasRemainPokerNum() {
		return (this.__flag[1] & 4) != 0;
	}

	public bool hasPanBase() {
		return (this.__flag[1] & 8) != 0;
	}

	public bool hasIsAddPanBase() {
		return (this.__flag[1] & 16) != 0;
	}

	public bool hasGroupPokerTime() {
		return (this.__flag[1] & 32) != 0;
	}

	public int historyPokerListCount() {
		return this.historyPokerList.Count;
	}

	public bool hasHistoryPokerList() {
		return (this.__flag[1] & 64) != 0;
	}

	public bool hasLoopEnd() {
		return (this.__flag[1] & 128) != 0;
	}

	public bool hasDealTime() {
		return (this.__flag[2] & 1) != 0;
	}

	public bool hasOwnerId() {
		return (this.__flag[2] & 2) != 0;
	}

	public bool hasOwnerNickName() {
		return (this.__flag[2] & 4) != 0;
	}

	public bool hasUnixtime() {
		return (this.__flag[2] & 8) != 0;
	}

	public List<int> getSettingIdList() {
		return this.settingId;
	}

	public List<GP_SEAT> getSeatListList() {
		return this.seatList;
	}

	public List<GP_POKER> getHistoryPokerListList() {
		return this.historyPokerList;
	}

}
}

