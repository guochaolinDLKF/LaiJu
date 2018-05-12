//===================================================
//Author      : DRB
//CreateTime  ：5/9/2017 10:15:25 AM
//Description ：
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIItemMatchRankList : MonoBehaviour 
{
    [SerializeField]
    private RawImage m_ImageHead;
    [SerializeField]
    private Text m_TextRanking;
    [SerializeField]
    private Text m_TextNickname;
    [SerializeField]
    private Text m_TextGold;
    [SerializeField]
    private Text m_TextReward;
    [SerializeField]
    private GameObject m_Crown;


    public void SetUI(int ranking,string avatar,string nickName,int gold,string reward)
    {
        if (ranking == 1)
        {
            m_Crown.gameObject.SetActive(true);
        }
        else
        {
            m_Crown.gameObject.SetActive(false);
        }
        TextureManager.Instance.LoadHead(avatar,OnLoadComplete);
        m_TextRanking.SafeSetText(ranking.ToString());
        m_TextNickname.SafeSetText(nickName);
        m_TextGold.SafeSetText(gold.ToString());
        m_TextReward.SafeSetText(reward);
    }

    private void OnLoadComplete(Texture2D tex)
    {
        m_ImageHead.texture = tex;
    }
}
