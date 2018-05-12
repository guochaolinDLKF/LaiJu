using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_FIGHT { 

	public const int CODE = 101013; 

	private byte[] __flag = new byte[16]; 

	private ENUM_POKER_SUBTYPE _subTypeId; 

	public ENUM_POKER_SUBTYPE subTypeId { 
		set { 
			if(!this.hasSubTypeId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this.poker.Add(value);
	} 

	private int _playerId; 

	public int playerId { 
		set { 
			if(!this.hasPlayerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._playerId = value;
		} 
		get { 
			return this._playerId;
		} 
	} 

	private long _countdown; 

	public long countdown { 
		set { 
			if(!this.hasCountdown()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._countdown = value;
		} 
		get { 
			return this._countdown;
		} 
	} 

	public static OP_ROOM_FIGHT newBuilder() { 
		return new OP_ROOM_FIGHT(); 
	} 

	public static OP_ROOM_FIGHT decode(byte[] data) { 
		OP_ROOM_FIGHT proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[5]; 

		int total = 0;
		if(this.hasSubTypeId()) {
			bytes[0] = ByteBuffer.allocate(1);
			bytes[0].put((byte) this.subTypeId);
			total += bytes[0].limit();
		}

		if(this.hasTypeId()) {
			bytes[1] = ByteBuffer.allocate(1);
			bytes[1].put((byte) this.typeId);
			total += bytes[1].limit();
		}

		if(this.hasPoker()) {
				int length = 0;
				for(int i=0, len=this.poker.Count; i<len; i++) {
					length += this.poker[i].encode().Length;
				}
				bytes[2] = ByteBuffer.allocate(this.poker.Count * 4 + length + 2);
				bytes[2].putShort((short) this.poker.Count);
				for(int i=0, len=this.poker.Count; i<len; i++) {
					byte[] _byte = this.poker[i].encode();
					bytes[2].putInt(_byte.Length);
					bytes[2].put(_byte);
				}
			total += bytes[2].limit();
		}

		if(this.hasPlayerId()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.playerId);
			total += bytes[3].limit();
		}

		if(this.hasCountdown()) {
			bytes[4] = ByteBuffer.allocate(8);
			bytes[4].putLong(this.countdown);
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

		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

		if(this.hasCountdown()) {
			this.countdown = buf.getLong();
		}

	} 

	public bool hasSubTypeId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasTypeId() {
		return (this.__flag[0] & 2) != 0;
	}

	public int pokerCount() {
		return this.poker.Count;
	}

	public bool hasPoker() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasCountdown() {
		return (this.__flag[0] & 16) != 0;
	}

	public List<OP_POKER> getPokerList() {
		return this.poker;
	}

}
}

