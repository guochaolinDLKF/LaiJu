//===================================================
//Author      : DRB
//CreateTime  ：10/30/2017 4:56:54 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.gp;
using GuPaiJiu;
using UnityEngine.UI;

public class GuPaiJiuGrabBankerView : UIBtnGuPaiJiuViewBase
{
    [SerializeField]
    private GameObject m_GrabBanker;//抢庄或者不抢    
 

    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(ConstantGuPaiJiu.GarbBankerSceneView, GarbBankerSceneView);//通知抢庄
        dic.Add("OnSeatInfoChanged", OnSeatInfoChanged);//断线重连和创建房间使用
        dic.Add(ConstantGuPaiJiu.CloseTime, CloseTime);//关闭时间
        return dic;
    }

    protected override void OnAwake()
    {
        base.OnAwake();       
        if (m_GrabBanker != null) m_GrabBanker.SetActive(false);      
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {           
            case ConstantGuPaiJiu.btnGuPaiJiuQiang://抢庄
                ConfirmGarbBanker(EnumGarbBaker.Qiang);
                break;
            case ConstantGuPaiJiu.btnGuPaiJiuNoQiang://不抢庄
                ConfirmGarbBanker(EnumGarbBaker.NoQiang);
                break;
        }
    }

    private void OnSeatInfoChanged(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        bool isPlayer = data.GetValue<bool>("IsPlayer");
        RoomEntity room = data.GetValue<RoomEntity>("Room");
        ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");     
        if (roomStatus == ROOM_STATUS.GRABBANKER)
        {
            if (isPlayer)
             SetGarbBankerBtn(roomStatus, seat.isGrabBanker, room.roomUnixtime);
        }
    }

    /// <summary>
    /// 抢庄按钮显示
    /// </summary>
    /// <param name="data"></param>
    private void GarbBankerSceneView(TransferData data)
    {
        ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
        int isGrabBaker = data.GetValue<int>("isGrabBaker");
        long roomUnixtime = data.GetValue<long>("Unixtime");
        SetGarbBankerBtn(roomStatus, isGrabBaker, roomUnixtime);
    }
    private void SetGarbBankerBtn(ROOM_STATUS roomStatus, int isGrabBaker, long roomUnixtime)
    {
        m_GrabBanker.SetActive(roomStatus == ROOM_STATUS.GRABBANKER && isGrabBaker == 3);
        if(isGrabBaker==3) OpenInvertedTime(roomUnixtime);//设置时间
    }

    /// <summary>
    /// 发送抢庄或者不抢
    /// </summary>
    private void ConfirmGarbBanker(EnumGarbBaker enumGarbBaker)
    {
        GuPaiJiuGameCtrl.Instance.ClientSendGrabBanker((int)enumGarbBaker);
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

   
}
