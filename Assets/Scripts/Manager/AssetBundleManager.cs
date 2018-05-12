//===================================================
//Author      : DRB
//CreateTime  ：7/4/2016 1:23:24 AM
//Description ：AssetBundle资源管理器
//===================================================
using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// AssetBundle资源管理器
/// </summary>
public class AssetBundleManager : Singleton<AssetBundleManager>
{
    #region Members
    /// <summary>
    /// 依赖文件配置
    /// </summary>
    private AssetBundleManifest m_ManiFest;
    /// <summary>
    /// 资源字典
    /// </summary>
    private Dictionary<string, UnityEngine.Object> m_AssetDic = new Dictionary<string, UnityEngine.Object>();
    /// <summary>
    /// 资源加载器字典
    /// </summary>
    private Dictionary<string, AssetBundleLoader> m_DpsAssetBundleLoaderDic = new Dictionary<string, AssetBundleLoader>();
    #endregion

    #region LoadManifestFile 加载依赖配置文件
    /// <summary>
    /// 加载依赖配置文件
    /// </summary>
    private void LoadManifestFile()
    {
        if (m_ManiFest != null) return;

        string assetName = string.Empty;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        assetName = "Windows";
#elif UNITY_ANDROID
        assetName = "Android";
#elif UNITY_IPHONE
        assetName = "IOS";
#endif
        if (!IOUtil.FileExists(LocalFileManager.Instance.LocalFilePath + assetName)) return;
        using (AssetBundleLoader loader = new AssetBundleLoader(assetName))
        {
            m_ManiFest = loader.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
    }
    #endregion

    #region CheckDpsAsync 检查依赖项（递归）
    /// <summary>
    /// 检查依赖项（递归）
    /// </summary>
    /// <param name="index">数组索引</param>
    /// <param name="arrDps">所有依赖项</param>
    /// <param name="onComplete">完成回调</param>
    private void CheckDpsAsync(int index, string[] arrDps, Action onComplete)
    {
        if (arrDps == null || arrDps.Length == 0)
        {
            if (onComplete != null)
            {
                onComplete();
            }
            return;
        }

        string fullPath = LocalFileManager.Instance.LocalFilePath + arrDps[index];
        if (!IOUtil.FileExists(fullPath))
        {
            Debug.LogWarning("文件" + fullPath + "不存在");
            DownloadDataEntity entity = DownloadManager.Instance.GetServerData(arrDps[index]);
            if (entity != null)
            {
                AssetBundleDownload.Instance.StartCoroutine(AssetBundleDownload.Instance.DownloadData(entity, (bool isSuccess) =>
                 {
                     ++index;
                     if (index == arrDps.Length)
                     {
                         if (onComplete != null)
                         {
                             onComplete();
                         }
                         return;
                     }
                     CheckDpsAsync(index, arrDps, onComplete);
                 }));
            }
        }
        else
        {
            ++index;
            if (index == arrDps.Length)
            {
                if (onComplete != null)
                {
                    onComplete();
                }
                return;
            }
            CheckDpsAsync(index, arrDps, onComplete);
        }
    }
    #endregion

    #region InitAssetBundle 初始化资源包
    /// <summary>
    /// 初始化资源包
    /// </summary>
    /// <param name="lst"></param>
    public void InitAssetBundle(List<DownloadDataEntity> lst)
    {
        if (lst == null) return;
        for (int i = 0; i < lst.Count; ++i)
        {
            if (lst[i].FullName.IndexOf("scene/scene_", StringComparison.CurrentCultureIgnoreCase) > -1) continue;

            string dpsPath = LocalFileManager.Instance.LocalFilePath + lst[i].FullName;
            if (!m_DpsAssetBundleLoaderDic.ContainsKey(dpsPath))
            {
                AssetBundleLoader loader = new AssetBundleLoader(lst[i].FullName);
                m_DpsAssetBundleLoaderDic[dpsPath] = loader;
            }
        }
    }
    #endregion

    #region Public Function
    #region CheckDps 检查依赖项
    /// <summary>
    /// 检查依赖项
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="onComplete">完成回调</param>
    public void CheckDps(string path, Action onComplete)
    {
        LoadManifestFile();
        if (m_ManiFest == null) return;
        string[] arrDps = m_ManiFest.GetAllDependencies(path);
        CheckDpsAsync(0, arrDps, () =>
        {
            if (onComplete != null)
            {
                onComplete();
            }
        });
    }
    #endregion

    #region LoadAssetBundle 同步加载AssetBundle
    /// <summary>
    /// 同步加载AssetBundle
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">路径</param>
    /// <param name="name">名称</param>
    /// <returns></returns>
    public T LoadAssetBundle<T>(string path, string name) where T : UnityEngine.Object
    {
        path = path.ToLower();
#if UNITY_EDITOR && DISABLE_ASSETBUNDLE
        if (path.Contains("/scene/"))
        {
            path = path.Replace(".drb", ".unity");
        }
        else if (path.Contains("/bgm/"))
        {
            path = path.Replace(".drb", ".mp3");
        }
        else if (path.Contains("/soundeffect/"))
        {
            path = path.Replace(".drb", ".wav");
        }
        else if (path.Contains("/uisource/"))
        {
            path = path.Replace(".drb", ".png");
        }
        else if (path.Contains("/material"))
        {
            path = path.Replace(".drb", ".mat");
        }
        else if (path.Contains("/font/"))
        {
            path = path.Replace(".drb", ".ttf");
        }
        else
        {
            path = path.Replace(".drb", ".prefab");
        }
        if (m_AssetDic.ContainsKey("Assets/" + path))
        {
            return m_AssetDic["Assets/" + path] as T;
        }
        else
        {
            T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>("Assets/" + path);
            if (asset == null)
            {
                int firstIndex = path.IndexOf('/');
                string overplusStr = path.Substring(firstIndex + 1, path.Length - firstIndex - 1);
                int secondIndex = overplusStr.IndexOf('/');
                string path2 = path.Replace(path.Substring(firstIndex + 1, secondIndex), "CommonAsset");
                asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>("Assets/" + path2);
            }
            m_AssetDic["Assets/" + path] = asset;
            return asset;
        }
#endif

        string fullPath = LocalFileManager.Instance.LocalFilePath + path;
        if (m_AssetDic.ContainsKey(fullPath + name))
        {
            return m_AssetDic[fullPath + name] as T;
        }
        LoadManifestFile();
        if (m_ManiFest == null) return null;
        string[] arrDps = m_ManiFest.GetAllDependencies(path);

        //float time = Time.realtimeSinceStartup;
        //加载依赖项
        for (int i = 0; i < arrDps.Length; ++i)
        {
            string dpsPath = LocalFileManager.Instance.LocalFilePath + arrDps[i];

            if (!m_DpsAssetBundleLoaderDic.ContainsKey(dpsPath))
            {
                AssetBundleLoader loader = new AssetBundleLoader(arrDps[i]);
                m_DpsAssetBundleLoaderDic[dpsPath] = loader;
            }
        }
        //Debug.Log("加载依赖项资源耗时" + (Time.realtimeSinceStartup - time).ToString() + "秒");

        if (!m_DpsAssetBundleLoaderDic.ContainsKey(fullPath))
        {
            AssetBundleLoader loader = new AssetBundleLoader(path);
            T obj = loader.LoadAsset<T>(name);

            m_DpsAssetBundleLoaderDic[fullPath] = loader;
            m_AssetDic[fullPath + name] = obj;
            return obj;
        }
        else
        {
            T obj = m_DpsAssetBundleLoaderDic[fullPath].LoadAsset<T>(name);
            m_AssetDic[fullPath + name] = obj;
            if (obj == null)
            {
                Debug.Log("但是资源包里没有这个资源" + name);
            }
            return obj;
        }
    }
    #endregion

    #region LoadOrDownload 加载或下载预制体
    /// <summary>
    /// 加载或下载预制体
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <param name="onComplete"></param>
    public void LoadOrDownload(string path, string name, Action<GameObject> onComplete)
    {
        LoadOrDownload<GameObject>(path, name, onComplete, 0);
    }
    #endregion

    #region LoadOrDownload 下载或加载资源
    /// <summary>
    /// 下载或加载资源
    /// </summary>
    /// <param name="path">短路径</param>
    /// <param name="name"></param>
    /// <param name="onComplete"></param>
    /// <param name="type">0=prefab 1=png</param>
    public void LoadOrDownload<T>(string path, string name, Action<T> onComplete, byte type) where T : UnityEngine.Object
    {
        path = path.ToLower();
#if UNITY_EDITOR && DISABLE_ASSETBUNDLE
        if (path.Contains("/scene/"))
        {
            path = path.Replace(".drb", ".unity");
        }
        else if (path.Contains("/bgm/"))
        {
            path = path.Replace(".drb", ".mp3");
        }
        else if (path.Contains("/soundeffect/"))
        {
            path = path.Replace(".drb", ".wav");
        }
        else if (path.Contains("/uisource/"))
        {
            path = path.Replace(".drb", ".png");
        }
        else if (path.Contains("/material"))
        {
            path = path.Replace(".drb", ".mat");
        }
        else if (path.Contains("/font/"))
        {
            path = path.Replace(".drb", ".ttf");
        }
        else
        {
            path = path.Replace(".drb", ".prefab");
        }

        T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>("Assets/" + path);
        if (asset == null)
        {
            int firstIndex = path.IndexOf('/');
            string overplusStr = path.Substring(firstIndex + 1, path.Length - firstIndex - 1);
            int secondIndex = overplusStr.IndexOf('/');
            path = path.Replace(path.Substring(firstIndex + 1, secondIndex), "CommonAsset");
            asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>("Assets/" + path);
        }
        if (onComplete != null)
        {
            onComplete(asset);
        }
#else
        string fullPath = LocalFileManager.Instance.LocalFilePath + path;
        if (m_AssetDic.ContainsKey(fullPath + name))
        {
            if (onComplete != null)
            {
                onComplete(m_AssetDic[fullPath + name] as T);
            }
            return;
        }
        LoadManifestFile();
        if(m_ManiFest == null)return;
        string[] arrDps = m_ManiFest.GetAllDependencies(path);
        CheckDpsAsync(0, arrDps, () =>
        {
            if (!IOUtil.FileExists(fullPath))
            {
                Debug.Log("文件" + fullPath + "不存在");
                DownloadDataEntity entity = DownloadManager.Instance.GetServerData(path);
                if (entity != null)
                {
                    AssetBundleDownload.Instance.StartCoroutine(AssetBundleDownload.Instance.DownloadData(entity, (bool isSuccess) =>
                    {
                        if (isSuccess)
                        {
                            //加载依赖项
                            for (int i = 0; i < arrDps.Length; ++i)
                            {
                                string dpsPath = LocalFileManager.Instance.LocalFilePath + arrDps[i];
                                if (!m_DpsAssetBundleLoaderDic.ContainsKey(dpsPath))
                                {
                                    AssetBundleLoader loader = new AssetBundleLoader(arrDps[i]);
                                    m_DpsAssetBundleLoaderDic[dpsPath] = loader;
                                }
                            }

                            if (!m_DpsAssetBundleLoaderDic.ContainsKey(fullPath))
                            {
                                AssetBundleLoader loader = new AssetBundleLoader(path);
                                T obj = loader.LoadAsset<T>(name);

                                m_DpsAssetBundleLoaderDic[fullPath] = loader;
                                m_AssetDic[fullPath + name] = obj;
                                if (onComplete != null)
                                {
                                    onComplete(obj);
                                }
                            }
                            else
                            {
                                T obj = m_DpsAssetBundleLoaderDic[fullPath].LoadAsset<T>(name);
                                m_AssetDic[fullPath + name] = obj;
                                if (onComplete != null)
                                {
                                    onComplete(obj);
                                }
                            }
                        }
                    }));
                }
                else
                {
                    Debug.LogWarning("资源不存在" + fullPath);
                }
            }
            else
            {
                //加载依赖项
                for (int i = 0; i < arrDps.Length; ++i)
                {
                    string dpsPath = LocalFileManager.Instance.LocalFilePath + arrDps[i];
                    if (!m_DpsAssetBundleLoaderDic.ContainsKey(dpsPath))
                    {
                        AssetBundleLoader loader = new AssetBundleLoader(arrDps[i]);
                        m_DpsAssetBundleLoaderDic[dpsPath] = loader;
                    }
                }

                if (!m_DpsAssetBundleLoaderDic.ContainsKey(fullPath))
                {
                    AssetBundleLoader loader = new AssetBundleLoader(path);
                    T obj = loader.LoadAsset<T>(name);

                    m_DpsAssetBundleLoaderDic[fullPath] = loader;
                    m_AssetDic[fullPath + name] = obj;
                    if (onComplete != null)
                    {
                        onComplete(obj);
                    }
                }
                else
                {
                    T obj = m_DpsAssetBundleLoaderDic[fullPath].LoadAsset<T>(name);
                    m_AssetDic[fullPath + name] = obj;
                    if (onComplete != null)
                    {
                        onComplete(obj);
                    }
                }
            }
        });
#endif
    }
    #endregion

    #region LoadSpriteAsync 异步加载图片精灵
    /// <summary>
    /// 异步加载图片精灵
    /// </summary>
    /// <param name="path"></param>
    /// <param name="spriteName"></param>
    /// <param name="onComplete"></param>
    public void LoadSpriteAsync(string path, string spriteName, Action<Sprite> onComplete)
    {
        LoadOrDownload<Texture2D>(path, spriteName, (Texture2D tex) =>
        {
            Rect iconRect = new Rect(0, 0, tex.width, tex.height);
            Sprite iconSprite = Sprite.Create(tex, iconRect, new Vector2(0.5f, 0.5f));
            //赋值
            if (onComplete != null)
            {
                onComplete(iconSprite);
            }
        }, 1);
    }
    #endregion

    #region LoadSprite 同步加载图片精灵
    /// <summary>
    /// 同步加载图片精灵
    /// </summary>
    /// <param name="path"></param>
    /// <param name="spriteName"></param>
    /// <returns></returns>
    public Sprite LoadSprite(string path, string spriteName)
    {
#if UNITY_EDITOR && DISABLE_ASSETBUNDLE
        path = path.Insert(path.LastIndexOf('.'), "/" + spriteName);
        Texture2D tex = LoadAssetBundle<Texture2D>(path, spriteName);
        Rect iconRect;
        try
        {
            Debug.Log("图片宽度：" + tex.width + "----图片高度：" + tex.height);
            iconRect = new Rect(0, 0, tex.width, tex.height);
        }
        catch
        {
            AppDebug.LogWarning(path + "加载图片失败");
            AppDebug.LogWarning(spriteName + "加载图片失败");
            return null;
        }
        Sprite iconSprite = Sprite.Create(tex, iconRect, new Vector2(0.5f, 0.5f));
        return iconSprite;
#else
        Sprite sprite = LoadAssetBundle<Sprite>(path, spriteName);
        return sprite;
#endif
    }
    #endregion

    #region UnloadAssetBundle 卸载资源
    /// <summary>
    /// 卸载资源
    /// </summary>
    public void UnloadAssetBundle()
    {
        m_AssetDic.Clear();
        foreach (var pair in m_DpsAssetBundleLoaderDic)
        {
            pair.Value.Dispose();
        }
        m_DpsAssetBundleLoaderDic.Clear();
    }
    #endregion

    #region Load 加载镜像(非Instantiate)
    /// <summary>
    /// 加载镜像(非Instantiate)
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject Load(string path, string name)
    {
        using (AssetBundleLoader loader = new AssetBundleLoader(path))
        {
            return loader.LoadAsset<GameObject>(name);
        }
    }
    #endregion

    #region LoadClone 加载克隆(Instantiate)
    /// <summary>
    /// 加载克隆(Instantiate)
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject LoadClone(string path, string name)
    {
        using (AssetBundleLoader loader = new AssetBundleLoader(path))
        {
            GameObject obj = loader.LoadAsset<GameObject>(name);
            return UnityEngine.Object.Instantiate(obj);
        }
    }
    #endregion

    #region LoadAsync 异步加载资源
    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public AssetBundleLoaderAsync LoadAsync(string path, string name, Action<UnityEngine.Object> onLoadComplete)
    {
        GameObject obj = new GameObject("AssetBundleLoadAsync");
        AssetBundleLoaderAsync async = obj.GetOrCreatComponent<AssetBundleLoaderAsync>();
        async.BeginLoad(path, name, onLoadComplete);
        return async;
    }
    #endregion
    #endregion
}
