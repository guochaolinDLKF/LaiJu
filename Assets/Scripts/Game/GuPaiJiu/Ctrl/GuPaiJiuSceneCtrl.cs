//===================================================
//Author      : CZH
//CreateTime  ：9/5/2017 3:35:53 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuPaiJiu;

public class GuPaiJiuSceneCtrl : SceneCtrlBase
{
    public static GuPaiJiuSceneCtrl Instance;
    private UISceneGuPaiJiuView m_UISceneGuPaiJiuView;

    protected override void OnAwake()
    {
        base.OnAwake();
        Instance = this;
        UIDispatcher.Instance.AddEventListener("OnBtnMicroUp", OnBtnMicroUp);
        UIDispatcher.Instance.AddEventListener("OnBtnMicroCancel", OnBtnMicroCancel);
        GuPaiJiuPrefabManager.Instance.ClosDic();
    }
    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        GuPaiJiuPrefabManager.Instance.ClosDic();

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
        GameObject go = UIViewManager.Instance.LoadSceneUIFromAssetBundle(UIViewManager.SceneUIType.GuPaiJiu);//加载 UI_ROOT
        m_UISceneGuPaiJiuView = go.GetComponent<UISceneGuPaiJiuView>();
        UIItemGuPaiJiuRoomInfo.Instance.SetUI(RoomGuPaiJiuProxy.Instance.CurrentRoom.roomId, 0);//设置房间底分
        RoomGuPaiJiuProxy.Instance.SendRoomInfoChangeNotify();//初始化房间数据      
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="room"></param>
    /// <param name="isPlayAnimation"></param>
    /// <param name="isReplay"></param>
    public void Begin()
    {
        if (RoomGuPaiJiuProxy.Instance.CurrentRoom.currentLoop == 1)
        {
            CheckIP(RoomGuPaiJiuProxy.Instance.CurrentRoom.seatList);
        }
    }

    /// <summary>
    /// 玩家掉线通知
    /// </summary>
    /// <param name="focus"></param>
    private void OnApplicationFocus(bool focus)
    {
        Debug.Log(focus+"                            是否切出去");//切出去的时候 发送 fales，切回来True
        GuPaiJiuGameCtrl.Instance.ClientSendFocus(focus);
    }

}
