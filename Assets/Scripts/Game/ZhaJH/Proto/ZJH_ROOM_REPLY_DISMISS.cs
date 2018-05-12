//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:49 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ZJH_ROOM_REPLY_DISMISS { 

	public const int CODE = 401035; 

	private byte[] __flag = new byte[1]; 

	private ENUM_ROOMRESULT _zjh_enum_roomresult; 

	public ENUM_ROOMRESULT zjh_enum_roomresult { 
		set { 
			if(!this.hasZjhEnumRoomresult()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._zjh_enum_roomresult = value;
		} 
		get { 
			return this._zjh_enum_roomresult;
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

	public static ZJH_ROOM_REPLY_DISMISS newBuilder() { 
		return new ZJH_ROOM_REPLY_DISMISS(); 
	} 

	public static ZJH_ROOM_REPLY_DISMISS decode(byte[] data) { 
		ZJH_ROOM_REPLY_DISMISS proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasZjhEnumRoomresult()) {
			bytes[0] = ByteBuffer.allocate(1);
			bytes[0].put((byte) this.zjh_enum_roomresult);
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
		  
		if(this.hasZjhEnumRoomresult()) {
			this.zjh_enum_roomresult = (ENUM_ROOMRESULT) buf.get();
		}

		if(this.hasZjhCommon()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.zjh_common.Add(SEAT_COMMON.decode(bytes));
			}
		}

	} 

	public bool hasZjhEnumRoomresult() {
		return (this.__flag[0] & 1) != 0;
	}

	public int zjhCommonCount() {
		return this.zjh_common.Count;
	}

	public bool hasZjhCommon() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<SEAT_COMMON> getZjhCommonList() {
		return this.zjh_common;
	}

}
}

