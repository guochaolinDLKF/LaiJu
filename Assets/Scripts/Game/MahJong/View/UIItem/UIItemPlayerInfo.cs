//===================================================
//Author      : DRB
//CreateTime  ：3/7/2017 4:55:25 PM
//Description ：角色信息UI
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using DRB.MahJong;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIItemPlayerInfo : UIModuleBase
{
    [SerializeField]
    private RawImage m_ImageHead;
    [SerializeField]
    private Text m_TextGold;
    [SerializeField]
    private Text m_TextNickname;
    [SerializeField]
    private Image m_Banker;
    [SerializeField]
    private Image m_Trustee;
    [SerializeField]
    private Image m_Ting;
    [SerializeField]
    private Image m_isDouble;
    [SerializeField]
    private GameObject m_Leave;
    [SerializeField]
    private Image m_ImgOperating;
    [SerializeField]
    private UIMoveText m_MoveText;
    [SerializeField]
    private Image m_ImgHu;
    [SerializeField]
    private Image m_ImgWan;
    [SerializeField]
    private Image m_ImgTong;
    [SerializeField]
    private Image m_ImgTiao;

    private int m_SeatPos;

    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(RoomMaJiangProxy.ON_CURRENT_OPERATOR_CHANGED, OnOperatorChanged);
        return dic;
    }

    private void OnOperatorChanged(TransferData obj)
    {
        int seatPos = obj.GetValue<int>("SeatPos");
        SetOperating(seatPos == m_SeatPos);
    }

    protected override void OnAwake()
    {
        Button btn = m_ImageHead.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnHeadClick);
        }

        if (m_ImgOperating != null)
        {
            m_ImgOperating.gameObject.SetActive(false);
            m_ImgOperating.fillAmount = 0f;
            PlayForwardAnimation();
        }
    }

    private void PlayForwardAnimation()
    {
        m_ImgOperating.transform.localEulerAngles = Vector3.zero;
        m_ImgOperating.DOFillAmount(1f, 2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            PlayBackwardsAnimation();
        });
    }

    private void PlayBackwardsAnimation()
    {
        m_ImgOperating.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        m_ImgOperating.DOFillAmount(0f, 2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            PlayForwardAnimation();
        });
    }

    private void OnHeadClick()
    {
        AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);

        SendNotification("OnMahjongViewHeadClick", m_SeatPos);
    }

    public void SetUI(SeatEntity seat)
    {
        m_SeatPos = seat.Pos;
        SetPlayerInfo(seat);
    }

    private void SetPlayerInfo(SeatEntity seat)
    {
        m_Leave.SetActive(seat.IsWaiver || !seat.IsFocus);

        m_TextGold.SafeSetText(seat.Gold.ToString());
        m_Banker.gameObject.SetActive(seat.IsBanker);
        m_TextNickname.SafeSetText(seat.Nickname);
        m_Trustee.gameObject.SetActive(seat.IsTrustee);

        if (m_Ting != null)
        {
            m_Ting.gameObject.SetActive(seat.IsTing);
        }
        if (m_isDouble != null)
        {
            m_isDouble.gameObject.SetActive(seat.isDouble);
        }
        TextureManager.Instance.LoadHead(seat.Avatar, (Texture2D tex) =>
        {
            if (m_ImageHead != null)
            {
                m_ImageHead.texture = tex;
            }
        });

        if (m_ImgHu != null)
        {
            m_ImgHu.gameObject.SetActive(seat.isHu);
        }
        m_ImgWan.SafeSetActive(seat.LackColor == 1);
        m_ImgTong.SafeSetActive(seat.LackColor == 2);
        m_ImgTiao.SafeSetActive(seat.LackColor == 3);
    }

    public void SetGold(int changeGold, int gold)
    {
        m_TextGold.SafeSetText(gold.ToString());

        if (m_MoveText != null)
        {
            m_MoveText.AddText(changeGold.ToString(ConstDefine.STRING_FORMAT_SIGN), changeGold >= 0 ? Color.red : Color.green);
        }
    }

    public void SetOperating(bool isOperating)
    {
        if (m_ImgOperating != null)
        {
            m_ImgOperating.gameObject.SetActive(isOperating);
        }
    }
}
