//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:43 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ROOM { 

	public const int CODE = 4003; 

	private byte[] __flag = new byte[2]; 

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

	private int _maxLoop; 

	public int maxLoop { 
		set { 
			if(!this.hasMaxLoop()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._maxLoop = value;
		} 
		get { 
			return this._maxLoop;
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

	private List<SEAT> seat = new List<SEAT>(); 

	public SEAT getSeat(int index) { 
			return this.seat[index];
	} 
	
	public void addSeat(SEAT value) { 
			if(!this.hasSeat()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this.seat.Add(value);
	} 

	private ENUM_ROOM_STATUS _roomstatus; 

	public ENUM_ROOM_STATUS roomstatus { 
		set { 
			if(!this.hasRoomstatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._roomstatus = value;
		} 
		get { 
			return this._roomstatus;
		} 
	} 

	private int _round; 

	public int round { 
		set { 
			if(!this.hasRound()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._round = value;
		} 
		get { 
			return this._round;
		} 
	} 

	private List<int> settingId = new List<int>(); 

	public int getSettingId(int index) { 
			return this.settingId[index];
	} 
	
	public void addSettingId(int value) { 
			if(!this.hasSettingId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this.settingId.Add(value);
	} 

	private float _baseScore; 

	public float baseScore { 
		set { 
			if(!this.hasBaseScore()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this._baseScore = value;
		} 
		get { 
			return this._baseScore;
		} 
	} 

	private float _RoomPour; 

	public float RoomPour { 
		set { 
			if(!this.hasRoomPour()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 2);
			}
			this._RoomPour = value;
		} 
		get { 
			return this._RoomPour;
		} 
	} 

	private List<SEAT_BILL> seat_bill = new List<SEAT_BILL>(); 

	public SEAT_BILL getSeatBill(int index) { 
			return this.seat_bill[index];
	} 
	
	public void addSeatBill(SEAT_BILL value) { 
			if(!this.hasSeatBill()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 4);
			}
			this.seat_bill.Add(value);
	} 

	private int _totalRound; 

	public int totalRound { 
		set { 
			if(!this.hasTotalRound()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 8);
			}
			this._totalRound = value;
		} 
		get { 
			return this._totalRound;
		} 
	} 

	private PLAYER _player; 

	public PLAYER player { 
		set { 
			if(!this.hasPlayer()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 16);
			}
			this._player = value;
		} 
		get { 
			return this._player;
		} 
	} 

	private int _clubId; 

	public int clubId { 
		set { 
			if(!this.hasClubId()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 32);
			}
			this._clubId = value;
		} 
		get { 
			return this._clubId;
		} 
	} 

	public static ROOM newBuilder() { 
		return new ROOM(); 
	} 

	public static ROOM decode(byte[] data) { 
		ROOM proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[14]; 

		int total = 0;
		if(this.hasRoomId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.roomId);
			total += bytes[0].limit();
		}

		if(this.hasMaxLoop()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.maxLoop);
			total += bytes[1].limit();
		}

		if(this.hasLoop()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.loop);
			total += bytes[2].limit();
		}

		if(this.hasScores()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.scores);
			total += bytes[3].limit();
		}

		if(this.hasSeat()) {
				int length = 0;
				for(int i=0, len=this.seat.Count; i<len; i++) {
					length += this.seat[i].encode().Length;
				}
				bytes[4] = ByteBuffer.allocate(this.seat.Count * 4 + length + 2);
				bytes[4].putShort((short) this.seat.Count);
				for(int i=0, len=this.seat.Count; i<len; i++) {
					byte[] _byte = this.seat[i].encode();
					bytes[4].putInt(_byte.Length);
					bytes[4].put(_byte);
				}
			total += bytes[4].limit();
		}

		if(this.hasRoomstatus()) {
			bytes[5] = ByteBuffer.allocate(1);
			bytes[5].put((byte) this.roomstatus);
			total += bytes[5].limit();
		}

		if(this.hasRound()) {
			bytes[6] = ByteBuffer.allocate(4);
			bytes[6].putInt(this.round);
			total += bytes[6].limit();
		}

		if(this.hasSettingId()) {
			bytes[7] = ByteBuffer.allocate(this.settingId.Count * 4 + 2);
			bytes[7].putShort((short) this.settingId.Count);
			for(int i=0, len=this.settingId.Count; i<len; i++) {
				bytes[7].putInt(this.settingId[i]);
			}
			total += bytes[7].limit();
		}

		if(this.hasBaseScore()) {
			bytes[8] = ByteBuffer.allocate(4);
			bytes[8].putFloat(this.baseScore);
			total += bytes[8].limit();
		}

		if(this.hasRoomPour()) {
			bytes[9] = ByteBuffer.allocate(4);
			bytes[9].putFloat(this.RoomPour);
			total += bytes[9].limit();
		}

		if(this.hasSeatBill()) {
				int length = 0;
				for(int i=0, len=this.seat_bill.Count; i<len; i++) {
					length += this.seat_bill[i].encode().Length;
				}
				bytes[10] = ByteBuffer.allocate(this.seat_bill.Count * 4 + length + 2);
				bytes[10].putShort((short) this.seat_bill.Count);
				for(int i=0, len=this.seat_bill.Count; i<len; i++) {
					byte[] _byte = this.seat_bill[i].encode();
					bytes[10].putInt(_byte.Length);
					bytes[10].put(_byte);
				}
			total += bytes[10].limit();
		}

		if(this.hasTotalRound()) {
			bytes[11] = ByteBuffer.allocate(4);
			bytes[11].putInt(this.totalRound);
			total += bytes[11].limit();
		}

		if(this.hasPlayer()) {
			byte[] _byte = this.player.encode();
			int len = _byte.Length;
			bytes[12] = ByteBuffer.allocate(4 + len);
			bytes[12].putInt(len);
			bytes[12].put(_byte);
			total += bytes[12].limit();
		}

		if(this.hasClubId()) {
			bytes[13] = ByteBuffer.allocate(4);
			bytes[13].putInt(this.clubId);
			total += bytes[13].limit();
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
		  
		if(this.hasRoomId()) {
			this.roomId = buf.getInt();
		}

		if(this.hasMaxLoop()) {
			this.maxLoop = buf.getInt();
		}

		if(this.hasLoop()) {
			this.loop = buf.getInt();
		}

		if(this.hasScores()) {
			this.scores = buf.getInt();
		}

		if(this.hasSeat()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.seat.Add(SEAT.decode(bytes));
			}
		}

		if(this.hasRoomstatus()) {
			this.roomstatus = (ENUM_ROOM_STATUS) buf.get();
		}

		if(this.hasRound()) {
			this.round = buf.getInt();
		}

		if(this.hasSettingId()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.settingId.Add(buf.getInt());
			}
		}

		if(this.hasBaseScore()) {
			this.baseScore = buf.getFloat();
		}

		if(this.hasRoomPour()) {
			this.RoomPour = buf.getFloat();
		}

		if(this.hasSeatBill()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.seat_bill.Add(SEAT_BILL.decode(bytes));
			}
		}

		if(this.hasTotalRound()) {
			this.totalRound = buf.getInt();
		}

		if(this.hasPlayer()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.player = PLAYER.decode(bytes);
		}

		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

	} 

	public bool hasRoomId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasMaxLoop() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasLoop() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasScores() {
		return (this.__flag[0] & 8) != 0;
	}

	public int seatCount() {
		return this.seat.Count;
	}

	public bool hasSeat() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasRoomstatus() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasRound() {
		return (this.__flag[0] & 64) != 0;
	}

	public int settingIdCount() {
		return this.settingId.Count;
	}

	public bool hasSettingId() {
		return (this.__flag[0] & 128) != 0;
	}

	public bool hasBaseScore() {
		return (this.__flag[1] & 1) != 0;
	}

	public bool hasRoomPour() {
		return (this.__flag[1] & 2) != 0;
	}

	public int seatBillCount() {
		return this.seat_bill.Count;
	}

	public bool hasSeatBill() {
		return (this.__flag[1] & 4) != 0;
	}

	public bool hasTotalRound() {
		return (this.__flag[1] & 8) != 0;
	}

	public bool hasPlayer() {
		return (this.__flag[1] & 16) != 0;
	}

	public bool hasClubId() {
		return (this.__flag[1] & 32) != 0;
	}

	public List<SEAT> getSeatList() {
		return this.seat;
	}

	public List<int> getSettingIdList() {
		return this.settingId;
	}

	public List<SEAT_BILL> getSeatBillList() {
		return this.seat_bill;
	}

}
}

