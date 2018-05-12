//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:26 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class PAIGOW_ROOM_DRAW { 

	public const int CODE = 501008; 

	private byte[] __flag = new byte[1]; 

	private List<PAIGOW_MAHJONG> paigow_mahjong = new List<PAIGOW_MAHJONG>(); 

	public PAIGOW_MAHJONG getPaigowMahjong(int index) { 
			return this.paigow_mahjong[index];
	} 
	
	public void addPaigowMahjong(PAIGOW_MAHJONG value) { 
			if(!this.hasPaigowMahjong()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.paigow_mahjong.Add(value);
	} 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	public static PAIGOW_ROOM_DRAW newBuilder() { 
		return new PAIGOW_ROOM_DRAW(); 
	} 

	public static PAIGOW_ROOM_DRAW decode(byte[] data) { 
		PAIGOW_ROOM_DRAW proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasPaigowMahjong()) {
				int length = 0;
				for(int i=0, len=this.paigow_mahjong.Count; i<len; i++) {
					length += this.paigow_mahjong[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.paigow_mahjong.Count * 4 + length + 2);
				bytes[0].putShort((short) this.paigow_mahjong.Count);
				for(int i=0, len=this.paigow_mahjong.Count; i<len; i++) {
					byte[] _byte = this.paigow_mahjong[i].encode();
					bytes[0].putInt(_byte.Length);
					bytes[0].put(_byte);
				}
			total += bytes[0].limit();
		}

		if(this.hasPos()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.pos);
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
		  
		if(this.hasPaigowMahjong()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.paigow_mahjong.Add(PAIGOW_MAHJONG.decode(bytes));
			}
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

	} 

	public int paigowMahjongCount() {
		return this.paigow_mahjong.Count;
	}

	public bool hasPaigowMahjong() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<PAIGOW_MAHJONG> getPaigowMahjongList() {
		return this.paigow_mahjong;
	}

}
}

