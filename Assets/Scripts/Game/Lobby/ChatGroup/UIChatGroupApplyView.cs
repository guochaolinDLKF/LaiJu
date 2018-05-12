//===================================================
//Author      : DRB
//CreateTime  ：9/28/2017 6:12:43 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;


public class UIChatGroupApplyView : UIWindowViewBase 
{
    [SerializeField]
    private Transform m_Container;

    private List<UIItemChatGroupApply> m_PlayerList = new List<UIItemChatGroupApply>();


    private int m_GroupId;

    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();

        dic.Add(ChatGroupProxy.ON_GROUP_ADD_APPLY, OnAddApply);
        dic.Add(ChatGroupProxy.ON_GROUP_REMOVE_APPLY, OnRemoveApply);

        return dic;
    }

    /// <summary>
    /// 添加申请
    /// </summary>
    /// <param name="data"></param>
    private void OnAddApply(TransferData data)
    {
        int groupId = data.GetValue<int>("GroupId");
        if (groupId != m_GroupId) return;
        PlayerEntity player = data.GetValue<PlayerEntity>("PlayerEntity");

        UIItemChatGroupApply item = UIPoolManager.Instance.Spawn("UIItemChatGroupApply").GetComponent<UIItemChatGroupApply>();
        item.SetUI(groupId, player.id, player.nickname, player.avatar);
        item.gameObject.SetParent(m_Container);
        m_PlayerList.Add(item);
    }
    
    /// <summary>
    /// 移除申请
    /// </summary>
    /// <param name="obj"></param>
    private void OnRemoveApply(TransferData data)
    {

        int groupId = data.GetValue<int>("GroupId");
        if (groupId != m_GroupId) return;
        int playerId = data.GetValue<int>("PlayerId");
        for (int i = 0; i < m_PlayerList.Count; ++i)
        {
            if (m_PlayerList[i].PlayerId == playerId)
            {
                UIPoolManager.Instance.Despawn(m_PlayerList[i].transform);
                m_PlayerList.Remove(m_PlayerList[i]);
                break;
            }
        }
    }

    public void SetUI(int groupId)
    {
        m_GroupId = groupId;
    }
}
