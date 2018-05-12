//===================================================
//Author      : DRB
//CreateTime  ：2017/05/26 10:02:21 AM
//Description ：加载场景控制器
//===================================================
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class LoadingSceneController : MonoBehaviour
{
    #region Variation
    /// <summary>
    /// Loading场景UI视图
    /// </summary>
    [SerializeField]
    private UISceneLoadingView m_UILoadingView;

    /// <summary>
    /// 异步加载场景信息
    /// </summary>
    private AsyncOperation m_Async = null;

    /// <summary>
    /// 当前进度
    /// </summary>
    private int m_CurrentProgress = 0;
    #endregion

    private float m_Timer = 0f;

    #region MonoBehaviour
    void Start ()
    {

        UIViewManager.Instance.CurrentUIScene = m_UILoadingView;

        m_Timer = Time.realtimeSinceStartup;

        DelegateDefine.Instance.OnSceneLoadComplete += OnSceneLoadFinishCallBack;
        StartCoroutine(LoadingScene());
	}

    void Update()
    {
        //if (m_Async == null) return;

        //if (m_Async.allowSceneActivation) return;
        //if (m_Async.progress >= 0.9f)
        //{
        //    m_Async.allowSceneActivation = true;
        //}

        //if (m_Async == null) return;
        //if (m_Async.allowSceneActivation) return;
        //int toProgress = 0;
        //if (m_Async.progress < 0.9f)
        //{
        //    toProgress = Mathf.Clamp((int)(m_Async.progress * 100), 1, 100);
        //}
        //else
        //{
        //    toProgress = 100;
        //}
        //if (m_CurrentProgress < toProgress)
        //{
        //    m_CurrentProgress++;
        //}
        //else
        //{
        //    if (toProgress == 100)
        //    {
        //        m_Async.allowSceneActivation = true;
        //    }
        //}

        //m_UILoadingView.SetProgressValue(m_CurrentProgress * 0.01f);
    }

    void OnDestroy()
    {
        Debug.Log("加载场景总耗时" + (Time.realtimeSinceStartup - m_Timer).ToString() + "秒");
        DelegateDefine.Instance.OnSceneLoadComplete -= OnSceneLoadFinishCallBack;
    }
    #endregion

    #region Private Function

    /// <summary>
    /// 场景加载完回调
    /// </summary>
    private void OnSceneLoadFinishCallBack()
    {
        Destroy(m_UILoadingView.gameObject);
        Destroy(gameObject);
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadingScene()
    {

        string strSceneName = "Scene_" + SceneMgr.Instance.CurrentSceneType.ToString();
        string path = string.Format("download/{0}/scene/{1}.drb", ConstDefine.GAME_NAME, strSceneName.ToLower());
        if (string.IsNullOrEmpty(strSceneName))
        {
            yield break;
        }
#if DISABLE_ASSETBUNDLE && UNITY_EDITOR
        m_Async = SceneManager.LoadSceneAsync(strSceneName, LoadSceneMode.Additive);
        m_Async.allowSceneActivation = true;
        Debug.Log("加载场景" + strSceneName);
        yield return null;
#else
        StartCoroutine(Load(path, strSceneName));
#endif
    }
    #endregion

    private IEnumerator Load(string path,string strSceneName)
    {
        string fullPath = LocalFileManager.Instance.LocalFilePath + path;
        if (!IOUtil.FileExists(fullPath))
        {
            DownloadDataEntity entity = DownloadManager.Instance.GetServerData(path);
            if (entity != null)
            {
                StartCoroutine(AssetBundleDownload.Instance.DownloadData(entity, (bool isSuccess) =>
                 {
                     if (isSuccess)
                     {
                         AssetBundleManager.Instance.CheckDps(path, () =>
                         {
                             StartCoroutine(LoadScene(fullPath, strSceneName));
                         });
                     }
                 }));
            }
        }
        else
        {
            AssetBundleManager.Instance.CheckDps(path,()=> 
            {
                StartCoroutine(LoadScene(fullPath, strSceneName));
            });
           
        }
        yield return null;
    }

    private IEnumerator LoadScene(string fullPath, string sceneName)
    {
        long time = TimeUtil.GetTimestampMS();
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(fullPath);
        yield return request;
        Debug.Log("场景资源加载用时:" + (TimeUtil.GetTimestampMS() - time));
        SceneMgr.Instance.CurrentSceneBundle = request.assetBundle;
        SceneMgr.Instance.SceneDic[SceneMgr.Instance.CurrentSceneType] = SceneMgr.Instance.CurrentSceneBundle;
        m_Async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        m_Async.allowSceneActivation = true;
    }
}
