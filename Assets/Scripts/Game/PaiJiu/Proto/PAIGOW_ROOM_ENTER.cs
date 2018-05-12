//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:24 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_ROOM_ENTER { 

	public const int CODE = 501002; 

	private byte[] __flag = new byte[2]; 

	private int _settingId; 

	public int settingId { 
		set { 
			if(!this.hasSettingId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._settingId = value;
		} 
		get { 
			return this._settingId;
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

	private string _nickname; 

	public string nickname { 
		set { 
			if(!this.hasNickname()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._isBanker = value;
		} 
		get { 
			return this._isBanker;
		} 
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

	private bool _isOwner; 

	public bool isOwner { 
		set { 
			if(!this.hasIsOwner()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this._isOwner = value;
		} 
		get { 
			return this._isOwner;
		} 
	} 

	public static PAIGOW_ROOM_ENTER newBuilder() { 
		return new PAIGOW_ROOM_ENTER(); 
	} 

	public static PAIGOW_ROOM_ENTER decode(byte[] data) { 
		PAIGOW_ROOM_ENTER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[9]; 

		int total = 0;
		if(this.hasSettingId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.settingId);
			total += bytes[0].limit();
		}

		if(this.hasPlayerId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.playerId);
			total += bytes[1].limit();
		}

		if(this.hasNickname()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.nickname);
			    short len = (short) _byte.Length;
			    bytes[2] = ByteBuffer.allocate(2 + len);
			    bytes[2].putShort(len);
				bytes[2].put(_byte);
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

		if(this.hasGender()) {
			bytes[4] = ByteBuffer.allocate(1);
			bytes[4].put(this.gender);
			total += bytes[4].limit();
		}

		if(this.hasGold()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putInt(this.gold);
			total += bytes[5].limit();
		}

		if(this.hasIsBanker()) {
			bytes[6] = ByteBuffer.allocate(1);
			if(this.isBanker) {
				bytes[6].put((byte) 1);
			}else{
				bytes[6].put((byte) 0);
			}
			total += bytes[6].limit();
		}

		if(this.hasPos()) {
			bytes[7] = ByteBuffer.allocate(4);
			bytes[7].putInt(this.pos);
			total += bytes[7].limit();
		}

		if(this.hasIsOwner()) {
			bytes[8] = ByteBuffer.allocate(1);
			if(this.isOwner) {
				bytes[8].put((byte) 1);
			}else{
				bytes[8].put((byte) 0);
			}
			total += bytes[8].limit();
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
		  
		if(this.hasSettingId()) {
			this.settingId = buf.getInt();
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

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasIsOwner()) {
			if(buf.get() == 1) {
				this.isOwner = true;
			}else{
				this.isOwner = false;
			}
		}

	} 

	public bool hasSettingId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasNickname() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasAvatar() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasGender() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasGold() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasIsBanker() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 128) != 0;
	}

	public bool hasIsOwner() {
		return (this.__flag[1] & 1) != 0;
	}

}
}

