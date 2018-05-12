//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 5:30:25 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.sss {

public class OP_CLUB_MSG_GET { 

	public const int CODE = 99316; 

	private byte[] __flag = new byte[1]; 

	private int _clubId; 

	public int clubId { 
		set { 
			if(!this.hasClubId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._clubId = value;
		} 
		get { 
			return this._clubId;
		} 
	} 

	private byte[] _content; 

	public byte[] content { 
		set { 
			if(!this.hasContent()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._content = value;
		} 
		get { 
			return this._content;
		} 
	} 

	private ENUM_PLAYER_MESSAGE _typeId; 

	public ENUM_PLAYER_MESSAGE typeId { 
		set { 
			if(!this.hasTypeId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._typeId = value;
		} 
		get { 
			return this._typeId;
		} 
	} 

	public static OP_CLUB_MSG_GET newBuilder() { 
		return new OP_CLUB_MSG_GET(); 
	} 

	public static OP_CLUB_MSG_GET decode(byte[] data) { 
		OP_CLUB_MSG_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasClubId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.clubId);
			total += bytes[0].limit();
		}

		if(this.hasContent()) {
			  byte[] _byte = this.content;
			    int len = _byte.Length;
			    bytes[1] = ByteBuffer.allocate(4 + len);
			    bytes[1].putInt(len);
				bytes[1].put(_byte);
			total += bytes[1].limit();
		}

		if(this.hasTypeId()) {
			bytes[2] = ByteBuffer.allocate(1);
			bytes[2].put((byte) this.typeId);
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
		  
		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

		if(this.hasContent()) {
			byte[] bytes = new byte[buf.getInt()];
			buf.get(ref bytes, 0, bytes.Length);
			this.content = bytes;
		}

		if(this.hasTypeId()) {
			this.typeId = (ENUM_PLAYER_MESSAGE) buf.get();
		}

	} 

	public bool hasClubId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasContent() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasTypeId() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

