using System.Collections.Generic;

namespace proto.jy {

public class JY_ROOM_JETTON { 

	public const int CODE = 601006; 

	private byte[] __flag = new byte[1]; 

	private int _pour; 

	public int pour { 
		set { 
			if(!this.hasPour()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._pour = value;
		} 
		get { 
			return this._pour;
		} 
	} 

	private JY_POKER _poker; 

	public JY_POKER poker { 
		set { 
			if(!this.hasPoker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._poker = value;
		} 
		get { 
			return this._poker;
		} 
	} 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	public static JY_ROOM_JETTON newBuilder() { 
		return new JY_ROOM_JETTON(); 
	} 

	public static JY_ROOM_JETTON decode(byte[] data) { 
		JY_ROOM_JETTON proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasPour()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pour);
			total += bytes[0].limit();
		}

		if(this.hasPoker()) {
			byte[] _byte = this.poker.encode();
			int len = _byte.Length;
			bytes[1] = ByteBuffer.allocate(4 + len);
			bytes[1].putInt(len);
			bytes[1].put(_byte);
			total += bytes[1].limit();
		}

		if(this.hasPos()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.pos);
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
		  
		if(this.hasPour()) {
			this.pour = buf.getInt();
		}

		if(this.hasPoker()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.poker = JY_POKER.decode(bytes);
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

	} 

	public bool hasPour() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPoker() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

