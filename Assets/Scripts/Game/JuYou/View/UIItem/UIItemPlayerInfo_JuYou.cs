//===================================================
//Author      : WZQ
//CreateTime  ：8/14/2017 10:36:45 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace JuYou
{
    public class UIItemPlayerInfo_JuYou : MonoBehaviour
    {
       
        [SerializeField]
        private RawImage m_ImageHead;//头像
        [SerializeField]
        private Text m_TextGold;//金币
        [SerializeField]
        private Text m_TextNickname;//昵称
        [SerializeField]
        private Transform m_Banker;//是否是庄

        [SerializeField]
        private Text m_ID;//ID
        [SerializeField]
        private Image m_isOnLine;//是否在线
        //[SerializeField]
        //private GameObject m_Leave;//是否弃牌（暂无此项）

        private int m_SeatIndex;//座位位置Index



        private void Awake()
        {
            m_isOnLine.gameObject.SetActive(false);
            //注册点击头像事件（暂无此项）
            //Button btn = m_ImageHead.GetComponent<Button>();
            //if (btn != null)
            //{
            //    btn.onClick.AddListener(OnHeadClick);
            //}
        }

        ////头像点击
        //private void OnHeadClick()
        //{
        //    AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);

        //    if (DelegateDefine.Instance.OnHeadClick != null)
        //    {
        //        DelegateDefine.Instance.OnHeadClick(m_SeatPos);
        //    }
        //}



        public void SetUI(SeatEntity seat)
        {
            m_SeatIndex = seat.Index;
            SetPlayerInfo(seat);
        }

        private void SetPlayerInfo(SeatEntity seat)
        {
            //m_Leave.SetActive(seat.IsWaiver);//是否弃牌（无此项）


            ////个人积分刷新依靠动画事件 及初始刷新
            //m_TextGold.SafeSetText(seat.Gold.ToString());


            m_Banker.gameObject.SetActive(seat.IsBanker);
            m_TextNickname.SafeSetText(seat.Nickname);
            m_ID.SafeSetText(string.Format("ID:{0}", seat.PlayerId.ToString()));
            m_isOnLine.gameObject.SetActive(!seat.isOnLine); //是否在线
            //if (m_Pour != null) m_Pour.transform.parent.gameObject.SetActive(seat.Pour > 0);
            //m_Pour.SafeSetText(seat.Pour.ToString());
            TextureManager.Instance.LoadHead(seat.Avatar, OnAvatarLoadCallBack);
        }

        private void OnAvatarLoadCallBack(Texture2D tex)
        {
            m_ImageHead.texture = tex;
        }


        /// <summary>
        /// 刷新金币   由动画事件或初始刷新 
        /// </summary>
        public void SetGold(SeatEntity seat)
        {
            m_TextGold.SafeSetText(seat.Gold.ToString());
        }



    }
}