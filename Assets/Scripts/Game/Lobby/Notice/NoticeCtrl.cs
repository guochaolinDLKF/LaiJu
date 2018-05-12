//===================================================
//Author      : DRB
//CreateTime  ：4/18/2017 9:14:15 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;


public class NoticeCtrl : SystemCtrlBase<NoticeCtrl>, ISystemCtrl
{
    private UINoticeView m_UINoticeView;

    private UIInformView m_UIInformView;

    private UIMailView m_UIMailView;


    private bool m_isBusy;

    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        dic.Add(ConstDefine.BtnMailViewNotice, OnBtnNoticeClick);
        dic.Add(ConstDefine.BtnMailViewMail, OnBtnMailClick);
        dic.Add("RawImage", OnBtnTextureClick); //注册图片按钮


        return dic;
    }

    public void OpenView(UIWindowType type)
    {
        //OpenView函数调用了OpenMailView
        switch (type)
        {
            case UIWindowType.Notice:   //小公告窗口
                Debug.Log("sfsdf++++++++++++++");
                OpenMailView();
                break;
            case UIWindowType.Mail:   //公告窗口
                Debug.Log("sfsdf+++++++++========================+++++");
                OpenMailView();
                break;
        }
    }
    /// <summary>
    /// 图片点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnTextureClick(object[] obj) 
    {
        if (!String.IsNullOrEmpty((string) obj[0]))
        {
            System.Diagnostics.Process.Start((string)obj[0]);//打开外部链接的API函数
        }
       
    }
    #region OnBtnNoticeClick 公告按钮点击
    /// <summary>
    /// 公告按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnNoticeClick(object[] obj)
    {
        //if (InformProxy.Instance.UnreadMailCount > 0 || InformProxy.Instance.AllMails == null)
        //{

        //}
        //RequestNotice();
        if (!string.IsNullOrEmpty(InformProxy.Instance.Notice))
        {
            m_UIMailView.SetNotice(InformProxy.Instance.Notice);
        }
    }
    #endregion

    #region OnBtnMailClick 邮件按钮点击
    /// <summary>
    /// 邮件按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnMailClick(object[] obj)
    {
        if (InformProxy.Instance.UnreadMailCount > 0 || InformProxy.Instance.AllMails == null)
        {
           //RequestMail();
            Debug.Log("***************消息***********************");
        }
        //else
        //{
        //    if (m_UIMailView != null)
        //    {
        //        m_UIMailView.SetMail(InformProxy.Instance.AllMails);
        //    }
        //    Debug.Log("**************************消息********");
        //}
    }
    #endregion

    #region OpenNoticeView 打开公告窗口
    /// <summary>
    /// 打开公告窗口
    /// </summary>
    public void OpenNoticeView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Notice, (GameObject go) =>
        {
            string notice = InformProxy.Instance.Notice;
            m_UINoticeView = go.GetComponent<UINoticeView>();
            m_UINoticeView.SetUI(notice);
        });
    }
    #endregion


    #region OpenMailView 打开邮件视图
    /// <summary>
    /// 打开邮件视图
    /// </summary>
    private void OpenMailView()
    {

        ////OpenMailView 再次调用了RequestNotice
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Mail, (GameObject go) =>
        {
            m_UIMailView = go.GetComponent<UIMailView>();
            //m_UIMailView.SetBtnLeft();
             RequestNotice();  //多余调用
        });
    }
    #endregion


    #region ShowInform 显示通知
    /// <summary>
    /// 显示通知
    /// </summary>
    /// <param name="message"></param>
    public void ShowInform(string message) // 显示通知  
    {
        if (UIViewManager.Instance.CurrentUIScene == null || UIViewManager.Instance.CurrentUIScene.Container_Center == null) return;
        if (m_UIInformView == null)
        {
            string windowName = "pan_inform";
            string path = string.Format("download/{0}/prefab/uiprefab/uiwindows/{1}.drb", ConstDefine.GAME_NAME, windowName);
            AssetBundleManager.Instance.LoadOrDownload(path, windowName, (GameObject go) =>
             {
                 m_UIInformView = UnityEngine.Object.Instantiate(go).GetComponent<UIInformView>();
                 m_UIInformView.gameObject.SetParent(UIViewManager.Instance.CurrentUIScene.Container_Center);
                 //Canvas canvas = m_UIInformView.GetComponentInChildren<Canvas>();
                 //canvas.overrideSorting = true;
                 //canvas.sortingOrder = 10;
                 m_UIInformView.Show(message);
             });
        }
        else
        {
            m_UIInformView.Show(message);
        }
    }
    #endregion


    #region RequestNotice 获取公告
    /// <summary>
    /// 获取公告
    /// </summary>
    private void RequestNotice()
    {
        if (m_isBusy) return;
        m_isBusy = true;
        Debug.Log("******************");
        Dictionary<string, object> dic = new Dictionary<string, object>();
        AccountEntity entity = AccountProxy.Instance.CurrentAccountEntity;
        dic["passportId"] = entity.passportId;
        dic["token"] = entity.token;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/noticePlus/", OnRequestNoticeCallBack, true, "noticePlus", dic);
    }
    #endregion

    #region OnRequestNoticeCallBack 获取公告回调
    /// <summary>
    /// 获取公告回调
    /// </summary>
    /// <param name="args"></param>

    private void OnRequestNoticeCallBack(NetWorkHttp.CallBackArgs args)
    {
        //RequestNotice的回调又调用了OpenView

        m_isBusy = false;
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

            List<NoticeEntity> lst = LitJson.JsonMapper.ToObject<List<NoticeEntity>>(args.Value.data.ToJson());
           // UIViewManager.Instance.OpenWindow(UIWindowType.Notice);           
           AppDebug.Log("服务器给的公告数据："+args.Value.data.ToJson());
            if (m_UIMailView != null)
            {
                //ShowMessage("提示", "这里需要打开");    
                m_UIMailView.SetBtnLeft(lst);//在公告面板初始化时，生成按钮
            }
        }
    }
    #endregion



    #region RequestMail 请求邮件信息
    /// <summary>
    /// 请求邮件信息
    /// </summary>
    public  void RequestMail()
    {
        if (m_isBusy) return;
        m_isBusy = true;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrMail, OnGetMailCallBack, true, ConstDefine.HTTPFuncMail, dic);
    }
    #endregion



    #region OnGetMailCallBack 请求邮件信息回调
    /// <summary>
    /// 请求邮件信息回调
    /// </summary>
    /// <param name="args"></param>
    private void OnGetMailCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        if (args.HasError)
        {
            ShowMessage("错误", args.ErrorMsg);
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("错误", args.Value.msg);
                return;
            }

            List<MailEntity> lst = LitJson.JsonMapper.ToObject<List<MailEntity>>(args.Value.data.ToJson());
            InformProxy.Instance.SetMail(lst);
           // Debug.Log("请求的到数据--"+ args.Value.data.ToJson());
            if (m_UIMailView != null)
            {
                m_UIMailView.SetMail(lst);
            }
        }
    }
    #endregion
}
