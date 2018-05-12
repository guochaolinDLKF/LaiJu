//===================================================
//Author      : DRB
//CreateTime  ：12/5/2017 12:01:47 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.pdk {

public class SEAT_SETTING { 

	public const int CODE = 6; 

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

	private int _gold; 

	public int gold { 
		set { 
			if(!this.hasGold()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._gold = value;
		} 
		get { 
			return this._gold;
		} 
	} 

	private List<POKER_INFO> pokerInfo = new List<POKER_INFO>(); 

	public POKER_INFO getPokerInfo(int index) { 
			return this.pokerInfo[index];
	} 
	
	public void addPokerInfo(POKER_INFO value) { 
			if(!this.hasPokerInfo()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this.pokerInfo.Add(value);
	} 

	public static SEAT_SETTING newBuilder() { 
		return new SEAT_SETTING(); 
	} 

	public static SEAT_SETTING decode(byte[] data) { 
		SEAT_SETTING proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.pos);
			total += bytes[0].limit();
		}

		if(this.hasGold()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.gold);
			total += bytes[1].limit();
		}

		if(this.hasPokerInfo()) {
				int length = 0;
				for(int i=0, len=this.pokerInfo.Count; i<len; i++) {
					length += this.pokerInfo[i].encode().Length;
				}
				bytes[2] = ByteBuffer.allocate(this.pokerInfo.Count * 4 + length + 2);
				bytes[2].putShort((short) this.pokerInfo.Count);
				for(int i=0, len=this.pokerInfo.Count; i<len; i++) {
					byte[] _byte = this.pokerInfo[i].encode();
					bytes[2].putInt(_byte.Length);
					bytes[2].put(_byte);
				}
			total += bytes[2].limit();
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

		if(this.hasGold()) {
			this.gold = buf.getInt();
		}

		if(this.hasPokerInfo()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.pokerInfo.Add(POKER_INFO.decode(bytes));
			}
		}

	} 

	public bool hasPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasGold() {
		return (this.__flag[0] & 2) != 0;
	}

	public int pokerInfoCount() {
		return this.pokerInfo.Count;
	}

	public bool hasPokerInfo() {
		return (this.__flag[0] & 4) != 0;
	}

	public List<POKER_INFO> getPokerInfoList() {
		return this.pokerInfo;
	}

}
}

