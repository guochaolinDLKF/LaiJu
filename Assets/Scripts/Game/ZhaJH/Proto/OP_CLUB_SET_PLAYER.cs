//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:31 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class OP_CLUB_SET_PLAYER { 

	public const int CODE = 99330; 

	private byte[] __flag = new byte[1]; 

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

	private ENUM_PLAYER_IDENTITY _identity; 

	public ENUM_PLAYER_IDENTITY identity { 
		set { 
			if(!this.hasIdentity()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._identity = value;
		} 
		get { 
			return this._identity;
		} 
	} 

	public static OP_CLUB_SET_PLAYER newBuilder() { 
		return new OP_CLUB_SET_PLAYER(); 
	} 

	public static OP_CLUB_SET_PLAYER decode(byte[] data) { 
		OP_CLUB_SET_PLAYER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasClubId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.clubId);
			total += bytes[0].limit();
		}

		if(this.hasPlayerId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.playerId);
			total += bytes[1].limit();
		}

		if(this.hasIdentity()) {
			bytes[2] = ByteBuffer.allocate(1);
			bytes[2].put((byte) this.identity);
			total += bytes[2].limit();
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
		  
		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

		if(this.hasIdentity()) {
			this.identity = (ENUM_PLAYER_IDENTITY) buf.get();
		}

	} 

	public bool hasClubId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasIdentity() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

