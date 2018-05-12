//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:20 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_ROOM_INFORM_JETTON { 

	public const int CODE = 501009; 

	private byte[] __flag = new byte[1]; 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	private List<int> pos = new List<int>(); 

	public int getPos(int index) { 
			return this.pos[index];
	} 
	
	public void addPos(int value) { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this.pos.Add(value);
	} 

	public static PAIGOW_ROOM_INFORM_JETTON newBuilder() { 
		return new PAIGOW_ROOM_INFORM_JETTON(); 
	} 

	public static PAIGOW_ROOM_INFORM_JETTON decode(byte[] data) { 
		PAIGOW_ROOM_INFORM_JETTON proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasUnixtime()) {
			bytes[0] = ByteBuffer.allocate(8);
			bytes[0].putLong(this.unixtime);
			total += bytes[0].limit();
		}

		if(this.hasPos()) {
			bytes[1] = ByteBuffer.allocate(this.pos.Count * 4 + 2);
			bytes[1].putShort((short) this.pos.Count);
			for(int i=0, len=this.pos.Count; i<len; i++) {
				bytes[1].putInt(this.pos[i]);
			}
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
		  
		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
		}

		if(this.hasPos()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.pos.Add(buf.getInt());
			}
		}

	} 

	public bool hasUnixtime() {
		return (this.__flag[0] & 1) != 0;
	}

	public int posCount() {
		return this.pos.Count;
	}

	public bool hasPos() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<int> getPosList() {
		return this.pos;
	}

}
}

