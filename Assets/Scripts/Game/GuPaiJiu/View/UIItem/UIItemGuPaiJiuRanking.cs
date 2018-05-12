//===================================================
//Author      : CZH
//CreateTime  ：9/14/2017 3:49:36 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuPaiJiu;
using UnityEngine.UI;

public class UIItemGuPaiJiuRanking : MonoBehaviour {

    [SerializeField]
    private RawImage m_ImageHead;//头像
    [SerializeField]
    private Text m_TextNickname;//名字
    [SerializeField]
    private Text m_TextGold;//分数
    [SerializeField]
    private Image rangKing;//名次

    public void SetUI(SeatEntity seat,Sprite sprite)
    {
        if (seat == null)
        {
            m_ImageHead.gameObject.SetActive(false);
            m_TextNickname.gameObject.SetActive(false);
            m_TextGold.gameObject.SetActive(false);
            rangKing.gameObject.SetActive(false);
        }
        else
        {
            m_TextGold.SafeSetText(seat.Gold.ToString());
            rangKing.sprite = sprite;
            m_TextNickname.SafeSetText(seat.Nickname);
            TextureManager.Instance.LoadHead(seat.Avatar, (Texture2D tex) =>
            {
                m_ImageHead.texture = tex;
            });
        }       
    }


}
