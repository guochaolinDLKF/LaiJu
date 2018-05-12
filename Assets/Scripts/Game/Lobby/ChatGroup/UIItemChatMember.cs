//===================================================
//Author      : DRB
//CreateTime  ：9/26/2017 8:12:59 PM
//Description ：聊天群成员UI项
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 聊天群成员UI项
/// </summary>
public class UIItemChatMember : UIItemBase
{
    [SerializeField]
    private RawImage m_ImgHead;
    [SerializeField]
    private Text m_TxtNickName;
    [SerializeField]
    private Text m_TxtPlayerId;
    [SerializeField]
    private Button m_BtnOption;
    [SerializeField]
    private Button m_BtnBG;
    [SerializeField]
    private Image m_ImgOwner;
    [SerializeField]
    private Image m_ImgManager;
    [SerializeField]
    private Image m_ImgOnline;
    [SerializeField]
    private Image m_ImgNoOnline;

    public Transform OptionContainer;
    [HideInInspector]
    public int PlayerId;
    [HideInInspector]
    public int GroupId;

    private float m_RectX;

    private static Texture m_DefaultHead;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_BtnOption.onClick.AddListener(OnOptionClick);
        if (m_BtnBG != null) m_BtnBG.onClick.AddListener(OnOptionClick);
        m_RectX = m_ImgManager.rectTransform.sizeDelta.x;

        m_DefaultHead = m_ImgHead.texture;
    }

    private void OnOptionClick()
    {
        SendNotification("OnChatMemberClick", GroupId, PlayerId);
    }

    public void SetUI(int groupId, int playerId, string nickname, bool isOnline, string avatar, bool isOwner,bool isManager, bool playerIsOwner, bool playerIsManager)
    {
        PlayerId = playerId;
        GroupId = groupId;
        m_TxtNickName.SafeSetText(nickname);
        m_TxtPlayerId.SafeSetText(string.Format("ID:{0}", playerId.ToString()));
        m_ImgOwner.gameObject.SetActive(isOwner);
        m_ImgManager.gameObject.SetActive(isManager && !isOwner);
        m_ImgOnline.gameObject.SetActive(isOnline);
        m_ImgNoOnline.gameObject.SetActive(!isOnline);
        TextureManager.Instance.LoadHead(avatar, (Texture2D tex) =>
         {
             if (m_ImgHead != null)
             {
                 m_ImgHead.texture = tex != null ? tex : m_DefaultHead;
             }
         });

        if (m_BtnOption != null)
        {
            if ((playerIsOwner && !isOwner) || (playerIsManager && !isManager && !isOwner))
            {
                m_BtnOption.gameObject.SetActive(true);
            }
            else
            {
                m_BtnOption.gameObject.SetActive(false);
            }
        }

    }
}
