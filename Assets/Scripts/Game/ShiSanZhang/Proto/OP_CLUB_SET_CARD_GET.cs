//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 5:30:36 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.sss {

public class OP_CLUB_SET_CARD_GET { 

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

	public static OP_CLUB_SET_CARD_GET newBuilder() { 
		return new OP_CLUB_SET_CARD_GET(); 
	} 

	public static OP_CLUB_SET_CARD_GET decode(byte[] data) { 
		OP_CLUB_SET_CARD_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

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

	} 

	public bool hasClubId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasCards() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

