//===================================================
//Author      : DRB
//CreateTime  ：11/7/2017 2:33:33 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRecordMahjong : UIRecordBase
{

    protected override string RecordPrefabName
    {
        get
        {
            return "UIItemRecordInfo";
        }
    }

    [SerializeField]
    private Toggle[] m_btnGame;//游戏类型按钮

    [SerializeField]
    private UIItemRecordGame[] m_ArrGames;//具体游戏战绩按钮

    protected override void OnAwake()
    {
        base.OnAwake();

        Debug.Log(m_btnGame.Length);
        for (int i = 0; i < m_btnGame.Length; ++i)
        {
            m_btnGame[i].onValueChanged.AddListener((bool value) =>
            {
                if (value) { OnGameTypeRecordChanged(); }
            });

        }

        for (int i = 0; i < m_ArrGames.Length; ++i)
        {
            m_ArrGames[i].Init();
            m_ArrGames[i].onGameChanged = OnGameRecordChanged;
        }

     
    }

    /// <summary>
    /// 游戏类型变更
    /// </summary>
    private void OnGameTypeRecordChanged()
    {
        for (int i = 0; i < m_btnGame.Length; ++i)
        {
            if (m_btnGame[i].isOn)
            {
                string gameType = m_btnGame[i].gameObject.name;
                SetShowArrGames(gameType);

                List<UIItemRecordGame> itemRecordGame = GetArrGamesToType(gameType);

                for (int j = 0; j < itemRecordGame.Count; ++j)
                {
                    if (itemRecordGame[j].GetToggleStart())
                    {
                        OnGameRecordChanged(itemRecordGame[j]);
                        return;
                    }
                }

                if (itemRecordGame.Count > 0)
                {
                    OnGameRecordChanged(itemRecordGame[0]);
                }

                break;
            }
        }


        
    }

    #region OnGameChanged 游戏变更
    /// <summary>
    /// 游戏变更
    /// </summary>
    /// <param name="arg0"></param>
    private void OnGameRecordChanged(UIItemRecordGame game)
    {
        //以ID获得战绩
        GameObject go = game.gameObject;
        for (int i = 0; i < m_ArrGames.Length; ++i)
        {
            if (go == m_ArrGames[i].gameObject)
            {
                SendNotification("btnRecordViewGame", game.GameId);
                break;
            }
        }
    }
    #endregion


    public override void ShowRecord(TransferData data)
    {
        base.ShowRecord(data);
        string gameType = data.GetValue<string>("GameType");
        int gameId = data.GetValue<int>("GameId");

        SetShowArrGames(gameType);
        for (int i = 0; i < m_ArrGames.Length; ++i)
        {
            if (gameId==m_ArrGames[i].GameId)
            {
                m_ArrGames[i].SetIsShowToggle(true);
                break;
            }
        }

        for (int i = 0; i < m_btnGame.Length; ++i)
        {
            if (m_btnGame[i].gameObject.name.Equals(gameType, StringComparison.CurrentCultureIgnoreCase) )
            {
                m_btnGame[i].isOn=true;
                break;
            }
        }


    }

    public void SetShowArrGames(string gameType)
    {
        for (int i = 0; i < m_ArrGames.Length; ++i)
        {
            m_ArrGames[i].gameObject.SetActive(gameType.Equals(m_ArrGames[i].GameType, StringComparison.CurrentCultureIgnoreCase));
        }

    }

    public List<UIItemRecordGame> GetArrGamesToType(string gameType)
    {
        List<UIItemRecordGame> itemRecordGame = new List<UIItemRecordGame>();
        for (int i = 0; i < m_ArrGames.Length; ++i)
        {
            if (gameType.Equals(m_ArrGames[i].GameType, StringComparison.CurrentCultureIgnoreCase))
            {
                itemRecordGame.Add(m_ArrGames[i]);
            }
        }
        return itemRecordGame;
    }

}
