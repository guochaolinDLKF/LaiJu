//===================================================
//Author      : DRB
//CreateTime  ：11/6/2017 2:21:44 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace guandan.proto {

public class GD_ENTER_ROOM { 

	public const int CODE = 801002; 

	private byte[] __flag = new byte[1]; 

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

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
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

	private float _longitude; 

	public float longitude { 
		set { 
			if(!this.hasLongitude()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._longitude = value;
		} 
		get { 
			return this._longitude;
		} 
	} 

	public static GD_ENTER_ROOM newBuilder() { 
		return new GD_ENTER_ROOM(); 
	} 

	public static GD_ENTER_ROOM decode(byte[] data) { 
		GD_ENTER_ROOM proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[7]; 

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

		if(this.hasPos()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.pos);
			total += bytes[4].limit();
		}

		if(this.hasLatitude()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putFloat(this.latitude);
			total += bytes[5].limit();
		}

		if(this.hasLongitude()) {
			bytes[6] = ByteBuffer.allocate(4);
			bytes[6].putFloat(this.longitude);
			total += bytes[6].limit();
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

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasLatitude()) {
			this.latitude = buf.getFloat();
		}

		if(this.hasLongitude()) {
			this.longitude = buf.getFloat();
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

	public bool hasPos() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasLatitude() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasLongitude() {
		return (this.__flag[0] & 64) != 0;
	}

}
}

