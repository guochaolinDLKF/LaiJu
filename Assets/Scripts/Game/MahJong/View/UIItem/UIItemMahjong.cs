//===================================================
//Author      : DRB
//CreateTime  ：9/22/2017 5:56:42 PM
//Description ：麻将UI项
//===================================================
using DRB.MahJong;
using UnityEngine;
using UnityEngine.UI;

public class UIItemMahjong : UIItemBase 
{
    [SerializeField]
    private Image m_ImgPoker;
    [SerializeField]
    private Image m_ImgHu;
    [SerializeField]
    private Image m_ImgBao;
    [SerializeField]
    private Image m_ImgMa;
    [SerializeField]
    private Image m_ImgUniversal;
    [SerializeField]
    private Image m_ImgUniversal_Peng;
    [SerializeField]
    private Image m_ImgUniversal_BottomLeft;



    protected override void OnAwake()
    {
        base.OnAwake();
    }

    /// <summary>
    /// 设置UI
    /// </summary>
    /// <param name="isHu">是否是胡的牌</param>
    /// <param name="isBao">是否是宝</param>
    /// <param name="isMa">是否是抓马</param>
    /// <param name="isUniversal">是否是万能牌</param>
    /// <param name="isDim">是否置灰</param>
    public void SetUI(Poker poker,bool isHu,bool isBao,bool isMa,bool isUniversal,bool isPeng,bool isDim)
    {
        m_ImgPoker.overrideSprite = MahJongManager.Instance.LoadPokerSprite(poker,isPeng);
        m_ImgHu.gameObject.SetActive(isHu);
        m_ImgBao.gameObject.SetActive(isBao);
        m_ImgMa.gameObject.SetActive(isMa);
        if (m_ImgUniversal != null)
        {
            m_ImgUniversal.gameObject.SetActive(isUniversal && !isPeng);
        }
        if (m_ImgUniversal_Peng != null)
        {
            m_ImgUniversal_Peng.gameObject.SetActive(isUniversal && isPeng);
        }
        if (m_ImgUniversal_BottomLeft != null)
        {
            m_ImgUniversal_BottomLeft.gameObject.SetActive(isUniversal && !isPeng);
        }
    }

    public void SetSize(Vector2 size)
    {
        m_ImgPoker.rectTransform.sizeDelta = size;
    }
}
