using System.Collections.Generic;

namespace proto.mahjong {

public class OP_POKER_GROUP { 

	public const int CODE = 1004; 

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

	private ENUM_POKER_SUBTYPE _subTypeId; 

	public ENUM_POKER_SUBTYPE subTypeId { 
		set { 
			if(!this.hasSubTypeId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._subTypeId = value;
		} 
		get { 
			return this._subTypeId;
		} 
	} 

	private ENUM_POKER_TYPE _typeId; 

	public ENUM_POKER_TYPE typeId { 
		set { 
			if(!this.hasTypeId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._typeId = value;
		} 
		get { 
			return this._typeId;
		} 
	} 

	private List<OP_POKER> poker = new List<OP_POKER>(); 

	public OP_POKER getPoker(int index) { 
			return this.poker[index];
	} 
	
	public void addPoker(OP_POKER value) { 
			if(!this.hasPoker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this.poker.Add(value);
	} 

	public static OP_POKER_GROUP newBuilder() { 
		return new OP_POKER_GROUP(); 
	} 

	public static OP_POKER_GROUP decode(byte[] data) { 
		OP_POKER_GROUP proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[5]; 

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

		if(this.hasSubTypeId()) {
			bytes[2] = ByteBuffer.allocate(1);
			bytes[2].put((byte) this.subTypeId);
			total += bytes[2].limit();
		}

		if(this.hasTypeId()) {
			bytes[3] = ByteBuffer.allocate(1);
			bytes[3].put((byte) this.typeId);
			total += bytes[3].limit();
		}

		if(this.hasPoker()) {
				int length = 0;
				for(int i=0, len=this.poker.Count; i<len; i++) {
					length += this.poker[i].encode().Length;
				}
				bytes[4] = ByteBuffer.allocate(this.poker.Count * 4 + length + 2);
				bytes[4].putShort((short) this.poker.Count);
				for(int i=0, len=this.poker.Count; i<len; i++) {
					byte[] _byte = this.poker[i].encode();
					bytes[4].putInt(_byte.Length);
					bytes[4].put(_byte);
				}
			total += bytes[4].limit();
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

		if(this.hasSubTypeId()) {
			this.subTypeId = (ENUM_POKER_SUBTYPE) buf.get();
		}

		if(this.hasTypeId()) {
			this.typeId = (ENUM_POKER_TYPE) buf.get();
		}

		if(this.hasPoker()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.poker.Add(OP_POKER.decode(bytes));
			}
		}

	} 

	public bool hasTimes() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasSubTypeId() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasTypeId() {
		return (this.__flag[0] & 8) != 0;
	}

	public int pokerCount() {
		return this.poker.Count;
	}

	public bool hasPoker() {
		return (this.__flag[0] & 16) != 0;
	}

	public List<OP_POKER> getPokerList() {
		return this.poker;
	}

}
}

