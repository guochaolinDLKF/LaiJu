//===================================================
//Author      : DRB
//CreateTime  ：7/20/2017 10:55:37 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhaJh;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class UIZJHTotalSettlement : UIWindowViewBase
{
    [SerializeField]
    private Transform[] playerBottomS;
    [SerializeField]
    private GameObject m_tiemPlayerInfo;
    [SerializeField]
    private Text roomID;//房间ID
    [SerializeField]
    private Text NumberGame;//局数
    [SerializeField]
    private Text PlayMethod;//玩法
    [SerializeField]
    private Text timeText;//时间





    [SerializeField]
    private GameObject btnShare;


    protected override void OnAwake()
    {
        base.OnAwake();
        Button[] btnArr = GetComponentsInChildren<Button>(true);
        for (int i = 0; i < btnArr.Length; i++)
        {
            EventTriggerListener.Get(btnArr[i].gameObject).onEnter += OnPointerEnter;
            EventTriggerListener.Get(btnArr[i].gameObject).onExit += OnPointerExit;
        }
        if (!SystemProxy.Instance.IsInstallWeChat || !SystemProxy.Instance.IsOpenWXLogin)
        {
            if (btnShare != null) btnShare.SetActive(false);

        }

    }

    public  void TotalSettlement(List<SeatEntity> seatList,RoomEntity room)
    {
        for (int i = 0; i < seatList.Count; i++)
        {
            if (m_tiemPlayerInfo!=null)
            {
                GameObject go = Instantiate(m_tiemPlayerInfo);
                go.SetActive(true);
                go.SetParent(playerBottomS[i]);
                go.GetComponent<ZJHConclusionItem>().SetConclusionItem(seatList[i], (i + 1));
            }

        }
        SetUI(room);
    }

    private void SetUI(RoomEntity room)
    {
        Debug.Log(room.roomId+"                                   结算房间号");
        roomID.SafeSetText(string.Format("房号:{0}",room.roomId.ToString()));
        NumberGame.SafeSetText(string.Format("局数:{0}",room.maxLoop.ToString()));
        string roomName = string.Empty;
        switch (room.roomSettingId)
        {
            case RoomMode.Ordinary:
                roomName = "普通玩法";
                break;
            case RoomMode.Senior:
                roomName = "高级玩法";
                break;
            case RoomMode.Passion:
                roomName = "激情玩法";
                break;
            default:
                break;
        }
        if (roomName!=string.Empty)
        {
            PlayMethod.SafeSetText(string.Format("玩法:{0}", roomName));
        }
        timeText.SafeSetText(DateTime.Now.ToString("yyyy/MM/dd   HH:mm"));
        
    }


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "fanhui":
                SendNotification(ZhaJHMethodname.OnZJHReturnHall);
                break;
            case "xuanyao":
                SendNotification("OnBtnResultViewZJHShareClick");
                break;
        }
    }
   

    /// <summary>
    /// 当鼠标离开的时候调用
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(GameObject go)
    {       
        if (go.name == "fanhui" || go.name == "xuanyao")
        {            
            go.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        }
    }
    /// <summary>
    /// 当鼠标进入时候调用
    /// 把按钮放大
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(GameObject go)
    {       
        if (go.name == "fanhui" || go.name == "xuanyao")
        {           
            go.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f),0.2f);
        }
    }


   
}
