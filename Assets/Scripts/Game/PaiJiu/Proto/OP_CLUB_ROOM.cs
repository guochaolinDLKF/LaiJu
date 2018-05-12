//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:11 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class OP_CLUB_ROOM { 

	public const int CODE = 99315; 

	private byte[] __flag = new byte[1]; 

	private int _gameId; 

	public int gameId { 
		set { 
			if(!this.hasGameId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._gameId = value;
		} 
		get { 
			return this._gameId;
		} 
	} 

	private int _roomId; 

	public int roomId { 
		set { 
			if(!this.hasRoomId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._roomId = value;
		} 
		get { 
			return this._roomId;
		} 
	} 

	private List<int> settingId = new List<int>(); 

	public int getSettingId(int index) { 
			return this.settingId[index];
	} 
	
	public void addSettingId(int value) { 
			if(!this.hasSettingId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this.settingId.Add(value);
	} 

	private int _loop; 

	public int loop { 
		set { 
			if(!this.hasLoop()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._loop = value;
		} 
		get { 
			return this._loop;
		} 
	} 

	private int _maxLoop; 

	public int maxLoop { 
		set { 
			if(!this.hasMaxLoop()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._maxLoop = value;
		} 
		get { 
			return this._maxLoop;
		} 
	} 

	private List<OP_CLUB_PLAYER> player = new List<OP_CLUB_PLAYER>(); 

	public OP_CLUB_PLAYER getPlayer(int index) { 
			return this.player[index];
	} 
	
	public void addPlayer(OP_CLUB_PLAYER value) { 
			if(!this.hasPlayer()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this.player.Add(value);
	} 

	private ENUM_CLUB_ROOM_STATUS _status; 

	public ENUM_CLUB_ROOM_STATUS status { 
		set { 
			if(!this.hasStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._status = value;
		} 
		get { 
			return this._status;
		} 
	} 

	private int _clubId; 

	public int clubId { 
		set { 
			if(!this.hasClubId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this._clubId = value;
		} 
		get { 
			return this._clubId;
		} 
	} 

	public static OP_CLUB_ROOM newBuilder() { 
		return new OP_CLUB_ROOM(); 
	} 

	public static OP_CLUB_ROOM decode(byte[] data) { 
		OP_CLUB_ROOM proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[8]; 

		int total = 0;
		if(this.hasGameId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.gameId);
			total += bytes[0].limit();
		}

		if(this.hasRoomId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.roomId);
			total += bytes[1].limit();
		}

		if(this.hasSettingId()) {
			bytes[2] = ByteBuffer.allocate(this.settingId.Count * 4 + 2);
			bytes[2].putShort((short) this.settingId.Count);
			for(int i=0, len=this.settingId.Count; i<len; i++) {
				bytes[2].putInt(this.settingId[i]);
			}
			total += bytes[2].limit();
		}

		if(this.hasLoop()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.loop);
			total += bytes[3].limit();
		}

		if(this.hasMaxLoop()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.maxLoop);
			total += bytes[4].limit();
		}

		if(this.hasPlayer()) {
				int length = 0;
				for(int i=0, len=this.player.Count; i<len; i++) {
					length += this.player[i].encode().Length;
				}
				bytes[5] = ByteBuffer.allocate(this.player.Count * 4 + length + 2);
				bytes[5].putShort((short) this.player.Count);
				for(int i=0, len=this.player.Count; i<len; i++) {
					byte[] _byte = this.player[i].encode();
					bytes[5].putInt(_byte.Length);
					bytes[5].put(_byte);
				}
			total += bytes[5].limit();
		}

		if(this.hasStatus()) {
			bytes[6] = ByteBuffer.allocate(1);
			bytes[6].put((byte) this.status);
			total += bytes[6].limit();
		}

		if(this.hasClubId()) {
			bytes[7] = ByteBuffer.allocate(4);
			bytes[7].putInt(this.clubId);
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
		  
		if(this.hasGameId()) {
			this.gameId = buf.getInt();
		}

		if(this.hasRoomId()) {
			this.roomId = buf.getInt();
		}

		if(this.hasSettingId()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.settingId.Add(buf.getInt());
			}
		}

		if(this.hasLoop()) {
			this.loop = buf.getInt();
		}

		if(this.hasMaxLoop()) {
			this.maxLoop = buf.getInt();
		}

		if(this.hasPlayer()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.player.Add(OP_CLUB_PLAYER.decode(bytes));
			}
		}

		if(this.hasStatus()) {
			this.status = (ENUM_CLUB_ROOM_STATUS) buf.get();
		}

		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

	} 

	public bool hasGameId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasRoomId() {
		return (this.__flag[0] & 2) != 0;
	}

	public int settingIdCount() {
		return this.settingId.Count;
	}

	public bool hasSettingId() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasLoop() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasMaxLoop() {
		return (this.__flag[0] & 16) != 0;
	}

	public int playerCount() {
		return this.player.Count;
	}

	public bool hasPlayer() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasStatus() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasClubId() {
		return (this.__flag[0] & 128) != 0;
	}

	public List<int> getSettingIdList() {
		return this.settingId;
	}

	public List<OP_CLUB_PLAYER> getPlayerList() {
		return this.player;
	}

}
}

