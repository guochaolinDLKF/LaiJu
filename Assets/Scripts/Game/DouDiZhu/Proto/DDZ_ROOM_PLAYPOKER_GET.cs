using System.Collections.Generic;

namespace ddz.proto {

public class DDZ_ROOM_PLAYPOKER_GET { 

	public const int CODE = 301005; 

	private byte[] __flag = new byte[16]; 

	private List<DDZ_POCKER> pokerList = new List<DDZ_POCKER>(); 

	public DDZ_POCKER getPokerList(int index) { 
			return this.pokerList[index];
	} 
	
	public void addPokerList(DDZ_POCKER value) { 
			if(!this.hasPokerList()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this.pokerList.Add(value);
	} 

	private POKERLIST_TYPE _pokerListType; 

	public POKERLIST_TYPE pokerListType { 
		set { 
			if(!this.hasPokerListType()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this._pokerListType = value;
		} 
		get { 
			return this._pokerListType;
		} 
	} 

	public static DDZ_ROOM_PLAYPOKER_GET newBuilder() { 
		return new DDZ_ROOM_PLAYPOKER_GET(); 
	} 

	public static DDZ_ROOM_PLAYPOKER_GET decode(byte[] data) { 
		DDZ_ROOM_PLAYPOKER_GET proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasPokerList()) {
				int length = 0;
				for(int i=0, len=this.pokerList.Count; i<len; i++) {
					length += this.pokerList[i].encode().Length;
				}
				bytes[0] = ByteBuffer.allocate(this.pokerList.Count * 4 + length + 2);
				bytes[0].putShort((short) this.pokerList.Count);
				for(int i=0, len=this.pokerList.Count; i<len; i++) {
					byte[] _byte = this.pokerList[i].encode();
					bytes[0].putInt(_byte.Length);
					bytes[0].put(_byte);
				}
			total += bytes[0].limit();
		}

		if(this.hasPokerListType()) {
			bytes[1] = ByteBuffer.allocate(1);
			bytes[1].put((byte) this.pokerListType);
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
		  
		if(this.hasPokerList()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.pokerList.Add(DDZ_POCKER.decode(bytes));
			}
		}

		if(this.hasPokerListType()) {
			this.pokerListType = (POKERLIST_TYPE) buf.get();
		}

	} 

	public int pokerListCount() {
		return this.pokerList.Count;
	}

	public bool hasPokerList() {
		return (this.__flag[0] & 1) != 0;
	}

	public bool hasPokerListType() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<DDZ_POCKER> getPokerListList() {
		return this.pokerList;
	}

}
}

