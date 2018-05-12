//===================================================
//Author      : DRB
//CreateTime  ：1/12/2018 11:10:26 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemRecordDouDiZhu : UIItemRecordBase
{
    [SerializeField]
    private Text m_txtIndex;
    [SerializeField]
    private Text m_txtRoomId;
    [SerializeField]
    private Text m_txtBattleId;
    [SerializeField]
    private Text m_txtMaxLoop;
    [SerializeField]
    private Text m_txtOwnerName;
    [SerializeField]
    private Text m_txtDateTime;
    [SerializeField]
    private Text[] m_ArrNickName;
    [SerializeField]
    private Text[] m_ArrGold;


    public override void SetUI(TransferData data)
    {
        base.SetUI(data);
        int index = data.GetValue<int>("Index");
        int roomId = data.GetValue<int>("RoomId");
        int battleId = data.GetValue<int>("BattleId");
        int maxLoop = data.GetValue<int>("MaxLoop");
        string ownerName = data.GetValue<string>("OwnerName");
        string time = data.GetValue<string>("DateTime");

        m_txtIndex.SafeSetText(index.ToString());
        m_txtRoomId.SafeSetText(roomId.ToString());
        m_txtBattleId.SafeSetText(battleId.ToString());
        m_txtMaxLoop.SafeSetText(maxLoop.ToString());
        m_txtOwnerName.SafeSetText(ownerName);
        m_txtDateTime.SafeSetText(time);


        if (m_ArrNickName == null || m_ArrNickName.Length == 0) return;
        if (m_ArrGold == null || m_ArrGold.Length == 0) return;
        if (m_ArrNickName.Length != m_ArrGold.Length) return;
        List<TransferData> lstPlayer = data.GetValue<List<TransferData>>("Player");
        for (int i = 0; i < m_ArrNickName.Length; ++i)
        {
            if (i >= lstPlayer.Count)
            {
                m_ArrNickName[i].SafeSetText(string.Empty);
                m_ArrGold[i].SafeSetText(string.Empty);
            }
            else
            {
                string nickName = lstPlayer[i].GetValue<string>("NickName");
                int gold = lstPlayer[i].GetValue<int>("Gold");
                m_ArrNickName[i].SafeSetText(nickName);
                m_ArrGold[i].SafeSetText(gold.ToString());
            }

        }
    }
}
