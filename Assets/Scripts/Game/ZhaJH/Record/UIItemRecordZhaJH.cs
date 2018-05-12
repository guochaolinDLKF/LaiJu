//===================================================
//Author      : DRB
//CreateTime  ：11/7/2017 8:44:59 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIItemRecordZhaJH : UIItemRecordBase
{
    //[SerializeField]
    //private Text m_TextBattleId;
    [SerializeField]
    private Text m_TextRoomId;
    [SerializeField]
    private Text m_TextLoopCount;
    [SerializeField]
    private Text m_TextRoomType;
    [SerializeField]
    private Text m_TextTime;
    private Action<int> m_OnClick;


    public override void SetUI(TransferData data)
    {
        base.SetUI(data);
        int index = data.GetValue<int>("Index");
        int roomId = data.GetValue<int>("RoomId");
        int battleId = data.GetValue<int>("BattleId");
        int maxLoop = data.GetValue<int>("MaxLoop");
        string ownerName = data.GetValue<string>("OwnerName");
        string time = data.GetValue<string>("DateTime");
        int roomType = data.GetValue<int>("RoomType");
        string roomTypeName = string.Empty;
        switch (roomType)
        {
            case 1:
                roomTypeName = "普通玩法";
                break;
            case 2:
                roomTypeName = "高级玩法";
                break;
            case 3:
                roomTypeName = "激情玩法";
                break;
            default:
                break;
        }

        m_TextRoomId.SafeSetText(battleId.ToString());
        m_TextLoopCount.SafeSetText(maxLoop.ToString());
        m_TextTime.SafeSetText(time.ToString());
        m_TextRoomType.SafeSetText(roomTypeName.ToString());


    }

}
