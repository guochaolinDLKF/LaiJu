using System.Collections.Generic;

namespace proto.mahjong {

public class OP_MATCH_EXISTS { 

	public const int CODE = 102009; 

	private byte[] __flag = new byte[16]; 

	private bool _isExists; 

	public bool isExists { 
		set { 
			if(!this.hasIsExists()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._isExists = value;
		} 
		get { 
			return this._isExists;
		} 
	} 

	private ENUM_MATCH_STATUS _status; 

	public ENUM_MATCH_STATUS status { 
		set { 
			if(!this.hasStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._total = value;
		} 
		get { 
			return this._total;
		} 
	} 

	public static OP_MATCH_EXISTS newBuilder() { 
		return new OP_MATCH_EXISTS(); 
	} 

	public static OP_MATCH_EXISTS decode(byte[] data) { 
		OP_MATCH_EXISTS proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[4]; 

		int total = 0;
		if(this.hasIsExists()) {
			bytes[0] = ByteBuffer.allocate(1);
			if(this.isExists) {
				bytes[0].put((byte) 1);
			}else{
				bytes[0].put((byte) 0);
			}
			total += bytes[0].limit();
		}

		if(this.hasStatus()) {
			bytes[1] = ByteBuffer.allocate(1);
			bytes[1].put((byte) this.status);
			total += bytes[1].limit();
		}

		if(this.hasCount()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.count);
			total += bytes[2].limit();
		}

		if(this.hasTotal()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.total);
			total += bytes[3].limit();
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
		  
		if(this.hasIsExists()) {
			if(buf.get() == 1) {
				this.isExists = true;
			}else{
				this.isExists = false;
			}
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

	public bool hasIsExists() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasStatus() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasCount() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasTotal() {
		return (this.__flag[0] & 8) != 0;
	}

}
}

