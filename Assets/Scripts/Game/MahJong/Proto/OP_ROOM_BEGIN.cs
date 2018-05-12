using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_BEGIN { 

	public const int CODE = 101008; 

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

	private ENUM_ROOM_STATUS _status; 

	public ENUM_ROOM_STATUS status { 
		set { 
			if(!this.hasStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._status = value;
		} 
		get { 
			return this._status;
		} 
	} 

	private List<OP_SEAT_FULL> seat = new List<OP_SEAT_FULL>(); 

	public OP_SEAT_FULL getSeat(int index) { 
			return this.seat[index];
	} 
	
	public void addSeat(OP_SEAT_FULL value) { 
			if(!this.hasSeat()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this.seat.Add(value);
	} 

	private long _beginTime; 

	public long beginTime { 
		set { 
			if(!this.hasBeginTime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._beginTime = value;
		} 
		get { 
			return this._beginTime;
		} 
	} 

	private int _pokerTotal; 

	public int pokerTotal { 
		set { 
			if(!this.hasPokerTotal()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._pokerTotal = value;
		} 
		get { 
			return this._pokerTotal;
		} 
	} 

	private int _pokerAmount; 

	public int pokerAmount { 
		set { 
			if(!this.hasPokerAmount()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._pokerAmount = value;
		} 
		get { 
			return this._pokerAmount;
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

	private int _diceFirst; 

	public int diceFirst { 
		set { 
			if(!this.hasDiceFirst()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this._diceFirst = value;
		} 
		get { 
			return this._diceFirst;
		} 
	} 

	private int _diceSecond; 

	public int diceSecond { 
		set { 
			if(!this.hasDiceSecond()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this._diceSecond = value;
		} 
		get { 
			return this._diceSecond;
		} 
	} 

	private OP_POKER _luckPoker; 

	public OP_POKER luckPoker { 
		set { 
			if(!this.hasLuckPoker()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 2);
			}
			this._luckPoker = value;
		} 
		get { 
			return this._luckPoker;
		} 
	} 

	private int _loop; 

	public int loop { 
		set { 
			if(!this.hasLoop()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 4);
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
	    		this.__flag[1] = (byte) (this.__flag[1] | 8);
			}
			this._maxLoop = value;
		} 
		get { 
			return this._maxLoop;
		} 
	} 

	private int _diceFirstA; 

	public int diceFirstA { 
		set { 
			if(!this.hasDiceFirstA()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 16);
			}
			this._diceFirstA = value;
		} 
		get { 
			return this._diceFirstA;
		} 
	} 

	private int _diceFirstB; 

	public int diceFirstB { 
		set { 
			if(!this.hasDiceFirstB()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 32);
			}
			this._diceFirstB = value;
		} 
		get { 
			return this._diceFirstB;
		} 
	} 

	private int _diceSecondA; 

	public int diceSecondA { 
		set { 
			if(!this.hasDiceSecondA()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 64);
			}
			this._diceSecondA = value;
		} 
		get { 
			return this._diceSecondA;
		} 
	} 

	private int _diceSecondB; 

	public int diceSecondB { 
		set { 
			if(!this.hasDiceSecondB()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 128);
			}
			this._diceSecondB = value;
		} 
		get { 
			return this._diceSecondB;
		} 
	} 

	public static OP_ROOM_BEGIN newBuilder() { 
		return new OP_ROOM_BEGIN(); 
	} 

	public static OP_ROOM_BEGIN decode(byte[] data) { 
		OP_ROOM_BEGIN proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[16]; 

		int total = 0;
		if(this.hasRoomId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.roomId);
			total += bytes[0].limit();
		}

		if(this.hasStatus()) {
			bytes[1] = ByteBuffer.allocate(1);
			bytes[1].put((byte) this.status);
			total += bytes[1].limit();
		}

		if(this.hasSeat()) {
				int length = 0;
				for(int i=0, len=this.seat.Count; i<len; i++) {
					length += this.seat[i].encode().Length;
				}
				bytes[2] = ByteBuffer.allocate(this.seat.Count * 4 + length + 2);
				bytes[2].putShort((short) this.seat.Count);
				for(int i=0, len=this.seat.Count; i<len; i++) {
					byte[] _byte = this.seat[i].encode();
					bytes[2].putInt(_byte.Length);
					bytes[2].put(_byte);
				}
			total += bytes[2].limit();
		}

		if(this.hasBeginTime()) {
			bytes[3] = ByteBuffer.allocate(8);
			bytes[3].putLong(this.beginTime);
			total += bytes[3].limit();
		}

		if(this.hasPokerTotal()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.pokerTotal);
			total += bytes[4].limit();
		}

		if(this.hasPokerAmount()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putInt(this.pokerAmount);
			total += bytes[5].limit();
		}

		if(this.hasBaseScore()) {
			bytes[6] = ByteBuffer.allocate(4);
			bytes[6].putInt(this.baseScore);
			total += bytes[6].limit();
		}

		if(this.hasDiceFirst()) {
			bytes[7] = ByteBuffer.allocate(4);
			bytes[7].putInt(this.diceFirst);
			total += bytes[7].limit();
		}

		if(this.hasDiceSecond()) {
			bytes[8] = ByteBuffer.allocate(4);
			bytes[8].putInt(this.diceSecond);
			total += bytes[8].limit();
		}

		if(this.hasLuckPoker()) {
			byte[] _byte = this.luckPoker.encode();
			int len = _byte.Length;
			bytes[9] = ByteBuffer.allocate(4 + len);
			bytes[9].putInt(len);
			bytes[9].put(_byte);
			total += bytes[9].limit();
		}

		if(this.hasLoop()) {
			bytes[10] = ByteBuffer.allocate(4);
			bytes[10].putInt(this.loop);
			total += bytes[10].limit();
		}

		if(this.hasMaxLoop()) {
			bytes[11] = ByteBuffer.allocate(4);
			bytes[11].putInt(this.maxLoop);
			total += bytes[11].limit();
		}

		if(this.hasDiceFirstA()) {
			bytes[12] = ByteBuffer.allocate(4);
			bytes[12].putInt(this.diceFirstA);
			total += bytes[12].limit();
		}

		if(this.hasDiceFirstB()) {
			bytes[13] = ByteBuffer.allocate(4);
			bytes[13].putInt(this.diceFirstB);
			total += bytes[13].limit();
		}

		if(this.hasDiceSecondA()) {
			bytes[14] = ByteBuffer.allocate(4);
			bytes[14].putInt(this.diceSecondA);
			total += bytes[14].limit();
		}

		if(this.hasDiceSecondB()) {
			bytes[15] = ByteBuffer.allocate(4);
			bytes[15].putInt(this.diceSecondB);
			total += bytes[15].limit();
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

		if(this.hasStatus()) {
			this.status = (ENUM_ROOM_STATUS) buf.get();
		}

		if(this.hasSeat()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.seat.Add(OP_SEAT_FULL.decode(bytes));
			}
		}

		if(this.hasBeginTime()) {
			this.beginTime = buf.getLong();
		}

		if(this.hasPokerTotal()) {
			this.pokerTotal = buf.getInt();
		}

		if(this.hasPokerAmount()) {
			this.pokerAmount = buf.getInt();
		}

		if(this.hasBaseScore()) {
			this.baseScore = buf.getInt();
		}

		if(this.hasDiceFirst()) {
			this.diceFirst = buf.getInt();
		}

		if(this.hasDiceSecond()) {
			this.diceSecond = buf.getInt();
		}

		if(this.hasLuckPoker()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.luckPoker = OP_POKER.decode(bytes);
		}

		if(this.hasLoop()) {
			this.loop = buf.getInt();
		}

		if(this.hasMaxLoop()) {
			this.maxLoop = buf.getInt();
		}

		if(this.hasDiceFirstA()) {
			this.diceFirstA = buf.getInt();
		}

		if(this.hasDiceFirstB()) {
			this.diceFirstB = buf.getInt();
		}

		if(this.hasDiceSecondA()) {
			this.diceSecondA = buf.getInt();
		}

		if(this.hasDiceSecondB()) {
			this.diceSecondB = buf.getInt();
		}

	} 

	public bool hasRoomId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasStatus() {
		return (this.__flag[0] & 2) != 0;
	}

	public int seatCount() {
		return this.seat.Count;
	}

	public bool hasSeat() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasBeginTime() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasPokerTotal() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasPokerAmount() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasBaseScore() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasDiceFirst() {
		return (this.__flag[0] & 128) != 0;
	}

	public bool hasDiceSecond() {
		return (this.__flag[1] & 1) != 0;
	}

	public bool hasLuckPoker() {
		return (this.__flag[1] & 2) != 0;
	}

	public bool hasLoop() {
		return (this.__flag[1] & 4) != 0;
	}

	public bool hasMaxLoop() {
		return (this.__flag[1] & 8) != 0;
	}

	public bool hasDiceFirstA() {
		return (this.__flag[1] & 16) != 0;
	}

	public bool hasDiceFirstB() {
		return (this.__flag[1] & 32) != 0;
	}

	public bool hasDiceSecondA() {
		return (this.__flag[1] & 64) != 0;
	}

	public bool hasDiceSecondB() {
		return (this.__flag[1] & 128) != 0;
	}

	public List<OP_SEAT_FULL> getSeatList() {
		return this.seat;
	}

}
}

