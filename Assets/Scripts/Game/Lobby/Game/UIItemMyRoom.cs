//===================================================
//Author      : DRB
//CreateTime  ：7/6/2017 11:43:20 AM
//Description ：
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIItemMyRoom : UIItemBase 
{
    [SerializeField]
    private Text m_TextId;
    [SerializeField]
    private Text m_TextGameName;
    [SerializeField]
    private Text m_TextLoop;
    [SerializeField]
    private Text m_TextPlayerCount;
    [SerializeField]
    private Text m_TextOwnerName;
    [SerializeField]
    private Button m_ButtonJoin;
    [SerializeField]
    private Button m_ButtonInvite;
    [SerializeField]
    private Button m_ButtonPlayer;
    [SerializeField]
    private Text m_TextPayment;

    private int m_nId;

    private Action<int> m_OnJoinClick;

    private Action<int> m_OnInviteClick;

    private Action<int,Transform> m_OnPlayerClick;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_ButtonJoin.onClick.AddListener(OnBtnJoinClick);
        m_ButtonInvite.onClick.AddListener(OnBtnInviteClick);
        m_ButtonPlayer.onClick.AddListener(OnBtnPlayerClick);
    }

    private void OnBtnJoinClick()
    {
        if (m_OnJoinClick != null)
        {
            m_OnJoinClick(m_nId);
        }
    }

    private void OnBtnInviteClick()
    {
        if (m_OnInviteClick != null)
        {
            m_OnInviteClick(m_nId);
        }
    }

    private void OnBtnPlayerClick()
    {
        if (m_OnPlayerClick != null)
        {
            m_OnPlayerClick(m_nId, m_ButtonPlayer.transform);
        }
    }

    public void SetUI(int id,string gameName,int currentLoop,int maxLoop,int currentPlayerCount,int maxPlayerCount, string ownerName, CardPaymentType payment, Action<int> onJoinClick, Action<int> onInviteClick,Action<int,Transform> onPlayerClick, bool canInvite)
    {
        m_nId = id;
        m_TextId.SafeSetText(id.ToString());
        m_TextGameName.SafeSetText(gameName);
        m_TextLoop.SafeSetText(string.Format("{0}/{1}", currentLoop.ToString(), maxLoop.ToString()));
        m_TextPlayerCount.SafeSetText(string.Format("{0}/{1}", currentPlayerCount.ToString(), maxPlayerCount.ToString()));
        m_TextOwnerName.SafeSetText(ownerName);

        m_TextPayment.SafeSetText(payment == CardPaymentType.AA?"AA":"房主");

        m_OnJoinClick = onJoinClick;
        m_OnInviteClick = onInviteClick;
        m_OnPlayerClick = onPlayerClick;

        m_ButtonInvite.gameObject.SetActive(canInvite);
    }
}
