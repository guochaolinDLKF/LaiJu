//===================================================
//Author      : DRB
//CreateTime  ：12/5/2017 12:01:57 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace proto.pdk {

public class PDK_GAME_OVER { 

	public const int CODE = 901009; 

	private byte[] __flag = new byte[1]; 

	private int _winnert_pos; 

	public int winnert_pos { 
		set { 
			if(!this.hasWinnertPos()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 1);
			}
			this._winnert_pos = value;
		} 
		get { 
			return this._winnert_pos;
		} 
	} 

	private List<SEAT_SETTING> seat_setting = new List<SEAT_SETTING>(); 

	public SEAT_SETTING getSeatSetting(int index) { 
			return this.seat_setting[index];
	} 
	
	public void addSeatSetting(SEAT_SETTING value) { 
			if(!this.hasSeatSetting()) {
	    		this.__flag[0] = (byte) (this.__flag[0] | 2);
			}
			this.seat_setting.Add(value);
	} 

	public static PDK_GAME_OVER newBuilder() { 
		return new PDK_GAME_OVER(); 
	} 

	public static PDK_GAME_OVER decode(byte[] data) { 
		PDK_GAME_OVER proto = newBuilder();
		proto.build(data);
		return proto; 
	} 

	public byte[] encode() { 

		ByteBuffer[] bytes = new ByteBuffer[2]; 

		int total = 0;
		if(this.hasWinnertPos()) {
			bytes[0] = ByteBuffer.allocate(4);
			bytes[0].putInt(this.winnert_pos);
			total += bytes[0].limit();
		}

		if(this.hasSeatSetting()) {
				int length = 0;
				for(int i=0, len=this.seat_setting.Count; i<len; i++) {
					length += this.seat_setting[i].encode().Length;
				}
				bytes[1] = ByteBuffer.allocate(this.seat_setting.Count * 4 + length + 2);
				bytes[1].putShort((short) this.seat_setting.Count);
				for(int i=0, len=this.seat_setting.Count; i<len; i++) {
					byte[] _byte = this.seat_setting[i].encode();
					bytes[1].putInt(_byte.Length);
					bytes[1].put(_byte);
				}
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
		  
		if(this.hasWinnertPos()) {
			this.winnert_pos = buf.getInt();
		}

		if(this.hasSeatSetting()) {
			int size = buf.getShort();
			for(int i=0; i<size; i++) {
			    byte[] bytes = new byte[buf.getInt()];
			    buf.get(ref bytes, 0, bytes.Length);
			    this.seat_setting.Add(SEAT_SETTING.decode(bytes));
			}
		}

	} 

	public bool hasWinnertPos() {
		return (this.__flag[0] & 1) != 0;
	}

	public int seatSettingCount() {
		return this.seat_setting.Count;
	}

	public bool hasSeatSetting() {
		return (this.__flag[0] & 2) != 0;
	}

	public List<SEAT_SETTING> getSeatSettingList() {
		return this.seat_setting;
	}

}
}

