//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:15 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class OP_PLAYER_CONNECT_GET { 

	public const int CODE = 99101; 

	private byte[] __flag = new byte[1]; 

	private int _passportId; 

	public int passportId { 
		set { 
			if(!this.hasPassportId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._passportId = value;
		} 
		get { 
			return this._passportId;
		} 
	} 

	private string _token; 

	public string token { 
		set { 
			if(!this.hasToken()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._token = value;
		} 
		get { 
			return this._token;
		} 
	} 

	private float _longitude; 

	public float longitude { 
		set { 
			if(!this.hasLongitude()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._longitude = value;
		} 
		get { 
			return this._longitude;
		} 
	} 

	private float _latitude; 

	public float latitude { 
		set { 
			if(!this.hasLatitude()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._latitude = value;
		} 
		get { 
			return this._latitude;
		} 
	} 

	public static OP_PLAYER_CONNECT_GET newBuilder() { 
		return new OP_PLAYER_CONNECT_GET(); 
	} 

	public static OP_PLAYER_CONNECT_GET decode(byte[] data) { 
		OP_PLAYER_CONNECT_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[4]; 

		int total = 0;
		if(this.hasPassportId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.passportId);
			total += bytes[0].limit();
		}

		if(this.hasToken()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.token);
			    short len = (short) _byte.Length;
			    bytes[1] = ByteBuffer.allocate(2 + len);
			    bytes[1].putShort(len);
				bytes[1].put(_byte);
			total += bytes[1].limit();
		}

		if(this.hasLongitude()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putFloat(this.longitude);
			total += bytes[2].limit();
		}

		if(this.hasLatitude()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putFloat(this.latitude);
			total += bytes[3].limit();
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
		  
		if(this.hasPassportId()) {
			this.passportId = buf.getInt();
		}

		if(this.hasToken()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.token = System.Text.Encoding.UTF8.GetString(bytes);
		}

		if(this.hasLongitude()) {
			this.longitude = buf.getFloat();
		}

		if(this.hasLatitude()) {
			this.latitude = buf.getFloat();
		}

	} 

	public bool hasPassportId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasToken() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasLongitude() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasLatitude() {
		return (this.__flag[0] & 8) != 0;
	}

}
}

