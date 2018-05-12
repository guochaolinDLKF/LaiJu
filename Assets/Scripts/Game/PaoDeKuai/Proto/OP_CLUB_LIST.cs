//===================================================
//Author      : DRB
//CreateTime  ：12/5/2017 12:01:56 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.pdk {

public class OP_CLUB_LIST { 

	public const int CODE = 99306; 

	private byte[] __flag = new byte[1]; 

	private List<OP_CLUB> club = new List<OP_CLUB>(); 

	public OP_CLUB getClub(int index) { 
			return this.club[index];
	} 
	
	public void addClub(OP_CLUB value) { 
			if(!this.hasClub()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.club.Add(value);
	} 

	public static OP_CLUB_LIST newBuilder() { 
		return new OP_CLUB_LIST(); 
	} 

	public static OP_CLUB_LIST decode(byte[] data) { 
		OP_CLUB_LIST proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasClub()) {
				int length = 0;
				for(int i=0, len=this.club.Count; i<len; i++) {
					length += this.club[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.club.Count * 4 + length + 2);
				bytes[0].putShort((short) this.club.Count);
				for(int i=0, len=this.club.Count; i<len; i++) {
					byte[] _byte = this.club[i].encode();
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
		  
		if(this.hasClub()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.club.Add(OP_CLUB.decode(bytes));
			}
		}

	} 

	public int clubCount() {
		return this.club.Count;
	}

	public bool hasClub() {
		return (this.__flag[0] & 1) != 0;
	}

	public List<OP_CLUB> getClubList() {
		return this.club;
	}

}
}

