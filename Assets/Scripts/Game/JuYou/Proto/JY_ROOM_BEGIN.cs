using System.Collections.Generic;

namespace proto.jy {

public class JY_ROOM_BEGIN { 

	public const int CODE = 601005; 

	private byte[] __flag = new byte[1]; 

	private JY_SEAT _seat; 

	public JY_SEAT seat { 
		set { 
			if(!this.hasSeat()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._seat = value;
		} 
		get { 
			return this._seat;
		} 
	} 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	public static JY_ROOM_BEGIN newBuilder() { 
		return new JY_ROOM_BEGIN(); 
	} 

	public static JY_ROOM_BEGIN decode(byte[] data) { 
		JY_ROOM_BEGIN proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasSeat()) {
			byte[] _byte = this.seat.encode();
			int len = _byte.Length;
			bytes[0] = ByteBuffer.allocate(4 + len);
			bytes[0].putInt(len);
			bytes[0].put(_byte);
			total += bytes[0].limit();
		}

		if(this.hasUnixtime()) {
			bytes[1] = ByteBuffer.allocate(8);
			bytes[1].putLong(this.unixtime);
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
		  
		if(this.hasSeat()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.seat = JY_SEAT.decode(bytes);
		}

		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
		}

	} 

	public bool hasSeat() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasUnixtime() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

