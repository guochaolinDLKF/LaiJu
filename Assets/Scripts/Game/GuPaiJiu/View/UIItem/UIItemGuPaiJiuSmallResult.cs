//===================================================
//Author      : DRB
//CreateTime  ：10/10/2017 5:30:26 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GuPaiJiu;

public class UIItemGuPaiJiuSmallResult : MonoBehaviour {


    [SerializeField]
    private RawImage m_ImageHead;//头像
    [SerializeField]
    private Text PlayerScore;


    public void SetUI(SeatEntity seat)
    {
        string scoreText = string.Format(seat.eamings > 0 ? "+{0}" : "{1}", seat.eamings, seat.eamings);
        PlayerScore.color = seat.eamings > 0 ? Color.white : Color.red;
        PlayerScore.text = scoreText;
        TextureManager.Instance.LoadHead(seat.Avatar, (Texture2D tex) =>
        {
            m_ImageHead.texture = tex;
        });
    }
}
