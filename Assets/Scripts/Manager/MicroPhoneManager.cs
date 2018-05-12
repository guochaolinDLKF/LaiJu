//===================================================
//Author      : DRB
//CreateTime  ：3/21/2017 4:01:29 PM
//Description ：麦克风管理器
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 麦克风管理器
/// </summary>
public class MicroPhoneManager : SingletonMono<MicroPhoneManager>
{
    #region About Music Variable
    /// <summary>
    /// 当前协程
    /// </summary>
    private UnityEngine.Coroutine m_CurrentCoroutine;
    #endregion

    #region About Record Variable
    /// <summary>
    /// 录制音乐声源
    /// </summary>
    private AudioSource m_MicroPhoneSource;
    /// <summary>
    /// 录音设备列表
    /// </summary>
    private string[] micArray = null;
    /// <summary>
    /// 录制最大时长
    /// </summary>
    private const int RECORD_MAX_TIME = 10;
    /// <summary>
    /// 录制最小时长
    /// </summary>
    private const int RECORD_MIN_TIME = 1;
    /// <summary>
    /// 录制时背景音乐音量
    /// </summary>
    private const float RECORDING_BGM_VOLOME = 0.0f;
    /// <summary>
    /// 帧率
    /// </summary>
    private const int FREQUENCY = 22050;
    /// <summary>
    /// 音量改变委托
    /// </summary>
    private Action<int> m_OnVolumeChanged;
    /// <summary>
    /// 录制结束委托
    /// </summary>
    private Action<bool> m_OnMicroStop;
    /// <summary>
    /// 当前录制时间
    /// </summary>
    private float m_CurrentRecordDuration;
    /// <summary>
    /// 当前音量等级
    /// </summary>
    private int m_CurrentVolumeLevel;
    /// <summary>
    /// 当前背景音乐音量
    /// </summary>
    private float m_CurrentBGMVolume;
    /// <summary>
    /// 是否正在录制
    /// </summary>
    [HideInInspector]
    public bool isRecoding = false;

    private AudioSource m_VoiceSource;


    private Queue<AudioClip> m_MicroQueue = new Queue<AudioClip>();

    private bool m_isPlaying;
    [HideInInspector]
    public bool CanRecord;
    /// <summary>
    /// 请求麦克风权限回调
    /// </summary>
    private Action<bool> m_onRequestMicroCallBack;
    #endregion

    #region MonoBehaviour
    protected override void OnAwake()
    {
        GameObject go = new GameObject("MicroPhoneSource");
        m_MicroPhoneSource = go.AddComponent<AudioSource>();
        m_MicroPhoneSource.loop = false;
        m_MicroPhoneSource.mute = true;
        m_MicroPhoneSource.volume = 1;
        go.SetParent(transform);
        micArray = Microphone.devices;
        CanRecord = !(micArray.Length == 0 || string.IsNullOrEmpty(micArray[0]));
        if (CanRecord)
        {
            Debug.Log(micArray[0]);
        }
        GameObject go2 = new GameObject("SoundEffect_AudioSource");
        go2.SetParent(transform);
        m_VoiceSource = go2.AddComponent<AudioSource>();
        m_VoiceSource.playOnAwake = false;
        m_VoiceSource.loop = false;
    }

    private void Update()
    {
        if (!m_VoiceSource.isPlaying && m_MicroQueue.Count > 0)
        {
            PlayAudio(m_MicroQueue.Dequeue(), AudioType.Micro);
            if (m_CurrentCoroutine != null)
            {
                StopCoroutine(m_CurrentCoroutine);
            }
            m_CurrentCoroutine = StartCoroutine(DelaySetBGMVolume(m_VoiceSource.clip.length));
        }
    }
    #endregion

    #region PlayExternalAudio 播放外部音频
    /// <summary>
    /// 播放外部音频
    /// </summary>
    /// <param name="bytes"></param>
    public void PlayExternalAudio(byte[] bytes)
    {
        bytes = GZipCompress.DeCompress(bytes);
        float[] fl = new float[bytes.Length / 4];

        using (MemoryStreamExt ms = new MemoryStreamExt(bytes))
        {
            for (int i = 0; i < fl.Length; ++i)
            {
                fl[i] = ms.ReadFloat();
            }
        }

        AudioClip clip = AudioClip.Create("MicroAudio", fl.Length, 1, FREQUENCY, false);
        clip.SetData(fl, 0);

        m_MicroQueue.Enqueue(clip);
    }
    #endregion

    #region DelaySetBGMVolume 延迟设置背景音乐音量
    /// <summary>
    /// 延迟设置背景音乐音量
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator DelaySetBGMVolume(float delay)
    {
        float bgmVolume = SystemProxy.Instance.AudioSetting.BGMVolume;
        AudioBackGroundManager.Instance.SetVolume(RECORDING_BGM_VOLOME);
        yield return new WaitForSeconds(delay);
        if (isRecoding) yield break;
        AudioBackGroundManager.Instance.SetVolume(bgmVolume);
    }
    #endregion

    #region PlayAudio 播放声音
    /// <summary>
    /// 播放声音
    /// </summary>
    /// <param name="audio"></param>
    /// <param name="type"></param>
    private void PlayAudio(AudioClip audio, AudioType type = AudioType.SoundEffect)
    {
        switch (type)
        {
            case AudioType.Micro:
                m_VoiceSource.volume = 1f;
                m_VoiceSource.clip = audio;
                m_VoiceSource.Play();
                break;
        }
    }
    #endregion

    #region RequestMicroPermission 请求麦克风权限
    /// <summary>
    /// 请求麦克风权限
    /// </summary>
    /// <param name="onRequestMicroCallBack"></param>
    public void RequestMicroPermission(Action<bool> onRequestMicroCallBack)
    {
        m_onRequestMicroCallBack = onRequestMicroCallBack;
#if UNITY_EDITOR
        if (m_onRequestMicroCallBack != null)
        {
            m_onRequestMicroCallBack(true);
        }
#else
        SDK.Instance.IsUseMic(IsUseMicCallBack);
#endif
    }
    #endregion

    #region StartRecord 开始录制
    /// <summary>
    /// 开始录制
    /// </summary>
    /// <param name="onVolumeChanged"></param>
    /// <param name="onMicroStop"></param>
    public void StartRecord(Action<int> onVolumeChanged, Action<bool> onMicroStop)
    {
        m_OnVolumeChanged = onVolumeChanged;
        m_OnMicroStop = onMicroStop;
        m_MicroPhoneSource.Stop();
        StartCoroutine(StartRecord());
    }
    #endregion

    #region IsUseMicCallBack 请求麦克风权限回调
    /// <summary>
    /// 请求麦克风权限回调
    /// </summary>
    /// <param name="isUseMicro"></param>
    private void IsUseMicCallBack(int isUseMicro)
    {
        if (!CanRecord)
        {
            if (m_onRequestMicroCallBack != null)
            {
                m_onRequestMicroCallBack(false);
            }
            return;
        }
        if (m_onRequestMicroCallBack != null)
        {
            m_onRequestMicroCallBack(isUseMicro == 1);
        }
    }
    #endregion

    #region StartRecord 开始录制
    /// <summary>
    /// 开始录制
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartRecord()
    {
        //m_CurrentBGMVolume = AudioSetting.BGMVolume;
        isRecoding = true;

        m_CurrentBGMVolume = SystemProxy.Instance.AudioSetting.BGMVolume;
        AudioBackGroundManager.Instance.SetVolume(RECORDING_BGM_VOLOME);
        
        m_MicroPhoneSource.clip = Microphone.Start(null, false, RECORD_MAX_TIME, FREQUENCY);

        while (Microphone.GetPosition(null) != 0)
        {
            yield return null;
        }

        m_MicroPhoneSource.Play();

        m_CurrentRecordDuration = 0;
        float time = Time.time;
        while (Microphone.IsRecording(null))
        {
            float currentVolume = GetVolume();
            int level = (int)(currentVolume * 99) / 20;
            if (level != m_CurrentVolumeLevel)
            {
                m_CurrentVolumeLevel = level;
                if (m_OnVolumeChanged != null)
                {
                    m_OnVolumeChanged(m_CurrentVolumeLevel);
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);

            m_CurrentRecordDuration = Time.time - time;
        }
        StopRecord();
        yield return null;
    }
    #endregion

    #region StopRecord 停止录制
    /// <summary>
    /// 停止录制
    /// </summary>
    /// <returns></returns>
    public bool StopRecord()
    {
#if !UNITY_EDITOR
        if (!CanRecord) return false;
#endif
        AudioBackGroundManager.Instance.SetVolume(m_CurrentBGMVolume);

        isRecoding = false;

        m_MicroPhoneSource.Stop();

        Microphone.End(null);

        bool isValid = m_CurrentRecordDuration > RECORD_MIN_TIME && m_CurrentRecordDuration < RECORD_MAX_TIME;
        if (m_OnMicroStop != null)
        {
            m_OnMicroStop(isValid);
        }

        return isValid;
    }
    #endregion

    #region GetClipData 获取录制字节数据
    /// <summary>
    /// 获取录制字节数据
    /// </summary>
    /// <returns></returns>
    public byte[] GetClipData()
    {
        if (m_MicroPhoneSource.clip == null)
        {
            Debug.Log("GetClipData audio.clip is null");
            return null;
        }

        int sample = m_MicroPhoneSource.clip.frequency * Mathf.CeilToInt(m_CurrentRecordDuration);
        float[] samples = new float[sample];


        m_MicroPhoneSource.clip.GetData(samples, 0);
        byte[] ret = null;
        using (MemoryStreamExt ms = new MemoryStreamExt())
        {
            for (int i = 0; i < samples.Length; ++i)
            {
                ms.WriteFloat(samples[i]);
            }
            ret = ms.ToArray();
        }
        ret = GZipCompress.Compress(ret);
        return ret;
    }
    #endregion

    #region PlayRecord 播放录制音频
    /// <summary>
    /// 播放录制音频
    /// </summary>
    public void PlayRecord()
    {
        if (m_MicroPhoneSource.clip == null)
        {
            Debug.Log("audio.clip=null");
            return;
        }
        m_MicroPhoneSource.mute = false;
        m_MicroPhoneSource.loop = false;
        m_MicroPhoneSource.Play();
        Debug.Log("PlayRecord");
    }
    #endregion

    #region GetVolume 获取音量
    /// <summary>
    /// 获取音量
    /// </summary>
    /// <returns></returns>
    private float GetVolume()
    {
        if (Microphone.IsRecording(null))
        {
            // 采样数
            int sampleSize = 128;
            float[] samples = new float[sampleSize];
            int startPosition = Microphone.GetPosition(null) - (sampleSize + 1);
            if (startPosition < 0) return 0f;
            // 得到数据
            this.m_MicroPhoneSource.clip.GetData(samples, startPosition);

            float levelMax = 0;
            for (int i = 0; i < sampleSize; ++i)
            {
                float wavePeak = samples[i];
                if (levelMax < wavePeak)
                    levelMax = wavePeak;
            }
            return levelMax;
            //return Mathf.Clamp01(levelMax * 100f);
        }
        return 0;
    }
    #endregion
}
