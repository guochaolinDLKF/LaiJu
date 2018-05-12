//===================================================
//Author      : DRB
//CreateTime  ：3/12/2017 1:36:41 PM
//Description ：下载器
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AssetBundleDownloadRoutine : MonoBehaviour
{
    /// <summary>
    /// 需要下载的数据列表
    /// </summary>
    private List<DownloadDataEntity> m_List = new List<DownloadDataEntity>();

    /// <summary>
    /// 当前正在下载的数据
    /// </summary>
    private DownloadDataEntity m_CurrentDownloadData;

    /// <summary>
    /// 需要下载的数量
    /// </summary>
    public int NeedDownloadCount
    {
        get;
        private set;
    }

    /// <summary>
    /// 已经下载完成的数量
    /// </summary>
    public int CompleteCount
    {
        get;
        private set;
    }

    private int m_DownloadSize;//已经下载好的文件的总大小
    private int m_CurrentDownloadSize; //当前下载的文件大小

    /// <summary>
    /// 已经下载的大小
    /// </summary>
    public int DownloadSize
    {
        get { return m_DownloadSize + m_CurrentDownloadSize; }
    }

    /// <summary>
    /// 添加下载对象
    /// </summary>
    /// <param name="entity"></param>
    public void AddDownload(DownloadDataEntity entity)
    {
        m_List.Add(entity);
        NeedDownloadCount = m_List.Count;
    }

    /// <summary>
    /// 开始下载
    /// </summary>
    public void StartDownload()
    {
        StartCoroutine(DownloadData());
    }

    private IEnumerator DownloadData()
    {
        if (NeedDownloadCount == 0)
        {
            Debug.Log("需要下载的数量是0");
            yield break;
        }
        m_CurrentDownloadData = m_List[0];
        string fullName = m_CurrentDownloadData.FullName.Replace('\\', '/');
        string dataUrl = DownloadManager.Instance.DownloadUrl + fullName;
        int lastIndex = fullName.LastIndexOf('/');
        string path = "";
        if (lastIndex > 0)
        {
            path = fullName.Substring(0, lastIndex);
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

                m_CurrentDownloadSize = (int)(m_CurrentDownloadData.Size * progress);
            }

            if (Time.time - timeOut > DownloadManager.DOWNLOAD_TIME_OUT)
            {
                www.Dispose();
                AppDebug.LogWarning("下载超时");
                StartCoroutine(DownloadData());
                yield break;
            }
            yield return null;
        }

        yield return www;

        if (www != null && www.error == null)
        {
            using (FileStream fs = new FileStream(DownloadManager.Instance.LocalFilePath + fullName, FileMode.Create, FileAccess.Write))
            {
                fs.Write(www.bytes, 0, www.bytes.Length);
            }
            www.Dispose();
        }
        else
        {
            AppDebug.LogWarning(www.url + "下载失败" + www.error);
            yield break;
        }
        m_CurrentDownloadSize = 0;
        m_DownloadSize += m_CurrentDownloadData.Size;

        DownloadManager.Instance.ModifyLocalData(m_CurrentDownloadData);

        m_List.RemoveAt(0);
        ++CompleteCount;

        if (m_List.Count == 0)
        {
            m_List.Clear();
        }
        else
        {
            StartDownload();
        }
    }
}
