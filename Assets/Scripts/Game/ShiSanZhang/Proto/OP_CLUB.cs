//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 5:30:33 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.sss {

public class OP_CLUB { 

	public const int CODE = 99313; 

	private byte[] __flag = new byte[2]; 

	private int _clubId; 

	public int clubId { 
		set { 
			if(!this.hasClubId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._clubId = value;
		} 
		get { 
			return this._clubId;
		} 
	} 

	private string _name; 

	public string name { 
		set { 
			if(!this.hasName()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._name = value;
		} 
		get { 
			return this._name;
		} 
	} 

	private string _announce; 

	public string announce { 
		set { 
			if(!this.hasAnnounce()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._announce = value;
		} 
		get { 
			return this._announce;
		} 
	} 

	private int _ownerId; 

	public int ownerId { 
		set { 
			if(!this.hasOwnerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._ownerId = value;
		} 
		get { 
			return this._ownerId;
		} 
	} 

	private int _playerCount; 

	public int playerCount { 
		set { 
			if(!this.hasPlayerCount()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._playerCount = value;
		} 
		get { 
			return this._playerCount;
		} 
	} 

	private int _playerTotal; 

	public int playerTotal { 
		set { 
			if(!this.hasPlayerTotal()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._playerTotal = value;
		} 
		get { 
			return this._playerTotal;
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

	private int _roomCount; 

	public int roomCount { 
		set { 
			if(!this.hasRoomCount()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this._roomCount = value;
		} 
		get { 
			return this._roomCount;
		} 
	} 

	private int _msgCount; 

	public int msgCount { 
		set { 
			if(!this.hasMsgCount()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this._msgCount = value;
		} 
		get { 
			return this._msgCount;
		} 
	} 

	private int _cards; 

	public int cards { 
		set { 
			if(!this.hasCards()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 2);
			}
			this._cards = value;
		} 
		get { 
			return this._cards;
		} 
	} 

	public static OP_CLUB newBuilder() { 
		return new OP_CLUB(); 
	} 

	public static OP_CLUB decode(byte[] data) { 
		OP_CLUB proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[10]; 

		int total = 0;
		if(this.hasClubId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.clubId);
			total += bytes[0].limit();
		}

		if(this.hasName()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.name);
			    short len = (short) _byte.Length;
			    bytes[1] = ByteBuffer.allocate(2 + len);
			    bytes[1].putShort(len);
				bytes[1].put(_byte);
			total += bytes[1].limit();
		}

		if(this.hasAnnounce()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.announce);
			    short len = (short) _byte.Length;
			    bytes[2] = ByteBuffer.allocate(2 + len);
			    bytes[2].putShort(len);
				bytes[2].put(_byte);
			total += bytes[2].limit();
		}

		if(this.hasOwnerId()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.ownerId);
			total += bytes[3].limit();
		}

		if(this.hasPlayerCount()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.playerCount);
			total += bytes[4].limit();
		}

		if(this.hasPlayerTotal()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putInt(this.playerTotal);
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

		if(this.hasRoomCount()) {
			bytes[7] = ByteBuffer.allocate(4);
			bytes[7].putInt(this.roomCount);
			total += bytes[7].limit();
		}

		if(this.hasMsgCount()) {
			bytes[8] = ByteBuffer.allocate(4);
			bytes[8].putInt(this.msgCount);
			total += bytes[8].limit();
		}

		if(this.hasCards()) {
			bytes[9] = ByteBuffer.allocate(4);
			bytes[9].putInt(this.cards);
			total += bytes[9].limit();
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
		  
		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

		if(this.hasName()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.name = System.Text.Encoding.UTF8.GetString(bytes);
		}

		if(this.hasAnnounce()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.announce = System.Text.Encoding.UTF8.GetString(bytes);
		}

		if(this.hasOwnerId()) {
			this.ownerId = buf.getInt();
		}

		if(this.hasPlayerCount()) {
			this.playerCount = buf.getInt();
		}

		if(this.hasPlayerTotal()) {
			this.playerTotal = buf.getInt();
		}

		if(this.hasAvatar()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.avatar = System.Text.Encoding.UTF8.GetString(bytes);
		}

		if(this.hasRoomCount()) {
			this.roomCount = buf.getInt();
		}

		if(this.hasMsgCount()) {
			this.msgCount = buf.getInt();
		}

		if(this.hasCards()) {
			this.cards = buf.getInt();
		}

	} 

	public bool hasClubId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasName() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasAnnounce() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasOwnerId() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasPlayerCount() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasPlayerTotal() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasAvatar() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasRoomCount() {
		return (this.__flag[0] & 128) != 0;
	}

	public bool hasMsgCount() {
		return (this.__flag[1] & 1) != 0;
	}

	public bool hasCards() {
		return (this.__flag[1] & 2) != 0;
	}

}
}

