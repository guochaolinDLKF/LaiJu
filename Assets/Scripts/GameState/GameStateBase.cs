//===================================================
//Author      : DRB
//CreateTime  ：6/13/2017 6:39:15 PM
//Description ：游戏状态基类
//===================================================
using UnityEngine;


public abstract class GameStateBase
{
    protected SceneMgrFuture CurrentMachine;

    public GameStateBase(SceneMgrFuture sceneManager)
    {
        CurrentMachine = sceneManager;
    }

    /// <summary>
    /// 进入场景
    /// </summary>
    public virtual void OnEnter() { }

    /// <summary>
    /// 场景更新
    /// </summary>
    public virtual void OnUpdate() { }

    /// <summary>
    /// 离开场景
    /// </summary>
    public virtual void OnExit() { }

    /// <summary>
    /// 场景销毁
    /// </summary>
    public virtual void BeforeOnDestroy() { }

    //protected abstract void OnEscape();

    #region LoadSceneUI 加载场景UI
    /// <summary>
    /// 加载场景UI
    /// </summary>
    protected virtual void LoadSceneUI() { }

    protected virtual void OnPlayerClick() { }
    #endregion
}
