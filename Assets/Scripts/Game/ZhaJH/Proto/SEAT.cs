//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:59 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class SEAT { 

	public const int CODE = 4001; 

	private byte[] __flag = new byte[4]; 

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

	private int _passportId; 

	public int passportId { 
		set { 
			if(!this.hasPassportId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._passportId = value;
		} 
		get { 
			return this._passportId;
		} 
	} 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	private List<POKER> zjh_poker = new List<POKER>(); 

	public POKER getZjhPoker(int index) { 
			return this.zjh_poker[index];
	} 
	
	public void addZjhPoker(POKER value) { 
			if(!this.hasZjhPoker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this.zjh_poker.Add(value);
	} 

	private ENUM_ROOMRESULT _zjh_enum_roomresult; 

	public ENUM_ROOMRESULT zjh_enum_roomresult { 
		set { 
			if(!this.hasZjhEnumRoomresult()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this._zjh_enum_roomresult = value;
		} 
		get { 
			return this._zjh_enum_roomresult;
		} 
	} 

	private ENUM_SEAT_STATUS _status; 

	public ENUM_SEAT_STATUS status { 
		set { 
			if(!this.hasStatus()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this._status = value;
		} 
		get { 
			return this._status;
		} 
	} 

	private bool _homeLorder; 

	public bool homeLorder { 
		set { 
			if(!this.hasHomeLorder()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 2);
			}
			this._homeLorder = value;
		} 
		get { 
			return this._homeLorder;
		} 
	} 

	private bool _isWiner; 

	public bool isWiner { 
		set { 
			if(!this.hasIsWiner()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 4);
			}
			this._isWiner = value;
		} 
		get { 
			return this._isWiner;
		} 
	} 

	private ENUM_POKER_STATUS _pokerstatus; 

	public ENUM_POKER_STATUS pokerstatus { 
		set { 
			if(!this.hasPokerstatus()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 8);
			}
			this._pokerstatus = value;
		} 
		get { 
			return this._pokerstatus;
		} 
	} 

	private ENUM_SEATOPERATE_STATUS _opreateStatus; 

	public ENUM_SEATOPERATE_STATUS opreateStatus { 
		set { 
			if(!this.hasOpreateStatus()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 16);
			}
			this._opreateStatus = value;
		} 
		get { 
			return this._opreateStatus;
		} 
	} 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 32);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	private float _gold; 

	public float gold { 
		set { 
			if(!this.hasGold()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 64);
			}
			this._gold = value;
		} 
		get { 
			return this._gold;
		} 
	} 

	private float _Profit; 

	public float Profit { 
		set { 
			if(!this.hasProfit()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 128);
			}
			this._Profit = value;
		} 
		get { 
			return this._Profit;
		} 
	} 

	private float _totalPour; 

	public float totalPour { 
		set { 
			if(!this.hasTotalPour()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 1);
			}
			this._totalPour = value;
		} 
		get { 
			return this._totalPour;
		} 
	} 

	private float _pour; 

	public float pour { 
		set { 
			if(!this.hasPour()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 2);
			}
			this._pour = value;
		} 
		get { 
			return this._pour;
		} 
	} 

	private int _maxLoop; 

	public int maxLoop { 
		set { 
			if(!this.hasMaxLoop()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 4);
			}
			this._maxLoop = value;
		} 
		get { 
			return this._maxLoop;
		} 
	} 

	private bool _ISLOWSCORE; 

	public bool ISLOWSCORE { 
		set { 
			if(!this.hasISLOWSCORE()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 8);
			}
			this._ISLOWSCORE = value;
		} 
		get { 
			return this._ISLOWSCORE;
		} 
	} 

	private int _type; 

	public int type { 
		set { 
			if(!this.hasType()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 16);
			}
			this._type = value;
		} 
		get { 
			return this._type;
		} 
	} 

	private string _ipaddr; 

	public string ipaddr { 
		set { 
			if(!this.hasIpaddr()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 32);
			}
			this._ipaddr = value;
		} 
		get { 
			return this._ipaddr;
		} 
	} 

	private float _longitude; 

	public float longitude { 
		set { 
			if(!this.hasLongitude()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 64);
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
	    		this.__flag[2] = (byte) (this.__flag[2] | 128);
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
	    		this.__flag[3] = (byte) (this.__flag[3] | 1);
			}
			this._isAfk = value;
		} 
		get { 
			return this._isAfk;
		} 
	} 

	public static SEAT newBuilder() { 
		return new SEAT(); 
	} 

	public static SEAT decode(byte[] data) { 
		SEAT proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[25]; 

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

		if(this.hasPassportId()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.passportId);
			total += bytes[4].limit();
		}

		if(this.hasPos()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putInt(this.pos);
			total += bytes[5].limit();
		}

		if(this.hasZjhPoker()) {
				int length = 0;
				for(int i=0, len=this.zjh_poker.Count; i<len; i++) {
					length += this.zjh_poker[i].encode().Length;
				}
				bytes[6] = ByteBuffer.allocate(this.zjh_poker.Count * 4 + length + 2);
				bytes[6].putShort((short) this.zjh_poker.Count);
				for(int i=0, len=this.zjh_poker.Count; i<len; i++) {
					byte[] _byte = this.zjh_poker[i].encode();
					bytes[6].putInt(_byte.Length);
					bytes[6].put(_byte);
				}
			total += bytes[6].limit();
		}

		if(this.hasZjhEnumRoomresult()) {
			bytes[7] = ByteBuffer.allocate(1);
			bytes[7].put((byte) this.zjh_enum_roomresult);
			total += bytes[7].limit();
		}

		if(this.hasStatus()) {
			bytes[8] = ByteBuffer.allocate(1);
			bytes[8].put((byte) this.status);
			total += bytes[8].limit();
		}

		if(this.hasHomeLorder()) {
			bytes[9] = ByteBuffer.allocate(1);
			if(this.homeLorder) {
				bytes[9].put((byte) 1);
			}else{
				bytes[9].put((byte) 0);
			}
			total += bytes[9].limit();
		}

		if(this.hasIsWiner()) {
			bytes[10] = ByteBuffer.allocate(1);
			if(this.isWiner) {
				bytes[10].put((byte) 1);
			}else{
				bytes[10].put((byte) 0);
			}
			total += bytes[10].limit();
		}

		if(this.hasPokerstatus()) {
			bytes[11] = ByteBuffer.allocate(1);
			bytes[11].put((byte) this.pokerstatus);
			total += bytes[11].limit();
		}

		if(this.hasOpreateStatus()) {
			bytes[12] = ByteBuffer.allocate(1);
			bytes[12].put((byte) this.opreateStatus);
			total += bytes[12].limit();
		}

		if(this.hasUnixtime()) {
			bytes[13] = ByteBuffer.allocate(8);
			bytes[13].putLong(this.unixtime);
			total += bytes[13].limit();
		}

		if(this.hasGold()) {
			bytes[14] = ByteBuffer.allocate(4);
			bytes[14].putFloat(this.gold);
			total += bytes[14].limit();
		}

		if(this.hasProfit()) {
			bytes[15] = ByteBuffer.allocate(4);
			bytes[15].putFloat(this.Profit);
			total += bytes[15].limit();
		}

		if(this.hasTotalPour()) {
			bytes[16] = ByteBuffer.allocate(4);
			bytes[16].putFloat(this.totalPour);
			total += bytes[16].limit();
		}

		if(this.hasPour()) {
			bytes[17] = ByteBuffer.allocate(4);
			bytes[17].putFloat(this.pour);
			total += bytes[17].limit();
		}

		if(this.hasMaxLoop()) {
			bytes[18] = ByteBuffer.allocate(4);
			bytes[18].putInt(this.maxLoop);
			total += bytes[18].limit();
		}

		if(this.hasISLOWSCORE()) {
			bytes[19] = ByteBuffer.allocate(1);
			if(this.ISLOWSCORE) {
				bytes[19].put((byte) 1);
			}else{
				bytes[19].put((byte) 0);
			}
			total += bytes[19].limit();
		}

		if(this.hasType()) {
			bytes[20] = ByteBuffer.allocate(4);
			bytes[20].putInt(this.type);
			total += bytes[20].limit();
		}

		if(this.hasIpaddr()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.ipaddr);
			    short len = (short) _byte.Length;
			    bytes[21] = ByteBuffer.allocate(2 + len);
			    bytes[21].putShort(len);
				bytes[21].put(_byte);
			total += bytes[21].limit();
		}

		if(this.hasLongitude()) {
			bytes[22] = ByteBuffer.allocate(4);
			bytes[22].putFloat(this.longitude);
			total += bytes[22].limit();
		}

		if(this.hasLatitude()) {
			bytes[23] = ByteBuffer.allocate(4);
			bytes[23].putFloat(this.latitude);
			total += bytes[23].limit();
		}

		if(this.hasIsAfk()) {
			bytes[24] = ByteBuffer.allocate(1);
			if(this.isAfk) {
				bytes[24].put((byte) 1);
			}else{
				bytes[24].put((byte) 0);
			}
			total += bytes[24].limit();
		}

	
		ByteBuffer buf = ByteBuffer.allocate(4 + total);
	
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

		if(this.hasPassportId()) {
			this.passportId = buf.getInt();
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasZjhPoker()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.zjh_poker.Add(POKER.decode(bytes));
			}
		}

		if(this.hasZjhEnumRoomresult()) {
			this.zjh_enum_roomresult = (ENUM_ROOMRESULT) buf.get();
		}

		if(this.hasStatus()) {
			this.status = (ENUM_SEAT_STATUS) buf.get();
		}

		if(this.hasHomeLorder()) {
			if(buf.get() == 1) {
				this.homeLorder = true;
			}else{
				this.homeLorder = false;
			}
		}

		if(this.hasIsWiner()) {
			if(buf.get() == 1) {
				this.isWiner = true;
			}else{
				this.isWiner = false;
			}
		}

		if(this.hasPokerstatus()) {
			this.pokerstatus = (ENUM_POKER_STATUS) buf.get();
		}

		if(this.hasOpreateStatus()) {
			this.opreateStatus = (ENUM_SEATOPERATE_STATUS) buf.get();
		}

		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
		}

		if(this.hasGold()) {
			this.gold = buf.getFloat();
		}

		if(this.hasProfit()) {
			this.Profit = buf.getFloat();
		}

		if(this.hasTotalPour()) {
			this.totalPour = buf.getFloat();
		}

		if(this.hasPour()) {
			this.pour = buf.getFloat();
		}

		if(this.hasMaxLoop()) {
			this.maxLoop = buf.getInt();
		}

		if(this.hasISLOWSCORE()) {
			if(buf.get() == 1) {
				this.ISLOWSCORE = true;
			}else{
				this.ISLOWSCORE = false;
			}
		}

		if(this.hasType()) {
			this.type = buf.getInt();
		}

		if(this.hasIpaddr()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.ipaddr = System.Text.Encoding.UTF8.GetString(bytes);
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

	public bool hasPassportId() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 32) != 0;
	}

	public int zjhPokerCount() {
		return this.zjh_poker.Count;
	}

	public bool hasZjhPoker() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasZjhEnumRoomresult() {
		return (this.__flag[0] & 128) != 0;
	}

	public bool hasStatus() {
		return (this.__flag[1] & 1) != 0;
	}

	public bool hasHomeLorder() {
		return (this.__flag[1] & 2) != 0;
	}

	public bool hasIsWiner() {
		return (this.__flag[1] & 4) != 0;
	}

	public bool hasPokerstatus() {
		return (this.__flag[1] & 8) != 0;
	}

	public bool hasOpreateStatus() {
		return (this.__flag[1] & 16) != 0;
	}

	public bool hasUnixtime() {
		return (this.__flag[1] & 32) != 0;
	}

	public bool hasGold() {
		return (this.__flag[1] & 64) != 0;
	}

	public bool hasProfit() {
		return (this.__flag[1] & 128) != 0;
	}

	public bool hasTotalPour() {
		return (this.__flag[2] & 1) != 0;
	}

	public bool hasPour() {
		return (this.__flag[2] & 2) != 0;
	}

	public bool hasMaxLoop() {
		return (this.__flag[2] & 4) != 0;
	}

	public bool hasISLOWSCORE() {
		return (this.__flag[2] & 8) != 0;
	}

	public bool hasType() {
		return (this.__flag[2] & 16) != 0;
	}

	public bool hasIpaddr() {
		return (this.__flag[2] & 32) != 0;
	}

	public bool hasLongitude() {
		return (this.__flag[2] & 64) != 0;
	}

	public bool hasLatitude() {
		return (this.__flag[2] & 128) != 0;
	}

	public bool hasIsAfk() {
		return (this.__flag[3] & 1) != 0;
	}

	public List<POKER> getZjhPokerList() {
		return this.zjh_poker;
	}

}
}

