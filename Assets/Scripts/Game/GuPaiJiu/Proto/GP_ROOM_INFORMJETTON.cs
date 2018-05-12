//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:17 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class GP_ROOM_INFORMJETTON { 

	public const int CODE = 701008; 

	private byte[] __flag = new byte[1]; 

	private List<int> pos = new List<int>(); 

	public int getPos(int index) { 
			return this.pos[index];
	} 
	
	public void addPos(int value) { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.pos.Add(value);
	} 

	public static GP_ROOM_INFORMJETTON newBuilder() { 
		return new GP_ROOM_INFORMJETTON(); 
	} 

	public static GP_ROOM_INFORMJETTON decode(byte[] data) { 
		GP_ROOM_INFORMJETTON proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasPos()) {
			bytes[0] = ByteBuffer.allocate(this.pos.Count * 4 + 2);
			bytes[0].putShort((short) this.pos.Count);
			for(int i=0, len=this.pos.Count; i<len; i++) {
				bytes[0].putInt(this.pos[i]);
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
		  
		if(this.hasPos()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.pos.Add(buf.getInt());
			}
		}

	} 

	public int posCount() {
		return this.pos.Count;
	}

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public List<int> getPosList() {
		return this.pos;
	}

}
}

