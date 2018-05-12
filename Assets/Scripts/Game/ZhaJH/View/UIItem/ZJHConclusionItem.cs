//===================================================
//Author      : DRB
//CreateTime  ：7/20/2017 1:39:37 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZhaJh;

public class ZJHConclusionItem : MonoBehaviour {

    [SerializeField]
    private Image m_ranking;                      //排名    
    [SerializeField]
    private RawImage m_avatarTexture;            //头像Terture
    [SerializeField]
    private Text m_nickname;                     //昵称
    [SerializeField]
    private Text playerID;                       //玩家ID

    [SerializeField]
    private Image[] m_goldImages;               //图片形式总积分

    private string pathK = "";

  
    /// <summary>
    ///  设置该总结Item
    /// </summary>
    /// <param name="seat"></param>
    public void SetConclusionItem(SeatEntity seat, int ranking)
    {
        //SetRankingUI(ranking);
        SetAvatarUI(seat.Avatar);
        SetNickNameUI(seat.Nickname);      
        SetGoldImages((int)seat.gold);
        SetPlayerID(seat.PlayerId.ToString());
    }


    /// <summary>
    /// 设置排名图片 
    /// </summary>
    /// <param name="avatar"></param>
    private void SetRankingUI(int ranking)
    {
        
        if (m_ranking != null)
        {
            //m_ranking.sprite = ResourceReference.Instance.NumRankingSprites[ranking];
            string tensStrName = "zjh_ranking" + ranking;
            string pathK = string.Format("download/{0}/source/uisource/zhajhpicture.drb", "lualu");
            Sprite currSprite = null;
            currSprite = AssetBundleManager.Instance.LoadSprite(pathK, tensStrName);
            if (currSprite != null)
            {
                m_ranking.sprite = currSprite;
            }
        }
    }



    ///// <summary>
    ///// 设置是否是房主
    ///// </summary>
    ///// <param name="nickName"></param>
    //private void SetIsHomeownersUI(bool isHomeowners)
    //{
    //    if (m_IsHomeowners != null)
    //    {
    //        m_IsHomeowners.gameObject.SetActive(isHomeowners);
    //    }
    //    if (m_IsHomeownersSign != null) m_IsHomeownersSign.enabled = isHomeowners;
    //}





    /// <summary>
    /// 设置头像  ------------------通过网络下载头像-----------------------------------
    /// </summary>
    /// <param name="avatar"></param>
    private void SetAvatarUI(string avatar)
    {
        TextureManager.Instance.LoadHead(avatar, OnAvatarLoadCallBack);
    }
    private void OnAvatarLoadCallBack(Texture avater)
    {
        if (m_avatarTexture != null && avater != null)
        {
            m_avatarTexture.texture = avater;

        }

    }

    /// <summary>
    /// 设置昵称
    /// </summary>
    /// <param name="nickName"></param>
    private void SetNickNameUI(string nickName)
    {
        if (m_nickname != null)
        {

            m_nickname.text = nickName;
        }
    }

    private void SetPlayerID(string playerID)
    {
        if (m_nickname != null)
        {            
            this.playerID.SafeSetText(string.Format("(ID:{0})", playerID));
        }
    }





    /// <summary>
    /// 设置该家已有积分 (图片形式)
    /// </summary>
    /// <param name="gold"></param>
    private void SetGoldImages(int gold)
    {
        if (m_goldImages == null || m_goldImages.Length < 1)
        {
            return;
        }

        pathK = string.Format("download/{0}/source/uisource/zhajhpicture.drb", ConstDefine.GAME_NAME);
        
        //符号
        if (m_goldImages.Length >= 1 && m_goldImages[0] != null)
        {           
            string symbolName = gold >= 0 ? "zjh_jia" : "zjh_jian";            
            Sprite currSprite = null;
            currSprite = AssetBundleManager.Instance.LoadSprite(pathK, symbolName);

            if (currSprite != null)
            {
                m_goldImages[0].sprite = currSprite;
            }
        }
       
        string earningsStr = string.Format("{0:D4}", gold);
        
        char thousandStr = earningsStr[earningsStr.Length - 4];
        char hundredStr = earningsStr[earningsStr.Length - 3];
        char tensStr = earningsStr[earningsStr.Length - 2];
        char onesStr = earningsStr[earningsStr.Length - 1];

        if (m_goldImages.Length >= 2) SetSprite(m_goldImages[1], gold, thousandStr);
        if (m_goldImages.Length >= 3) SetSprite(m_goldImages[2], gold, hundredStr);
        if (m_goldImages.Length >= 4) SetSprite(m_goldImages[3], gold, tensStr);
        if (m_goldImages.Length >= 5) SetSprite(m_goldImages[4], gold, onesStr);

        for (int i = m_goldImages.Length; i > 1; i--)
        {
            if (gold==0)
            {
                m_goldImages[4].gameObject.SetActive(true);
                m_goldImages[3].gameObject.SetActive(false);
                m_goldImages[2].gameObject.SetActive(false);
                m_goldImages[1].gameObject.SetActive(false);
            }
            else
            {
                m_goldImages[(m_goldImages.Length - i + 1)].gameObject.SetActive(Mathf.Abs(gold) >= Mathf.Pow(10, i - 2));
            }
           
        }

    }

    private void SetSprite(Image setSprite, int earningsValue, char strChar)
    {

        if (setSprite != null)
        {
            // string onesStrName = earningsValue >= 0 ? "img_fenshu" + strChar : "jian" + strChar;
            string onesStrName = "img_fenshu" + strChar;
            Sprite currSprite = null;
            currSprite = AssetBundleManager.Instance.LoadSprite(pathK, onesStrName);

            if (currSprite != null)
            {
                setSprite.sprite = currSprite;
            }

        }
    }

    ///// <summary>
    ///// 设置牌型  文字格式
    ///// </summary>
    ///// <param name="gold"></param>
    //private void SetPokerType(int pokerType)
    //{
    //    if (m_PokerType != null)
    //    {
    //        //pokerType = Mathf.Clamp(pokerType, -1, 13);
    //        if (pokerType == -1)
    //        {
    //            m_PokerType.text = NiuNiu.String_NiuNiu.Instance.PokerTypeChinese[0];

    //        }
    //        else if (pokerType > 0 && pokerType < 13)
    //        {
    //            m_PokerType.text = NiuNiu.String_NiuNiu.Instance.PokerTypeChinese[pokerType];


    //        }
    //    }
    //}

    ///// <summary>
    ///// 设置下注
    ///// </summary>
    ///// <param name="gold"></param>
    //private void SetPour(int pour)
    //{
    //    if (m_pour != null && pour != 0)
    //    {
    //        m_pour.text = pour.ToString();

    //    }

    //}




    ///// <summary>
    ///// 设置该小局收益
    ///// </summary>
    ///// <param name="gold"></param>
    //private void SetEarnings(int earnings, int index)
    //{

    //    string richText = earnings >= 0 ? "<color=#FFAD0AFF>" : "<color=#FF0E0AFF>";
    //    string earningsText = "";
    //    if (m_earnings != null)
    //    {

    //        if (earnings > 0)
    //        {
    //            earningsText = "+";
    //        }

    //        earningsText += earnings.ToString();
    //        //更改颜色
    //        m_earnings.text = string.Format("{0}{1}{2}", richText, earningsText, "</color>");

    //    }

    //}


}

