//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:34 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ZJH_ROOM_COMPARE_POKER { 

	public const int CODE = 401039; 

	private byte[] __flag = new byte[1]; 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	private List<SEAT_COMMON> zjh_common = new List<SEAT_COMMON>(); 

	public SEAT_COMMON getZjhCommon(int index) { 
			return this.zjh_common[index];
	} 
	
	public void addZjhCommon(SEAT_COMMON value) { 
			if(!this.hasZjhCommon()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this.zjh_common.Add(value);
	} 

	private int _round; 

	public int round { 
		set { 
			if(!this.hasRound()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._round = value;
		} 
		get { 
			return this._round;
		} 
	} 

	public static ZJH_ROOM_COMPARE_POKER newBuilder() { 
		return new ZJH_ROOM_COMPARE_POKER(); 
	} 

	public static ZJH_ROOM_COMPARE_POKER decode(byte[] data) { 
		ZJH_ROOM_COMPARE_POKER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pos);
			total += bytes[0].limit();
		}

		if(this.hasZjhCommon()) {
				int length = 0;
				for(int i=0, len=this.zjh_common.Count; i<len; i++) {
					length += this.zjh_common[i].encode().Length;
				}
				bytes[1] = ByteBuffer.allocate(this.zjh_common.Count * 4 + length + 2);
				bytes[1].putShort((short) this.zjh_common.Count);
				for(int i=0, len=this.zjh_common.Count; i<len; i++) {
					byte[] _byte = this.zjh_common[i].encode();
					bytes[1].putInt(_byte.Length);
					bytes[1].put(_byte);
				}
			total += bytes[1].limit();
		}

		if(this.hasRound()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.round);
			total += bytes[2].limit();
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
		  
		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasZjhCommon()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.zjh_common.Add(SEAT_COMMON.decode(bytes));
			}
		}

		if(this.hasRound()) {
			this.round = buf.getInt();
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public int zjhCommonCount() {
		return this.zjh_common.Count;
	}

	public bool hasZjhCommon() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasRound() {
		return (this.__flag[0] & 4) != 0;
	}

	public List<SEAT_COMMON> getZjhCommonList() {
		return this.zjh_common;
	}

}
}

