//===================================================
//Author      : DRB
//CreateTime  ：7/5/2016 11:34:37 PM
//Description ：UI视图基类
//===================================================

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 所有UI视图基类
/// </summary>
public class UIViewBase : UIModuleBase
{

    protected override void OnAwake()
    {
        base.OnAwake();
        Button[] btnArr = GetComponentsInChildren<Button>(true);
        for (int i = 0; i < btnArr.Length; i++)
        {
            EventTriggerListener.Get(btnArr[i].gameObject).onClick += BtnClick;
        }
    }

    private void BtnClick(GameObject go)
    {
        OnBtnClick(go);
    }

    protected virtual void OnBtnClick(GameObject go)
    {
        if (go.name.Equals("btnClose"))
        {
            AudioEffectManager.Instance.Play("btnclose", Vector3.zero, false);
        }
        else
        {
            AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);
        }
    }


}