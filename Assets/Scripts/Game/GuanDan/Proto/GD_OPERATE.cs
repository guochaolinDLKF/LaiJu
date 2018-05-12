//===================================================
//Author      : DRB
//CreateTime  ：11/6/2017 2:21:58 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace guandan.proto {

public class GD_OPERATE { 

	public const int CODE = 801008; 

	private byte[] __flag = new byte[1]; 

	private int _type; 

	public int type { 
		set { 
			if(!this.hasType()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._type = value;
		} 
		get { 
			return this._type;
		} 
	} 

	private List<POCKER_INFO> pocker_info = new List<POCKER_INFO>(); 

	public POCKER_INFO getPockerInfo(int index) { 
			return this.pocker_info[index];
	} 
	
	public void addPockerInfo(POCKER_INFO value) { 
			if(!this.hasPockerInfo()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this.pocker_info.Add(value);
	} 

	public static GD_OPERATE newBuilder() { 
		return new GD_OPERATE(); 
	} 

	public static GD_OPERATE decode(byte[] data) { 
		GD_OPERATE proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasType()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.type);
			total += bytes[0].limit();
		}

		if(this.hasPockerInfo()) {
				int length = 0;
				for(int i=0, len=this.pocker_info.Count; i<len; i++) {
					length += this.pocker_info[i].encode().Length;
				}
				bytes[1] = ByteBuffer.allocate(this.pocker_info.Count * 4 + length + 2);
				bytes[1].putShort((short) this.pocker_info.Count);
				for(int i=0, len=this.pocker_info.Count; i<len; i++) {
					byte[] _byte = this.pocker_info[i].encode();
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
		  
		if(this.hasType()) {
			this.type = buf.getInt();
		}

		if(this.hasPockerInfo()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.pocker_info.Add(POCKER_INFO.decode(bytes));
			}
		}

	} 

	public bool hasType() {
		return (this.__flag[0] & 1) != 0;
	}

	public int pockerInfoCount() {
		return this.pocker_info.Count;
	}

	public bool hasPockerInfo() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<POCKER_INFO> getPockerInfoList() {
		return this.pocker_info;
	}

}
}

