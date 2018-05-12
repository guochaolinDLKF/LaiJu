using System.Collections.Generic;

namespace ddz.proto {

public class DDZ_SEAT { 

	public const int CODE = 3002; 

	private byte[] __flag = new byte[16]; 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	private int _playerId; 

	public int playerId { 
		set { 
			if(!this.hasPlayerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._playerId = value;
		} 
		get { 
			return this._playerId;
		} 
	} 

	private int _gold; 

	public int gold { 
		set { 
			if(!this.hasGold()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._gold = value;
		} 
		get { 
			return this._gold;
		} 
	} 

	private int _pour; 

	public int pour { 
		set { 
			if(!this.hasPour()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._pour = value;
		} 
		get { 
			return this._pour;
		} 
	} 

	private SEAT_STATUS _status; 

	public SEAT_STATUS status { 
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

	private string _nickname; 

	public string nickname { 
		set { 
			if(!this.hasNickname()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this._gender = value;
		} 
		get { 
			return this._gender;
		} 
	} 

	private bool _isWiner; 

	public bool isWiner { 
		set { 
			if(!this.hasIsWiner()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this._isWiner = value;
		} 
		get { 
			return this._isWiner;
		} 
	} 

	private List<DDZ_POCKER> pokerList = new List<DDZ_POCKER>(); 

	public DDZ_POCKER getPokerList(int index) { 
			return this.pokerList[index];
	} 
	
	public void addPokerList(DDZ_POCKER value) { 
			if(!this.hasPokerList()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 2);
			}
			this.pokerList.Add(value);
	} 

	private List<DDZ_POCKER> lastPlayPokerList = new List<DDZ_POCKER>(); 

	public DDZ_POCKER getLastPlayPokerList(int index) { 
			return this.lastPlayPokerList[index];
	} 
	
	public void addLastPlayPokerList(DDZ_POCKER value) { 
			if(!this.hasLastPlayPokerList()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 4);
			}
			this.lastPlayPokerList.Add(value);
	} 

	private int _loopEarnings; 

	public int loopEarnings { 
		set { 
			if(!this.hasLoopEarnings()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 8);
			}
			this._loopEarnings = value;
		} 
		get { 
			return this._loopEarnings;
		} 
	} 

	private int _totalEarnings; 

	public int totalEarnings { 
		set { 
			if(!this.hasTotalEarnings()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 16);
			}
			this._totalEarnings = value;
		} 
		get { 
			return this._totalEarnings;
		} 
	} 

	private bool _isBanker; 

	public bool isBanker { 
		set { 
			if(!this.hasIsBanker()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 32);
			}
			this._isBanker = value;
		} 
		get { 
			return this._isBanker;
		} 
	} 

	private bool _isSpring; 

	public bool isSpring { 
		set { 
			if(!this.hasIsSpring()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 64);
			}
			this._isSpring = value;
		} 
		get { 
			return this._isSpring;
		} 
	} 

	public static DDZ_SEAT newBuilder() { 
		return new DDZ_SEAT(); 
	} 

	public static DDZ_SEAT decode(byte[] data) { 
		DDZ_SEAT proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[15]; 

		int total = 0;
		if(this.hasPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pos);
			total += bytes[0].limit();
		}

		if(this.hasPlayerId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.playerId);
			total += bytes[1].limit();
		}

		if(this.hasGold()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.gold);
			total += bytes[2].limit();
		}

		if(this.hasPour()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.pour);
			total += bytes[3].limit();
		}

		if(this.hasStatus()) {
			bytes[4] = ByteBuffer.allocate(1);
			bytes[4].put((byte) this.status);
			total += bytes[4].limit();
		}

		if(this.hasNickname()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.nickname);
			    short len = (short) _byte.Length;
			    bytes[5] = ByteBuffer.allocate(2 + len);
			    bytes[5].putShort(len);
				bytes[5].put(_byte);
			total += bytes[5].limit();
		}

		if(this.hasAvatar()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.avatar);
			    short len = (short) _byte.Length;
			    bytes[6] = ByteBuffer.allocate(2 + len);
			    bytes[6].putShort(len);
				bytes[6].put(_byte);
			total += bytes[6].limit();
		}

		if(this.hasGender()) {
			bytes[7] = ByteBuffer.allocate(1);
			bytes[7].put(this.gender);
			total += bytes[7].limit();
		}

		if(this.hasIsWiner()) {
			bytes[8] = ByteBuffer.allocate(1);
			if(this.isWiner) {
				bytes[8].put((byte) 1);
			}else{
				bytes[8].put((byte) 0);
			}
			total += bytes[8].limit();
		}

		if(this.hasPokerList()) {
				int length = 0;
				for(int i=0, len=this.pokerList.Count; i<len; i++) {
					length += this.pokerList[i].encode().Length;
				}
				bytes[9] = ByteBuffer.allocate(this.pokerList.Count * 4 + length + 2);
				bytes[9].putShort((short) this.pokerList.Count);
				for(int i=0, len=this.pokerList.Count; i<len; i++) {
					byte[] _byte = this.pokerList[i].encode();
					bytes[9].putInt(_byte.Length);
					bytes[9].put(_byte);
				}
			total += bytes[9].limit();
		}

		if(this.hasLastPlayPokerList()) {
				int length = 0;
				for(int i=0, len=this.lastPlayPokerList.Count; i<len; i++) {
					length += this.lastPlayPokerList[i].encode().Length;
				}
				bytes[10] = ByteBuffer.allocate(this.lastPlayPokerList.Count * 4 + length + 2);
				bytes[10].putShort((short) this.lastPlayPokerList.Count);
				for(int i=0, len=this.lastPlayPokerList.Count; i<len; i++) {
					byte[] _byte = this.lastPlayPokerList[i].encode();
					bytes[10].putInt(_byte.Length);
					bytes[10].put(_byte);
				}
			total += bytes[10].limit();
		}

		if(this.hasLoopEarnings()) {
			bytes[11] = ByteBuffer.allocate(4);
			bytes[11].putInt(this.loopEarnings);
			total += bytes[11].limit();
		}

		if(this.hasTotalEarnings()) {
			bytes[12] = ByteBuffer.allocate(4);
			bytes[12].putInt(this.totalEarnings);
			total += bytes[12].limit();
		}

		if(this.hasIsBanker()) {
			bytes[13] = ByteBuffer.allocate(1);
			if(this.isBanker) {
				bytes[13].put((byte) 1);
			}else{
				bytes[13].put((byte) 0);
			}
			total += bytes[13].limit();
		}

		if(this.hasIsSpring()) {
			bytes[14] = ByteBuffer.allocate(1);
			if(this.isSpring) {
				bytes[14].put((byte) 1);
			}else{
				bytes[14].put((byte) 0);
			}
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
		  
		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

		if(this.hasGold()) {
			this.gold = buf.getInt();
		}

		if(this.hasPour()) {
			this.pour = buf.getInt();
		}

		if(this.hasStatus()) {
			this.status = (SEAT_STATUS) buf.get();
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

		if(this.hasIsWiner()) {
			if(buf.get() == 1) {
				this.isWiner = true;
			}else{
				this.isWiner = false;
			}
		}

		if(this.hasPokerList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.pokerList.Add(DDZ_POCKER.decode(bytes));
			}
		}

		if(this.hasLastPlayPokerList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.lastPlayPokerList.Add(DDZ_POCKER.decode(bytes));
			}
		}

		if(this.hasLoopEarnings()) {
			this.loopEarnings = buf.getInt();
		}

		if(this.hasTotalEarnings()) {
			this.totalEarnings = buf.getInt();
		}

		if(this.hasIsBanker()) {
			if(buf.get() == 1) {
				this.isBanker = true;
			}else{
				this.isBanker = false;
			}
		}

		if(this.hasIsSpring()) {
			if(buf.get() == 1) {
				this.isSpring = true;
			}else{
				this.isSpring = false;
			}
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasGold() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasPour() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasStatus() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasNickname() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasAvatar() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasGender() {
		return (this.__flag[0] & 128) != 0;
	}

	public bool hasIsWiner() {
		return (this.__flag[1] & 1) != 0;
	}

	public int pokerListCount() {
		return this.pokerList.Count;
	}

	public bool hasPokerList() {
		return (this.__flag[1] & 2) != 0;
	}

	public int lastPlayPokerListCount() {
		return this.lastPlayPokerList.Count;
	}

	public bool hasLastPlayPokerList() {
		return (this.__flag[1] & 4) != 0;
	}

	public bool hasLoopEarnings() {
		return (this.__flag[1] & 8) != 0;
	}

	public bool hasTotalEarnings() {
		return (this.__flag[1] & 16) != 0;
	}

	public bool hasIsBanker() {
		return (this.__flag[1] & 32) != 0;
	}

	public bool hasIsSpring() {
		return (this.__flag[1] & 64) != 0;
	}

	public List<DDZ_POCKER> getPokerListList() {
		return this.pokerList;
	}

	public List<DDZ_POCKER> getLastPlayPokerListList() {
		return this.lastPlayPokerList;
	}

}
}

