//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:06 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class OP_CLUB_SET_CARD { 

	public const int CODE = 99331; 

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

	private int _cards; 

	public int cards { 
		set { 
			if(!this.hasCards()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._cards = value;
		} 
		get { 
			return this._cards;
		} 
	} 

	private int _playerId; 

	public int playerId { 
		set { 
			if(!this.hasPlayerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._playerId = value;
		} 
		get { 
			return this._playerId;
		} 
	} 

	public static OP_CLUB_SET_CARD newBuilder() { 
		return new OP_CLUB_SET_CARD(); 
	} 

	public static OP_CLUB_SET_CARD decode(byte[] data) { 
		OP_CLUB_SET_CARD proto = newBuilder();
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

		if(this.hasCards()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.cards);
			total += bytes[1].limit();
		}

		if(this.hasPlayerId()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.playerId);
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

		if(this.hasCards()) {
			this.cards = buf.getInt();
		}

		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

	} 

	public bool hasClubId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasCards() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

