//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:50 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ZJH_ROOM_APPLY_ENTER { 

	public const int CODE = 401050; 

	private byte[] __flag = new byte[1]; 

	private bool _agree_or_not; 

	public bool agree_or_not { 
		set { 
			if(!this.hasAgreeOrNot()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._agree_or_not = value;
		} 
		get { 
			return this._agree_or_not;
		} 
	} 

	private PLAYER _player; 

	public PLAYER player { 
		set { 
			if(!this.hasPlayer()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._player = value;
		} 
		get { 
			return this._player;
		} 
	} 

	public static ZJH_ROOM_APPLY_ENTER newBuilder() { 
		return new ZJH_ROOM_APPLY_ENTER(); 
	} 

	public static ZJH_ROOM_APPLY_ENTER decode(byte[] data) { 
		ZJH_ROOM_APPLY_ENTER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasAgreeOrNot()) {
			bytes[0] = ByteBuffer.allocate(1);
			if(this.agree_or_not) {
				bytes[0].put((byte) 1);
			}else{
				bytes[0].put((byte) 0);
			}
			total += bytes[0].limit();
		}

		if(this.hasPlayer()) {
			byte[] _byte = this.player.encode();
			int len = _byte.Length;
			bytes[1] = ByteBuffer.allocate(4 + len);
			bytes[1].putInt(len);
			bytes[1].put(_byte);
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
		  
		if(this.hasAgreeOrNot()) {
			if(buf.get() == 1) {
				this.agree_or_not = true;
			}else{
				this.agree_or_not = false;
			}
		}

		if(this.hasPlayer()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.player = PLAYER.decode(bytes);
		}

	} 

	public bool hasAgreeOrNot() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPlayer() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

