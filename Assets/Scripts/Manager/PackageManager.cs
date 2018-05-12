//===================================================
//Author      : DRB
//CreateTime  ：5/11/2017 3:26:23 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using DRB.Common;


public class PackageManager : Singleton<PackageManager> 
{
    private const string PACKAGE_FILE_NAME = "PackageInfo.txt";//包信息文件名称
    
    private Action m_OnComplete;

    public readonly string PACKAGE_PATH = LocalFileManager.Instance.LocalFilePath + "Package.apk";

    private string m_Url;
    private int m_Size;
    private string m_Version;


    public void RequestNewPackage(Action onComplete, AssetBundleDownload.OnDownloadProgressChanged onDownloadProgressChanged)
    {
        m_OnComplete = onComplete;
        this.onDownloadProgressChanged = onDownloadProgressChanged;
#if UNITY_EDITOR
        CancelDownload();
        return;
#endif
        RequestNewPackage();
    }

    private void RequestNewPackage()
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
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/upgrade/", OnRequestNewPackageCallBack, true, "upgrade", dic);
    }

    private void OnRequestNewPackageCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            UIViewManager.Instance.ShowMessage("提示","网络连接失败",MessageViewType.Ok, RequestNewPackage);
        }
        else
        {
            int code = args.Value.data["code"].ToString().ToInt();
            string md5 = args.Value.data["md5"].ToString();
            m_Url = args.Value.data["url"].ToString();
            m_Size = args.Value.data["size"].ToString().ToInt();
            m_Version = args.Value.data["version"].ToString();
            if (code == 1)
            {
#if UNITY_ANDROID
                if (IOUtil.FileExists(PACKAGE_PATH))
                {
                    string localMd5 = EncryptUtil.GetFileMD5(PACKAGE_PATH);
                    if (localMd5.Equals(md5, StringComparison.CurrentCultureIgnoreCase))
                    {
                        UIViewManager.Instance.ShowMessage("更新提示", "是否安装新版本（不用下载）", MessageViewType.OkAndCancel, InstallNewPack, CancelDownload);
                        return;
                    }
                    UIViewManager.Instance.ShowMessage("更新提示", "是否下载新版本", MessageViewType.OkAndCancel, DownloadNewPack, CancelDownload);
                }
                else
                {
                    UIViewManager.Instance.ShowMessage("更新提示", "请下载最新版本", MessageViewType.Ok, DownloadNewPack, CancelDownload);
                }
#elif UNITY_IPHONE
                UIViewManager.Instance.ShowMessage("更新提示", "是否下载新版本", MessageViewType.OkAndCancel, DownloadNewPack, CancelDownload);
#endif
            }
            else if (code == 2)
            {
#if UNITY_ANDROID
                if (IOUtil.FileExists(PACKAGE_PATH))
                {
                    string localMd5 = EncryptUtil.GetFileMD5(PACKAGE_PATH);
                    if (localMd5.Equals(md5, StringComparison.CurrentCultureIgnoreCase))
                    {
                        UIViewManager.Instance.ShowMessage("更新提示", "请安装最新版本（不用下载）", MessageViewType.Ok, InstallNewPack, CancelDownload);
                        return;
                    }
                    UIViewManager.Instance.ShowMessage("更新提示", "请下载最新版本", MessageViewType.Ok, DownloadNewPack, CancelDownload);
                }
                else
                {
                    UIViewManager.Instance.ShowMessage("更新提示", "请下载最新版本", MessageViewType.Ok, DownloadNewPack, CancelDownload);
                }
#elif UNITY_IPHONE
                UIViewManager.Instance.ShowMessage("更新提示", "请下载最新版本", MessageViewType.Ok, DownloadNewPack, CancelDownload);
#endif
            }
            else
            {
                CancelDownload();
            }
        }
    }

    private void CancelDownload()
    {
        if (m_OnComplete != null)
        {
            m_OnComplete();
        }
    }

    /// <summary>
    /// 安装新包
    /// </summary>
    private void InstallNewPack()
    {
        OnDownloadCompleteCallBack(PACKAGE_PATH);
    }

    private AssetBundleDownload.OnDownloadProgressChanged onDownloadProgressChanged;

    /// <summary>
    /// 下载新包
    /// </summary>
    private void DownloadNewPack()
    {
#if UNITY_ANDROID
        AssetBundleDownload.Instance.DownLoadPackage(m_Url, m_Version, m_Size, OnDownloadCompleteCallBack, onDownloadProgressChanged);
#elif UNITY_IPHONE
        Application.OpenURL(m_Url);
#endif
    }

    /// <summary>
    /// 下载完成回调
    /// </summary>
    /// <param name="path"></param>
    private void OnDownloadCompleteCallBack(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            UIViewManager.Instance.ShowMessage("提示","下载失败",MessageViewType.Ok, DownloadNewPack);
        }
        else
        {
            SDK.Instance.UpgradeAPK(path);
        }
    }


    private VersionData PackDownloadData(string content)
    {
        string version = string.Empty;
        DownloadDataEntity gamePack = null;
        string[] arrLines = content.Split('\n');
        if (arrLines.Length >= 2)
        {
            version = arrLines[0];
            string[] arrData = arrLines[1].Split(' ');
            if (arrData.Length == 4)
            {
                gamePack = new DownloadDataEntity()
                {
                    FullName = arrData[0],
                    MD5 = arrData[1],
                    Size = arrData[2].ToInt(),
                    IsFirstData = arrData[3].ToBool()
                };
            }
        }
        return new VersionData(version, gamePack);
    }

}
