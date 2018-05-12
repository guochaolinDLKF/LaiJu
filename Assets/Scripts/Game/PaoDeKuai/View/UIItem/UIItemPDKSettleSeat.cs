//===================================================
//Author      : WZQ
//CreateTime  ：12/1/2017 5:18:42 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace PaoDeKuai
{
    public class UIItemPDKSettleSeat : MonoBehaviour
    {
        [SerializeField]
        private RawImage m_Avatar;//头像

        [SerializeField]
        private Text m_NiceName;//名字

        [SerializeField]
        private Text m_Earnings;//本局收益


        public void SetUI(SeatEntity seat)
        {
            m_NiceName.SafeSetText(seat.Nickname);
            m_Earnings.SafeSetText(string.Format("{0}{1}", seat.Earnings>0?"+":"", seat.Earnings.ToString()));

            TextureManager.Instance.LoadHead(seat.Avatar, (Texture2D tex) =>
            {
                if (m_Avatar != null)
                {
                    m_Avatar.texture = tex;
                }
            });

        }
      




    }
}