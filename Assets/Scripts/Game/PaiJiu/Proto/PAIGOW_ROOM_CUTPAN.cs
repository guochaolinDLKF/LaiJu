//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:22 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_ROOM_CUTPAN { 

	public const int CODE = 501018; 

	private byte[] __flag = new byte[1]; 

	private bool _isCutGuo; 

	public bool isCutGuo { 
		set { 
			if(!this.hasIsCutGuo()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._isCutGuo = value;
		} 
		get { 
			return this._isCutGuo;
		} 
	} 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	public static PAIGOW_ROOM_CUTPAN newBuilder() { 
		return new PAIGOW_ROOM_CUTPAN(); 
	} 

	public static PAIGOW_ROOM_CUTPAN decode(byte[] data) { 
		PAIGOW_ROOM_CUTPAN proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasIsCutGuo()) {
			bytes[0] = ByteBuffer.allocate(1);
			if(this.isCutGuo) {
				bytes[0].put((byte) 1);
			}else{
				bytes[0].put((byte) 0);
			}
			total += bytes[0].limit();
		}

		if(this.hasPos()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.pos);
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
		  
		if(this.hasIsCutGuo()) {
			if(buf.get() == 1) {
				this.isCutGuo = true;
			}else{
				this.isCutGuo = false;
			}
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

	} 

	public bool hasIsCutGuo() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

