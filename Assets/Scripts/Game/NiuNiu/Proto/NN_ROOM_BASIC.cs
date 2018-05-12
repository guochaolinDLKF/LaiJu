//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:18 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class NN_ROOM_BASIC { 

	public const int CODE = 2004; 

	private byte[] __flag = new byte[1]; 

	private int _roomId; 

	public int roomId { 
		set { 
			if(!this.hasRoomId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._roomId = value;
		} 
		get { 
			return this._roomId;
		} 
	} 

	private int _loop; 

	public int loop { 
		set { 
			if(!this.hasLoop()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._loop = value;
		} 
		get { 
			return this._loop;
		} 
	} 

	private int _maxLoop; 

	public int maxLoop { 
		set { 
			if(!this.hasMaxLoop()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._maxLoop = value;
		} 
		get { 
			return this._maxLoop;
		} 
	} 

	private long _unixtime; 

	public long unixtime { 
		set { 
			if(!this.hasUnixtime()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	private int _settingId; 

	public int settingId { 
		set { 
			if(!this.hasSettingId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._settingId = value;
		} 
		get { 
			return this._settingId;
		} 
	} 

	private NN_ENUM_ROOM_STATUS _nn_room_status; 

	public NN_ENUM_ROOM_STATUS nn_room_status { 
		set { 
			if(!this.hasNnRoomStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this._nn_room_status = value;
		} 
		get { 
			return this._nn_room_status;
		} 
	} 

	private int _nn_seatCount; 

	public int nn_seatCount { 
		set { 
			if(!this.hasNnSeatCount()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 64);
			}
			this._nn_seatCount = value;
		} 
		get { 
			return this._nn_seatCount;
		} 
	} 

	private int _clubId; 

	public int clubId { 
		set { 
			if(!this.hasClubId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this._clubId = value;
		} 
		get { 
			return this._clubId;
		} 
	} 

	public static NN_ROOM_BASIC newBuilder() { 
		return new NN_ROOM_BASIC(); 
	} 

	public static NN_ROOM_BASIC decode(byte[] data) { 
		NN_ROOM_BASIC proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[8]; 

		int total = 0;
		if(this.hasRoomId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.roomId);
			total += bytes[0].limit();
		}

		if(this.hasLoop()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.loop);
			total += bytes[1].limit();
		}

		if(this.hasMaxLoop()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.maxLoop);
			total += bytes[2].limit();
		}

		if(this.hasUnixtime()) {
			bytes[3] = ByteBuffer.allocate(8);
			bytes[3].putLong(this.unixtime);
			total += bytes[3].limit();
		}

		if(this.hasSettingId()) {
			bytes[4] = ByteBuffer.allocate(4);
			bytes[4].putInt(this.settingId);
			total += bytes[4].limit();
		}

		if(this.hasNnRoomStatus()) {
			bytes[5] = ByteBuffer.allocate(1);
			bytes[5].put((byte) this.nn_room_status);
			total += bytes[5].limit();
		}

		if(this.hasNnSeatCount()) {
			bytes[6] = ByteBuffer.allocate(4);
			bytes[6].putInt(this.nn_seatCount);
			total += bytes[6].limit();
		}

		if(this.hasClubId()) {
			bytes[7] = ByteBuffer.allocate(4);
			bytes[7].putInt(this.clubId);
			total += bytes[7].limit();
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
		  
		if(this.hasRoomId()) {
			this.roomId = buf.getInt();
		}

		if(this.hasLoop()) {
			this.loop = buf.getInt();
		}

		if(this.hasMaxLoop()) {
			this.maxLoop = buf.getInt();
		}

		if(this.hasUnixtime()) {
			this.unixtime = buf.getLong();
		}

		if(this.hasSettingId()) {
			this.settingId = buf.getInt();
		}

		if(this.hasNnRoomStatus()) {
			this.nn_room_status = (NN_ENUM_ROOM_STATUS) buf.get();
		}

		if(this.hasNnSeatCount()) {
			this.nn_seatCount = buf.getInt();
		}

		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

	} 

	public bool hasRoomId() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasLoop() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasMaxLoop() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasUnixtime() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasSettingId() {
		return (this.__flag[0] & 16) != 0;
	}

	public bool hasNnRoomStatus() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasNnSeatCount() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasClubId() {
		return (this.__flag[0] & 128) != 0;
	}

}
}

