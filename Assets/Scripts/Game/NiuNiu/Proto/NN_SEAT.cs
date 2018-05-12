//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:35 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class NN_SEAT { 

	public const int CODE = 2002; 

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

	private bool _isBanker; 

	public bool isBanker { 
		set { 
			if(!this.hasIsBanker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._isWiner = value;
		} 
		get { 
			return this._isWiner;
		} 
	} 

	private int _nn_earnings; 

	public int nn_earnings { 
		set { 
			if(!this.hasNnEarnings()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this._nn_earnings = value;
		} 
		get { 
			return this._nn_earnings;
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

	private List<NN_POKER> nn_poker = new List<NN_POKER>(); 

	public NN_POKER getNnPoker(int index) { 
			return this.nn_poker[index];
	} 
	
	public void addNnPoker(NN_POKER value) { 
			if(!this.hasNnPoker()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 4);
			}
			this.nn_poker.Add(value);
	} 

	private bool _IsHomeowners; 

	public bool IsHomeowners { 
		set { 
			if(!this.hasIsHomeowners()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 8);
			}
			this._IsHomeowners = value;
		} 
		get { 
			return this._IsHomeowners;
		} 
	} 

	private NN_ENUM_SEAT_DISSOLVE _dissolve; 

	public NN_ENUM_SEAT_DISSOLVE dissolve { 
		set { 
			if(!this.hasDissolve()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 16);
			}
			this._dissolve = value;
		} 
		get { 
			return this._dissolve;
		} 
	} 

	private int _nn_pokerType; 

	public int nn_pokerType { 
		set { 
			if(!this.hasNnPokerType()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 32);
			}
			this._nn_pokerType = value;
		} 
		get { 
			return this._nn_pokerType;
		} 
	} 

	private bool _ready; 

	public bool ready { 
		set { 
			if(!this.hasReady()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 64);
			}
			this._ready = value;
		} 
		get { 
			return this._ready;
		} 
	} 

	private int _rob_zhuang; 

	public int rob_zhuang { 
		set { 
			if(!this.hasRobZhuang()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 128);
			}
			this._rob_zhuang = value;
		} 
		get { 
			return this._rob_zhuang;
		} 
	} 

	private long _online; 

	public long online { 
		set { 
			if(!this.hasOnline()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 1);
			}
			this._online = value;
		} 
		get { 
			return this._online;
		} 
	} 

	private float _longitude; 

	public float longitude { 
		set { 
			if(!this.hasLongitude()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 2);
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
	    		this.__flag[2] = (byte) (this.__flag[2] | 4);
			}
			this._latitude = value;
		} 
		get { 
			return this._latitude;
		} 
	} 

	public static NN_SEAT newBuilder() { 
		return new NN_SEAT(); 
	} 

	public static NN_SEAT decode(byte[] data) { 
		NN_SEAT proto = newBuilder();
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

		if(this.hasGold()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.gold);
			total += bytes[4].limit();
		}

		if(this.hasIsBanker()) {
			bytes[5] = ByteBuffer.allocate(1);
			if(this.isBanker) {
				bytes[5].put((byte) 1);
			}else{
				bytes[5].put((byte) 0);
			}
			total += bytes[5].limit();
		}

		if(this.hasIsWiner()) {
			bytes[6] = ByteBuffer.allocate(1);
			if(this.isWiner) {
				bytes[6].put((byte) 1);
			}else{
				bytes[6].put((byte) 0);
			}
			total += bytes[6].limit();
		}

		if(this.hasNnEarnings()) {
			bytes[7] = ByteBuffer.allocate(4);
			bytes[7].putInt(this.nn_earnings);
			total += bytes[7].limit();
		}

		if(this.hasPour()) {
			bytes[8] = ByteBuffer.allocate(4);
			bytes[8].putInt(this.pour);
			total += bytes[8].limit();
		}

		if(this.hasPos()) {
			bytes[9] = ByteBuffer.allocate(4);
			bytes[9].putInt(this.pos);
			total += bytes[9].limit();
		}

		if(this.hasNnPoker()) {
				int length = 0;
				for(int i=0, len=this.nn_poker.Count; i<len; i++) {
					length += this.nn_poker[i].encode().Length;
				}
				bytes[10] = ByteBuffer.allocate(this.nn_poker.Count * 4 + length + 2);
				bytes[10].putShort((short) this.nn_poker.Count);
				for(int i=0, len=this.nn_poker.Count; i<len; i++) {
					byte[] _byte = this.nn_poker[i].encode();
					bytes[10].putInt(_byte.Length);
					bytes[10].put(_byte);
				}
			total += bytes[10].limit();
		}

		if(this.hasIsHomeowners()) {
			bytes[11] = ByteBuffer.allocate(1);
			if(this.IsHomeowners) {
				bytes[11].put((byte) 1);
			}else{
				bytes[11].put((byte) 0);
			}
			total += bytes[11].limit();
		}

		if(this.hasDissolve()) {
			bytes[12] = ByteBuffer.allocate(1);
			bytes[12].put((byte) this.dissolve);
			total += bytes[12].limit();
		}

		if(this.hasNnPokerType()) {
			bytes[13] = ByteBuffer.allocate(4);
			bytes[13].putInt(this.nn_pokerType);
			total += bytes[13].limit();
		}

		if(this.hasReady()) {
			bytes[14] = ByteBuffer.allocate(1);
			if(this.ready) {
				bytes[14].put((byte) 1);
			}else{
				bytes[14].put((byte) 0);
			}
			total += bytes[14].limit();
		}

		if(this.hasRobZhuang()) {
			bytes[15] = ByteBuffer.allocate(4);
			bytes[15].putInt(this.rob_zhuang);
			total += bytes[15].limit();
		}

		if(this.hasOnline()) {
			bytes[16] = ByteBuffer.allocate(8);
			bytes[16].putLong(this.online);
			total += bytes[16].limit();
		}

		if(this.hasLongitude()) {
			bytes[17] = ByteBuffer.allocate(4);
			bytes[17].putFloat(this.longitude);
			total += bytes[17].limit();
		}

		if(this.hasLatitude()) {
			bytes[18] = ByteBuffer.allocate(4);
			bytes[18].putFloat(this.latitude);
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

		if(this.hasGold()) {
			this.gold = buf.getInt();
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

		if(this.hasNnEarnings()) {
			this.nn_earnings = buf.getInt();
		}

		if(this.hasPour()) {
			this.pour = buf.getInt();
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasNnPoker()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.nn_poker.Add(NN_POKER.decode(bytes));
			}
		}

		if(this.hasIsHomeowners()) {
			if(buf.get() == 1) {
				this.IsHomeowners = true;
			}else{
				this.IsHomeowners = false;
			}
		}

		if(this.hasDissolve()) {
			this.dissolve = (NN_ENUM_SEAT_DISSOLVE) buf.get();
		}

		if(this.hasNnPokerType()) {
			this.nn_pokerType = buf.getInt();
		}

		if(this.hasReady()) {
			if(buf.get() == 1) {
				this.ready = true;
			}else{
				this.ready = false;
			}
		}

		if(this.hasRobZhuang()) {
			this.rob_zhuang = buf.getInt();
		}

		if(this.hasOnline()) {
			this.online = buf.getLong();
		}

		if(this.hasLongitude()) {
			this.longitude = buf.getFloat();
		}

		if(this.hasLatitude()) {
			this.latitude = buf.getFloat();
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

	public bool hasIsBanker() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasIsWiner() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasNnEarnings() {
		return (this.__flag[0] & 128) != 0;
	}

	public bool hasPour() {
		return (this.__flag[1] & 1) != 0;
	}

	public bool hasPos() {
		return (this.__flag[1] & 2) != 0;
	}

	public int nnPokerCount() {
		return this.nn_poker.Count;
	}

	public bool hasNnPoker() {
		return (this.__flag[1] & 4) != 0;
	}

	public bool hasIsHomeowners() {
		return (this.__flag[1] & 8) != 0;
	}

	public bool hasDissolve() {
		return (this.__flag[1] & 16) != 0;
	}

	public bool hasNnPokerType() {
		return (this.__flag[1] & 32) != 0;
	}

	public bool hasReady() {
		return (this.__flag[1] & 64) != 0;
	}

	public bool hasRobZhuang() {
		return (this.__flag[1] & 128) != 0;
	}

	public bool hasOnline() {
		return (this.__flag[2] & 1) != 0;
	}

	public bool hasLongitude() {
		return (this.__flag[2] & 2) != 0;
	}

	public bool hasLatitude() {
		return (this.__flag[2] & 4) != 0;
	}

	public List<NN_POKER> getNnPokerList() {
		return this.nn_poker;
	}

}
}

