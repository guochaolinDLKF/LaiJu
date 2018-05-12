//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:30 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class NN_ROOM_DRAW { 

	public const int CODE = 201014; 

	private byte[] __flag = new byte[1]; 

	private List<NN_POKER> nn_poker = new List<NN_POKER>(); 

	public NN_POKER getNnPoker(int index) { 
			return this.nn_poker[index];
	} 
	
	public void addNnPoker(NN_POKER value) { 
			if(!this.hasNnPoker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.nn_poker.Add(value);
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

	public static NN_ROOM_DRAW newBuilder() { 
		return new NN_ROOM_DRAW(); 
	} 

	public static NN_ROOM_DRAW decode(byte[] data) { 
		NN_ROOM_DRAW proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasNnPoker()) {
				int length = 0;
				for(int i=0, len=this.nn_poker.Count; i<len; i++) {
					length += this.nn_poker[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.nn_poker.Count * 4 + length + 2);
				bytes[0].putShort((short) this.nn_poker.Count);
				for(int i=0, len=this.nn_poker.Count; i<len; i++) {
					byte[] _byte = this.nn_poker[i].encode();
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
		  
		if(this.hasNnPoker()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.nn_poker.Add(NN_POKER.decode(bytes));
			}
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

	} 

	public int nnPokerCount() {
		return this.nn_poker.Count;
	}

	public bool hasNnPoker() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<NN_POKER> getNnPokerList() {
		return this.nn_poker;
	}

}
}

