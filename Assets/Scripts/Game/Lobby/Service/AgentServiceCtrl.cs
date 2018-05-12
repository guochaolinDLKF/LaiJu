//===================================================
//Author      : DRB
//CreateTime  ：1/15/2018 9:48:18 AM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentServiceCtrl : SystemCtrlBase<AgentServiceCtrl>, ISystemCtrl
{

    private UIAgentServiceWindow m_UIAgentServiceWindow;

    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.AgentService:
                OpenAgentServiceView();
                break;
        }
    }


    private void OpenAgentServiceView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.AgentService, (GameObject go) =>
        {
            m_UIAgentServiceWindow = go.GetComponent<UIAgentServiceWindow>();
            RequestAgentService();
        });
    }


    private void RequestAgentService()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/agent_show_list/", OnAgentServiceConfigCallBack, true, "agent_show_list", dic);
    }


    private void OnAgentServiceConfigCallBack(NetWorkHttp.CallBackArgs args)
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
            List<TransferData> lstData = new List<TransferData>();     
            if (args.Value.data == null || args.Value.data.Count == 0) return;
            for (int i = 0; i < args.Value.data.Count; i++)
            {
                LitJson.JsonData jsonData = args.Value.data[i];
                string id = jsonData["id"].ToString();//ID     
                string textWX = jsonData["wx"].ToString();//微信号
                string phoneNumber = jsonData["tel"].ToString();//电话
                string remark = jsonData["remark"].ToString();//地区
                string status = jsonData["status"].ToString();//状态
                TransferData data = new TransferData();
                data.SetValue("wx",textWX);
                data.SetValue("tel",phoneNumber);
                lstData.Add(data);                
            }
            m_UIAgentServiceWindow.SetUI(lstData, OnBtnInfoClick);         
        }
    }

    private void OnBtnInfoClick(string content)
    {
        SDK.Instance.CopyTextToClipboard(content);

        UIViewManager.Instance.ShowTip("已复制到剪切板");
    }
}
