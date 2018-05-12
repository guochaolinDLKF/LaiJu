//===================================================
//Author      : DRB
//CreateTime  ：4/7/2017 3:40:21 PM
//Description ：设置模块控制器
//===================================================
using System.Collections.Generic;
using UnityEngine;

public class SettingCtrl : SystemCtrlBase<SettingCtrl>, ISystemCtrl
{

    private UISettingWindow m_UISettingWindow;

    #region DicNotificationInterests 注册事件
    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();

        dic[ConstDefine.BtnSettingViewDisband] = OnBtnSettingViewDisbandClick;
        dic[ConstDefine.BtnSettingViewShare] = OnBtnSettingViewShareClick;
        dic[ConstDefine.BtnSettingViewRule] = OnBtnSettingViewRuleClick;
        dic[ConstDefine.BtnSettingViewAudio] = OnBtnSettingViewAudioClick;
        dic[ConstDefine.BtnSettingViewQuit] = OnBtnSettingViewQuitClick;
        dic[ConstDefine.BtnSettingViewBind] = OnBtnSettingViewBindClick;
        dic[ConstDefine.BtnSettingViewMail] = OnBtnSettingViewMailClick;
        dic[ConstDefine.BtnSettingViewLeave] = OnBtnSettingViewLeaveClick;

        return dic;
    }
    #endregion

    #region ISystemCtrl
    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.Setting:
                OpenMenuView();
                break;
            case UIWindowType.MainMenu:
                break;
        }
    }
    #endregion

    #region OpenMenuView 打开菜单界面
    /// <summary>
    /// 打开菜单界面
    /// </summary>
    private void OpenMenuView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Setting, (GameObject go) =>
        {
            m_UISettingWindow = go.GetComponent<UISettingWindow>();
        });
    }
    #endregion

    #region OpenMainMenuView 打开主界面菜单界面
    /// <summary>
    /// 打开主界面菜单界面
    /// </summary>
    private void OpenMainMenuView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Setting, (GameObject go) =>
        {
            m_UISettingWindow = go.GetComponent<UISettingWindow>();
        });
    }
    #endregion

    #region OnBtnSettingViewQuitClick 退出按钮点击
    /// <summary>
    /// 退出按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnSettingViewQuitClick(object[] obj)
    {
        AccountCtrl.Instance.ChangeUser();
    }
    #endregion

    #region OnBtnSettingViewAudioClick 声音按钮点击
    /// <summary>
    /// 声音按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnSettingViewAudioClick(object[] obj)
    {
        UIViewManager.Instance.OpenWindow(UIWindowType.AudioSetting);
    }
    #endregion

    #region OnBtnSettingViewRuleClick 规则按钮点击
    /// <summary>
    /// 规则按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnSettingViewRuleClick(object[] obj)
    {
        UIViewManager.Instance.OpenWindow(UIWindowType.Rule);
    }
    #endregion

    #region OnBtnSettingViewBindClick 绑定按钮点击
    /// <summary>
    /// 绑定按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnSettingViewBindClick(object[] obj)
    {
        UIViewManager.Instance.OpenWindow(UIWindowType.Bind);
    }
    #endregion

    #region OnBtnSettingViewMailClick 邮件按钮点击
    /// <summary>
    /// 邮件按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnSettingViewMailClick(object[] obj)
    {
        UIViewManager.Instance.OpenWindow(UIWindowType.Mail);
    }
    #endregion



    #region OnBtnSettingViewDisbandClick 解散按钮点击
    /// <summary>
    /// 解散按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnSettingViewDisbandClick(object[] obj)
    {
        GameCtrl.Instance.DisbandRoom();
    }
    #endregion

    #region OnBtnSettingViewShareClick 分享按钮点击
    /// <summary> 
    /// 分享按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnSettingViewShareClick(object[] obj)
    {
        ShareCtrl.Instance.ShareURL(ShareType.InGame);
    }
    #endregion

    #region OnBtnSettingViewExitClick 离开按钮点击
    /// <summary>
    /// 离开按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnSettingViewLeaveClick(object[] obj)
    {
        GameCtrl.Instance.QuitRoom();        
    }
    #endregion
}
