//===================================================
//Author      : DRB
//CreateTime  ：7/4/2016 1:13:34 AM
//Description ：AssetBundle异步加载器
//===================================================
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// AssetBundle异步加载器
/// </summary>
public class AssetBundleLoaderAsync : MonoBehaviour
{
    #region Variation
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    private const string c_strFileProtocal = "file:///";
#else
    private const string c_strFileProtocal = "file://";
#endif
    private string m_strFullPath;

    private string m_strName;

    private Action<UnityEngine.Object> m_OnLoadCompleteCallBack;

    private AssetBundle m_Bundle;

    [System.Obsolete]
    private AssetBundleCreateRequest m_Request;
    #endregion

    #region Public Function
    public void BeginLoad(string path, string name, Action<UnityEngine.Object> onLoadComplete)
    {
        m_strFullPath = LocalFileManager.Instance.LocalFilePath + path;
        m_strName = name;
        m_OnLoadCompleteCallBack = onLoadComplete;
        StartCoroutine(Load());
    }

    public void Dispose()
    {
        Destroy(this.gameObject);
    }
    #endregion

    #region MonoBehaviour

    void OnDestroy()
    {
        if (m_Bundle != null)
        {
            m_Bundle.Unload(false);
        }
        m_strFullPath = null;
        m_strName = null;
        m_OnLoadCompleteCallBack = null;
        m_Request = null;
    }
    #endregion

    #region Coroutine

    private IEnumerator Load()
    {
        WWW www = new WWW(c_strFileProtocal + m_strFullPath);
        yield return www;
        if (www.isDone && www.error == null)
        {
            m_Bundle = www.assetBundle;
            if (m_OnLoadCompleteCallBack != null)
            {
                m_OnLoadCompleteCallBack(m_Bundle.LoadAsset(m_strName));
            }
        }
    }

    [System.Obsolete("请使用Load方法，WWW的效率比LoadFromMemory的效率要高")]
    private IEnumerator LoadAsset()
    {
        m_Request = AssetBundle.LoadFromMemoryAsync(LocalFileManager.Instance.GetBuffer(m_strFullPath));
        yield return m_Request;
        m_Bundle = m_Request.assetBundle;
        if (m_OnLoadCompleteCallBack != null)
        {
            m_OnLoadCompleteCallBack(m_Bundle.LoadAsset(m_strName));
            Destroy(this.gameObject);
        }
    }
    #endregion
}
