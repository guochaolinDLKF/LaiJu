//===================================================
//Author      : DRB
//CreateTime  ：11/6/2017 2:21:49 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace guandan.proto {

public class GD_ANSWER_DISMISS { 

	public const int CODE = 801014; 

	private byte[] __flag = new byte[1]; 

	private int _playerTotal; 

	public int playerTotal { 
		set { 
			if(!this.hasPlayerTotal()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._playerTotal = value;
		} 
		get { 
			return this._playerTotal;
		} 
	} 

	private DISMISS_STATUS _dismiss_status; 

	public DISMISS_STATUS dismiss_status { 
		set { 
			if(!this.hasDismissStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._dismiss_status = value;
		} 
		get { 
			return this._dismiss_status;
		} 
	} 

	public static GD_ANSWER_DISMISS newBuilder() { 
		return new GD_ANSWER_DISMISS(); 
	} 

	public static GD_ANSWER_DISMISS decode(byte[] data) { 
		GD_ANSWER_DISMISS proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasPlayerTotal()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.playerTotal);
			total += bytes[0].limit();
		}

		if(this.hasDismissStatus()) {
			bytes[1] = ByteBuffer.allocate(1);
			bytes[1].put((byte) this.dismiss_status);
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
		  
		if(this.hasPlayerTotal()) {
			this.playerTotal = buf.getInt();
		}

		if(this.hasDismissStatus()) {
			this.dismiss_status = (DISMISS_STATUS) buf.get();
		}

	} 

	public bool hasPlayerTotal() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasDismissStatus() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

