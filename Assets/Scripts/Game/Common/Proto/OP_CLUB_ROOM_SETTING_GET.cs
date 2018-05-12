//===================================================
//Author      : DRB
//CreateTime  ：1/16/2018 2:59:18 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.common {

public class OP_CLUB_ROOM_SETTING_GET { 

	public const int CODE = 99333; 

	private byte[] __flag = new byte[16]; 

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

	private int _gameId; 

	public int gameId { 
		set { 
			if(!this.hasGameId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._gameId = value;
		} 
		get { 
			return this._gameId;
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

	public static OP_CLUB_ROOM_SETTING_GET newBuilder() { 
		return new OP_CLUB_ROOM_SETTING_GET(); 
	} 

	public static OP_CLUB_ROOM_SETTING_GET decode(byte[] data) { 
		OP_CLUB_ROOM_SETTING_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[4]; 

		int total = 0;
		if(this.hasClubId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.clubId);
			total += bytes[0].limit();
		}

		if(this.hasGameId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.gameId);
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

		if(this.hasGameIndex()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.gameIndex);
			total += bytes[3].limit();
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
		  
		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

		if(this.hasGameId()) {
			this.gameId = buf.getInt();
		}

		if(this.hasSettingId()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.settingId.Add(buf.getInt());
			}
		}

		if(this.hasGameIndex()) {
			this.gameIndex = buf.getInt();
		}

	} 

	public bool hasClubId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasGameId() {
		return (this.__flag[0] & 2) != 0;
	}

	public int settingIdCount() {
		return this.settingId.Count;
	}

	public bool hasSettingId() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasGameIndex() {
		return (this.__flag[0] & 8) != 0;
	}

	public List<int> getSettingIdList() {
		return this.settingId;
	}

}
}

