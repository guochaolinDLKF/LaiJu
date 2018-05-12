//===================================================
//Author      : DRB
//CreateTime  ：8/30/2017 12:07:02 PM
//Description ：解散房间界面座位UI项
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemDisbandSeat : UIItemBase 
{
    [SerializeField]
    private RawImage m_ImgHead;
    [SerializeField]
    private Text m_TxtNickName;
    [SerializeField]
    private Image m_ImgAgree;
    [SerializeField]
    private Image m_ImgRefuse;
    [SerializeField]
    private Image m_ImgWaiting;

    private int m_playerId;


    public void SetUI(SeatEntityBase seat)
    {
        m_playerId = seat.PlayerId;
        TextureManager.Instance.LoadHead(seat.Avatar,(Texture2D tex)=> 
        {
            if (m_ImgHead != null)
            {
                m_ImgHead.texture = tex;
            }
        });

        m_TxtNickName.SafeSetText(seat.Nickname);

        m_ImgAgree.gameObject.SetActive(seat.DisbandState == DisbandState.Agree || seat.DisbandState == DisbandState.Apply);
        m_ImgRefuse.gameObject.SetActive(seat.DisbandState == DisbandState.Refuse);
        m_ImgWaiting.gameObject.SetActive(seat.DisbandState == DisbandState.Wait);
    }



}
