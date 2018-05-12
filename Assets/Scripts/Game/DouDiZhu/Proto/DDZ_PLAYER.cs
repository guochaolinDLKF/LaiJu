using System.Collections.Generic;

namespace ddz.proto {

public class DDZ_PLAYER { 

	public const int CODE = 3003; 

	private byte[] __flag = new byte[16]; 

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

	private int _roomId; 

	public int roomId { 
		set { 
			if(!this.hasRoomId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._roomId = value;
		} 
		get { 
			return this._roomId;
		} 
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

	private List<DDZ_POCKER> pocker = new List<DDZ_POCKER>(); 

	public DDZ_POCKER getPocker(int index) { 
			return this.pocker[index];
	} 
	
	public void addPocker(DDZ_POCKER value) { 
			if(!this.hasPocker()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this.pocker.Add(value);
	} 

	public static DDZ_PLAYER newBuilder() { 
		return new DDZ_PLAYER(); 
	} 

	public static DDZ_PLAYER decode(byte[] data) { 
		DDZ_PLAYER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[9]; 

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

		if(this.hasCountdown()) {
			bytes[5] = ByteBuffer.allocate(8);
			bytes[5].putLong(this.countdown);
			total += bytes[5].limit();
		}

		if(this.hasRoomId()) {
			bytes[6] = ByteBuffer.allocate(4);
			bytes[6].putInt(this.roomId);
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

		if(this.hasPocker()) {
				int length = 0;
				for(int i=0, len=this.pocker.Count; i<len; i++) {
					length += this.pocker[i].encode().Length;
				}
				bytes[8] = ByteBuffer.allocate(this.pocker.Count * 4 + length + 2);
				bytes[8].putShort((short) this.pocker.Count);
				for(int i=0, len=this.pocker.Count; i<len; i++) {
					byte[] _byte = this.pocker[i].encode();
					bytes[8].putInt(_byte.Length);
					bytes[8].put(_byte);
				}
			total += bytes[8].limit();
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

		if(this.hasCountdown()) {
			this.countdown = buf.getLong();
		}

		if(this.hasRoomId()) {
			this.roomId = buf.getInt();
		}

		if(this.hasIsBanker()) {
			if(buf.get() == 1) {
				this.isBanker = true;
			}else{
				this.isBanker = false;
			}
		}

		if(this.hasPocker()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.pocker.Add(DDZ_POCKER.decode(bytes));
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

	public bool hasGold() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasCountdown() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasRoomId() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasIsBanker() {
		return (this.__flag[0] & 128) != 0;
	}

	public int pockerCount() {
		return this.pocker.Count;
	}

	public bool hasPocker() {
		return (this.__flag[1] & 1) != 0;
	}

	public List<DDZ_POCKER> getPockerList() {
		return this.pocker;
	}

}
}

