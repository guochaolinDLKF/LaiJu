//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:13 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_ROOM_BEGIN { 

	public const int CODE = 501004; 

	private byte[] __flag = new byte[1]; 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this.paigow_seat.Add(value);
	} 

	private bool _isOwner; 

	public bool isOwner { 
		set { 
			if(!this.hasIsOwner()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._isOwner = value;
		} 
		get { 
			return this._isOwner;
		} 
	} 

	private int _diceFirst; 

	public int diceFirst { 
		set { 
			if(!this.hasDiceFirst()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._diceSecond = value;
		} 
		get { 
			return this._diceSecond;
		} 
	} 

	private int _remainMahjong; 

	public int remainMahjong { 
		set { 
			if(!this.hasRemainMahjong()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._remainMahjong = value;
		} 
		get { 
			return this._remainMahjong;
		} 
	} 

	private int _firstGivePos; 

	public int firstGivePos { 
		set { 
			if(!this.hasFirstGivePos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._firstGivePos = value;
		} 
		get { 
			return this._firstGivePos;
		} 
	} 

	private List<PAIGOW_MAHJONG> mahjongs＿remain = new List<PAIGOW_MAHJONG>(); 

	public PAIGOW_MAHJONG getMahjongs＿remain(int index) { 
			return this.mahjongs＿remain[index];
	} 
	
	public void addMahjongs＿remain(PAIGOW_MAHJONG value) { 
			if(!this.hasMahjongs＿remain()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this.mahjongs＿remain.Add(value);
	} 

	public static PAIGOW_ROOM_BEGIN newBuilder() { 
		return new PAIGOW_ROOM_BEGIN(); 
	} 

	public static PAIGOW_ROOM_BEGIN decode(byte[] data) { 
		PAIGOW_ROOM_BEGIN proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[8]; 

		int total = 0;
		if(this.hasUnixtime()) {
			bytes[0] = ByteBuffer.allocate(8);
			bytes[0].putLong(this.unixtime);
			total += bytes[0].limit();
		}

		if(this.hasPaigowSeat()) {
				int length = 0;
				for(int i=0, len=this.paigow_seat.Count; i<len; i++) {
					length += this.paigow_seat[i].encode().Length;
				}
				bytes[1] = ByteBuffer.allocate(this.paigow_seat.Count * 4 + length + 2);
				bytes[1].putShort((short) this.paigow_seat.Count);
				for(int i=0, len=this.paigow_seat.Count; i<len; i++) {
					byte[] _byte = this.paigow_seat[i].encode();
					bytes[1].putInt(_byte.Length);
					bytes[1].put(_byte);
				}
			total += bytes[1].limit();
		}

		if(this.hasIsOwner()) {
			bytes[2] = ByteBuffer.allocate(1);
			if(this.isOwner) {
				bytes[2].put((byte) 1);
			}else{
				bytes[2].put((byte) 0);
			}
			total += bytes[2].limit();
		}

		if(this.hasDiceFirst()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.diceFirst);
			total += bytes[3].limit();
		}

		if(this.hasDiceSecond()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.diceSecond);
			total += bytes[4].limit();
		}

		if(this.hasRemainMahjong()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putInt(this.remainMahjong);
			total += bytes[5].limit();
		}

		if(this.hasFirstGivePos()) {
			bytes[6] = ByteBuffer.allocate(4);
			bytes[6].putInt(this.firstGivePos);
			total += bytes[6].limit();
		}

		if(this.hasMahjongs＿remain()) {
				int length = 0;
				for(int i=0, len=this.mahjongs＿remain.Count; i<len; i++) {
					length += this.mahjongs＿remain[i].encode().Length;
				}
				bytes[7] = ByteBuffer.allocate(this.mahjongs＿remain.Count * 4 + length + 2);
				bytes[7].putShort((short) this.mahjongs＿remain.Count);
				for(int i=0, len=this.mahjongs＿remain.Count; i<len; i++) {
					byte[] _byte = this.mahjongs＿remain[i].encode();
					bytes[7].putInt(_byte.Length);
					bytes[7].put(_byte);
				}
			total += bytes[7].limit();
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

		if(this.hasIsOwner()) {
			if(buf.get() == 1) {
				this.isOwner = true;
			}else{
				this.isOwner = false;
			}
		}

		if(this.hasDiceFirst()) {
			this.diceFirst = buf.getInt();
		}

		if(this.hasDiceSecond()) {
			this.diceSecond = buf.getInt();
		}

		if(this.hasRemainMahjong()) {
			this.remainMahjong = buf.getInt();
		}

		if(this.hasFirstGivePos()) {
			this.firstGivePos = buf.getInt();
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

	public bool hasUnixtime() {
		return (this.__flag[0] & 1) != 0;
	}

	public int paigowSeatCount() {
		return this.paigow_seat.Count;
	}

	public bool hasPaigowSeat() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasIsOwner() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasDiceFirst() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasDiceSecond() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasRemainMahjong() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasFirstGivePos() {
		return (this.__flag[0] & 64) != 0;
	}

	public int mahjongs＿remainCount() {
		return this.mahjongs＿remain.Count;
	}

	public bool hasMahjongs＿remain() {
		return (this.__flag[0] & 128) != 0;
	}

	public List<PAIGOW_SEAT> getPaigowSeatList() {
		return this.paigow_seat;
	}

	public List<PAIGOW_MAHJONG> getMahjongs＿remainList() {
		return this.mahjongs＿remain;
	}

}
}

