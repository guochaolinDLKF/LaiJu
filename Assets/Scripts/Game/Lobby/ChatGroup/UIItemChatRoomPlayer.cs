//===================================================
//Author      : DRB
//CreateTime  ：9/27/2017 11:00:21 AM
//Description ：
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIItemChatRoomPlayer : UIItemBase 
{
    [SerializeField]
    private RawImage m_ImgHead;
    [SerializeField]
    private Button m_Btn;
    [SerializeField]
    private Image m_ImgClose;
    [HideInInspector]
    public int GroupId;
    [HideInInspector]
    public int RoomId;
    [HideInInspector]
    public int PlayerId;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_Btn.onClick.AddListener(OnHeadClick);
    }

    private void OnHeadClick()
    {
        SendNotification("OnChatRoomPlayerClick", GroupId, RoomId, PlayerId);
    }

    public void SetUI(int groupId, int roomId, int playerId, string avatar, bool isManager)
    {
        GroupId = groupId;
        RoomId = roomId;
        PlayerId = playerId;
        m_ImgClose.gameObject.SetActive(isManager);
        if (playerId > 0)
        {
            m_ImgHead.gameObject.SetActive(true);
            TextureManager.Instance.LoadHead(avatar, (Texture2D tex) =>
            {
                if (m_ImgHead != null && tex != null)
                {
                    m_ImgHead.gameObject.SetActive(true);
                    m_ImgHead.texture = tex;
                }
            });
        }
        else
        {
            m_ImgHead.gameObject.SetActive(false);
        }

    }

    public void SetAuthority(bool isManager)
    {
        m_ImgClose.gameObject.SetActive(isManager);
    }
}
