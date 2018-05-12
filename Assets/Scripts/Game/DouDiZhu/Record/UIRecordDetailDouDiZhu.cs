//===================================================
//Author      : DRB
//CreateTime  ：1/12/2018 11:09:56 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRecordDetailDouDiZhu : UIRecordDetailBase
{

    [SerializeField]
    private Text[] m_ArrNickName;


    protected override string RecordDetailPrefabName
    {
        get
        {
            return "UIItemRecordDetailDouDiZhu";
        }
    }

    public override void ShowRecordDetail(TransferData data)
    {
        base.ShowRecordDetail(data);

        List<TransferData> lstRecord = data.GetValue<List<TransferData>>("RecordDetail");
        if (lstRecord == null || lstRecord.Count == 0) return;
        List<TransferData> lstPlayer = lstRecord[0].GetValue<List<TransferData>>("Player"); ;
        for (int i = 0; i < m_ArrNickName.Length; ++i)
        {
            if (i < lstPlayer.Count)
            {
                m_ArrNickName[i].gameObject.SetActive(true);
                m_ArrNickName[i].SafeSetText(lstPlayer[i].GetValue<string>("NickName"));
            }
            else
            {
                m_ArrNickName[i].gameObject.SetActive(false);
            }
        }
    }
}
