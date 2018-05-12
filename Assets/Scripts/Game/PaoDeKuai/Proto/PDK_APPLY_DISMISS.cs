//===================================================
//Author      : DRB
//CreateTime  ：12/5/2017 12:02:03 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.pdk {

public class PDK_APPLY_DISMISS { 

	public const int CODE = 901012; 

	private byte[] __flag = new byte[1]; 

	private int _playerId; 

	public int playerId { 
		set { 
			if(!this.hasPlayerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._playerId = value;
		} 
		get { 
			return this._playerId;
		} 
	} 

	private long _dismissTime; 

	public long dismissTime { 
		set { 
			if(!this.hasDismissTime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._dismissTime = value;
		} 
		get { 
			return this._dismissTime;
		} 
	} 

	private long _dismissMaxTime; 

	public long dismissMaxTime { 
		set { 
			if(!this.hasDismissMaxTime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._dismissMaxTime = value;
		} 
		get { 
			return this._dismissMaxTime;
		} 
	} 

	public static PDK_APPLY_DISMISS newBuilder() { 
		return new PDK_APPLY_DISMISS(); 
	} 

	public static PDK_APPLY_DISMISS decode(byte[] data) { 
		PDK_APPLY_DISMISS proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasPlayerId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.playerId);
			total += bytes[0].limit();
		}

		if(this.hasDismissTime()) {
			bytes[1] = ByteBuffer.allocate(8);
			bytes[1].putLong(this.dismissTime);
			total += bytes[1].limit();
		}

		if(this.hasDismissMaxTime()) {
			bytes[2] = ByteBuffer.allocate(8);
			bytes[2].putLong(this.dismissMaxTime);
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
		  
		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

		if(this.hasDismissTime()) {
			this.dismissTime = buf.getLong();
		}

		if(this.hasDismissMaxTime()) {
			this.dismissMaxTime = buf.getLong();
		}

	} 

	public bool hasPlayerId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasDismissTime() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasDismissMaxTime() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

