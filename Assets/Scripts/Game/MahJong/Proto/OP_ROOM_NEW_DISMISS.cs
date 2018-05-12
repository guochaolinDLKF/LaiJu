using System.Collections.Generic;

namespace proto.mahjong {

public class OP_ROOM_NEW_DISMISS { 

	public const int CODE = 101030; 

	private byte[] __flag = new byte[16]; 

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

	private bool _isSucceed; 

	public bool isSucceed { 
		set { 
			if(!this.hasIsSucceed()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._isSucceed = value;
		} 
		get { 
			return this._isSucceed;
		} 
	} 

	private ENUM_DISMISS_STATUS _dismiss; 

	public ENUM_DISMISS_STATUS dismiss { 
		set { 
			if(!this.hasDismiss()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._dismiss = value;
		} 
		get { 
			return this._dismiss;
		} 
	} 

	private long _dismissTime; 

	public long dismissTime { 
		set { 
			if(!this.hasDismissTime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._dismissMaxTime = value;
		} 
		get { 
			return this._dismissMaxTime;
		} 
	} 

	public static OP_ROOM_NEW_DISMISS newBuilder() { 
		return new OP_ROOM_NEW_DISMISS(); 
	} 

	public static OP_ROOM_NEW_DISMISS decode(byte[] data) { 
		OP_ROOM_NEW_DISMISS proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[5]; 

		int total = 0;
		if(this.hasPlayerId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.playerId);
			total += bytes[0].limit();
		}

		if(this.hasIsSucceed()) {
			bytes[1] = ByteBuffer.allocate(1);
			if(this.isSucceed) {
				bytes[1].put((byte) 1);
			}else{
				bytes[1].put((byte) 0);
			}
			total += bytes[1].limit();
		}

		if(this.hasDismiss()) {
			bytes[2] = ByteBuffer.allocate(1);
			bytes[2].put((byte) this.dismiss);
			total += bytes[2].limit();
		}

		if(this.hasDismissTime()) {
			bytes[3] = ByteBuffer.allocate(8);
			bytes[3].putLong(this.dismissTime);
			total += bytes[3].limit();
		}

		if(this.hasDismissMaxTime()) {
			bytes[4] = ByteBuffer.allocate(8);
			bytes[4].putLong(this.dismissMaxTime);
			total += bytes[4].limit();
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
		  
		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

		if(this.hasIsSucceed()) {
			if(buf.get() == 1) {
				this.isSucceed = true;
			}else{
				this.isSucceed = false;
			}
		}

		if(this.hasDismiss()) {
			this.dismiss = (ENUM_DISMISS_STATUS) buf.get();
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

	public bool hasIsSucceed() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasDismiss() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasDismissTime() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasDismissMaxTime() {
		return (this.__flag[0] & 16) != 0;
	}

}
}

