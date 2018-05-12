//===================================================
//Author      : DRB
//CreateTime  ：7/4/2016 1:23:24 AM
//Description ：场景管理器
//===================================================
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class SceneMgrFuture :SingletonMono<SceneMgrFuture>
{
    #region Private Variable
    private SceneType m_NextType;        //下一个状态
    #endregion


    #region Public Property
    public SceneType NextType
    {
        get
        {
            return m_NextType;
        }
    }

    private Dictionary<SceneType, GameStateBase> m_Dic;//状态字典

    private GameStateBase m_CurrentSceneState;
    /// <summary>
    /// 当前场景状态
    /// </summary>
    public GameStateBase CurrentSceneState { get { return m_CurrentSceneState; } }

    public AssetBundle CurrentSceneBundle;
    /// <summary>
    /// 场景资源字典
    /// </summary>
    public Dictionary<SceneType, AssetBundle> SceneDic;
    #endregion

    protected override void OnAwake()
    {
        base.OnAwake();
        SceneManager.sceneLoaded += OnSceneLoaded;
        m_Dic = new Dictionary<SceneType, GameStateBase>();
        m_Dic[SceneType.Login] = new GameStateLogin(this);
        m_Dic[SceneType.Init] = new GameStateInit(this);
        m_Dic[SceneType.Loading] = new GameStateLoading(this);
        m_Dic[SceneType.Main] = new GameStateMain(this);
        m_Dic[SceneType.MaJiang3D] = new GameStateMahJong(this);

        SceneDic = new Dictionary<SceneType, AssetBundle>();
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        SceneManager.sceneLoaded -= OnSceneLoaded;
        m_Dic.Clear();
        m_Dic = null;
    }

    /// <summary>
    /// 场景加载完成回调
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (m_CurrentSceneState is GameStateLoading)
        {
            m_CurrentSceneState.OnExit();
        }
        if (scene.name.Equals("Scene_Loading"))
        {
            m_CurrentSceneState = m_Dic[SceneType.Loading];
        }
        else
        {
            m_CurrentSceneState = m_Dic[m_NextType];
        }
        m_CurrentSceneState.OnEnter();
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="newState"></param>
    public void LoadScene(SceneType type)
    {
        if (m_CurrentSceneState != null)
        {
            m_CurrentSceneState.OnExit();
        }

        m_NextType = type;

        if (SceneDic.ContainsKey(type))
        {
            //if (SceneMgr.Instance.CurrentSceneBundle != null)
            //{
            //    SceneMgr.Instance.CurrentSceneBundle.Unload(true);
            //    SceneMgr.Instance.CurrentSceneBundle = null;
            //}
            AudioEffectManager.Instance.StopAllAudio();
            UIViewManager.Instance.ClearWindows();
            //AssetBundleManager.Instance.UnloadAssetBundle();
            Resources.UnloadUnusedAssets();
            SceneManager.LoadScene("Scene_" + type.ToString());
        }
        else
        {
            SceneManager.LoadScene("Scene_Loading");
        }
    }



    /// <summary>
    /// 设置当前状态
    /// </summary>
    /// <param name="newState"></param>
    public void SetState(SceneType newState)
    {
        if (m_Dic.ContainsKey(newState))
        {
            if (m_CurrentSceneState != null)
            {
                m_CurrentSceneState.OnExit();
            }

            m_CurrentSceneState = m_Dic[newState];
            m_CurrentSceneState.OnEnter();
        }
    }
}
