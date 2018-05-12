//===================================================
//Author      : DRB
//CreateTime  ：1/16/2018 2:59:34 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.common {

public class OP_CLUB_ROOM_SETTING { 

	public const int CODE = 99333; 

	private byte[] __flag = new byte[16]; 

	private bool _isSucceed; 

	public bool isSucceed { 
		set { 
			if(!this.hasIsSucceed()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._isSucceed = value;
		} 
		get { 
			return this._isSucceed;
		} 
	} 

	private int _clubId; 

	public int clubId { 
		set { 
			if(!this.hasClubId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._clubId = value;
		} 
		get { 
			return this._clubId;
		} 
	} 

	private int _gameId; 

	public int gameId { 
		set { 
			if(!this.hasGameId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._gameId = value;
		} 
		get { 
			return this._gameId;
		} 
	} 

	private int _gameIndex; 

	public int gameIndex { 
		set { 
			if(!this.hasGameIndex()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._gameIndex = value;
		} 
		get { 
			return this._gameIndex;
		} 
	} 

	private List<int> settingId = new List<int>(); 

	public int getSettingId(int index) { 
			return this.settingId[index];
	} 
	
	public void addSettingId(int value) { 
			if(!this.hasSettingId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this.settingId.Add(value);
	} 

	private int _playerId; 

	public int playerId { 
		set { 
			if(!this.hasPlayerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._playerId = value;
		} 
		get { 
			return this._playerId;
		} 
	} 

	public static OP_CLUB_ROOM_SETTING newBuilder() { 
		return new OP_CLUB_ROOM_SETTING(); 
	} 

	public static OP_CLUB_ROOM_SETTING decode(byte[] data) { 
		OP_CLUB_ROOM_SETTING proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[6]; 

		int total = 0;
		if(this.hasIsSucceed()) {
			bytes[0] = ByteBuffer.allocate(1);
			if(this.isSucceed) {
				bytes[0].put((byte) 1);
			}else{
				bytes[0].put((byte) 0);
			}
			total += bytes[0].limit();
		}

		if(this.hasClubId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.clubId);
			total += bytes[1].limit();
		}

		if(this.hasGameId()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.gameId);
			total += bytes[2].limit();
		}

		if(this.hasGameIndex()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.gameIndex);
			total += bytes[3].limit();
		}

		if(this.hasSettingId()) {
			bytes[4] = ByteBuffer.allocate(this.settingId.Count * 4 + 2);
			bytes[4].putShort((short) this.settingId.Count);
			for(int i=0, len=this.settingId.Count; i<len; i++) {
				bytes[4].putInt(this.settingId[i]);
			}
			total += bytes[4].limit();
		}

		if(this.hasPlayerId()) {
			bytes[5] = ByteBuffer.allocate(4);
			bytes[5].putInt(this.playerId);
			total += bytes[5].limit();
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
		  
		if(this.hasIsSucceed()) {
			if(buf.get() == 1) {
				this.isSucceed = true;
			}else{
				this.isSucceed = false;
			}
		}

		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

		if(this.hasGameId()) {
			this.gameId = buf.getInt();
		}

		if(this.hasGameIndex()) {
			this.gameIndex = buf.getInt();
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

	} 

	public bool hasIsSucceed() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasClubId() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasGameId() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasGameIndex() {
		return (this.__flag[0] & 8) != 0;
	}

	public int settingIdCount() {
		return this.settingId.Count;
	}

	public bool hasSettingId() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[0] & 32) != 0;
	}

	public List<int> getSettingIdList() {
		return this.settingId;
	}

}
}

