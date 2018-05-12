//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:19 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class GP_ROOM_BOTTOMPOUR { 

	public const int CODE = 701009; 

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

	private int _firstPour; 

	public int firstPour { 
		set { 
			if(!this.hasFirstPour()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._firstPour = value;
		} 
		get { 
			return this._firstPour;
		} 
	} 

	private int _secondPour; 

	public int secondPour { 
		set { 
			if(!this.hasSecondPour()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._secondPour = value;
		} 
		get { 
			return this._secondPour;
		} 
	} 

	private int _thirdPour; 

	public int thirdPour { 
		set { 
			if(!this.hasThirdPour()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._thirdPour = value;
		} 
		get { 
			return this._thirdPour;
		} 
	} 

	public static GP_ROOM_BOTTOMPOUR newBuilder() { 
		return new GP_ROOM_BOTTOMPOUR(); 
	} 

	public static GP_ROOM_BOTTOMPOUR decode(byte[] data) { 
		GP_ROOM_BOTTOMPOUR proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[4]; 

		int total = 0;
		if(this.hasPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pos);
			total += bytes[0].limit();
		}

		if(this.hasFirstPour()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.firstPour);
			total += bytes[1].limit();
		}

		if(this.hasSecondPour()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.secondPour);
			total += bytes[2].limit();
		}

		if(this.hasThirdPour()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.thirdPour);
			total += bytes[3].limit();
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

		if(this.hasFirstPour()) {
			this.firstPour = buf.getInt();
		}

		if(this.hasSecondPour()) {
			this.secondPour = buf.getInt();
		}

		if(this.hasThirdPour()) {
			this.thirdPour = buf.getInt();
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasFirstPour() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasSecondPour() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasThirdPour() {
		return (this.__flag[0] & 8) != 0;
	}

}
}

