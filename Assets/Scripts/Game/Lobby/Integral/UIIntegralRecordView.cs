//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 10:13:02 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIntegralRecordView : UIWindowViewBase
{
    public int index;
    [SerializeField]
    private Text exchangeDateText;
    [SerializeField]
    private Text exchangeTimeText;
    [SerializeField]
    private Text exchangeGoodsText;
    [SerializeField]
    private Button exchangeEunm;
    [SerializeField]
    private bool isReceived;

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btn_receive":
                SendNotification("btn_receive", index);
                break;
        }
    }

    public void SetUI(TransferData data)
    {
        IntegralRecordEntity integralRecordEntity = data.GetValue<IntegralRecordEntity>("integralRecordEntity");

        index = integralRecordEntity.index;
        exchangeDateText.SafeSetText(integralRecordEntity.exchangeDate.ToString());
        exchangeTimeText.SafeSetText(integralRecordEntity.exchangeTime.ToString());
        exchangeGoodsText.SafeSetText(integralRecordEntity.exchangeGoods.ToString());
        isReceived = integralRecordEntity.isApply;
        if (isReceived)
        {
            ReceiveGoods();
        }
        else
        {
            exchangeEunm.gameObject.SetActive(true);
        }
    }
    public void ReceiveGoods()
    {
        exchangeEunm.gameObject.SetActive(false);
    }
}
