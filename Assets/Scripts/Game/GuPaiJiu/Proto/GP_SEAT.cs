//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:16 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class GP_SEAT { 

	public const int CODE = 7002; 

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

	private SEAT_STATUS _status; 

	public SEAT_STATUS status { 
		set { 
			if(!this.hasStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._status = value;
		} 
		get { 
			return this._status;
		} 
	} 

	private List<GP_POKER> pokerList = new List<GP_POKER>(); 

	public GP_POKER getPokerList(int index) { 
			return this.pokerList[index];
	} 
	
	public void addPokerList(GP_POKER value) { 
			if(!this.hasPokerList()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this.pokerList.Add(value);
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

	private bool _isBanker; 

	public bool isBanker { 
		set { 
			if(!this.hasIsBanker()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
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
	    		this.__flag[1] = (byte) (this.__flag[1] | 2);
			}
			this._isDismiss = value;
		} 
		get { 
			return this._isDismiss;
		} 
	} 

	private bool _isWin; 

	public bool isWin { 
		set { 
			if(!this.hasIsWin()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 4);
			}
			this._isWin = value;
		} 
		get { 
			return this._isWin;
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

	private int _loopEarnings; 

	public int loopEarnings { 
		set { 
			if(!this.hasLoopEarnings()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 16);
			}
			this._loopEarnings = value;
		} 
		get { 
			return this._loopEarnings;
		} 
	} 

	private List<GP_POKER> historyPokerList = new List<GP_POKER>(); 

	public GP_POKER getHistoryPokerList(int index) { 
			return this.historyPokerList[index];
	} 
	
	public void addHistoryPokerList(GP_POKER value) { 
			if(!this.hasHistoryPokerList()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 32);
			}
			this.historyPokerList.Add(value);
	} 

	private List<int> pourList = new List<int>(); 

	public int getPourList(int index) { 
			return this.pourList[index];
	} 
	
	public void addPourList(int value) { 
			if(!this.hasPourList()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 64);
			}
			this.pourList.Add(value);
	} 

	private string _ipAddr; 

	public string ipAddr { 
		set { 
			if(!this.hasIpAddr()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 128);
			}
			this._ipAddr = value;
		} 
		get { 
			return this._ipAddr;
		} 
	} 

	private float _longitude; 

	public float longitude { 
		set { 
			if(!this.hasLongitude()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 1);
			}
			this._longitude = value;
		} 
		get { 
			return this._longitude;
		} 
	} 

	private float _latitude; 

	public float latitude { 
		set { 
			if(!this.hasLatitude()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 2);
			}
			this._latitude = value;
		} 
		get { 
			return this._latitude;
		} 
	} 

	private bool _isAfk; 

	public bool isAfk { 
		set { 
			if(!this.hasIsAfk()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 4);
			}
			this._isAfk = value;
		} 
		get { 
			return this._isAfk;
		} 
	} 

	private List<int> drawPokerList = new List<int>(); 

	public int getDrawPokerList(int index) { 
			return this.drawPokerList[index];
	} 
	
	public void addDrawPokerList(int value) { 
			if(!this.hasDrawPokerList()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 8);
			}
			this.drawPokerList.Add(value);
	} 

	private int _isGrabBanker; 

	public int isGrabBanker { 
		set { 
			if(!this.hasIsGrabBanker()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 16);
			}
			this._isGrabBanker = value;
		} 
		get { 
			return this._isGrabBanker;
		} 
	} 

	private int _isCuoPai; 

	public int isCuoPai { 
		set { 
			if(!this.hasIsCuoPai()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 32);
			}
			this._isCuoPai = value;
		} 
		get { 
			return this._isCuoPai;
		} 
	} 

	private List<GP_POKER> maxPokerList = new List<GP_POKER>(); 

	public GP_POKER getMaxPokerList(int index) { 
			return this.maxPokerList[index];
	} 
	
	public void addMaxPokerList(GP_POKER value) { 
			if(!this.hasMaxPokerList()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 64);
			}
			this.maxPokerList.Add(value);
	} 

	public static GP_SEAT newBuilder() { 
		return new GP_SEAT(); 
	} 

	public static GP_SEAT decode(byte[] data) { 
		GP_SEAT proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[23]; 

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

		if(this.hasStatus()) {
			bytes[5] = ByteBuffer.allocate(1);
			bytes[5].put((byte) this.status);
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

		if(this.hasPos()) {
			bytes[7] = ByteBuffer.allocate(4);
			bytes[7].putInt(this.pos);
			total += bytes[7].limit();
		}

		if(this.hasIsBanker()) {
			bytes[8] = ByteBuffer.allocate(1);
			if(this.isBanker) {
				bytes[8].put((byte) 1);
			}else{
				bytes[8].put((byte) 0);
			}
			total += bytes[8].limit();
		}

		if(this.hasIsDismiss()) {
			bytes[9] = ByteBuffer.allocate(1);
			if(this.isDismiss) {
				bytes[9].put((byte) 1);
			}else{
				bytes[9].put((byte) 0);
			}
			total += bytes[9].limit();
		}

		if(this.hasIsWin()) {
			bytes[10] = ByteBuffer.allocate(1);
			if(this.isWin) {
				bytes[10].put((byte) 1);
			}else{
				bytes[10].put((byte) 0);
			}
			total += bytes[10].limit();
		}

		if(this.hasEarnings()) {
			bytes[11] = ByteBuffer.allocate(4);
			bytes[11].putInt(this.earnings);
			total += bytes[11].limit();
		}

		if(this.hasLoopEarnings()) {
			bytes[12] = ByteBuffer.allocate(4);
			bytes[12].putInt(this.loopEarnings);
			total += bytes[12].limit();
		}

		if(this.hasHistoryPokerList()) {
				int length = 0;
				for(int i=0, len=this.historyPokerList.Count; i<len; i++) {
					length += this.historyPokerList[i].encode().Length;
				}
				bytes[13] = ByteBuffer.allocate(this.historyPokerList.Count * 4 + length + 2);
				bytes[13].putShort((short) this.historyPokerList.Count);
				for(int i=0, len=this.historyPokerList.Count; i<len; i++) {
					byte[] _byte = this.historyPokerList[i].encode();
					bytes[13].putInt(_byte.Length);
					bytes[13].put(_byte);
				}
			total += bytes[13].limit();
		}

		if(this.hasPourList()) {
			bytes[14] = ByteBuffer.allocate(this.pourList.Count * 4 + 2);
			bytes[14].putShort((short) this.pourList.Count);
			for(int i=0, len=this.pourList.Count; i<len; i++) {
				bytes[14].putInt(this.pourList[i]);
			}
			total += bytes[14].limit();
		}

		if(this.hasIpAddr()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.ipAddr);
			    short len = (short) _byte.Length;
			    bytes[15] = ByteBuffer.allocate(2 + len);
			    bytes[15].putShort(len);
				bytes[15].put(_byte);
			total += bytes[15].limit();
		}

		if(this.hasLongitude()) {
			bytes[16] = ByteBuffer.allocate(4);
			bytes[16].putFloat(this.longitude);
			total += bytes[16].limit();
		}

		if(this.hasLatitude()) {
			bytes[17] = ByteBuffer.allocate(4);
			bytes[17].putFloat(this.latitude);
			total += bytes[17].limit();
		}

		if(this.hasIsAfk()) {
			bytes[18] = ByteBuffer.allocate(1);
			if(this.isAfk) {
				bytes[18].put((byte) 1);
			}else{
				bytes[18].put((byte) 0);
			}
			total += bytes[18].limit();
		}

		if(this.hasDrawPokerList()) {
			bytes[19] = ByteBuffer.allocate(this.drawPokerList.Count * 4 + 2);
			bytes[19].putShort((short) this.drawPokerList.Count);
			for(int i=0, len=this.drawPokerList.Count; i<len; i++) {
				bytes[19].putInt(this.drawPokerList[i]);
			}
			total += bytes[19].limit();
		}

		if(this.hasIsGrabBanker()) {
			bytes[20] = ByteBuffer.allocate(4);
			bytes[20].putInt(this.isGrabBanker);
			total += bytes[20].limit();
		}

		if(this.hasIsCuoPai()) {
			bytes[21] = ByteBuffer.allocate(4);
			bytes[21].putInt(this.isCuoPai);
			total += bytes[21].limit();
		}

		if(this.hasMaxPokerList()) {
				int length = 0;
				for(int i=0, len=this.maxPokerList.Count; i<len; i++) {
					length += this.maxPokerList[i].encode().Length;
				}
				bytes[22] = ByteBuffer.allocate(this.maxPokerList.Count * 4 + length + 2);
				bytes[22].putShort((short) this.maxPokerList.Count);
				for(int i=0, len=this.maxPokerList.Count; i<len; i++) {
					byte[] _byte = this.maxPokerList[i].encode();
					bytes[22].putInt(_byte.Length);
					bytes[22].put(_byte);
				}
			total += bytes[22].limit();
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

		if(this.hasGold()) {
			this.gold = buf.getInt();
		}

		if(this.hasStatus()) {
			this.status = (SEAT_STATUS) buf.get();
		}

		if(this.hasPokerList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.pokerList.Add(GP_POKER.decode(bytes));
			}
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
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

		if(this.hasIsWin()) {
			if(buf.get() == 1) {
				this.isWin = true;
			}else{
				this.isWin = false;
			}
		}

		if(this.hasEarnings()) {
			this.earnings = buf.getInt();
		}

		if(this.hasLoopEarnings()) {
			this.loopEarnings = buf.getInt();
		}

		if(this.hasHistoryPokerList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.historyPokerList.Add(GP_POKER.decode(bytes));
			}
		}

		if(this.hasPourList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.pourList.Add(buf.getInt());
			}
		}

		if(this.hasIpAddr()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.ipAddr = System.Text.Encoding.UTF8.GetString(bytes);
		}

		if(this.hasLongitude()) {
			this.longitude = buf.getFloat();
		}

		if(this.hasLatitude()) {
			this.latitude = buf.getFloat();
		}

		if(this.hasIsAfk()) {
			if(buf.get() == 1) {
				this.isAfk = true;
			}else{
				this.isAfk = false;
			}
		}

		if(this.hasDrawPokerList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.drawPokerList.Add(buf.getInt());
			}
		}

		if(this.hasIsGrabBanker()) {
			this.isGrabBanker = buf.getInt();
		}

		if(this.hasIsCuoPai()) {
			this.isCuoPai = buf.getInt();
		}

		if(this.hasMaxPokerList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.maxPokerList.Add(GP_POKER.decode(bytes));
			}
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

	public bool hasStatus() {
		return (this.__flag[0] & 32) != 0;
	}

	public int pokerListCount() {
		return this.pokerList.Count;
	}

	public bool hasPokerList() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 128) != 0;
	}

	public bool hasIsBanker() {
		return (this.__flag[1] & 1) != 0;
	}

	public bool hasIsDismiss() {
		return (this.__flag[1] & 2) != 0;
	}

	public bool hasIsWin() {
		return (this.__flag[1] & 4) != 0;
	}

	public bool hasEarnings() {
		return (this.__flag[1] & 8) != 0;
	}

	public bool hasLoopEarnings() {
		return (this.__flag[1] & 16) != 0;
	}

	public int historyPokerListCount() {
		return this.historyPokerList.Count;
	}

	public bool hasHistoryPokerList() {
		return (this.__flag[1] & 32) != 0;
	}

	public int pourListCount() {
		return this.pourList.Count;
	}

	public bool hasPourList() {
		return (this.__flag[1] & 64) != 0;
	}

	public bool hasIpAddr() {
		return (this.__flag[1] & 128) != 0;
	}

	public bool hasLongitude() {
		return (this.__flag[2] & 1) != 0;
	}

	public bool hasLatitude() {
		return (this.__flag[2] & 2) != 0;
	}

	public bool hasIsAfk() {
		return (this.__flag[2] & 4) != 0;
	}

	public int drawPokerListCount() {
		return this.drawPokerList.Count;
	}

	public bool hasDrawPokerList() {
		return (this.__flag[2] & 8) != 0;
	}

	public bool hasIsGrabBanker() {
		return (this.__flag[2] & 16) != 0;
	}

	public bool hasIsCuoPai() {
		return (this.__flag[2] & 32) != 0;
	}

	public int maxPokerListCount() {
		return this.maxPokerList.Count;
	}

	public bool hasMaxPokerList() {
		return (this.__flag[2] & 64) != 0;
	}

	public List<GP_POKER> getPokerListList() {
		return this.pokerList;
	}

	public List<GP_POKER> getHistoryPokerListList() {
		return this.historyPokerList;
	}

	public List<int> getPourListList() {
		return this.pourList;
	}

	public List<int> getDrawPokerListList() {
		return this.drawPokerList;
	}

	public List<GP_POKER> getMaxPokerListList() {
		return this.maxPokerList;
	}

}
}

