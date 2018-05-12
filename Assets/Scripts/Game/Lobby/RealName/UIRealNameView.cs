//===================================================
//Author      : DRB
//CreateTime  ：12/15/2017 10:35:45 AM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRealNameView : UIWindowViewBase
{
    [SerializeField]
    private Button m_button;
    [SerializeField]
    private InputField m_FieldName;
    [SerializeField]
    private InputField m_FieldPlayerID;


    public InputField Telephone;
    public InputField VerifyCode;
    //[SerializeField]
    //private Text m_TextPhone;
    //[SerializeField]
    //private Button m_ButtonBind;
    //[SerializeField]
    //private Button m_ButtonUnbind;



    private Action<string,string,string, string> m_OnClick;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_button.onClick.AddListener(OnBtnClick);
    }


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnRealNameViewGetVerifyCode":
                SendNotification("btnRealNameViewGetVerifyCode");
                break;
            case "btnBindViewValidate":
                SendNotification("btnBindViewValidate");
                break;
            case "btnBindViewUnBind":
                SendNotification("btnBindViewUnBind");
                break;
        }
    }

    public void SetUI(Action<string,string,string,string> onClick)
    {
        m_OnClick = onClick;      
    }

    private void OnBtnClick()
    {
        if (m_OnClick != null)
        {
            m_OnClick(m_FieldName.text, m_FieldPlayerID.text, Telephone.text, VerifyCode.text);
        }
    }
}
