//===================================================
//Author      : DRB
//CreateTime  ：11/9/2017 1:36:44 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIRecordDetailJuYou : UIRecordDetailBase {

    [SerializeField]
    private Text[] m_ArrNickName;


    protected override string RecordDetailPrefabName
    {
        get
        {
            return "UIItemRecordDetailJuYou";
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
