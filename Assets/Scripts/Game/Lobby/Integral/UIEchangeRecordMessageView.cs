//===================================================
//Author      : DRB
//CreateTime  ：11/30/2017 11:40:23 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEchangeRecordMessageView : UIWindowViewBase
{
    public Text nameInput;
    public Text telephoneInput;
    public Text addressInput;

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        //btn_MakeSure
        switch (go.name)
        {
            case "btn_MakeSure":
                SendNotification("btn_SendPlayerMessage", nameInput.text, telephoneInput.text, addressInput.text);
                break;
            default:
                break;
        }
    }
}
