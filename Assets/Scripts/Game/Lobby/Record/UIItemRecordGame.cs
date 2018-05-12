//===================================================
//Author      : DRB
//CreateTime  ：12/13/2017 4:39:22 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIItemRecordGame : MonoBehaviour
{
    [SerializeField]
    private Toggle m_ToggleGame;

    public string GameType;

    public int GameId;
    public Action<UIItemRecordGame> onGameChanged;

    public void Init()
    {
        m_ToggleGame.onValueChanged.AddListener(OnGameChanged);

    }

    private void OnGameChanged(bool isOn)
    {
        if (isOn)
        {
            if (onGameChanged != null)
            {
                onGameChanged(this);
            }
        }
    }

    public void SetIsShowToggle(bool isShow)
    {
        m_ToggleGame.isOn = isShow;
    }

    public bool GetToggleStart()
    {
        if (m_ToggleGame != null)
        {
            return m_ToggleGame.isOn;
        }
        return false;
    }

}
