//===================================================
//Author      : WZQ
//CreateTime  ：5/23/2017 7:19:29 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NiuNiu
{
    public class NiuNiuConclusionItem : MonoBehaviour
    {

        [SerializeField]
        private Image m_ranking;                      //排名

        [SerializeField]
        private Text m_rankingText;                 //排名文字

        [SerializeField]
        private Image m_avatar;                       //头像
        [SerializeField]
        private RawImage m_avatarTexture;            //头像Terture
        [SerializeField]
        private Image m_IsHomeowners;            //是否是房主
        [SerializeField]
        private Image m_IsHomeownersSign;         //凸显房主
        [SerializeField]
        public Image m_banker;                      //是否是庄
        [SerializeField]
        public Image m_bankerSign;                 //凸显庄           
        [SerializeField]
        private Text m_nickname;                     //昵称
        [SerializeField]
        private Text m_ID;                           //玩家ID
        [SerializeField]
        private Text m_PokerType;                     //牌型



        [SerializeField]
        private Text m_pour;                        //下注

        [SerializeField]
        private Text m_earnings;                    //收益

        [SerializeField]
        private Text m_gold;                        //总积分

        [SerializeField]
        private Image[] m_goldImages;               //图片形式总积分

        private string pathK = "";
       



        /// <summary>
        ///  设置该总结Item
        /// </summary>
        /// <param name="seat"></param>
        public void SetConclusionItem(NiuNiu.Seat seat, int ranking)
        {
            SetRankingUI(ranking);
            SetAvatarUI(seat.Avatar);
            SetNickNameUI(seat.Nickname);
            SetPlayerID(seat.PlayerId);
            SetIsHomeownersUI(seat.IsHomeowners);
            SetBankerUI(seat.IsBanker);

            SetGold(seat.Gold);
            SetGoldImages(seat.Gold);

            SetPour(seat.Pour);
            SetPokerType(seat.PockeType);
            SetEarnings(seat.Earnings, (ranking - 1));
        }


        /// <summary>
        /// 设置排名图片 
        /// </summary>
        /// <param name="avatar"></param>
        private void SetRankingUI(int ranking)
        {
            if (m_rankingText != null)
            {

                m_rankingText.text = ranking.ToString();
            }
            if (m_ranking != null)
            {

                //m_ranking.sprite = ResourceReference.Instance.NumRankingSprites[ranking];

                string tensStrName = "niuniu_ranking" + ranking;
                string pathK = string.Format("download/{0}/source/uisource/niuniu.drb", ConstDefine.GAME_NAME);
                Sprite currSprite = null;
                currSprite = AssetBundleManager.Instance.LoadSprite(pathK, tensStrName);
                if (currSprite != null)
                {
                    m_ranking.sprite = currSprite;
                }



            }



        }



        /// <summary>
        /// 设置是否是房主
        /// </summary>
        /// <param name="nickName"></param>
        private void SetIsHomeownersUI(bool isHomeowners)
        {
            if (m_IsHomeowners != null)
            {
                m_IsHomeowners.gameObject.SetActive(isHomeowners);
            }
            if (m_IsHomeownersSign != null) m_IsHomeownersSign.enabled = isHomeowners;
        }





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

        /// <summary>
        /// 设置ID
        /// </summary>
        /// <param name="ID"></param>
        private void SetPlayerID(int ID)
        {
          
          if(ID > 0)  m_ID.SafeSetText(string.Format("ID:{0}", ID));
        }




        /// <summary>
        /// 设置是否是庄 标识
        /// </summary>
        /// <param name="isBanker"></param>
        private void SetBankerUI(bool isBanker)
        {
            if (m_banker != null)
            {
                m_banker.gameObject.SetActive(isBanker);
            }
            if (m_bankerSign != null) m_bankerSign.enabled = isBanker;

        }


        /// <summary>
        /// 设置该家已有积分
        /// </summary>
        /// <param name="gold"></param>
        private void SetGold(int gold)
        {
            if (m_gold != null)
            {
                m_gold.text = gold.ToString();

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

              pathK = string.Format("download/{0}/source/uisource/niuniu.drb", ConstDefine.GAME_NAME);

            //符号
            if (m_goldImages.Length >= 1 && m_goldImages[0] != null)
            {
                string symbolName = gold >= 0 ? "img_jia" : "img_jian";

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
            if (m_goldImages.Length >= 3)  SetSprite(m_goldImages[2], gold, hundredStr);
            if (m_goldImages.Length >= 4) SetSprite(m_goldImages[3], gold, tensStr);
            if (m_goldImages.Length >= 5) SetSprite(m_goldImages[4], gold, onesStr);

            for (int i = 1 ; i < m_goldImages.Length - 1; i++)
            {

             m_goldImages[i].gameObject.SetActive(Mathf.Abs(gold) >= Mathf.Pow(10, m_goldImages.Length - i - 1));

              
            }

        }

        private void SetSprite(Image setSprite, int earningsValue, char strChar)
        {

            if (setSprite != null)
            {

                string onesStrName = earningsValue >= 0 ? "img_fenshu" + strChar : "jian" + strChar;
                Sprite currSprite = null;
                currSprite = AssetBundleManager.Instance.LoadSprite(pathK, onesStrName);

                if (currSprite != null)
                {
                    setSprite.sprite = currSprite;
                }

            }
        }




        /// <summary>
        /// 设置牌型  文字格式
        /// </summary>
        /// <param name="gold"></param>
        private void SetPokerType(int pokerType)
        {

            if (m_PokerType != null)
            {
                m_PokerType.text = ((ConstDefine_NiuNiu.PokerTypeChinese)pokerType).ToString();
                ////pokerType = Mathf.Clamp(pokerType, -1, 13);
                //if (pokerType == -1)
                //{
                //    m_PokerType.text = ((ConstDefine_NiuNiu.PokerTypeChinese)pokerType).ToString();
                //    //m_PokerType.text =    ConstDefine_NiuNiu.Instance.PokerTypeChinese[0];

                //}
                //else if (pokerType > 0 && pokerType < 13)
                //{

                //    m_PokerType.text = NiuNiu.ConstDefine_NiuNiu.Instance.PokerTypeChinese[pokerType];


                //}



            }















        }




        /// <summary>
        /// 设置下注
        /// </summary>
        /// <param name="gold"></param>
        private void SetPour(int pour)
        {
            if (m_pour != null && pour != 0)
            {
                m_pour.text = pour.ToString();

            }

        }




        /// <summary>
        /// 设置该小局收益
        /// </summary>
        /// <param name="gold"></param>
        private void SetEarnings(int earnings, int index)
        {

            string richText = earnings >= 0  ? "<color=#FFAD0AFF>" : "<color=#FF0E0AFF>"; 
            string earningsText = "";
            if (m_earnings != null)
            {

                if (earnings > 0)
                {
                    earningsText = "+";
                }

                earningsText += earnings.ToString();
                //更改颜色
                m_earnings.text = string.Format("{0}{1}{2}", richText, earningsText, "</color>");

            }

        }


    }
}