//===================================================
//Author      : DRB
//CreateTime  ：5/9/2017 9:44:27 AM
//Description ：
//===================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfoView : UIWindowViewBase 
{
    [SerializeField]
    private GameObject[] m_Pages;
    [SerializeField]
    private Text m_CardsCount;
    [SerializeField]
    private InputField m_PlayerId;
    [SerializeField]
    private UIPagePlayerRecharge m_UIPagePlayerRecharge;

    public UIPageRechargeRecord UIPageRechargeRecord;



    /// <summary>
    /// 充值数量
    /// </summary>
    public int RechargeCount
    {
        get
        {
            if (m_UIPagePlayerRecharge == null) return 0;
            return m_UIPagePlayerRecharge.CurrentCardsCount;
        }
    }

    public int TargetPlayerId
    {
        get
        {
            return m_PlayerId.text.ToInt();
        }
    }


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnPlayerInfoViewPlayerRecharge"://玩家充值
                SendNotification("btnPlayerInfoViewPlayerRecharge");
                break;
            case "btnPlayerInfoViewRechargeRecord"://充值记录
                SendNotification("btnPlayerInfoViewRechargeRecord");
                break;
            case "btnPlayerInfoViewReceiveDividents"://领取分红
                break;
            case "btnPlayerInfoViewNext"://下一步
                SendNotification("btnPlayerInfoViewNext");
                break;
            case "btnPlayerInfoViewBack"://返回
                SendNotification("btnPlayerInfoViewBack");
                break;
            case "btnPlayerInfoViewRecharge"://充值
                SendNotification("btnPlayerInfoViewRecharge");
                break;
            case "btnPlayerInfoViewQuery"://查询
                SendNotification("btnPlayerInfoViewQuery");
                break;
            case "btnPlayerInfoViewSubmit"://提交
                break;
        }
    }


    public void SwitchPage(int currentPage)
    {
        for (int i = 0; i < m_Pages.Length; ++i)
        {
            m_Pages[i].SetActive(currentPage == i);
        }
    }

    public void SetCards(int cards)
    {
        SwitchPage(0);
        m_CardsCount.SafeSetText(cards.ToString());
    }

    public void SetPlayerInfo(int playerId,int targetPlayerId,string nickname,int cardsCount)
    {
        SwitchPage(1);
        m_UIPagePlayerRecharge.SetUI(playerId, targetPlayerId, nickname, cardsCount);
    }
    public void SetPlayerInfo(int playerCards,int cardsCount)
    {
        m_UIPagePlayerRecharge.SetUI(playerCards,cardsCount);
    }


    public void SetRechargeRecord(List<RechargeRecordEntity> lst)
    {
        SwitchPage(2);
        UIPageRechargeRecord.SetUI(lst);
    }
}
