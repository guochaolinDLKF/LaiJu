//===================================================
//Author      : DRB
//CreateTime  ：11/7/2017 7:54:48 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIItemRecordDetailZhaJH : UIItemRecordDetailBase
{

    [SerializeField]
    private Text m_Number;   
    [SerializeField]
    private Text[] m_TextScores;
    [SerializeField]
    private Transform[] pokerTran;
    


    private int m_nBattleId;

    private Action<int> m_OnReplayClick;


    public override void SetUI(TransferData data)
    {
        base.SetUI(data);      
        int loop = data.GetValue<int>("Loop");               
        m_Number.SafeSetText(string.Format("第{0}局", loop));

        List<TransferData> lstPlayer = data.GetValue<List<TransferData>>("Player");              
        Close();
        for (int i = 0; i < m_TextScores.Length; i++)
        {
            if (i < lstPlayer.Count)
            {
                m_TextScores[i].gameObject.SetActive(true);
                m_TextScores[i].SafeSetText(lstPlayer[i].GetValue<int>("Gold").ToString());
                for (int j = 0; j < lstPlayer[i].GetValue<List<TransferData>>("Pokers").Count; j++)
                {
                    GameObject go = ZJHPrefabManager.Instance.RecordLoadPoker(lstPlayer[i].GetValue<List<TransferData>>("Pokers")[j].GetValue<int>("Size"), lstPlayer[i].GetValue<List<TransferData>>("Pokers")[j].GetValue<int>("Color"), null, "normalpoker");
                    go.transform.SetParent(pokerTran[i]);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
            }
            else
            {
                m_TextScores[i].gameObject.SetActive(false);
            }
        }
    }

    private void Close()
    {
        for (int i = 0; i < pokerTran.Length; i++)
        {
            for (int j = 0; j < pokerTran[i].childCount; j++)
            {
                Destroy(pokerTran[i].GetChild(j).gameObject);
            }
        }
    }
}
