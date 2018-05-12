using System.Collections.Generic;

namespace proto.jy {

public class JY_ROOM_GAMESTART { 

	public const int CODE = 601014; 

	private byte[] __flag = new byte[1]; 

	private List<JY_SEAT> seatList = new List<JY_SEAT>(); 

	public JY_SEAT getSeatList(int index) { 
			return this.seatList[index];
	} 
	
	public void addSeatList(JY_SEAT value) { 
			if(!this.hasSeatList()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.seatList.Add(value);
	} 

	private int _baseScore; 

	public int baseScore { 
		set { 
			if(!this.hasBaseScore()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._baseScore = value;
		} 
		get { 
			return this._baseScore;
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

	public static JY_ROOM_GAMESTART newBuilder() { 
		return new JY_ROOM_GAMESTART(); 
	} 

	public static JY_ROOM_GAMESTART decode(byte[] data) { 
		JY_ROOM_GAMESTART proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasSeatList()) {
				int length = 0;
				for(int i=0, len=this.seatList.Count; i<len; i++) {
					length += this.seatList[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.seatList.Count * 4 + length + 2);
				bytes[0].putShort((short) this.seatList.Count);
				for(int i=0, len=this.seatList.Count; i<len; i++) {
					byte[] _byte = this.seatList[i].encode();
					bytes[0].putInt(_byte.Length);
					bytes[0].put(_byte);
				}
			total += bytes[0].limit();
		}

		if(this.hasBaseScore()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.baseScore);
			total += bytes[1].limit();
		}

		if(this.hasLoop()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.loop);
			total += bytes[2].limit();
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
		  
		if(this.hasSeatList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.seatList.Add(JY_SEAT.decode(bytes));
			}
		}

		if(this.hasBaseScore()) {
			this.baseScore = buf.getInt();
		}

		if(this.hasLoop()) {
			this.loop = buf.getInt();
		}

	} 

	public int seatListCount() {
		return this.seatList.Count;
	}

	public bool hasSeatList() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasBaseScore() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasLoop() {
		return (this.__flag[0] & 4) != 0;
	}

	public List<JY_SEAT> getSeatListList() {
		return this.seatList;
	}

}
}

