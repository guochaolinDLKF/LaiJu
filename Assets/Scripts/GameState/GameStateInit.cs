//===================================================
//Author      : DRB
//CreateTime  ：6/13/2017 9:56:41 PM
//Description ：初始化状态
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;


public class GameStateInit : GameStateBase
{
    private UISceneInitView m_UISceneInitView;

    public GameStateInit(SceneMgrFuture sceneManager) : base(sceneManager)
    {
    }


    public override void OnEnter()
    {
        base.OnEnter();

        m_UISceneInitView = UnityEngine.Object.FindObjectOfType(typeof(UISceneInitView)) as UISceneInitView;

        SDK.Instance.GetRoomId(OnGetRoomIdCallBack);
#if DEBUG_MODE
        InitVersion(string.Format("{0}.{1}", ConstDefine.VERSION.GameVersion, ConstDefine.VERSION.UpdateVersion));
#else
        SDK.Instance.InitGameVersion();
#endif

        SDK.Instance.InitSecretKey();
    }


    #region OnGetRoomIdCallBack 获取房间号回调
    /// <summary>
    /// 获取房间号回调
    /// </summary>
    /// <param name="roomId"></param>
    private void OnGetRoomIdCallBack(int roomId, int playerId)
    {
        if (roomId != GlobalInit.Instance.InviteRoomId && roomId != 0)
        {
            GlobalInit.Instance.IsAutoJoin = true;
            GlobalInit.Instance.InviteRoomId = roomId;
            GlobalInit.Instance.ParentId = playerId;
            if (DelegateDefine.Instance.OnAutoJoinRoom != null)
            {
                DelegateDefine.Instance.OnAutoJoinRoom(GlobalInit.Instance.InviteRoomId, GlobalInit.Instance.ParentId);
            }
        }
        else
        {
            GlobalInit.Instance.IsAutoJoin = false;
        }
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
        GlobalInit.Instance.CurrentVersion = new DRB.Common.Version(localversion + ".0");
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

        SceneMgr.Instance.LoadScene(SceneType.Login);
    }
    #endregion
}
