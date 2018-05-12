//===================================================
//Author      : DRB
//CreateTime  ：11/13/2017 4:13:00 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIItemRecordDetailPaoDeKuai : UIItemRecordDetailBase
{
    [SerializeField]
    private Text m_txtLoop;
    [SerializeField]
    private Text m_txtDateTime;
    [SerializeField]
    private Text[] m_ArrGold;

    public override void SetUI(TransferData data)
    {
        base.SetUI(data);

        int loop = data.GetValue<int>("Loop");
        string time = data.GetValue<string>("DateTime");
        List<TransferData> lstPlayer = data.GetValue<List<TransferData>>("Player");
        for (int i = 0; i < m_ArrGold.Length; ++i)
        {
            if (i < lstPlayer.Count)
            {
                m_ArrGold[i].gameObject.SetActive(true);
                TransferData playerData = lstPlayer[i];
                int gold = playerData.GetValue<int>("Gold");
                m_ArrGold[i].SafeSetText(string.Format("<color={0}>{1}</color>", gold > 0 ? "red" : "green", gold.ToString()));
            }
            else
            {
                m_ArrGold[i].gameObject.SetActive(false);
            }
        }

        m_txtLoop.SafeSetText(string.Format("第{0}局", loop.ToString()));
        m_txtDateTime.SafeSetText(time);

    }

}
