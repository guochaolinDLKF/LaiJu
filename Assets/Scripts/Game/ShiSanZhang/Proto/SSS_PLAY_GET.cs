//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 5:30:35 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.sss {

public class SSS_PLAY_GET { 

	public const int CODE = 1011006; 

	private byte[] __flag = new byte[1]; 

	private SEAT_INFO _seatInfo; 

	public SEAT_INFO seatInfo { 
		set { 
			if(!this.hasSeatInfo()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._seatInfo = value;
		} 
		get { 
			return this._seatInfo;
		} 
	} 

	public static SSS_PLAY_GET newBuilder() { 
		return new SSS_PLAY_GET(); 
	} 

	public static SSS_PLAY_GET decode(byte[] data) { 
		SSS_PLAY_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasSeatInfo()) {
			byte[] _byte = this.seatInfo.encode();
			int len = _byte.Length;
			bytes[0] = ByteBuffer.allocate(4 + len);
			bytes[0].putInt(len);
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
		  
		if(this.hasSeatInfo()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.seatInfo = SEAT_INFO.decode(bytes);
		}

	} 

	public bool hasSeatInfo() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

