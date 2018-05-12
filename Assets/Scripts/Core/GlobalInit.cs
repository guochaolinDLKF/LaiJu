//===================================================
//Author      : DRB
//CreateTime  ：3/8/2017 10:09:06 AM
//Description ：全局初始化
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using proto.common;
using UnityEngine;

/// <summary>
/// 充值类型
/// </summary>
public enum PaymentType
{
    /// <summary>
    /// 官方
    /// </summary>
    Official,
    /// <summary>
    /// 第三方
    /// </summary>
    ThirdParty,
}

public class GlobalInit : MonoBehaviour
{
    [SerializeField]
    private UISceneInitView m_UISceneInitView;

    
#if DEBUG_MODE
    public string TestIP = "";
    public int TestPort = 0;

    public int AccountId;
    public string Token;
#endif
    [SerializeField]
    private LogSystem.LogMode m_LogMode;
    public static GlobalInit Instance
    {
        get; private set;
    }

    #region MonoBehaviour
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        Initialize();
    }

    private void OnApplicationFocus(bool isFocus)
    {
        if (isFocus)
        {
            SDK.Instance.GetRoomId(OnGetRoomIdCallBack);
        }
    }

    private void Start()
    {

        //SceneMgrFuture.Instance.SetState(SceneType.Init);
        //return;


        SDK.Instance.GetRoomId(OnGetRoomIdCallBack);
#if DEBUG_MODE
        InitVersion(string.Format("{0}.{1}", ConstDefine.VERSION.GameVersion, ConstDefine.VERSION.UpdateVersion));
#else
        SDK.Instance.InitGameVersion();
#endif

        SDK.Instance.InitSecretKey();
    }


#if UNITY_EDITOR && DEBUG_MODE
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            AccountCtrl.Instance.AccountLogin(AccountId, Token);
        }
    }
#endif

    private void OnDestroy()
    {
        NetDispatcher.Instance.RemoveEventListener(99002, OnServerError);
    }
    #endregion

    private void OnTokenError(NetWorkHttp.CallBackArgs args)
    {
        UIViewManager.Instance.ShowMessage("提示", args.Value.msg, okAction:AccountCtrl.Instance.QuitToLogin);
    }

    #region Initialize 初始化
    /// <summary>
    /// 初始化
    /// </summary>
    private void Initialize()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        AudioBackGroundManager.CreateInstance();
        UIViewManager.CreateInstance();
        LPSManager.CreateInstance();
        LogSystem.CurrentLogMode = m_LogMode;
        NetDispatcher.Instance.AddEventListener(OP_SYS_ERROR.CODE, OnServerError);
        NetWorkHttp.Instance.OnTokenError = OnTokenError;
        LPSManager.Instance.StartGPS(null);
    }
    #endregion

    #region OnGetRoomIdCallBack 获取房间号回调
    /// <summary>
    /// 获取房间号回调
    /// </summary>
    /// <param name="roomId"></param>
    private void OnGetRoomIdCallBack(int roomId, int playerId)
    {
        if (roomId != InviteRoomId && roomId != 0)
        {
            IsAutoJoin = true;
            InviteRoomId = roomId;
            ParentId = playerId;
            if (DelegateDefine.Instance.OnAutoJoinRoom != null)
            {
                DelegateDefine.Instance.OnAutoJoinRoom(InviteRoomId, ParentId);
            }
        }
        else
        {
            IsAutoJoin = false;
        }
    }
    #endregion

    #region OnServerError 服务器返回错误消息
    /// <summary>
    /// 服务器返回错误消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerError(byte[] obj)
    {
        UIViewManager.Instance.CloseWait();
        OP_SYS_ERROR proto = OP_SYS_ERROR.decode(obj);
        LogSystem.Log(proto.code + "   " + proto.msg);
        UIViewManager.Instance.ShowMessage("提示", proto.msg);
    }
    #endregion

    #region SetVersionInfo 设置UI版本信息
    /// <summary>
    /// 设置UI版本信息
    /// </summary>
    /// <param name="localVersion"></param>
    /// <param name="serverVersion"></param>
    public void SetVersionInfo(string localVersion, string serverVersion)
    {
        if (m_UISceneInitView != null)
        {
            m_UISceneInitView.SetVersionInfo(localVersion, serverVersion);
        }
    }
    #endregion

    #region InitVersion 初始化版本
    /// <summary>
    /// 初始化版本
    /// </summary>
    /// <param name="localversion"></param>
    public void InitVersion(string localversion)
    {
        CurrentVersion = new DRB.Common.Version(localversion + ".0");
        RequestDownloadURL();
    }
    #endregion

    #region RequestDownloadURL 获取下载地址
    /// <summary>
    /// 获取下载地址
    /// </summary>
    private void RequestDownloadURL()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["gameName"] = ConstDefine.GAME_NAME;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        dic["platform"] = "windows";
#elif UNITY_ANDROID
        dic["platform"] = "android";
#elif UNITY_IPHONE
        dic["platform"] = "ios";
#endif
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrInit, OnRequestDownloadURLCallBack, true, ConstDefine.HTTPFuncInit, dic);
    }
    #endregion

    #region OnRequestDownloadURLCallBack 获取下载地址回调
    /// <summary>
    /// 获取下载地址回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestDownloadURLCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            UIViewManager.Instance.ShowMessage("错误", "网络连接失败,请重新尝试", MessageViewType.OkAndCancel, RequestDownloadURL, Application.Quit);
        }
        else
        {
            if (args.Value.code < 0)
            {
                UIViewManager.Instance.ShowMessage("提示", args.Value.msg, MessageViewType.OkAndCancel, RequestDownloadURL, Application.Quit);
                return;
            }

            string url = args.Value.data["downloadUrl"].ToString();
            bool isOpenPresent = args.Value.data["isGive"].ToString().ToBool();
            bool isWXLogin = args.Value.data["isWxLogin"].ToString().ToBool();

            bool isOpenInvite = args.Value.data["isInvite"].ToString().ToBool();

            if (((IDictionary)args.Value.data).Contains("payMode"))
            {
                PaymentType = (PaymentType)args.Value.data["payMode"].ToString().ToInt();
            }

            SystemProxy.Instance.IsOpenPresent = isOpenPresent;
            SystemProxy.Instance.IsOpenWXLogin = isWXLogin;
            SystemProxy.Instance.IsOpenInvite = isOpenInvite;

            DownloadManager.Instance.DownloadBaseUrl = url;

            CheckNewPackage();
        }
    }
    #endregion

    #region CheckNewPackage 检查新包
    /// <summary>
    /// 检查新包
    /// </summary>
    private void CheckNewPackage()
    {
        PackageManager.Instance.RequestNewPackage(OnCheckNewPackageCallBack, OnDownloadProgressChangedCallBack);
    }
    #endregion

    #region OnCheckNewPackageCallBack 检查新包回调
    /// <summary>
    /// 检查新包回调
    /// </summary>
    private void OnCheckNewPackageCallBack()
    {
        CheckResources();
    }
    #endregion

    #region CheckResources 检查资源更新
    /// <summary>
    /// 检查资源更新
    /// </summary>
    private void CheckResources()
    {
        DownloadManager.Instance.InitCheckVersion(OnUpdateResourcesComplete, OnDownloadProgressChangedCallBack);
    }
    #endregion

    #region OnDownloadProgressChanged 下载进度更新回调
    /// <summary>
    /// 下载进度更新回调
    /// </summary>
    /// <param name="currentCount"></param>
    /// <param name="totalCount"></param>
    /// <param name="currentSize"></param>
    /// <param name="totalSize"></param>
    private void OnDownloadProgressChangedCallBack(int currentCount, int totalCount, int currentSize, int totalSize)
    {
        if (m_UISceneInitView != null)
        {
            m_UISceneInitView.SetUI(currentCount, totalCount, currentSize, totalSize);
        }
    }
    #endregion

    #region OnUpdateResourcesComplete 更新资源完成回调
    /// <summary>
    /// 更新资源完成回调
    /// </summary>
    private void OnUpdateResourcesComplete(bool isSuccess)
    {
        Debug.Log("资源更新完成");
        if (!isSuccess)
        {
            UIViewManager.Instance.ShowMessage("提示", "初始化资源失败", MessageViewType.Ok, CheckResources);
            return;
        }

        //AssetBundleManager.Instance.InitAssetBundle(DownloadManager.Instance.ServerList);
        ShareCtrl.Instance.RequestShare();
        SceneMgr.Instance.LoadScene(SceneType.Login);
    }
    #endregion

    #region Variable
    /// <summary>
    /// 服务器时间差(客户端比服务器快多长时间 ms) 
    /// </summary>
    [HideInInspector]
    public long TimeDistance;

    /// <summary>
    /// 当前服务器时间
    /// </summary>
    public long CurrentServerTime
    {
        get
        {
            return TimeUtil.GetTimestampMS() - TimeDistance;
        }
    }
    /// <summary>
    /// 当前版本信息
    /// </summary>
    public DRB.Common.Version CurrentVersion;
    /// <summary>
    /// 上级代理Id
    /// </summary>
    [HideInInspector]
    public int ParentId;
    /// <summary>
    /// 邀请房间Id
    /// </summary>
    [HideInInspector]
    public int InviteRoomId = 0;
    /// <summary>
    /// 是否自动进入房间
    /// </summary>
    [HideInInspector]
    public bool IsAutoJoin = false;

    public AILevel AILevel = AILevel.Normal;
    /// <summary>
    /// 支付类型
    /// </summary>
    public PaymentType PaymentType = PaymentType.Official;
    #endregion
}