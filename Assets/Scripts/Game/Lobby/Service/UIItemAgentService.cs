//===================================================
//Author      : CZH
//CreateTime  ：1/15/2018 9:49:18 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIItemAgentService : UIItemBase
{
    [SerializeField]
    private Text m_textWX;
    [SerializeField]
    private Button btnCopy;

    private Action<string> onClick;
    private string m_Content;

    protected override void OnAwake()
    {
        base.OnAwake();
        btnCopy.gameObject.GetOrCreatComponent<Button>().onClick.AddListener(OnBtnClick);
    }

    private void OnBtnClick()
    {
        if (onClick != null)
        {
            onClick(m_Content);
        }
    }


    public void SetUI(string textWX, Action<string> onClick)
    {
        m_textWX.SafeSetText(textWX);
        m_Content = textWX;
        this.onClick = onClick;
    }

}
