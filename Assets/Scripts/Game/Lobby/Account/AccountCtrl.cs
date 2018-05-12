//===================================================
//Author      : DRB
//CreateTime  ：3/7/2017 2:40:35 PM
//Description ：账户模块控制器
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AccountCtrl : SystemCtrlBase<AccountCtrl>, ISystemCtrl
{
    #region Variable
    private UILoginView m_UILoginView;//登录窗口

    private UIPlayerInfoView m_UIPlayerInfoView;//角色信息窗口

    private UIBindView m_UIBindView;//绑定手机号窗口

    private UIInviteView m_UIInviteView;

    private int m_BindCode;

    private bool m_isBusy;//是否操作繁忙

    private bool m_isAgree = true;//同意服务条款
    #endregion

    #region DicNotificationInterests 注册事件
    /// <summary>
    /// 注册事件
    /// </summary>
    /// <returns></returns>
    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        dic.Add("btnGuestLogin", OnBtnGuestLoginClick);
        dic.Add("btnWXLogin", OnBtnWXLoginClick);
        dic.Add("btnBindViewGetVerifyCode", OnBtnGetVerifyCodeClick);
        dic.Add("btnBindViewValidate", OnValidateClick);
        dic.Add("btnBindViewUnBind", OnUnBindClick);

        dic.Add("btnPlayerInfoViewPlayerRecharge", OnBtnPlayerInfoViewPlayerRecharge);
        dic.Add("btnPlayerInfoViewNext", OnBtnPlayerInfoViewNext);//玩家信息界面下一步按钮
        dic.Add("btnPlayerInfoViewRecharge", OnBtnPlayerInfoViewRecharge);//玩家信息界面充值按钮
        dic.Add("btnPlayerInfoViewBack", OnBtnPlayerInfoViewBackClick);//玩家信息界面返回按钮
        dic.Add("btnMainViewHead", OnBtnMainViewHead);//主界面头像按钮
        dic.Add("btnPlayerInfoViewRechargeRecord", OnBtnPlayerInfoViewRechargeRecord);//玩家信息界面充值记录按钮
        dic.Add("btnPlayerInfoViewQuery", OnBtnPlayerInfoViewQuery);//玩家信息界面查询按钮

        dic.Add("btnUserAgreement", OnUserAgreementClick);//用户使用协议按钮点击
        dic.Add("OnAgreementValueChanged", OnAgreementValueChanged);

        dic.Add("btnInviteViewOk", OnBtnInviteViewOk);//邀请码窗口确认按钮

        return dic;
    }
    #endregion

    #region OpenView
    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.Login:
                OpenLoginView();
                break;
            case UIWindowType.PlayerInfo:
                OpenPlayerInfoView();
                break;
            case UIWindowType.Bind:
                OpenTelBind();
                break;
            case UIWindowType.Invite:
                OpenInviteView();
                break;
        }
    }
    #endregion

    #region OpenLoginView 打开登录窗口
    /// <summary>
    /// 打开登录窗口
    /// </summary>
    private void OpenLoginView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Login, (GameObject go) =>
        {
            m_UILoginView = go.GetComponent<UILoginView>();
        });
    }
    #endregion

    #region OpenPlayerInfoView 打开角色信息窗口
    /// <summary>
    /// 打开角色信息窗口
    /// </summary>
    private void OpenPlayerInfoView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.PlayerInfo, (GameObject go) =>
        {
            m_UIPlayerInfoView = go.GetComponent<UIPlayerInfoView>();
            m_UIPlayerInfoView.SetCards(AccountProxy.Instance.CurrentAccountEntity.cards);
        });
        //m_UIPlayerInfoView = UIViewUtil.Instance.OpenWindow(UIWindowType.PlayerInfo).GetComponent<UIPlayerInfoView>();
        //m_UIPlayerInfoView.SetCards(AccountProxy.Instance.CurrentAccountEntity.cards);
    }
    #endregion

    #region OpenTelBind 打开手机绑定窗口
    /// <summary>
    /// 打开手机绑定窗口
    /// </summary>
    private void OpenTelBind()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Bind, (GameObject go) =>
        {
            m_UIBindView = go.GetComponent<UIBindView>();
            m_UIBindView.SetUI(AccountProxy.Instance.CurrentAccountEntity.phone);
        });
    }
    #endregion

    #region OpenInviteView 打开邀请码窗口
    /// <summary>
    /// 打开邀请码窗口
    /// </summary>
    private void OpenInviteView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Invite, (GameObject go) =>
        {
            m_UIInviteView = go.GetComponent<UIInviteView>();
            m_UIInviteView.SetUI(AccountProxy.Instance.CurrentAccountEntity.codebind);
        });
    }
    #endregion

    #region 登录相关
    #region QuickLogin 快速登陆
    /// <summary>
    /// 快速登录
    /// </summary>
    public void QuickLogin()
    {
        if (!PlayerPrefs.HasKey("passportId")) return;
        AccountLogin(PlayerPrefs.GetInt("passportId"), PlayerPrefs.GetString("token"));
    }
    #endregion

    #region OnBtnGuestLoginClick 游客登陆按钮点击
    /// <summary>
    /// 游客登陆按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnGuestLoginClick(object[] obj)
    {
        if (!m_isAgree)
        {
            ShowMessage("提示","请确认及同意服务条款，即可进入游戏");
            return;
        }
        GuestLogin();
    }
    #endregion

    #region OnBtnWXLoginClick 微信登陆按钮点击
    /// <summary>
    /// 微信登陆按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnWXLoginClick(object[] obj)
    {
        if (!m_isAgree)
        {
            ShowMessage("提示", "请确认及同意服务条款，即可进入游戏");
            return;
        }
        WXLogin();
    }
    #endregion

    #region 微信登陆相关
    #region WXLogin 微信登录
    /// <summary>
    /// 微信登录
    /// </summary>
    /// <param name="isQuick"></param>
    public void WXLogin()
    {
        if (!PlayerPrefs.HasKey("passportId"))
        {
            SDK.Instance.WXLogin(OnWXLoginCallBack);
        }
        else
        {
            AccountLogin(PlayerPrefs.GetInt("passportId"), PlayerPrefs.GetString("token"));
        }
    }
    #endregion

    #region OnWXLoginCallBack 微信登陆回调（平台返回）
    /// <summary>
    /// 微信登陆回调（平台返回）
    /// </summary>
    /// <param name="code"></param>
    private void OnWXLoginCallBack(string code)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["code"] = code;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrWXCheck, OnWXLoginCallBack, true, ConstDefine.HTTPFuncWXCheck, dic);
    }
    #endregion

    #region WXLogin 微信登陆
    /// <summary>
    /// 微信登陆
    /// </summary>
    /// <param name="token"></param>
    private void WXLogin(int passportId, string token)
    {
        if (m_isBusy) return;
        m_isBusy = true;
        UIViewManager.Instance.ShowWait();
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = passportId;
        dic["token"] = token;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrWXLogin, OnWXLoginCallBack, true, ConstDefine.HTTPFuncWXLogin, dic);
    }
    #endregion

    #region OnWXLoginCallBack 微信登陆回调（服务器返回）
    /// <summary>
    /// 微信登陆回调（服务器返回）
    /// </summary>
    /// <param name="args"></param>
    private void OnWXLoginCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        UIViewManager.Instance.CloseWait();
        if (args.HasError)
        {
            ShowMessage("错误","网络连接失败");
            if (PlayerPrefs.HasKey("passportId"))
            {
                PlayerPrefs.DeleteKey("passportId");
                PlayerPrefs.DeleteKey("token");
            }
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("错误", args.Value.msg);
                if (PlayerPrefs.HasKey("passportId"))
                {
                    PlayerPrefs.DeleteKey("passportId");
                    PlayerPrefs.DeleteKey("token");
                }
                return;
            }
            AccountEntity entity = LitJson.JsonMapper.ToObject<AccountEntity>(LitJson.JsonMapper.ToJson(args.Value.data));
            PlayerPrefs.SetInt("passportId", entity.passportId);
            PlayerPrefs.SetString("token", entity.token);
            AccountProxy.Instance.CurrentAccountEntity = entity;

            GameCtrl.Instance.RequestGameServer();
        }
    }
    #endregion
    #endregion

    #region GuestLogin 游客登录
    /// <summary>
    /// 游客登录
    /// </summary>
    public void GuestLogin()
    {
        if (m_isBusy) return;
        m_isBusy = true;
        UIViewManager.Instance.ShowWait();
        Dictionary<string, object> dic = new Dictionary<string, object>();
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrGuest, OnGuestCallBack, true, ConstDefine.HTTPFuncGuest, dic);
    }
    #endregion

    #region OnGuestCallBack 游客登陆回调
    /// <summary>
    /// 游客登陆回调
    /// </summary>
    /// <param name="args"></param>
    private void OnGuestCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        UIViewManager.Instance.CloseWait();
        if (args.HasError)
        {
            ShowMessage("错误", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("错误",args.Value.msg);
                return;
            }

            int passportId = args.Value.data["passportId"].ToString().ToInt();
            string token = args.Value.data["token"].ToString();
            AccountLogin(passportId, token);
        }
    }
    #endregion

    #region AccountLogin 账号登陆
    /// <summary>
    /// 账号登陆
    /// </summary>
    /// <param name="passportId"></param>
    /// <param name="token"></param>
    public void AccountLogin(int passportId, string token)
    {
        if (m_isBusy) return;
        m_isBusy = true;
        UIViewManager.Instance.ShowWait();
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = passportId;
        dic["token"] = token;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrRelogin, OnLoginCallBack, true, ConstDefine.HTTPFuncRelogin, dic);
    }
    #endregion

    #region OnLoginCallBack 账号登录回调
    /// <summary>
    /// 账号登录回调
    /// </summary>
    /// <param name="args"></param>
    private void OnLoginCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        UIViewManager.Instance.CloseWait();
        if (args.HasError)
        {
            ShowMessage("错误提示", args.ErrorMsg, MessageViewType.Ok);
            if (PlayerPrefs.HasKey("passportId"))
            {
                PlayerPrefs.DeleteKey("passportId");
                PlayerPrefs.DeleteKey("token");
            }
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("错误提示", args.Value.msg, MessageViewType.Ok);
                if (PlayerPrefs.HasKey("passportId"))
                {
                    PlayerPrefs.DeleteKey("passportId");
                    PlayerPrefs.DeleteKey("token");
                }
                return;
            }
            AccountProxy.Instance.CurrentAccountEntity = LitJson.JsonMapper.ToObject<AccountEntity>(LitJson.JsonMapper.ToJson(args.Value.data));
            PlayerPrefs.SetInt("passportId", AccountProxy.Instance.CurrentAccountEntity.passportId);
            PlayerPrefs.SetString("token", AccountProxy.Instance.CurrentAccountEntity.token);

            GameCtrl.Instance.RequestGameServer();
        }
    }
    #endregion
    #endregion

    #region RefreshCards 刷新房卡
    /// <summary>
    /// 刷新房卡
    /// </summary>
    public void RequestCards()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        AccountEntity entity = AccountProxy.Instance.CurrentAccountEntity;
        dic["passportId"] = entity.passportId;
        dic["token"] = entity.token;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrCards, OnRequestCardsCallBack, true, ConstDefine.HTTPFuncCards, dic);
    }
    #endregion

    #region OnRequestCardsCallBack 请求房卡回调
    /// <summary>
    /// 请求房卡数量回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestCardsCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            //UIViewManager.Instance.ShowMessage("错误提示", "网络连接失败", MessageViewType.Ok);
        }
        else
        {
            if (args.Value.code < 0)
            {
                UIViewManager.Instance.ShowMessage("错误提示", args.Value.msg, MessageViewType.Ok);
                return;
            }

            int cards = args.Value.data["cards"].ToString().ToInt();
            AccountProxy.Instance.SetCards(cards);
        }
    }
    #endregion

    #region ChangeUser 切换用户
    /// <summary>
    /// 切换用户
    /// </summary>
    public void ChangeUser()
    {
        ShowMessage("提示", "是否要切换用户", MessageViewType.OkAndCancel, QuitToLogin);
    }

    public void QuitToLogin()
    {
        PlayerPrefs.DeleteKey("passportId");
        PlayerPrefs.DeleteKey("token");
        AccountProxy.Instance.CurrentAccountEntity = null;
        if (SceneMgr.Instance.CurrentSceneType != SceneType.Login)
        {
            SceneMgr.Instance.LoadScene(SceneType.Login);
        }
    }
    #endregion

    #region 绑定手机相关

    #region OnBtnGetVerifyCodeClick 获取验证码按钮点击
    /// <summary>
    /// 获取验证码按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnGetVerifyCodeClick(object[] obj)
    {
        if (string.IsNullOrEmpty(AccountProxy.Instance.CurrentAccountEntity.phone))
        {
            if (string.IsNullOrEmpty(m_UIBindView.Telephone.text))
            {
                ShowMessage("提示", "请输入手机号", MessageViewType.Ok);
                return;
            }
            if (!m_UIBindView.Telephone.text.Regex(ConstDefine.TelNumRegex))
            {
                ShowMessage("错误提示", "请输入正确的手机号码", MessageViewType.Ok);
                return;
            }
            BindPhoneGetVerifyCode(m_UIBindView.Telephone.text);
        }
        else
        {
            UnBindPhoneGetVerifyCode();
        }
    }
    #endregion

    #region OnValidateClick 验证按钮点击
    /// <summary>
    /// 验证按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnValidateClick(object[] obj)
    {
        if (string.IsNullOrEmpty(m_UIBindView.Telephone.text))
        {
            ShowMessage("提示", "请输入手机号", MessageViewType.Ok);
            return;
        }
        if (!m_UIBindView.Telephone.text.Regex(ConstDefine.TelNumRegex))
        {
            ShowMessage("错误提示", "请输入正确的手机号码", MessageViewType.Ok);
            return;
        }
        if (string.IsNullOrEmpty(m_UIBindView.VerifyCode.text))
        {
            ShowMessage("提示", "请输入验证码", MessageViewType.Ok);
            return;
        }
        BindPhone(m_UIBindView.Telephone.text, m_UIBindView.VerifyCode.text);
    }
    #endregion

    #region OnUnBindClick 解绑按钮点击
    /// <summary>
    /// 解绑按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnUnBindClick(object[] obj)
    {
        if (string.IsNullOrEmpty(m_UIBindView.VerifyCode.text))
        {
            ShowMessage("提示", "请输入验证码", MessageViewType.Ok);
            return;
        }
        UnBindPhone(m_UIBindView.VerifyCode.text);
    }
    #endregion

    #region BindPhoneGetVerifyCode 绑定手机获取验证码
    /// <summary>
    /// 绑定手机获取验证码
    /// </summary>
    /// <param name="telephoneNumber"></param>
    public void BindPhoneGetVerifyCode(string telephoneNumber)
    {
        if (m_isBusy) return;
        m_isBusy = true;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["phone"] = telephoneNumber;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrBindCode, OnBindPhoneGetVerifyCodeCallBack, true, ConstDefine.HTTPFuncBindCode, dic);
    }
    #endregion

    #region OnBindPhoneGetVerifyCodeCallBack 绑定手机获取验证码回调
    /// <summary>
    /// 绑定手机获取验证码回调
    /// </summary>
    /// <param name="args"></param>
    private void OnBindPhoneGetVerifyCodeCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        if (args.HasError)
        {
            ShowMessage("错误", "网络连接失败", MessageViewType.Ok);
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("错误提示", args.Value.msg, MessageViewType.Ok);
                return;
            }

            ShowMessage("提示", "验证码已发送");
        }
    }
    #endregion

    #region BindPhone 绑定手机号
    /// <summary>
    /// 绑定手机号
    /// </summary>
    /// <param name="telePhoneNumber"></param>
    /// <param name="verifyCode"></param>
    public void BindPhone(string telePhoneNumber, string verifyCode)
    {
        if (m_isBusy) return;
        m_isBusy = true;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["phone"] = telePhoneNumber;
        dic["verifyCode"] = verifyCode;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrBindPhone, OnBindPhoneCallBack, true, ConstDefine.HTTPFuncBindPhone, dic);
    }
    #endregion

    #region OnBindPhoneCallBack 绑定手机号回调
    /// <summary>
    /// 绑定手机号回调
    /// </summary>
    /// <param name="args"></param>
    private void OnBindPhoneCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        if (args.HasError)
        {
            ShowMessage("错误", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("错误", args.Value.msg);
                return;
            }

            int cards = args.Value.data["cards"].ToString().ToInt();
            string phone = args.Value.data["phone"].ToString();
            AccountProxy.Instance.SetCards(cards);
            AccountProxy.Instance.SetPhone(phone);
            if (m_UIBindView != null)
            {
                m_UIBindView.SetUI(phone);
            }
            ShowMessage("成功", "绑定成功");
        }
    }
    #endregion

    #region UnBindPhoneGetVerifyCode 解绑手机号获取验证码
    /// <summary>
    /// 解绑手机号获取验证码
    /// </summary>
    private void UnBindPhoneGetVerifyCode()
    {
        if (m_isBusy) return;
        m_isBusy = true;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrVerifyCode, UnBindPhoneGetVerifyCodeCallBack, true, ConstDefine.HTTPFuncVerifyCode, dic);
    }
    #endregion

    #region UnBindPhoneGetVerifyCodeCallBack 解绑手机号获取验证码回调
    /// <summary>
    /// 解绑手机号获取验证码回调
    /// </summary>
    /// <param name="args"></param>
    private void UnBindPhoneGetVerifyCodeCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        if (args.HasError)
        {
            ShowMessage("错误", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("错误", args.Value.msg);
                return;
            }

            ShowMessage("提示", "验证码已发送");
        }
    }
    #endregion

    #region UnBindPhone 解绑手机
    /// <summary>
    /// 解绑手机
    /// </summary>
    private void UnBindPhone(string verifyCode)
    {
        if (m_isBusy) return;
        m_isBusy = true;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["verifyCode"] = verifyCode;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrUnBindPhone, OnUnBindPhoneCallBack, true, ConstDefine.HTTPFuncUnBindPhone, dic);
    }
    #endregion

    #region OnUnBindPhoneCallBack 解绑手机回调
    /// <summary>
    /// 解绑手机回调
    /// </summary>
    /// <param name="args"></param>
    private void OnUnBindPhoneCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        if (args.HasError)
        {
            ShowMessage("错误", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("错误", args.Value.msg);
                return;
            }
            AccountProxy.Instance.SetPhone(string.Empty);
            if (m_UIBindView != null)
            {
                m_UIBindView.SetUI(string.Empty);
            }
            ShowMessage("成功", "解绑成功");
        }
    }
    #endregion
    #endregion

    #region 绑定邀请码相关
    #region OnBtnInviteViewOk 邀请码窗口确认按钮点击
    /// <summary>
    /// 邀请码窗口确认按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnInviteViewOk(object[] obj)
    {
        int inviteCode = (int)obj[0];

        BindInviteCode(inviteCode);
    }
    #endregion

    #region BindInviteCode 绑定邀请码
    /// <summary>
    /// 绑定邀请码
    /// </summary>
    /// <param name="parentId"></param>
    private void BindInviteCode(int parentId)
    {
        if (m_isBusy) return;
        m_isBusy = true;
        m_BindCode = parentId;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["parentId"] = parentId;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrCodeBind, OnBindInviteCodeCallBack, true, ConstDefine.HTTPFuncCodeBind, dic);
    }
    #endregion

    #region OnBindInviteCodeCallBack 绑定邀请码回调
    /// <summary>
    /// 绑定邀请码回调
    /// </summary>
    /// <param name="args"></param>
    private void OnBindInviteCodeCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        if (args.HasError)
        {
            ShowMessage("错误", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("提示", args.Value.msg);
                return;
            }

            if (m_UIInviteView != null)
            {
                AccountProxy.Instance.CurrentAccountEntity.codebind = m_BindCode;
                m_UIInviteView.SetUI(m_BindCode);
            }

            ShowMessage("提示", "绑定成功");
        }
    }
    #endregion
    #endregion

    #region OnBtnMainViewHead 主界面头像按钮点击
    /// <summary>
    /// 主界面头像按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnMainViewHead(object[] obj)
    {
        if (AccountProxy.Instance.CurrentAccountEntity.identity == 0) return;
        UIViewManager.Instance.OpenWindow(UIWindowType.PlayerInfo);
    }
    #endregion

    #region OnBtnPlayerInfoViewNext 玩家信息界面下一步按钮点击
    /// <summary>
    /// 玩家信息界面下一步按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPlayerInfoViewNext(object[] obj)
    {
        if (m_UIPlayerInfoView.TargetPlayerId == 0)
        {
            ShowMessage("提示", "请输入玩家Id");
            return;
        }
        if (m_UIPlayerInfoView.TargetPlayerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
        {
            ShowMessage("提示","不能给自己充值");
            return;
        }
        AgentRequestPlayerInfo(m_UIPlayerInfoView.TargetPlayerId);
    }
    #endregion

    #region AgentRequestPlayerInfo 代理查询角色信息
    /// <summary>
    /// 代理查询角色信息
    /// </summary>
    /// <param name="playerId"></param>
    private void AgentRequestPlayerInfo(int playerId)
    {
        if (m_isBusy) return;
        m_isBusy = true;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["toId"] = playerId;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrPlayerInfo, OnAgentRequestPlayerInfoCallBack,true,ConstDefine.HTTPFuncPlayerInfo,dic);
    }
    #endregion

    #region OnAgentRequestPlayerInfoCallBack 查询角色信息回调
    /// <summary>
    /// 查询角色信息回调
    /// </summary>
    /// <param name="playerEntity"></param>
    private void OnAgentRequestPlayerInfoCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        if (args.HasError)
        {
            ShowMessage("错误","网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("错误",args.Value.msg);
                return;
            }

            int playerId = args.Value.data["id"].ToString().ToInt();
            string nickName = args.Value.data["nickname"].ToString();
            int cardsCount = args.Value.data["cards"].ToString().ToInt();

            if (m_UIPlayerInfoView != null)
            {
                m_UIPlayerInfoView.SetPlayerInfo(AccountProxy.Instance.CurrentAccountEntity.passportId, playerId,nickName,cardsCount);
            }
        }
    }
    #endregion

    #region OnBtnPlayerInfoViewRecharge 玩家信息界面充值按钮点击
    /// <summary>
    /// 玩家信息界面充值按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPlayerInfoViewRecharge(object[] obj)
    {
        if (m_UIPlayerInfoView.RechargeCount == 0)
        {
            ShowMessage("提示","请输入房卡数量");
            return;
        }
        if (m_UIPlayerInfoView.RechargeCount > AccountProxy.Instance.CurrentAccountEntity.cards)
        {
            ShowMessage("提示","没有这么多房卡");
            return;
        }
        AgentRecharge(m_UIPlayerInfoView.TargetPlayerId,m_UIPlayerInfoView.RechargeCount);
    }
    #endregion

    #region AgentRecharge 代理充值
    /// <summary>
    /// 代理充值
    /// </summary>
    /// <param name="targetPlayerId"></param>
    /// <param name="amount"></param>
    private void AgentRecharge(int targetPlayerId,int amount)
    {
        if (m_isBusy) return;
        m_isBusy = true;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["toId"] = targetPlayerId;
        dic["amount"] = amount;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrRecharge,OnAgentRechargeCallBack,true,ConstDefine.HTTPFuncRecharge,dic);
    }
    #endregion

    #region OnAgentRechargeCallBack 代理充值回调
    /// <summary>
    /// 代理充值回调
    /// </summary>
    /// <param name="args"></param>
    private void OnAgentRechargeCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        if (args.HasError)
        {
            ShowMessage("错误", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("错误",args.Value.msg);
                return;
            }
            int playerCardsCount = args.Value.data["self"].ToString().ToInt();
            int targetCardsCount = args.Value.data["cards"].ToString().ToInt();
            AccountProxy.Instance.SetCards(playerCardsCount);
            if (m_UIPlayerInfoView != null)
            {
                m_UIPlayerInfoView.SetPlayerInfo(playerCardsCount,targetCardsCount);
            }
            ShowMessage("提示","充值成功");
        }
    }
    #endregion

    #region OnBtnPlayerInfoViewBackClick玩家信息界面返回按钮点击
    /// <summary>
    /// 玩家信息界面返回按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPlayerInfoViewBackClick(object[] obj)
    {
        m_UIPlayerInfoView.SetCards(AccountProxy.Instance.CurrentAccountEntity.cards);
    }
    #endregion

    #region OnBtnPlayerInfoViewPlayerRecharge 玩家信息界面玩家充值按钮点击
    /// <summary>
    /// 玩家信息界面玩家充值按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPlayerInfoViewPlayerRecharge(object[] obj)
    {
        m_UIPlayerInfoView.SetCards(AccountProxy.Instance.CurrentAccountEntity.cards);
    }
    #endregion

    #region OnBtnPlayerInfoViewRechargeRecord 玩家信息界面充值记录按钮点击
    /// <summary>
    /// 玩家信息界面充值记录按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPlayerInfoViewRechargeRecord(object[] obj)
    {
        m_UIPlayerInfoView.SetRechargeRecord(null);
    }
    #endregion

    #region OnBtnPlayerInfoViewQuery 玩家信息界面查询按钮点击
    /// <summary>
    /// 玩家信息界面查询按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPlayerInfoViewQuery(object[] obj)
    {
        RequestRechargeRecord(m_UIPlayerInfoView.UIPageRechargeRecord.InputUserId.text.ToInt(),
            m_UIPlayerInfoView.UIPageRechargeRecord.StartDateTime,
            m_UIPlayerInfoView.UIPageRechargeRecord.EndDateTime);
    }
    #endregion

    #region RequestRechargeRecord 获取充值记录
    /// <summary>
    /// 获取充值记录
    /// </summary>
    private void RequestRechargeRecord(int id,string beginTime,string endTime)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["toId"] = id;
        dic["begin_time"] = beginTime;
        dic["end_time"] = endTime;
        dic["limit"] = 500;
        dic["page"] = 1;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrLog, OnRequestRechargeRecordCallBack, true, ConstDefine.HTTPFuncLog, dic);
    }
    #endregion

    #region OnRequestRechargeRecordCallBack 获取充值记录回调
    /// <summary>
    /// 获取充值记录回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestRechargeRecordCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            ShowMessage("错误", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("提示", args.Value.msg);
                return;
            }

            HttpDataPage<List<RechargeRecordEntity>> page = LitJson.JsonMapper.ToObject<HttpDataPage<List<RechargeRecordEntity>>>(args.Value.data.ToJson());

            if (m_UIPlayerInfoView != null)
            {
                m_UIPlayerInfoView.SetRechargeRecord(page.data);
            }
        }
    }
    #endregion

    #region OnUserAgreementClick 用户使用协议按钮点击
    /// <summary>
    /// 用户使用协议按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnUserAgreementClick(object[] obj)
    {
        OpenUserAgreementWindow();
    }
    #endregion

    #region OpenUserAgreementWindow 打开用户协议界面
    /// <summary>
    /// 打开用户协议界面
    /// </summary>
    private void OpenUserAgreementWindow()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.UserAgreement,(GameObject go)=> 
        {
            
        });
    }
    #endregion

    #region OnAgreementValueChanged 用户使用协议勾选
    /// <summary>
    /// 用户使用协议勾选
    /// </summary>
    /// <param name="obj"></param>
    private void OnAgreementValueChanged(object[] obj)
    {
        m_isAgree = !m_isAgree;
    }
    #endregion
}
