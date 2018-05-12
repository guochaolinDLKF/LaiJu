using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_SHOW_LUCK_GET { 

	public const int CODE = 101023; 

	private byte[] __flag = new byte[16]; 

	private List<OP_POKER> poker = new List<OP_POKER>(); 

	public OP_POKER getPoker(int index) { 
			return this.poker[index];
	} 
	
	public void addPoker(OP_POKER value) { 
			if(!this.hasPoker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.poker.Add(value);
	} 

	public static OP_ROOM_SHOW_LUCK_GET newBuilder() { 
		return new OP_ROOM_SHOW_LUCK_GET(); 
	} 

	public static OP_ROOM_SHOW_LUCK_GET decode(byte[] data) { 
		OP_ROOM_SHOW_LUCK_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasPoker()) {
				int length = 0;
				for(int i=0, len=this.poker.Count; i<len; i++) {
					length += this.poker[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.poker.Count * 4 + length + 2);
				bytes[0].putShort((short) this.poker.Count);
				for(int i=0, len=this.poker.Count; i<len; i++) {
					byte[] _byte = this.poker[i].encode();
					bytes[0].putInt(_byte.Length);
					bytes[0].put(_byte);
				}
			total += bytes[0].limit();
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
		  
		if(this.hasPoker()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.poker.Add(OP_POKER.decode(bytes));
			}
		}

	} 

	public int pokerCount() {
		return this.poker.Count;
	}

	public bool hasPoker() {
		return (this.__flag[0] & 1) != 0;
	}

	public List<OP_POKER> getPokerList() {
		return this.poker;
	}

}
}

