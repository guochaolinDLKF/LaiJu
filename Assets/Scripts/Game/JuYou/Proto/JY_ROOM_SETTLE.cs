using System.Collections.Generic;

namespace proto.jy {

public class JY_ROOM_SETTLE { 

	public const int CODE = 601018; 

	private byte[] __flag = new byte[1]; 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	private bool _isWin; 

	public bool isWin { 
		set { 
			if(!this.hasIsWin()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._isWin = value;
		} 
		get { 
			return this._isWin;
		} 
	} 

	private int _gold; 

	public int gold { 
		set { 
			if(!this.hasGold()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._gold = value;
		} 
		get { 
			return this._gold;
		} 
	} 

	private int _baseScore; 

	public int baseScore { 
		set { 
			if(!this.hasBaseScore()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._baseScore = value;
		} 
		get { 
			return this._baseScore;
		} 
	} 

	private int _earnings; 

	public int earnings { 
		set { 
			if(!this.hasEarnings()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._earnings = value;
		} 
		get { 
			return this._earnings;
		} 
	} 

	public static JY_ROOM_SETTLE newBuilder() { 
		return new JY_ROOM_SETTLE(); 
	} 

	public static JY_ROOM_SETTLE decode(byte[] data) { 
		JY_ROOM_SETTLE proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[5]; 

		int total = 0;
		if(this.hasPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pos);
			total += bytes[0].limit();
		}

		if(this.hasIsWin()) {
			bytes[1] = ByteBuffer.allocate(1);
			if(this.isWin) {
				bytes[1].put((byte) 1);
			}else{
				bytes[1].put((byte) 0);
			}
			total += bytes[1].limit();
		}

		if(this.hasGold()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.gold);
			total += bytes[2].limit();
		}

		if(this.hasBaseScore()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.baseScore);
			total += bytes[3].limit();
		}

		if(this.hasEarnings()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.earnings);
			total += bytes[4].limit();
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
		  
		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasIsWin()) {
			if(buf.get() == 1) {
				this.isWin = true;
			}else{
				this.isWin = false;
			}
		}

		if(this.hasGold()) {
			this.gold = buf.getInt();
		}

		if(this.hasBaseScore()) {
			this.baseScore = buf.getInt();
		}

		if(this.hasEarnings()) {
			this.earnings = buf.getInt();
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasIsWin() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasGold() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasBaseScore() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasEarnings() {
		return (this.__flag[0] & 16) != 0;
	}

}
}

