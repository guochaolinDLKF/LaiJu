//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:19 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_ROOM_CUTPOKER { 

	public const int CODE = 501017; 

	private byte[] __flag = new byte[1]; 

	private int _cutPokerIndex; 

	public int cutPokerIndex { 
		set { 
			if(!this.hasCutPokerIndex()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._cutPokerIndex = value;
		} 
		get { 
			return this._cutPokerIndex;
		} 
	} 

	private int _isCutPoker; 

	public int isCutPoker { 
		set { 
			if(!this.hasIsCutPoker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._isCutPoker = value;
		} 
		get { 
			return this._isCutPoker;
		} 
	} 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	public static PAIGOW_ROOM_CUTPOKER newBuilder() { 
		return new PAIGOW_ROOM_CUTPOKER(); 
	} 

	public static PAIGOW_ROOM_CUTPOKER decode(byte[] data) { 
		PAIGOW_ROOM_CUTPOKER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasCutPokerIndex()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.cutPokerIndex);
			total += bytes[0].limit();
		}

		if(this.hasIsCutPoker()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.isCutPoker);
			total += bytes[1].limit();
		}

		if(this.hasPos()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.pos);
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
		  
		if(this.hasCutPokerIndex()) {
			this.cutPokerIndex = buf.getInt();
		}

		if(this.hasIsCutPoker()) {
			this.isCutPoker = buf.getInt();
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

	} 

	public bool hasCutPokerIndex() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasIsCutPoker() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

