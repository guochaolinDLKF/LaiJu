//===================================================
//Author      : DRB
//CreateTime  ：7/5/2016 11:34:36 PM
//Description ：场景UI视图基类
//===================================================

using UnityEngine;
using System.Collections;
using System;

public class UISceneViewBase : UIViewBase
{
    /// <summary>
    /// 当前UI画布
    /// </summary>
    public Canvas CurrentCanvas;

    /// <summary>
    /// 当前相机
    /// </summary>
    public Camera CurrentCamare;

    /// <summary>
    /// 容器_居中
    /// </summary>
    [SerializeField]
    public Transform Container_Center;

    /// <summary>
    /// 容器_右上
    /// </summary>
    [SerializeField]
    public Transform Container_TopRight;
    /// <summary>
    /// 容器_左
    /// </summary>
    [SerializeField]
    public Transform Container_Left;
    /// <summary>
    /// 持久化类型
    /// </summary>
    public UIScenePersistenceType persistenceType = UIScenePersistenceType.LoadSceneDestroy;

    public UIViewManager.OnInitComplete OnInitComplete;

    protected override void OnStart()
    {
        base.OnStart();
        if (OnInitComplete != null)
        {
            OnInitComplete();
        }
    }
}