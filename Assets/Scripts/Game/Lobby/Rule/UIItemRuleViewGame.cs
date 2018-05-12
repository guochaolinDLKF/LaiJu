//===================================================
//Author      : DRB
//CreateTime  ：12/6/2017 2:08:22 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemRuleViewGame : UIItemBase
{
    [SerializeField]
    private Transform m_RulePage;
    [SerializeField]
    private Toggle m_ToggleGame;

    public string GameType;
    public int GameId;
    public int SettingId;
    public Action<UIItemRuleViewGame> onGameChanged;
    private const string SAVE_NAME = "RuleSettingCache";

   


    public void Init()
    {
        m_ToggleGame.onValueChanged.AddListener(OnGameChanged);

      

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

                
            }
        }
    }
    private void OnGameChanged(bool isOn)
    {
        if(isOn)
        {
            if(onGameChanged !=null)
            {
                onGameChanged(this);
            }
        }
    }
    private void OnRuleChanged(int ruleId, bool isOn)
    {
       
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
       
        PlayerPrefs.SetString(string.Format("{0}_{1}", SAVE_NAME, SettingId.ToString()), ruleSetting);
    }
}
