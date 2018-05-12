//===================================================
//Author      : DRB
//CreateTime  ：4/19/2017 2:51:39 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRuleView : UIWindowViewBase 
{
    #region
    //[SerializeField]
    //private Image[] m_ArrBtns;
    //[SerializeField]
    //private GameObject[] m_ArrRules;
    //[SerializeField]
    //private Sprite m_DefaultState;
    //[SerializeField]
    //private Sprite m_SelectState;

    //public void SetUI(int index)
    //{
    //    for (int i = 0; i < m_ArrBtns.Length; ++i)
    //    {
    //        bool isSelect = i == index;
    //        if (m_ArrBtns[i] == null) continue;
    //        m_ArrBtns[i].overrideSprite = isSelect ? m_SelectState : m_DefaultState;
    //        if (m_ArrRules[i] == null) continue;
    //        m_ArrRules[i].SetActive(isSelect);
    //    }
    //}

    //protected override void OnBtnClick(GameObject go)
    //{
    //    base.OnBtnClick(go);
    //    for (int i = 0; i < m_ArrBtns.Length; ++i)
    //    {
    //        if (m_ArrBtns[i] == null) continue;
    //        m_ArrBtns[i].overrideSprite = go == m_ArrBtns[i].gameObject ? m_SelectState : m_DefaultState;
    //        if (m_ArrRules[i] == null) continue;
    //        m_ArrRules[i].SetActive(go == m_ArrBtns[i].gameObject);
    //    }
    //}
    #endregion

    [SerializeField]
    private Toggle[] m_ArrToggleGameType;
    [SerializeField]
    private UIItemRuleViewGame[] m_ArrGames;


    private string m_CurrentGameType;

    private UIItemRuleViewGame m_CurrentGame;

    private const string LAST_TYPE_GAME = "LastTypeSettingId";

    private const string LAST_GAME_TYPE = "LastGameType";


    protected override void OnAwake()
    {
        base.OnAwake();

        for (int i = 0; i < m_ArrGames.Length; ++i)
        {
            m_ArrGames[i].Init();
            m_ArrGames[i].onGameChanged = OnGameChanged;
        }

        for (int i = 0; i < m_ArrToggleGameType.Length; ++i)
        {
            m_ArrToggleGameType[i].onValueChanged.AddListener(OnGameTypeChanged);
        }
    }

    #region OnGameTypeChanged 游戏类型变更
    /// <summary>
    /// 游戏类型变更
    /// </summary>
    /// <param name="arg0"></param>
    private void OnGameTypeChanged(bool arg0)
    {
        for (int i = 0; i < m_ArrToggleGameType.Length; ++i)
        {
            if (m_ArrToggleGameType[i].isOn)
            {
                SetCurrentGameType(m_ArrToggleGameType[i].name);
            }
        }

        if (m_CurrentGame == null || !m_CurrentGame.GameType.Equals(m_CurrentGameType, StringComparison.CurrentCultureIgnoreCase))
        {
            int cacheSettingId = PlayerPrefs.GetString(LAST_TYPE_GAME + m_CurrentGameType, string.Empty).ToInt();
            for (int i = 0; i < m_ArrGames.Length; ++i)
            {
                if (cacheSettingId == 0)
                {
                    if (m_ArrGames[i].GameType.Equals(m_CurrentGameType, StringComparison.CurrentCultureIgnoreCase))
                    {
                        SetCurrentGame(m_ArrGames[i]);
                        break;
                    }
                }
                else
                {
                    if (m_ArrGames[i].SettingId == cacheSettingId)
                    {
                        SetCurrentGame(m_ArrGames[i]);
                        break;
                    }
                }
            }
        }
    }
    #endregion

    #region OnGameChanged 游戏变更
    /// <summary>
    /// 游戏变更
    /// </summary>
    /// <param name="arg0"></param>
    private void OnGameChanged(UIItemRuleViewGame game)
    {
        SetCurrentGame(game);
    }
    #endregion

    #region SetCurrentGameType 设置当前游戏类型
    /// <summary>
    /// 设置当前游戏类型
    /// </summary>
    /// <param name="gameType"></param>
    private void SetCurrentGameType(string gameType)
    {
        m_CurrentGameType = gameType;
        for (int i = 0; i < m_ArrGames.Length; ++i)
        {
            if (m_ArrGames[i].GameType.Equals(gameType, StringComparison.CurrentCultureIgnoreCase))
            {
                m_ArrGames[i].ShowTag();
            }
            else
            {
                m_ArrGames[i].HideTag();
            }
        }
    }
    #endregion

   

    #region SetCurrentGame 设置当前游戏
    /// <summary>
    /// 设置当前游戏
    /// </summary>
    /// <param name="gameId"></param>
    private void SetCurrentGame(UIItemRuleViewGame game)
    {
        if (m_CurrentGame != null && m_CurrentGame == game) return;
        m_CurrentGame = game;
        for (int i = 0; i < m_ArrGames.Length; ++i)
        {
            if (m_ArrGames[i] != m_CurrentGame)
            {
                m_ArrGames[i].isOn = false;
                m_ArrGames[i].HideRule();
            }
        }
        m_CurrentGame.isOn = true;
        m_CurrentGame.ShowRule();

        PlayerPrefs.SetString(LAST_TYPE_GAME + game.GameType, game.SettingId.ToString());
        PlayerPrefs.SetString(LAST_GAME_TYPE, game.GameType);
    }
    #endregion

    #region SetGame 设置游戏
    /// <summary>
    /// 设置游戏
    /// </summary>
    /// <param name="gameId"></param>
    public void SetGame(string gameType)
    {
        if (string.IsNullOrEmpty(gameType))
        {
            gameType = PlayerPrefs.GetString(LAST_GAME_TYPE);
        }
        for (int j = 0; j < m_ArrToggleGameType.Length; ++j)
        {
            if (m_ArrToggleGameType[j].name.Equals(gameType, StringComparison.CurrentCultureIgnoreCase))
            {
                m_ArrToggleGameType[j].isOn = true;
                return;
            }
        }
        Debug.Log("居然没有缓存");
        m_ArrToggleGameType[0].isOn = true;
    }
    #endregion

}
