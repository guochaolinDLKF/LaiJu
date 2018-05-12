//===================================================
//Author      : DRB
//CreateTime  ：12/5/2017 12:01:51 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.pdk {

public class PDK_CREATE_ROOM_GET { 

	public const int CODE = 901001; 

	private byte[] __flag = new byte[1]; 

	private List<int> settingId = new List<int>(); 

	public int getSettingId(int index) { 
			return this.settingId[index];
	} 
	
	public void addSettingId(int value) { 
			if(!this.hasSettingId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.settingId.Add(value);
	} 

	public static PDK_CREATE_ROOM_GET newBuilder() { 
		return new PDK_CREATE_ROOM_GET(); 
	} 

	public static PDK_CREATE_ROOM_GET decode(byte[] data) { 
		PDK_CREATE_ROOM_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasSettingId()) {
			bytes[0] = ByteBuffer.allocate(this.settingId.Count * 4 + 2);
			bytes[0].putShort((short) this.settingId.Count);
			for(int i=0, len=this.settingId.Count; i<len; i++) {
				bytes[0].putInt(this.settingId[i]);
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
		  
		if(this.hasSettingId()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.settingId.Add(buf.getInt());
			}
		}

	} 

	public int settingIdCount() {
		return this.settingId.Count;
	}

	public bool hasSettingId() {
		return (this.__flag[0] & 1) != 0;
	}

	public List<int> getSettingIdList() {
		return this.settingId;
	}

}
}

