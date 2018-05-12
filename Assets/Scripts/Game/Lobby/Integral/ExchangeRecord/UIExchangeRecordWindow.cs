//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 10:49:02 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIExchangeRecordWindow : UIWindowViewBase
{
    [SerializeField]
    private Transform m_Container;
    [SerializeField]
    private List<UIExchangeRecordView> m_lstUIIntegralRecordView;
    public void SetUI(TransferData data)
    {
        List<TransferData> lstData = data.GetValue<List<TransferData>>("lstRecord");
        for (int i = 0; i < lstData.Count; i++)
        {

            if (m_lstUIIntegralRecordView == null)
            {
                m_lstUIIntegralRecordView = new List<UIExchangeRecordView>();
            }

            Transform uiItemIntegral = null;
            bool isExists = false;
            for (int j = 0; j < m_lstUIIntegralRecordView.Count; j++)
            {
                if (m_lstUIIntegralRecordView[j].GetIndex() == lstData[i].GetValue<ExchangeRecordEntity>("ExchangeRecordEntity").id)
                {
                    uiItemIntegral = m_lstUIIntegralRecordView[j].transform;
                    isExists = true;
                    break;
                }
            }
            if (!isExists)
            {
                uiItemIntegral = UIPoolManager.Instance.Spawn("UIItemGiftRecord");
                m_lstUIIntegralRecordView.Add(uiItemIntegral.GetComponent<UIExchangeRecordView>());
            }

            uiItemIntegral.SetParent(m_Container);
            if (uiItemIntegral.localPosition.z != 0)
            {
                uiItemIntegral.localPosition = Vector3.zero;
            }
            uiItemIntegral.localScale = Vector3.one;
            uiItemIntegral.SetSiblingIndex(0);
            uiItemIntegral.GetComponent<UIExchangeRecordView>().SetUI(lstData[i]);
        }
    }
    public void SetExchangeRecord(TransferData data)
    {
        int id = data.GetValue<int>("id");

        for (int i = 0; i < m_lstUIIntegralRecordView.Count; i++)
        {
            if (m_lstUIIntegralRecordView[i].GetIndex() == id)
            {
                m_lstUIIntegralRecordView[i].GiftReceived();
                break;
            }
        }
    }
    public UIPlayerReceiveRecordInfoView OpenPrompt(string name,string phone,string address,int index, System.Action action, PrizeType prizeType)
    {
        Transform uiItemGiftPrompt = UIViewUtil.Instance.LoadWindow("PlayerReceiveRecordInfo").transform;
        //uiItemGiftPrompt.SetParent(transform);
        //uiItemGiftPrompt.localPosition = Vector3.zero;
        //uiItemGiftPrompt.localScale = Vector3.one;
        uiItemGiftPrompt.GetComponent<UIPlayerReceiveRecordInfoView>().SetUI(name, phone, address,index, action,prizeType);
        return uiItemGiftPrompt.GetComponent<UIPlayerReceiveRecordInfoView>();
    }

}
