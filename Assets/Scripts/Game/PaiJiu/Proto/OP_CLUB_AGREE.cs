//===================================================
//Author      : DRB
//CreateTime  ：10/25/2017 7:24:13 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.paigow {

public class OP_CLUB_AGREE { 

	public const int CODE = 99310; 

	private byte[] __flag = new byte[1]; 

	private bool _isSucceed; 

	public bool isSucceed { 
		set { 
			if(!this.hasIsSucceed()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._isSucceed = value;
		} 
		get { 
			return this._isSucceed;
		} 
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

	private int _playerId; 

	public int playerId { 
		set { 
			if(!this.hasPlayerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 4);
			}
			this._playerId = value;
		} 
		get { 
			return this._playerId;
		} 
	} 

	public static OP_CLUB_AGREE newBuilder() { 
		return new OP_CLUB_AGREE(); 
	} 

	public static OP_CLUB_AGREE decode(byte[] data) { 
		OP_CLUB_AGREE proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[3]; 

		int total = 0;
		if(this.hasIsSucceed()) {
			bytes[0] = ByteBuffer.allocate(1);
			if(this.isSucceed) {
				bytes[0].put((byte) 1);
			}else{
				bytes[0].put((byte) 0);
			}
			total += bytes[0].limit();
		}

		if(this.hasClubId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.clubId);
			total += bytes[1].limit();
		}

		if(this.hasPlayerId()) {
			bytes[2] = ByteBuffer.allocate(4);
			bytes[2].putInt(this.playerId);
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
		  
		if(this.hasIsSucceed()) {
			if(buf.get() == 1) {
				this.isSucceed = true;
			}else{
				this.isSucceed = false;
			}
		}

		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

	} 

	public bool hasIsSucceed() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasClubId() {
		return (this.__flag[0] & 2) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[0] & 4) != 0;
	}

}
}

