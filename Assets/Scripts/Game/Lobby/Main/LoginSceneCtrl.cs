//===================================================
//Author      : DRB
//CreateTime  ：3/7/2017 3:25:24 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using com.oegame.mahjong.protobuf;
using UnityEngine;


public class LoginSceneCtrl : MonoBehaviour
{

    private void Awake()
    {
        SDK.Instance.IsWXAppInstalled();
    }

    void Start()
    {
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
