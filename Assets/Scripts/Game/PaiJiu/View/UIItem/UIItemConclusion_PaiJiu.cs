//===================================================
//Author      : WZQ
//CreateTime  ：7/24/2017 4:26:54 PM
//Description ：牌九 结算的单条项目  
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace PaiJiu
{
    public class UIItemConclusion_PaiJiu : MonoBehaviour
    {
          
        [SerializeField]
        private RawImage m_avatarTexture;            //头像Terture
        //[SerializeField]
        //private Image m_IsHomeowners;              //是否是房主
        //[SerializeField]
        //private Image m_IsHomeownersSign;          //凸显房主
        [SerializeField]
        public Image m_banker;                       //是否是庄
        //[SerializeField]
        //public Image m_bankerSign;                 //凸显庄           
        [SerializeField]
        private Text m_nickname;                     //昵称
        [SerializeField]
        private Text m_ID;                           //玩家ID
   


        [SerializeField]
        private Text m_bankerPourText;              //一局庄家下注

        [SerializeField]
        private Text m_earnings;                    //一局收益

        [SerializeField]
        private Text m_gold;                        //总积分

        [SerializeField]
        private Image[] _symbol;
        [SerializeField]
        private Image place;                       // 个位图片  图片形式总积分

        [SerializeField]
        private Image m_isBigWinner;               // 大赢家

        private string pathK = "";


        public void SetUI(Seat seat)
        {
           
           if(m_avatarTexture !=null) SetAvatarUI(seat.Avatar);
            if (m_banker != null)  m_banker.gameObject.SetActive(seat.IsBanker);//是否是庄
            if (m_nickname != null) m_nickname.text=seat.Nickname;//昵称
            if (m_ID != null) m_ID.text = string.Format("ID:{0}", seat.PlayerId.ToString()) ;//ID
           if(m_earnings != null) SetEarningsText(seat.LoopEarnings);//本局收益
            m_gold.SafeSetText(seat.TotalEarnings.ToString());//文字总收益
            if (place != null && _symbol != null) SetSetEarningsImages(seat.TotalEarnings);//图片总收益

           if(m_bankerPourText != null) SetBankerPour(seat.IsBanker,(seat.Pour - seat.LoopEarnings));//设置庄下注
#if IS_ZHANGJIAKOU
            if(m_isBigWinner != null) m_isBigWinner.gameObject.SetActive(seat.isBigWinner);//是大赢家
#endif

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
        /// 设置庄下注
        /// </summary>
        /// <param name="isBanker"></param>
        /// <param name="bankerPour"></param>
        private void SetBankerPour(bool isBanker,int bankerPour)
        {
            m_bankerPourText.transform.parent.gameObject.SetActive(isBanker);
            if (isBanker)  m_bankerPourText.SafeSetText(bankerPour.ToString());

        }





        /// <summary>
        /// 设置该小局收益 (文字) 
        /// </summary>
        /// <param name="gold"></param>
        private void SetEarningsText(int earnings)
        {

            string richText = earnings >= 0 ? "<color=#FFAD0AFF>" : "<color=#FF0E0AFF>";
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

#region 设置数字图片

        public void SetSetEarningsImages(int earningsValue)
        {
            //设置符号

            _symbol[0].gameObject.SetActive(earningsValue >= 0);
            _symbol[1].gameObject.SetActive(earningsValue < 0);


            List<int> intList = new List<int>();
            List<Image> imageList = new List<Image>();
            imageList.Add(place); 

            for (int i = 0; i < imageList.Count; i++)
            {
                imageList[i].gameObject.SetActive(false);
            }

            int earningsABSValue = Mathf.Abs(earningsValue);

            SetFindString(earningsABSValue, intList);

            //根据intlist值设置ImageList

            for (int i = 0; i < intList.Count; i++)
            {
                if ((i + 1) > imageList.Count)
                {
                    GameObject imageClone = Instantiate(place.gameObject) as GameObject;
                    imageList.Add(imageClone.GetComponent<Image>());
                    imageClone.SetParent(place.transform.parent);
                    imageClone.transform.SetSiblingIndex(1);
                }

                SetSprite(imageList[i], earningsValue, intList[i].ToString());

                imageList[i].gameObject.SetActive(true);
            }
        
           




        }


        //设置

        //找到每一位数
        private void SetFindString(int sum,List<int> intList)
        {
            int x = sum % 10;

            intList.Add(x);
            int shang = sum / 10;//商
            if (sum >= 10)
            {
                SetFindString(shang,intList);
            }
            else
            {
                return;
            }

        }



        //设置图片
        private void SetSprite(Image setSprite, int earningsValue, string strChar)
        {

            if (setSprite != null)
            {

                string onesStrName = earningsValue >= 0 ? "img_fenshu" + strChar : "jian" + strChar;
                Sprite currSprite = null;
                //设置加载路径
                string prefabPath = string.Format(ConstDefine_PaiJiu.UISpritePath, ConstDefine.GAME_NAME);
                currSprite = AssetBundleManager.Instance.LoadSprite(prefabPath, onesStrName);

                if (currSprite != null)
                {
                    setSprite.sprite = currSprite;
                }

            }
        }
#endregion



    }
}