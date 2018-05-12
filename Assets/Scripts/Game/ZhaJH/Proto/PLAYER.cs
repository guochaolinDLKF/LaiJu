//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:34 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class PLAYER { 

	public const int CODE = 4004; 

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

	private List<POKER> zjh_poker = new List<POKER>(); 

	public POKER getZjhPoker(int index) { 
			return this.zjh_poker[index];
	} 
	
	public void addZjhPoker(POKER value) { 
			if(!this.hasZjhPoker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this.zjh_poker.Add(value);
	} 

	private float _gold; 

	public float gold { 
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

	public static PLAYER newBuilder() { 
		return new PLAYER(); 
	} 

	public static PLAYER decode(byte[] data) { 
		PLAYER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[6]; 

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

		if(this.hasZjhPoker()) {
				int length = 0;
				for(int i=0, len=this.zjh_poker.Count; i<len; i++) {
					length += this.zjh_poker[i].encode().Length;
				}
				bytes[4] = ByteBuffer.allocate(this.zjh_poker.Count * 4 + length + 2);
				bytes[4].putShort((short) this.zjh_poker.Count);
				for(int i=0, len=this.zjh_poker.Count; i<len; i++) {
					byte[] _byte = this.zjh_poker[i].encode();
					bytes[4].putInt(_byte.Length);
					bytes[4].put(_byte);
				}
			total += bytes[4].limit();
		}

		if(this.hasGold()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putFloat(this.gold);
			total += bytes[5].limit();
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

		if(this.hasGender()) {
			this.gender = buf.get();
		}

		if(this.hasZjhPoker()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.zjh_poker.Add(POKER.decode(bytes));
			}
		}

		if(this.hasGold()) {
			this.gold = buf.getFloat();
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

	public int zjhPokerCount() {
		return this.zjh_poker.Count;
	}

	public bool hasZjhPoker() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasGold() {
		return (this.__flag[0] & 32) != 0;
	}

	public List<POKER> getZjhPokerList() {
		return this.zjh_poker;
	}

}
}

