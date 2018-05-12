//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 10:49:02 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIntegralRecordWindow : UIWindowViewBase
{
    [SerializeField]
    private Transform m_Container;
    [SerializeField]
    private List<UIIntegralRecordView> m_lstUIIntegralRecordView;
    public void SetUI(TransferData data)
    {
        List<TransferData> lstData = data.GetValue<List<TransferData>>("lstRecord");
        for (int i = 0; i < lstData.Count; i++)
        {

            if (m_lstUIIntegralRecordView == null)
            {
                m_lstUIIntegralRecordView = new List<UIIntegralRecordView>();
            }

            Transform uiItemIntegral = null;
            bool isExists = false;
            for (int j = 0; j < m_lstUIIntegralRecordView.Count; j++)
            {
                if (m_lstUIIntegralRecordView[j].index == lstData[i].GetValue<IntegralRecordEntity>("integralRecordEntity").index)
                {
                    uiItemIntegral = m_lstUIIntegralRecordView[j].transform;
                    isExists = true;
                    break;
                }
            }
            if (!isExists)
            {
                uiItemIntegral = UIPoolManager.Instance.Spawn("UIItemIntegralRecord");
                m_lstUIIntegralRecordView.Add(uiItemIntegral.GetComponent<UIIntegralRecordView>());
            }

            uiItemIntegral.SetParent(m_Container);
            uiItemIntegral.localPosition = Vector3.zero;
            uiItemIntegral.localScale = Vector3.one;
            uiItemIntegral.GetComponent<UIIntegralRecordView>().SetUI(lstData[i]);
            

        }
    }
    public void SetIntegralRecord(TransferData data)
    {
        IntegralRecordEntity integralRecordEntity = data.GetValue<IntegralRecordEntity>("integralRecordEntity");

        for (int i = 0; i < m_lstUIIntegralRecordView.Count; i++)
        {
            if (m_lstUIIntegralRecordView[i].index == integralRecordEntity.index)
            {
                m_lstUIIntegralRecordView[i].ReceiveGoods();
                break;
            }
        }
    }
    public UIExchangeRecordMessageView OpenPrompt()
    {
        Transform uiItemGiftPrompt = UIViewUtil.Instance.LoadWindow("ExchangeRecordMessage").transform;
        //uiItemGiftPrompt.SetParent(transform);
        //uiItemGiftPrompt.localPosition = Vector3.zero;
        //uiItemGiftPrompt.localScale = Vector3.one;
        return uiItemGiftPrompt.GetComponent<UIExchangeRecordMessageView>();
    }

}
