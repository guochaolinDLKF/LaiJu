//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 5:30:26 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.sss {

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

	private int _gold; 

	public int gold { 
		set { 
			if(!this.hasGold()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._gold = value;
		} 
		get { 
			return this._gold;
		} 
	} 

	private float _longitude; 

	public float longitude { 
		set { 
			if(!this.hasLongitude()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._latitude = value;
		} 
		get { 
			return this._latitude;
		} 
	} 

	private string _ipaddr; 

	public string ipaddr { 
		set { 
			if(!this.hasIpaddr()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._ipaddr = value;
		} 
		get { 
			return this._ipaddr;
		} 
	} 

	private List<FIRST_POKER_INFO> firstPokerInfo = new List<FIRST_POKER_INFO>(); 

	public FIRST_POKER_INFO getFirstPokerInfo(int index) { 
			return this.firstPokerInfo[index];
	} 
	
	public void addFirstPokerInfo(FIRST_POKER_INFO value) { 
			if(!this.hasFirstPokerInfo()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this.firstPokerInfo.Add(value);
	} 

	private List<SECOND_POKER_INFO> secondPokerInfo = new List<SECOND_POKER_INFO>(); 

	public SECOND_POKER_INFO getSecondPokerInfo(int index) { 
			return this.secondPokerInfo[index];
	} 
	
	public void addSecondPokerInfo(SECOND_POKER_INFO value) { 
			if(!this.hasSecondPokerInfo()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this.secondPokerInfo.Add(value);
	} 

	private List<THIRD_POKER_INFO> thirdPokerInfo = new List<THIRD_POKER_INFO>(); 

	public THIRD_POKER_INFO getThirdPokerInfo(int index) { 
			return this.thirdPokerInfo[index];
	} 
	
	public void addThirdPokerInfo(THIRD_POKER_INFO value) { 
			if(!this.hasThirdPokerInfo()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 2);
			}
			this.thirdPokerInfo.Add(value);
	} 

	private bool _isDismiss; 

	public bool isDismiss { 
		set { 
			if(!this.hasIsDismiss()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 4);
			}
			this._isDismiss = value;
		} 
		get { 
			return this._isDismiss;
		} 
	} 

	private byte _gender; 

	public byte gender { 
		set { 
			if(!this.hasGender()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 8);
			}
			this._gender = value;
		} 
		get { 
			return this._gender;
		} 
	} 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 16);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	private SEAT_STATUS _seatStatus; 

	public SEAT_STATUS seatStatus { 
		set { 
			if(!this.hasSeatStatus()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 32);
			}
			this._seatStatus = value;
		} 
		get { 
			return this._seatStatus;
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

		ByteBuffer[] bytes = new ByteBuffer[14]; 

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

		if(this.hasGold()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.gold);
			total += bytes[3].limit();
		}

		if(this.hasLongitude()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putFloat(this.longitude);
			total += bytes[4].limit();
		}

		if(this.hasLatitude()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putFloat(this.latitude);
			total += bytes[5].limit();
		}

		if(this.hasIpaddr()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.ipaddr);
			    short len = (short) _byte.Length;
			    bytes[6] = ByteBuffer.allocate(2 + len);
			    bytes[6].putShort(len);
				bytes[6].put(_byte);
			total += bytes[6].limit();
		}

		if(this.hasFirstPokerInfo()) {
				int length = 0;
				for(int i=0, len=this.firstPokerInfo.Count; i<len; i++) {
					length += this.firstPokerInfo[i].encode().Length;
				}
				bytes[7] = ByteBuffer.allocate(this.firstPokerInfo.Count * 4 + length + 2);
				bytes[7].putShort((short) this.firstPokerInfo.Count);
				for(int i=0, len=this.firstPokerInfo.Count; i<len; i++) {
					byte[] _byte = this.firstPokerInfo[i].encode();
					bytes[7].putInt(_byte.Length);
					bytes[7].put(_byte);
				}
			total += bytes[7].limit();
		}

		if(this.hasSecondPokerInfo()) {
				int length = 0;
				for(int i=0, len=this.secondPokerInfo.Count; i<len; i++) {
					length += this.secondPokerInfo[i].encode().Length;
				}
				bytes[8] = ByteBuffer.allocate(this.secondPokerInfo.Count * 4 + length + 2);
				bytes[8].putShort((short) this.secondPokerInfo.Count);
				for(int i=0, len=this.secondPokerInfo.Count; i<len; i++) {
					byte[] _byte = this.secondPokerInfo[i].encode();
					bytes[8].putInt(_byte.Length);
					bytes[8].put(_byte);
				}
			total += bytes[8].limit();
		}

		if(this.hasThirdPokerInfo()) {
				int length = 0;
				for(int i=0, len=this.thirdPokerInfo.Count; i<len; i++) {
					length += this.thirdPokerInfo[i].encode().Length;
				}
				bytes[9] = ByteBuffer.allocate(this.thirdPokerInfo.Count * 4 + length + 2);
				bytes[9].putShort((short) this.thirdPokerInfo.Count);
				for(int i=0, len=this.thirdPokerInfo.Count; i<len; i++) {
					byte[] _byte = this.thirdPokerInfo[i].encode();
					bytes[9].putInt(_byte.Length);
					bytes[9].put(_byte);
				}
			total += bytes[9].limit();
		}

		if(this.hasIsDismiss()) {
			bytes[10] = ByteBuffer.allocate(1);
			if(this.isDismiss) {
				bytes[10].put((byte) 1);
			}else{
				bytes[10].put((byte) 0);
			}
			total += bytes[10].limit();
		}

		if(this.hasGender()) {
			bytes[11] = ByteBuffer.allocate(1);
			bytes[11].put(this.gender);
			total += bytes[11].limit();
		}

		if(this.hasPos()) {
			bytes[12] = ByteBuffer.allocate(4);
			bytes[12].putInt(this.pos);
			total += bytes[12].limit();
		}

		if(this.hasSeatStatus()) {
			bytes[13] = ByteBuffer.allocate(1);
			bytes[13].put((byte) this.seatStatus);
			total += bytes[13].limit();
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

		if(this.hasGold()) {
			this.gold = buf.getInt();
		}

		if(this.hasLongitude()) {
			this.longitude = buf.getFloat();
		}

		if(this.hasLatitude()) {
			this.latitude = buf.getFloat();
		}

		if(this.hasIpaddr()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.ipaddr = System.Text.Encoding.UTF8.GetString(bytes);
		}

		if(this.hasFirstPokerInfo()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.firstPokerInfo.Add(FIRST_POKER_INFO.decode(bytes));
			}
		}

		if(this.hasSecondPokerInfo()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.secondPokerInfo.Add(SECOND_POKER_INFO.decode(bytes));
			}
		}

		if(this.hasThirdPokerInfo()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.thirdPokerInfo.Add(THIRD_POKER_INFO.decode(bytes));
			}
		}

		if(this.hasIsDismiss()) {
			if(buf.get() == 1) {
				this.isDismiss = true;
			}else{
				this.isDismiss = false;
			}
		}

		if(this.hasGender()) {
			this.gender = buf.get();
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasSeatStatus()) {
			this.seatStatus = (SEAT_STATUS) buf.get();
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

	public bool hasGold() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasLongitude() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasLatitude() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasIpaddr() {
		return (this.__flag[0] & 64) != 0;
	}

	public int firstPokerInfoCount() {
		return this.firstPokerInfo.Count;
	}

	public bool hasFirstPokerInfo() {
		return (this.__flag[0] & 128) != 0;
	}

	public int secondPokerInfoCount() {
		return this.secondPokerInfo.Count;
	}

	public bool hasSecondPokerInfo() {
		return (this.__flag[1] & 1) != 0;
	}

	public int thirdPokerInfoCount() {
		return this.thirdPokerInfo.Count;
	}

	public bool hasThirdPokerInfo() {
		return (this.__flag[1] & 2) != 0;
	}

	public bool hasIsDismiss() {
		return (this.__flag[1] & 4) != 0;
	}

	public bool hasGender() {
		return (this.__flag[1] & 8) != 0;
	}

	public bool hasPos() {
		return (this.__flag[1] & 16) != 0;
	}

	public bool hasSeatStatus() {
		return (this.__flag[1] & 32) != 0;
	}

	public List<FIRST_POKER_INFO> getFirstPokerInfoList() {
		return this.firstPokerInfo;
	}

	public List<SECOND_POKER_INFO> getSecondPokerInfoList() {
		return this.secondPokerInfo;
	}

	public List<THIRD_POKER_INFO> getThirdPokerInfoList() {
		return this.thirdPokerInfo;
	}

}
}

