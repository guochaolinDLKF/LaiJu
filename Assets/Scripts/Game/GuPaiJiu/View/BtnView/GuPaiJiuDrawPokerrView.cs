//===================================================
//Author      : DRB
//CreateTime  ：10/30/2017 8:10:41 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GuPaiJiu;
using proto.gp;

public class GuPaiJiuDrawPokerrView : UIBtnGuPaiJiuViewBase
{
    [SerializeField]
    private GameObject m_BtnOpen;//全开      
    [SerializeField]
    private Image OpenPokerTimeImage;//开牌提示    

    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(ConstantGuPaiJiu.TellIsBankeDraw, TellIsBankeDraw);//通知翻牌
        dic.Add("OnSeatInfoChanged", OnSeatInfoChanged);//断线重连和创建房间使用
        dic.Add(ConstantGuPaiJiu.CloseDrawPoker, CloseDrawPoker);//结算的时候关闭翻牌                  
        return dic;
    }


    protected override void OnAwake()
    {
        base.OnAwake();
        if (m_BtnOpen != null) m_BtnOpen.SetActive(false);
        if (OpenPokerTimeImage != null) OpenPokerTimeImage.gameObject.SetActive(false); ;
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case ConstantGuPaiJiu.btnGuPaiJiuViewQuanOpen:
                GuPaiJiuGameCtrl.Instance.GuPaiJiuClisentSendDrawOpen();
                break;
        }
    }
    private void OnSeatInfoChanged(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        bool isPlayer = data.GetValue<bool>("IsPlayer");
        RoomEntity room = data.GetValue<RoomEntity>("Room");
        ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
        if (roomStatus == ROOM_STATUS.CHECK)
        {
            if (isPlayer)
                SetCheckPoker(roomStatus, seat, isPlayer, room.roomUnixtime);
        }
    }


    /// <summary>
    /// 关闭翻牌
    /// </summary>
    private void CloseDrawPoker(TransferData data)
    {
        m_BtnOpen.SetActive(false);
        OpenPokerTimeImage.gameObject.SetActive(false);
        CloseTime(null);
    }

    /// <summary>
    /// 关闭时间的方法
    /// </summary>
    /// <param name="data"></param>
    private void CloseTime(TransferData data)
    {
        TimeObj.SetActive(false);
        isOpenTime = false;
    }


    /// <summary>
    /// 通知翻牌
    /// </summary>
    private void TellIsBankeDraw(TransferData data)
    {
        long drawTime = data.GetValue<long>("Time");
        bool IsPlayer = data.GetValue<bool>("IsPlayer");
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
        SetCheckPoker(roomStatus, seat, IsPlayer, drawTime);
    }
    private void SetCheckPoker(ROOM_STATUS roomStatus, SeatEntity seat, bool IsPlayer, long drawTime)
    {
        if (roomStatus == ROOM_STATUS.CHECK)
        {
#if IS_CHUANTONGPAIJIU
            if (IsPlayer)
            {
                m_BtnOpen.SetActive(seat.IsBanker);
            }
#endif
            OpenPokerTimeImage.gameObject.SetActive(true);
            OpenInvertedTime(drawTime);
        }
    }

}
