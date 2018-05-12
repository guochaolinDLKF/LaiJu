using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_FIGHT_GET { 

	public const int CODE = 101013; 

	private byte[] __flag = new byte[16]; 

	private List<int> index = new List<int>(); 

	public int getIndex(int index) { 
			return this.index[index];
	} 
	
	public void addIndex(int value) { 
			if(!this.hasIndex()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.index.Add(value);
	} 

	private ENUM_POKER_TYPE _typeId; 

	public ENUM_POKER_TYPE typeId { 
		set { 
			if(!this.hasTypeId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._typeId = value;
		} 
		get { 
			return this._typeId;
		} 
	} 

	private bool _isListen; 

	public bool isListen { 
		set { 
			if(!this.hasIsListen()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._isListen = value;
		} 
		get { 
			return this._isListen;
		} 
	} 

	public static OP_ROOM_FIGHT_GET newBuilder() { 
		return new OP_ROOM_FIGHT_GET(); 
	} 

	public static OP_ROOM_FIGHT_GET decode(byte[] data) { 
		OP_ROOM_FIGHT_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasIndex()) {
			bytes[0] = ByteBuffer.allocate(this.index.Count * 4 + 2);
			bytes[0].putShort((short) this.index.Count);
			for(int i=0, len=this.index.Count; i<len; i++) {
				bytes[0].putInt(this.index[i]);
			}
			total += bytes[0].limit();
		}

		if(this.hasTypeId()) {
			bytes[1] = ByteBuffer.allocate(1);
			bytes[1].put((byte) this.typeId);
			total += bytes[1].limit();
		}

		if(this.hasIsListen()) {
			bytes[2] = ByteBuffer.allocate(1);
			if(this.isListen) {
				bytes[2].put((byte) 1);
			}else{
				bytes[2].put((byte) 0);
			}
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
		  
		if(this.hasIndex()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.index.Add(buf.getInt());
			}
		}

		if(this.hasTypeId()) {
			this.typeId = (ENUM_POKER_TYPE) buf.get();
		}

		if(this.hasIsListen()) {
			if(buf.get() == 1) {
				this.isListen = true;
			}else{
				this.isListen = false;
			}
		}

	} 

	public int indexCount() {
		return this.index.Count;
	}

	public bool hasIndex() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasTypeId() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasIsListen() {
		return (this.__flag[0] & 4) != 0;
	}

	public List<int> getIndexList() {
		return this.index;
	}

}
}

