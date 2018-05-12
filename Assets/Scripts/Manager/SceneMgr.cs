//===================================================
//Author      : DRB
//CreateTime  ：7/4/2016 1:23:24 AM
//Description ：场景管理器
//===================================================
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneMgr :Singleton<SceneMgr>
{
    #region Public Property
  

    private SceneType m_CurrentSceneType = SceneType.Login;
    public SceneType CurrentSceneType
    {
        get { return m_CurrentSceneType; }
        private set { m_CurrentSceneType = value; }
    }

    public AssetBundle CurrentSceneBundle;

    public Dictionary<SceneType, AssetBundle> SceneDic = new Dictionary<SceneType, AssetBundle>();
    #endregion
        

    #region Public Function
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="type"></param>
    public void LoadScene(SceneType type)
    {
        if (m_CurrentSceneType == type && m_CurrentSceneType == SceneType.Main) return;
        m_CurrentSceneType = type;



        //if (SceneMgr.Instance.CurrentSceneBundle != null)
        //{
        //    SceneMgr.Instance.CurrentSceneBundle.Unload(true);
        //    SceneMgr.Instance.CurrentSceneBundle = null;
        //}
        AudioEffectManager.Instance.StopAllAudio();
        UIViewManager.Instance.ClearWindows();
        //AssetBundleManager.Instance.UnloadAssetBundle();
        Resources.UnloadUnusedAssets();

        if (SceneDic.ContainsKey(type) && SceneDic[type] != null)
        {
            SceneManager.LoadScene("Scene_" + type.ToString());
            return;
        }

#if UNITY_EDITOR
        SceneManager.LoadScene("Scene_" + type.ToString());
#else
        SceneManager.LoadScene(string.Format("Scene_Loading_{0}",ConstDefine.GAME_NAME));
#endif


    }
    #endregion
}
