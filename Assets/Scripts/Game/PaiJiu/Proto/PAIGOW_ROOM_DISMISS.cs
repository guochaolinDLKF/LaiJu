//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:23 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_ROOM_DISMISS { 

	public const int CODE = 501016; 

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

	private bool _isDismiss; 

	public bool isDismiss { 
		set { 
			if(!this.hasIsDismiss()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._isDismiss = value;
		} 
		get { 
			return this._isDismiss;
		} 
	} 

	private bool _isSucceed; 

	public bool isSucceed { 
		set { 
			if(!this.hasIsSucceed()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._isSucceed = value;
		} 
		get { 
			return this._isSucceed;
		} 
	} 

	private ROOM_STATUS _room_status; 

	public ROOM_STATUS room_status { 
		set { 
			if(!this.hasRoomStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._room_status = value;
		} 
		get { 
			return this._room_status;
		} 
	} 

	public static PAIGOW_ROOM_DISMISS newBuilder() { 
		return new PAIGOW_ROOM_DISMISS(); 
	} 

	public static PAIGOW_ROOM_DISMISS decode(byte[] data) { 
		PAIGOW_ROOM_DISMISS proto = newBuilder();
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

		if(this.hasIsDismiss()) {
			bytes[1] = ByteBuffer.allocate(1);
			if(this.isDismiss) {
				bytes[1].put((byte) 1);
			}else{
				bytes[1].put((byte) 0);
			}
			total += bytes[1].limit();
		}

		if(this.hasIsSucceed()) {
			bytes[2] = ByteBuffer.allocate(1);
			if(this.isSucceed) {
				bytes[2].put((byte) 1);
			}else{
				bytes[2].put((byte) 0);
			}
			total += bytes[2].limit();
		}

		if(this.hasRoomStatus()) {
			bytes[3] = ByteBuffer.allocate(1);
			bytes[3].put((byte) this.room_status);
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

		if(this.hasIsDismiss()) {
			if(buf.get() == 1) {
				this.isDismiss = true;
			}else{
				this.isDismiss = false;
			}
		}

		if(this.hasIsSucceed()) {
			if(buf.get() == 1) {
				this.isSucceed = true;
			}else{
				this.isSucceed = false;
			}
		}

		if(this.hasRoomStatus()) {
			this.room_status = (ROOM_STATUS) buf.get();
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasIsDismiss() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasIsSucceed() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasRoomStatus() {
		return (this.__flag[0] & 8) != 0;
	}

}
}

