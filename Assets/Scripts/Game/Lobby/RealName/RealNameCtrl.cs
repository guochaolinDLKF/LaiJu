//===================================================
//Author      : DRB
//CreateTime  ：12/15/2017 10:14:09 AM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class RealNameCtrl : SystemCtrlBase<RealNameCtrl>, ISystemCtrl
{

    private UIRealNameView m_UIRealNameView;
    private bool m_isBusy;//是否操作繁忙

    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {                      
        Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        dic.Add(ConstDefine.BtnRealName, OnBtnRealNameClick);
        dic.Add("btnRealNameViewGetVerifyCode", OnBtnGetVerifyCodeClick);
        
        //dic.Add("btnBindViewUnBind", OnUnBindClick);
        return dic;
    }

    public void OpenView(UIWindowType type)
    {
        
    }

    #region OnBtnRealNameClick 实名制认证点击
    /// <summary>
    /// 实名制认证点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnRealNameClick(object[] obj)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "passport/realNameAuthenticationInfo/", OnRequesRealNameConfigCallBack, true, "realNameAuthenticationInfo", dic);
    }
    #endregion

    private void OnRequesRealNameConfigCallBack(NetWorkHttp.CallBackArgs args)
    {        
        if (args.HasError)
        {
            ShowMessage("提示", "网络连接失败");
        }
        else
        {      
            if (args.Value.code == 1)
            {                
                ShowMessage("提示", args.Value.msg);
                return;
            }
            OpenRealNameView();
        }
    }

    private void OpenRealNameView()
    {        
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.RealName, (GameObject go) =>
        {
            m_UIRealNameView = go.GetComponent<UIRealNameView>();
            m_UIRealNameView.SetUI(OnRealNameCallback);
        });
    }

    #region 确认按钮回调
    private void OnRealNameCallback(string name,string playerID,string phone,string VerifyCode)
    {     
        bool isName = IsChineseCh(name);
        if (!isName) return;
        bool isplayerID = CheckCidInfo(playerID);
        if (!isplayerID) return;
        if (string.IsNullOrEmpty(phone))
        {
            ShowMessage("提示", "请输入手机号", MessageViewType.Ok);
            return;
        }
        if (!phone.Regex(ConstDefine.TelNumRegex))
        {
            ShowMessage("错误提示", "请输入正确的手机号码", MessageViewType.Ok);
            return;
        }
        if (string.IsNullOrEmpty(VerifyCode))
        {
            ShowMessage("提示", "请输入验证码", MessageViewType.Ok);
            return;
        }
        if (isplayerID&&isName)
        {
            BindPhone(name, playerID, phone, VerifyCode);           
        }
    }
    #endregion

    public void BindPhone(string name,string playerID,string telePhoneNumber, string verifyCode)
    {
        if (m_isBusy) return;
        m_isBusy = true;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;       
        dic["realName"] = get_uft8(name);       
        dic["idCard"] = get_uft8(playerID);
        dic["phone"] = telePhoneNumber;
        dic["verifyCode"] = verifyCode;        
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "passport/realNameAuthentication/", OnRealNameSendSuccess, true, "realNameAuthentication", dic);
    }



    private string get_uft8(string unicodeString)
    {
        string msg = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(unicodeString));    
        return msg;
    }

    #region 检测身份证号和姓名
    /// <summary>
    /// 检查身份证是否合法
    /// </summary>
    /// <param name="cid"></param>
    /// <returns></returns>
    private bool CheckCidInfo(string cid)
    {       
        if (cid == "")
        {
            ShowMessage("提示", "身份证号不能为空");
            return false ;
        }       
        string[] aCity = new string[] { null, null, null, null, null, null, null, null, null, null, null, "北京", "天津", "河北", "山西", "内蒙古", null, null, null, null, null, "辽宁", "吉林", "黑龙江", null, null, null, null, null, null, null, "上海", "江苏", "浙江", "安微", "福建", "江西", "山东", null, null, null, "河南", "湖北", "湖南", "广东", "广西", "海南", null, null, null, "重庆", "四川", "贵州", "云南", "西藏", null, null, null, null, null, null, "陕西", "甘肃", "青海", "宁夏", "新疆", null, null, null, null, null, "台湾", null, null, null, null, null, null, null, null, null, "香港", "澳门", null, null, null, null, null, null, null, null, "国外" };
        double iSum = 0;
        string info = "";
        System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^\d{17}(\d|x)$");
        System.Text.RegularExpressions.Match mc = rg.Match(cid);
        if (!mc.Success)
        {
            //return "";
            return false;
        }
        cid = cid.ToLower();
        cid = cid.Replace("x", "a");
        if (aCity[int.Parse(cid.Substring(0, 2))] == null)
        {
            ShowMessage("提示", "非法地区");
            return false;
        }
        try
        {
            DateTime.Parse(cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2));
        }
        catch
        {
            ShowMessage("提示", "非法生日");     
            return false;
        }
        for (int i = 17; i >= 0; i--)
        {
            iSum += (System.Math.Pow(2, i) % 11) * int.Parse(cid[17 - i].ToString(), System.Globalization.NumberStyles.HexNumber);
        }
        if (iSum % 11 != 1)
        {
            ShowMessage("提示", "非法证号");          
            return false;
        }
        return true;
       // return (aCity[int.Parse(cid.Substring(0, 2))] + "," + cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2) + "," + (int.Parse(cid.Substring(16, 1)) % 2 == 1 ? "男" : "女"));
    }
    #endregion

    #region 检测姓名
    /// <summary>  
    /// 判断输入的字符串只包含汉字  
    /// </summary>  
    /// <param name="input"></param>  
    /// <returns></returns>  
    public bool IsChineseCh(string input)
    {
        if (input == "")
        {
            ShowMessage("提示", "姓名不能为空");
            return false;
        }       
        bool res = false;
        //改了一下  
        if (Regex.IsMatch(input, @"[\u4e00-\u9fbb]+$"))
        {
            res = true;
        }
        else
        {
            ShowMessage("提示", "请输入正确姓名");
        }
        return res;       
    }
    #endregion

    #region 注册成功失败回调
    private void OnRealNameSendSuccess(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            ShowMessage("提示", "网络连接失败");
        }
        else
        {
            if (args.Value.code == 1)
            {
                Debug.Log(args.Value.data["phone"]+"                    手机号");
                m_UIRealNameView.Close();
                ShowMessage("提示", args.Value.msg);             
                return;
            }
            else
            {
                ShowMessage("提示", args.Value.msg);
            }
        }
    }
    #endregion




    #region OnBtnGetVerifyCodeClick 获取验证码按钮点击
    /// <summary>
    /// 获取验证码按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnGetVerifyCodeClick(object[] obj)
    {
        if (string.IsNullOrEmpty(AccountProxy.Instance.CurrentAccountEntity.phone))
        {
            if (string.IsNullOrEmpty(m_UIRealNameView.Telephone.text))
            {
                ShowMessage("提示", "请输入手机号", MessageViewType.Ok);
                return;
            }
            if (!m_UIRealNameView.Telephone.text.Regex(ConstDefine.TelNumRegex))
            {
                ShowMessage("错误提示", "请输入正确的手机号码", MessageViewType.Ok);
                return;
            }
            BindPhoneGetVerifyCode(m_UIRealNameView.Telephone.text);
        }
        else
        {
            UnBindPhoneGetVerifyCode();
        }
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
}
