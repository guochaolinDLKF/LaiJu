using System.Collections.Generic;

namespace proto.mahjong {

public class OP_MATCH_RESULT { 

	public const int CODE = 102006; 

	private byte[] __flag = new byte[16]; 

	private string _rewardDesc; 

	public string rewardDesc { 
		set { 
			if(!this.hasRewardDesc()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._rewardDesc = value;
		} 
		get { 
			return this._rewardDesc;
		} 
	} 

	private List<OP_MATCH_PLAYER_INFO> ranking = new List<OP_MATCH_PLAYER_INFO>(); 

	public OP_MATCH_PLAYER_INFO getRanking(int index) { 
			return this.ranking[index];
	} 
	
	public void addRanking(OP_MATCH_PLAYER_INFO value) { 
			if(!this.hasRanking()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this.ranking.Add(value);
	} 

	public static OP_MATCH_RESULT newBuilder() { 
		return new OP_MATCH_RESULT(); 
	} 

	public static OP_MATCH_RESULT decode(byte[] data) { 
		OP_MATCH_RESULT proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasRewardDesc()) {
			    byte[] _byte = System.Text.Encoding.UTF8.GetBytes(this.rewardDesc);
			    short len = (short) _byte.Length;
			    bytes[0] = ByteBuffer.allocate(2 + len);
			    bytes[0].putShort(len);
				bytes[0].put(_byte);
			total += bytes[0].limit();
		}

		if(this.hasRanking()) {
				int length = 0;
				for(int i=0, len=this.ranking.Count; i<len; i++) {
					length += this.ranking[i].encode().Length;
				}
				bytes[1] = ByteBuffer.allocate(this.ranking.Count * 4 + length + 2);
				bytes[1].putShort((short) this.ranking.Count);
				for(int i=0, len=this.ranking.Count; i<len; i++) {
					byte[] _byte = this.ranking[i].encode();
					bytes[1].putInt(_byte.Length);
					bytes[1].put(_byte);
				}
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
		  
		if(this.hasRewardDesc()) {
			byte[] bytes = new byte[buf.getShort()];
			buf.get(ref bytes, 0, bytes.Length);
			this.rewardDesc = System.Text.Encoding.UTF8.GetString(bytes);
		}

		if(this.hasRanking()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.ranking.Add(OP_MATCH_PLAYER_INFO.decode(bytes));
			}
		}

	} 

	public bool hasRewardDesc() {
		return (this.__flag[0] & 1) != 0;
	}

	public int rankingCount() {
		return this.ranking.Count;
	}

	public bool hasRanking() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<OP_MATCH_PLAYER_INFO> getRankingList() {
		return this.ranking;
	}

}
}

