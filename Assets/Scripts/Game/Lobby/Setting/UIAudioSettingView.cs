//===================================================
//Author      : DRB
//CreateTime  ：4/19/2017 3:02:05 PM
//Description ：
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIAudioSettingView : UIWindowViewBase 
{
    [SerializeField]
    private Slider m_SliderBGMVolume;

    [SerializeField]
    private Slider m_SliderSoundEffectVolume;

    public Action<float> OnBGMVolumeChanged;

    public Action<float> OnSoundEffectVolumeChanged;

    private bool m_HasHand;
    [SerializeField]
    private Toggle m_ToggleHasHand;
    [SerializeField]
    private Toggle m_ToggleNotHasHand;
    [SerializeField]
    private Button[] m_ArrBtnTableColor;
    [SerializeField]
    private Button[] m_ArrBtnPokerColor;
    [SerializeField]
    private Toggle[] m_ArrToggleTableColor;
    [SerializeField]
    private Toggle[] m_ArrTogglePokerColor;
    [SerializeField]
    private Toggle[] m_ArrLanguage;
    [SerializeField]
    private Toggle m_ToggleShake;
    [SerializeField]
    private Toggle m_TogglePlayExternalAudio;
    [SerializeField]
    private Toggle m_ToggleBGM;
    [SerializeField]
    private Toggle m_ToggleSoundEffect;


    protected override void OnStart()
    {
        base.OnStart();
        m_SliderBGMVolume.onValueChanged.AddListener(OnSliderBGMVolumeChanged);
        m_SliderSoundEffectVolume.onValueChanged.AddListener(OnSliderSoundEffectVolumeChanged);
        if (m_ToggleHasHand != null)
        {
            m_ToggleHasHand.onValueChanged.AddListener(OnHasHandToggleChanged);
        }

        if (m_ToggleShake != null)
        {
            m_ToggleShake.onValueChanged.AddListener(OnShakeValueChanged);
        }

        if (m_TogglePlayExternalAudio != null)
        {
            m_TogglePlayExternalAudio.onValueChanged.AddListener(OnTogglePlayExternalAudio);
        }

        if (m_ToggleBGM != null)
        {
            m_ToggleBGM.onValueChanged.AddListener(OnToggleBGM);
        }
        if (m_ToggleSoundEffect != null)
        {
            m_ToggleSoundEffect.onValueChanged.AddListener(OnToggleSoundEffect);
        }

        if (m_ArrToggleTableColor != null)
        {
            for (int i = 0; i < m_ArrToggleTableColor.Length; i++)
            {
                m_ArrToggleTableColor[i].onValueChanged.AddListener(OnToggleTableChanged);
            }
        }

        if (m_ArrTogglePokerColor != null)
        {
            for (int i = 0; i < m_ArrTogglePokerColor.Length; i++)
            {
                m_ArrTogglePokerColor[i].onValueChanged.AddListener(OnTogglePokerChanged); 
            }
        }

        if (m_ArrLanguage != null && m_ArrLanguage.Length > 0)
        {
            for (int i = 0; i < m_ArrLanguage.Length; ++i)
            {
                m_ArrLanguage[i].onValueChanged.AddListener(OnLanguageChanged);
            }
        }
    }
    /// <summary>
    /// 语言变更
    /// </summary>
    /// <param name="arg0"></param>
    private void OnLanguageChanged(bool isOn)
    {
        for (int i = 0; i < m_ArrLanguage.Length; ++i)
        {
            if (m_ArrLanguage[i].isOn)
            {
                //SendNotification("OnLanguageClick", m_ArrLanguage[i].name);
                SendNotification("OnLanguageClick", m_ArrLanguage[i].isOn);
                break;
            }
        }
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        if (m_ArrBtnTableColor != null)
        {
            for (int i = 0; i < m_ArrBtnTableColor.Length; ++i)
            {
                if (m_ArrBtnTableColor[i].gameObject == go)
                {
                    SendNotification("OnTableColorClick",go.name);
                    return;
                }
            }
        }

        if (m_ArrBtnPokerColor != null)
        {
            for (int i = 0; i < m_ArrBtnPokerColor.Length; ++i)
            {
                if (m_ArrBtnPokerColor[i].gameObject == go)
                {
                    SendNotification("OnPokerColorClick", go.name);
                    return;
                }
            }
        }
        if (go.name == "btnMakeSure")
        {
            Close();
        }
    }

    /// <summary>
    /// 背景音乐音量条值变化
    /// </summary>
    /// <param name="value"></param>
    private void OnSliderBGMVolumeChanged(float value)
    {
        if (OnBGMVolumeChanged != null)
        {
            OnBGMVolumeChanged(value);
            if (value < 0.01f)
            {
                m_ToggleBGM.isOn = true;
            }
            else
            {
                m_ToggleBGM.isOn = false;
            }
        }
    }

    private void OnSliderSoundEffectVolumeChanged(float value)
    {
        if (OnSoundEffectVolumeChanged != null)
        {
            OnSoundEffectVolumeChanged(value);
            if (value < 0.01f)
            {
                m_ToggleSoundEffect.isOn = true;
            }
            else
            {
                m_ToggleSoundEffect.isOn = false;
            }
        }
    }

    private void OnHasHandToggleChanged(bool isOn)
    {
        //if (m_HasHand == isOn) return;

        m_HasHand = isOn;
        SendNotification("OnHasHandChanged", m_HasHand);
    }

    public void SetUI(float bgmVolume,float soundEffectVolume,bool hasHand, LanguageType languageType, bool isShake)
    {
        m_SliderBGMVolume.value = bgmVolume;
        m_SliderSoundEffectVolume.value = soundEffectVolume;

        m_HasHand = hasHand;
        if (m_ToggleHasHand != null)
        {
            m_ToggleHasHand.isOn = hasHand;
        }

        if (m_ToggleNotHasHand != null)
        {
            m_ToggleNotHasHand.isOn = !hasHand;
        }

        if (m_ToggleShake != null)
        {
            m_ToggleShake.isOn = isShake;
        }

        if (m_ArrLanguage != null)
        {
            for (int i = 0; i < m_ArrLanguage.Length; ++i)
            {
                if (m_ArrLanguage[i].name.Equals(languageType.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    m_ArrLanguage[i].isOn = true;
                }
            }
        }
    }
    private void OnToggleTableChanged(bool isOn)
    {
        for (int i = 0; i < m_ArrToggleTableColor.Length; i++)
        {
            if (m_ArrToggleTableColor[i].isOn)
            {
                SendNotification("OnTableColorClick", m_ArrToggleTableColor[i].name);
            }
        }
    }
    private void OnTogglePokerChanged(bool isOn)
    {
        for (int i = 0; i < m_ArrTogglePokerColor.Length; i++)
        {
            if (m_ArrTogglePokerColor[i].isOn)
            {
                SendNotification("OnPokerColorClick", m_ArrTogglePokerColor[i].name);
            }
        }
    }
    private void OnToggleBGM(bool isOn)
    {
        if (m_ToggleBGM.isOn)
        {
            m_SliderBGMVolume.value = 0;
        }
        else
        {
            if (m_SliderBGMVolume.value < 0.01f)
            {
                m_SliderBGMVolume.value = 0.6f;
            }
        }
    }
    private void OnToggleSoundEffect(bool isOn)
    {
        if (m_ToggleSoundEffect.isOn)
        {
            m_SliderSoundEffectVolume.value = 0;
        }
        else
        {
            if (m_SliderSoundEffectVolume.value < 0.01f)
            {
                m_SliderSoundEffectVolume.value = 0.6f;
            }
        }
    }

    private void OnShakeValueChanged(bool isOn)
    {
        SendNotification("OnShakeClick",isOn);
    }

    private void OnTogglePlayExternalAudio(bool isOn)
    {
        SendNotification("OnPlayExternalAudioClick", m_TogglePlayExternalAudio.isOn);
        //Debug.Log(ChatCtrl.Instance.isPlayExternalAudio);
        //if (m_TogglePlayExternalAudio.isOn)
        //{
        //    ChatCtrl.Instance.isPlayExternalAudio = true;
        //}
        //else
        //{
        //    ChatCtrl.Instance.isPlayExternalAudio = false;
        //}
    }
}
