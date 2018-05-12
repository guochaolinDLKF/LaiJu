using System.Collections.Generic;

namespace proto.jy {

public class JY_ROOM_OPENPOKER_GET { 

	public const int CODE = 601017; 

	private byte[] __flag = new byte[1]; 

	private List<int> pokerIndex = new List<int>(); 

	public int getPokerIndex(int index) { 
			return this.pokerIndex[index];
	} 
	
	public void addPokerIndex(int value) { 
			if(!this.hasPokerIndex()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.pokerIndex.Add(value);
	} 

	public static JY_ROOM_OPENPOKER_GET newBuilder() { 
		return new JY_ROOM_OPENPOKER_GET(); 
	} 

	public static JY_ROOM_OPENPOKER_GET decode(byte[] data) { 
		JY_ROOM_OPENPOKER_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasPokerIndex()) {
			bytes[0] = ByteBuffer.allocate(this.pokerIndex.Count * 4 + 2);
			bytes[0].putShort((short) this.pokerIndex.Count);
			for(int i=0, len=this.pokerIndex.Count; i<len; i++) {
				bytes[0].putInt(this.pokerIndex[i]);
			}
			total += bytes[0].limit();
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
		  
		if(this.hasPokerIndex()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.pokerIndex.Add(buf.getInt());
			}
		}

	} 

	public int pokerIndexCount() {
		return this.pokerIndex.Count;
	}

	public bool hasPokerIndex() {
		return (this.__flag[0] & 1) != 0;
	}

	public List<int> getPokerIndexList() {
		return this.pokerIndex;
	}

}
}

