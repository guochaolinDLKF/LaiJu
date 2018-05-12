//===================================================
//Author      : DRB
//CreateTime  ：4/12/2017 10:57:26 AM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSettingMenu : MonoBehaviour
{
    [SerializeField]
    private Button m_Button;
    [SerializeField]
    private Text m_TextGameName;

    public int GameId;

    private Action<int> m_OnClick;

    [SerializeField]
    private Sprite[] m_SpriteButtonState;

    private void Awake()
    {
        m_Button.onClick.AddListener(OnBtnClick);
    }

    public void SetUI(int gameId, string gameName, Action<int> onClick)
    {
        GameId = gameId;
        m_TextGameName.SafeSetText(gameName);
        m_OnClick = onClick;
    }

    public void SetState(bool isSelect)
    {
        m_Button.image.overrideSprite = m_SpriteButtonState[isSelect ? 1 : 0];
    }

    private void OnBtnClick()
    {
        AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);
        if (m_OnClick != null)
        {
            m_OnClick(GameId);
        }
    }
    
}
