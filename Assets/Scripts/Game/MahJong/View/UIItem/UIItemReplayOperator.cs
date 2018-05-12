//===================================================
//Author      : DRB
//CreateTime  ：5/3/2017 10:04:19 AM
//Description ：
//===================================================
using System;
using DRB.MahJong;
using UnityEngine;
using UnityEngine.UI;

public class UIItemReplayOperator : MonoBehaviour 
{
    [SerializeField]
    private Button m_ButtonFF;
    [SerializeField]
    private Button m_ButtonFB;
    [SerializeField]
    private Button m_ButtonStop;
    [SerializeField]
    private Button m_ButtonPlay;
    [SerializeField]
    private Button m_ButtonBack;


    private const float MAX_TIME_SCALE = 4f;

    private const float MIN_TIME_SCALE = 0.25f;

    private void Start()
    {
        if (!RoomMaJiangProxy.Instance.CurrentRoom.isReplay)
        {
            this.gameObject.SetActive(false);
        }

        m_ButtonFF.onClick.AddListener(OnFFClick);
        m_ButtonFB.onClick.AddListener(OnFBClick);
        m_ButtonStop.onClick.AddListener(OnStopClick);
        m_ButtonPlay.onClick.AddListener(OnPlayClick);
        m_ButtonPlay.gameObject.SetActive(false);
        m_ButtonBack.onClick.AddListener(OnBackClick);
    }

    private void OnFFClick()
    {
        Time.timeScale *= 2f;
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            m_ButtonStop.gameObject.SetActive(true);
            m_ButtonPlay.gameObject.SetActive(false);
        }
        Time.timeScale = Mathf.Clamp(Time.timeScale, MIN_TIME_SCALE, MAX_TIME_SCALE);
    }

    private void OnFBClick()
    {
        Time.timeScale /= 2f;
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            m_ButtonStop.gameObject.SetActive(true);
            m_ButtonPlay.gameObject.SetActive(false);
        }
        Time.timeScale = Mathf.Clamp(Time.timeScale, MIN_TIME_SCALE, MAX_TIME_SCALE);
    }

    private void OnStopClick()
    {
        Time.timeScale = 0;
        m_ButtonStop.gameObject.SetActive(false);
        m_ButtonPlay.gameObject.SetActive(true);
    }

    private void OnPlayClick()
    {
        Time.timeScale = 1f;
        m_ButtonStop.gameObject.SetActive(true);
        m_ButtonPlay.gameObject.SetActive(false);
    }

    private void OnBackClick()
    {
        SceneMgr.Instance.LoadScene(SceneType.Main);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
