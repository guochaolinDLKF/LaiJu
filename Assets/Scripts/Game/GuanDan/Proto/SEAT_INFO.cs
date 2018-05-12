//===================================================
//Author      : DRB
//CreateTime  ：11/6/2017 2:21:46 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace guandan.proto {

public class SEAT_INFO { 

	public const int CODE = 818; 

	private byte[] __flag = new byte[2]; 

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

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	private float _longitude; 

	public float longitude { 
		set { 
			if(!this.hasLongitude()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._latitude = value;
		} 
		get { 
			return this._latitude;
		} 
	} 

	private int _playerId; 

	public int playerId { 
		set { 
			if(!this.hasPlayerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
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

	private int _gold; 

	public int gold { 
		set { 
			if(!this.hasGold()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this._gold = value;
		} 
		get { 
			return this._gold;
		} 
	} 

	private List<POCKER_INFO> pocker_info = new List<POCKER_INFO>(); 

	public POCKER_INFO getPockerInfo(int index) { 
			return this.pocker_info[index];
	} 
	
	public void addPockerInfo(POCKER_INFO value) { 
			if(!this.hasPockerInfo()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 2);
			}
			this.pocker_info.Add(value);
	} 

	private SEAT_STATUS _seat_status; 

	public SEAT_STATUS seat_status { 
		set { 
			if(!this.hasSeatStatus()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 4);
			}
			this._seat_status = value;
		} 
		get { 
			return this._seat_status;
		} 
	} 

	private int _HandPocker; 

	public int HandPocker { 
		set { 
			if(!this.hasHandPocker()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 8);
			}
			this._HandPocker = value;
		} 
		get { 
			return this._HandPocker;
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
		if(this.hasUnixtime()) {
			bytes[0] = ByteBuffer.allocate(8);
			bytes[0].putLong(this.unixtime);
			total += bytes[0].limit();
		}

		if(this.hasPos()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.pos);
			total += bytes[1].limit();
		}

		if(this.hasLongitude()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putFloat(this.longitude);
			total += bytes[2].limit();
		}

		if(this.hasLatitude()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putFloat(this.latitude);
			total += bytes[3].limit();
		}

		if(this.hasPlayerId()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.playerId);
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

		if(this.hasGold()) {
			bytes[8] = ByteBuffer.allocate(4);
			bytes[8].putInt(this.gold);
			total += bytes[8].limit();
		}

		if(this.hasPockerInfo()) {
				int length = 0;
				for(int i=0, len=this.pocker_info.Count; i<len; i++) {
					length += this.pocker_info[i].encode().Length;
				}
				bytes[9] = ByteBuffer.allocate(this.pocker_info.Count * 4 + length + 2);
				bytes[9].putShort((short) this.pocker_info.Count);
				for(int i=0, len=this.pocker_info.Count; i<len; i++) {
					byte[] _byte = this.pocker_info[i].encode();
					bytes[9].putInt(_byte.Length);
					bytes[9].put(_byte);
				}
			total += bytes[9].limit();
		}

		if(this.hasSeatStatus()) {
			bytes[10] = ByteBuffer.allocate(1);
			bytes[10].put((byte) this.seat_status);
			total += bytes[10].limit();
		}

		if(this.hasHandPocker()) {
			bytes[11] = ByteBuffer.allocate(4);
			bytes[11].putInt(this.HandPocker);
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
		  
		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasLongitude()) {
			this.longitude = buf.getFloat();
		}

		if(this.hasLatitude()) {
			this.latitude = buf.getFloat();
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

		if(this.hasPockerInfo()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.pocker_info.Add(POCKER_INFO.decode(bytes));
			}
		}

		if(this.hasSeatStatus()) {
			this.seat_status = (SEAT_STATUS) buf.get();
		}

		if(this.hasHandPocker()) {
			this.HandPocker = buf.getInt();
		}

	} 

	public bool hasUnixtime() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasLongitude() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasLatitude() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasPlayerId() {
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

	public bool hasGold() {
		return (this.__flag[1] & 1) != 0;
	}

	public int pockerInfoCount() {
		return this.pocker_info.Count;
	}

	public bool hasPockerInfo() {
		return (this.__flag[1] & 2) != 0;
	}

	public bool hasSeatStatus() {
		return (this.__flag[1] & 4) != 0;
	}

	public bool hasHandPocker() {
		return (this.__flag[1] & 8) != 0;
	}

	public List<POCKER_INFO> getPockerInfoList() {
		return this.pocker_info;
	}

}
}

