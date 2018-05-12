//===================================================
//Author      : DRB
//CreateTime  ：10/17/2017 4:23:16 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemCreateRoomViewGame : UIItemBase 
{
    [SerializeField]
    private Transform m_RulePage;
    [SerializeField]
    private Toggle m_ToggleGame;

    private UIItemCreateRoomViewRule[] m_Rules;

    public string GameType;

    public int GameId;

    public int SettingId;

    public Action<UIItemCreateRoomViewGame> onGameChanged;

    private const string SAVE_NAME = "RuleSettingCache";

    public void Init()
    {
        m_ToggleGame.onValueChanged.AddListener(OnGameChanged);

        m_Rules = GetComponentsInChildren<UIItemCreateRoomViewRule>(true);

        if (m_Rules == null || m_Rules.Length == 0) return;

        for (int i = 0; i < m_Rules.Length; ++i)
        {
            m_Rules[i].Init();
            m_Rules[i].onToggleValueChanged = OnRuleChanged;
        }

        string ruleSetting = PlayerPrefs.GetString(string.Format("{0}_{1}", SAVE_NAME, SettingId.ToString()), string.Empty);
        if (string.IsNullOrEmpty(ruleSetting)) return;
        string[] split = ruleSetting.Split(';');
        for (int i = 0; i < split.Length; ++i)
        {
            string[] split2 = split[i].Split('_');
            if (split2.Length == 3)
            {
                int ruleId = split2[0].ToInt();
                bool isShow = split2[1].ToBool();
                bool isOn = split2[2].ToBool();

                for (int j = 0; j < m_Rules.Length; ++j)
                {
                    if (m_Rules[j].RuleId == ruleId)
                    {
                        m_Rules[j].gameObject.SetActive(isShow);
                        m_Rules[j].isSelect = isOn;
                        break;
                    }
                }
            }
        }
    }

    private void OnGameChanged(bool isOn)
    {
        if (isOn)
        {
            if (onGameChanged != null)
            {
                onGameChanged(this);
            }
        }
    }

    private void OnRuleChanged(int ruleId, bool isOn)
    {
        for (int i = 0; i < m_Rules.Length; ++i)
        {
            if (m_Rules[i].RuleId == ruleId)
            {
                if (isOn)
                {
                    if (m_Rules[i].selectEvent != null && m_Rules[i].selectEvent.Length > 0)
                    {
                        for (int j = 0; j < m_Rules[i].selectEvent.Length; ++j)
                        {
                            for (int k = 0; k < m_Rules.Length; ++k)
                            {
                                if (m_Rules[i].selectEvent[j].id == m_Rules[k].RuleId)
                                {
                                    m_Rules[k].gameObject.SetActive(m_Rules[i].selectEvent[j].isShow);
                                    m_Rules[k].isSelect = m_Rules[i].selectEvent[j].isOn;
                                    m_Rules[k].Description = m_Rules[i].selectEvent[j].Description;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (m_Rules[i].unselectEvent != null && m_Rules[i].unselectEvent.Length > 0)
                    {
                        for (int j = 0; j < m_Rules[i].unselectEvent.Length; ++j)
                        {
                            for (int k = 0; k < m_Rules.Length; ++k)
                            {
                                if (m_Rules[i].unselectEvent[j].id == m_Rules[k].RuleId)
                                {
                                    m_Rules[k].gameObject.SetActive(m_Rules[i].unselectEvent[j].isShow);
                                    m_Rules[k].isSelect = m_Rules[i].unselectEvent[j].isOn;
                                    m_Rules[k].Description = m_Rules[i].unselectEvent[j].Description;
                                }
                            }
                        }
                    }
                }
                
            }
        }
        SaveRuleSetting();
    }

    public void ShowTag()
    {
        m_ToggleGame.gameObject.SetActive(true);
    }

    public void HideTag()
    {
        m_ToggleGame.gameObject.SetActive(false);
    }

    public void ShowRule()
    {
        m_RulePage.gameObject.SetActive(true);
    }

    public void HideRule()
    {
        m_RulePage.gameObject.SetActive(false);
    }

    public List<int> GetRule()
    {
        List<int> lst = new List<int>();
        for (int i = 0; i < m_Rules.Length; ++i)
        {
            if (m_Rules[i] != null && m_Rules[i].isSelect)
            {
                lst.Add(m_Rules[i].RuleId);
            }
        }
        return lst;
    }

    public bool isOn
    {
        get { return m_ToggleGame.isOn; }
        set
        {
            if (m_ToggleGame.isOn == value) return;
            m_ToggleGame.isOn = value;
        }
    }


    private void SaveRuleSetting()
    {
        string ruleSetting = string.Empty;
        for (int i = 0; i < m_Rules.Length; ++i)
        {
            ruleSetting += m_Rules[i].RuleId + "_" + m_Rules[i].gameObject.activeSelf + "_" + (m_Rules[i].isSelect ? "1" : "0") + ";";
        }
        PlayerPrefs.SetString(string.Format("{0}_{1}", SAVE_NAME, SettingId.ToString()), ruleSetting);
    }
}
