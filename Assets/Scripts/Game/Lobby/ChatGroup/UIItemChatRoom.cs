//===================================================
//Author      : DRB
//CreateTime  ：9/27/2017 10:57:04 AM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemChatRoom : UIItemBase 
{
    [SerializeField]
    private Button m_BtnDetail;
    [SerializeField]
    private Button m_BtnInvite;
    [SerializeField]
    private Transform m_PlayerContainer;
    [SerializeField]
    private Image m_ImgWait;
    [SerializeField]
    private Image m_ImgGaming;
    [SerializeField]
    private Text m_GameName;

    public int RoomId;

    public int GroupId;

    public List<UIItemChatRoomPlayer> PlayerList;

    protected override void OnAwake()
    {
        base.OnAwake();
        if (m_BtnDetail != null) m_BtnDetail.onClick.AddListener(OnDetailClick);
        if (m_BtnInvite != null) m_BtnInvite.onClick.AddListener(OnInviteClick);

        if (!(SystemProxy.Instance.IsInstallWeChat && SystemProxy.Instance.IsOpenWXLogin))
        {
            if (m_BtnInvite != null) m_BtnInvite.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 详情按钮点击
    /// </summary>
    private void OnDetailClick()
    {
        SendNotification("OnChatRoomDetailClick", GroupId, RoomId);
    }

    /// <summary>
    /// 邀请按钮点击
    /// </summary>
    private void OnInviteClick()
    {
        SendNotification("OnChatRoomInviteClick",GroupId, RoomId);
    }

    public void SetUI(int groupId, int roomId, bool isPlaying , string gameName)
    {
        RoomId = roomId;
        GroupId = groupId;
        m_ImgWait.gameObject.SetActive(!isPlaying);
        m_ImgGaming.gameObject.SetActive(isPlaying);
        m_GameName.SafeSetText(gameName);
    }

    public void SetPlayer(List<UIItemChatRoomPlayer> lst)
    {
        PlayerList = lst;
        for (int i = 0; i < lst.Count; ++i)
        {
            lst[i].gameObject.SetParent(m_PlayerContainer);
        }
    }
}
