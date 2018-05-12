//===================================================
//Author      : DRB
//CreateTime  ：5/9/2017 10:14:08 AM
//Description ：
//===================================================
using System.Collections.Generic;
using com.oegame.mahjong.protobuf;
using proto.mahjong;
using UnityEngine;
using UnityEngine.UI;

public class UIMatchRankListView : UIWindowViewBase 
{

    [SerializeField]
    private Transform m_Container;
    [SerializeField]
    private Button m_BtnShare;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_BtnShare.gameObject.SetActive(SystemProxy.Instance.IsInstallWeChat);
    }

    public void SetUI(List<OP_MATCH_PLAYER_INFO> lst,MatchHTTPEntity entity)
    {
        if (lst == null || lst.Count == 0 || entity == null) return;
        UIViewManager.Instance.LoadItemAsync("UIItemMatchRankList",(GameObject prefab)=> 
        {
            for (int i = 0; i < lst.Count; ++i)
            {
                GameObject go = Instantiate(prefab);
                go.SetParent(m_Container);
                UIItemMatchRankList item = go.GetComponent<UIItemMatchRankList>();
                string reward = string.Empty;
                if (entity.rewardSetting != null)
                {
                    for (int j = 0; j < entity.rewardSetting.Count; ++j)
                    {
                        if (i + 1 <= entity.rewardSetting[j].end && i + 1 >= entity.rewardSetting[j].begin)
                        {
                            reward = string.Format("房卡×{0}", entity.rewardSetting[j].amount);
                            break;
                        }
                    }
                }
                item.SetUI(i + 1, lst[i].avatar, lst[i].nickname, lst[i].gold, reward);
            }
        });
       
    }


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnMatchRankListViewBack":
                SendNotification("btnMatchRankListViewBack");
                break;
            case "btnMatchRankListViewShare":
                SendNotification("btnMatchRankListViewShare");
                break;
        }
    }

}
