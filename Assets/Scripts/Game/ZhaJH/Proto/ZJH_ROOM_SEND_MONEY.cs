//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:33 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ZJH_ROOM_SEND_MONEY { 

	public const int CODE = 401058; 

	private byte[] __flag = new byte[1]; 

	private List<SEND_MONEY_SEAT> send_money_seat = new List<SEND_MONEY_SEAT>(); 

	public SEND_MONEY_SEAT getSendMoneySeat(int index) { 
			return this.send_money_seat[index];
	} 
	
	public void addSendMoneySeat(SEND_MONEY_SEAT value) { 
			if(!this.hasSendMoneySeat()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.send_money_seat.Add(value);
	} 

	public static ZJH_ROOM_SEND_MONEY newBuilder() { 
		return new ZJH_ROOM_SEND_MONEY(); 
	} 

	public static ZJH_ROOM_SEND_MONEY decode(byte[] data) { 
		ZJH_ROOM_SEND_MONEY proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasSendMoneySeat()) {
				int length = 0;
				for(int i=0, len=this.send_money_seat.Count; i<len; i++) {
					length += this.send_money_seat[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.send_money_seat.Count * 4 + length + 2);
				bytes[0].putShort((short) this.send_money_seat.Count);
				for(int i=0, len=this.send_money_seat.Count; i<len; i++) {
					byte[] _byte = this.send_money_seat[i].encode();
					bytes[0].putInt(_byte.Length);
					bytes[0].put(_byte);
				}
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
		  
		if(this.hasSendMoneySeat()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.send_money_seat.Add(SEND_MONEY_SEAT.decode(bytes));
			}
		}

	} 

	public int sendMoneySeatCount() {
		return this.send_money_seat.Count;
	}

	public bool hasSendMoneySeat() {
		return (this.__flag[0] & 1) != 0;
	}

	public List<SEND_MONEY_SEAT> getSendMoneySeatList() {
		return this.send_money_seat;
	}

}
}

