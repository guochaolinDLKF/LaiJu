using System.Collections.Generic;

namespace proto.mahjong {

public class OP_MATCH_APPLY_GET { 

	public const int CODE = 102001; 

	private byte[] __flag = new byte[16]; 

	private int _settingId; 

	public int settingId { 
		set { 
			if(!this.hasSettingId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._settingId = value;
		} 
		get { 
			return this._settingId;
		} 
	} 

	public static OP_MATCH_APPLY_GET newBuilder() { 
		return new OP_MATCH_APPLY_GET(); 
	} 

	public static OP_MATCH_APPLY_GET decode(byte[] data) { 
		OP_MATCH_APPLY_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[1]; 

		int total = 0;
		if(this.hasSettingId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.settingId);
			total += bytes[0].limit();
		}

	
		ByteBuffer buf = ByteBuffer.allocate(16 + total);
	
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
			this.settingId = buf.getInt();
		}

	} 

	public bool hasSettingId() {
		return (this.__flag[0] & 1) != 0;
	}

}
}

