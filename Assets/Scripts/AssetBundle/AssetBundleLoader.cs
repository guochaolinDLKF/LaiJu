//===================================================
//Author      : DRB
//CreateTime  ：7/4/2016 1:13:34 AM
//Description ：AssetBundle同步加载
//===================================================
using UnityEngine;
using System.Collections;
using System;

public class AssetBundleLoader : IDisposable
{
    #region Variation
    public AssetBundle m_Bundle;
    #endregion

    #region Public Function
    public AssetBundleLoader(string path)
    {
        string fullPath = LocalFileManager.Instance.LocalFilePath + path;
        m_Bundle = AssetBundle.LoadFromFile(fullPath);
    }

    public void Dispose()
    {
        if (m_Bundle != null)
        {
            m_Bundle.Unload(false);
        }
    }

    public T LoadAsset<T>(string name) where T : UnityEngine.Object
    {
        if (m_Bundle == null)
        {
            return default(T);
        }
        else
        {
            return m_Bundle.LoadAsset(name) as T;
        }
    }
    #endregion
}
