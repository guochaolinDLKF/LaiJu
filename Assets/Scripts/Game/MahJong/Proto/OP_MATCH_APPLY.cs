using System.Collections.Generic;

namespace proto.mahjong {

public class OP_MATCH_APPLY { 

	public const int CODE = 102001; 

	private byte[] __flag = new byte[16]; 

	private int _matchId; 

	public int matchId { 
		set { 
			if(!this.hasMatchId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._matchId = value;
		} 
		get { 
			return this._matchId;
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

	public static OP_MATCH_APPLY newBuilder() { 
		return new OP_MATCH_APPLY(); 
	} 

	public static OP_MATCH_APPLY decode(byte[] data) { 
		OP_MATCH_APPLY proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasMatchId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.matchId);
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
		  
		if(this.hasMatchId()) {
			this.matchId = buf.getInt();
		}

		if(this.hasCount()) {
			this.count = buf.getInt();
		}

		if(this.hasTotal()) {
			this.total = buf.getInt();
		}

	} 

	public bool hasMatchId() {
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

