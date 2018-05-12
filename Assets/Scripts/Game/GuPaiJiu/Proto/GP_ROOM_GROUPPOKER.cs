//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:10 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class GP_ROOM_GROUPPOKER { 

	public const int CODE = 701011; 

	private byte[] __flag = new byte[1]; 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
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

	private List<int> pokerIndexList = new List<int>(); 

	public int getPokerIndexList(int index) { 
			return this.pokerIndexList[index];
	} 
	
	public void addPokerIndexList(int value) { 
			if(!this.hasPokerIndexList()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this.pokerIndexList.Add(value);
	} 

	public static GP_ROOM_GROUPPOKER newBuilder() { 
		return new GP_ROOM_GROUPPOKER(); 
	} 

	public static GP_ROOM_GROUPPOKER decode(byte[] data) { 
		GP_ROOM_GROUPPOKER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasUnixtime()) {
			bytes[0] = ByteBuffer.allocate(8);
			bytes[0].putLong(this.unixtime);
			total += bytes[0].limit();
		}

		if(this.hasPos()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.pos);
			total += bytes[1].limit();
		}

		if(this.hasPokerIndexList()) {
			bytes[2] = ByteBuffer.allocate(this.pokerIndexList.Count * 4 + 2);
			bytes[2].putShort((short) this.pokerIndexList.Count);
			for(int i=0, len=this.pokerIndexList.Count; i<len; i++) {
				bytes[2].putInt(this.pokerIndexList[i]);
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
		  
		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasPokerIndexList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.pokerIndexList.Add(buf.getInt());
			}
		}

	} 

	public bool hasUnixtime() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPos() {
		return (this.__flag[0] & 2) != 0;
	}

	public int pokerIndexListCount() {
		return this.pokerIndexList.Count;
	}

	public bool hasPokerIndexList() {
		return (this.__flag[0] & 4) != 0;
	}

	public List<int> getPokerIndexListList() {
		return this.pokerIndexList;
	}

}
}

