//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:05 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_ROOM_GRABBANKER { 

	public const int CODE = 501019; 

	private byte[] __flag = new byte[1]; 

	private int _secondDice; 

	public int secondDice { 
		set { 
			if(!this.hasSecondDice()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._secondDice = value;
		} 
		get { 
			return this._secondDice;
		} 
	} 

	private int _diceFirst; 

	public int diceFirst { 
		set { 
			if(!this.hasDiceFirst()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._diceFirst = value;
		} 
		get { 
			return this._diceFirst;
		} 
	} 

	private int _isGrabBanker; 

	public int isGrabBanker { 
		set { 
			if(!this.hasIsGrabBanker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._isGrabBanker = value;
		} 
		get { 
			return this._isGrabBanker;
		} 
	} 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	public static PAIGOW_ROOM_GRABBANKER newBuilder() { 
		return new PAIGOW_ROOM_GRABBANKER(); 
	} 

	public static PAIGOW_ROOM_GRABBANKER decode(byte[] data) { 
		PAIGOW_ROOM_GRABBANKER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[4]; 

		int total = 0;
		if(this.hasSecondDice()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.secondDice);
			total += bytes[0].limit();
		}

		if(this.hasDiceFirst()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.diceFirst);
			total += bytes[1].limit();
		}

		if(this.hasIsGrabBanker()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.isGrabBanker);
			total += bytes[2].limit();
		}

		if(this.hasPos()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.pos);
			total += bytes[3].limit();
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
		  
		if(this.hasSecondDice()) {
			this.secondDice = buf.getInt();
		}

		if(this.hasDiceFirst()) {
			this.diceFirst = buf.getInt();
		}

		if(this.hasIsGrabBanker()) {
			this.isGrabBanker = buf.getInt();
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

	} 

	public bool hasSecondDice() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasDiceFirst() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasIsGrabBanker() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 8) != 0;
	}

}
}

