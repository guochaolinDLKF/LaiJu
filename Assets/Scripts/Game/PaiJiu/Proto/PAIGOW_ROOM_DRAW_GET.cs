//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:34 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_ROOM_DRAW_GET { 

	public const int CODE = 501008; 

	private byte[] __flag = new byte[1]; 

	private List<int> index = new List<int>(); 

	public int getIndex(int index) { 
			return this.index[index];
	} 
	
	public void addIndex(int value) { 
			if(!this.hasIndex()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.index.Add(value);
	} 

	public static PAIGOW_ROOM_DRAW_GET newBuilder() { 
		return new PAIGOW_ROOM_DRAW_GET(); 
	} 

	public static PAIGOW_ROOM_DRAW_GET decode(byte[] data) { 
		PAIGOW_ROOM_DRAW_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasIndex()) {
			bytes[0] = ByteBuffer.allocate(this.index.Count * 4 + 2);
			bytes[0].putShort((short) this.index.Count);
			for(int i=0, len=this.index.Count; i<len; i++) {
				bytes[0].putInt(this.index[i]);
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
		  
		if(this.hasIndex()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.index.Add(buf.getInt());
			}
		}

	} 

	public int indexCount() {
		return this.index.Count;
	}

	public bool hasIndex() {
		return (this.__flag[0] & 1) != 0;
	}

	public List<int> getIndexList() {
		return this.index;
	}

}
}

