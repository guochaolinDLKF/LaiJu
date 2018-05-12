//===================================================
//Author      : DRB
//CreateTime  ：7/6/2017 10:31:38 AM
//Description ：
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIItemGame : UIItemBase 
{
    [SerializeField]
    private Button m_Button;
    [SerializeField]
    private Image m_Image;

    private int m_GameId;
    private Action<int> m_OnClick;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_Button.onClick.AddListener(OnBtnClick);
    }

    private void OnBtnClick()
    {
        if (m_OnClick != null)
        {
            m_OnClick(m_GameId);
        }
    }

    public void SetUI(int gameId,string icon,Action<int> onClick)
    {
        m_GameId = gameId;
        m_OnClick = onClick;
        string path = string.Format("download/{0}/source/uisource/gameuisource/game.drb",ConstDefine.GAME_NAME);
        Sprite sprite = AssetBundleManager.Instance.LoadSprite(path, icon);
        m_Image.overrideSprite = sprite;
    }

}
