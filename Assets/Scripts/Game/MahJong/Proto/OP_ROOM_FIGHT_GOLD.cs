using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_FIGHT_GOLD { 

	public const int CODE = 101028; 

	private byte[] __flag = new byte[16]; 

	private List<OP_SEAT_GOLD> seat = new List<OP_SEAT_GOLD>(); 

	public OP_SEAT_GOLD getSeat(int index) { 
			return this.seat[index];
	} 
	
	public void addSeat(OP_SEAT_GOLD value) { 
			if(!this.hasSeat()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.seat.Add(value);
	} 

	public static OP_ROOM_FIGHT_GOLD newBuilder() { 
		return new OP_ROOM_FIGHT_GOLD(); 
	} 

	public static OP_ROOM_FIGHT_GOLD decode(byte[] data) { 
		OP_ROOM_FIGHT_GOLD proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasSeat()) {
				int length = 0;
				for(int i=0, len=this.seat.Count; i<len; i++) {
					length += this.seat[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.seat.Count * 4 + length + 2);
				bytes[0].putShort((short) this.seat.Count);
				for(int i=0, len=this.seat.Count; i<len; i++) {
					byte[] _byte = this.seat[i].encode();
					bytes[0].putInt(_byte.Length);
					bytes[0].put(_byte);
				}
			total += bytes[0].limit();
		}

	
		ByteBuffer buf = ByteBuffer.allocate(16 + total);
	
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
		  
		if(this.hasSeat()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.seat.Add(OP_SEAT_GOLD.decode(bytes));
			}
		}

	} 

	public int seatCount() {
		return this.seat.Count;
	}

	public bool hasSeat() {
		return (this.__flag[0] & 1) != 0;
	}

	public List<OP_SEAT_GOLD> getSeatList() {
		return this.seat;
	}

}
}

