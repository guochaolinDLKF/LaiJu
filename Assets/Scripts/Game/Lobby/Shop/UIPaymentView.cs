//===================================================
//Author      : DRB
//CreateTime  ：8/31/2017 7:08:38 PM
//Description ：
//===================================================
using UnityEngine;
using UnityEngine.UI;

public class UIPaymentView : UIWindowViewBase
{
    [SerializeField]
    private Text m_TxtAmount;
    [SerializeField]
    private Text m_TxtPrice;


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);

        switch (go.name)
        {
            case "btnPaymentViewAlipay":
                SendNotification("btnPaymentViewAlipay");
                break;
            case "btnPaymentViewWeChatPay":
                SendNotification("btnPaymentViewWeChatPay");
                break;
        }
    }

    public void SetUI(int amount, int price)
    {
        m_TxtAmount.SafeSetText(amount.ToString());
        m_TxtPrice.SafeSetText(price.ToString());
    }

}
