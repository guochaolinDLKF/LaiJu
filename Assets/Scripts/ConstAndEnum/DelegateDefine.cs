//===================================================
//Author      : DRB
//CreateTime  ：7/5/2016 11:32:12 PM
//Description ：委托定义
//===================================================
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// 委托定义
/// </summary>
public class DelegateDefine : Singleton<DelegateDefine>
{
    /// <summary>
    /// 场景加载完毕
    /// </summary>
    public Action OnSceneLoadComplete;
    
    /// <summary>
    /// 玩家出牌
    /// </summary>
    public Action<DRB.MahJong.Poker> OnPlayPoker;
    
    /// <summary>
    /// 自动加入房间
    /// </summary>
    public Action<int,int> OnAutoJoinRoom;
}
