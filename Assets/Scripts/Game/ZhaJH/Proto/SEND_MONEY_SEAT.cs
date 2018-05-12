//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:50 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class SEND_MONEY_SEAT { 

	public const int CODE = 4013; 

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

	private int _type; 

	public int type { 
		set { 
			if(!this.hasType()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._type = value;
		} 
		get { 
			return this._type;
		} 
	} 

	public static SEND_MONEY_SEAT newBuilder() { 
		return new SEND_MONEY_SEAT(); 
	} 

	public static SEND_MONEY_SEAT decode(byte[] data) { 
		SEND_MONEY_SEAT proto = newBuilder();
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

		if(this.hasType()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.type);
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

		if(this.hasType()) {
			this.type = buf.getInt();
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasType() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

