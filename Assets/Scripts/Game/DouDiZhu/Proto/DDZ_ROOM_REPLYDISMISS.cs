using System.Collections.Generic;

namespace ddz.proto {

public class DDZ_ROOM_REPLYDISMISS { 

	public const int CODE = 301027; 

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

	private bool _isDismiss; 

	public bool isDismiss { 
		set { 
			if(!this.hasIsDismiss()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._isDismiss = value;
		} 
		get { 
			return this._isDismiss;
		} 
	} 

	public static DDZ_ROOM_REPLYDISMISS newBuilder() { 
		return new DDZ_ROOM_REPLYDISMISS(); 
	} 

	public static DDZ_ROOM_REPLYDISMISS decode(byte[] data) { 
		DDZ_ROOM_REPLYDISMISS proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasPlayerId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.playerId);
			total += bytes[0].limit();
		}

		if(this.hasIsDismiss()) {
			bytes[1] = ByteBuffer.allocate(1);
			if(this.isDismiss) {
				bytes[1].put((byte) 1);
			}else{
				bytes[1].put((byte) 0);
			}
			total += bytes[1].limit();
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

		if(this.hasIsDismiss()) {
			if(buf.get() == 1) {
				this.isDismiss = true;
			}else{
				this.isDismiss = false;
			}
		}

	} 

	public bool hasPlayerId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasIsDismiss() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

