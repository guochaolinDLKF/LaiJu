//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:12 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_ROOM_CUTPAN_GET { 

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

	public static PAIGOW_ROOM_CUTPAN_GET newBuilder() { 
		return new PAIGOW_ROOM_CUTPAN_GET(); 
	} 

	public static PAIGOW_ROOM_CUTPAN_GET decode(byte[] data) { 
		PAIGOW_ROOM_CUTPAN_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

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

	} 

	public bool hasIsCutGuo() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

