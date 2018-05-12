//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:20 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class GP_ROOM_VALIDDEAL { 

	public const int CODE = 701019; 

	private byte[] __flag = new byte[1]; 

	private List<GP_SEAT> seatList = new List<GP_SEAT>(); 

	public GP_SEAT getSeatList(int index) { 
			return this.seatList[index];
	} 
	
	public void addSeatList(GP_SEAT value) { 
			if(!this.hasSeatList()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.seatList.Add(value);
	} 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	public static GP_ROOM_VALIDDEAL newBuilder() { 
		return new GP_ROOM_VALIDDEAL(); 
	} 

	public static GP_ROOM_VALIDDEAL decode(byte[] data) { 
		GP_ROOM_VALIDDEAL proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasSeatList()) {
				int length = 0;
				for(int i=0, len=this.seatList.Count; i<len; i++) {
					length += this.seatList[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.seatList.Count * 4 + length + 2);
				bytes[0].putShort((short) this.seatList.Count);
				for(int i=0, len=this.seatList.Count; i<len; i++) {
					byte[] _byte = this.seatList[i].encode();
					bytes[0].putInt(_byte.Length);
					bytes[0].put(_byte);
				}
			total += bytes[0].limit();
		}

		if(this.hasUnixtime()) {
			bytes[1] = ByteBuffer.allocate(8);
			bytes[1].putLong(this.unixtime);
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
		  
		if(this.hasSeatList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.seatList.Add(GP_SEAT.decode(bytes));
			}
		}

		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
		}

	} 

	public int seatListCount() {
		return this.seatList.Count;
	}

	public bool hasSeatList() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasUnixtime() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<GP_SEAT> getSeatListList() {
		return this.seatList;
	}

}
}

