//===================================================
//Author      : DRB
//CreateTime  ：3/7/2017 2:30:24 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoginView : UIWindowViewBase
{
    public InputField TelNum;

    public InputField IdentifyingCode;

    [SerializeField]
    private Button m_BtnWXLogin;
    [SerializeField]
    private Button m_BtnGuestLogin;
    [SerializeField]
    private Toggle m_ToggleAgree;

    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, ModelDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add("OnIsInstallWeChatChanged", OnIsInstallWeChatChanged);
        return dic;
    }

    private void OnIsInstallWeChatChanged(TransferData data)
    {
        bool isInstall = data.GetValue<bool>("IsInstallWeChat");
        m_BtnWXLogin.gameObject.SetActive(SystemProxy.Instance.IsInstallWeChat);
        m_BtnGuestLogin.gameObject.SetActive(!SystemProxy.Instance.IsInstallWeChat);
    }

    protected override void OnStart()
    {
        base.OnStart();
        m_BtnWXLogin.gameObject.SetActive(SystemProxy.Instance.IsInstallWeChat);
        m_BtnGuestLogin.gameObject.SetActive(!SystemProxy.Instance.IsInstallWeChat);
        if (m_ToggleAgree != null)
        {
            m_ToggleAgree.onValueChanged.AddListener(OnToggleAgreeChanged);
        }
    }

    private void OnToggleAgreeChanged(bool isSelect)
    {
        SendNotification("OnAgreementValueChanged");
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case ConstDefine.BtnGetIdentifyingCode:
                SendNotification(ConstDefine.BtnGetIdentifyingCode);
                break;
            case ConstDefine.BtnLogin:
                SendNotification(ConstDefine.BtnLogin);
                break;
            case "btnGuestLogin":
                SendNotification("btnGuestLogin");
                break;
            case "btnWXLogin":
                SendNotification("btnWXLogin");
                break;
            case "btnUserAgreement":
                SendNotification("btnUserAgreement");
                break;
        }
    }
}
