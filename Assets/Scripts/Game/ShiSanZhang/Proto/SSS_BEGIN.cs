//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 5:30:45 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.sss {

public class SSS_BEGIN { 

	public const int CODE = 1011005; 

	private byte[] __flag = new byte[1]; 

	private List<DEAL_POKER> dealPoker = new List<DEAL_POKER>(); 

	public DEAL_POKER getDealPoker(int index) { 
			return this.dealPoker[index];
	} 
	
	public void addDealPoker(DEAL_POKER value) { 
			if(!this.hasDealPoker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.dealPoker.Add(value);
	} 

	public static SSS_BEGIN newBuilder() { 
		return new SSS_BEGIN(); 
	} 

	public static SSS_BEGIN decode(byte[] data) { 
		SSS_BEGIN proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasDealPoker()) {
				int length = 0;
				for(int i=0, len=this.dealPoker.Count; i<len; i++) {
					length += this.dealPoker[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.dealPoker.Count * 4 + length + 2);
				bytes[0].putShort((short) this.dealPoker.Count);
				for(int i=0, len=this.dealPoker.Count; i<len; i++) {
					byte[] _byte = this.dealPoker[i].encode();
					bytes[0].putInt(_byte.Length);
					bytes[0].put(_byte);
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
		  
		if(this.hasDealPoker()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.dealPoker.Add(DEAL_POKER.decode(bytes));
			}
		}

	} 

	public int dealPokerCount() {
		return this.dealPoker.Count;
	}

	public bool hasDealPoker() {
		return (this.__flag[0] & 1) != 0;
	}

	public List<DEAL_POKER> getDealPokerList() {
		return this.dealPoker;
	}

}
}

