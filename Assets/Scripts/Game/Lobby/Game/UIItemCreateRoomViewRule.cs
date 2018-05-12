//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 4:23:39 PM
//Description ：
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct RuleEvent
{
    /// <summary>
    /// 配置Id
    /// </summary>
    public int id;
    /// <summary>
    /// 是否显示
    /// </summary>
    public bool isShow;
    /// <summary>
    /// 是否选中
    /// </summary>
    public bool isOn;
    /// <summary>
    /// 描述
    /// </summary>
    public string Description;
}

public class UIItemCreateRoomViewRule : UIItemBase
{
    public int RuleId;

    public RuleEvent[] selectEvent;

    public RuleEvent[] unselectEvent;
    [SerializeField]
    private string m_Description;

    private string m_Content;

    public string Description
    {
        get { return m_Description; }
        set
        {
            m_Description = value;
            Text text = GetComponentInChildren<Text>();
            if (text != null && !string.IsNullOrEmpty(m_Description))
            {
                text.text = m_Content + m_Description;
            }
        }
    }

    public Action<int, bool> onToggleValueChanged;

    public void Init()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(OnToggleValueChanged);
        Text text = GetComponentInChildren<Text>();
        if (text != null && !string.IsNullOrEmpty(m_Description))
        {
            m_Content = text.text;
            text.text += m_Description;
        }
    }

    private void OnToggleValueChanged(bool isOn)
    {
        if (onToggleValueChanged != null)
        {
            onToggleValueChanged(RuleId, isOn);
        }
    }

    public bool isSelect
    {
        get { return GetComponent<Toggle>().isOn; }
        set
        {
            GetComponent<Toggle>().isOn = value;
        }
    }

}
