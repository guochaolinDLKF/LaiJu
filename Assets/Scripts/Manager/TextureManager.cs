//===================================================
//Author      : DRB
//CreateTime  ：4/22/2017 3:30:33 PM
//Description ：玩家管理器
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TextureManager : SingletonMono<TextureManager>
{
    private Dictionary<int, Texture2D> m_TextureDic = new Dictionary<int, Texture2D>();

    private Dictionary<int, Action<Texture2D>> m_LoadingDic = new Dictionary<int, Action<Texture2D>>();

    public static string CACHE_PATH;

    protected override void OnAwake()
    {
        base.OnAwake();
        CACHE_PATH = LocalFileManager.Instance.LocalFilePath + "cache/texture/";
    }

    public void LoadHead(string avatarUrl, Action<Texture2D> onLoadFinish, bool isCache = false)
    {
        if (string.IsNullOrEmpty(avatarUrl)) return;
        int hash = avatarUrl.GetHashCode();
        if (m_TextureDic.ContainsKey(hash))
        {
            if (onLoadFinish != null)
            {
                onLoadFinish(m_TextureDic[hash]);
            }
        }
        else
        {
            if (onLoadFinish != null)
            {
                if (m_LoadingDic.ContainsKey(hash))
                {
                    m_LoadingDic[hash] += onLoadFinish;
                    return;
                }
                m_LoadingDic[hash] = onLoadFinish;
            }
            if (isCache && IOUtil.FileExists(CACHE_PATH + hash.ToString()))
            {
                LoadHeadByCache(hash);
                return;
            }
            LoadHead(avatarUrl, isCache);
        }
    }

    #region LoadHeadByCache 从本地缓存加载
    /// <summary>
    /// 从本地缓存加载
    /// </summary>
    /// <param name="hash"></param>
    private void LoadHeadByCache(int hash)
    {
        byte[] bytes = LocalFileManager.Instance.GetBuffer(CACHE_PATH + hash.ToString());
        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(bytes);
        if (!m_TextureDic.ContainsKey(hash))
        {
            m_TextureDic.Add(hash, tex);
        }
        if (m_LoadingDic.ContainsKey(hash) && m_LoadingDic[hash] != null)
        {
            m_LoadingDic[hash](m_TextureDic[hash]);
            m_LoadingDic.Remove(hash);
        }
    }
    #endregion

    private void LoadHead(string avatarUrl, bool isCache)
    {
        StartCoroutine(LoadHeadCoroutine(avatarUrl, isCache));
    }

    #region LoadHeadCoroutine 从网络下载
    /// <summary>
    /// 从网络下载
    /// </summary>
    /// <param name="avatarUrl"></param>
    /// <param name="isCache"></param>
    /// <returns></returns>
    private IEnumerator LoadHeadCoroutine(string avatarUrl, bool isCache)
    {
        LogSystem.Log("下载图片:" + avatarUrl);
        WWW www = new WWW(avatarUrl + "?t=" + TimeUtil.GetTimestampMS().ToString());

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
                LoadHead(avatarUrl, isCache);
                yield break;
            }
            yield return null;
        }

        yield return www;

        if (www.error == null)
        {
            int hash = avatarUrl.GetHashCode();
            if (isCache)
            {
                if (!IOUtil.DirectoryExists(CACHE_PATH))
                {
                    IOUtil.CreateDirectory(CACHE_PATH);
                }
                IOUtil.Write(CACHE_PATH + hash.ToString(), www.bytes);
            }

            if (!m_TextureDic.ContainsKey(hash))
            {
                m_TextureDic.Add(hash, www.texture);
            }
            if (m_LoadingDic.ContainsKey(hash) && m_LoadingDic[hash] != null)
            {
                m_LoadingDic[hash](m_TextureDic[hash]);
                m_LoadingDic.Remove(hash);
            }
            www.Dispose();
        }
        yield return null;
    }
    #endregion
}
