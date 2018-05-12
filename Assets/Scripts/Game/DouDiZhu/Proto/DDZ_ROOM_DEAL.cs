using System.Collections.Generic;

namespace ddz.proto {

public class DDZ_ROOM_DEAL { 

	public const int CODE = 301004; 

	private byte[] __flag = new byte[16]; 

	private List<DDZ_SEAT> seat = new List<DDZ_SEAT>(); 

	public DDZ_SEAT getSeat(int index) { 
			return this.seat[index];
	} 
	
	public void addSeat(DDZ_SEAT value) { 
			if(!this.hasSeat()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.seat.Add(value);
	} 

	private int _remainPokerNum; 

	public int remainPokerNum { 
		set { 
			if(!this.hasRemainPokerNum()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._remainPokerNum = value;
		} 
		get { 
			return this._remainPokerNum;
		} 
	} 

	private int _loop; 

	public int loop { 
		set { 
			if(!this.hasLoop()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._loop = value;
		} 
		get { 
			return this._loop;
		} 
	} 

	public static DDZ_ROOM_DEAL newBuilder() { 
		return new DDZ_ROOM_DEAL(); 
	} 

	public static DDZ_ROOM_DEAL decode(byte[] data) { 
		DDZ_ROOM_DEAL proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

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

		if(this.hasRemainPokerNum()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.remainPokerNum);
			total += bytes[1].limit();
		}

		if(this.hasLoop()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.loop);
			total += bytes[2].limit();
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
			    this.seat.Add(DDZ_SEAT.decode(bytes));
			}
		}

		if(this.hasRemainPokerNum()) {
			this.remainPokerNum = buf.getInt();
		}

		if(this.hasLoop()) {
			this.loop = buf.getInt();
		}

	} 

	public int seatCount() {
		return this.seat.Count;
	}

	public bool hasSeat() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasRemainPokerNum() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasLoop() {
		return (this.__flag[0] & 4) != 0;
	}

	public List<DDZ_SEAT> getSeatList() {
		return this.seat;
	}

}
}

