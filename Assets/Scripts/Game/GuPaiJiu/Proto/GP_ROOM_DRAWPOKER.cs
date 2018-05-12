//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:09 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class GP_ROOM_DRAWPOKER { 

	public const int CODE = 701031; 

	private byte[] __flag = new byte[1]; 

	private List<GP_POKER> drawPokerList = new List<GP_POKER>(); 

	public GP_POKER getDrawPokerList(int index) { 
			return this.drawPokerList[index];
	} 
	
	public void addDrawPokerList(GP_POKER value) { 
			if(!this.hasDrawPokerList()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.drawPokerList.Add(value);
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

	private int _index; 

	public int index { 
		set { 
			if(!this.hasIndex()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._index = value;
		} 
		get { 
			return this._index;
		} 
	} 

	public static GP_ROOM_DRAWPOKER newBuilder() { 
		return new GP_ROOM_DRAWPOKER(); 
	} 

	public static GP_ROOM_DRAWPOKER decode(byte[] data) { 
		GP_ROOM_DRAWPOKER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasDrawPokerList()) {
				int length = 0;
				for(int i=0, len=this.drawPokerList.Count; i<len; i++) {
					length += this.drawPokerList[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.drawPokerList.Count * 4 + length + 2);
				bytes[0].putShort((short) this.drawPokerList.Count);
				for(int i=0, len=this.drawPokerList.Count; i<len; i++) {
					byte[] _byte = this.drawPokerList[i].encode();
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

		if(this.hasIndex()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.index);
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
		  
		if(this.hasDrawPokerList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.drawPokerList.Add(GP_POKER.decode(bytes));
			}
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasIndex()) {
			this.index = buf.getInt();
		}

	} 

	public int drawPokerListCount() {
		return this.drawPokerList.Count;
	}

	public bool hasDrawPokerList() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasIndex() {
		return (this.__flag[0] & 4) != 0;
	}

	public List<GP_POKER> getDrawPokerListList() {
		return this.drawPokerList;
	}

}
}

