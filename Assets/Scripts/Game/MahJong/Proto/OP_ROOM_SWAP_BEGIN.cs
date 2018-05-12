using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_SWAP_BEGIN { 

	public const int CODE = 101035; 

	private byte[] __flag = new byte[16]; 

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

	private List<OP_POKER> poker = new List<OP_POKER>(); 

	public OP_POKER getPoker(int index) { 
			return this.poker[index];
	} 
	
	public void addPoker(OP_POKER value) { 
			if(!this.hasPoker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this.poker.Add(value);
	} 

	private ENUM_SWAP_MODE _mode; 

	public ENUM_SWAP_MODE mode { 
		set { 
			if(!this.hasMode()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._mode = value;
		} 
		get { 
			return this._mode;
		} 
	} 

	public static OP_ROOM_SWAP_BEGIN newBuilder() { 
		return new OP_ROOM_SWAP_BEGIN(); 
	} 

	public static OP_ROOM_SWAP_BEGIN decode(byte[] data) { 
		OP_ROOM_SWAP_BEGIN proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasPlayerId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.playerId);
			total += bytes[0].limit();
		}

		if(this.hasPoker()) {
				int length = 0;
				for(int i=0, len=this.poker.Count; i<len; i++) {
					length += this.poker[i].encode().Length;
				}
				bytes[1] = ByteBuffer.allocate(this.poker.Count * 4 + length + 2);
				bytes[1].putShort((short) this.poker.Count);
				for(int i=0, len=this.poker.Count; i<len; i++) {
					byte[] _byte = this.poker[i].encode();
					bytes[1].putInt(_byte.Length);
					bytes[1].put(_byte);
				}
			total += bytes[1].limit();
		}

		if(this.hasMode()) {
			bytes[2] = ByteBuffer.allocate(1);
			bytes[2].put((byte) this.mode);
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
		  
		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

		if(this.hasPoker()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.poker.Add(OP_POKER.decode(bytes));
			}
		}

		if(this.hasMode()) {
			this.mode = (ENUM_SWAP_MODE) buf.get();
		}

	} 

	public bool hasPlayerId() {
		return (this.__flag[0] & 1) != 0;
	}

	public int pokerCount() {
		return this.poker.Count;
	}

	public bool hasPoker() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasMode() {
		return (this.__flag[0] & 4) != 0;
	}

	public List<OP_POKER> getPokerList() {
		return this.poker;
	}

}
}

