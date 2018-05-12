//===================================================
//Author      : DRB
//CreateTime  ：5/4/2017 1:22:23 PM
//Description ：比赛窗口
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMatchView : UIWindowViewBase
{
    [SerializeField]
    private Transform m_Container;
    [SerializeField]
    private Text m_txtCards;
    [SerializeField]
    private GameObject m_Tip;
    /// <summary>
    /// 报名按钮委托
    /// </summary>
    public Action<MatchHTTPEntity> OnEnterClick;
    /// <summary>
    /// 查看详情按钮委托
    /// </summary>
    public Action<MatchHTTPEntity> OnSeeDetailClick;
    private List<UIItemMatch> m_List = new List<UIItemMatch>();//UI项列表

    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(AccountProxy.ON_ACCOUNT_CARDS_CHANGED, OnCardsChanged);
        return dic;
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnMatchViewMatchRecord":
                SendNotification("btnMatchViewMatchRecord");
                break;
            case "btnClose":
                AudioBackGroundManager.Instance.Play("bgm_main");
                break;
        }
    }

    public void SetUI(List<MatchHTTPEntity> lst)
    {
        if (lst == null || lst.Count == 0)
        {
            m_Tip.SetActive(true);
            return;
        }
        m_Tip.SetActive(false);

        for (int i = 0; i < m_List.Count; ++i)
        {
            UIPoolManager.Instance.Despawn(m_List[i].transform);
        }
        m_List.Clear();

        for (int i = 0; i < lst.Count; ++i)
        {
            GameObject go = UIPoolManager.Instance.Spawn("UIItemMatch").gameObject;
            go.SetParent(m_Container);
            UIItemMatch battle = go.GetComponent<UIItemMatch>();
            m_List.Add(battle);
            battle.SetUI(lst[i], OnEnterClick, OnSeeDetailClick);
        }
    }


    private void OnCardsChanged(TransferData data)
    {
        int cardCount = data.GetValue<int>("CardCount");
        SetCards(cardCount);
    }

    public void SetCards(int cards)
    {
        m_txtCards.SafeSetText(cards.ToString());
    }
}
