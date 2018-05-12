//===================================================
//Author      : DRB
//CreateTime  ：4/17/2017 10:35:33 AM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemChatExpression : MonoBehaviour
{
    private int m_ExpressionId;

    private Action<int> m_OnClick;

    private void Start()
    {
        Button btn = gameObject.GetOrCreatComponent<Button>();
        btn.onClick.AddListener(OnBtnClick);
    }

    private void OnBtnClick()
    {
        AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);
        if (m_OnClick != null)
        {
            m_OnClick(m_ExpressionId);
        }
    }


    public void SetUI(int id,string image,Action<int> onClick)
    {
        m_ExpressionId = id;
        m_OnClick = onClick;
    }


}
