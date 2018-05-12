//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:26 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_SEAT { 

	public const int CODE = 5003; 

	private byte[] __flag = new byte[3]; 

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

	private bool _isWiner; 

	public bool isWiner { 
		set { 
			if(!this.hasIsWiner()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._isWiner = value;
		} 
		get { 
			return this._isWiner;
		} 
	} 

	private List<PAIGOW_MAHJONG> paigow_mahjong = new List<PAIGOW_MAHJONG>(); 

	public PAIGOW_MAHJONG getPaigowMahjong(int index) { 
			return this.paigow_mahjong[index];
	} 
	
	public void addPaigowMahjong(PAIGOW_MAHJONG value) { 
			if(!this.hasPaigowMahjong()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this.paigow_mahjong.Add(value);
	} 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	private int _pour; 

	public int pour { 
		set { 
			if(!this.hasPour()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this._pour = value;
		} 
		get { 
			return this._pour;
		} 
	} 

	private int _earnings; 

	public int earnings { 
		set { 
			if(!this.hasEarnings()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 2);
			}
			this._earnings = value;
		} 
		get { 
			return this._earnings;
		} 
	} 

	private int _loopEarnings; 

	public int loopEarnings { 
		set { 
			if(!this.hasLoopEarnings()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 4);
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
	    		this.__flag[1] = (byte) (this.__flag[1] | 8);
			}
			this._totalEarnings = value;
		} 
		get { 
			return this._totalEarnings;
		} 
	} 

	private int _gold; 

	public int gold { 
		set { 
			if(!this.hasGold()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 16);
			}
			this._gold = value;
		} 
		get { 
			return this._gold;
		} 
	} 

	private SEAT_STATUS _seat_status; 

	public SEAT_STATUS seat_status { 
		set { 
			if(!this.hasSeatStatus()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 32);
			}
			this._seat_status = value;
		} 
		get { 
			return this._seat_status;
		} 
	} 

	private bool _isDismiss; 

	public bool isDismiss { 
		set { 
			if(!this.hasIsDismiss()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 64);
			}
			this._isDismiss = value;
		} 
		get { 
			return this._isDismiss;
		} 
	} 

	private List<PAIGOW_MAHJONG> historyPoker = new List<PAIGOW_MAHJONG>(); 

	public PAIGOW_MAHJONG getHistoryPoker(int index) { 
			return this.historyPoker[index];
	} 
	
	public void addHistoryPoker(PAIGOW_MAHJONG value) { 
			if(!this.hasHistoryPoker()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 128);
			}
			this.historyPoker.Add(value);
	} 

	private int _isGrabBanker; 

	public int isGrabBanker { 
		set { 
			if(!this.hasIsGrabBanker()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 1);
			}
			this._isGrabBanker = value;
		} 
		get { 
			return this._isGrabBanker;
		} 
	} 

	private int _isCutPoker; 

	public int isCutPoker { 
		set { 
			if(!this.hasIsCutPoker()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 2);
			}
			this._isCutPoker = value;
		} 
		get { 
			return this._isCutPoker;
		} 
	} 

	private int _isCutPan; 

	public int isCutPan { 
		set { 
			if(!this.hasIsCutPan()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 4);
			}
			this._isCutPan = value;
		} 
		get { 
			return this._isCutPan;
		} 
	} 

	public static PAIGOW_SEAT newBuilder() { 
		return new PAIGOW_SEAT(); 
	} 

	public static PAIGOW_SEAT decode(byte[] data) { 
		PAIGOW_SEAT proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[19]; 

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

		if(this.hasIsBanker()) {
			bytes[4] = ByteBuffer.allocate(1);
			if(this.isBanker) {
				bytes[4].put((byte) 1);
			}else{
				bytes[4].put((byte) 0);
			}
			total += bytes[4].limit();
		}

		if(this.hasIsWiner()) {
			bytes[5] = ByteBuffer.allocate(1);
			if(this.isWiner) {
				bytes[5].put((byte) 1);
			}else{
				bytes[5].put((byte) 0);
			}
			total += bytes[5].limit();
		}

		if(this.hasPaigowMahjong()) {
				int length = 0;
				for(int i=0, len=this.paigow_mahjong.Count; i<len; i++) {
					length += this.paigow_mahjong[i].encode().Length;
				}
				bytes[6] = ByteBuffer.allocate(this.paigow_mahjong.Count * 4 + length + 2);
				bytes[6].putShort((short) this.paigow_mahjong.Count);
				for(int i=0, len=this.paigow_mahjong.Count; i<len; i++) {
					byte[] _byte = this.paigow_mahjong[i].encode();
					bytes[6].putInt(_byte.Length);
					bytes[6].put(_byte);
				}
			total += bytes[6].limit();
		}

		if(this.hasPos()) {
			bytes[7] = ByteBuffer.allocate(4);
			bytes[7].putInt(this.pos);
			total += bytes[7].limit();
		}

		if(this.hasPour()) {
			bytes[8] = ByteBuffer.allocate(4);
			bytes[8].putInt(this.pour);
			total += bytes[8].limit();
		}

		if(this.hasEarnings()) {
			bytes[9] = ByteBuffer.allocate(4);
			bytes[9].putInt(this.earnings);
			total += bytes[9].limit();
		}

		if(this.hasLoopEarnings()) {
			bytes[10] = ByteBuffer.allocate(4);
			bytes[10].putInt(this.loopEarnings);
			total += bytes[10].limit();
		}

		if(this.hasTotalEarnings()) {
			bytes[11] = ByteBuffer.allocate(4);
			bytes[11].putInt(this.totalEarnings);
			total += bytes[11].limit();
		}

		if(this.hasGold()) {
			bytes[12] = ByteBuffer.allocate(4);
			bytes[12].putInt(this.gold);
			total += bytes[12].limit();
		}

		if(this.hasSeatStatus()) {
			bytes[13] = ByteBuffer.allocate(1);
			bytes[13].put((byte) this.seat_status);
			total += bytes[13].limit();
		}

		if(this.hasIsDismiss()) {
			bytes[14] = ByteBuffer.allocate(1);
			if(this.isDismiss) {
				bytes[14].put((byte) 1);
			}else{
				bytes[14].put((byte) 0);
			}
			total += bytes[14].limit();
		}

		if(this.hasHistoryPoker()) {
				int length = 0;
				for(int i=0, len=this.historyPoker.Count; i<len; i++) {
					length += this.historyPoker[i].encode().Length;
				}
				bytes[15] = ByteBuffer.allocate(this.historyPoker.Count * 4 + length + 2);
				bytes[15].putShort((short) this.historyPoker.Count);
				for(int i=0, len=this.historyPoker.Count; i<len; i++) {
					byte[] _byte = this.historyPoker[i].encode();
					bytes[15].putInt(_byte.Length);
					bytes[15].put(_byte);
				}
			total += bytes[15].limit();
		}

		if(this.hasIsGrabBanker()) {
			bytes[16] = ByteBuffer.allocate(4);
			bytes[16].putInt(this.isGrabBanker);
			total += bytes[16].limit();
		}

		if(this.hasIsCutPoker()) {
			bytes[17] = ByteBuffer.allocate(4);
			bytes[17].putInt(this.isCutPoker);
			total += bytes[17].limit();
		}

		if(this.hasIsCutPan()) {
			bytes[18] = ByteBuffer.allocate(4);
			bytes[18].putInt(this.isCutPan);
			total += bytes[18].limit();
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

		if(this.hasIsBanker()) {
			if(buf.get() == 1) {
				this.isBanker = true;
			}else{
				this.isBanker = false;
			}
		}

		if(this.hasIsWiner()) {
			if(buf.get() == 1) {
				this.isWiner = true;
			}else{
				this.isWiner = false;
			}
		}

		if(this.hasPaigowMahjong()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.paigow_mahjong.Add(PAIGOW_MAHJONG.decode(bytes));
			}
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasPour()) {
			this.pour = buf.getInt();
		}

		if(this.hasEarnings()) {
			this.earnings = buf.getInt();
		}

		if(this.hasLoopEarnings()) {
			this.loopEarnings = buf.getInt();
		}

		if(this.hasTotalEarnings()) {
			this.totalEarnings = buf.getInt();
		}

		if(this.hasGold()) {
			this.gold = buf.getInt();
		}

		if(this.hasSeatStatus()) {
			this.seat_status = (SEAT_STATUS) buf.get();
		}

		if(this.hasIsDismiss()) {
			if(buf.get() == 1) {
				this.isDismiss = true;
			}else{
				this.isDismiss = false;
			}
		}

		if(this.hasHistoryPoker()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.historyPoker.Add(PAIGOW_MAHJONG.decode(bytes));
			}
		}

		if(this.hasIsGrabBanker()) {
			this.isGrabBanker = buf.getInt();
		}

		if(this.hasIsCutPoker()) {
			this.isCutPoker = buf.getInt();
		}

		if(this.hasIsCutPan()) {
			this.isCutPan = buf.getInt();
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

	public bool hasIsBanker() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasIsWiner() {
		return (this.__flag[0] & 32) != 0;
	}

	public int paigowMahjongCount() {
		return this.paigow_mahjong.Count;
	}

	public bool hasPaigowMahjong() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 128) != 0;
	}

	public bool hasPour() {
		return (this.__flag[1] & 1) != 0;
	}

	public bool hasEarnings() {
		return (this.__flag[1] & 2) != 0;
	}

	public bool hasLoopEarnings() {
		return (this.__flag[1] & 4) != 0;
	}

	public bool hasTotalEarnings() {
		return (this.__flag[1] & 8) != 0;
	}

	public bool hasGold() {
		return (this.__flag[1] & 16) != 0;
	}

	public bool hasSeatStatus() {
		return (this.__flag[1] & 32) != 0;
	}

	public bool hasIsDismiss() {
		return (this.__flag[1] & 64) != 0;
	}

	public int historyPokerCount() {
		return this.historyPoker.Count;
	}

	public bool hasHistoryPoker() {
		return (this.__flag[1] & 128) != 0;
	}

	public bool hasIsGrabBanker() {
		return (this.__flag[2] & 1) != 0;
	}

	public bool hasIsCutPoker() {
		return (this.__flag[2] & 2) != 0;
	}

	public bool hasIsCutPan() {
		return (this.__flag[2] & 4) != 0;
	}

	public List<PAIGOW_MAHJONG> getPaigowMahjongList() {
		return this.paigow_mahjong;
	}

	public List<PAIGOW_MAHJONG> getHistoryPokerList() {
		return this.historyPoker;
	}

}
}

