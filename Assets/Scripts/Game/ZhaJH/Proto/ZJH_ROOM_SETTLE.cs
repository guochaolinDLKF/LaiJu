//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:54 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ZJH_ROOM_SETTLE { 

	public const int CODE = 401047; 

	private byte[] __flag = new byte[1]; 

	private List<SEAT> zjh_seat = new List<SEAT>(); 

	public SEAT getZjhSeat(int index) { 
			return this.zjh_seat[index];
	} 
	
	public void addZjhSeat(SEAT value) { 
			if(!this.hasZjhSeat()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.zjh_seat.Add(value);
	} 

	public static ZJH_ROOM_SETTLE newBuilder() { 
		return new ZJH_ROOM_SETTLE(); 
	} 

	public static ZJH_ROOM_SETTLE decode(byte[] data) { 
		ZJH_ROOM_SETTLE proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasZjhSeat()) {
				int length = 0;
				for(int i=0, len=this.zjh_seat.Count; i<len; i++) {
					length += this.zjh_seat[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.zjh_seat.Count * 4 + length + 2);
				bytes[0].putShort((short) this.zjh_seat.Count);
				for(int i=0, len=this.zjh_seat.Count; i<len; i++) {
					byte[] _byte = this.zjh_seat[i].encode();
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
		  
		if(this.hasZjhSeat()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.zjh_seat.Add(SEAT.decode(bytes));
			}
		}

	} 

	public int zjhSeatCount() {
		return this.zjh_seat.Count;
	}

	public bool hasZjhSeat() {
		return (this.__flag[0] & 1) != 0;
	}

	public List<SEAT> getZjhSeatList() {
		return this.zjh_seat;
	}

}
}

