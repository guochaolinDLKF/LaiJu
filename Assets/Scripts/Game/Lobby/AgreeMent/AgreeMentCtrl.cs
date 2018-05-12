//===================================================
//Author      : DRB
//CreateTime  ：1/3/2018 5:29:35 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgreeMentCtrl : SystemCtrlBase<AgreeMentCtrl>, ISystemCtrl
{

    private UIAgreeMentWindow m_UIAgreeMentWindow;


    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.AgreeMent:              
                OpenAgreeMentView();               
                break;
        }     
    }

    private void OpenAgreeMentView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.AgreeMent, (GameObject go) =>
        {
            m_UIAgreeMentWindow = go.GetComponent<UIAgreeMentWindow>();
            RequestAgreeMent();
        });        
    }


    private void RequestAgreeMent()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/agreement/", OnAgreeMentConfigCallBack, true, "agreement", dic);
    }


    private void OnAgreeMentConfigCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            ShowMessage("提示", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("错误", args.Value.msg);            
                return;
            }
            AgreeMentEntity agreeMent = LitJson.JsonMapper.ToObject<AgreeMentEntity>(args.Value.data.ToJson());
            if (m_UIAgreeMentWindow == null) return;
            m_UIAgreeMentWindow.SetUI(agreeMent.content);
        }
    }
}
