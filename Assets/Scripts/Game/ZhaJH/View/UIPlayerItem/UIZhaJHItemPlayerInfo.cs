//===================================================
//Author      : DRB
//CreateTime  ：6/14/2017 3:20:15 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZhaJh;

public class UIZhaJHItemPlayerInfo : UIModuleBase
{
    [SerializeField]
    private RawImage m_ImageHead;//头像
    [SerializeField]
    private Text m_TextGold;//分数
    [SerializeField]
    private Text m_TextNickname;  //昵称      
    [SerializeField]
    private GameObject m_Leave;//离开

    [SerializeField]
    private Text seatTotalPour;//玩家每局下的总分

    [HideInInspector]
    public int m_SeatPos;

    public Image m_Imagekuang; 
    [SerializeField]
    private Image btnMask;
    
    private void Awake()
    {        
        Button btn = m_Imagekuang.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnHeadZJHClick);
        }
        //Button btn = m_ImageHead.GetComponent<Button>();
        //if (btn != null)
        //{
        //    btn.onClick.AddListener(OnHeadClick);
        //}
    }

    private void OnHeadZJHClick()
    {
        // AudioEffectMnager.Instance.Play("btnclick", Vector3.zero, false);
        SendNotification("OnHeadZJHClick", m_SeatPos);       
        //if (DelegateDefine.Instance.OnHeadClick != null)
        //{
        //    DelegateDefine.Instance.OnHeadClick(m_SeatPos);
        //}
    }

    public void SetUI(SeatEntity seat)
    {
        m_SeatPos = seat.pos;
        SetPlayerInfo(seat);
    }

    private void SetPlayerInfo(SeatEntity seat)
    {        
        m_Leave.SetActive(false);  //离开状态 
        if (RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId!= RoomMode.Senior)
        {          
            seatTotalPour.SafeSetText(seat.totalPour.ToString()); ;//玩家每局下的总分
            m_TextGold.SafeSetText(seat.gold.ToString());
        }
        else if (RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId == RoomMode.Senior)
        {
            if (seat!= RoomZhaJHProxy.Instance.PlayerSeat&&seat.pos==7)
            {               
                seat.gold = 0;
            }
            seatTotalPour.SafeSetText(seat.totalPour.ToString()); ;//玩家每局下的总分
            m_TextGold.SafeSetText(seat.gold.ToString("0.00"));
        }       
       // m_Banker.gameObject.SetActive(seat.isBanker);
        m_TextNickname.SafeSetText(seat.Nickname);
        TextureManager.Instance.LoadHead(seat.Avatar, OnAvatarLoadCallBack);
    }

    private void OnAvatarLoadCallBack(Texture2D tex)
    {
        m_ImageHead.texture = tex;
    }

    /// <summary>
    /// 比牌按钮点击事件
    /// </summary>
    public void BtnBPOnClik()
    {
        RoomZhaJHProxy.Instance.theCardPos = m_SeatPos;        
        SendNotification(ZhaJHMethodname.OnZJHThanPoker);        
    }
    public void BtnMaskOnClik()
    {
        btnMask.gameObject.SetActive(false);
        //RoomZhaJHProxy.Instance.DicEvent("isbool", false, "NoMaskP");
    }

}
