//===================================================
//Author      : DRB
//CreateTime  ：11/6/2017 2:21:58 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace guandan.proto {

public class GD_SETTLE { 

	public const int CODE = 801010; 

	private byte[] __flag = new byte[1]; 

	private List<SEAT_SETTLE> seat_settle = new List<SEAT_SETTLE>(); 

	public SEAT_SETTLE getSeatSettle(int index) { 
			return this.seat_settle[index];
	} 
	
	public void addSeatSettle(SEAT_SETTLE value) { 
			if(!this.hasSeatSettle()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.seat_settle.Add(value);
	} 

	public static GD_SETTLE newBuilder() { 
		return new GD_SETTLE(); 
	} 

	public static GD_SETTLE decode(byte[] data) { 
		GD_SETTLE proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasSeatSettle()) {
				int length = 0;
				for(int i=0, len=this.seat_settle.Count; i<len; i++) {
					length += this.seat_settle[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.seat_settle.Count * 4 + length + 2);
				bytes[0].putShort((short) this.seat_settle.Count);
				for(int i=0, len=this.seat_settle.Count; i<len; i++) {
					byte[] _byte = this.seat_settle[i].encode();
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
		  
		if(this.hasSeatSettle()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.seat_settle.Add(SEAT_SETTLE.decode(bytes));
			}
		}

	} 

	public int seatSettleCount() {
		return this.seat_settle.Count;
	}

	public bool hasSeatSettle() {
		return (this.__flag[0] & 1) != 0;
	}

	public List<SEAT_SETTLE> getSeatSettleList() {
		return this.seat_settle;
	}

}
}

