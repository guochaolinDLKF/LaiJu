//===================================================
//Author      : DRB
//CreateTime  ：7/6/2017 11:52:07 AM
//Description ：我的房间数据实体
//===================================================
using System.Collections.Generic;
using UnityEngine;


public class MyRoomEntity 
{
    public int roomId;

    public int gameId;

    public string gameName;

    public int loop;

    public int maxLoop;

    public int player;

    public int maxPlayer;

    public string ownerName;

    public List<MyRoomPlayerEntity> players;

    public int[] roomSetting;

    public CardPaymentType payment
    {
        get
        {
            CardPaymentType payment = CardPaymentType.AA;
            if (roomSetting != null)
            {
                for (int j = 0; j < roomSetting.Length; ++j)
                {
                    cfg_settingEntity setting = cfg_settingDBModel.Instance.Get(roomSetting[j]);
                    if (setting != null)
                    {
                        if (setting.label.Equals("支付"))
                        {
                            payment = (CardPaymentType)setting.value;
                        }
                    }
                }
            }
            return payment;
        }
    }
}

public class MyRoomPlayerEntity
{
    public int playerId;

    public string nickname;
}
