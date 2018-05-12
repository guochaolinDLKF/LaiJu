//===================================================
//Author      : DRB
//CreateTime  ：11/7/2017 2:45:11 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemRecordMahjong : UIItemRecordBase
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
    [SerializeField]
    private Text m_txtPlayerTotalScore;//总分数

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


        List<TransferData> lstPlayer = data.GetValue<List<TransferData>>("Player");
        Debug.Log("lstPlayer.Count " + lstPlayer.Count);
        for (int i = 0; i < lstPlayer.Count; ++i)
        {
            bool isPlayer = lstPlayer[i].GetValue<bool>("IsPlayer");
            Debug.Log("lstPlayer.Count IsPlayer" + isPlayer);
            if (isPlayer)
            {
                int gold = lstPlayer[i].GetValue<int>("Gold");
                m_txtPlayerTotalScore.SafeSetText(gold.ToString());
            }
        }

        if (m_ArrNickName == null || m_ArrNickName.Length == 0) return;
        if (m_ArrGold == null || m_ArrGold.Length == 0) return;
        if (m_ArrNickName.Length != m_ArrGold.Length) return;
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
