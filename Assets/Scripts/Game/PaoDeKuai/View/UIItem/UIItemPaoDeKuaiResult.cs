//===================================================
//Author      : WZQ
//CreateTime  ：11/21/2017 11:01:38 AM
//Description ：跑得快总结算Item
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace PaoDeKuai
{
    public class UIItemPaoDeKuaiResult : MonoBehaviour
    {
        [SerializeField]
        private Image m_ImageLandlord;//房主标识

        [SerializeField]
        private RawImage m_HeadImage;//头像

        [SerializeField]
        private Image m_ImageBieWinner;//大赢家标识

        [SerializeField]
        private Text m_NickName;//昵称

        [SerializeField]
        private Text m_PlayerID;//ID

        [SerializeField]
        private Text m_BaseScore;//底分

        [SerializeField]
        private Text m_Gold;//总分


        public void SetUI(SeatEntity seat, bool isWiner,int baseScore)
        {
            m_ImageLandlord.gameObject.SetActive(seat.isLandlord);
            m_ImageBieWinner.gameObject.SetActive(isWiner);
            m_NickName.SafeSetText(seat.Nickname);
            m_PlayerID.SafeSetText( "ID:"+seat.PlayerId);
            m_BaseScore.SafeSetText(baseScore.ToString());
            m_Gold.SafeSetText(seat.Gold.ToString());
            TextureManager.Instance.LoadHead(seat.Avatar, (Texture2D tex) =>
            {
                m_HeadImage.texture = tex;
            });

        }





    }
}