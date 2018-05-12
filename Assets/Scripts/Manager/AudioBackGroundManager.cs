//===================================================
//Author      : DRB
//CreateTime  ：5/9/2017 11:25:13 PM
//Description ：
//===================================================
using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioListener))]
public class AudioBackGroundManager : SingletonMono<AudioBackGroundManager>
{

    private AudioSource m_AudioSource;

    private AudioClip m_PrevAudioClip;

    private string m_AudioName;

    public const float FadeOutTime = 1f;

    public const float FadeInTime = 1f;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.volume = 0;
        m_AudioSource.loop = true;
        m_AudioSource.spatialBlend = 0f;

        DontDestroyOnLoad(gameObject);
    }
    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
    }

    public void SetVolume(float value)
    {
        SystemProxy.Instance.SetBgmVolume(value);
        if (value - m_AudioSource.volume == 1f)
        {
            StartCoroutine(FadeIn(FadeInTime));
        }
        else
        {
            m_AudioSource.volume = value;
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    public void Play(string name)
    {
        m_AudioName = name;
        string path = string.Format("download/{0}/audio/bgm/{1}.drb", ConstDefine.GAME_NAME, m_AudioName);
        AssetBundleManager.Instance.LoadOrDownload<AudioClip>(path, m_AudioName, (AudioClip audioClip) =>
        {
            StartCoroutine(DoPlay(audioClip));
        }, 0);
        
    }

    private IEnumerator DoPlay(AudioClip clip)
    {
        float delay = 0f;
        if (m_AudioSource.isPlaying && m_AudioSource.clip == clip)
        {
            yield return null;
        }
        else
        {
            float time1 = Time.time;
            if (m_PrevAudioClip != null)
            {
                yield return StartCoroutine(FadeOut(FadeOutTime));
            }

            float time2 = Time.time - time1;
            if (delay > time2)
            {
                yield return new WaitForSeconds(delay - time2);
            }

            m_AudioSource.clip = clip;
            m_PrevAudioClip = clip;
            m_AudioSource.Play();

            yield return StartCoroutine(FadeIn(FadeInTime));
        }
    }

    /// <summary>
    /// 声音淡出
    /// </summary>
    /// <param name="fadeOut"></param>
    /// <returns></returns>
    private IEnumerator FadeOut(float fadeOut)
    {
        float time = 0f;
        while (time <= fadeOut)
        {
            if (time != 0f)
            {
                m_AudioSource.volume = Mathf.Lerp(SystemProxy.Instance.AudioSetting.BGMVolume, 0f,time/fadeOut);
            }
            time += Time.deltaTime;
            yield return 1;
        }
        m_AudioSource.volume = 0f;
    }

    private IEnumerator FadeIn(float fadeIn)
    {
        float time = 0f;
        while (time <= fadeIn)
        {
            if (time != 0f)
            {
                m_AudioSource.volume = Mathf.Lerp(0f,SystemProxy.Instance.AudioSetting.BGMVolume, time / fadeIn);
            }
            time += Time.deltaTime;
            yield return 1;
        }
        m_AudioSource.volume = SystemProxy.Instance.AudioSetting.BGMVolume;
    }
}
