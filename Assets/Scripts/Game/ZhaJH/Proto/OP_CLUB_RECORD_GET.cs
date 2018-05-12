//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:37 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class OP_CLUB_RECORD_GET { 

	public const int CODE = 99328; 

	private byte[] __flag = new byte[1]; 

	private int _recordId; 

	public int recordId { 
		set { 
			if(!this.hasRecordId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._recordId = value;
		} 
		get { 
			return this._recordId;
		} 
	} 

	public static OP_CLUB_RECORD_GET newBuilder() { 
		return new OP_CLUB_RECORD_GET(); 
	} 

	public static OP_CLUB_RECORD_GET decode(byte[] data) { 
		OP_CLUB_RECORD_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasRecordId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.recordId);
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
		  
		if(this.hasRecordId()) {
			this.recordId = buf.getInt();
		}

	} 

	public bool hasRecordId() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

