using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_OPERATE_GET { 

	public const int CODE = 101010; 

	private byte[] __flag = new byte[16]; 

	private bool _isListen; 

	public bool isListen { 
		set { 
			if(!this.hasIsListen()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._isListen = value;
		} 
		get { 
			return this._isListen;
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

	public static OP_ROOM_OPERATE_GET newBuilder() { 
		return new OP_ROOM_OPERATE_GET(); 
	} 

	public static OP_ROOM_OPERATE_GET decode(byte[] data) { 
		OP_ROOM_OPERATE_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasIsListen()) {
			bytes[0] = ByteBuffer.allocate(1);
			if(this.isListen) {
				bytes[0].put((byte) 1);
			}else{
				bytes[0].put((byte) 0);
			}
			total += bytes[0].limit();
		}

		if(this.hasIndex()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.index);
			total += bytes[1].limit();
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
		  
		if(this.hasIsListen()) {
			if(buf.get() == 1) {
				this.isListen = true;
			}else{
				this.isListen = false;
			}
		}

		if(this.hasIndex()) {
			this.index = buf.getInt();
		}

	} 

	public bool hasIsListen() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasIndex() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

