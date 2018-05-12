//===================================================
//Author      : DRB
//CreateTime  ：9/5/2017 10:33:59 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GuPaiJiu;

public class UIGuPaiJiuItemPlayerInfo : UIModuleBase
{
    [SerializeField]
    private RawImage m_ImageHead;//头像
    [SerializeField]
    private Text m_TextGold;//分数
    [SerializeField]
    private Text m_TextNickname;//名字
    [SerializeField]
    private Image m_Banker;//是否是庄
  

    private int m_SeatPos;



    private void Awake()
    {
        Button btn = m_ImageHead.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnHeadGuPaiJiuClick);
        }
        //if (m_TxtChangeGold != null)
        //{
        //    m_ChangeGoldTweener = m_TxtChangeGold.transform.DOLocalMoveY(m_TxtChangeGold.transform.localPosition.y + 100f, 1.5f).OnComplete(() =>
        //    {
        //        m_TxtChangeGold.gameObject.SetActive(false);
        //    }).SetAutoKill(false).Pause();
        //}
    }

    public void SetUI(SeatEntity seat)
    {
        m_SeatPos = seat.Pos;
        SetPlayerInfo(seat);
    }

    private void SetPlayerInfo(SeatEntity seat)
    {
        m_TextGold.SafeSetText(seat.Gold.ToString());
        m_Banker.gameObject.SetActive(seat.IsBanker);
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
