//===================================================
//Author      : DRB
//CreateTime  ：12/13/2017 10:29:07 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleItem : UIToggleBase
{
    [SerializeField]
    private UIMailView m_MailView;

    public GameObject m_Select;

    public GameObject m_Normal;
    [SerializeField]
    private int m_Index;

  
    
    protected override void OnChangedTag(GameObject go)
    {
        base.OnChangedTag(go);
        AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);
         m_MailView.OnChangeTag(m_Index);

    }
}
