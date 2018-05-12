//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 5:30:34 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.sss {

public class SSS_ANSWER_DIMISS { 

	public const int CODE = 1011011; 

	private byte[] __flag = new byte[1]; 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	private int _playerCount; 

	public int playerCount { 
		set { 
			if(!this.hasPlayerCount()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._playerCount = value;
		} 
		get { 
			return this._playerCount;
		} 
	} 

	public static SSS_ANSWER_DIMISS newBuilder() { 
		return new SSS_ANSWER_DIMISS(); 
	} 

	public static SSS_ANSWER_DIMISS decode(byte[] data) { 
		SSS_ANSWER_DIMISS proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pos);
			total += bytes[0].limit();
		}

		if(this.hasPlayerCount()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.playerCount);
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
		  
		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasPlayerCount()) {
			this.playerCount = buf.getInt();
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPlayerCount() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

