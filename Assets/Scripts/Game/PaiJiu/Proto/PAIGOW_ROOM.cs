//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:30 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_ROOM { 

	public const int CODE = 5001; 

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

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	private List<PAIGOW_SEAT> paigow_seat = new List<PAIGOW_SEAT>(); 

	public PAIGOW_SEAT getPaigowSeat(int index) { 
			return this.paigow_seat[index];
	} 
	
	public void addPaigowSeat(PAIGOW_SEAT value) { 
			if(!this.hasPaigowSeat()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this.paigow_seat.Add(value);
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

	private ROOM_MODEL _room_model; 

	public ROOM_MODEL room_model { 
		set { 
			if(!this.hasRoomModel()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this._room_model = value;
		} 
		get { 
			return this._room_model;
		} 
	} 

	private int _remainMahjong; 

	public int remainMahjong { 
		set { 
			if(!this.hasRemainMahjong()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this._remainMahjong = value;
		} 
		get { 
			return this._remainMahjong;
		} 
	} 

	private bool _loopEnd; 

	public bool loopEnd { 
		set { 
			if(!this.hasLoopEnd()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 2);
			}
			this._loopEnd = value;
		} 
		get { 
			return this._loopEnd;
		} 
	} 

	private int _chooseBankerPos; 

	public int chooseBankerPos { 
		set { 
			if(!this.hasChooseBankerPos()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 4);
			}
			this._chooseBankerPos = value;
		} 
		get { 
			return this._chooseBankerPos;
		} 
	} 

	private int _mahJongSum; 

	public int mahJongSum { 
		set { 
			if(!this.hasMahJongSum()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 8);
			}
			this._mahJongSum = value;
		} 
		get { 
			return this._mahJongSum;
		} 
	} 

	private List<int> operatePosList = new List<int>(); 

	public int getOperatePosList(int index) { 
			return this.operatePosList[index];
	} 
	
	public void addOperatePosList(int value) { 
			if(!this.hasOperatePosList()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 16);
			}
			this.operatePosList.Add(value);
	} 

	private int _dealTime; 

	public int dealTime { 
		set { 
			if(!this.hasDealTime()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 32);
			}
			this._dealTime = value;
		} 
		get { 
			return this._dealTime;
		} 
	} 

	private bool _isCutPan; 

	public bool isCutPan { 
		set { 
			if(!this.hasIsCutPan()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 64);
			}
			this._isCutPan = value;
		} 
		get { 
			return this._isCutPan;
		} 
	} 

	private bool _isBombPan; 

	public bool isBombPan { 
		set { 
			if(!this.hasIsBombPan()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 128);
			}
			this._isBombPan = value;
		} 
		get { 
			return this._isBombPan;
		} 
	} 

	private List<PAIGOW_MAHJONG> mahjongs＿remain = new List<PAIGOW_MAHJONG>(); 

	public PAIGOW_MAHJONG getMahjongs＿remain(int index) { 
			return this.mahjongs＿remain[index];
	} 
	
	public void addMahjongs＿remain(PAIGOW_MAHJONG value) { 
			if(!this.hasMahjongs＿remain()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 1);
			}
			this.mahjongs＿remain.Add(value);
	} 

	public static PAIGOW_ROOM newBuilder() { 
		return new PAIGOW_ROOM(); 
	} 

	public static PAIGOW_ROOM decode(byte[] data) { 
		PAIGOW_ROOM proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[17]; 

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

		if(this.hasUnixtime()) {
			bytes[4] = ByteBuffer.allocate(8);
			bytes[4].putLong(this.unixtime);
			total += bytes[4].limit();
		}

		if(this.hasPaigowSeat()) {
				int length = 0;
				for(int i=0, len=this.paigow_seat.Count; i<len; i++) {
					length += this.paigow_seat[i].encode().Length;
				}
				bytes[5] = ByteBuffer.allocate(this.paigow_seat.Count * 4 + length + 2);
				bytes[5].putShort((short) this.paigow_seat.Count);
				for(int i=0, len=this.paigow_seat.Count; i<len; i++) {
					byte[] _byte = this.paigow_seat[i].encode();
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

		if(this.hasRoomModel()) {
			bytes[7] = ByteBuffer.allocate(1);
			bytes[7].put((byte) this.room_model);
			total += bytes[7].limit();
		}

		if(this.hasRemainMahjong()) {
			bytes[8] = ByteBuffer.allocate(4);
			bytes[8].putInt(this.remainMahjong);
			total += bytes[8].limit();
		}

		if(this.hasLoopEnd()) {
			bytes[9] = ByteBuffer.allocate(1);
			if(this.loopEnd) {
				bytes[9].put((byte) 1);
			}else{
				bytes[9].put((byte) 0);
			}
			total += bytes[9].limit();
		}

		if(this.hasChooseBankerPos()) {
			bytes[10] = ByteBuffer.allocate(4);
			bytes[10].putInt(this.chooseBankerPos);
			total += bytes[10].limit();
		}

		if(this.hasMahJongSum()) {
			bytes[11] = ByteBuffer.allocate(4);
			bytes[11].putInt(this.mahJongSum);
			total += bytes[11].limit();
		}

		if(this.hasOperatePosList()) {
			bytes[12] = ByteBuffer.allocate(this.operatePosList.Count * 4 + 2);
			bytes[12].putShort((short) this.operatePosList.Count);
			for(int i=0, len=this.operatePosList.Count; i<len; i++) {
				bytes[12].putInt(this.operatePosList[i]);
			}
			total += bytes[12].limit();
		}

		if(this.hasDealTime()) {
			bytes[13] = ByteBuffer.allocate(4);
			bytes[13].putInt(this.dealTime);
			total += bytes[13].limit();
		}

		if(this.hasIsCutPan()) {
			bytes[14] = ByteBuffer.allocate(1);
			if(this.isCutPan) {
				bytes[14].put((byte) 1);
			}else{
				bytes[14].put((byte) 0);
			}
			total += bytes[14].limit();
		}

		if(this.hasIsBombPan()) {
			bytes[15] = ByteBuffer.allocate(1);
			if(this.isBombPan) {
				bytes[15].put((byte) 1);
			}else{
				bytes[15].put((byte) 0);
			}
			total += bytes[15].limit();
		}

		if(this.hasMahjongs＿remain()) {
				int length = 0;
				for(int i=0, len=this.mahjongs＿remain.Count; i<len; i++) {
					length += this.mahjongs＿remain[i].encode().Length;
				}
				bytes[16] = ByteBuffer.allocate(this.mahjongs＿remain.Count * 4 + length + 2);
				bytes[16].putShort((short) this.mahjongs＿remain.Count);
				for(int i=0, len=this.mahjongs＿remain.Count; i<len; i++) {
					byte[] _byte = this.mahjongs＿remain[i].encode();
					bytes[16].putInt(_byte.Length);
					bytes[16].put(_byte);
				}
			total += bytes[16].limit();
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

		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
		}

		if(this.hasPaigowSeat()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.paigow_seat.Add(PAIGOW_SEAT.decode(bytes));
			}
		}

		if(this.hasRoomStatus()) {
			this.room_status = (ROOM_STATUS) buf.get();
		}

		if(this.hasRoomModel()) {
			this.room_model = (ROOM_MODEL) buf.get();
		}

		if(this.hasRemainMahjong()) {
			this.remainMahjong = buf.getInt();
		}

		if(this.hasLoopEnd()) {
			if(buf.get() == 1) {
				this.loopEnd = true;
			}else{
				this.loopEnd = false;
			}
		}

		if(this.hasChooseBankerPos()) {
			this.chooseBankerPos = buf.getInt();
		}

		if(this.hasMahJongSum()) {
			this.mahJongSum = buf.getInt();
		}

		if(this.hasOperatePosList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.operatePosList.Add(buf.getInt());
			}
		}

		if(this.hasDealTime()) {
			this.dealTime = buf.getInt();
		}

		if(this.hasIsCutPan()) {
			if(buf.get() == 1) {
				this.isCutPan = true;
			}else{
				this.isCutPan = false;
			}
		}

		if(this.hasIsBombPan()) {
			if(buf.get() == 1) {
				this.isBombPan = true;
			}else{
				this.isBombPan = false;
			}
		}

		if(this.hasMahjongs＿remain()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.mahjongs＿remain.Add(PAIGOW_MAHJONG.decode(bytes));
			}
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

	public bool hasUnixtime() {
		return (this.__flag[0] & 16) != 0;
	}

	public int paigowSeatCount() {
		return this.paigow_seat.Count;
	}

	public bool hasPaigowSeat() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasRoomStatus() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasRoomModel() {
		return (this.__flag[0] & 128) != 0;
	}

	public bool hasRemainMahjong() {
		return (this.__flag[1] & 1) != 0;
	}

	public bool hasLoopEnd() {
		return (this.__flag[1] & 2) != 0;
	}

	public bool hasChooseBankerPos() {
		return (this.__flag[1] & 4) != 0;
	}

	public bool hasMahJongSum() {
		return (this.__flag[1] & 8) != 0;
	}

	public int operatePosListCount() {
		return this.operatePosList.Count;
	}

	public bool hasOperatePosList() {
		return (this.__flag[1] & 16) != 0;
	}

	public bool hasDealTime() {
		return (this.__flag[1] & 32) != 0;
	}

	public bool hasIsCutPan() {
		return (this.__flag[1] & 64) != 0;
	}

	public bool hasIsBombPan() {
		return (this.__flag[1] & 128) != 0;
	}

	public int mahjongs＿remainCount() {
		return this.mahjongs＿remain.Count;
	}

	public bool hasMahjongs＿remain() {
		return (this.__flag[2] & 1) != 0;
	}

	public List<int> getSettingIdList() {
		return this.settingId;
	}

	public List<PAIGOW_SEAT> getPaigowSeatList() {
		return this.paigow_seat;
	}

	public List<int> getOperatePosListList() {
		return this.operatePosList;
	}

	public List<PAIGOW_MAHJONG> getMahjongs＿remainList() {
		return this.mahjongs＿remain;
	}

}
}

