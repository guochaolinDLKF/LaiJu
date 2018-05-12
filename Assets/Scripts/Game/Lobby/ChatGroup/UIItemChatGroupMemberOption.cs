//===================================================
//Author      : DRB
//CreateTime  ：10/23/2017 10:16:23 AM
//Description ：群友会成员选项UI项
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIItemChatGroupMemberOption : UIItemBase 
{
    [SerializeField]
    private Button m_BtnAppoint;
    [SerializeField]
    private Button m_BtnDimission;
    [SerializeField]
    private Button m_BtnKick;
    [SerializeField]
    private Button m_BtnClose;

    private int m_GroupId;

    private int m_PlayerId;


    protected override void OnAwake()
    {
        base.OnAwake();
        m_BtnAppoint.onClick.AddListener(OnAppointClick);
        m_BtnDimission.onClick.AddListener(OnDimissionClick);
        m_BtnKick.onClick.AddListener(OnKickClick);
        m_BtnClose.onClick.AddListener(OnCloseClick);
    }

    private void OnAppointClick()
    {
        SendNotification("btnChatGroupViewAppoint", m_GroupId, m_PlayerId);
        Hide();
    }

    private void OnDimissionClick()
    {
        SendNotification("btnChatGroupViewDimission", m_GroupId, m_PlayerId);
        Hide();
    }

    private void OnKickClick()
    {
        SendNotification("btnChatGroupViewKick", m_GroupId, m_PlayerId);
        Hide();
    }

    private void OnCloseClick()
    {
        Hide();
    }

    public void SetUI(int groupId, int playerId, bool canAppoint, bool canDimission, bool canKick)
    {
        m_GroupId = groupId;
        m_PlayerId = playerId;
        m_BtnAppoint.gameObject.SetActive(canAppoint);
        m_BtnDimission.gameObject.SetActive(canDimission);
        m_BtnKick.gameObject.SetActive(canKick);
    }


    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
