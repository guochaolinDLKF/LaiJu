//===================================================
//Author      : DRB
//CreateTime  ：11/28/2017 2:37:33 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIntegralWindow : UIWindowViewBase
{
    [SerializeField]
    private Transform m_Container;
    [SerializeField]
    private Text integralCountText;
    [SerializeField]
    private List<UIIntegralView> m_lstUIIntegralView;


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btn_integralRule":
                SendNotification("btn_integralRule");
                break;
            case "btn_integralRecord":
                SendNotification("btn_integralRecord");
                break;
        }
    }
    public void SetUI(TransferData data)
    {
        SetIntegralCount(data);

        List<IntegralEntity> lst = new List<IntegralEntity>();

        List<TransferData> integralEntityData = data.GetValue<List<TransferData>>("lstData");

        if (m_lstUIIntegralView == null)
        {
            m_lstUIIntegralView = new List<UIIntegralView>();
        }

        for (int i = 0; i < integralEntityData.Count; i++)
        {
            Transform uiItemIntegral = null;
            lst.Add(integralEntityData[i].GetValue<IntegralEntity>("integralEntity"));
            bool isExists = false;
            for (int j = 0; j < m_lstUIIntegralView.Count; j++)
            {
                if (m_lstUIIntegralView[j].index == integralEntityData[i].GetValue<IntegralEntity>("integralEntity").id)
                {
                    uiItemIntegral = m_lstUIIntegralView[j].transform;
                    isExists = true;
                    break;
                }
            }
            if (!isExists)
            {
                uiItemIntegral = UIPoolManager.Instance.Spawn("UIItemGift");
                m_lstUIIntegralView.Add(uiItemIntegral.GetComponent<UIIntegralView>());
            }

            uiItemIntegral.SetParent(m_Container);
            if (uiItemIntegral.localPosition.z != 0)
            {
                uiItemIntegral.localPosition = Vector3.zero;
            }
            uiItemIntegral.localScale = Vector3.one;
            uiItemIntegral.GetComponent<UIIntegralView>().SetUI(integralEntityData[i]);
        }
    }
    public void SetIntegralCount(TransferData data)
    {
        int count = data.GetValue<int>("Count");
        integralCountText.SafeSetText(count.ToString());
    }
}
