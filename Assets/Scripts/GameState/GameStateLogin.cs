//===================================================
//Author      : DRB
//CreateTime  ：6/13/2017 9:14:21 PM
//Description ：登陆状态
//===================================================
using System;
using UnityEngine;


public class GameStateLogin : GameStateBase
{
    public GameStateLogin(SceneMgrFuture sceneManager) : base(sceneManager) { }

    public override void OnEnter()
    {
        base.OnEnter();

        SDK.Instance.IsWXAppInstalled();//是否安装微信

        UIViewManager.Instance.LoadSceneUIFromAssetBundle(UIViewManager.SceneUIType.Login);
        if (DelegateDefine.Instance.OnSceneLoadComplete != null)
        {
            DelegateDefine.Instance.OnSceneLoadComplete();
        }
        UIViewManager.Instance.OpenWindow(UIWindowType.Login);

        if (AccountProxy.Instance.CurrentAccountEntity == null)
        {
            AccountCtrl.Instance.QuickLogin();
        }
    }
}
