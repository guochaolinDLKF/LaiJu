using System.Collections.Generic;

namespace proto.mahjong {

public class OP_POKER_SETTLE { 

	public const int CODE = 1007; 

	private byte[] __flag = new byte[16]; 

	private int _times; 

	public int times { 
		set { 
			if(!this.hasTimes()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._times = value;
		} 
		get { 
			return this._times;
		} 
	} 

	private int _playerId; 

	public int playerId { 
		set { 
			if(!this.hasPlayerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._playerId = value;
		} 
		get { 
			return this._playerId;
		} 
	} 

	private int _cfgId; 

	public int cfgId { 
		set { 
			if(!this.hasCfgId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._cfgId = value;
		} 
		get { 
			return this._cfgId;
		} 
	} 

	private OP_POKER _poker; 

	public OP_POKER poker { 
		set { 
			if(!this.hasPoker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._poker = value;
		} 
		get { 
			return this._poker;
		} 
	} 

	public static OP_POKER_SETTLE newBuilder() { 
		return new OP_POKER_SETTLE(); 
	} 

	public static OP_POKER_SETTLE decode(byte[] data) { 
		OP_POKER_SETTLE proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[4]; 

		int total = 0;
		if(this.hasTimes()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.times);
			total += bytes[0].limit();
		}

		if(this.hasPlayerId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.playerId);
			total += bytes[1].limit();
		}

		if(this.hasCfgId()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.cfgId);
			total += bytes[2].limit();
		}

		if(this.hasPoker()) {
			byte[] _byte = this.poker.encode();
			int len = _byte.Length;
			bytes[3] = ByteBuffer.allocate(4 + len);
			bytes[3].putInt(len);
			bytes[3].put(_byte);
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
		  
		if(this.hasTimes()) {
			this.times = buf.getInt();
		}

		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

		if(this.hasCfgId()) {
			this.cfgId = buf.getInt();
		}

		if(this.hasPoker()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.poker = OP_POKER.decode(bytes);
		}

	} 

	public bool hasTimes() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasCfgId() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasPoker() {
		return (this.__flag[0] & 8) != 0;
	}

}
}

