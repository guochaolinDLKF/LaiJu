//===================================================
//Author      : DRB
//CreateTime  ：5/4/2017 3:52:57 PM
//Description ：
//===================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMatchDetailView : UIWindowViewBase 
{
    [SerializeField]
    private Text m_TextContent;
    [SerializeField]
    private Button m_ButtonReward;
    [SerializeField]
    private Button m_ButtonRule;

    [SerializeField]
    private Transform m_RecordContainer;


    private GameObject m_Page1;

    private GameObject m_Page2;


    private MatchHTTPEntity m_Entity;


    private List<UIItemRewardDetail> m_List = new List<UIItemRewardDetail>();

    public void SetUI(MatchHTTPEntity entity)
    {
        m_Entity = entity;
        OnRewardClick();
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnMatchDetailViewReward":
                OnRewardClick();
                break;
            case "btnMatchDetailViewDetail":
                OnRuleClick();
                break;
        }
    }

    /// <summary>
    /// 奖励按钮点击
    /// </summary>
    private void OnRewardClick()
    {
        for (int i = 0; i < m_List.Count; ++i)
        {
            m_List[i].gameObject.SetActive(false);
        }

        UIViewManager.Instance.LoadItemAsync("UIItemRewardDetail",(GameObject prefab)=> 
        {
            for (int i = 0; i < m_Entity.reward.Count; ++i)
            {
                UIItemRewardDetail detail = null;
                if (i < m_List.Count)
                {
                    detail = m_List[i];
                    detail.gameObject.SetActive(true);
                }
                else
                {
                    GameObject go = Instantiate(prefab);
                    go.SetParent(m_RecordContainer);
                    detail = go.GetComponent<UIItemRewardDetail>();
                    string[] arr = m_Entity.reward[i].Split('，');
                    string ranking = arr[0];
                    string reward = arr[1];
                    detail.SetUI(ranking, reward);
                    m_List.Add(detail);
                }

            }
        });
    }

    /// <summary>
    /// 规则按钮点击
    /// </summary>
    private void OnRuleClick()
    {
        m_TextContent.SafeSetText(m_Entity.rule);
    }

}
