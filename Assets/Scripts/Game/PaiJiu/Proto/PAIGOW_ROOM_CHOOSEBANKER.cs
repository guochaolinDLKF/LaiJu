//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:16 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_ROOM_CHOOSEBANKER { 

	public const int CODE = 501012; 

	private byte[] __flag = new byte[1]; 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	private bool _isBanker; 

	public bool isBanker { 
		set { 
			if(!this.hasIsBanker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._isBanker = value;
		} 
		get { 
			return this._isBanker;
		} 
	} 

	public static PAIGOW_ROOM_CHOOSEBANKER newBuilder() { 
		return new PAIGOW_ROOM_CHOOSEBANKER(); 
	} 

	public static PAIGOW_ROOM_CHOOSEBANKER decode(byte[] data) { 
		PAIGOW_ROOM_CHOOSEBANKER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pos);
			total += bytes[0].limit();
		}

		if(this.hasIsBanker()) {
			bytes[1] = ByteBuffer.allocate(1);
			if(this.isBanker) {
				bytes[1].put((byte) 1);
			}else{
				bytes[1].put((byte) 0);
			}
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
		  
		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasIsBanker()) {
			if(buf.get() == 1) {
				this.isBanker = true;
			}else{
				this.isBanker = false;
			}
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasIsBanker() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

