//===================================================
//Author      : DRB
//CreateTime  ：5/19/2017 11:59:35 PM
//Description ：音效管理器
//===================================================
using UnityEngine;
using System.Collections.Generic;

public class AudioEffectManager : SingletonMono<AudioEffectManager>
{
    
    private List<AudioInfo> m_AudioList = new List<AudioInfo>();

    public void SetVolume(float value)
    {
        SystemProxy.Instance.SetSoundEffectVolume(value);
        for (int i = 0; i < m_AudioList.Count; ++i)
        {
            m_AudioList[i].CurrentAudioSource.volume = value;
        }
    }

    public void StopAllAudio()
    {
        for (int i = 0; i < m_AudioList.Count; ++i)
        {
            m_AudioList[i].Destroy();
        }
        m_AudioList.Clear();
    }

    public void Play(string name,Vector3 pos = default(Vector3) , bool is3D = false)
    {
        string audioPath = string.Format("download/{0}/audio/soundeffect/{1}.drb", ConstDefine.GAME_NAME, name);
        int index = name.LastIndexOf('/');
        if (index > -1)
        {
            name = name.Substring(index + 1);
        }
        Play(audioPath, name,pos,is3D);
        Debug.Log("播放声音: " + name);
    }

    private void Play(string audioPath,string name, Vector3 pos, bool is3D = false)
    {
        AssetBundleManager.Instance.LoadOrDownload(audioPath,name,(AudioClip clip)=> 
        {
            if (clip == null) return;
            AudioInfo info = FindSameAudio(clip.name);
            if (info != null)
            {
                info.AudioName = clip.name;
                info.CurrentAudioSource.clip = clip;
                info.Play(pos, is3D);
            }
            else
            {
                RemoveInvalidSound();

                info = new AudioInfo(clip, gameObject);
                m_AudioList.Add(info);

                info.Play(pos, is3D);
            }
        },0);
    }

    private AudioInfo FindSameAudio(string audioName)
    {
        for (int i = 0; i < m_AudioList.Count; ++i)
        {
            if (Time.time > m_AudioList[i].PlayEndTime)
            {
                return m_AudioList[i];
            }
        }

        List<AudioInfo> infoArray = new List<AudioInfo>();
        for (int i = 0; i < m_AudioList.Count; ++i)
        {
            if (m_AudioList[i].AudioName.Equals(audioName))
            {
                infoArray.Add(m_AudioList[i]);
            }
        }

        if (infoArray.Count <= 1)
        {
            infoArray = null;
            return null;
        }

        AudioInfo info = infoArray[0];
        for (int i = 1; i < infoArray.Count; ++i)
        {
            if (info.PlayEndTime > infoArray[i].PlayEndTime)
            {
                info = infoArray[i];
            }
        }
        infoArray = null;
        return info;
    }

    private void RemoveInvalidSound()
    {
        for (int i = m_AudioList.Count - 1; i >= 0; --i)
        {
            if (m_AudioList[i].PlayEndTime <= Time.time)
            {
                m_AudioList[i].Destroy();
                m_AudioList.RemoveAt(i);
            }
        }
    }
}

public class AudioInfo
{
    public AudioSource CurrentAudioSource;

    public string AudioName;

    public float PlayEndTime = 0f;

    public bool Is3D;

    public AudioInfo(AudioClip clip,GameObject root = null)
    {
        AudioName = clip.name;
        GameObject obj = new GameObject("Audio_" + AudioName);
        if (root != null)
        {
            obj.transform.parent = root.transform;
        }

        CurrentAudioSource = obj.AddComponent<AudioSource>();
        CurrentAudioSource.loop = false;
        CurrentAudioSource.volume = SystemProxy.Instance.AudioSetting.SoundEffectVolume;
        CurrentAudioSource.rolloffMode = AudioRolloffMode.Linear;
        CurrentAudioSource.minDistance = 30;
        CurrentAudioSource.maxDistance = 200;
        CurrentAudioSource.clip = clip;
        CurrentAudioSource.panStereo = 0;
    }

    public void Destroy()
    {
        Stop();

        UnityEngine.Object.Destroy(this.CurrentAudioSource.gameObject);
    }

    public void Play(Vector3 pos, bool is3D = false)
    {
        PlayEndTime = Time.time + this.CurrentAudioSource.clip.length;

        CurrentAudioSource.transform.position = pos;
        CurrentAudioSource.Stop();

        CurrentAudioSource.spatialBlend = is3D ? 1 : 0;
        CurrentAudioSource.Play();
    }

    public void Stop()
    {
        CurrentAudioSource.Stop();
        PlayEndTime = 0f;
    }
}
