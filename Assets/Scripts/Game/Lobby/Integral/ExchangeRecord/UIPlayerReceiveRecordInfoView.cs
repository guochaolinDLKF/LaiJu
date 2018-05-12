//===================================================
//Author      : DRB
//CreateTime  ：11/30/2017 11:40:23 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerReceiveRecordInfoView : UIWindowViewBase
{
    public InputField nameInput;
    public InputField telephoneInput;
    public InputField addressInput;
    private PrizeType prizeType;
    [SerializeField]
    private int index;
    [SerializeField]
    private string name;
    [SerializeField]
    private string phone;
    [SerializeField]
    private string address;
    [SerializeField]
    System.Action action;

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        //btn_MakeSure
        switch (go.name)
        {
            case "btn_MakeSure":
                SetText();
                action();
                SendNotification("btn_SendPlayerMessage", name, phone, address, index, prizeType);
                break;
            default:
                break;
        }
    }
    public void SetUI(string name,string phone,string address,int index, System.Action action, PrizeType prizeType)
    {
        nameInput.text = name;
        telephoneInput.text = phone;
        addressInput.text = address;
        this.name = name;
        this.phone = phone;
        this.address = address;
        this.index = index;
        this.action = action;
        this.prizeType = prizeType;
    }
    public void SetText()
    {
        name = nameInput.text;
        phone = telephoneInput.text;
        address = addressInput.text;
    }
}
