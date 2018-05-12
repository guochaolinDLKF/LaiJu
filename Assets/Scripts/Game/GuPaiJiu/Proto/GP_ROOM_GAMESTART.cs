//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 1:48:21 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.gp {

public class GP_ROOM_GAMESTART { 

	public const int CODE = 701006; 

	private byte[] __flag = new byte[1]; 

	private bool _isAddPanBase; 

	public bool isAddPanBase { 
		set { 
			if(!this.hasIsAddPanBase()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._isAddPanBase = value;
		} 
		get { 
			return this._isAddPanBase;
		} 
	} 

	private int _panBase; 

	public int panBase { 
		set { 
			if(!this.hasPanBase()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._panBase = value;
		} 
		get { 
			return this._panBase;
		} 
	} 

	private int _loop; 

	public int loop { 
		set { 
			if(!this.hasLoop()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._loop = value;
		} 
		get { 
			return this._loop;
		} 
	} 

	public static GP_ROOM_GAMESTART newBuilder() { 
		return new GP_ROOM_GAMESTART(); 
	} 

	public static GP_ROOM_GAMESTART decode(byte[] data) { 
		GP_ROOM_GAMESTART proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasIsAddPanBase()) {
			bytes[0] = ByteBuffer.allocate(1);
			if(this.isAddPanBase) {
				bytes[0].put((byte) 1);
			}else{
				bytes[0].put((byte) 0);
			}
			total += bytes[0].limit();
		}

		if(this.hasPanBase()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.panBase);
			total += bytes[1].limit();
		}

		if(this.hasLoop()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.loop);
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
		  
		if(this.hasIsAddPanBase()) {
			if(buf.get() == 1) {
				this.isAddPanBase = true;
			}else{
				this.isAddPanBase = false;
			}
		}

		if(this.hasPanBase()) {
			this.panBase = buf.getInt();
		}

		if(this.hasLoop()) {
			this.loop = buf.getInt();
		}

	} 

	public bool hasIsAddPanBase() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPanBase() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasLoop() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

