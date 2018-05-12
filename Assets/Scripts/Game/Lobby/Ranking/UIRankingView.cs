//===================================================
//Author      : DRB
//CreateTime  ：7/10/2017 2:51:24 PM
//Description ：排行榜窗口
//===================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRankingView : UIWindowViewBase 
{
    [SerializeField]
    private Transform m_Container;
    [SerializeField]
    private Text m_Description;
    [SerializeField]
    private Text m_txtOwnerScore;

    private string m_Content;


    private List<UIItemRanking> m_Cache = new List<UIItemRanking>();

    protected override void OnAwake()
    {
        base.OnAwake();
        if (m_Description != null)
        {
            m_Content = m_Description.text;
        }
    }


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "BtnScoreList":
                SendNotification("BtnScoreList");
                break;
            case "BtnCountList":
                SendNotification("BtnCountList");
                break;
        }
    }

    public void SetUI(TransferData data)
    {
        List<TransferData> lstRanking = data.GetValue<List<TransferData>>("Ranking");
        RankingListType rankingType = data.GetValue<RankingListType>("RankingType");
        for (int i = 0; i < m_Cache.Count; ++i)
        {
            UIPoolManager.Instance.Despawn(m_Cache[i].transform);
        }
        m_Cache.Clear();
        for (int i = 0; i < lstRanking.Count; ++i)
        {
            string nickname = lstRanking[i].GetValue<string>("NickName");
            string avatar = lstRanking[i].GetValue<string>("Avatar");
            int score = lstRanking[i].GetValue<int>("Score");
            UIItemRanking item = UIPoolManager.Instance.Spawn("UIItemRanking").GetComponent<UIItemRanking>();
            m_Cache.Add(item);
            item.gameObject.SetParent(m_Container);
            item.SetUI(i + 1, nickname, avatar, score, rankingType);
        }

        if (m_Description != null)
        {
            m_Description.SafeSetText(string.Format(m_Content,rankingType == RankingListType.Count? "局数" : "积分"));
        }
    }

    public void SetOwnerRanking(TransferData data)
    {
        int score = data.GetValue<int>("Score");
        m_txtOwnerScore.SafeSetText(score.ToString());
    }
}
