//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:08 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class OP_CLUB_MSG { 

	public const int CODE = 99316; 

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

	private byte[] _content; 

	public byte[] content { 
		set { 
			if(!this.hasContent()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._content = value;
		} 
		get { 
			return this._content;
		} 
	} 

	private ENUM_PLAYER_MESSAGE _typeId; 

	public ENUM_PLAYER_MESSAGE typeId { 
		set { 
			if(!this.hasTypeId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._typeId = value;
		} 
		get { 
			return this._typeId;
		} 
	} 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	private int _clubId; 

	public int clubId { 
		set { 
			if(!this.hasClubId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._clubId = value;
		} 
		get { 
			return this._clubId;
		} 
	} 

	public static OP_CLUB_MSG newBuilder() { 
		return new OP_CLUB_MSG(); 
	} 

	public static OP_CLUB_MSG decode(byte[] data) { 
		OP_CLUB_MSG proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[5]; 

		int total = 0;
		if(this.hasPlayerId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.playerId);
			total += bytes[0].limit();
		}

		if(this.hasContent()) {
			  byte[] _byte = this.content;
			    int len = _byte.Length;
			    bytes[1] = ByteBuffer.allocate(4 + len);
			    bytes[1].putInt(len);
				bytes[1].put(_byte);
			total += bytes[1].limit();
		}

		if(this.hasTypeId()) {
			bytes[2] = ByteBuffer.allocate(1);
			bytes[2].put((byte) this.typeId);
			total += bytes[2].limit();
		}

		if(this.hasUnixtime()) {
			bytes[3] = ByteBuffer.allocate(8);
			bytes[3].putLong(this.unixtime);
			total += bytes[3].limit();
		}

		if(this.hasClubId()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.clubId);
			total += bytes[4].limit();
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

		if(this.hasContent()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.content = bytes;
		}

		if(this.hasTypeId()) {
			this.typeId = (ENUM_PLAYER_MESSAGE) buf.get();
		}

		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
		}

		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

	} 

	public bool hasPlayerId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasContent() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasTypeId() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasUnixtime() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasClubId() {
		return (this.__flag[0] & 16) != 0;
	}

}
}

