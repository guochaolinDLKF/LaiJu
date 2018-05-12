//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 5:30:26 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.sss {

public class SECOND_POKER_INFO { 

	public const int CODE = 5; 

	private byte[] __flag = new byte[1]; 

	private int _index; 

	public int index { 
		set { 
			if(!this.hasIndex()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._index = value;
		} 
		get { 
			return this._index;
		} 
	} 

	private int _color; 

	public int color { 
		set { 
			if(!this.hasColor()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._color = value;
		} 
		get { 
			return this._color;
		} 
	} 

	private int _size; 

	public int size { 
		set { 
			if(!this.hasSize()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._size = value;
		} 
		get { 
			return this._size;
		} 
	} 

	private int _pocker_status; 

	public int pocker_status { 
		set { 
			if(!this.hasPockerStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._pocker_status = value;
		} 
		get { 
			return this._pocker_status;
		} 
	} 

	private int _type; 

	public int type { 
		set { 
			if(!this.hasType()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._type = value;
		} 
		get { 
			return this._type;
		} 
	} 

	public static SECOND_POKER_INFO newBuilder() { 
		return new SECOND_POKER_INFO(); 
	} 

	public static SECOND_POKER_INFO decode(byte[] data) { 
		SECOND_POKER_INFO proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[5]; 

		int total = 0;
		if(this.hasIndex()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.index);
			total += bytes[0].limit();
		}

		if(this.hasColor()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.color);
			total += bytes[1].limit();
		}

		if(this.hasSize()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.size);
			total += bytes[2].limit();
		}

		if(this.hasPockerStatus()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.pocker_status);
			total += bytes[3].limit();
		}

		if(this.hasType()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.type);
			total += bytes[4].limit();
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
		  
		if(this.hasIndex()) {
			this.index = buf.getInt();
		}

		if(this.hasColor()) {
			this.color = buf.getInt();
		}

		if(this.hasSize()) {
			this.size = buf.getInt();
		}

		if(this.hasPockerStatus()) {
			this.pocker_status = buf.getInt();
		}

		if(this.hasType()) {
			this.type = buf.getInt();
		}

	} 

	public bool hasIndex() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasColor() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasSize() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasPockerStatus() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasType() {
		return (this.__flag[0] & 16) != 0;
	}

}
}

