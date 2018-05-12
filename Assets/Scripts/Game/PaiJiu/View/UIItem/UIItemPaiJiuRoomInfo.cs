//===================================================
//Author      : WZQ
//CreateTime  ：7/5/2017 5:59:14 PM
//Description ：牌九房间基本信息（电量 局数 等）
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemPaiJiuRoomInfo : UIItemRoomInfoBase
{

    public static UIItemPaiJiuRoomInfo Instance;

    //[SerializeField]
    //private Text m_TextOverplusWallCount;

    [SerializeField]
    private Image m_TextLuckPoker;

    protected override void OnAwake()
    {
        base.OnAwake();
        Instance = this;
        //ModelDispatcher.Instance.AddEventListener("OnOverplusWallCountChange", OnOverplusWallCountChange);(父类中设置剩余牌墙数)
        ModelDispatcher.Instance.AddEventListener(PaiJiu.ConstDefine_PaiJiu.ObKey_RoomInfoChanged, OnRoomInfoChanged); //(重新注册设置局数)
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        //ModelDispatcher.Instance.RemoveEventListener("OnOverplusWallCountChange", OnOverplusWallCountChange);
        ModelDispatcher.Instance.RemoveEventListener(PaiJiu.ConstDefine_PaiJiu.ObKey_RoomInfoChanged, OnRoomInfoChanged);
    }

    /// <summary>
    /// 剩余牌墙数量变更（暂无）
    /// </summary>
    /// <param name="data"></param>
    //private void OnOverplusWallCountChange(TransferData data)
    //{
    //    int count = data.GetValue<int>("OverplusWallCount");
    //    m_TextOverplusWallCount.SafeSetText(string.Format("剩余:{0}", count.ToString()));
    //}

    /// <summary>
    /// 房间信息变更 （base中已注册  若要改变 需覆盖该方法）
    /// </summary>
    /// <param name="obj"></param>
    private void OnRoomInfoChanged(TransferData data)
    {
        Debug.Log("子类UIItemPaiJiuRoomInfo");
        PaiJiu.Room entity = data.GetValue<PaiJiu.Room>("Room");
        ShowLoop(entity.currentLoop, entity.maxLoop);
    }
    private void ShowLoop(int currentLoop, int maxLoop)
    {
        m_TextLoop.SafeSetText(string.Format("游戏局数:{0}/{1}", currentLoop, maxLoop));
    }

}
