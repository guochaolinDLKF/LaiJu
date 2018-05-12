//===================================================
//Author      : DRB
//CreateTime  ：11/7/2017 7:31:43 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIRecordDetailZhaJH : UIRecordDetailBase
{
    [SerializeField]
    private Text[] m_ArrNickName;
    [SerializeField]
    private Text[] m_TextScores;

    protected override string RecordDetailPrefabName
    {
        get
        {
            return "UIItemRecordDetailZhaJinHua";
        }
    }

    public override void ShowRecordDetail(TransferData data)
    {
        base.ShowRecordDetail(data);
        List<TransferData> lstPlayers = data.GetValue<List<TransferData>>("Player");
        //if (lstRecord == null || lstRecord.Count == 0) return;
        //List<TransferData> lstPlayer = lstRecord[0].GetValue<List<TransferData>>("Player");           
        for (int i = 0; i < m_ArrNickName.Length; ++i)
        {
            if (i < lstPlayers.Count)
            {
                m_ArrNickName[i].gameObject.SetActive(true);
                m_ArrNickName[i].SafeSetText(lstPlayers[i].GetValue<string>("NickName"));
                m_TextScores[i].gameObject.SetActive(true);
                m_TextScores[i].SafeSetText(lstPlayers[i].GetValue<int>("Gold").ToString());
            }
            else
            {
                m_ArrNickName[i].gameObject.SetActive(false);
                m_TextScores[i].gameObject.SetActive(false);
            }
        }

    }

   
}
