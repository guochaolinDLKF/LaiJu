//===================================================
//Author      : DRB
//CreateTime  ：1/16/2018 2:59:14 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.common {

public class OP_CLUB_QUICK_CREATE_GET { 

	public const int CODE = 99334; 

	private byte[] __flag = new byte[16]; 

	private int _clubId; 

	public int clubId { 
		set { 
			if(!this.hasClubId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._clubId = value;
		} 
		get { 
			return this._clubId;
		} 
	} 

	public static OP_CLUB_QUICK_CREATE_GET newBuilder() { 
		return new OP_CLUB_QUICK_CREATE_GET(); 
	} 

	public static OP_CLUB_QUICK_CREATE_GET decode(byte[] data) { 
		OP_CLUB_QUICK_CREATE_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasClubId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.clubId);
			total += bytes[0].limit();
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
		  
		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

	} 

	public bool hasClubId() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

