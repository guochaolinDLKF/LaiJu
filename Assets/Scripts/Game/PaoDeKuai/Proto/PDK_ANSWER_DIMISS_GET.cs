//===================================================
//Author      : DRB
//CreateTime  ：12/5/2017 12:02:03 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.pdk {

public class PDK_ANSWER_DIMISS_GET { 

	public const int CODE = 901015; 

	private byte[] __flag = new byte[1]; 

	private DISMISS_STATUS _dismiss_status; 

	public DISMISS_STATUS dismiss_status { 
		set { 
			if(!this.hasDismissStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._dismiss_status = value;
		} 
		get { 
			return this._dismiss_status;
		} 
	} 

	public static PDK_ANSWER_DIMISS_GET newBuilder() { 
		return new PDK_ANSWER_DIMISS_GET(); 
	} 

	public static PDK_ANSWER_DIMISS_GET decode(byte[] data) { 
		PDK_ANSWER_DIMISS_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasDismissStatus()) {
			bytes[0] = ByteBuffer.allocate(1);
			bytes[0].put((byte) this.dismiss_status);
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
		  
		if(this.hasDismissStatus()) {
			this.dismiss_status = (DISMISS_STATUS) buf.get();
		}

	} 

	public bool hasDismissStatus() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

