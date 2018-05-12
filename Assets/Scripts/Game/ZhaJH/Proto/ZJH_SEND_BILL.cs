//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:52 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ZJH_SEND_BILL { 

	public const int CODE = 401056; 

	private byte[] __flag = new byte[1]; 

	private SEAT_BILL _seat_bill; 

	public SEAT_BILL seat_bill { 
		set { 
			if(!this.hasSeatBill()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._seat_bill = value;
		} 
		get { 
			return this._seat_bill;
		} 
	} 

	public static ZJH_SEND_BILL newBuilder() { 
		return new ZJH_SEND_BILL(); 
	} 

	public static ZJH_SEND_BILL decode(byte[] data) { 
		ZJH_SEND_BILL proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasSeatBill()) {
			byte[] _byte = this.seat_bill.encode();
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
		  
		if(this.hasSeatBill()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.seat_bill = SEAT_BILL.decode(bytes);
		}

	} 

	public bool hasSeatBill() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

