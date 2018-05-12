//===================================================
//Author      : DRB
//CreateTime  ：10/21/2017 10:50:26 AM
//Description ：聊天群战绩UI项
//===================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemChatGroupRecord : UIItemBase
{
    [SerializeField]
    private Transform m_PlayerContainer;
    [SerializeField]
    private Text m_TxtRoomId;
    [SerializeField]
    private Text m_TxtDateTime;
    [SerializeField]
    private Text m_TxtGameName;
    [SerializeField]
    private Text m_TxtTimer;
    public int BattleId;

    private List<UIItemChatGroupRecordPlayer> m_PlayerList = new List<UIItemChatGroupRecordPlayer>();

    public void SetUI(TransferData data)
    {
        BattleId = data.GetValue<int>("BattleId");

        for (int i = 0; i < m_PlayerList.Count; ++i)
        {
            UIPoolManager.Instance.Despawn(m_PlayerList[i].transform);
        }
        m_PlayerList.Clear();

        m_TxtRoomId.SafeSetText(data.GetValue<int>("RoomId").ToString());

        string timeStr = data.GetValue<string>("DateTime");
        string[] timeStrYMDHMS = timeStr.Split(' ');
        if (timeStrYMDHMS != null && timeStrYMDHMS.Length > 0) m_TxtDateTime.SafeSetText(timeStrYMDHMS[0]);
        if (timeStrYMDHMS != null && timeStrYMDHMS.Length > 1) m_TxtTimer.SafeSetText(timeStrYMDHMS[1]);

        m_TxtGameName.SafeSetText(data.GetValue<string>("GameName"));
        List<TransferData> playerData = data.GetValue<List<TransferData>>("Player");
        if (playerData == null) return;
        for (int i = 0; i < playerData.Count; ++i)
        {
            UIItemChatGroupRecordPlayer item = UIPoolManager.Instance.Spawn("UIItemChatGroupRecordPlayer").GetComponent<UIItemChatGroupRecordPlayer>();
            item.SetUI(playerData[i]);
            item.gameObject.SetParent(m_PlayerContainer);
            m_PlayerList.Add(item);
        }
    }
}
