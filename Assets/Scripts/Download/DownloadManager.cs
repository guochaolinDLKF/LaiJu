//===================================================
//Author      : DRB
//CreateTime  ：3/12/2017 1:35:50 PM
//Description ：下载管理器
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

public class DownloadManager : Singleton<DownloadManager>
{
    /// <summary>
    /// 下载超时时间
    /// </summary>
    public const int DOWNLOAD_TIME_OUT = 20;

    public string DownloadBaseUrl;

    public string DownloadUrl
    {
        get
        {
            string platform = string.Empty;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            platform = "Windows/";
#elif UNITY_ANDROID
            platform = "Android/";
#elif UNITY_IPHONE
            platform = "IOS/";
#endif
            return DownloadBaseUrl + platform;
        }
    }

    /// <summary>
    /// 下载器数量
    /// </summary>
    public const int DOWNLOAD_ROUTINE_NUM = 5;

    public string LocalFilePath = Application.persistentDataPath + "/";

    private List<DownloadDataEntity> m_NeedDownloadDataList = new List<DownloadDataEntity>();//需要下载的数据列表

    private List<DownloadDataEntity> m_LocalList = new List<DownloadDataEntity>();//本地版本信息
    private List<DownloadDataEntity> m_ServerList;//服务器版本信息
    /// <summary>
    /// 本地版本文件路径
    /// </summary>
    private string m_LocalVersionPath;
    private const string VERSION_FILE_NAME = "VersionInfo.txt";//版本文件名称

    private string m_StreamingAssetsPath; //资源初始化原始路径

    private AssetBundleDownload.OnDownloadComplete m_OnDownloadComplete;

    public AssetBundleDownload.OnDownloadProgressChanged m_onDownloadProgressChanged;


    public List<DownloadDataEntity> ServerList { get { return m_ServerList; } }

    #region InitCheckVersion 检查版本文件
    /// <summary>
    /// 检查版本文件
    /// </summary>
    /// <param name="onComplete">完成回调</param>
    /// <param name="onDownloadProgressChanged">下载进度更新回调</param>
    public void InitCheckVersion(AssetBundleDownload.OnDownloadComplete onComplete,AssetBundleDownload.OnDownloadProgressChanged onDownloadProgressChanged)
    {
#if UNITY_EDITOR && DISABLE_ASSETBUNDLE
        if (onComplete != null)
        {
            onComplete(true);
        }
#else
        m_onDownloadProgressChanged = onDownloadProgressChanged;
        m_OnDownloadComplete = onComplete;
        m_LocalVersionPath = LocalFilePath + VERSION_FILE_NAME;
        AssetBundleDownload.Instance.InitServerVersion(DownloadUrl + VERSION_FILE_NAME, OnInitVersionCallBack);
#endif

    }
    #endregion

    #region OnInitVersionCallBack 初始化版本回调
    /// <summary>
    /// 初始化版本文件回调
    /// </summary>
    /// <param name="serverDownloadData">服务器下载数据</param>
    private void OnInitVersionCallBack(string serverDownloadData)
    {
        if (string.IsNullOrEmpty(serverDownloadData))
        {
            Debug.Log("服务器版本文件是空的");
            if (m_OnDownloadComplete != null)
            {
                m_OnDownloadComplete(false);
            }
            return;
        }
        m_ServerList = PackDownloadData(serverDownloadData);
        DownloadResources();
    }
    #endregion

    #region DownloadResources 下载资源
    /// <summary>
    /// 下载资源
    /// </summary>
    private void DownloadResources()
    {
        //如果本地有版本文件
        if (IOUtil.FileExists(m_LocalVersionPath))
        {
            Debug.Log("本地有版本文件");
            string content = IOUtil.GetFileText(m_LocalVersionPath);
            Dictionary<string, string> localDic = PackDownloadDataDic(content);
            m_LocalList = PackDownloadData(content);

            //更新资源
            for (int i = 0; i < m_ServerList.Count; ++i)
            {
                if (!localDic.ContainsKey(m_ServerList[i].FullName.Trim()))
                {
                    if (m_ServerList[i].IsFirstData)
                    {
                        m_NeedDownloadDataList.Add(m_ServerList[i]);
                    }
                    continue;
                }
                if (localDic[m_ServerList[i].FullName.Trim()] != m_ServerList[i].MD5)
                {
                    m_NeedDownloadDataList.Add(m_ServerList[i]);
                }
            }
        }
        else
        {
            Debug.Log("本地没有版本文件");
            for (int i = 0; i < m_ServerList.Count; ++i)
            {
                if (m_ServerList[i].IsFirstData)
                {
                    m_NeedDownloadDataList.Add(m_ServerList[i]);
                }
            }
        }
        Debug.Log("需要下载资源数量" + m_NeedDownloadDataList.Count.ToString());
        if (m_NeedDownloadDataList.Count == 0)
        {
            m_OnDownloadComplete(true);
            return;
        }
        AssetBundleDownload.Instance.DownloadFiles(m_NeedDownloadDataList, m_OnDownloadComplete, m_onDownloadProgressChanged);
    }
    #endregion

    #region GetDownloadData 根据资源名称获取资源实体
    /// <summary>
    /// 根据资源名称获取资源实体
    /// </summary>
    /// <param name="fullName">资源名称</param>
    /// <param name="lst">所有资源实体</param>
    /// <returns></returns>
    private DownloadDataEntity GetDownloadData(string fullName, List<DownloadDataEntity> lst)
    {
        for (int i = 0; i < lst.Count; ++i)
        {
            if (lst[i].FullName.Equals(fullName, StringComparison.CurrentCultureIgnoreCase))
            {
                return lst[i];
            }
        }
        return null;
    }
    #endregion

    #region PackDownloadDataDic 封装字典
    /// <summary>
    /// 封装字典
    /// </summary>
    /// <param name="lst">下载数据实体列表</param>
    /// <returns></returns>
    public Dictionary<string, string> PackDownloadDataDic(List<DownloadDataEntity> lst)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();

        for (int i = 0; i < lst.Count; ++i)
        {
            dic[lst[i].FullName] = lst[i].MD5;
        }

        return dic;
    }
    #endregion

    #region PackDownloadDataDic 封装字典
    /// <summary>
    /// 封装字典
    /// </summary>
    /// <param name="content">版本文件内容</param>
    /// <returns></returns>
    public Dictionary<string, string> PackDownloadDataDic(string content)
    {
        if (string.IsNullOrEmpty(content)) return null;
        Dictionary<string, string> dic = new Dictionary<string, string>();

        string[] arrLines = content.Split('\n');
        for (int i = 0; i < arrLines.Length; ++i)
        {
            string[] arrData = arrLines[i].Split(';');
            if (arrData.Length == 4)
            {
                string FullName = arrData[0];
                string MD5 = arrData[1];
                dic[FullName] = MD5;
            }
        }
        return dic;
    }
    #endregion

    #region PackDownloadData 封装下载数据
    /// <summary>
    /// 封装下载数据
    /// </summary>
    /// <param name="content">版本文件内容</param>
    /// <returns></returns>
    public List<DownloadDataEntity> PackDownloadData(string content)
    {
        List<DownloadDataEntity> lst = new List<DownloadDataEntity>();

        string[] arrLines = content.Split('\n');
        for (int i = 0; i < arrLines.Length; ++i)
        {
            string[] arrData = arrLines[i].Split(';');
            if (arrData.Length == 4)
            {
                DownloadDataEntity entity = new DownloadDataEntity()
                {
                    FullName = arrData[0],
                    MD5 = arrData[1],
                    Size = arrData[2].ToInt(),
                    IsFirstData = arrData[3].ToBool()
                };
                lst.Add(entity);

            }
        }
        return lst;
    }
    #endregion

    #region ModifyLocalData 修改本地文件
    /// <summary>
    /// 修改本地文件
    /// </summary>
    /// <param name="entity">更新的下载数据</param>
    public void ModifyLocalData(DownloadDataEntity entity)
    {
        if (m_LocalList == null) return;
        bool isExists = false;
        for (int i = 0; i < m_LocalList.Count; ++i)
        {
            if (m_LocalList[i].FullName.Equals(entity.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                m_LocalList[i].MD5 = entity.MD5;
                m_LocalList[i].Size = entity.Size;
                m_LocalList[i].IsFirstData = entity.IsFirstData;
                isExists = true;
                break;
            }
        }

        if (!isExists)
        {
            m_LocalList.Add(entity);
        }

        SaveLocalVersion();
    }
    #endregion

    #region SaveLocalVersion 保存本地版本文件
    /// <summary>
    /// 保存本地版本文件
    /// </summary>
    private void SaveLocalVersion()
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < m_LocalList.Count; ++i)
        {
            sb.AppendLine(string.Format("{0};{1};{2};{3}", m_LocalList[i].FullName, m_LocalList[i].MD5, m_LocalList[i].Size, m_LocalList[i].IsFirstData ? 1 : 0));
        }

        IOUtil.CreateTextFile(m_LocalVersionPath, sb.ToString());
    }
    #endregion

    #region GetServerData 根据路径获取服务器数据
    /// <summary>
    /// 根据路径获取服务器数据
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns></returns>
    public DownloadDataEntity GetServerData(string path)
    {
        if (m_ServerList == null) return null;
        for (int i = 0; i < m_ServerList.Count; ++i)
        {
            if (m_ServerList[i].FullName.Equals(path))
            {
                return m_ServerList[i];
            }
        }
        return null;
    }
    #endregion
}