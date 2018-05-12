//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:08 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class GP_ROOM_CUTPOKER_GET { 

	public const int CODE = 701028; 

	private byte[] __flag = new byte[1]; 

	private int _isCutPoker; 

	public int isCutPoker { 
		set { 
			if(!this.hasIsCutPoker()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._cutPokerIndex = value;
		} 
		get { 
			return this._cutPokerIndex;
		} 
	} 

	public static GP_ROOM_CUTPOKER_GET newBuilder() { 
		return new GP_ROOM_CUTPOKER_GET(); 
	} 

	public static GP_ROOM_CUTPOKER_GET decode(byte[] data) { 
		GP_ROOM_CUTPOKER_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasIsCutPoker()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.isCutPoker);
			total += bytes[0].limit();
		}

		if(this.hasCutPokerIndex()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.cutPokerIndex);
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
		  
		if(this.hasIsCutPoker()) {
			this.isCutPoker = buf.getInt();
		}

		if(this.hasCutPokerIndex()) {
			this.cutPokerIndex = buf.getInt();
		}

	} 

	public bool hasIsCutPoker() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasCutPokerIndex() {
		return (this.__flag[0] & 2) != 0;
	}

}
}

