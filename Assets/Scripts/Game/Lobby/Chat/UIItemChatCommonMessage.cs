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

public class UIItemChatCommonMessage : MonoBehaviour
{
    [SerializeField]
    private Text m_TextMessage;



    private Action<string> m_OnClick;


    void Start()
    {
        Button btn = m_TextMessage.gameObject.AddComponent<Button>();
        btn.onClick.AddListener(OnBtnClick);
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
