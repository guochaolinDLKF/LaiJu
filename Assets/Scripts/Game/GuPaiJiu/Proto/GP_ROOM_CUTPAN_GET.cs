//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:12 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class GP_ROOM_CUTPAN_GET { 

	public const int CODE = 701030; 

	private byte[] __flag = new byte[1]; 

	private int _isCutPan; 

	public int isCutPan { 
		set { 
			if(!this.hasIsCutPan()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._isCutPan = value;
		} 
		get { 
			return this._isCutPan;
		} 
	} 

	public static GP_ROOM_CUTPAN_GET newBuilder() { 
		return new GP_ROOM_CUTPAN_GET(); 
	} 

	public static GP_ROOM_CUTPAN_GET decode(byte[] data) { 
		GP_ROOM_CUTPAN_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasIsCutPan()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.isCutPan);
			total += bytes[0].limit();
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
		  
		if(this.hasIsCutPan()) {
			this.isCutPan = buf.getInt();
		}

	} 

	public bool hasIsCutPan() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

