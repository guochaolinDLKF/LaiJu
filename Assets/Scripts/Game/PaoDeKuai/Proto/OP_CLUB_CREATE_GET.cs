//===================================================
//Author      : DRB
//CreateTime  ：12/5/2017 12:01:50 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.pdk {

public class OP_CLUB_CREATE_GET { 

	public const int CODE = 99301; 

	private byte[] __flag = new byte[1]; 

	private string _name; 

	public string name { 
		set { 
			if(!this.hasName()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._name = value;
		} 
		get { 
			return this._name;
		} 
	} 

	private string _announce; 

	public string announce { 
		set { 
			if(!this.hasAnnounce()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._announce = value;
		} 
		get { 
			return this._announce;
		} 
	} 

	private string _avatar; 

	public string avatar { 
		set { 
			if(!this.hasAvatar()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._avatar = value;
		} 
		get { 
			return this._avatar;
		} 
	} 

	public static OP_CLUB_CREATE_GET newBuilder() { 
		return new OP_CLUB_CREATE_GET(); 
	} 

	public static OP_CLUB_CREATE_GET decode(byte[] data) { 
		OP_CLUB_CREATE_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasName()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.name);
			    short len = (short) _byte.Length;
			    bytes[0] = ByteBuffer.allocate(2 + len);
			    bytes[0].putShort(len);
				bytes[0].put(_byte);
			total += bytes[0].limit();
		}

		if(this.hasAnnounce()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.announce);
			    short len = (short) _byte.Length;
			    bytes[1] = ByteBuffer.allocate(2 + len);
			    bytes[1].putShort(len);
				bytes[1].put(_byte);
			total += bytes[1].limit();
		}

		if(this.hasAvatar()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.avatar);
			    short len = (short) _byte.Length;
			    bytes[2] = ByteBuffer.allocate(2 + len);
			    bytes[2].putShort(len);
				bytes[2].put(_byte);
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
		  
		if(this.hasName()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.name = System.Text.Encoding.UTF8.GetString(bytes);
		}

		if(this.hasAnnounce()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.announce = System.Text.Encoding.UTF8.GetString(bytes);
		}

		if(this.hasAvatar()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.avatar = System.Text.Encoding.UTF8.GetString(bytes);
		}

	} 

	public bool hasName() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasAnnounce() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasAvatar() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

