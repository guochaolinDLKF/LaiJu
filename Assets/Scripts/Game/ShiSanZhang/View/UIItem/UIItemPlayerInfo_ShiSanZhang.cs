//===================================================
//Author      : DRB
//CreateTime  ：12/4/2017 4:27:38 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShiSanZhang
{
    public class UIItemPlayerInfo_ShiSanZhang : UIModuleBase
    {

        [SerializeField]
        private RawImage m_ImageHead;//头像
        [SerializeField]
        private Text m_TextGold;//分数
        [SerializeField]
        private Text m_TextNickname;//名字


        private int m_SeatPos;



        private void Awake()
        {
            Button btn = m_ImageHead.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(OnHeadGuPaiJiuClick);
            }          
        }

        public void SetUI(SeatEntity seat)
        {
            m_SeatPos = seat.Pos;
            SetPlayerInfo(seat);
        }

        private void SetPlayerInfo(SeatEntity seat)
        {
            m_TextGold.SafeSetText(seat.Gold.ToString());          
            m_TextNickname.SafeSetText(seat.Nickname);
            TextureManager.Instance.LoadHead(seat.Avatar, (Texture2D tex) =>
            {
                m_ImageHead.texture = tex;
            });
        }

        public void SetGold(int changeGold, int gold)
        {
            m_TextGold.SafeSetText(gold.ToString());
        }

        /// <summary>
        /// 头像点击
        /// </summary>
        private void OnHeadGuPaiJiuClick()
        {
            SendNotification("OnHeadGuPaiJiuClick", m_SeatPos);
        }
    }
}
