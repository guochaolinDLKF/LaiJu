//===================================================
//Author      : DRB
//CreateTime  ：4/26/2017 2:58:33 PM
//Description ：结束界面UI项
//===================================================
using proto.mahjong;
using UnityEngine;
using UnityEngine.UI;

public class UIItemResult : MonoBehaviour 
{
    [SerializeField]
    private Image m_ImageLandlord;//房主图标

    [SerializeField]
    private RawImage m_HeadImage;//头像

    [SerializeField]
    private Text m_TextNickname;//昵称

    [SerializeField]
    private Text m_TextId;//Id

    [SerializeField]
    private Text m_TextZiMoTimes;//自摸次数

    [SerializeField]
    private Text m_TextHuTimes;//胡牌次数

    [SerializeField]
    private Text m_TextZhuaMaTimes;//抓马次数

    [SerializeField]
    private Text m_TextAnGangTimes;//暗杠次数

    [SerializeField]
    private Text m_TextBuGangTimes;//补杠次数

    [SerializeField]
    private Text m_TextMingGangTimes;//明杠次数

    [SerializeField]
    private Text m_TextHuDianPaoTimes;//胡点炮次数

    [SerializeField]
    private Text m_DianPaoTimes;//点炮次数

    [SerializeField]
    private Text m_BaoTimes;//摸宝次数

    [SerializeField]
    private Text m_InBaoTimes;//宝中宝次数

    [SerializeField]
    private Text m_BankerTimes;//坐庄次数

    [SerializeField]
    private Text m_Score;//总得分

    [SerializeField]
    private Image m_ImageWiner;//大赢家图标

    [SerializeField]
    private Image m_ProbCount;






    public void SetUI(OP_SEAT_RESULT result,bool isWiner)
    {
        m_ImageLandlord.SafeSetActive(result.isOwner);
        m_TextNickname.SafeSetText(result.nickname);
        m_TextId.SafeSetText(result.playerId.ToString());
        m_ImageWiner.SafeSetActive(isWiner);

        m_TextZiMoTimes.SafeSetText(result.zimoCount.ToString());//自摸次数
        m_TextZhuaMaTimes.SafeSetText(result.probCount.ToString());//抓马次数
        m_TextAnGangTimes.SafeSetText(result.agangCount.ToString());//暗杠次数
        m_TextMingGangTimes.SafeSetText(result.mgangCount.ToString());//明杠次数
        m_TextBuGangTimes.SafeSetText(result.bgangCount.ToString());//补杠次数
        m_TextHuTimes.SafeSetText((result.paoCount + result.zimoCount).ToString());//胡牌次数
        m_TextHuDianPaoTimes.SafeSetText(result.paoCount.ToString());//胡点炮次数
        m_DianPaoTimes.SafeSetText(result.dianpaoCount.ToString());//点炮次数
        m_BaoTimes.SafeSetText(result.baoCount.ToString());//摸宝次数
        m_InBaoTimes.SafeSetText(result.inBaoCount.ToString());//宝中宝次数
        m_BankerTimes.SafeSetText(result.bankerCount.ToString());//坐庄次数


        m_Score.SafeSetText(result.gold.ToString());
        TextureManager.Instance.LoadHead(result.avatar,(Texture2D tex)=> 
        {
            m_HeadImage.texture = tex;
        });
    }
}
