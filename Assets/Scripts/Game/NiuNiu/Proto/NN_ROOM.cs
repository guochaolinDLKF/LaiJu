//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 7:00:13 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace niuniu.proto {

public class NN_ROOM { 

	public const int CODE = 2003; 

	private byte[] __flag = new byte[2]; 

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

	private List<int> settingId = new List<int>(); 

	public int getSettingId(int index) { 
			return this.settingId[index];
	} 
	
	public void addSettingId(int value) { 
			if(!this.hasSettingId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this.settingId.Add(value);
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

	private int _maxLoop; 

	public int maxLoop { 
		set { 
			if(!this.hasMaxLoop()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 8);
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
	    		this.__flag[0] = (byte) (this.__flag[0] | 16);
			}
			this._unixtime = value;
		} 
		get { 
			return this._unixtime;
		} 
	} 

	private List<NN_SEAT> nn_seat = new List<NN_SEAT>(); 

	public NN_SEAT getNnSeat(int index) { 
			return this.nn_seat[index];
	} 
	
	public void addNnSeat(NN_SEAT value) { 
			if(!this.hasNnSeat()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 32);
			}
			this.nn_seat.Add(value);
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

	private NN_ENUM_ROOM_STATUS _nn_room_status; 

	public NN_ENUM_ROOM_STATUS nn_room_status { 
		set { 
			if(!this.hasNnRoomStatus()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 128);
			}
			this._nn_room_status = value;
		} 
		get { 
			return this._nn_room_status;
		} 
	} 

	private int _pos; 

	public int pos { 
		set { 
			if(!this.hasPos()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 1);
			}
			this._pos = value;
		} 
		get { 
			return this._pos;
		} 
	} 

	private int _clubId; 

	public int clubId { 
		set { 
			if(!this.hasClubId()) {
	    		this.__flag[1] = (byte) (this.__flag[1] | 2);
			}
			this._clubId = value;
		} 
		get { 
			return this._clubId;
		} 
	} 

	public static NN_ROOM newBuilder() { 
		return new NN_ROOM(); 
	} 

	public static NN_ROOM decode(byte[] data) { 
		NN_ROOM proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[10]; 

		int total = 0;
		if(this.hasRoomId()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.roomId);
			total += bytes[0].limit();
		}

		if(this.hasSettingId()) {
			bytes[1] = ByteBuffer.allocate(this.settingId.Count * 4 + 2);
			bytes[1].putShort((short) this.settingId.Count);
			for(int i=0, len=this.settingId.Count; i<len; i++) {
				bytes[1].putInt(this.settingId[i]);
			}
			total += bytes[1].limit();
		}

		if(this.hasLoop()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.loop);
			total += bytes[2].limit();
		}

		if(this.hasMaxLoop()) {
			bytes[3] = ByteBuffer.allocate(4);
			bytes[3].putInt(this.maxLoop);
			total += bytes[3].limit();
		}

		if(this.hasUnixtime()) {
			bytes[4] = ByteBuffer.allocate(8);
			bytes[4].putLong(this.unixtime);
			total += bytes[4].limit();
		}

		if(this.hasNnSeat()) {
				int length = 0;
				for(int i=0, len=this.nn_seat.Count; i<len; i++) {
					length += this.nn_seat[i].encode().Length;
				}
				bytes[5] = ByteBuffer.allocate(this.nn_seat.Count * 4 + length + 2);
				bytes[5].putShort((short) this.nn_seat.Count);
				for(int i=0, len=this.nn_seat.Count; i<len; i++) {
					byte[] _byte = this.nn_seat[i].encode();
					bytes[5].putInt(_byte.Length);
					bytes[5].put(_byte);
				}
			total += bytes[5].limit();
		}

		if(this.hasNnSeatCount()) {
			bytes[6] = ByteBuffer.allocate(4);
			bytes[6].putInt(this.nn_seatCount);
			total += bytes[6].limit();
		}

		if(this.hasNnRoomStatus()) {
			bytes[7] = ByteBuffer.allocate(1);
			bytes[7].put((byte) this.nn_room_status);
			total += bytes[7].limit();
		}

		if(this.hasPos()) {
			bytes[8] = ByteBuffer.allocate(4);
			bytes[8].putInt(this.pos);
			total += bytes[8].limit();
		}

		if(this.hasClubId()) {
			bytes[9] = ByteBuffer.allocate(4);
			bytes[9].putInt(this.clubId);
			total += bytes[9].limit();
		}

	
		ByteBuffer buf = ByteBuffer.allocate(2 + total);
	
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

		if(this.hasSettingId()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    this.settingId.Add(buf.getInt());
			}
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

		if(this.hasNnSeat()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.nn_seat.Add(NN_SEAT.decode(bytes));
			}
		}

		if(this.hasNnSeatCount()) {
			this.nn_seatCount = buf.getInt();
		}

		if(this.hasNnRoomStatus()) {
			this.nn_room_status = (NN_ENUM_ROOM_STATUS) buf.get();
		}

		if(this.hasPos()) {
			this.pos = buf.getInt();
		}

		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

	} 

	public bool hasRoomId() {
		return (this.__flag[0] & 1) != 0;
	}

	public int settingIdCount() {
		return this.settingId.Count;
	}

	public bool hasSettingId() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasLoop() {
		return (this.__flag[0] & 4) != 0;
	}

	public bool hasMaxLoop() {
		return (this.__flag[0] & 8) != 0;
	}

	public bool hasUnixtime() {
		return (this.__flag[0] & 16) != 0;
	}

	public int nnSeatCount() {
		return this.nn_seat.Count;
	}

	public bool hasNnSeat() {
		return (this.__flag[0] & 32) != 0;
	}

	public bool hasNnSeatCount() {
		return (this.__flag[0] & 64) != 0;
	}

	public bool hasNnRoomStatus() {
		return (this.__flag[0] & 128) != 0;
	}

	public bool hasPos() {
		return (this.__flag[1] & 1) != 0;
	}

	public bool hasClubId() {
		return (this.__flag[1] & 2) != 0;
	}

	public List<int> getSettingIdList() {
		return this.settingId;
	}

	public List<NN_SEAT> getNnSeatList() {
		return this.nn_seat;
	}

}
}

