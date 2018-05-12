//===================================================
//Author      : DRB
//CreateTime  ：4/29/2017 2:18:02 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;


public class AccountProxy : ProxyBase<AccountProxy>
{
    public const string ON_ACCOUNT_CARDS_CHANGED = "OnCardCountChanged";

    public AccountEntity CurrentAccountEntity;

    public void ChangeUser(int passportId,string token)
    {
        if (CurrentAccountEntity != null)
        {
            CurrentAccountEntity.passportId = passportId;
            CurrentAccountEntity.token = token;
        }
    }

    public void SetPhone(string phone)
    {
        CurrentAccountEntity.phone = phone;
    }

    public void SetCards(int cardCount)
    {
        if (CurrentAccountEntity != null)
        {
            CurrentAccountEntity.cards = cardCount;

            TransferData data = new TransferData();
            data.SetValue("CardCount", cardCount);
            SendNotification(ON_ACCOUNT_CARDS_CHANGED, data);
        }
    }



    //#region 获取验证码
    ///// <summary>
    ///// 获取验证码按钮点击
    ///// </summary>
    ///// <param name="obj"></param>
    //private void OnBtnGetIdentifyingCodeClick(object[] obj)
    //{
    //    if (string.IsNullOrEmpty(m_UILoginView.TelNum.text))
    //    {
    //        ShowMessage("错误提示", "手机号码不能为空", MessageViewType.Ok);
    //        return;
    //    }
    //    if (!m_UILoginView.TelNum.text.Regex(ConstDefine.TelNumRegex))
    //    {
    //        ShowMessage("错误提示", "请输入正确的手机号码", MessageViewType.Ok);
    //        return;
    //    }

    //    string telNum = m_UILoginView.TelNum.text;

    //    string timeStamp = TimeUtil.GetTimestamp().ToString();

    //    Dictionary<string, object> dic = new Dictionary<string, object>();
    //    dic["phone"] = telNum;
    //    dic["sign"] = EncryptUtil.Md5(string.Format("verifyCode{0}{1}mj12321jm", telNum, timeStamp));

    //    NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "verifyCode/" + timeStamp, OnGetIdentifyingCodeCallBack, true, dic);
    //}

    //private void OnGetIdentifyingCodeCallBack(NetWorkHttp.CallBackArgs args)
    //{
    //    if (args.HasError)
    //    {

    //    }
    //    else
    //    {
    //        if (args.Value.code < 0)
    //        {
    //            ShowMessage("错误提示", args.Value.msg, MessageViewType.Ok);
    //            return;
    //        }
    //    }
    //}
    //#endregion

    //#region 登陆
    ///// <summary>
    ///// 登陆按钮点击
    ///// </summary>
    ///// <param name="obj"></param>
    //private void OnBtnLoginClick(object[] obj)
    //{
    //    if (string.IsNullOrEmpty(m_UILoginView.TelNum.text))
    //    {
    //        ShowMessage("错误提示", "手机号码不能为空", MessageViewType.Ok);
    //        return;
    //    }
    //    if (string.IsNullOrEmpty(m_UILoginView.IdentifyingCode.text))
    //    {
    //        ShowMessage("错误提示", "请输入验证码", MessageViewType.Ok);
    //        return;
    //    }

    //    if (!m_UILoginView.TelNum.text.Regex(ConstDefine.TelNumRegex))
    //    {
    //        ShowMessage("错误提示", "请输入正确的手机号码", MessageViewType.Ok);
    //        return;
    //    }

    //    if (!m_UILoginView.IdentifyingCode.text.Regex(ConstDefine.VerifyCodeRegex))
    //    {
    //        ShowMessage("错误提示", "请输入正确的验证码", MessageViewType.Ok);
    //        return;
    //    }
    //    isQuickLogin = false;
    //    string telNum = m_UILoginView.TelNum.text;
    //    string verifyCode = m_UILoginView.IdentifyingCode.text;
    //    string timeStamp = TimeUtil.GetTimestamp().ToString();
    //    Dictionary<string, object> dic = new Dictionary<string, object>();
    //    dic["phone"] = telNum;
    //    dic["verifyCode"] = verifyCode;
    //    dic["sign"] = EncryptUtil.Md5(string.Format("verifyPswd{0}{1}{2}mj12321jm", telNum, verifyCode, timeStamp));

    //    NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "verifyPswd/" + timeStamp, OnGetPassWordCallBack, true, dic);
    //}

    ///// <summary>
    ///// 获取密码回调
    ///// </summary>
    ///// <param name="args"></param>
    //private void OnGetPassWordCallBack(NetWorkHttp.CallBackArgs args)
    //{
    //    if (args.HasError)
    //    {

    //    }
    //    else
    //    {
    //        if (args.Value.code < 0)
    //        {
    //            ShowMessage("错误提示", args.Value.msg, MessageViewType.Ok);
    //            return;
    //        }
    //        m_PasswordEntity = LitJson.JsonMapper.ToObject<PasswordEntity>(LitJson.JsonMapper.ToJson(args.Value.data));

    //        PlayerPrefs.SetString("phone", m_UILoginView.TelNum.text);

    //        PlayerPrefs.SetString("password", XorHelper.Encrypt(m_PasswordEntity.password.ToBytes()).ToUnicodeString());

    //        Login(m_UILoginView.TelNum.text, m_PasswordEntity.password);
    //        m_PasswordEntity = null;
    //    }
    //}
    //#endregion

    //private void Login(string phone, string password)
    //{
    //    Dictionary<string, object> dic = new Dictionary<string, object>();
    //    string timeStamp = TimeUtil.GetTimestamp().ToString();

    //    dic["phone"] = phone;
    //    dic["password"] = password;

    //    dic["sign"] = EncryptUtil.Md5(string.Format("login{0}{1}{2}mj12321jm", phone, password, timeStamp));

    //    NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "login/" + timeStamp, OnLoginCallBack, true, dic);
    //}
}
