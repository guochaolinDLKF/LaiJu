//===================================================
//Author      : DRB
//CreateTime  ：4/25/2017 8:11:10 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using DRB.MahJong;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSeat : UIItemBase 
{
    [SerializeField]
    protected UIItemPlayerInfo m_PlayerInfo;

    [SerializeField]
    protected GameObject m_Ready;

    [SerializeField]
    protected Transform m_UIAnimationContainer;

    [SerializeField]
    protected int m_nSeatIndex = -1;
    [SerializeField]
    protected Button m_BtnChangeSeat;

    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(RoomMaJiangProxy.ON_SEAT_INFO_CHANGED, OnSeatInfoChanged);
        dic.Add(RoomMaJiangProxy.ON_SEAT_GOLD_CHANGED, OnSeatGoldChanged);
        dic.Add(RoomMaJiangProxy.ON_SEAT_INFO_CLEAR, OnSeatInfoClear);
        return dic;
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        m_PlayerInfo.gameObject.SetActive(false);
        m_Ready.gameObject.SetActive(false);
        if (m_BtnChangeSeat != null)
        {
            m_BtnChangeSeat.gameObject.SetActive(false);
            m_BtnChangeSeat.onClick.AddListener(OnChangeSeatClick);
        }
    }

    private void OnChangeSeatClick()
    {
        SendNotification("OnBtnChangeSeatClick", m_nSeatIndex);
    }

    private void OnSeatGoldChanged(TransferData data)
    {
        int seatIndex = data.GetValue<int>("SeatIndex");
        int changeGold = data.GetValue<int>("ChangeGold");
        int gold = data.GetValue<int>("Gold");
        if (m_nSeatIndex == seatIndex)
        {
            m_PlayerInfo.SetGold(changeGold, gold);
        }
    }

    private void OnSeatInfoChanged(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        RoomEntity.RoomStatus roomStatus = data.GetValue<RoomEntity.RoomStatus>("RoomStatus");
        SeatEntity.SeatStatus playerStatus = data.GetValue<SeatEntity.SeatStatus>("PlayerStatus");
        SetSeatInfo(seat, roomStatus, playerStatus);
    }

    private void SetSeatInfo(SeatEntity seat,RoomEntity.RoomStatus roomStatus, SeatEntity.SeatStatus playerStatus)
    {
        if (m_nSeatIndex == seat.Index)
        {
            if (seat.PlayerId == 0)
            {
                m_PlayerInfo.gameObject.SetActive(false);
                m_Ready.gameObject.SetActive(false);
                if (roomStatus == RoomEntity.RoomStatus.Ready && playerStatus == SeatEntity.SeatStatus.Idle)
                {
                    if (m_BtnChangeSeat != null)
                    {
                        m_BtnChangeSeat.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (m_BtnChangeSeat != null)
                    {
                        m_BtnChangeSeat.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                m_PlayerInfo.gameObject.SetActive(true);
                m_Ready.gameObject.SetActive(seat.Status == SeatEntity.SeatStatus.Ready);
                if (m_BtnChangeSeat != null)
                {
                    m_BtnChangeSeat.gameObject.SetActive(false);
                }
            }
            m_PlayerInfo.SetUI(seat);

            if (seat.isDouble && roomStatus == RoomEntity.RoomStatus.Pao)
            {
                PlayUIAnimation(UIAnimationType.UIAnimation_Pao);
            }
        }
    }

    #region PlayUIAnimation 播放UI动画
    /// <summary>
    /// 播放UI动画
    /// </summary>
    /// <param name="type"></param>
    public void PlayUIAnimation(UIAnimationType type)
    {
        PlayUIAnimation(type.ToString());
    }
    #endregion

    #region PlayUIAnimation 播放UI动画
    /// <summary>
    /// 播放UI动画
    /// </summary>
    /// <param name="animationName"></param>
    public void PlayUIAnimation(string animationName)
    {
        animationName = animationName.ToLower();
        string path = string.Format("download/{0}/prefab/uiprefab/uianimations/{1}.drb", ConstDefine.GAME_NAME, animationName);
        GameObject go = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(path,animationName);
        if (go != null)
        {
            go = Instantiate(go);
            go.SetParent(m_UIAnimationContainer);
        }
        else
        {
            AppDebug.LogWarning("UI动画" + animationName + "不存在");
        }
    }
    #endregion

    public void SetOperating(bool isOperating)
    {
        m_PlayerInfo.SetOperating(isOperating);
    }

    /// <summary>
    /// 清空座位信息
    /// </summary>
    /// <param name="obj"></param>
    private void OnSeatInfoClear(TransferData obj)
    {
        m_PlayerInfo.gameObject.SetActive(false);
        m_Ready.gameObject.SetActive(false);
        if (m_BtnChangeSeat != null)
        {
            m_BtnChangeSeat.gameObject.SetActive(false);
        }
    }
}
