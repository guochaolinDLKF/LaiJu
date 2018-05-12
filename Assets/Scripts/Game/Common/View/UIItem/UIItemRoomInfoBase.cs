//===================================================
//Author      : DRB
//CreateTime  ：5/8/2017 11:08:52 AM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using DRB.MahJong;
using UnityEngine;
using UnityEngine.UI;

public class UIItemRoomInfoBase : UIItemBase 
{
    [SerializeField]
    protected Text m_RoomId;

    [SerializeField]
    protected Text m_TextTime;

    [SerializeField]
    protected Text m_TextBaseScore;

    [SerializeField]
    protected Text m_TextLoop;
    [SerializeField]
    protected Text m_TextRule;
    [SerializeField]
    private Image[] m_DianLiang;
    [SerializeField]
    private Sprite[] m_DianliangSprite;
    [SerializeField]
    private GameObject m_RuleGO;
    [SerializeField]
    private Text m_TextGameName;

    private float m_Timer;

    private const float UPDATE_SPACE = 10f;//时间更新间隔

    private const int ELECTRICITY_LEVEL = 10;//电量等级

    protected override void OnAwake()
    {
        base.OnAwake();
        ModelDispatcher.Instance.AddEventListener(SystemProxy.ON_ELECTRICITY_CHANGED, OnGetElectricityCallBack);

        SetElectricity(SystemProxy.Instance.Electricity);
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        ModelDispatcher.Instance.RemoveEventListener(SystemProxy.ON_ELECTRICITY_CHANGED, OnGetElectricityCallBack);
    }

    private void OnGetElectricityCallBack(TransferData data)
    {
        float electricity = data.GetValue<float>("Electricity");
        SetElectricity(electricity);
    }

    private void SetElectricity(float electricity)
    {
        
        if (m_DianLiang != null)
        {
            int elec = (int)(electricity * ELECTRICITY_LEVEL);
            for (int i = 0; i < m_DianLiang.Length; ++i)
            {
                m_DianLiang[i].gameObject.SetActive(i <= elec);
                m_DianLiang[i].overrideSprite = m_DianliangSprite[elec < 2?1:0];
            }
        }
    }

    void Update()
    {
        if (Time.time > m_Timer)
        {
            m_Timer = Time.time + UPDATE_SPACE;
            m_TextTime.SafeSetText(TimeUtil.GetLocalTime());
        }
    }


    public void SetUI(int roomId, int baseScore,string param = "")
    {
        m_RoomId.SafeSetText(string.Format("房间号:{0}", roomId.ToString()));
        if (string.IsNullOrEmpty(param))
        {
            m_TextBaseScore.SafeSetText(string.Format("底    分:{0}", baseScore.ToString()));
        }
        else
        {
            m_TextBaseScore.SafeSetText(string.Format(param));
        }
    }


    protected void ShowLoop(int currentLoop, int maxLoop,bool isQuan)
    {
        m_TextLoop.SafeSetText(string.Format("游戏{0}数:{1}/{2}", isQuan?"圈":"局",currentLoop, maxLoop));
    }

    public void SetGameName(string gameName)
    {
        m_TextGameName.SafeSetText(gameName);
    }

    public void SetRoomConfig(List<cfg_settingEntity> setting)
    {
        if (m_TextRule == null) return;
        string str = string.Empty;
        List<string> lstRule = new List<string>();
        for (int i = 0; i < setting.Count; ++i)
        {
            if (lstRule.Contains(setting[i].label)) continue;
            lstRule.Add(setting[i].label);
            str += setting[i].label + ":";
            for (int j = 0; j < setting.Count; ++j)
            {
                if (setting[j].label == setting[i].label)
                {
                    str += setting[j].name + "  ";
                }
            }
            str += "\r\n";
        }
        m_TextRule.SafeSetText(str);
    }

    public void ChangeRuleActive()
    {
        if (m_RuleGO != null)
        {
            m_RuleGO.gameObject.SetActive(!m_RuleGO.activeInHierarchy);
        }
    }
}
