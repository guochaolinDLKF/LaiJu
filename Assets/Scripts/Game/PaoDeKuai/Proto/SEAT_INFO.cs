//===================================================
//Author      : DRB
//CreateTime  ：12/5/2017 12:01:42 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.pdk {

public class SEAT_INFO { 

	public const int CODE = 2; 

	private byte[] __flag = new byte[2]; 

	private int _playerId; 

	public int playerId { 
		set { 
			if(!this.hasPlayerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._playerId = value;
		} 
		get { 
			return this._playerId;
		} 
	} 

	private string _nickname; 

	public string nickname { 
		set { 
			if(!this.hasNickname()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._nickname = value;
		} 
		get { 
			return this._nickname;
		} 
	} 

	private string _avatar; 

	public string avatar { 
		set { 
			if(!this.hasAvatar()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._avatar = value;
		} 
		get { 
			return this._avatar;
		} 
	} 

	private byte _gender; 

	public byte gender { 
		set { 
			if(!this.hasGender()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._gender = value;
		} 
		get { 
			return this._gender;
		} 
	} 

	private int _gold; 

	public int gold { 
		set { 
			if(!this.hasGold()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._gold = value;
		} 
		get { 
			return this._gold;
		} 
	} 

	private int _type; 

	public int type { 
		set { 
			if(!this.hasType()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._type = value;
		} 
		get { 
			return this._type;
		} 
	} 

	private int _HandPocker; 

	public int HandPocker { 
		set { 
			if(!this.hasHandPocker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._HandPocker = value;
		} 
		get { 
			return this._HandPocker;
		} 
	} 

	private SEAT_STATUS _seatStatus; 

	public SEAT_STATUS seatStatus { 
		set { 
			if(!this.hasSeatStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this._seatStatus = value;
		} 
		get { 
			return this._seatStatus;
		} 
	} 

	private List<POKER_INFO> pokerInfo = new List<POKER_INFO>(); 

	public POKER_INFO getPokerInfo(int index) { 
			return this.pokerInfo[index];
	} 
	
	public void addPokerInfo(POKER_INFO value) { 
			if(!this.hasPokerInfo()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this.pokerInfo.Add(value);
	} 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 2);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	private bool _isReady; 

	public bool isReady { 
		set { 
			if(!this.hasIsReady()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 4);
			}
			this._isReady = value;
		} 
		get { 
			return this._isReady;
		} 
	} 

	private DISMISS_STATUS _dismiss_status; 

	public DISMISS_STATUS dismiss_status { 
		set { 
			if(!this.hasDismissStatus()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 8);
			}
			this._dismiss_status = value;
		} 
		get { 
			return this._dismiss_status;
		} 
	} 

	public static SEAT_INFO newBuilder() { 
		return new SEAT_INFO(); 
	} 

	public static SEAT_INFO decode(byte[] data) { 
		SEAT_INFO proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[12]; 

		int total = 0;
		if(this.hasPlayerId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.playerId);
			total += bytes[0].limit();
		}

		if(this.hasNickname()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.nickname);
			    short len = (short) _byte.Length;
			    bytes[1] = ByteBuffer.allocate(2 + len);
			    bytes[1].putShort(len);
				bytes[1].put(_byte);
			total += bytes[1].limit();
		}

		if(this.hasAvatar()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.avatar);
			    short len = (short) _byte.Length;
			    bytes[2] = ByteBuffer.allocate(2 + len);
			    bytes[2].putShort(len);
				bytes[2].put(_byte);
			total += bytes[2].limit();
		}

		if(this.hasGender()) {
			bytes[3] = ByteBuffer.allocate(1);
			bytes[3].put(this.gender);
			total += bytes[3].limit();
		}

		if(this.hasGold()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.gold);
			total += bytes[4].limit();
		}

		if(this.hasType()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putInt(this.type);
			total += bytes[5].limit();
		}

		if(this.hasHandPocker()) {
			bytes[6] = ByteBuffer.allocate(4);
			bytes[6].putInt(this.HandPocker);
			total += bytes[6].limit();
		}

		if(this.hasSeatStatus()) {
			bytes[7] = ByteBuffer.allocate(1);
			bytes[7].put((byte) this.seatStatus);
			total += bytes[7].limit();
		}

		if(this.hasPokerInfo()) {
				int length = 0;
				for(int i=0, len=this.pokerInfo.Count; i<len; i++) {
					length += this.pokerInfo[i].encode().Length;
				}
				bytes[8] = ByteBuffer.allocate(this.pokerInfo.Count * 4 + length + 2);
				bytes[8].putShort((short) this.pokerInfo.Count);
				for(int i=0, len=this.pokerInfo.Count; i<len; i++) {
					byte[] _byte = this.pokerInfo[i].encode();
					bytes[8].putInt(_byte.Length);
					bytes[8].put(_byte);
				}
			total += bytes[8].limit();
		}

		if(this.hasPos()) {
			bytes[9] = ByteBuffer.allocate(4);
			bytes[9].putInt(this.pos);
			total += bytes[9].limit();
		}

		if(this.hasIsReady()) {
			bytes[10] = ByteBuffer.allocate(1);
			if(this.isReady) {
				bytes[10].put((byte) 1);
			}else{
				bytes[10].put((byte) 0);
			}
			total += bytes[10].limit();
		}

		if(this.hasDismissStatus()) {
			bytes[11] = ByteBuffer.allocate(1);
			bytes[11].put((byte) this.dismiss_status);
			total += bytes[11].limit();
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
		  
		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

		if(this.hasNickname()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.nickname = System.Text.Encoding.UTF8.GetString(bytes);
		}

		if(this.hasAvatar()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.avatar = System.Text.Encoding.UTF8.GetString(bytes);
		}

		if(this.hasGender()) {
			this.gender = buf.get();
		}

		if(this.hasGold()) {
			this.gold = buf.getInt();
		}

		if(this.hasType()) {
			this.type = buf.getInt();
		}

		if(this.hasHandPocker()) {
			this.HandPocker = buf.getInt();
		}

		if(this.hasSeatStatus()) {
			this.seatStatus = (SEAT_STATUS) buf.get();
		}

		if(this.hasPokerInfo()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.pokerInfo.Add(POKER_INFO.decode(bytes));
			}
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasIsReady()) {
			if(buf.get() == 1) {
				this.isReady = true;
			}else{
				this.isReady = false;
			}
		}

		if(this.hasDismissStatus()) {
			this.dismiss_status = (DISMISS_STATUS) buf.get();
		}

	} 

	public bool hasPlayerId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasNickname() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasAvatar() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasGender() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasGold() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasType() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasHandPocker() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasSeatStatus() {
		return (this.__flag[0] & 128) != 0;
	}

	public int pokerInfoCount() {
		return this.pokerInfo.Count;
	}

	public bool hasPokerInfo() {
		return (this.__flag[1] & 1) != 0;
	}

	public bool hasPos() {
		return (this.__flag[1] & 2) != 0;
	}

	public bool hasIsReady() {
		return (this.__flag[1] & 4) != 0;
	}

	public bool hasDismissStatus() {
		return (this.__flag[1] & 8) != 0;
	}

	public List<POKER_INFO> getPokerInfoList() {
		return this.pokerInfo;
	}

}
}

