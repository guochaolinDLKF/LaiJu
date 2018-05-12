//===================================================
//Author      : DRB
//CreateTime  ：12/15/2017 10:09:33 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BtnItem : UIItemBase
{
    [SerializeField]
    private Text selectImg;//选择状态
    [SerializeField]
    private Text normalImg;//未选择状态   
    
    private int id;

    private Action<int> m_onChanged;
    protected override void OnAwake()
    {
        base.OnAwake();
        this.GetComponent<Toggle>().onValueChanged.AddListener(OnToggleChanged);
    }

    public void SetUI(string str,int id, Action<int> onChanged)
    {        
        this.id = id;
        selectImg.text = str;
        normalImg.text = str;
        m_onChanged = onChanged;                
    }

    public void SetToggle()
    {
        this.GetComponent<Toggle>().isOn=true;
    }

    private void OnToggleChanged(bool isOn)
    {
        if (m_onChanged!=null&&isOn)
        {
            m_onChanged(id);
        }
    }

}
