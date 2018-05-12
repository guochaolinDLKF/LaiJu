//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:17 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_ROOM_READY { 

	public const int CODE = 501005; 

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

	private SEAT_STATUS _seat_status; 

	public SEAT_STATUS seat_status { 
		set { 
			if(!this.hasSeatStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._seat_status = value;
		} 
		get { 
			return this._seat_status;
		} 
	} 

	public static PAIGOW_ROOM_READY newBuilder() { 
		return new PAIGOW_ROOM_READY(); 
	} 

	public static PAIGOW_ROOM_READY decode(byte[] data) { 
		PAIGOW_ROOM_READY proto = newBuilder();
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

		if(this.hasSeatStatus()) {
			bytes[1] = ByteBuffer.allocate(1);
			bytes[1].put((byte) this.seat_status);
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

		if(this.hasSeatStatus()) {
			this.seat_status = (SEAT_STATUS) buf.get();
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasSeatStatus() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

