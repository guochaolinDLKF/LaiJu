//===================================================
//Author      : DRB
//CreateTime  ：9/26/2017 6:54:49 PM
//Description ：聊天群UI项
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 聊天群UI项
/// </summary>
public class UIItemChatGroup : UIItemBase
{
    [SerializeField]
    private Text m_TxtGroupName;
    [SerializeField]
    private Text m_TxtRoomCount;
    [SerializeField]
    private RawImage m_ImgGroupHead;
    [SerializeField]
    private Text m_TxtMemberCount;
    [SerializeField]
    private Text m_TxtGroupID;
    [SerializeField]
    private Button m_Button;
    [HideInInspector]
    public int GroupId;
    [SerializeField]
    private GameObject m_NewTip;
    [SerializeField]
    private GameObject m_Select;

    private static Texture m_DefaultHead;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_Button.onClick.AddListener(OnClick);
        m_DefaultHead = m_ImgGroupHead.texture;
    }

    private void OnClick()
    {
        SendNotification("OnChatGroupClick", GroupId);
    }

    public void SetUI(int groupId, string groupName, string head, int currMemberCount, int maxMemberCount, int roomCount)
    {
        GroupId = groupId;
        m_TxtGroupName.SafeSetText(groupName);
        TextureManager.Instance.LoadHead(head, (Texture2D tex) =>
        {
            if (m_ImgGroupHead != null)
            {
                m_ImgGroupHead.texture = tex != null ? tex : m_DefaultHead;
            }
        });
        m_TxtMemberCount.SafeSetText(string.Format("（人数{0}）", currMemberCount.ToString()));
        m_TxtRoomCount.SafeSetText(string.Format("已开房间({0})", roomCount.ToString()));
        m_TxtGroupID.SafeSetText(string.Format("ID：{0}", groupId.ToString()));
    }

    public void SetNewTip(bool hasNewTip)
    {
        m_NewTip.gameObject.SetActive(hasNewTip);
    }

    public void SetSelect(bool isSelect)
    {
        m_Select.gameObject.SetActive(isSelect);
    }
}
