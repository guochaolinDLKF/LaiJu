//===================================================
//Author      : DRB
//CreateTime  ：9/28/2017 11:39:35 AM
//Description ：
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonState : MonoBehaviour 
{
    private Button m_Button;

    public Sprite NormalSprite;

    public Sprite SelectSprite;

    private bool m_isSelect = false;

    private void Awake()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        m_isSelect = !m_isSelect;
        m_Button.GetComponent<Image>().overrideSprite = m_isSelect ? SelectSprite : NormalSprite;
    }
}
