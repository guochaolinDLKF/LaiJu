//===================================================
//Author      : DRB
//CreateTime  ：4/19/2017 2:58:46 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;


public class AudioSettingCtrl : SystemCtrlBase<AudioSettingCtrl>, ISystemCtrl
{

    private UIAudioSettingView m_UIAudioSettingView;

    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        dic.Add("OnHasHandChanged",OnHasHandChanged);
        dic.Add("OnTableColorClick", OnTableColorClick);
        dic.Add("OnPokerColorClick", OnPokerColorClick);
        dic.Add("OnLanguageClick",OnLanguageClick);
        dic.Add("OnPlayExternalAudioClick", OnPlayExternalAudioClick);
        dic.Add("OnShakeClick", OnShakeChanged);
        return dic;
    }

    private void OnPlayExternalAudioClick(object[] obj)
    {
        bool isPlayExternalAudio = bool.Parse(obj[0].ToString());
        //Debug.Log(ChatCtrl.Instance.isPlayExternalAudio);
        if (isPlayExternalAudio)
        {
            ChatCtrl.Instance.isPlayExternalAudio = true;
        }
        else
        {
            ChatCtrl.Instance.isPlayExternalAudio = false;
        }

    }

    private void OnLanguageClick(object[] obj)
    {
        //SystemProxy.Instance.LanguageType = (LanguageType)Enum.Parse(typeof(LanguageType), obj[0].ToString(),true);
        if (bool.Parse(obj[0].ToString()))
        {
            SystemProxy.Instance.LanguageType = LanguageType.FuDing;
        }
        else
        {
            SystemProxy.Instance.LanguageType = LanguageType.Mandarin;
        }
    }

    #region OnPokerColorClick 麻将颜色按钮点击
    /// <summary>
    /// 麻将颜色按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnPokerColorClick(object[] obj)
    {
        string color = obj[0] as string;
        SystemProxy.Instance.PokerColor = color;
    }
    #endregion

    #region OnTableColorClick 桌子颜色变更
    /// <summary>
    /// 桌子颜色变更
    /// </summary>
    /// <param name="obj"></param>
    private void OnTableColorClick(object[] obj)
    {
        string color = obj[0] as string;
        SystemProxy.Instance.TableColor = color;
    }
    #endregion

    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.AudioSetting:
                UIViewUtil.Instance.LoadWindowAsync(UIWindowType.AudioSetting, (GameObject go) =>
                {
                    m_UIAudioSettingView = go.GetComponent<UIAudioSettingView>();
                    m_UIAudioSettingView.SetUI(SystemProxy.Instance.AudioSetting.BGMVolume, SystemProxy.Instance.AudioSetting.SoundEffectVolume,
                        SystemProxy.Instance.HasHand, SystemProxy.Instance.LanguageType,SystemProxy.Instance.IsShake);
                    m_UIAudioSettingView.OnBGMVolumeChanged = OnBGMVolumeChanged;
                    m_UIAudioSettingView.OnSoundEffectVolumeChanged = OnSoundEffectVolumeChanged;
                });
                //m_UIAudioSettingView = UIViewUtil.Instance.OpenWindow(UIWindowType.AudioSetting).GetComponent<UIAudioSettingView>();
                //m_UIAudioSettingView.SetUI(SystemProxy.Instance.AudioSetting.BGMVolume, SystemProxy.Instance.AudioSetting.SoundEffectVolume);
                //m_UIAudioSettingView.OnBGMVolumeChanged = OnBGMVolumeChanged;
                //m_UIAudioSettingView.OnSoundEffectVolumeChanged = OnSoundEffectVolumeChanged;
                break;
        }
    }

    /// <summary>
    /// 背景音乐音量更改回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnBGMVolumeChanged(float value)
    {
        AudioBackGroundManager.Instance.SetVolume(value);
    }

    /// <summary>
    /// 音效音量更改回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnSoundEffectVolumeChanged(float value)
    {
        AudioEffectManager.Instance.SetVolume(value);
    }

    private void OnHasHandChanged(object[] obj)
    {
        bool hasHand = (bool)obj[0];
        SystemProxy.Instance.HasHand = hasHand;
    }

    private void OnShakeChanged(object[] obj)
    {
        bool isShake = (bool)obj[0];
        SystemProxy.Instance.IsShake = isShake;
    }
}

public class AudioSetting
{
    public float BGMVolume;

    public float SoundEffectVolume;

    public AudioSetting(float bgmVolume, float soundEffectVolume)
    {
        BGMVolume = bgmVolume;
        SoundEffectVolume = soundEffectVolume;
    }
}
