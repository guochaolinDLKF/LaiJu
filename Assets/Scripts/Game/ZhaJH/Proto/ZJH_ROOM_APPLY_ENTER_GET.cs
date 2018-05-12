//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:52 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ZJH_ROOM_APPLY_ENTER_GET { 

	public const int CODE = 401050; 

	private byte[] __flag = new byte[1]; 

	private PLAYER _player; 

	public PLAYER player { 
		set { 
			if(!this.hasPlayer()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._player = value;
		} 
		get { 
			return this._player;
		} 
	} 

	private bool _agree_or_not; 

	public bool agree_or_not { 
		set { 
			if(!this.hasAgreeOrNot()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._agree_or_not = value;
		} 
		get { 
			return this._agree_or_not;
		} 
	} 

	public static ZJH_ROOM_APPLY_ENTER_GET newBuilder() { 
		return new ZJH_ROOM_APPLY_ENTER_GET(); 
	} 

	public static ZJH_ROOM_APPLY_ENTER_GET decode(byte[] data) { 
		ZJH_ROOM_APPLY_ENTER_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasPlayer()) {
			byte[] _byte = this.player.encode();
			int len = _byte.Length;
			bytes[0] = ByteBuffer.allocate(4 + len);
			bytes[0].putInt(len);
			bytes[0].put(_byte);
			total += bytes[0].limit();
		}

		if(this.hasAgreeOrNot()) {
			bytes[1] = ByteBuffer.allocate(1);
			if(this.agree_or_not) {
				bytes[1].put((byte) 1);
			}else{
				bytes[1].put((byte) 0);
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
		  
		if(this.hasPlayer()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.player = PLAYER.decode(bytes);
		}

		if(this.hasAgreeOrNot()) {
			if(buf.get() == 1) {
				this.agree_or_not = true;
			}else{
				this.agree_or_not = false;
			}
		}

	} 

	public bool hasPlayer() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasAgreeOrNot() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

