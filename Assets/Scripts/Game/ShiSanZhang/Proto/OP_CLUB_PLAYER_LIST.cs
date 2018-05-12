//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 5:30:25 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.sss {

public class OP_CLUB_PLAYER_LIST { 

	public const int CODE = 99307; 

	private byte[] __flag = new byte[1]; 

	private List<OP_CLUB_PLAYER> player = new List<OP_CLUB_PLAYER>(); 

	public OP_CLUB_PLAYER getPlayer(int index) { 
			return this.player[index];
	} 
	
	public void addPlayer(OP_CLUB_PLAYER value) { 
			if(!this.hasPlayer()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.player.Add(value);
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

	public static OP_CLUB_PLAYER_LIST newBuilder() { 
		return new OP_CLUB_PLAYER_LIST(); 
	} 

	public static OP_CLUB_PLAYER_LIST decode(byte[] data) { 
		OP_CLUB_PLAYER_LIST proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasPlayer()) {
				int length = 0;
				for(int i=0, len=this.player.Count; i<len; i++) {
					length += this.player[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.player.Count * 4 + length + 2);
				bytes[0].putShort((short) this.player.Count);
				for(int i=0, len=this.player.Count; i<len; i++) {
					byte[] _byte = this.player[i].encode();
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
		  
		if(this.hasPlayer()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.player.Add(OP_CLUB_PLAYER.decode(bytes));
			}
		}

		if(this.hasClubId()) {
			this.clubId = buf.getInt();
		}

	} 

	public int playerCount() {
		return this.player.Count;
	}

	public bool hasPlayer() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasClubId() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<OP_CLUB_PLAYER> getPlayerList() {
		return this.player;
	}

}
}

