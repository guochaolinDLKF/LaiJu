//===================================================
//Author      : DRB
//CreateTime  ：12/5/2017 12:01:39 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.pdk {

public class OP_CLUB_RECORD { 

	public const int CODE = 99328; 

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

	private long _beginTime; 

	public long beginTime { 
		set { 
			if(!this.hasBeginTime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._beginTime = value;
		} 
		get { 
			return this._beginTime;
		} 
	} 

	private int _maxLoop; 

	public int maxLoop { 
		set { 
			if(!this.hasMaxLoop()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._maxLoop = value;
		} 
		get { 
			return this._maxLoop;
		} 
	} 

	private List<OP_CLUB_RECORD_PLAYER> player = new List<OP_CLUB_RECORD_PLAYER>(); 

	public OP_CLUB_RECORD_PLAYER getPlayer(int index) { 
			return this.player[index];
	} 
	
	public void addPlayer(OP_CLUB_RECORD_PLAYER value) { 
			if(!this.hasPlayer()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this.player.Add(value);
	} 

	private int _recordId; 

	public int recordId { 
		set { 
			if(!this.hasRecordId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._recordId = value;
		} 
		get { 
			return this._recordId;
		} 
	} 

	private int _clubId; 

	public int clubId { 
		set { 
			if(!this.hasClubId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._clubId = value;
		} 
		get { 
			return this._clubId;
		} 
	} 

	public static OP_CLUB_RECORD newBuilder() { 
		return new OP_CLUB_RECORD(); 
	} 

	public static OP_CLUB_RECORD decode(byte[] data) { 
		OP_CLUB_RECORD proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[7]; 

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

		if(this.hasBeginTime()) {
			bytes[2] = ByteBuffer.allocate(8);
			bytes[2].putLong(this.beginTime);
			total += bytes[2].limit();
		}

		if(this.hasMaxLoop()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.maxLoop);
			total += bytes[3].limit();
		}

		if(this.hasPlayer()) {
				int length = 0;
				for(int i=0, len=this.player.Count; i<len; i++) {
					length += this.player[i].encode().Length;
				}
				bytes[4] = ByteBuffer.allocate(this.player.Count * 4 + length + 2);
				bytes[4].putShort((short) this.player.Count);
				for(int i=0, len=this.player.Count; i<len; i++) {
					byte[] _byte = this.player[i].encode();
					bytes[4].putInt(_byte.Length);
					bytes[4].put(_byte);
				}
			total += bytes[4].limit();
		}

		if(this.hasRecordId()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putInt(this.recordId);
			total += bytes[5].limit();
		}

		if(this.hasClubId()) {
			bytes[6] = ByteBuffer.allocate(4);
			bytes[6].putInt(this.clubId);
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
		  
		if(this.hasGameId()) {
			this.gameId = buf.getInt();
		}

		if(this.hasRoomId()) {
			this.roomId = buf.getInt();
		}

		if(this.hasBeginTime()) {
			this.beginTime = buf.getLong();
		}

		if(this.hasMaxLoop()) {
			this.maxLoop = buf.getInt();
		}

		if(this.hasPlayer()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.player.Add(OP_CLUB_RECORD_PLAYER.decode(bytes));
			}
		}

		if(this.hasRecordId()) {
			this.recordId = buf.getInt();
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

	public bool hasBeginTime() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasMaxLoop() {
		return (this.__flag[0] & 8) != 0;
	}

	public int playerCount() {
		return this.player.Count;
	}

	public bool hasPlayer() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasRecordId() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasClubId() {
		return (this.__flag[0] & 64) != 0;
	}

	public List<OP_CLUB_RECORD_PLAYER> getPlayerList() {
		return this.player;
	}

}
}

