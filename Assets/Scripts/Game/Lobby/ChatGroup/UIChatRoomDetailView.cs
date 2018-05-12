//===================================================
//Author      : DRB
//CreateTime  ：9/27/2017 12:00:06 PM
//Description ：群友会房间详情窗口
//===================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 群友会房间详情窗口
/// </summary>
public class UIChatRoomDetailView : UIWindowViewBase
{
    [SerializeField]
    private Text m_TxtRoomId;
    [SerializeField]
    private Text m_TxtGameName;
    [SerializeField]
    private Text m_TxtPlayerCount;
    [SerializeField]
    private Text m_TxtGameLoop;
    [SerializeField]
    private Text m_TxtSetting;
    [SerializeField]
    private Transform m_PlayerContainer;
    [SerializeField]
    private Button m_BtnDisband;
    [SerializeField]
    private GameObject m_ruleView;

    private int m_GroupId;

    private int m_RoomId;


    private List<UIItemChatRoomDetailPlayer> m_ItemList = new List<UIItemChatRoomDetailPlayer>();

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnChatRoomDetailJoin":
                SendNotification("btnChatRoomDetailJoin", m_GroupId, m_RoomId);
                break;
            case "btnChatRoomDetailDisband":
                SendNotification("btnChatRoomDetailDisband", m_GroupId, m_RoomId);
                break;
            case "btnChatRoomRuleViewer":
                SetRuleViewActive();
                break;
        }
    }

    /// <summary>
    /// 设置房间信息
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="roomId"></param>
    /// <param name="gameName"></param>
    /// <param name="playerCount"></param>
    /// <param name="currLoop"></param>
    /// <param name="maxLoop"></param>
    public void SetRoomInfo(int groupId, int roomId, string gameName, int playerCount, int currLoop, int maxLoop, bool isManager)
    {
        m_GroupId = groupId;
        m_RoomId = roomId;
        m_TxtRoomId.SafeSetText(roomId.ToString());
        m_TxtGameName.SafeSetText(gameName);
        m_TxtPlayerCount.SafeSetText(playerCount.ToString());
        m_TxtGameLoop.SafeSetText(string.Format("{0}/{1}", currLoop.ToString(), maxLoop.ToString()));
        if (m_BtnDisband != null) m_BtnDisband.gameObject.SetActive(isManager);
        if (m_ruleView != null) m_ruleView.SetActive(false);
    }

    /// <summary>
    /// 设置房间玩家
    /// </summary>
    /// <param name="players"></param>
    public void SetRoomPlayer(List<PlayerEntity> players)
    {
        for (int i = 0; i < m_ItemList.Count; ++i)
        {
            UIPoolManager.Instance.Despawn(m_ItemList[i].transform);
        }
        m_ItemList.Clear();
        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i] == null) continue;
            if (players[i].id == 0) continue;
            UIItemChatRoomDetailPlayer item = UIPoolManager.Instance.Spawn("UIItemChatRoomDetailPlayer").GetComponent<UIItemChatRoomDetailPlayer>();
            item.gameObject.SetParent(m_PlayerContainer);
            item.SetUI(players[i].id, players[i].nickname, players[i].avatar, players[i].isOwner);
            m_ItemList.Add(item);
        }
    }

    /// <summary>
    /// 设置房间规则
    /// </summary>
    /// <param name="setting"></param>
    public void SetRoomSetting(string setting)
    {
        m_TxtSetting.SafeSetText(setting);
    }


    private void SetRuleViewActive()
    {
        if (m_ruleView != null)
            m_ruleView.SetActive(!m_ruleView.activeSelf);

    }

}
