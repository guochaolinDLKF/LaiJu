//===================================================
//Author      : DRB
//CreateTime  ：12/5/2017 12:01:57 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.pdk {

public class PDK_BEGIN { 

	public const int CODE = 901004; 

	private byte[] __flag = new byte[1]; 

	private List<SEAT_INFO> seatInfo = new List<SEAT_INFO>(); 

	public SEAT_INFO getSeatInfo(int index) { 
			return this.seatInfo[index];
	} 
	
	public void addSeatInfo(SEAT_INFO value) { 
			if(!this.hasSeatInfo()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.seatInfo.Add(value);
	} 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	public static PDK_BEGIN newBuilder() { 
		return new PDK_BEGIN(); 
	} 

	public static PDK_BEGIN decode(byte[] data) { 
		PDK_BEGIN proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasSeatInfo()) {
				int length = 0;
				for(int i=0, len=this.seatInfo.Count; i<len; i++) {
					length += this.seatInfo[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.seatInfo.Count * 4 + length + 2);
				bytes[0].putShort((short) this.seatInfo.Count);
				for(int i=0, len=this.seatInfo.Count; i<len; i++) {
					byte[] _byte = this.seatInfo[i].encode();
					bytes[0].putInt(_byte.Length);
					bytes[0].put(_byte);
				}
			total += bytes[0].limit();
		}

		if(this.hasPos()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.pos);
			total += bytes[1].limit();
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
		  
		if(this.hasSeatInfo()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.seatInfo.Add(SEAT_INFO.decode(bytes));
			}
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

	} 

	public int seatInfoCount() {
		return this.seatInfo.Count;
	}

	public bool hasSeatInfo() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<SEAT_INFO> getSeatInfoList() {
		return this.seatInfo;
	}

}
}

