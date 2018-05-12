//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:16 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class OP_SYS_LOG_GET { 

	public const int CODE = 99203; 

	private byte[] __flag = new byte[1]; 

	private string _msg; 

	public string msg { 
		set { 
			if(!this.hasMsg()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._msg = value;
		} 
		get { 
			return this._msg;
		} 
	} 

	public static OP_SYS_LOG_GET newBuilder() { 
		return new OP_SYS_LOG_GET(); 
	} 

	public static OP_SYS_LOG_GET decode(byte[] data) { 
		OP_SYS_LOG_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasMsg()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.msg);
			    short len = (short) _byte.Length;
			    bytes[0] = ByteBuffer.allocate(2 + len);
			    bytes[0].putShort(len);
				bytes[0].put(_byte);
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
		  
		if(this.hasMsg()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.msg = System.Text.Encoding.UTF8.GetString(bytes);
		}

	} 

	public bool hasMsg() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

