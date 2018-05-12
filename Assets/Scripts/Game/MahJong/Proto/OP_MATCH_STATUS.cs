using System.Collections.Generic;

namespace proto.mahjong {

public class OP_MATCH_STATUS { 

	public const int CODE = 102003; 

	private byte[] __flag = new byte[16]; 

	private ENUM_MATCH_STATUS _status; 

	public ENUM_MATCH_STATUS status { 
		set { 
			if(!this.hasStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._status = value;
		} 
		get { 
			return this._status;
		} 
	} 

	private int _count; 

	public int count { 
		set { 
			if(!this.hasCount()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._count = value;
		} 
		get { 
			return this._count;
		} 
	} 

	private int _total; 

	public int total { 
		set { 
			if(!this.hasTotal()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._total = value;
		} 
		get { 
			return this._total;
		} 
	} 

	public static OP_MATCH_STATUS newBuilder() { 
		return new OP_MATCH_STATUS(); 
	} 

	public static OP_MATCH_STATUS decode(byte[] data) { 
		OP_MATCH_STATUS proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasStatus()) {
			bytes[0] = ByteBuffer.allocate(1);
			bytes[0].put((byte) this.status);
			total += bytes[0].limit();
		}

		if(this.hasCount()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.count);
			total += bytes[1].limit();
		}

		if(this.hasTotal()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.total);
			total += bytes[2].limit();
		}

	
		ByteBuffer buf = ByteBuffer.allocate(16 + total);
	
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
		  
		if(this.hasStatus()) {
			this.status = (ENUM_MATCH_STATUS) buf.get();
		}

		if(this.hasCount()) {
			this.count = buf.getInt();
		}

		if(this.hasTotal()) {
			this.total = buf.getInt();
		}

	} 

	public bool hasStatus() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasCount() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasTotal() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

