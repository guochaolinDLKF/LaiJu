//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:18 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_ROOM_GRABBANKER_GET { 

	public const int CODE = 501019; 

	private byte[] __flag = new byte[1]; 

	private int _isGrabBanker; 

	public int isGrabBanker { 
		set { 
			if(!this.hasIsGrabBanker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._isGrabBanker = value;
		} 
		get { 
			return this._isGrabBanker;
		} 
	} 

	public static PAIGOW_ROOM_GRABBANKER_GET newBuilder() { 
		return new PAIGOW_ROOM_GRABBANKER_GET(); 
	} 

	public static PAIGOW_ROOM_GRABBANKER_GET decode(byte[] data) { 
		PAIGOW_ROOM_GRABBANKER_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasIsGrabBanker()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.isGrabBanker);
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
		  
		if(this.hasIsGrabBanker()) {
			this.isGrabBanker = buf.getInt();
		}

	} 

	public bool hasIsGrabBanker() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

