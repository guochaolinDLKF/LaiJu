using System.Collections.Generic;

namespace ddz.proto {

public class DDZ_ROOM_INFORMBANKER { 

	public const int CODE = 301023; 

	private byte[] __flag = new byte[16]; 

	private List<DDZ_POCKER> basePokerList = new List<DDZ_POCKER>(); 

	public DDZ_POCKER getBasePokerList(int index) { 
			return this.basePokerList[index];
	} 
	
	public void addBasePokerList(DDZ_POCKER value) { 
			if(!this.hasBasePokerList()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.basePokerList.Add(value);
	} 

	private int _playerId; 

	public int playerId { 
		set { 
			if(!this.hasPlayerId()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._playerId = value;
		} 
		get { 
			return this._playerId;
		} 
	} 

	public static DDZ_ROOM_INFORMBANKER newBuilder() { 
		return new DDZ_ROOM_INFORMBANKER(); 
	} 

	public static DDZ_ROOM_INFORMBANKER decode(byte[] data) { 
		DDZ_ROOM_INFORMBANKER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasBasePokerList()) {
				int length = 0;
				for(int i=0, len=this.basePokerList.Count; i<len; i++) {
					length += this.basePokerList[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.basePokerList.Count * 4 + length + 2);
				bytes[0].putShort((short) this.basePokerList.Count);
				for(int i=0, len=this.basePokerList.Count; i<len; i++) {
					byte[] _byte = this.basePokerList[i].encode();
					bytes[0].putInt(_byte.Length);
					bytes[0].put(_byte);
				}
			total += bytes[0].limit();
		}

		if(this.hasPlayerId()) {
			bytes[1] = ByteBuffer.allocate(4);
			bytes[1].putInt(this.playerId);
			total += bytes[1].limit();
		}

	
		ByteBuffer buf = ByteBuffer.allocate(16 + total);
	
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
		  
		if(this.hasBasePokerList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.basePokerList.Add(DDZ_POCKER.decode(bytes));
			}
		}

		if(this.hasPlayerId()) {
			this.playerId = buf.getInt();
		}

	} 

	public int basePokerListCount() {
		return this.basePokerList.Count;
	}

	public bool hasBasePokerList() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPlayerId() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<DDZ_POCKER> getBasePokerListList() {
		return this.basePokerList;
	}

}
}

