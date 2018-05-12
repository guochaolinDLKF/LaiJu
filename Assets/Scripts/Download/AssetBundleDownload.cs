//===================================================
//Author      : DRB
//CreateTime  ：3/12/2017 1:36:14 PM
//Description ：
//===================================================
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;

public class AssetBundleDownload : SingletonMono<AssetBundleDownload>
{
    /// <summary>
    /// 版本文件URL
    /// </summary>
    private string m_VersionUrl;

    private Action<string> m_OnInitVersion;

    /// <summary>
    /// 下载器
    /// </summary>
    private AssetBundleDownloadRoutine[] m_Routine = new AssetBundleDownloadRoutine[DownloadManager.DOWNLOAD_ROUTINE_NUM];
    /// <summary>
    /// 下载器索引
    /// </summary>
    private int m_nRoutineIndex = 0;

    /// <summary>
    /// 是否下载完成
    /// </summary>
    private bool isDownloadOver = false;


    private int m_CurrentDownloadSize = 0;

    private int m_CurrentDownloadCount = 0;


    
    private string m_Path;

    private float m_Time = 1;//采样时间
    private float m_AlreadyTime = 0;//已经下载的时间
    private float m_NeedTime = 0f;//剩余时间
    private float m_Speed = 0f;//下载速度

    public delegate void OnDownloadProgressChanged(int currentSize,int totalSize,int currentCount,int totalCount);
    private OnDownloadProgressChanged onDownloadProgressChanged;
    public delegate void OnDownloadComplete(bool isSuccess);
    private OnDownloadComplete onDownloadComplete;

    protected override void OnStart()
    {
        base.OnStart();

    }

    private void Update()
    {
        if (!isDownloadOver && TotalCount > 0)
        {
            int completeCount = CurrentCompleteCount();

            int completeSize = CurrentCompleteSize();



            m_AlreadyTime += Time.deltaTime;
            if (m_AlreadyTime > m_Time && m_Speed == 0)
            {
                m_Speed = completeSize / m_Time;
            }

            if (m_Speed > 0)
            {
                m_NeedTime = (TotalSize - completeSize) / m_Speed;
            }

            if (onDownloadProgressChanged != null)
            {
                onDownloadProgressChanged(completeCount, TotalCount, completeSize, TotalSize);
            }

            if (m_NeedTime > 0)
            {
            }
            if (completeCount == TotalCount)
            {
                if (onDownloadComplete != null)
                {
                    onDownloadComplete(true);
                }
                isDownloadOver = true;
            }
        }
    }

    /// <summary>
    /// 初始化服务器版本信息
    /// </summary>
    /// <param name="url"></param>
    /// <param name="onInitVersion"></param>
    public void InitServerVersion(string url,Action<string> onInitVersionComplete)
    {
        m_VersionUrl = url;
        StartCoroutine(DownloadVersion(m_VersionUrl, onInitVersionComplete));
    }

    private IEnumerator DownloadVersion(string url,Action<string> onInitVersionComplete)
    {
        UnityWebRequest request = UnityWebRequest.Get(url + "?t=" + TimeUtil.GetTimestampMS().ToString());
        Debug.Log(url);
        AsyncOperation async = request.Send();
        float timeOut = Time.time;
        float progress = request.downloadProgress;
        while (request != null && !request.isDone)
        {
            if (progress < request.downloadProgress)
            {
                timeOut = Time.time;
                progress = request.downloadProgress;
            }
            yield return null;
            if (Time.time - timeOut > DownloadManager.DOWNLOAD_TIME_OUT)
            {
                request.Dispose();
                Debug.LogWarning("下载超时");
                InitServerVersion(url, onInitVersionComplete);
                yield break;
            }
        }
        yield return async;
        if (request != null && request.error == null)
        {
            string content = request.downloadHandler.text;
         
            if (onInitVersionComplete != null)
            {
                onInitVersionComplete(content);
            }
        }
        else
        {
            Debug.Log("下载失败" + request.error);
            if (onInitVersionComplete != null)
            {
                onInitVersionComplete(string.Empty);
            }
        }
        request.Dispose();
    }

    public void DownLoadPackage(string url,string version,int size, Action<string> onDownloadPackageComplete, OnDownloadProgressChanged onDownloadProgressChanged)
    {
        this.onDownloadProgressChanged = onDownloadProgressChanged;
        m_CurrentDownloadSize = 0;
        m_CurrentDownloadCount = 0;
        TotalSize = size;
        TotalCount = 1;
        StartCoroutine(DownLoadPackageCoroutine(url, version, size, onDownloadPackageComplete));
    }

    private IEnumerator DownLoadPackageCoroutine(string url,string version,int size,Action<string> onDownloadPackageComplete)
    {
        Debug.Log(url);
        WWW www = new WWW(url + "?t=" + TimeUtil.GetTimestampMS().ToString());

        float timeOut = Time.time;
        float progress = www.progress;
        while (www != null && !www.isDone)
        {
           
            if (progress < www.progress)
            {
                timeOut = Time.time;
                progress = www.progress;
                m_CurrentDownloadSize = (int)(size * progress);
                if (onDownloadProgressChanged != null)
                {
                    onDownloadProgressChanged(m_CurrentDownloadCount, TotalCount, m_CurrentDownloadSize, TotalSize);
                }
            }
            if (Time.time - timeOut > DownloadManager.DOWNLOAD_TIME_OUT)
            {
                www.Dispose();
                Debug.LogWarning("下载超时");
                DownLoadPackage(url, version, size, onDownloadPackageComplete, onDownloadProgressChanged);
                yield break;
            }
            yield return null;
        }
        yield return www;
        if (www != null && www.error == null)
        {
            m_CurrentDownloadCount = 1;
            m_CurrentDownloadSize = size;
            string path = PackageManager.Instance.PACKAGE_PATH;
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(www.bytes, 0, www.bytes.Length);
            }

            if (onDownloadPackageComplete != null)
            {
                onDownloadPackageComplete(path);
            }
        }
        else
        {
            Debug.Log("下载失败" + www.error);
            if (onDownloadPackageComplete != null)
            {
                onDownloadPackageComplete(string.Empty);
            }
        }
        www.Dispose();
    }
    
    /// <summary>
    /// 总大小
    /// </summary>
    public int TotalSize
    {
        get;
        private set;
    }

    /// <summary>
    /// 当前已经完成的大小
    /// </summary>
    public int CurrentCompleteSize()
    {
        int size = m_CurrentDownloadSize;

        for (int i = 0; i < m_Routine.Length; ++i)
        {
            if (m_Routine[i] == null) continue;
            size += m_Routine[i].DownloadSize;
        }

        return size;
    }

    /// <summary>
    /// 总数量
    /// </summary>
    public int TotalCount
    {
        get;
        private set;
    }

    /// <summary>
    /// 当前已经完成的数量
    /// </summary>
    public int CurrentCompleteCount()
    {
        int count = m_CurrentDownloadCount;

        for (int i = 0; i < m_Routine.Length; ++i)
        {
            if (m_Routine[i] == null) continue;
            count += m_Routine[i].CompleteCount;
        }

        return count;
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="downloadList"></param>
    public void DownloadFiles(List<DownloadDataEntity> downloadList, OnDownloadComplete onComplete,OnDownloadProgressChanged onDownloadProgressChanged)
    {
        this.onDownloadProgressChanged = onDownloadProgressChanged;
        onDownloadComplete = onComplete;
        TotalSize = 0;
        TotalCount = 0;
        m_CurrentDownloadSize = 0;
        m_CurrentDownloadCount = 0;
        for (int i = 0; i < m_Routine.Length; ++i)
        {
            if (m_Routine[i] == null)
            {
                m_Routine[i] = gameObject.AddComponent<AssetBundleDownloadRoutine>();
            }
        }
        TotalCount = downloadList.Count;
        for (int i = 0; i < downloadList.Count; ++i)
        {
            m_nRoutineIndex = i % m_Routine.Length;

            m_Routine[m_nRoutineIndex].AddDownload(downloadList[i]);

            TotalSize += downloadList[i].Size;
        }
        
        for (int i = 0; i < m_Routine.Length; ++i)
        {
            m_Routine[i].StartDownload();
        }
    }



    public IEnumerator DownloadData(DownloadDataEntity entity,Action<bool> onComplete)
    {
        string dataUrl = DownloadManager.Instance.DownloadUrl + entity.FullName;
        Debug.Log("下载资源:" + dataUrl);
        int lastIndex = entity.FullName.LastIndexOf('/');
        string path = "";
        if (lastIndex > 0)
        {
            path = entity.FullName.Substring(0, lastIndex);
        }
        string localFilePath = DownloadManager.Instance.LocalFilePath + path;
        if (!IOUtil.DirectoryExists(localFilePath))
        {
            IOUtil.CreateDirectory(localFilePath);
        }

        WWW www = new WWW(dataUrl + "?t=" + TimeUtil.GetTimestampMS().ToString());
        float timeOut = Time.time;
        float progress = www.progress;
        while (www != null && !www.isDone)
        {
            if (progress < www.progress)
            {
                timeOut = Time.time;
                progress = www.progress;
            }

            if (Time.time - timeOut > DownloadManager.DOWNLOAD_TIME_OUT)
            {
                www.Dispose();
                AppDebug.LogWarning("下载超时");
                if (onComplete != null)
                {
                    onComplete(false);
                }
                yield break;
            }
            yield return null;
        }

        yield return www;

        if (www != null && www.error == null)
        {
            using (FileStream fs = new FileStream(DownloadManager.Instance.LocalFilePath + entity.FullName, FileMode.Create, FileAccess.Write))
            {
                fs.Write(www.bytes, 0, www.bytes.Length);
            }
            www.Dispose();
        }
        else
        {
            AppDebug.LogWarning("下载失败" + www.url + "  " + www.error);
            if (onComplete != null)
            {
                onComplete(false);
            }
            www.Dispose();
            yield break;
        }

        DownloadManager.Instance.ModifyLocalData(entity);

        if (onComplete != null)
        {
            onComplete(true);
        }
    }
}
