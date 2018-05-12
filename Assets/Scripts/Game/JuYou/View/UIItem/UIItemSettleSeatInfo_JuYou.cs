//===================================================
//Author      : WZQ
//CreateTime  ：8/21/2017 5:41:01 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace JuYou
{
    public class UIItemSettleSeatInfo_JuYou : MonoBehaviour
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
        private Text m_earnings;                    //一局收益

        [SerializeField]
        private Text m_gold;                        //总积分

        [SerializeField]
        private Image m_richMan;                //土豪

        [SerializeField]
        private Image m_daYingJia;              //大赢家
        
        public void SetUI(SeatEntity seat)
        {
           //UIWindowType.Settle
            if (m_avatarTexture != null) SetAvatarUI(seat.Avatar);
            if (m_banker != null) m_banker.gameObject.SetActive(seat.IsBanker);//是否是庄
            if (m_nickname != null) m_nickname.text = seat.Nickname;//昵称
            if (m_ID != null) m_ID.text = string.Format("ID:{0}", seat.PlayerId.ToString());//ID
            if (m_earnings != null) m_earnings.text =  seat.loopEarnings.ToString();//本局收益（自定义字体）
            if (m_gold != null) m_gold.text =  seat.Gold.ToString();//总金币（自定义字体）
            //if (m_earnings != null) SetEarningsText(seat.LoopEarnings);//本局收益
            //m_gold.SafeSetText(seat.TotalEarnings.ToString());//文字总收益

            //自定义字体 总收益

        }


        /// <summary>
        /// 设置头像  
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

        /// <summary>
        /// 设置土豪
        /// </summary>
        /// <param name="isShow"></param>
        public void SetRichMan(bool isShow)
        {
            if (m_richMan == null) return;
            m_richMan.gameObject.SetActive(isShow);
        }

        /// <summary>
        /// 设置大赢家
        /// </summary>
        /// <param name="isShow"></param>
        public void SetDaYingJia(bool isShow)
        {
            if (m_daYingJia == null) return;
            m_daYingJia.gameObject.SetActive(isShow);
            
        }

    }
}