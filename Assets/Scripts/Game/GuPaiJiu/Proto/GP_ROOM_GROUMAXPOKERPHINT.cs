//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:18 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class GP_ROOM_GROUMAXPOKERPHINT { 

	public const int CODE = 701035; 

	private byte[] __flag = new byte[1]; 

	private List<int> pokerIndexList = new List<int>(); 

	public int getPokerIndexList(int index) { 
			return this.pokerIndexList[index];
	} 
	
	public void addPokerIndexList(int value) { 
			if(!this.hasPokerIndexList()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.pokerIndexList.Add(value);
	} 

	public static GP_ROOM_GROUMAXPOKERPHINT newBuilder() { 
		return new GP_ROOM_GROUMAXPOKERPHINT(); 
	} 

	public static GP_ROOM_GROUMAXPOKERPHINT decode(byte[] data) { 
		GP_ROOM_GROUMAXPOKERPHINT proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasPokerIndexList()) {
			bytes[0] = ByteBuffer.allocate(this.pokerIndexList.Count * 4 + 2);
			bytes[0].putShort((short) this.pokerIndexList.Count);
			for(int i=0, len=this.pokerIndexList.Count; i<len; i++) {
				bytes[0].putInt(this.pokerIndexList[i]);
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
		  
		if(this.hasPokerIndexList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.pokerIndexList.Add(buf.getInt());
			}
		}

	} 

	public int pokerIndexListCount() {
		return this.pokerIndexList.Count;
	}

	public bool hasPokerIndexList() {
		return (this.__flag[0] & 1) != 0;
	}

	public List<int> getPokerIndexListList() {
		return this.pokerIndexList;
	}

}
}

