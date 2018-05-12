//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:12 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class GP_ROOM_CUTPAN { 

	public const int CODE = 701030; 

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

	private int _isCutPan; 

	public int isCutPan { 
		set { 
			if(!this.hasIsCutPan()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._isCutPan = value;
		} 
		get { 
			return this._isCutPan;
		} 
	} 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	public static GP_ROOM_CUTPAN newBuilder() { 
		return new GP_ROOM_CUTPAN(); 
	} 

	public static GP_ROOM_CUTPAN decode(byte[] data) { 
		GP_ROOM_CUTPAN proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pos);
			total += bytes[0].limit();
		}

		if(this.hasIsCutPan()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.isCutPan);
			total += bytes[1].limit();
		}

		if(this.hasUnixtime()) {
			bytes[2] = ByteBuffer.allocate(8);
			bytes[2].putLong(this.unixtime);
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
		  
		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasIsCutPan()) {
			this.isCutPan = buf.getInt();
		}

		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasIsCutPan() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasUnixtime() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

