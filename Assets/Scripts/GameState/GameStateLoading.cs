//===================================================
//Author      : DRB
//CreateTime  ：6/13/2017 8:20:56 PM
//Description ：
//===================================================
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateLoading : GameStateBase
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

    private float m_Timer = 0f;
    #endregion

    public GameStateLoading(SceneMgrFuture machine) : base(machine)
    {

    }


    public override void OnEnter()
    {
        base.OnEnter();

        m_UILoadingView = UnityEngine.Object.FindObjectOfType(typeof(UISceneLoadingView)) as UISceneLoadingView;

        UIViewManager.Instance.CurrentUIScene = m_UILoadingView;

        m_Timer = Time.realtimeSinceStartup;
        //if (SceneMgr.Instance.CurrentSceneBundle != null)
        //{
        //    SceneMgr.Instance.CurrentSceneBundle.Unload(true);
        //    SceneMgr.Instance.CurrentSceneBundle = null;
        //}
        AudioEffectManager.Instance.StopAllAudio();
        UIViewManager.Instance.ClearWindows();
        //AssetBundleManager.Instance.UnloadAssetBundle();
        Resources.UnloadUnusedAssets();
        CurrentMachine.StartCoroutine(LoadingScene());
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        if (m_UILoadingView != null)
        {
            UnityEngine.Object.Destroy(m_UILoadingView.gameObject);
        }
    }

    public override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        Debug.Log("加载场景总耗时" + (Time.realtimeSinceStartup - m_Timer).ToString() + "秒");
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
        CurrentMachine.StartCoroutine(Load(path, strSceneName));
#endif
    }

    private IEnumerator Load(string path, string strSceneName)
    {
        string fullPath = LocalFileManager.Instance.LocalFilePath + path;
        if (!IOUtil.FileExists(fullPath))
        {
            DownloadDataEntity entity = DownloadManager.Instance.GetServerData(path);
            if (entity != null)
            {
                CurrentMachine.StartCoroutine(AssetBundleDownload.Instance.DownloadData(entity, (bool isSuccess) =>
                {
                    if (isSuccess)
                    {
                        AssetBundleManager.Instance.CheckDps(path, () =>
                        {
                            CurrentMachine.StartCoroutine(LoadScene(fullPath, strSceneName));
                        });
                    }
                }));
            }
        }
        else
        {
            AssetBundleManager.Instance.CheckDps(path, () =>
            {
                CurrentMachine.StartCoroutine(LoadScene(fullPath, strSceneName));
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
