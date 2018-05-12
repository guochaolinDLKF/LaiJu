using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_ASK_FIGHT { 

	public const int CODE = 101011; 

	private byte[] __flag = new byte[16]; 

	private List<OP_POKER_GROUP> askPokerGroup = new List<OP_POKER_GROUP>(); 

	public OP_POKER_GROUP getAskPokerGroup(int index) { 
			return this.askPokerGroup[index];
	} 
	
	public void addAskPokerGroup(OP_POKER_GROUP value) { 
			if(!this.hasAskPokerGroup()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.askPokerGroup.Add(value);
	} 

	private long _countdown; 

	public long countdown { 
		set { 
			if(!this.hasCountdown()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._countdown = value;
		} 
		get { 
			return this._countdown;
		} 
	} 

	public static OP_ROOM_ASK_FIGHT newBuilder() { 
		return new OP_ROOM_ASK_FIGHT(); 
	} 

	public static OP_ROOM_ASK_FIGHT decode(byte[] data) { 
		OP_ROOM_ASK_FIGHT proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasAskPokerGroup()) {
				int length = 0;
				for(int i=0, len=this.askPokerGroup.Count; i<len; i++) {
					length += this.askPokerGroup[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.askPokerGroup.Count * 4 + length + 2);
				bytes[0].putShort((short) this.askPokerGroup.Count);
				for(int i=0, len=this.askPokerGroup.Count; i<len; i++) {
					byte[] _byte = this.askPokerGroup[i].encode();
					bytes[0].putInt(_byte.Length);
					bytes[0].put(_byte);
				}
			total += bytes[0].limit();
		}

		if(this.hasCountdown()) {
			bytes[1] = ByteBuffer.allocate(8);
			bytes[1].putLong(this.countdown);
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
		  
		if(this.hasAskPokerGroup()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.askPokerGroup.Add(OP_POKER_GROUP.decode(bytes));
			}
		}

		if(this.hasCountdown()) {
			this.countdown = buf.getLong();
		}

	} 

	public int askPokerGroupCount() {
		return this.askPokerGroup.Count;
	}

	public bool hasAskPokerGroup() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasCountdown() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<OP_POKER_GROUP> getAskPokerGroupList() {
		return this.askPokerGroup;
	}

}
}

