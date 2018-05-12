//===================================================
//Author      : DRB
//CreateTime  ：4/15/2017 6:07:38 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIItemCommonMessage : MonoBehaviour
{
    [SerializeField]
    private Button m_Button;
    [SerializeField]
    private Text m_TextMessage;



    private Action<string> m_OnClick;


    void Awake()
    {
        m_Button.onClick.AddListener(OnBtnClick);
    }


    public void SetUI(string message,Action<string> onClick)
    {
        m_TextMessage.SafeSetText(message);
        m_OnClick = onClick;
    }

    private void OnBtnClick()
    {
        AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);
        if (m_OnClick != null)
        {
            m_OnClick(m_TextMessage.text);
        }
    }

    
}
