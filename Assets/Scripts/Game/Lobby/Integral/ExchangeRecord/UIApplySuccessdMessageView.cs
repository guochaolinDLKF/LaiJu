//===================================================
//Author      : DRB
//CreateTime  ：11/30/2017 4:12:24 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIApplySuccessdMessageView : UIWindowViewBase
{
    [SerializeField]
    private Text wxCustomerServiceText;
   

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        //btn_MakeSure
        switch (go.name)
        {
            case "btn_copy":
                OnBtnCopy();
                break;
            default:
                break;
        }
    }
    private void OnBtnCopy()
    {
        SDK.Instance.CopyTextToClipboard(wxCustomerServiceText.text);

        UIViewManager.Instance.ShowTip("已复制到剪切板");
    }
    public void SetUI(TransferData data)
    {
        string wx = data.GetValue<string>("wxCustonmerService");
        wxCustomerServiceText.SafeSetText(wx);
    }
}
