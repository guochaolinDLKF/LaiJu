using System.Collections.Generic;

namespace proto.jy {

public class JY_SEAT { 

	public const int CODE = 6002; 

	private byte[] __flag = new byte[3]; 

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

	private bool _isBanker; 

	public bool isBanker { 
		set { 
			if(!this.hasIsBanker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._isBanker = value;
		} 
		get { 
			return this._isBanker;
		} 
	} 

	private bool _isDismiss; 

	public bool isDismiss { 
		set { 
			if(!this.hasIsDismiss()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._isDismiss = value;
		} 
		get { 
			return this._isDismiss;
		} 
	} 

	private List<JY_POKER> pokerList = new List<JY_POKER>(); 

	public JY_POKER getPokerList(int index) { 
			return this.pokerList[index];
	} 
	
	public void addPokerList(JY_POKER value) { 
			if(!this.hasPokerList()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this.pokerList.Add(value);
	} 

	private SEAT_STATUS _status; 

	public SEAT_STATUS status { 
		set { 
			if(!this.hasStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
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
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
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
	    		this.__flag[1] = (byte) (this.__flag[1] | 2);
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
	    		this.__flag[1] = (byte) (this.__flag[1] | 4);
			}
			this._gender = value;
		} 
		get { 
			return this._gender;
		} 
	} 

	private int _earnings; 

	public int earnings { 
		set { 
			if(!this.hasEarnings()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 8);
			}
			this._earnings = value;
		} 
		get { 
			return this._earnings;
		} 
	} 

	private List<JY_POKER> historyPokerList = new List<JY_POKER>(); 

	public JY_POKER getHistoryPokerList(int index) { 
			return this.historyPokerList[index];
	} 
	
	public void addHistoryPokerList(JY_POKER value) { 
			if(!this.hasHistoryPokerList()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 16);
			}
			this.historyPokerList.Add(value);
	} 

	private bool _isReady; 

	public bool isReady { 
		set { 
			if(!this.hasIsReady()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 32);
			}
			this._isReady = value;
		} 
		get { 
			return this._isReady;
		} 
	} 

	private bool _isJoinGame; 

	public bool isJoinGame { 
		set { 
			if(!this.hasIsJoinGame()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 64);
			}
			this._isJoinGame = value;
		} 
		get { 
			return this._isJoinGame;
		} 
	} 

	private int _loopEarnings; 

	public int loopEarnings { 
		set { 
			if(!this.hasLoopEarnings()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 128);
			}
			this._loopEarnings = value;
		} 
		get { 
			return this._loopEarnings;
		} 
	} 

	private bool _isOnLine; 

	public bool isOnLine { 
		set { 
			if(!this.hasIsOnLine()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 1);
			}
			this._isOnLine = value;
		} 
		get { 
			return this._isOnLine;
		} 
	} 

	public static JY_SEAT newBuilder() { 
		return new JY_SEAT(); 
	} 

	public static JY_SEAT decode(byte[] data) { 
		JY_SEAT proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[17]; 

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

		if(this.hasIsBanker()) {
			bytes[4] = ByteBuffer.allocate(1);
			if(this.isBanker) {
				bytes[4].put((byte) 1);
			}else{
				bytes[4].put((byte) 0);
			}
			total += bytes[4].limit();
		}

		if(this.hasIsDismiss()) {
			bytes[5] = ByteBuffer.allocate(1);
			if(this.isDismiss) {
				bytes[5].put((byte) 1);
			}else{
				bytes[5].put((byte) 0);
			}
			total += bytes[5].limit();
		}

		if(this.hasPokerList()) {
				int length = 0;
				for(int i=0, len=this.pokerList.Count; i<len; i++) {
					length += this.pokerList[i].encode().Length;
				}
				bytes[6] = ByteBuffer.allocate(this.pokerList.Count * 4 + length + 2);
				bytes[6].putShort((short) this.pokerList.Count);
				for(int i=0, len=this.pokerList.Count; i<len; i++) {
					byte[] _byte = this.pokerList[i].encode();
					bytes[6].putInt(_byte.Length);
					bytes[6].put(_byte);
				}
			total += bytes[6].limit();
		}

		if(this.hasStatus()) {
			bytes[7] = ByteBuffer.allocate(1);
			bytes[7].put((byte) this.status);
			total += bytes[7].limit();
		}

		if(this.hasNickname()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.nickname);
			    short len = (short) _byte.Length;
			    bytes[8] = ByteBuffer.allocate(2 + len);
			    bytes[8].putShort(len);
				bytes[8].put(_byte);
			total += bytes[8].limit();
		}

		if(this.hasAvatar()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.avatar);
			    short len = (short) _byte.Length;
			    bytes[9] = ByteBuffer.allocate(2 + len);
			    bytes[9].putShort(len);
				bytes[9].put(_byte);
			total += bytes[9].limit();
		}

		if(this.hasGender()) {
			bytes[10] = ByteBuffer.allocate(1);
			bytes[10].put(this.gender);
			total += bytes[10].limit();
		}

		if(this.hasEarnings()) {
			bytes[11] = ByteBuffer.allocate(4);
			bytes[11].putInt(this.earnings);
			total += bytes[11].limit();
		}

		if(this.hasHistoryPokerList()) {
				int length = 0;
				for(int i=0, len=this.historyPokerList.Count; i<len; i++) {
					length += this.historyPokerList[i].encode().Length;
				}
				bytes[12] = ByteBuffer.allocate(this.historyPokerList.Count * 4 + length + 2);
				bytes[12].putShort((short) this.historyPokerList.Count);
				for(int i=0, len=this.historyPokerList.Count; i<len; i++) {
					byte[] _byte = this.historyPokerList[i].encode();
					bytes[12].putInt(_byte.Length);
					bytes[12].put(_byte);
				}
			total += bytes[12].limit();
		}

		if(this.hasIsReady()) {
			bytes[13] = ByteBuffer.allocate(1);
			if(this.isReady) {
				bytes[13].put((byte) 1);
			}else{
				bytes[13].put((byte) 0);
			}
			total += bytes[13].limit();
		}

		if(this.hasIsJoinGame()) {
			bytes[14] = ByteBuffer.allocate(1);
			if(this.isJoinGame) {
				bytes[14].put((byte) 1);
			}else{
				bytes[14].put((byte) 0);
			}
			total += bytes[14].limit();
		}

		if(this.hasLoopEarnings()) {
			bytes[15] = ByteBuffer.allocate(4);
			bytes[15].putInt(this.loopEarnings);
			total += bytes[15].limit();
		}

		if(this.hasIsOnLine()) {
			bytes[16] = ByteBuffer.allocate(1);
			if(this.isOnLine) {
				bytes[16].put((byte) 1);
			}else{
				bytes[16].put((byte) 0);
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

		if(this.hasIsBanker()) {
			if(buf.get() == 1) {
				this.isBanker = true;
			}else{
				this.isBanker = false;
			}
		}

		if(this.hasIsDismiss()) {
			if(buf.get() == 1) {
				this.isDismiss = true;
			}else{
				this.isDismiss = false;
			}
		}

		if(this.hasPokerList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.pokerList.Add(JY_POKER.decode(bytes));
			}
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

		if(this.hasEarnings()) {
			this.earnings = buf.getInt();
		}

		if(this.hasHistoryPokerList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.historyPokerList.Add(JY_POKER.decode(bytes));
			}
		}

		if(this.hasIsReady()) {
			if(buf.get() == 1) {
				this.isReady = true;
			}else{
				this.isReady = false;
			}
		}

		if(this.hasIsJoinGame()) {
			if(buf.get() == 1) {
				this.isJoinGame = true;
			}else{
				this.isJoinGame = false;
			}
		}

		if(this.hasLoopEarnings()) {
			this.loopEarnings = buf.getInt();
		}

		if(this.hasIsOnLine()) {
			if(buf.get() == 1) {
				this.isOnLine = true;
			}else{
				this.isOnLine = false;
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

	public bool hasIsBanker() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasIsDismiss() {
		return (this.__flag[0] & 32) != 0;
	}

	public int pokerListCount() {
		return this.pokerList.Count;
	}

	public bool hasPokerList() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasStatus() {
		return (this.__flag[0] & 128) != 0;
	}

	public bool hasNickname() {
		return (this.__flag[1] & 1) != 0;
	}

	public bool hasAvatar() {
		return (this.__flag[1] & 2) != 0;
	}

	public bool hasGender() {
		return (this.__flag[1] & 4) != 0;
	}

	public bool hasEarnings() {
		return (this.__flag[1] & 8) != 0;
	}

	public int historyPokerListCount() {
		return this.historyPokerList.Count;
	}

	public bool hasHistoryPokerList() {
		return (this.__flag[1] & 16) != 0;
	}

	public bool hasIsReady() {
		return (this.__flag[1] & 32) != 0;
	}

	public bool hasIsJoinGame() {
		return (this.__flag[1] & 64) != 0;
	}

	public bool hasLoopEarnings() {
		return (this.__flag[1] & 128) != 0;
	}

	public bool hasIsOnLine() {
		return (this.__flag[2] & 1) != 0;
	}

	public List<JY_POKER> getPokerListList() {
		return this.pokerList;
	}

	public List<JY_POKER> getHistoryPokerListList() {
		return this.historyPokerList;
	}

}
}

