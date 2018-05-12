//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:09 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class GP_ROOM_CUTPOKER { 

	public const int CODE = 701028; 

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

	private int _isCutPoker; 

	public int isCutPoker { 
		set { 
			if(!this.hasIsCutPoker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._isCutPoker = value;
		} 
		get { 
			return this._isCutPoker;
		} 
	} 

	private int _cutPokerIndex; 

	public int cutPokerIndex { 
		set { 
			if(!this.hasCutPokerIndex()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._cutPokerIndex = value;
		} 
		get { 
			return this._cutPokerIndex;
		} 
	} 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	public static GP_ROOM_CUTPOKER newBuilder() { 
		return new GP_ROOM_CUTPOKER(); 
	} 

	public static GP_ROOM_CUTPOKER decode(byte[] data) { 
		GP_ROOM_CUTPOKER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[4]; 

		int total = 0;
		if(this.hasPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pos);
			total += bytes[0].limit();
		}

		if(this.hasIsCutPoker()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.isCutPoker);
			total += bytes[1].limit();
		}

		if(this.hasCutPokerIndex()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.cutPokerIndex);
			total += bytes[2].limit();
		}

		if(this.hasUnixtime()) {
			bytes[3] = ByteBuffer.allocate(8);
			bytes[3].putLong(this.unixtime);
			total += bytes[3].limit();
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

		if(this.hasIsCutPoker()) {
			this.isCutPoker = buf.getInt();
		}

		if(this.hasCutPokerIndex()) {
			this.cutPokerIndex = buf.getInt();
		}

		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasIsCutPoker() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasCutPokerIndex() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasUnixtime() {
		return (this.__flag[0] & 8) != 0;
	}

}
}

