//===================================================
//Author      : DRB
//CreateTime  ：10/27/2017 10:58:36 AM
//Description ：
//===================================================
using System.Collections.Generic;

namespace zjh.proto {

public class ZJH_ROOM_OPEN_LOOK_POCKER { 

	public const int CODE = 401045; 

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

	private List<POKER> zjh_poker = new List<POKER>(); 

	public POKER getZjhPoker(int index) { 
			return this.zjh_poker[index];
	} 
	
	public void addZjhPoker(POKER value) { 
			if(!this.hasZjhPoker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this.zjh_poker.Add(value);
	} 

	private int _round; 

	public int round { 
		set { 
			if(!this.hasRound()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._round = value;
		} 
		get { 
			return this._round;
		} 
	} 

	private ENUM_POKER_STATUS _pokerstatus; 

	public ENUM_POKER_STATUS pokerstatus { 
		set { 
			if(!this.hasPokerstatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._pokerstatus = value;
		} 
		get { 
			return this._pokerstatus;
		} 
	} 

	private int _type; 

	public int type { 
		set { 
			if(!this.hasType()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._type = value;
		} 
		get { 
			return this._type;
		} 
	} 

	public static ZJH_ROOM_OPEN_LOOK_POCKER newBuilder() { 
		return new ZJH_ROOM_OPEN_LOOK_POCKER(); 
	} 

	public static ZJH_ROOM_OPEN_LOOK_POCKER decode(byte[] data) { 
		ZJH_ROOM_OPEN_LOOK_POCKER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[5]; 

		int total = 0;
		if(this.hasPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pos);
			total += bytes[0].limit();
		}

		if(this.hasZjhPoker()) {
				int length = 0;
				for(int i=0, len=this.zjh_poker.Count; i<len; i++) {
					length += this.zjh_poker[i].encode().Length;
				}
				bytes[1] = ByteBuffer.allocate(this.zjh_poker.Count * 4 + length + 2);
				bytes[1].putShort((short) this.zjh_poker.Count);
				for(int i=0, len=this.zjh_poker.Count; i<len; i++) {
					byte[] _byte = this.zjh_poker[i].encode();
					bytes[1].putInt(_byte.Length);
					bytes[1].put(_byte);
				}
			total += bytes[1].limit();
		}

		if(this.hasRound()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.round);
			total += bytes[2].limit();
		}

		if(this.hasPokerstatus()) {
			bytes[3] = ByteBuffer.allocate(1);
			bytes[3].put((byte) this.pokerstatus);
			total += bytes[3].limit();
		}

		if(this.hasType()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.type);
			total += bytes[4].limit();
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

		if(this.hasZjhPoker()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.zjh_poker.Add(POKER.decode(bytes));
			}
		}

		if(this.hasRound()) {
			this.round = buf.getInt();
		}

		if(this.hasPokerstatus()) {
			this.pokerstatus = (ENUM_POKER_STATUS) buf.get();
		}

		if(this.hasType()) {
			this.type = buf.getInt();
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public int zjhPokerCount() {
		return this.zjh_poker.Count;
	}

	public bool hasZjhPoker() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasRound() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasPokerstatus() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasType() {
		return (this.__flag[0] & 16) != 0;
	}

	public List<POKER> getZjhPokerList() {
		return this.zjh_poker;
	}

}
}

