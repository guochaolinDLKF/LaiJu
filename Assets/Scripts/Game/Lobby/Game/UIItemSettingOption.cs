//===================================================
//Author      : DRB
//CreateTime  ：4/12/2017 1:21:25 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSettingOption : MonoBehaviour
{
    [SerializeField]
    private Button m_Button;
    [SerializeField]
    private Image m_ImageBG;
    [SerializeField]
    private Image m_ImageOn;

    [SerializeField]
    private Text m_TextOptionName;

    private Action<int> m_OnToggle;

    private int m_OptionValue;

    [SerializeField]
    private Sprite[] m_SpriteCheckMark;
    [SerializeField]
    private Sprite[] m_SpriteBackGround;


    private bool m_isPlayerClick;

    private int m_nId;

    private void Awake()
    {
        m_Button.onClick.AddListener(OnBtnClick);

        ModelDispatcher.Instance.AddEventListener("OnSettingRuleOptionSelectedChange", OnSelectedChange);
    }

    private void OnDestroy()
    {
        ModelDispatcher.Instance.RemoveEventListener("OnSettingRuleOptionSelectedChange", OnSelectedChange);
    }

    private void OnSelectedChange(TransferData data)
    {
        int id = data.GetValue<int>("Id");
        if (id == m_nId)
        {
            bool isOn = data.GetValue<int>("IsOn") == 1 ? true : false;
            IsOn = isOn;
        }
    }

    public void SetUI(int id, bool isRadio, string optionLabel, int value, bool isOn, Action<int> onToggle, string tags, bool isAA,int payment,int playerCount, int cost)
    {
        m_nId = id;
        m_OnToggle = onToggle;
        if (isRadio)
        {
            m_ImageBG.overrideSprite = m_SpriteBackGround[0];
            m_ImageOn.overrideSprite = m_SpriteCheckMark[0];
        }
        else
        {
            m_ImageBG.overrideSprite = m_SpriteBackGround[1];
            m_ImageOn.overrideSprite = m_SpriteCheckMark[1];
        }
        m_ImageBG.SetNativeSize();
        m_ImageOn.SetNativeSize();
        m_OptionValue = value;
        m_TextOptionName.SafeSetText(optionLabel);
        if (tags.Equals("loop", StringComparison.CurrentCultureIgnoreCase) || tags.Equals("quan", StringComparison.CurrentCultureIgnoreCase))
        {
            string probName = "房卡";
#if IS_LAOGUI
            probName = "钻石";
#endif
            m_TextOptionName.SafeSetText(string.Format("{0}({1}×{2})", optionLabel, probName, ((payment == 2 && !isAA) ? cost * playerCount:cost).ToString()));
        }

        IsOn = isOn;
    }

    private bool m_isOn;
    public bool IsOn
    {
        get { return m_isOn; }
        set
        {
            m_ImageOn.gameObject.SetActive(value);
            m_isOn = value;
        }
    }

    private void OnBtnClick()
    {
        AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);
        if (m_OnToggle != null)
        {
            m_OnToggle(m_nId);
        }
    }


}
