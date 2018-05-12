//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:53 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ZJH_ROOM_BREAK_POKER { 

	public const int CODE = 401059; 

	private byte[] __flag = new byte[1]; 

	private List<SEAT_COMMON> zjh_common = new List<SEAT_COMMON>(); 

	public SEAT_COMMON getZjhCommon(int index) { 
			return this.zjh_common[index];
	} 
	
	public void addZjhCommon(SEAT_COMMON value) { 
			if(!this.hasZjhCommon()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.zjh_common.Add(value);
	} 

	private int _winnert_pos; 

	public int winnert_pos { 
		set { 
			if(!this.hasWinnertPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._winnert_pos = value;
		} 
		get { 
			return this._winnert_pos;
		} 
	} 

	public static ZJH_ROOM_BREAK_POKER newBuilder() { 
		return new ZJH_ROOM_BREAK_POKER(); 
	} 

	public static ZJH_ROOM_BREAK_POKER decode(byte[] data) { 
		ZJH_ROOM_BREAK_POKER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasZjhCommon()) {
				int length = 0;
				for(int i=0, len=this.zjh_common.Count; i<len; i++) {
					length += this.zjh_common[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.zjh_common.Count * 4 + length + 2);
				bytes[0].putShort((short) this.zjh_common.Count);
				for(int i=0, len=this.zjh_common.Count; i<len; i++) {
					byte[] _byte = this.zjh_common[i].encode();
					bytes[0].putInt(_byte.Length);
					bytes[0].put(_byte);
				}
			total += bytes[0].limit();
		}

		if(this.hasWinnertPos()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.winnert_pos);
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
		  
		if(this.hasZjhCommon()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.zjh_common.Add(SEAT_COMMON.decode(bytes));
			}
		}

		if(this.hasWinnertPos()) {
			this.winnert_pos = buf.getInt();
		}

	} 

	public int zjhCommonCount() {
		return this.zjh_common.Count;
	}

	public bool hasZjhCommon() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasWinnertPos() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<SEAT_COMMON> getZjhCommonList() {
		return this.zjh_common;
	}

}
}

