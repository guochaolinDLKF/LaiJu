//===================================================
//Author      : DRB
//CreateTime  ：12/5/2017 12:02:02 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.pdk {

public class OP_PLAYER_STATUS { 

	public const int CODE = 99104; 

	private byte[] __flag = new byte[1]; 

	private long _online; 

	public long online { 
		set { 
			if(!this.hasOnline()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._online = value;
		} 
		get { 
			return this._online;
		} 
	} 

	private int _playerId; 

	public int playerId { 
		set { 
			if(!this.hasPlayerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._playerId = value;
		} 
		get { 
			return this._playerId;
		} 
	} 

	public static OP_PLAYER_STATUS newBuilder() { 
		return new OP_PLAYER_STATUS(); 
	} 

	public static OP_PLAYER_STATUS decode(byte[] data) { 
		OP_PLAYER_STATUS proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasOnline()) {
			bytes[0] = ByteBuffer.allocate(8);
			bytes[0].putLong(this.online);
			total += bytes[0].limit();
		}

		if(this.hasPlayerId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.playerId);
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
		  
		if(this.hasOnline()) {
			this.online = buf.getLong();
		}

		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

	} 

	public bool hasOnline() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

