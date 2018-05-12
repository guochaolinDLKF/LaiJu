using System.Collections.Generic;

namespace proto.mahjong {

public class OP_SEAT_FULL { 

	public const int CODE = 1003; 

	private byte[] __flag = new byte[16]; 

	private bool _isDismiss; 

	public bool isDismiss { 
		set { 
			if(!this.hasIsDismiss()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._isDismiss = value;
		} 
		get { 
			return this._isDismiss;
		} 
	} 

	private string _ipaddr; 

	public string ipaddr { 
		set { 
			if(!this.hasIpaddr()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._ipaddr = value;
		} 
		get { 
			return this._ipaddr;
		} 
	} 

	private byte _gender; 

	public byte gender { 
		set { 
			if(!this.hasGender()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._gender = value;
		} 
		get { 
			return this._gender;
		} 
	} 

	private string _avatar; 

	public string avatar { 
		set { 
			if(!this.hasAvatar()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._avatar = value;
		} 
		get { 
			return this._avatar;
		} 
	} 

	private string _nickname; 

	public string nickname { 
		set { 
			if(!this.hasNickname()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._nickname = value;
		} 
		get { 
			return this._nickname;
		} 
	} 

	private long _countdown; 

	public long countdown { 
		set { 
			if(!this.hasCountdown()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._countdown = value;
		} 
		get { 
			return this._countdown;
		} 
	} 

	private List<OP_POKER> universal = new List<OP_POKER>(); 

	public OP_POKER getUniversal(int index) { 
			return this.universal[index];
	} 
	
	public void addUniversal(OP_POKER value) { 
			if(!this.hasUniversal()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this.universal.Add(value);
	} 

	private bool _isBanker; 

	public bool isBanker { 
		set { 
			if(!this.hasIsBanker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this._isBanker = value;
		} 
		get { 
			return this._isBanker;
		} 
	} 

	private List<OP_POKER_SETTLE> scores = new List<OP_POKER_SETTLE>(); 

	public OP_POKER_SETTLE getScores(int index) { 
			return this.scores[index];
	} 
	
	public void addScores(OP_POKER_SETTLE value) { 
			if(!this.hasScores()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this.scores.Add(value);
	} 

	private List<OP_POKER_SETTLE> incomes = new List<OP_POKER_SETTLE>(); 

	public OP_POKER_SETTLE getIncomes(int index) { 
			return this.incomes[index];
	} 
	
	public void addIncomes(OP_POKER_SETTLE value) { 
			if(!this.hasIncomes()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 2);
			}
			this.incomes.Add(value);
	} 

	private List<OP_POKER_GROUP> usePokerGroup = new List<OP_POKER_GROUP>(); 

	public OP_POKER_GROUP getUsePokerGroup(int index) { 
			return this.usePokerGroup[index];
	} 
	
	public void addUsePokerGroup(OP_POKER_GROUP value) { 
			if(!this.hasUsePokerGroup()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 4);
			}
			this.usePokerGroup.Add(value);
	} 

	private int _probMulti; 

	public int probMulti { 
		set { 
			if(!this.hasProbMulti()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 8);
			}
			this._probMulti = value;
		} 
		get { 
			return this._probMulti;
		} 
	} 

	private int _settle; 

	public int settle { 
		set { 
			if(!this.hasSettle()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 16);
			}
			this._settle = value;
		} 
		get { 
			return this._settle;
		} 
	} 

	private bool _isWaiver; 

	public bool isWaiver { 
		set { 
			if(!this.hasIsWaiver()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 32);
			}
			this._isWaiver = value;
		} 
		get { 
			return this._isWaiver;
		} 
	} 

	private bool _isWiner; 

	public bool isWiner { 
		set { 
			if(!this.hasIsWiner()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 64);
			}
			this._isWiner = value;
		} 
		get { 
			return this._isWiner;
		} 
	} 

	private bool _isLoser; 

	public bool isLoser { 
		set { 
			if(!this.hasIsLoser()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 128);
			}
			this._isLoser = value;
		} 
		get { 
			return this._isLoser;
		} 
	} 

	private int _playerId; 

	public int playerId { 
		set { 
			if(!this.hasPlayerId()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 1);
			}
			this._playerId = value;
		} 
		get { 
			return this._playerId;
		} 
	} 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 2);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	private int _gold; 

	public int gold { 
		set { 
			if(!this.hasGold()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 4);
			}
			this._gold = value;
		} 
		get { 
			return this._gold;
		} 
	} 

	private ENUM_SEAT_STATUS _status; 

	public ENUM_SEAT_STATUS status { 
		set { 
			if(!this.hasStatus()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 8);
			}
			this._status = value;
		} 
		get { 
			return this._status;
		} 
	} 

	private int _handCount; 

	public int handCount { 
		set { 
			if(!this.hasHandCount()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 16);
			}
			this._handCount = value;
		} 
		get { 
			return this._handCount;
		} 
	} 

	private OP_POKER _hitPoker; 

	public OP_POKER hitPoker { 
		set { 
			if(!this.hasHitPoker()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 32);
			}
			this._hitPoker = value;
		} 
		get { 
			return this._hitPoker;
		} 
	} 

	private int _pokerAmount; 

	public int pokerAmount { 
		set { 
			if(!this.hasPokerAmount()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 64);
			}
			this._pokerAmount = value;
		} 
		get { 
			return this._pokerAmount;
		} 
	} 

	private List<OP_POKER> desktop = new List<OP_POKER>(); 

	public OP_POKER getDesktop(int index) { 
			return this.desktop[index];
	} 
	
	public void addDesktop(OP_POKER value) { 
			if(!this.hasDesktop()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 128);
			}
			this.desktop.Add(value);
	} 

	private List<OP_POKER> poker = new List<OP_POKER>(); 

	public OP_POKER getPoker(int index) { 
			return this.poker[index];
	} 
	
	public void addPoker(OP_POKER value) { 
			if(!this.hasPoker()) {
	    		this.__flag[3] = (byte) (this.__flag[3] | 1);
			}
			this.poker.Add(value);
	} 

	private bool _isTrustee; 

	public bool isTrustee { 
		set { 
			if(!this.hasIsTrustee()) {
	    		this.__flag[3] = (byte) (this.__flag[3] | 2);
			}
			this._isTrustee = value;
		} 
		get { 
			return this._isTrustee;
		} 
	} 

	private bool _isListen; 

	public bool isListen { 
		set { 
			if(!this.hasIsListen()) {
	    		this.__flag[3] = (byte) (this.__flag[3] | 4);
			}
			this._isListen = value;
		} 
		get { 
			return this._isListen;
		} 
	} 

	private int _huScore; 

	public int huScore { 
		set { 
			if(!this.hasHuScore()) {
	    		this.__flag[3] = (byte) (this.__flag[3] | 8);
			}
			this._huScore = value;
		} 
		get { 
			return this._huScore;
		} 
	} 

	private List<OP_POKER_GROUP> keepPokerGroup = new List<OP_POKER_GROUP>(); 

	public OP_POKER_GROUP getKeepPokerGroup(int index) { 
			return this.keepPokerGroup[index];
	} 
	
	public void addKeepPokerGroup(OP_POKER_GROUP value) { 
			if(!this.hasKeepPokerGroup()) {
	    		this.__flag[3] = (byte) (this.__flag[3] | 16);
			}
			this.keepPokerGroup.Add(value);
	} 

	private bool _isLockListen; 

	public bool isLockListen { 
		set { 
			if(!this.hasIsLockListen()) {
	    		this.__flag[3] = (byte) (this.__flag[3] | 32);
			}
			this._isLockListen = value;
		} 
		get { 
			return this._isLockListen;
		} 
	} 

	private int _wind; 

	public int wind { 
		set { 
			if(!this.hasWind()) {
	    		this.__flag[3] = (byte) (this.__flag[3] | 64);
			}
			this._wind = value;
		} 
		get { 
			return this._wind;
		} 
	} 

	private float _longitude; 

	public float longitude { 
		set { 
			if(!this.hasLongitude()) {
	    		this.__flag[3] = (byte) (this.__flag[3] | 128);
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
	    		this.__flag[4] = (byte) (this.__flag[4] | 1);
			}
			this._latitude = value;
		} 
		get { 
			return this._latitude;
		} 
	} 

	private long _online; 

	public long online { 
		set { 
			if(!this.hasOnline()) {
	    		this.__flag[4] = (byte) (this.__flag[4] | 2);
			}
			this._online = value;
		} 
		get { 
			return this._online;
		} 
	} 

	private bool _isAfk; 

	public bool isAfk { 
		set { 
			if(!this.hasIsAfk()) {
	    		this.__flag[4] = (byte) (this.__flag[4] | 4);
			}
			this._isAfk = value;
		} 
		get { 
			return this._isAfk;
		} 
	} 

	private List<OP_POKER_GROUP> dingPokerGroup = new List<OP_POKER_GROUP>(); 

	public OP_POKER_GROUP getDingPokerGroup(int index) { 
			return this.dingPokerGroup[index];
	} 
	
	public void addDingPokerGroup(OP_POKER_GROUP value) { 
			if(!this.hasDingPokerGroup()) {
	    		this.__flag[4] = (byte) (this.__flag[4] | 8);
			}
			this.dingPokerGroup.Add(value);
	} 

	private bool _isDouble; 

	public bool isDouble { 
		set { 
			if(!this.hasIsDouble()) {
	    		this.__flag[4] = (byte) (this.__flag[4] | 16);
			}
			this._isDouble = value;
		} 
		get { 
			return this._isDouble;
		} 
	} 

	private ENUM_DISMISS_STATUS _dismiss; 

	public ENUM_DISMISS_STATUS dismiss { 
		set { 
			if(!this.hasDismiss()) {
	    		this.__flag[4] = (byte) (this.__flag[4] | 32);
			}
			this._dismiss = value;
		} 
		get { 
			return this._dismiss;
		} 
	} 

	private List<int> kou = new List<int>(); 

	public int getKou(int index) { 
			return this.kou[index];
	} 
	
	public void addKou(int value) { 
			if(!this.hasKou()) {
	    		this.__flag[4] = (byte) (this.__flag[4] | 64);
			}
			this.kou.Add(value);
	} 

	private List<OP_POKER> swap = new List<OP_POKER>(); 

	public OP_POKER getSwap(int index) { 
			return this.swap[index];
	} 
	
	public void addSwap(OP_POKER value) { 
			if(!this.hasSwap()) {
	    		this.__flag[4] = (byte) (this.__flag[4] | 128);
			}
			this.swap.Add(value);
	} 

	private int _lackColor; 

	public int lackColor { 
		set { 
			if(!this.hasLackColor()) {
	    		this.__flag[5] = (byte) (this.__flag[5] | 1);
			}
			this._lackColor = value;
		} 
		get { 
			return this._lackColor;
		} 
	} 

	public static OP_SEAT_FULL newBuilder() { 
		return new OP_SEAT_FULL(); 
	} 

	public static OP_SEAT_FULL decode(byte[] data) { 
		OP_SEAT_FULL proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[41]; 

		int total = 0;
		if(this.hasIsDismiss()) {
			bytes[0] = ByteBuffer.allocate(1);
			if(this.isDismiss) {
				bytes[0].put((byte) 1);
			}else{
				bytes[0].put((byte) 0);
			}
			total += bytes[0].limit();
		}

		if(this.hasIpaddr()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.ipaddr);
			    short len = (short) _byte.Length;
			    bytes[1] = ByteBuffer.allocate(2 + len);
			    bytes[1].putShort(len);
				bytes[1].put(_byte);
			total += bytes[1].limit();
		}

		if(this.hasGender()) {
			bytes[2] = ByteBuffer.allocate(1);
			bytes[2].put(this.gender);
			total += bytes[2].limit();
		}

		if(this.hasAvatar()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.avatar);
			    short len = (short) _byte.Length;
			    bytes[3] = ByteBuffer.allocate(2 + len);
			    bytes[3].putShort(len);
				bytes[3].put(_byte);
			total += bytes[3].limit();
		}

		if(this.hasNickname()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.nickname);
			    short len = (short) _byte.Length;
			    bytes[4] = ByteBuffer.allocate(2 + len);
			    bytes[4].putShort(len);
				bytes[4].put(_byte);
			total += bytes[4].limit();
		}

		if(this.hasCountdown()) {
			bytes[5] = ByteBuffer.allocate(8);
			bytes[5].putLong(this.countdown);
			total += bytes[5].limit();
		}

		if(this.hasUniversal()) {
				int length = 0;
				for(int i=0, len=this.universal.Count; i<len; i++) {
					length += this.universal[i].encode().Length;
				}
				bytes[6] = ByteBuffer.allocate(this.universal.Count * 4 + length + 2);
				bytes[6].putShort((short) this.universal.Count);
				for(int i=0, len=this.universal.Count; i<len; i++) {
					byte[] _byte = this.universal[i].encode();
					bytes[6].putInt(_byte.Length);
					bytes[6].put(_byte);
				}
			total += bytes[6].limit();
		}

		if(this.hasIsBanker()) {
			bytes[7] = ByteBuffer.allocate(1);
			if(this.isBanker) {
				bytes[7].put((byte) 1);
			}else{
				bytes[7].put((byte) 0);
			}
			total += bytes[7].limit();
		}

		if(this.hasScores()) {
				int length = 0;
				for(int i=0, len=this.scores.Count; i<len; i++) {
					length += this.scores[i].encode().Length;
				}
				bytes[8] = ByteBuffer.allocate(this.scores.Count * 4 + length + 2);
				bytes[8].putShort((short) this.scores.Count);
				for(int i=0, len=this.scores.Count; i<len; i++) {
					byte[] _byte = this.scores[i].encode();
					bytes[8].putInt(_byte.Length);
					bytes[8].put(_byte);
				}
			total += bytes[8].limit();
		}

		if(this.hasIncomes()) {
				int length = 0;
				for(int i=0, len=this.incomes.Count; i<len; i++) {
					length += this.incomes[i].encode().Length;
				}
				bytes[9] = ByteBuffer.allocate(this.incomes.Count * 4 + length + 2);
				bytes[9].putShort((short) this.incomes.Count);
				for(int i=0, len=this.incomes.Count; i<len; i++) {
					byte[] _byte = this.incomes[i].encode();
					bytes[9].putInt(_byte.Length);
					bytes[9].put(_byte);
				}
			total += bytes[9].limit();
		}

		if(this.hasUsePokerGroup()) {
				int length = 0;
				for(int i=0, len=this.usePokerGroup.Count; i<len; i++) {
					length += this.usePokerGroup[i].encode().Length;
				}
				bytes[10] = ByteBuffer.allocate(this.usePokerGroup.Count * 4 + length + 2);
				bytes[10].putShort((short) this.usePokerGroup.Count);
				for(int i=0, len=this.usePokerGroup.Count; i<len; i++) {
					byte[] _byte = this.usePokerGroup[i].encode();
					bytes[10].putInt(_byte.Length);
					bytes[10].put(_byte);
				}
			total += bytes[10].limit();
		}

		if(this.hasProbMulti()) {
			bytes[11] = ByteBuffer.allocate(4);
			bytes[11].putInt(this.probMulti);
			total += bytes[11].limit();
		}

		if(this.hasSettle()) {
			bytes[12] = ByteBuffer.allocate(4);
			bytes[12].putInt(this.settle);
			total += bytes[12].limit();
		}

		if(this.hasIsWaiver()) {
			bytes[13] = ByteBuffer.allocate(1);
			if(this.isWaiver) {
				bytes[13].put((byte) 1);
			}else{
				bytes[13].put((byte) 0);
			}
			total += bytes[13].limit();
		}

		if(this.hasIsWiner()) {
			bytes[14] = ByteBuffer.allocate(1);
			if(this.isWiner) {
				bytes[14].put((byte) 1);
			}else{
				bytes[14].put((byte) 0);
			}
			total += bytes[14].limit();
		}

		if(this.hasIsLoser()) {
			bytes[15] = ByteBuffer.allocate(1);
			if(this.isLoser) {
				bytes[15].put((byte) 1);
			}else{
				bytes[15].put((byte) 0);
			}
			total += bytes[15].limit();
		}

		if(this.hasPlayerId()) {
			bytes[16] = ByteBuffer.allocate(4);
			bytes[16].putInt(this.playerId);
			total += bytes[16].limit();
		}

		if(this.hasPos()) {
			bytes[17] = ByteBuffer.allocate(4);
			bytes[17].putInt(this.pos);
			total += bytes[17].limit();
		}

		if(this.hasGold()) {
			bytes[18] = ByteBuffer.allocate(4);
			bytes[18].putInt(this.gold);
			total += bytes[18].limit();
		}

		if(this.hasStatus()) {
			bytes[19] = ByteBuffer.allocate(1);
			bytes[19].put((byte) this.status);
			total += bytes[19].limit();
		}

		if(this.hasHandCount()) {
			bytes[20] = ByteBuffer.allocate(4);
			bytes[20].putInt(this.handCount);
			total += bytes[20].limit();
		}

		if(this.hasHitPoker()) {
			byte[] _byte = this.hitPoker.encode();
			int len = _byte.Length;
			bytes[21] = ByteBuffer.allocate(4 + len);
			bytes[21].putInt(len);
			bytes[21].put(_byte);
			total += bytes[21].limit();
		}

		if(this.hasPokerAmount()) {
			bytes[22] = ByteBuffer.allocate(4);
			bytes[22].putInt(this.pokerAmount);
			total += bytes[22].limit();
		}

		if(this.hasDesktop()) {
				int length = 0;
				for(int i=0, len=this.desktop.Count; i<len; i++) {
					length += this.desktop[i].encode().Length;
				}
				bytes[23] = ByteBuffer.allocate(this.desktop.Count * 4 + length + 2);
				bytes[23].putShort((short) this.desktop.Count);
				for(int i=0, len=this.desktop.Count; i<len; i++) {
					byte[] _byte = this.desktop[i].encode();
					bytes[23].putInt(_byte.Length);
					bytes[23].put(_byte);
				}
			total += bytes[23].limit();
		}

		if(this.hasPoker()) {
				int length = 0;
				for(int i=0, len=this.poker.Count; i<len; i++) {
					length += this.poker[i].encode().Length;
				}
				bytes[24] = ByteBuffer.allocate(this.poker.Count * 4 + length + 2);
				bytes[24].putShort((short) this.poker.Count);
				for(int i=0, len=this.poker.Count; i<len; i++) {
					byte[] _byte = this.poker[i].encode();
					bytes[24].putInt(_byte.Length);
					bytes[24].put(_byte);
				}
			total += bytes[24].limit();
		}

		if(this.hasIsTrustee()) {
			bytes[25] = ByteBuffer.allocate(1);
			if(this.isTrustee) {
				bytes[25].put((byte) 1);
			}else{
				bytes[25].put((byte) 0);
			}
			total += bytes[25].limit();
		}

		if(this.hasIsListen()) {
			bytes[26] = ByteBuffer.allocate(1);
			if(this.isListen) {
				bytes[26].put((byte) 1);
			}else{
				bytes[26].put((byte) 0);
			}
			total += bytes[26].limit();
		}

		if(this.hasHuScore()) {
			bytes[27] = ByteBuffer.allocate(4);
			bytes[27].putInt(this.huScore);
			total += bytes[27].limit();
		}

		if(this.hasKeepPokerGroup()) {
				int length = 0;
				for(int i=0, len=this.keepPokerGroup.Count; i<len; i++) {
					length += this.keepPokerGroup[i].encode().Length;
				}
				bytes[28] = ByteBuffer.allocate(this.keepPokerGroup.Count * 4 + length + 2);
				bytes[28].putShort((short) this.keepPokerGroup.Count);
				for(int i=0, len=this.keepPokerGroup.Count; i<len; i++) {
					byte[] _byte = this.keepPokerGroup[i].encode();
					bytes[28].putInt(_byte.Length);
					bytes[28].put(_byte);
				}
			total += bytes[28].limit();
		}

		if(this.hasIsLockListen()) {
			bytes[29] = ByteBuffer.allocate(1);
			if(this.isLockListen) {
				bytes[29].put((byte) 1);
			}else{
				bytes[29].put((byte) 0);
			}
			total += bytes[29].limit();
		}

		if(this.hasWind()) {
			bytes[30] = ByteBuffer.allocate(4);
			bytes[30].putInt(this.wind);
			total += bytes[30].limit();
		}

		if(this.hasLongitude()) {
			bytes[31] = ByteBuffer.allocate(4);
			bytes[31].putFloat(this.longitude);
			total += bytes[31].limit();
		}

		if(this.hasLatitude()) {
			bytes[32] = ByteBuffer.allocate(4);
			bytes[32].putFloat(this.latitude);
			total += bytes[32].limit();
		}

		if(this.hasOnline()) {
			bytes[33] = ByteBuffer.allocate(8);
			bytes[33].putLong(this.online);
			total += bytes[33].limit();
		}

		if(this.hasIsAfk()) {
			bytes[34] = ByteBuffer.allocate(1);
			if(this.isAfk) {
				bytes[34].put((byte) 1);
			}else{
				bytes[34].put((byte) 0);
			}
			total += bytes[34].limit();
		}

		if(this.hasDingPokerGroup()) {
				int length = 0;
				for(int i=0, len=this.dingPokerGroup.Count; i<len; i++) {
					length += this.dingPokerGroup[i].encode().Length;
				}
				bytes[35] = ByteBuffer.allocate(this.dingPokerGroup.Count * 4 + length + 2);
				bytes[35].putShort((short) this.dingPokerGroup.Count);
				for(int i=0, len=this.dingPokerGroup.Count; i<len; i++) {
					byte[] _byte = this.dingPokerGroup[i].encode();
					bytes[35].putInt(_byte.Length);
					bytes[35].put(_byte);
				}
			total += bytes[35].limit();
		}

		if(this.hasIsDouble()) {
			bytes[36] = ByteBuffer.allocate(1);
			if(this.isDouble) {
				bytes[36].put((byte) 1);
			}else{
				bytes[36].put((byte) 0);
			}
			total += bytes[36].limit();
		}

		if(this.hasDismiss()) {
			bytes[37] = ByteBuffer.allocate(1);
			bytes[37].put((byte) this.dismiss);
			total += bytes[37].limit();
		}

		if(this.hasKou()) {
			bytes[38] = ByteBuffer.allocate(this.kou.Count * 4 + 2);
			bytes[38].putShort((short) this.kou.Count);
			for(int i=0, len=this.kou.Count; i<len; i++) {
				bytes[38].putInt(this.kou[i]);
			}
			total += bytes[38].limit();
		}

		if(this.hasSwap()) {
				int length = 0;
				for(int i=0, len=this.swap.Count; i<len; i++) {
					length += this.swap[i].encode().Length;
				}
				bytes[39] = ByteBuffer.allocate(this.swap.Count * 4 + length + 2);
				bytes[39].putShort((short) this.swap.Count);
				for(int i=0, len=this.swap.Count; i<len; i++) {
					byte[] _byte = this.swap[i].encode();
					bytes[39].putInt(_byte.Length);
					bytes[39].put(_byte);
				}
			total += bytes[39].limit();
		}

		if(this.hasLackColor()) {
			bytes[40] = ByteBuffer.allocate(4);
			bytes[40].putInt(this.lackColor);
			total += bytes[40].limit();
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
		  
		if(this.hasIsDismiss()) {
			if(buf.get() == 1) {
				this.isDismiss = true;
			}else{
				this.isDismiss = false;
			}
		}

		if(this.hasIpaddr()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.ipaddr = System.Text.Encoding.UTF8.GetString(bytes);
		}

		if(this.hasGender()) {
			this.gender = buf.get();
		}

		if(this.hasAvatar()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.avatar = System.Text.Encoding.UTF8.GetString(bytes);
		}

		if(this.hasNickname()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.nickname = System.Text.Encoding.UTF8.GetString(bytes);
		}

		if(this.hasCountdown()) {
			this.countdown = buf.getLong();
		}

		if(this.hasUniversal()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.universal.Add(OP_POKER.decode(bytes));
			}
		}

		if(this.hasIsBanker()) {
			if(buf.get() == 1) {
				this.isBanker = true;
			}else{
				this.isBanker = false;
			}
		}

		if(this.hasScores()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.scores.Add(OP_POKER_SETTLE.decode(bytes));
			}
		}

		if(this.hasIncomes()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.incomes.Add(OP_POKER_SETTLE.decode(bytes));
			}
		}

		if(this.hasUsePokerGroup()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.usePokerGroup.Add(OP_POKER_GROUP.decode(bytes));
			}
		}

		if(this.hasProbMulti()) {
			this.probMulti = buf.getInt();
		}

		if(this.hasSettle()) {
			this.settle = buf.getInt();
		}

		if(this.hasIsWaiver()) {
			if(buf.get() == 1) {
				this.isWaiver = true;
			}else{
				this.isWaiver = false;
			}
		}

		if(this.hasIsWiner()) {
			if(buf.get() == 1) {
				this.isWiner = true;
			}else{
				this.isWiner = false;
			}
		}

		if(this.hasIsLoser()) {
			if(buf.get() == 1) {
				this.isLoser = true;
			}else{
				this.isLoser = false;
			}
		}

		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasGold()) {
			this.gold = buf.getInt();
		}

		if(this.hasStatus()) {
			this.status = (ENUM_SEAT_STATUS) buf.get();
		}

		if(this.hasHandCount()) {
			this.handCount = buf.getInt();
		}

		if(this.hasHitPoker()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.hitPoker = OP_POKER.decode(bytes);
		}

		if(this.hasPokerAmount()) {
			this.pokerAmount = buf.getInt();
		}

		if(this.hasDesktop()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.desktop.Add(OP_POKER.decode(bytes));
			}
		}

		if(this.hasPoker()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.poker.Add(OP_POKER.decode(bytes));
			}
		}

		if(this.hasIsTrustee()) {
			if(buf.get() == 1) {
				this.isTrustee = true;
			}else{
				this.isTrustee = false;
			}
		}

		if(this.hasIsListen()) {
			if(buf.get() == 1) {
				this.isListen = true;
			}else{
				this.isListen = false;
			}
		}

		if(this.hasHuScore()) {
			this.huScore = buf.getInt();
		}

		if(this.hasKeepPokerGroup()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.keepPokerGroup.Add(OP_POKER_GROUP.decode(bytes));
			}
		}

		if(this.hasIsLockListen()) {
			if(buf.get() == 1) {
				this.isLockListen = true;
			}else{
				this.isLockListen = false;
			}
		}

		if(this.hasWind()) {
			this.wind = buf.getInt();
		}

		if(this.hasLongitude()) {
			this.longitude = buf.getFloat();
		}

		if(this.hasLatitude()) {
			this.latitude = buf.getFloat();
		}

		if(this.hasOnline()) {
			this.online = buf.getLong();
		}

		if(this.hasIsAfk()) {
			if(buf.get() == 1) {
				this.isAfk = true;
			}else{
				this.isAfk = false;
			}
		}

		if(this.hasDingPokerGroup()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.dingPokerGroup.Add(OP_POKER_GROUP.decode(bytes));
			}
		}

		if(this.hasIsDouble()) {
			if(buf.get() == 1) {
				this.isDouble = true;
			}else{
				this.isDouble = false;
			}
		}

		if(this.hasDismiss()) {
			this.dismiss = (ENUM_DISMISS_STATUS) buf.get();
		}

		if(this.hasKou()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.kou.Add(buf.getInt());
			}
		}

		if(this.hasSwap()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.swap.Add(OP_POKER.decode(bytes));
			}
		}

		if(this.hasLackColor()) {
			this.lackColor = buf.getInt();
		}

	} 

	public bool hasIsDismiss() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasIpaddr() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasGender() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasAvatar() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasNickname() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasCountdown() {
		return (this.__flag[0] & 32) != 0;
	}

	public int universalCount() {
		return this.universal.Count;
	}

	public bool hasUniversal() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasIsBanker() {
		return (this.__flag[0] & 128) != 0;
	}

	public int scoresCount() {
		return this.scores.Count;
	}

	public bool hasScores() {
		return (this.__flag[1] & 1) != 0;
	}

	public int incomesCount() {
		return this.incomes.Count;
	}

	public bool hasIncomes() {
		return (this.__flag[1] & 2) != 0;
	}

	public int usePokerGroupCount() {
		return this.usePokerGroup.Count;
	}

	public bool hasUsePokerGroup() {
		return (this.__flag[1] & 4) != 0;
	}

	public bool hasProbMulti() {
		return (this.__flag[1] & 8) != 0;
	}

	public bool hasSettle() {
		return (this.__flag[1] & 16) != 0;
	}

	public bool hasIsWaiver() {
		return (this.__flag[1] & 32) != 0;
	}

	public bool hasIsWiner() {
		return (this.__flag[1] & 64) != 0;
	}

	public bool hasIsLoser() {
		return (this.__flag[1] & 128) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[2] & 1) != 0;
	}

	public bool hasPos() {
		return (this.__flag[2] & 2) != 0;
	}

	public bool hasGold() {
		return (this.__flag[2] & 4) != 0;
	}

	public bool hasStatus() {
		return (this.__flag[2] & 8) != 0;
	}

	public bool hasHandCount() {
		return (this.__flag[2] & 16) != 0;
	}

	public bool hasHitPoker() {
		return (this.__flag[2] & 32) != 0;
	}

	public bool hasPokerAmount() {
		return (this.__flag[2] & 64) != 0;
	}

	public int desktopCount() {
		return this.desktop.Count;
	}

	public bool hasDesktop() {
		return (this.__flag[2] & 128) != 0;
	}

	public int pokerCount() {
		return this.poker.Count;
	}

	public bool hasPoker() {
		return (this.__flag[3] & 1) != 0;
	}

	public bool hasIsTrustee() {
		return (this.__flag[3] & 2) != 0;
	}

	public bool hasIsListen() {
		return (this.__flag[3] & 4) != 0;
	}

	public bool hasHuScore() {
		return (this.__flag[3] & 8) != 0;
	}

	public int keepPokerGroupCount() {
		return this.keepPokerGroup.Count;
	}

	public bool hasKeepPokerGroup() {
		return (this.__flag[3] & 16) != 0;
	}

	public bool hasIsLockListen() {
		return (this.__flag[3] & 32) != 0;
	}

	public bool hasWind() {
		return (this.__flag[3] & 64) != 0;
	}

	public bool hasLongitude() {
		return (this.__flag[3] & 128) != 0;
	}

	public bool hasLatitude() {
		return (this.__flag[4] & 1) != 0;
	}

	public bool hasOnline() {
		return (this.__flag[4] & 2) != 0;
	}

	public bool hasIsAfk() {
		return (this.__flag[4] & 4) != 0;
	}

	public int dingPokerGroupCount() {
		return this.dingPokerGroup.Count;
	}

	public bool hasDingPokerGroup() {
		return (this.__flag[4] & 8) != 0;
	}

	public bool hasIsDouble() {
		return (this.__flag[4] & 16) != 0;
	}

	public bool hasDismiss() {
		return (this.__flag[4] & 32) != 0;
	}

	public int kouCount() {
		return this.kou.Count;
	}

	public bool hasKou() {
		return (this.__flag[4] & 64) != 0;
	}

	public int swapCount() {
		return this.swap.Count;
	}

	public bool hasSwap() {
		return (this.__flag[4] & 128) != 0;
	}

	public bool hasLackColor() {
		return (this.__flag[5] & 1) != 0;
	}

	public List<OP_POKER> getUniversalList() {
		return this.universal;
	}

	public List<OP_POKER_SETTLE> getScoresList() {
		return this.scores;
	}

	public List<OP_POKER_SETTLE> getIncomesList() {
		return this.incomes;
	}

	public List<OP_POKER_GROUP> getUsePokerGroupList() {
		return this.usePokerGroup;
	}

	public List<OP_POKER> getDesktopList() {
		return this.desktop;
	}

	public List<OP_POKER> getPokerList() {
		return this.poker;
	}

	public List<OP_POKER_GROUP> getKeepPokerGroupList() {
		return this.keepPokerGroup;
	}

	public List<OP_POKER_GROUP> getDingPokerGroupList() {
		return this.dingPokerGroup;
	}

	public List<int> getKouList() {
		return this.kou;
	}

	public List<OP_POKER> getSwapList() {
		return this.swap;
	}

}
}

