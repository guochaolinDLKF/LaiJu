//===================================================
//Author      : DRB
//CreateTime  ：12/5/2017 12:01:58 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.pdk {

public class PDK_OPERATE_GET { 

	public const int CODE = 901006; 

	private byte[] __flag = new byte[1]; 

	private List<POKER_INFO> pokerInfo = new List<POKER_INFO>(); 

	public POKER_INFO getPokerInfo(int index) { 
			return this.pokerInfo[index];
	} 
	
	public void addPokerInfo(POKER_INFO value) { 
			if(!this.hasPokerInfo()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.pokerInfo.Add(value);
	} 

	public static PDK_OPERATE_GET newBuilder() { 
		return new PDK_OPERATE_GET(); 
	} 

	public static PDK_OPERATE_GET decode(byte[] data) { 
		PDK_OPERATE_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasPokerInfo()) {
				int length = 0;
				for(int i=0, len=this.pokerInfo.Count; i<len; i++) {
					length += this.pokerInfo[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.pokerInfo.Count * 4 + length + 2);
				bytes[0].putShort((short) this.pokerInfo.Count);
				for(int i=0, len=this.pokerInfo.Count; i<len; i++) {
					byte[] _byte = this.pokerInfo[i].encode();
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
		  
		if(this.hasPokerInfo()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.pokerInfo.Add(POKER_INFO.decode(bytes));
			}
		}

	} 

	public int pokerInfoCount() {
		return this.pokerInfo.Count;
	}

	public bool hasPokerInfo() {
		return (this.__flag[0] & 1) != 0;
	}

	public List<POKER_INFO> getPokerInfoList() {
		return this.pokerInfo;
	}

}
}

