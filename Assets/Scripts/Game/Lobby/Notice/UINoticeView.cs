//===================================================
//Author      : DRB
//CreateTime  ：4/18/2017 9:16:45 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINoticeView : UIWindowViewBase 
{
    [SerializeField]
    private Text m_TextMessage;



    public void SetUI(string msg)
    {
        m_TextMessage.SafeSetText(msg);
    }


    //private void Start()
    //{
    //    Invoke("RequestNotice",30.0f);
    //}

    ///// <summary>
    ///// 请求公告
    ///// </summary>
    //private void RequestNotice()
    //{
    //    Dictionary<string, object> dic = new Dictionary<string, object>();
    //    AccountEntity entity = AccountProxy.Instance.CurrentAccountEntity;
    //    dic["passportId"] = entity.passportId;
    //    dic["token"] = entity.token;
    //    string stamp = TimeUtil.GetTimestampMS().ToString();
    //    dic["sign"] = "notice".GetSign(entity.passportId.ToString(), entity.token, stamp);
    //    NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "notice/" + stamp,OnRequestNoticeCallBack,true,dic);
    //}

    //private void OnRequestNoticeCallBack(NetWorkHttp.CallBackArgs args)
    //{
    //    if (args.HasError)
    //    {

    //    }
    //    else
    //    {
    //        if (args.Value.code < 0)
    //        {
    //            MessageCtrl.Instance.Show("错误", args.Value.msg);
    //            return;
    //        }
    //        string msg = args.Value.data["msg"].ToString();

    //        ShowMsg(msg);
    //    }
    //}

    //private void ShowMsg(string msg)
    //{
    //    m_TextMessage.SafeSetText(msg);
    //}
}
