//===================================================
//Author      : DRB
//CreateTime  ：9/12/2017 7:52:54 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuPaiJiu;
using UnityEngine.UI;

public class GuPaiJiuRoomPokerView : UIViewBase
{
    [SerializeField]
    private Transform roomPokerTran;//房间桌子扑克的挂载点

    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(ConstantGuPaiJiu.LoadRoomPoker, LoadRoomPoker);
        dic.Add(ConstantGuPaiJiu.OnGuPaiRoomInfoChanged, OnRoomInfoChanged);
        dic.Add(ConstantGuPaiJiu.CloseRoomPokerTran, CloseRoomPokerTran);
        return dic;
    }


    private void OnRoomInfoChanged(TransferData data)
    {
        RoomEntity room = data.GetValue<RoomEntity>("Room");
        Debug.Log(room.roomPokerList.Count+"                               房间牌的长度");        
        if (room.roomPokerList.Count>0)
        {           
            LoadRoomPoker(room);
        }
    }

    /// <summary>
    /// 加载房间的牌
    /// </summary>
    /// <param name="data"></param>
    private void LoadRoomPoker(TransferData data)
    {
        RoomEntity room = data.GetValue<RoomEntity>("Room");
        if (room.dealSecond==(room.roomPlay==EnumPlay.BigPaiJiu?2:4)) return;
        LoadRoomPoker(room);
    }

    private void LoadRoomPoker(RoomEntity room)
    {
        CloseRoomPokerTran(null);
        for (int i = 0; i < room.roomPokerList.Count; i++)
        {           
            GuPaiJiuPrefabManager.Instance.LoadPoker(0, room.roomPokerList[i].Type, (GameObject go) =>
            {
                go.transform.SetParent(roomPokerTran);
                go.GetComponent<Image>().raycastTarget = false;
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = new Vector3(0.25f,0.25f,0.25f);
                go.name = room.roomPokerList[i].Type.ToString();
            });
        }
    }

    /// <summary>
    /// 每局结束清空房间牌的挂载点
    /// </summary>

    private void CloseRoomPokerTran(TransferData data)
    {
        if (roomPokerTran.childCount!=0)
        {
            for (int i = 0; i < roomPokerTran.childCount; i++)
            {
                Destroy(roomPokerTran.GetChild(i).gameObject);
            }
        }
    }
  
}
