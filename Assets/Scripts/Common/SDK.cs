//===================================================
//Author      : DRB
//CreateTime  ：3/27/2017 4:27:05 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class SDK : SingletonMono<SDK>
{
//#if UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern int __WXLogin();

    [DllImport("__Internal")]
    private static extern int __WXShareText(String scene, string text);

    [DllImport("__Internal")]
    private static extern int __WXSharePicture(string scene, string path);

    [DllImport("__Internal")]
    private static extern int __WXShareUrl(string scene, string url,string title, string desc);

    [DllImport("__Internal")]
    private static extern int __Pay(string unifiedorder,string channelId);

    [DllImport("__Internal")]
    private static extern int __WXPay(string unifiedorder,string channelId);

    [DllImport("__Internal")]
    private static extern int __InitGameVersion();

    [DllImport("__Internal")]
    private static extern int __GetRoomId();
    
    [DllImport("__Internal")]
    private static extern int __IsUseMic();
    [DllImport("__Internal")]
    private static extern int __IsWXAppInstalled();
    [DllImport("__Internal")]
    private static extern int __InitSecretKey();
    [DllImport("__Internal")]
    private static extern int __CopyTextToClipboard(string content);
    //#endif

    [DllImport("__Internal")]
    private static extern string __GetIpV6(string ipv4);


#if UNITY_ANDROID && !UNITY_EDITOR
    private AndroidJavaClass m_JC = null;
    private AndroidJavaObject m_JO = null;
#endif



    private Action<string> m_OnWXLoginCallBack;

    private Action<int,int> m_OnGetRoomIdCallBack;

    private Action<int> m_OnIsUseMicCallBack;
    /// <summary>
    /// 微信分享委托 是否成功，是否有奖励
    /// </summary>
    private Action<bool,bool> m_OnWXShareCallBack;

    private bool m_isReward;

    private void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        m_JC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        m_JO = m_JC.GetStatic<AndroidJavaObject>("currentActivity");
#endif
    }

    /// <summary>
    /// 调用安卓方法
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="param"></param>
    private void CallMethod(string methodName, params object[] param)
    {
        Debug.Log("调用安卓方法" + methodName);
#if UNITY_ANDROID && !UNITY_EDITOR && !DEBUG_MODE
        m_JO.Call(methodName, param);
#endif
    }

    public void InitSecretKey()
    {
#if !UNITY_EDITOR
        CallMethod("InitSecretKey");
#if UNITY_IPHONE
        __InitSecretKey();
#endif
#endif
    }

    public void InitSecretKeyCallBack(string secretKey)
    {
        string[] arr = secretKey.Split('_');

        SystemProxy.Instance.NetCorrected = arr[0].ToInt();
        SystemProxy.Instance.NetKey = arr[1];
    }

    /// <summary>
    /// 更新APK
    /// </summary>
    /// <param name="path"></param>
    public void UpgradeAPK(string path)
    {
#if !UNITY_EDITOR
        CallMethod("UpgradeAPK", path);
#endif
    }

    /// <summary>
    /// 是否安装微信
    /// </summary>
    public void IsWXAppInstalled()
    {
#if !UNITY_EDITOR
        CallMethod("IsWXAppInstalled");
#if UNITY_IPHONE
        __IsWXAppInstalled();
#endif
#endif

    }

    public void IsWXAppInstalledCallBack(string isInstall)
    {
        Debug.Log("微信安装了?" + isInstall);
        SystemProxy.Instance.SetIsInstallWechat(isInstall.ToBool());
    }

    /// <summary>
    /// 微信登录
    /// </summary>
    /// <param name="onWXLoginCallBack"></param>
    public void WXLogin(Action<string> onWXLoginCallBack)
    {
#if !UNITY_EDITOR
        m_OnWXLoginCallBack = onWXLoginCallBack;
        CallMethod("WXLogin");
#if UNITY_IPHONE
        __WXLogin();
#endif
#endif
    }

    /// <summary>
    /// 微信登录回调
    /// </summary>
    /// <param name="code"></param>
    public void WXLoginCallBack(string code)
    {
        if (m_OnWXLoginCallBack != null)
        {
            m_OnWXLoginCallBack(code);
        }
    }

    /// <summary>
    /// 微信分享图片
    /// </summary>
    /// <param name="shareType"></param>
    /// <param name="path"></param>
    public void WXSharePicture(WXShareType shareType,string path,Action<bool,bool> onComplete)
    {
        m_OnWXShareCallBack = onComplete;
        m_isReward = false;
#if !UNITY_EDITOR
        CallMethod("WXSharePicture", shareType.ToString(), path);
#if UNITY_IPHONE
        __WXSharePicture(shareType.ToString(),path);
#endif
#endif
    }

    /// <summary>
    /// 微信分享文本
    /// </summary>
    /// <param name="shareType"></param>
    /// <param name="text"></param>
    public void WXShareText(WXShareType shareType, string text,Action<bool, bool> onComplete)
    {
        m_OnWXShareCallBack = onComplete;
        m_isReward = false;
#if !UNITY_EDITOR
        CallMethod("WXShareText", shareType.ToString(), text);
#if UNITY_IPHONE
        __WXShareText(shareType.ToString(),text);
#endif
#endif
    }

    /// <summary>
    /// 微信分享Url
    /// </summary>
    /// <param name="shareType"></param>
    /// <param name="url"></param>
    /// <param name="title"></param>
    /// <param name="desc"></param>
    public void WXShareUrl(WXShareType shareType,string url,string title, string desc,Action<bool,bool> onComplete,bool isReward)
    {
        m_OnWXShareCallBack = onComplete;
        m_isReward = isReward;
#if !UNITY_EDITOR
        CallMethod("WXShareUrl", shareType.ToString(), url, title, desc);
#if UNITY_IPHONE
        __WXShareUrl(shareType.ToString(),url, title, desc);
#endif
#endif
    }

    public void WXShareCallBack(string isSuccess)
    {
        if (m_OnWXShareCallBack != null)
        {
            m_OnWXShareCallBack(isSuccess.ToBool(),m_isReward);
        }
    }

    /// <summary>
    /// 微信支付
    /// </summary>
    /// <param name="passportId"></param>
    /// <param name="code"></param>
    public void WXPay(string unifiedorder,string channelId)
    {
#if !UNITY_EDITOR
        CallMethod("Pay", unifiedorder,channelId);
#if UNITY_IPHONE
        __WXPay(unifiedorder,channelId);
#endif
#endif
    }

    public void ApplePay(string unifiedorder, string channelId)
    {
#if UNITY_IPHONE
        __Pay(unifiedorder,channelId);
#endif
    }

    public void WXPayCallBack()
    {
        Debug.Log("微信支付回调咯");
    }

    public void ApplePayCallBack(string receipt)
    {
        Debug.Log("苹果支付回调咯");
        if (string.IsNullOrEmpty(receipt))
        {
            ShopCtrl.Instance.Receipt("", "");
            return;
        }
        string[] arr = receipt.Split('_');
        if (arr.Length == 2)
        {
            string orderId = arr[0];
            string rec = arr[1];
            ShopCtrl.Instance.Receipt(orderId, rec);
        }
    }

    /// <summary>
    /// 初始化游戏版本号
    /// </summary>
    public void InitGameVersion()
    {
        CallMethod("InitGameVersion");
#if UNITY_IPHONE
        __InitGameVersion();
#endif
    }

    /// <summary>
    /// 初始化游戏版本回调
    /// </summary>
    /// <param name="version"></param>
    public void InitGameVersionCallBack(string version)
    {
        GlobalInit.Instance.InitVersion(version);
    }

    /// <summary>
    /// 获取房间号
    /// </summary>
    public void GetRoomId(Action<int,int> onGetRoomIdCallBack)
    {
#if !UNITY_EDITOR
        m_OnGetRoomIdCallBack = onGetRoomIdCallBack;
        CallMethod("GetRoomId");
#if UNITY_IPHONE
        __GetRoomId();
#endif
#endif
    }

    /// <summary>
    /// 获取房间号回调
    /// </summary>
    /// <param name="roomId"></param>
    public void GetRoomIdCallBack(string content)
    {
        string[] arr = content.Split('_');
        int roomId = arr[0].ToInt();
        int playerId = 0;
        if (arr.Length == 2)
        {
            playerId = arr[1].ToInt();
        }

        if (m_OnGetRoomIdCallBack != null)
        {
            m_OnGetRoomIdCallBack(roomId, playerId);
        }
    }

    /// <summary>
    /// 请求麦克风权限
    /// </summary>
    /// <param name="callBack"></param>
    public void IsUseMic(Action<int> callBack)
    {
        m_OnIsUseMicCallBack = callBack;
        CallMethod("IsUseMic");
#if UNITY_IPHONE
        __IsUseMic();
#endif
    }

    public void IsUseMicCallBack(string value)
    {
        if (m_OnIsUseMicCallBack != null)
        {
            m_OnIsUseMicCallBack(value.ToInt());
        }
    }

    public void GetElectricityCallBack(string electricity)
    {
        Debug.Log("电量" + electricity);
        SystemProxy.Instance.SetElecricity(electricity.ToFloat());
    }



    public void CopyTextToClipboard(string content)
    {
        CallMethod("CopyTextToClipboard", content);
#if UNITY_IPHONE
        __CopyTextToClipboard(content);
#endif
    }

    #region GetIpV6 获取IPV6地址
    /// <summary>
    /// 获取IPV6地址
    /// </summary>
    /// <param name="ipv4"></param>
    /// <returns></returns>
    public string GetIpV6(string ipv4)
    {
#if UNITY_IPHONE
        return __GetIpV6(ipv4);
#else
        return ipv4 + "&ipv4";
#endif
    }
    #endregion
}
