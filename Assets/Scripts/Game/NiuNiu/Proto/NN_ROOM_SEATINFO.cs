//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:32 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class NN_ROOM_SEATINFO { 

	public const int CODE = 2005; 

	private byte[] __flag = new byte[1]; 

	private List<NN_SEAT> nn_seat = new List<NN_SEAT>(); 

	public NN_SEAT getNnSeat(int index) { 
			return this.nn_seat[index];
	} 
	
	public void addNnSeat(NN_SEAT value) { 
			if(!this.hasNnSeat()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.nn_seat.Add(value);
	} 

	public static NN_ROOM_SEATINFO newBuilder() { 
		return new NN_ROOM_SEATINFO(); 
	} 

	public static NN_ROOM_SEATINFO decode(byte[] data) { 
		NN_ROOM_SEATINFO proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasNnSeat()) {
				int length = 0;
				for(int i=0, len=this.nn_seat.Count; i<len; i++) {
					length += this.nn_seat[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.nn_seat.Count * 4 + length + 2);
				bytes[0].putShort((short) this.nn_seat.Count);
				for(int i=0, len=this.nn_seat.Count; i<len; i++) {
					byte[] _byte = this.nn_seat[i].encode();
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
		  
		if(this.hasNnSeat()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.nn_seat.Add(NN_SEAT.decode(bytes));
			}
		}

	} 

	public int nnSeatCount() {
		return this.nn_seat.Count;
	}

	public bool hasNnSeat() {
		return (this.__flag[0] & 1) != 0;
	}

	public List<NN_SEAT> getNnSeatList() {
		return this.nn_seat;
	}

}
}

