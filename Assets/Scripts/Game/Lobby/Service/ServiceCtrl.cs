//===================================================
//Author      : DRB
//CreateTime  ：4/7/2017 3:40:21 PM
//Description ：客服模块控制器
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceCtrl : SystemCtrlBase<ServiceCtrl>, ISystemCtrl
{
    private UIServiceWindow m_UIServiceWindow;


    public void OpenView(UIWindowType type)
    {

        switch (type)
        {
            case UIWindowType.Service:
                OpenServiceView();
                break;

        }
    }

    private void OpenServiceView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Service, (GameObject go) =>
        {
            m_UIServiceWindow = go.GetComponent<UIServiceWindow>();
            //List<cfg_serviceEntity> lst = cfg_serviceDBModel.Instance.GetList();

            //List<TransferData> lstData = new List<TransferData>();
            //for (int i = 0; i < lst.Count; ++i)
            //{
            //    TransferData data = new TransferData();
            //    data.SetValue("Key",lst[i].key);
            //    data.SetValue("Value",lst[i].value);
            //    lstData.Add(data);
            //}
            //m_UIServiceWindow.SetUI(lstData, OnBtnInfoClick);

            RequestServiceInfo();
        });
    }

    private void OnBtnInfoClick(string content)
    {
        SDK.Instance.CopyTextToClipboard(content);

        UIViewManager.Instance.ShowTip("已复制到剪切板");
    }

    #region RequestServiceInfo 请求客服信息
    /// <summary>
    /// 请求客服信息
    /// </summary>
    private void RequestServiceInfo()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/service/",OnRequestServerInfoCallBack,true,"service",dic);
    }
    #endregion

    #region OnRequestServerInfoCallBack 请求客服信息回调
    /// <summary>
    /// 请求客服信息回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestServerInfoCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            ShowMessage("提示", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("提示",args.Value.msg);
                return;
            }
            if (args.Value.data == null || args.Value.data.Count == 0) return;
            LitJson.JsonData jsonData = args.Value.data["service"];
            string logo = args.Value.data["logo"].ToString();
            List<TransferData> lstData = new List<TransferData>();
            for (int i = 0; i < jsonData.Count; ++i)
            {
                string key = string.Empty;
                string value = string.Empty;
                if (jsonData[i]["key"] != null)
                {
                    key = jsonData[i]["key"].ToString();
                }
                if (jsonData[i]["value"] != null)
                {
                    value = jsonData[i]["value"].ToString();
                }
                if (value == string.Empty) continue;
                TransferData data = new TransferData();
                data.SetValue("Key", key);
                data.SetValue("Value", value);
                lstData.Add(data);
            }
            m_UIServiceWindow.SetUI(logo, lstData, OnBtnInfoClick);
        }
    }
    #endregion
}
