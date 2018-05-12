using System.Collections.Generic;

namespace proto.jy {

public class OP_PLAYER_MESSAGE { 

	public const int CODE = 99103; 

	private byte[] __flag = new byte[1]; 

	private int _playerId; 

	public int playerId { 
		set { 
			if(!this.hasPlayerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._playerId = value;
		} 
		get { 
			return this._playerId;
		} 
	} 

	private byte[] _content; 

	public byte[] content { 
		set { 
			if(!this.hasContent()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._content = value;
		} 
		get { 
			return this._content;
		} 
	} 

	private ENUM_PLAYER_MESSAGE _typeId; 

	public ENUM_PLAYER_MESSAGE typeId { 
		set { 
			if(!this.hasTypeId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._typeId = value;
		} 
		get { 
			return this._typeId;
		} 
	} 

	private int _toId; 

	public int toId { 
		set { 
			if(!this.hasToId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._toId = value;
		} 
		get { 
			return this._toId;
		} 
	} 

	public static OP_PLAYER_MESSAGE newBuilder() { 
		return new OP_PLAYER_MESSAGE(); 
	} 

	public static OP_PLAYER_MESSAGE decode(byte[] data) { 
		OP_PLAYER_MESSAGE proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[4]; 

		int total = 0;
		if(this.hasPlayerId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.playerId);
			total += bytes[0].limit();
		}

		if(this.hasContent()) {
			  byte[] _byte = this.content;
			    int len = _byte.Length;
			    bytes[1] = ByteBuffer.allocate(4 + len);
			    bytes[1].putInt(len);
				bytes[1].put(_byte);
			total += bytes[1].limit();
		}

		if(this.hasTypeId()) {
			bytes[2] = ByteBuffer.allocate(1);
			bytes[2].put((byte) this.typeId);
			total += bytes[2].limit();
		}

		if(this.hasToId()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.toId);
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
		  
		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

		if(this.hasContent()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.content = bytes;
		}

		if(this.hasTypeId()) {
			this.typeId = (ENUM_PLAYER_MESSAGE) buf.get();
		}

		if(this.hasToId()) {
			this.toId = buf.getInt();
		}

	} 

	public bool hasPlayerId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasContent() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasTypeId() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasToId() {
		return (this.__flag[0] & 8) != 0;
	}

}
}

