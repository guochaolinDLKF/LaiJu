//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:35 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class OP_SYS_ERROR { 

	public const int CODE = 99202; 

	private byte[] __flag = new byte[1]; 

	private int _code; 

	public int code { 
		set { 
			if(!this.hasCode()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._code = value;
		} 
		get { 
			return this._code;
		} 
	} 

	private string _msg; 

	public string msg { 
		set { 
			if(!this.hasMsg()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._msg = value;
		} 
		get { 
			return this._msg;
		} 
	} 

	public static OP_SYS_ERROR newBuilder() { 
		return new OP_SYS_ERROR(); 
	} 

	public static OP_SYS_ERROR decode(byte[] data) { 
		OP_SYS_ERROR proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasCode()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.code);
			total += bytes[0].limit();
		}

		if(this.hasMsg()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.msg);
			    short len = (short) _byte.Length;
			    bytes[1] = ByteBuffer.allocate(2 + len);
			    bytes[1].putShort(len);
				bytes[1].put(_byte);
			total += bytes[1].limit();
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
		  
		if(this.hasCode()) {
			this.code = buf.getInt();
		}

		if(this.hasMsg()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.msg = System.Text.Encoding.UTF8.GetString(bytes);
		}

	} 

	public bool hasCode() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasMsg() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

