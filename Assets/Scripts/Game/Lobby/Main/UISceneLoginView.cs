//===================================================
//Author      : DRB
//CreateTime  ：3/7/2017 3:19:53 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISceneLoginView : UISceneViewBase
{
    [SerializeField]
    private Text m_Version;

    protected override void OnStart()
    {
        base.OnStart();
        m_Version.SafeSetText(GlobalInit.Instance.CurrentVersion.ToString());
    }
}
