using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_RESULT { 

	public const int CODE = 101017; 

	private byte[] __flag = new byte[16]; 

	private int _maxLoop; 

	public int maxLoop { 
		set { 
			if(!this.hasMaxLoop()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._maxLoop = value;
		} 
		get { 
			return this._maxLoop;
		} 
	} 

	private int _loop; 

	public int loop { 
		set { 
			if(!this.hasLoop()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._loop = value;
		} 
		get { 
			return this._loop;
		} 
	} 

	private int _roomId; 

	public int roomId { 
		set { 
			if(!this.hasRoomId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._roomId = value;
		} 
		get { 
			return this._roomId;
		} 
	} 

	private List<OP_SEAT_RESULT> result = new List<OP_SEAT_RESULT>(); 

	public OP_SEAT_RESULT getResult(int index) { 
			return this.result[index];
	} 
	
	public void addResult(OP_SEAT_RESULT value) { 
			if(!this.hasResult()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this.result.Add(value);
	} 

	public static OP_ROOM_RESULT newBuilder() { 
		return new OP_ROOM_RESULT(); 
	} 

	public static OP_ROOM_RESULT decode(byte[] data) { 
		OP_ROOM_RESULT proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[4]; 

		int total = 0;
		if(this.hasMaxLoop()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.maxLoop);
			total += bytes[0].limit();
		}

		if(this.hasLoop()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.loop);
			total += bytes[1].limit();
		}

		if(this.hasRoomId()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.roomId);
			total += bytes[2].limit();
		}

		if(this.hasResult()) {
				int length = 0;
				for(int i=0, len=this.result.Count; i<len; i++) {
					length += this.result[i].encode().Length;
				}
				bytes[3] = ByteBuffer.allocate(this.result.Count * 4 + length + 2);
				bytes[3].putShort((short) this.result.Count);
				for(int i=0, len=this.result.Count; i<len; i++) {
					byte[] _byte = this.result[i].encode();
					bytes[3].putInt(_byte.Length);
					bytes[3].put(_byte);
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
		  
		if(this.hasMaxLoop()) {
			this.maxLoop = buf.getInt();
		}

		if(this.hasLoop()) {
			this.loop = buf.getInt();
		}

		if(this.hasRoomId()) {
			this.roomId = buf.getInt();
		}

		if(this.hasResult()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.result.Add(OP_SEAT_RESULT.decode(bytes));
			}
		}

	} 

	public bool hasMaxLoop() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasLoop() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasRoomId() {
		return (this.__flag[0] & 4) != 0;
	}

	public int resultCount() {
		return this.result.Count;
	}

	public bool hasResult() {
		return (this.__flag[0] & 8) != 0;
	}

	public List<OP_SEAT_RESULT> getResultList() {
		return this.result;
	}

}
}

