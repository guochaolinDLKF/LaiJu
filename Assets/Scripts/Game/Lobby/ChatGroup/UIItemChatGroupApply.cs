//===================================================
//Author      : DRB
//CreateTime  ：9/28/2017 7:02:11 PM
//Description ：
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIItemChatGroupApply : UIItemBase 
{
    [SerializeField]
    private RawImage m_ImgHead;
    [SerializeField]
    private Text m_TxtNickname;
    [SerializeField]
    private Text m_TxtPlayerId;
    [SerializeField]
    private Button m_BtnAgree;
    [SerializeField]
    private Button m_BtnRefuse;
    [HideInInspector]
    public int GroupId;
    [HideInInspector]
    public int PlayerId;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_BtnAgree.onClick.AddListener(OnAgreeClick);
        m_BtnRefuse.onClick.AddListener(OnRefuseClick);
    }

    #region OnAgreeClick 同意按钮点击
    /// <summary>
    /// 同意按钮点击
    /// </summary>
    private void OnAgreeClick()
    {
        SendNotification("OnChatGroupAgreeApplyClick", GroupId, PlayerId);
    }
    #endregion

    #region OnRefuseClick 拒绝按钮点击
    /// <summary>
    /// 拒绝按钮点击
    /// </summary>
    private void OnRefuseClick()
    {
        SendNotification("OnChatGroupRefuseApplyClick", GroupId, PlayerId);
    }
    #endregion

    public void SetUI(int groupId, int playerId, string nickName, string avatar)
    {
        GroupId = groupId;
        PlayerId = playerId;
        m_TxtPlayerId.SafeSetText(playerId.ToString());
        m_TxtNickname.SafeSetText(nickName);

    }
}
