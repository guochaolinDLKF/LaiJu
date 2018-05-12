//===================================================
//Author      : DRB
//CreateTime  ：12/5/2017 12:02:06 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.pdk {

public class PDK_ENTER_ROOM { 

	public const int CODE = 901002; 

	private byte[] __flag = new byte[1]; 

	private List<int> settingId = new List<int>(); 

	public int getSettingId(int index) { 
			return this.settingId[index];
	} 
	
	public void addSettingId(int value) { 
			if(!this.hasSettingId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.settingId.Add(value);
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

	private float _longitude; 

	public float longitude { 
		set { 
			if(!this.hasLongitude()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._latitude = value;
		} 
		get { 
			return this._latitude;
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

	public static PDK_ENTER_ROOM newBuilder() { 
		return new PDK_ENTER_ROOM(); 
	} 

	public static PDK_ENTER_ROOM decode(byte[] data) { 
		PDK_ENTER_ROOM proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[8]; 

		int total = 0;
		if(this.hasSettingId()) {
			bytes[0] = ByteBuffer.allocate(this.settingId.Count * 4 + 2);
			bytes[0].putShort((short) this.settingId.Count);
			for(int i=0, len=this.settingId.Count; i<len; i++) {
				bytes[0].putInt(this.settingId[i]);
			}
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

		if(this.hasLongitude()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putFloat(this.longitude);
			total += bytes[5].limit();
		}

		if(this.hasLatitude()) {
			bytes[6] = ByteBuffer.allocate(4);
			bytes[6].putFloat(this.latitude);
			total += bytes[6].limit();
		}

		if(this.hasPos()) {
			bytes[7] = ByteBuffer.allocate(4);
			bytes[7].putInt(this.pos);
			total += bytes[7].limit();
		}

	
		ByteBuffer buf = ByteBuffer.allocate(1 + total);
	
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
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.settingId.Add(buf.getInt());
			}
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

		if(this.hasLongitude()) {
			this.longitude = buf.getFloat();
		}

		if(this.hasLatitude()) {
			this.latitude = buf.getFloat();
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

	} 

	public int settingIdCount() {
		return this.settingId.Count;
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

	public bool hasLongitude() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasLatitude() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 128) != 0;
	}

	public List<int> getSettingIdList() {
		return this.settingId;
	}

}
}

