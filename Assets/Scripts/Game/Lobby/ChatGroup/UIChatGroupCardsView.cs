//===================================================
//Author      : DRB
//CreateTime  ：10/24/2017 1:48:53 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChatGroupCardsView : UIWindowViewBase
{
    [SerializeField]
    private InputField m_InputCards;
    [SerializeField]
    private Text m_TxtGroupCardCount;
    [SerializeField]
    private Text m_TxtPlayerCardCount;

    private int m_CurrentCardCount;

    private int m_PlayerCardCount;

    private int m_GroupCardCount;

    private int m_GroupId;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_InputCards.onEndEdit.AddListener(OnInputFieldEndEdit);
    }

    private void OnInputFieldEndEdit(string value)
    {
        m_CurrentCardCount = value.ToInt();
        m_CurrentCardCount = Mathf.Clamp(m_CurrentCardCount, 0, Mathf.Max(m_GroupCardCount, m_PlayerCardCount));
        m_InputCards.text = m_CurrentCardCount.ToString();
    }

    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(ChatGroupProxy.ON_GROUP_INFO_CHANGED,OnGroupInfoChanged);
        dic.Add(AccountProxy.ON_ACCOUNT_CARDS_CHANGED,OnAccountCardsChanged);
        return dic;
    }

    private void OnGroupInfoChanged(TransferData data)
    {
        ChatGroupEntity group = data.GetValue<ChatGroupEntity>("GroupEntity");
        if (group == null) return;
        m_GroupCardCount = group.cards;
        m_TxtGroupCardCount.SafeSetText(group.cards.ToString());
    }

    private void OnAccountCardsChanged(TransferData data)
    {
        m_PlayerCardCount = data.GetValue<int>("CardCount");
        m_TxtPlayerCardCount.SafeSetText(m_PlayerCardCount.ToString());
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnChatGroupViewDeposit"://存钱
                SendNotification("btnChatGroupViewDeposit", m_GroupId, m_CurrentCardCount);
                break;
            case "btnChatGroupViewEnchashment"://提现
                SendNotification("btnChatGroupViewEnchashment", m_GroupId, m_CurrentCardCount);
                break;
            case "btnChatGroupViewAddCard"://增加
                m_CurrentCardCount += 10;
                m_CurrentCardCount = Mathf.Clamp(m_CurrentCardCount, 0, Mathf.Max(m_GroupCardCount, m_PlayerCardCount));
                m_InputCards.text = m_CurrentCardCount.ToString();
                break;
            case "btnChatGroupViewReduceCard"://减少
                m_CurrentCardCount -= 10;
                m_CurrentCardCount = Mathf.Clamp(m_CurrentCardCount, 0, Mathf.Max(m_GroupCardCount, m_PlayerCardCount));
                m_InputCards.text = m_CurrentCardCount.ToString();
                break;
        }
    }


    public void SetUI(TransferData data)
    {
        m_GroupId = data.GetValue<int>("GroupId");
        m_GroupCardCount = data.GetValue<int>("GroupCardCount");
        m_PlayerCardCount = data.GetValue<int>("PlayerCardCount");

        m_InputCards.text = string.Empty;
        m_CurrentCardCount = 0;
        m_TxtGroupCardCount.SafeSetText(m_GroupCardCount.ToString());
        m_TxtPlayerCardCount.SafeSetText(m_PlayerCardCount.ToString());
    }
}
