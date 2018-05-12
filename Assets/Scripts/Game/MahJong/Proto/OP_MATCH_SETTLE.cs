using System.Collections.Generic;

namespace proto.mahjong {

public class OP_MATCH_SETTLE { 

	public const int CODE = 102005; 

	private byte[] __flag = new byte[16]; 

	private int _number; 

	public int number { 
		set { 
			if(!this.hasNumber()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._number = value;
		} 
		get { 
			return this._number;
		} 
	} 

	private bool _isOut; 

	public bool isOut { 
		set { 
			if(!this.hasIsOut()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._isOut = value;
		} 
		get { 
			return this._isOut;
		} 
	} 

	private string _rewardDesc; 

	public string rewardDesc { 
		set { 
			if(!this.hasRewardDesc()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._rewardDesc = value;
		} 
		get { 
			return this._rewardDesc;
		} 
	} 

	public static OP_MATCH_SETTLE newBuilder() { 
		return new OP_MATCH_SETTLE(); 
	} 

	public static OP_MATCH_SETTLE decode(byte[] data) { 
		OP_MATCH_SETTLE proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasNumber()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.number);
			total += bytes[0].limit();
		}

		if(this.hasIsOut()) {
			bytes[1] = ByteBuffer.allocate(1);
			if(this.isOut) {
				bytes[1].put((byte) 1);
			}else{
				bytes[1].put((byte) 0);
			}
			total += bytes[1].limit();
		}

		if(this.hasRewardDesc()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.rewardDesc);
			    short len = (short) _byte.Length;
			    bytes[2] = ByteBuffer.allocate(2 + len);
			    bytes[2].putShort(len);
				bytes[2].put(_byte);
			total += bytes[2].limit();
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
		  
		if(this.hasNumber()) {
			this.number = buf.getInt();
		}

		if(this.hasIsOut()) {
			if(buf.get() == 1) {
				this.isOut = true;
			}else{
				this.isOut = false;
			}
		}

		if(this.hasRewardDesc()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.rewardDesc = System.Text.Encoding.UTF8.GetString(bytes);
		}

	} 

	public bool hasNumber() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasIsOut() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasRewardDesc() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

