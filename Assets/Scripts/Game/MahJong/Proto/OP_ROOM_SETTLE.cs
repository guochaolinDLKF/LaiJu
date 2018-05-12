using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_SETTLE { 

	public const int CODE = 101014; 

	private byte[] __flag = new byte[16]; 

	private List<OP_SEAT_FULL> seat = new List<OP_SEAT_FULL>(); 

	public OP_SEAT_FULL getSeat(int index) { 
			return this.seat[index];
	} 
	
	public void addSeat(OP_SEAT_FULL value) { 
			if(!this.hasSeat()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.seat.Add(value);
	} 

	private List<OP_POKER> probPoker = new List<OP_POKER>(); 

	public OP_POKER getProbPoker(int index) { 
			return this.probPoker[index];
	} 
	
	public void addProbPoker(OP_POKER value) { 
			if(!this.hasProbPoker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this.probPoker.Add(value);
	} 

	private OP_POKER _luckPoker; 

	public OP_POKER luckPoker { 
		set { 
			if(!this.hasLuckPoker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._luckPoker = value;
		} 
		get { 
			return this._luckPoker;
		} 
	} 

	private bool _isResult; 

	public bool isResult { 
		set { 
			if(!this.hasIsResult()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._isResult = value;
		} 
		get { 
			return this._isResult;
		} 
	} 

	public static OP_ROOM_SETTLE newBuilder() { 
		return new OP_ROOM_SETTLE(); 
	} 

	public static OP_ROOM_SETTLE decode(byte[] data) { 
		OP_ROOM_SETTLE proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[4]; 

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

		if(this.hasProbPoker()) {
				int length = 0;
				for(int i=0, len=this.probPoker.Count; i<len; i++) {
					length += this.probPoker[i].encode().Length;
				}
				bytes[1] = ByteBuffer.allocate(this.probPoker.Count * 4 + length + 2);
				bytes[1].putShort((short) this.probPoker.Count);
				for(int i=0, len=this.probPoker.Count; i<len; i++) {
					byte[] _byte = this.probPoker[i].encode();
					bytes[1].putInt(_byte.Length);
					bytes[1].put(_byte);
				}
			total += bytes[1].limit();
		}

		if(this.hasLuckPoker()) {
			byte[] _byte = this.luckPoker.encode();
			int len = _byte.Length;
			bytes[2] = ByteBuffer.allocate(4 + len);
			bytes[2].putInt(len);
			bytes[2].put(_byte);
			total += bytes[2].limit();
		}

		if(this.hasIsResult()) {
			bytes[3] = ByteBuffer.allocate(1);
			if(this.isResult) {
				bytes[3].put((byte) 1);
			}else{
				bytes[3].put((byte) 0);
			}
			total += bytes[3].limit();
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
			    this.seat.Add(OP_SEAT_FULL.decode(bytes));
			}
		}

		if(this.hasProbPoker()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.probPoker.Add(OP_POKER.decode(bytes));
			}
		}

		if(this.hasLuckPoker()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.luckPoker = OP_POKER.decode(bytes);
		}

		if(this.hasIsResult()) {
			if(buf.get() == 1) {
				this.isResult = true;
			}else{
				this.isResult = false;
			}
		}

	} 

	public int seatCount() {
		return this.seat.Count;
	}

	public bool hasSeat() {
		return (this.__flag[0] & 1) != 0;
	}

	public int probPokerCount() {
		return this.probPoker.Count;
	}

	public bool hasProbPoker() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasLuckPoker() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasIsResult() {
		return (this.__flag[0] & 8) != 0;
	}

	public List<OP_SEAT_FULL> getSeatList() {
		return this.seat;
	}

	public List<OP_POKER> getProbPokerList() {
		return this.probPoker;
	}

}
}

