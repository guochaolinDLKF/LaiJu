//===================================================
//Author      : CZH
//CreateTime  ：6/14/2017 11:08:35 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZhaJHSceneCtrl : SceneCtrlBase
{
    public static ZhaJHSceneCtrl Instance;
    private UISceneZhaJHView m_UISceneZhaJHView;

    protected override void OnAwake()
    {
        base.OnAwake();
        Instance = this;
        UIDispatcher.Instance.AddEventListener(ZhaJHMethodname.OnZJHBtnMicroUp, OnBtnMicroUp);
        UIDispatcher.Instance.AddEventListener(ZhaJHMethodname.OnZJHBtnMicroCancel, OnBtnMicroCancel);
    }
    /// <summary>
    /// 语音按钮取消
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnMicroCancel(object[] obj)
    {
        ChatCtrl.Instance.CancelMicro();
    }

    /// <summary>
    /// 语音按钮抬起
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnMicroUp(object[] obj)
    {
        ChatCtrl.Instance.SendMicro();
    }



    protected override void OnStart()
    {        
        base.OnStart();
        if (DelegateDefine.Instance.OnSceneLoadComplete != null)
        {
            DelegateDefine.Instance.OnSceneLoadComplete();
        }

        GameObject go = UIViewManager.Instance.LoadSceneUIFromAssetBundle(UIViewManager.SceneUIType.ZhaJH);//加载 UI_ROOT
        m_UISceneZhaJHView = go.GetComponent<UISceneZhaJHView>();
        UIItemZhaJHRoomInfo.Instance.SetUI(RoomZhaJHProxy.Instance.CurrentRoom.roomId, RoomZhaJHProxy.Instance.CurrentRoom.scores);//设置房间底分
        UIItemZhaJHRoomInfo.Instance.SetBaseScoreUI(RoomZhaJHProxy.Instance.CurrentRoom.baseScore);//设置房间下注总分
        UIItemZhaJHRoomInfo.Instance.SetTotalRound(RoomZhaJHProxy.Instance.CurrentRoom.round, RoomZhaJHProxy.Instance.CurrentRoom.totalRound);//设置房间轮数
        UIItemZhaJHRoomInfo.Instance.SeatChip(RoomZhaJHProxy.Instance.CurrentRoom.baseScore);//断线重连的时候时候加载房间的筹码
       // RoomZhaJHProxy.Instance.AoutReady();//5秒自动准备
        RoomZhaJHProxy.Instance.SendRoomInfoChangeNotify();//初始化房间数据
        RoomZhaJHProxy.Instance.SendRoomSeatOperation(); //断线重连的时候如果座位是下注状态，应该显示的下注按钮

        AudioBackGroundManager.Instance.Play("bgm_zhajinhua");//背景音乐
        //if (DelegateDefine.Instance.OnSceneLoadComplete != null)
        //{
        //    DelegateDefine.Instance.OnSceneLoadComplete();
        //}

        //GameObject go = UIViewManager.Instance.LoadSceneUIFromAssetBundle(UIViewManager.SceneUIType.ZhaJH);
        //m_UISceneZhaJHView = go.GetComponent<UISceneZhaJHView>();
        //UIItemMahJongRoomInfo.Instance.SetUI(RoomMaJiangProxy.Instance.CurrentRoom.roomId, RoomMaJiangProxy.Instance.CurrentRoom.BaseScore);

    }



    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="room"></param>
    /// <param name="isPlayAnimation"></param>
    /// <param name="isReplay"></param>
    public void Begin()
    {
        if (RoomZhaJHProxy.Instance.CurrentRoom.currentLoop == 1)
        {
            CheckIP(RoomZhaJHProxy.Instance.CurrentRoom.seatList);
        }
    }

    ///// <summary>
    ///// 切换后台
    ///// </summary>
    ///// <param name="focus"></param>
    //private void OnApplicationFocus(bool focus)
    //{
    //    ZhaJHGameCtrl.Instance.ClientSendFocus(focus);
    //}
}