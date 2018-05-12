//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:28 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_MAHJONG { 

	public const int CODE = 5002; 

	private byte[] __flag = new byte[1]; 

	private int _size; 

	public int size { 
		set { 
			if(!this.hasSize()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._size = value;
		} 
		get { 
			return this._size;
		} 
	} 

	private int _index; 

	public int index { 
		set { 
			if(!this.hasIndex()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._index = value;
		} 
		get { 
			return this._index;
		} 
	} 

	private int _type; 

	public int type { 
		set { 
			if(!this.hasType()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._type = value;
		} 
		get { 
			return this._type;
		} 
	} 

	private PAIGOW_STATUS _mahjong_status; 

	public PAIGOW_STATUS mahjong_status { 
		set { 
			if(!this.hasMahjongStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._mahjong_status = value;
		} 
		get { 
			return this._mahjong_status;
		} 
	} 

	public static PAIGOW_MAHJONG newBuilder() { 
		return new PAIGOW_MAHJONG(); 
	} 

	public static PAIGOW_MAHJONG decode(byte[] data) { 
		PAIGOW_MAHJONG proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[4]; 

		int total = 0;
		if(this.hasSize()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.size);
			total += bytes[0].limit();
		}

		if(this.hasIndex()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.index);
			total += bytes[1].limit();
		}

		if(this.hasType()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.type);
			total += bytes[2].limit();
		}

		if(this.hasMahjongStatus()) {
			bytes[3] = ByteBuffer.allocate(1);
			bytes[3].put((byte) this.mahjong_status);
			total += bytes[3].limit();
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
		  
		if(this.hasSize()) {
			this.size = buf.getInt();
		}

		if(this.hasIndex()) {
			this.index = buf.getInt();
		}

		if(this.hasType()) {
			this.type = buf.getInt();
		}

		if(this.hasMahjongStatus()) {
			this.mahjong_status = (PAIGOW_STATUS) buf.get();
		}

	} 

	public bool hasSize() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasIndex() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasType() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasMahjongStatus() {
		return (this.__flag[0] & 8) != 0;
	}

}
}

