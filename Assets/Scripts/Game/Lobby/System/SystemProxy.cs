//===================================================
//Author      : DRB
//CreateTime  ：5/19/2017 12:07:32 PM
//Description ：
//===================================================
using DRB.Common;
using UnityEngine;

public class SystemProxy : ProxyBase<SystemProxy> 
{
    public const string ON_ELECTRICITY_CHANGED = "OnElectricityChanged";
    public const string ON_TABLE_COLOR_CHANGED = "OnTableColorChanged";
    public const string ON_POKER_COLOR_CHANGED = "OnPokerColorChanged";



#if IS_HONGHU
    private const string DEFAULT_TABLE_COLOR = "blue";
#else
    private const string DEFAULT_TABLE_COLOR = "green";
#endif

#if IS_GONGXIAN
    private const string DEFAULT_POKER_COLOR = "yellow";
#else
    private const string DEFAULT_POKER_COLOR = "green";
#endif

    /// <summary>
    /// 默认手开关
    /// </summary>
#if IS_LUALU
    private const int DEFAULT_HAS_HAND = 1;
#else
    private const int DEFAULT_HAS_HAND = 0;
#endif

    /// <summary>
    /// 默认震动开关
    /// </summary>
    private const int DEFAULT_IS_SHAKE = 1;

    public bool IsTestMode = false;

    /// <summary>
    /// 是否安装微信
    /// </summary>
    public bool IsInstallWeChat { get; private set; }
    /// <summary>
    /// 是否开启赠送功能
    /// </summary>
    public bool IsOpenPresent { get; set; }
    /// <summary>
    /// 是否开启微信登陆
    /// </summary>
    public bool IsOpenWXLogin { get; set; }
    /// <summary>
    /// 是否开启邀请码功能
    /// </summary>
    public bool IsOpenInvite { get; set; }


    private string m_TableColor;
    /// <summary>
    /// 桌子颜色
    /// </summary>
    public string TableColor
    {
        get
        {
            if (string.IsNullOrEmpty(m_TableColor))
            {
                m_TableColor = PlayerPrefs.GetString("TableColor", DEFAULT_TABLE_COLOR);
            }
            return m_TableColor;
        }
        set
        {
            m_TableColor = value;
            PlayerPrefs.SetString("TableColor", m_TableColor);
            TransferData data = new TransferData();
            data.SetValue("TableColor", m_TableColor);
            SendNotification(ON_TABLE_COLOR_CHANGED, data);
        }
    }

    private string m_PokerColor;
    /// <summary>
    /// 桌子颜色
    /// </summary>
    public string PokerColor
    {
        get
        {
            if (string.IsNullOrEmpty(m_PokerColor))
            {
                m_PokerColor = PlayerPrefs.GetString("PokerColor", DEFAULT_POKER_COLOR);
            }
            return m_PokerColor;
        }
        set
        {
            m_PokerColor = value;
            PlayerPrefs.SetString("PokerColor", m_PokerColor);
            TransferData data = new TransferData();
            data.SetValue("PokerColor", m_PokerColor);
            SendNotification(ON_POKER_COLOR_CHANGED, data);
        }
    }


    private int m_NetCorrected = 7;
    public int NetCorrected
    {
        get { return m_NetCorrected; }
        set
        {
            m_NetCorrected = value;
        }
    }

#if OUTER_NET && IS_LEPING
    private string m_NetKey = "w52zr77fgkr6c6ab";
#else
    private string m_NetKey = "w92rxtavrkr6c6ab";
#endif
    public string NetKey
    {
        get { return m_NetKey; }
        set
        {
            m_NetKey = value;
        }
    }

    /// <summary>
    /// 是否开启手部动画
    /// </summary>
    public bool HasHand
    {
        get
        {
            return PlayerPrefs.GetInt("HasHand", DEFAULT_HAS_HAND) == 1?true:false;
        }
        set
        {
            PlayerPrefs.SetInt("HasHand", value ? 1:0);
        }
    }
    
    public bool IsShake
    {
        get
        {
            return PlayerPrefs.GetInt("IsShake", DEFAULT_IS_SHAKE) == 1 ? true : false;
        }
        set
        {
            PlayerPrefs.SetInt("IsShake", value ? 1 : 0);
        }
    }
    /// <summary>
    /// 当前版本号
    /// </summary>
    public Version CurrentVersion;




    /// <summary>
    /// 电量
    /// </summary>
    public float Electricity { get; private set; }
    /// <summary>
    /// 音量
    /// </summary>
    public AudioSetting AudioSetting { get; private set; }

#if IS_LEPING
    private const float DEFAULT_BGM_VOLUME = 0.5f;
#else
    private const float DEFAULT_BGM_VOLUME = 1f;
#endif

#if IS_LEPING
    private const float DEFAULT_SOUND_EFFECT_VOLUME = 0.5f;
#else
    private const float DEFAULT_SOUND_EFFECT_VOLUME = 1f;
#endif
    /// <summary>
    /// 语言类型
    /// </summary>
    public LanguageType LanguageType
    {
        get
        {
            LanguageType languageType = (LanguageType)PlayerPrefs.GetInt("Language", 0);
            return languageType;
        }
        set
        {
            PlayerPrefs.SetInt("Language", (int)value);
        }
    }


    public SystemProxy()
    {
        AudioSetting = new AudioSetting(PlayerPrefs.GetFloat("BGMVolume", DEFAULT_BGM_VOLUME), PlayerPrefs.GetFloat("SoundEffectVolume", DEFAULT_SOUND_EFFECT_VOLUME));
    }

    public void SetElecricity(float electricity)
    {
        Electricity = electricity;
        TransferData data = new TransferData();
        data.SetValue("Electricity",Electricity);
        SendNotification(ON_ELECTRICITY_CHANGED, data);
    }

    public void SetBgmVolume(float bgmVolume)
    {
        AudioSetting.BGMVolume = bgmVolume;
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);

        TransferData data = new TransferData();
        data.SetValue("BGMVolume", bgmVolume);
        SendNotification("OnBGMVolumeChanged", data);
    }

    public void SetSoundEffectVolume(float effectSoundVolume)
    {
        AudioSetting.SoundEffectVolume = effectSoundVolume;
        PlayerPrefs.SetFloat("SoundEffectVolume", effectSoundVolume);

        TransferData data = new TransferData();
        data.SetValue("SoundEffectVolume", effectSoundVolume);
        SendNotification("OnSoundEffectVolumeChanged", data);
    }

    public void SetIsInstallWechat(bool isInstall)
    {
        if (!IsOpenWXLogin) return;
        IsInstallWeChat = isInstall;
        TransferData data = new TransferData();
        data.SetValue("IsInstallWeChat", IsInstallWeChat);
        SendNotification("OnIsInstallWeChatChanged", data);
    }
}
