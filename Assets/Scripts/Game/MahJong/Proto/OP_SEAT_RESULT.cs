using System.Collections.Generic;

namespace proto.mahjong {

public class OP_SEAT_RESULT { 

	public const int CODE = 1005; 

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

	private int _paoCount; 

	public int paoCount { 
		set { 
			if(!this.hasPaoCount()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._paoCount = value;
		} 
		get { 
			return this._paoCount;
		} 
	} 

	private int _zimoCount; 

	public int zimoCount { 
		set { 
			if(!this.hasZimoCount()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._zimoCount = value;
		} 
		get { 
			return this._zimoCount;
		} 
	} 

	private int _probCount; 

	public int probCount { 
		set { 
			if(!this.hasProbCount()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._probCount = value;
		} 
		get { 
			return this._probCount;
		} 
	} 

	private int _agangCount; 

	public int agangCount { 
		set { 
			if(!this.hasAgangCount()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._agangCount = value;
		} 
		get { 
			return this._agangCount;
		} 
	} 

	private int _bgangCount; 

	public int bgangCount { 
		set { 
			if(!this.hasBgangCount()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._bgangCount = value;
		} 
		get { 
			return this._bgangCount;
		} 
	} 

	private int _probMulti; 

	public int probMulti { 
		set { 
			if(!this.hasProbMulti()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._probMulti = value;
		} 
		get { 
			return this._probMulti;
		} 
	} 

	private int _playerId; 

	public int playerId { 
		set { 
			if(!this.hasPlayerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
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

	private int _gold; 

	public int gold { 
		set { 
			if(!this.hasGold()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 8);
			}
			this._gold = value;
		} 
		get { 
			return this._gold;
		} 
	} 

	private int _mgangCount; 

	public int mgangCount { 
		set { 
			if(!this.hasMgangCount()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 16);
			}
			this._mgangCount = value;
		} 
		get { 
			return this._mgangCount;
		} 
	} 

	private bool _isOwner; 

	public bool isOwner { 
		set { 
			if(!this.hasIsOwner()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 32);
			}
			this._isOwner = value;
		} 
		get { 
			return this._isOwner;
		} 
	} 

	private int _baoCount; 

	public int baoCount { 
		set { 
			if(!this.hasBaoCount()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 64);
			}
			this._baoCount = value;
		} 
		get { 
			return this._baoCount;
		} 
	} 

	private int _inBaoCount; 

	public int inBaoCount { 
		set { 
			if(!this.hasInBaoCount()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 128);
			}
			this._inBaoCount = value;
		} 
		get { 
			return this._inBaoCount;
		} 
	} 

	private int _bankerCount; 

	public int bankerCount { 
		set { 
			if(!this.hasBankerCount()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 1);
			}
			this._bankerCount = value;
		} 
		get { 
			return this._bankerCount;
		} 
	} 

	private int _dianpaoCount; 

	public int dianpaoCount { 
		set { 
			if(!this.hasDianpaoCount()) {
	    		this.__flag[2] = (byte) (this.__flag[2] | 2);
			}
			this._dianpaoCount = value;
		} 
		get { 
			return this._dianpaoCount;
		} 
	} 

	public static OP_SEAT_RESULT newBuilder() { 
		return new OP_SEAT_RESULT(); 
	} 

	public static OP_SEAT_RESULT decode(byte[] data) { 
		OP_SEAT_RESULT proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[18]; 

		int total = 0;
		if(this.hasPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pos);
			total += bytes[0].limit();
		}

		if(this.hasPaoCount()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.paoCount);
			total += bytes[1].limit();
		}

		if(this.hasZimoCount()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.zimoCount);
			total += bytes[2].limit();
		}

		if(this.hasProbCount()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.probCount);
			total += bytes[3].limit();
		}

		if(this.hasAgangCount()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.agangCount);
			total += bytes[4].limit();
		}

		if(this.hasBgangCount()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putInt(this.bgangCount);
			total += bytes[5].limit();
		}

		if(this.hasProbMulti()) {
			bytes[6] = ByteBuffer.allocate(4);
			bytes[6].putInt(this.probMulti);
			total += bytes[6].limit();
		}

		if(this.hasPlayerId()) {
			bytes[7] = ByteBuffer.allocate(4);
			bytes[7].putInt(this.playerId);
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

		if(this.hasGold()) {
			bytes[11] = ByteBuffer.allocate(4);
			bytes[11].putInt(this.gold);
			total += bytes[11].limit();
		}

		if(this.hasMgangCount()) {
			bytes[12] = ByteBuffer.allocate(4);
			bytes[12].putInt(this.mgangCount);
			total += bytes[12].limit();
		}

		if(this.hasIsOwner()) {
			bytes[13] = ByteBuffer.allocate(1);
			if(this.isOwner) {
				bytes[13].put((byte) 1);
			}else{
				bytes[13].put((byte) 0);
			}
			total += bytes[13].limit();
		}

		if(this.hasBaoCount()) {
			bytes[14] = ByteBuffer.allocate(4);
			bytes[14].putInt(this.baoCount);
			total += bytes[14].limit();
		}

		if(this.hasInBaoCount()) {
			bytes[15] = ByteBuffer.allocate(4);
			bytes[15].putInt(this.inBaoCount);
			total += bytes[15].limit();
		}

		if(this.hasBankerCount()) {
			bytes[16] = ByteBuffer.allocate(4);
			bytes[16].putInt(this.bankerCount);
			total += bytes[16].limit();
		}

		if(this.hasDianpaoCount()) {
			bytes[17] = ByteBuffer.allocate(4);
			bytes[17].putInt(this.dianpaoCount);
			total += bytes[17].limit();
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

		if(this.hasPaoCount()) {
			this.paoCount = buf.getInt();
		}

		if(this.hasZimoCount()) {
			this.zimoCount = buf.getInt();
		}

		if(this.hasProbCount()) {
			this.probCount = buf.getInt();
		}

		if(this.hasAgangCount()) {
			this.agangCount = buf.getInt();
		}

		if(this.hasBgangCount()) {
			this.bgangCount = buf.getInt();
		}

		if(this.hasProbMulti()) {
			this.probMulti = buf.getInt();
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

		if(this.hasMgangCount()) {
			this.mgangCount = buf.getInt();
		}

		if(this.hasIsOwner()) {
			if(buf.get() == 1) {
				this.isOwner = true;
			}else{
				this.isOwner = false;
			}
		}

		if(this.hasBaoCount()) {
			this.baoCount = buf.getInt();
		}

		if(this.hasInBaoCount()) {
			this.inBaoCount = buf.getInt();
		}

		if(this.hasBankerCount()) {
			this.bankerCount = buf.getInt();
		}

		if(this.hasDianpaoCount()) {
			this.dianpaoCount = buf.getInt();
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPaoCount() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasZimoCount() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasProbCount() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasAgangCount() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasBgangCount() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasProbMulti() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasPlayerId() {
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

	public bool hasGold() {
		return (this.__flag[1] & 8) != 0;
	}

	public bool hasMgangCount() {
		return (this.__flag[1] & 16) != 0;
	}

	public bool hasIsOwner() {
		return (this.__flag[1] & 32) != 0;
	}

	public bool hasBaoCount() {
		return (this.__flag[1] & 64) != 0;
	}

	public bool hasInBaoCount() {
		return (this.__flag[1] & 128) != 0;
	}

	public bool hasBankerCount() {
		return (this.__flag[2] & 1) != 0;
	}

	public bool hasDianpaoCount() {
		return (this.__flag[2] & 2) != 0;
	}

}
}

