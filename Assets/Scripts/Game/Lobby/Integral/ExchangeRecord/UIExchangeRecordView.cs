//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 10:13:02 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExchangeRecordView : UIWindowViewBase
{
    [SerializeField]
    private int index;
    [SerializeField]
    private int status;
    [SerializeField]
    private PrizeType prizeType;
    [SerializeField]
    private Text exchangeDateText;
    [SerializeField]
    private Text exchangeTimeText;
    [SerializeField]
    private Text exchangeGoodsText;
    [SerializeField]
    private Button btn_exchangeState;
    [SerializeField]
    private GameObject exchangeFictitiousStateObj;
    [SerializeField]
    private GameObject exchangeActualStateObj;
    public int GetIndex()
    {
        return index;
    }
    public int GetStatus()
    {
        return status;
    }


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btn_receive":
                SendNotification("btn_receive", index,prizeType);
                break;
        }
    }

    public void SetUI(TransferData data)
    {
        ExchangeRecordEntity exchangeRecordEntity = data.GetValue<ExchangeRecordEntity>("ExchangeRecordEntity");
        prizeType = data.GetValue<PrizeType>("PrizeType");
        index = exchangeRecordEntity.id;
        exchangeDateText.SafeSetText(exchangeRecordEntity.date.ToString());
        exchangeTimeText.SafeSetText(exchangeRecordEntity.add_time.ToString());
        exchangeGoodsText.SafeSetText(exchangeRecordEntity.name.ToString());
        status = exchangeRecordEntity.status;
        SortSilbing();
        if (status == 0)
        {
            btn_exchangeState.gameObject.SetActive(true);
        }
        else if (status == 1)
        {
            GiftReceived();
            exchangeFictitiousStateObj.SetActive(true);
            exchangeActualStateObj.SetActive(false);
        }
        else
        {
            GiftReceived();
            exchangeFictitiousStateObj.SetActive(false);
            exchangeActualStateObj.SetActive(true);

        }
    }
    public void GiftReceived()
    {
        btn_exchangeState.gameObject.SetActive(false);
        //SortSilbing();
    }
    private void SortSilbing()
    {
        if (transform.parent.childCount > 0)
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i).GetComponent<UIExchangeRecordView>().GetStatus() != 0)
                {
                    transform.parent.GetChild(i).SetSiblingIndex(transform.parent.childCount - 1);
                }
            }
            //for (int i = transform.parent.childCount - 1; i >= 0 ; i--)
            //{

            //    //Debug.LogWarning(transform.parent.GetChild(i).GetComponent<UIExchangeRecordView>().GetStatus());
            //    //Debug.LogWarning(transform.parent.GetChild(i).GetComponent<UIExchangeRecordView>().name);
            //    //Debug.LogWarning("===========================================================");
            //    if (transform.parent.GetChild(i).GetComponent<UIExchangeRecordView>().GetStatus() == 0)
            //    {
            //        Debug.LogWarning(transform.parent.GetChild(i).GetComponent<UIExchangeRecordView>().GetStatus());
            //        Debug.LogWarning(transform.parent.GetChild(i).GetComponent<UIExchangeRecordView>().name);
            //        transform.parent.GetChild(i).SetSiblingIndex(3);
            //        Debug.LogWarning("===========================================================");
            //    }
            //}
        }
    }
}
