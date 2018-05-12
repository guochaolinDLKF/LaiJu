//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:26 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class NN_ROOM_BEGIN { 

	public const int CODE = 201006; 

	private byte[] __flag = new byte[1]; 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	private List<NN_SEAT> nn_seat = new List<NN_SEAT>(); 

	public NN_SEAT getNnSeat(int index) { 
			return this.nn_seat[index];
	} 
	
	public void addNnSeat(NN_SEAT value) { 
			if(!this.hasNnSeat()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this.nn_seat.Add(value);
	} 

	public static NN_ROOM_BEGIN newBuilder() { 
		return new NN_ROOM_BEGIN(); 
	} 

	public static NN_ROOM_BEGIN decode(byte[] data) { 
		NN_ROOM_BEGIN proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasUnixtime()) {
			bytes[0] = ByteBuffer.allocate(8);
			bytes[0].putLong(this.unixtime);
			total += bytes[0].limit();
		}

		if(this.hasNnSeat()) {
				int length = 0;
				for(int i=0, len=this.nn_seat.Count; i<len; i++) {
					length += this.nn_seat[i].encode().Length;
				}
				bytes[1] = ByteBuffer.allocate(this.nn_seat.Count * 4 + length + 2);
				bytes[1].putShort((short) this.nn_seat.Count);
				for(int i=0, len=this.nn_seat.Count; i<len; i++) {
					byte[] _byte = this.nn_seat[i].encode();
					bytes[1].putInt(_byte.Length);
					bytes[1].put(_byte);
				}
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
		  
		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
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

	public bool hasUnixtime() {
		return (this.__flag[0] & 1) != 0;
	}

	public int nnSeatCount() {
		return this.nn_seat.Count;
	}

	public bool hasNnSeat() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<NN_SEAT> getNnSeatList() {
		return this.nn_seat;
	}

}
}

