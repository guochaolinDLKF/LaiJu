//===================================================
//Author      : DRB
//CreateTime  ：5/11/2017 6:39:13 PM
//Description ：
//===================================================
using UnityEngine;
using UnityEngine.UI;

public class UIItemRewardDetail : MonoBehaviour 
{
    [SerializeField]
    private Text m_TextRanking;
    [SerializeField]
    private Text m_TextReward;

    public void SetUI(string ranking,string reward)
    {
        m_TextRanking.SafeSetText(ranking);
        m_TextReward.SafeSetText(reward);
    }
}
