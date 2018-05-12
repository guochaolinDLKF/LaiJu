//===================================================
//Author      : DRB
//CreateTime  ：7/10/2017 2:53:36 PM
//Description ：
//===================================================
using UnityEngine;
using UnityEngine.UI;

public class UIItemRanking : UIItemBase 
{
    [SerializeField]
    private RawImage m_ImgHead;
    [SerializeField]
    private Text m_TxtNickName;
    [SerializeField]
    private Text m_TxtRanking;
    [SerializeField]
    private Text m_TxtValue;
    [SerializeField]
    private Image m_ImageScore;
    [SerializeField]
    private Image m_ImageCount;

    private Texture m_DefaultTexture;

    protected override void OnAwake()
    {
        base.OnAwake();
        if (m_ImgHead != null)
        {
            m_DefaultTexture = m_ImgHead.texture;
        }
    }


    public void SetUI(int ranking,string nickname,string headUrl,int value,RankingListType rankingType)
    {
        if (m_ImgHead != null)
        {
            m_ImgHead.texture = m_DefaultTexture;
        }
        TextureManager.Instance.LoadHead(headUrl,(Texture2D tex)=> 
        {
            if (m_ImgHead != null)
            {
                m_ImgHead.texture = tex;
            }
        });
        m_TxtNickName.SafeSetText(nickname);
        m_TxtRanking.SafeSetText(ranking.ToString());
        m_TxtValue.SafeSetText(value.ToString());

        bool isScore = rankingType == RankingListType.Score;
        if (m_ImageScore != null)
        {
            m_ImageScore.gameObject.SetActive(isScore);
        }
        if (m_ImageCount != null)
        {
            m_ImageCount.gameObject.SetActive(!isScore);
        }
    }

}
