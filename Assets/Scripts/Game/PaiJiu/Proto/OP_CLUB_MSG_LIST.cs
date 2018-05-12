//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:06 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class OP_CLUB_MSG_LIST { 

	public const int CODE = 99312; 

	private byte[] __flag = new byte[1]; 

	private List<OP_CLUB_MSG> msg = new List<OP_CLUB_MSG>(); 

	public OP_CLUB_MSG getMsg(int index) { 
			return this.msg[index];
	} 
	
	public void addMsg(OP_CLUB_MSG value) { 
			if(!this.hasMsg()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.msg.Add(value);
	} 

	private int _clubId; 

	public int clubId { 
		set { 
			if(!this.hasClubId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._clubId = value;
		} 
		get { 
			return this._clubId;
		} 
	} 

	public static OP_CLUB_MSG_LIST newBuilder() { 
		return new OP_CLUB_MSG_LIST(); 
	} 

	public static OP_CLUB_MSG_LIST decode(byte[] data) { 
		OP_CLUB_MSG_LIST proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasMsg()) {
				int length = 0;
				for(int i=0, len=this.msg.Count; i<len; i++) {
					length += this.msg[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.msg.Count * 4 + length + 2);
				bytes[0].putShort((short) this.msg.Count);
				for(int i=0, len=this.msg.Count; i<len; i++) {
					byte[] _byte = this.msg[i].encode();
					bytes[0].putInt(_byte.Length);
					bytes[0].put(_byte);
				}
			total += bytes[0].limit();
		}

		if(this.hasClubId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.clubId);
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
		  
		if(this.hasMsg()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.msg.Add(OP_CLUB_MSG.decode(bytes));
			}
		}

		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

	} 

	public int msgCount() {
		return this.msg.Count;
	}

	public bool hasMsg() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasClubId() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<OP_CLUB_MSG> getMsgList() {
		return this.msg;
	}

}
}

