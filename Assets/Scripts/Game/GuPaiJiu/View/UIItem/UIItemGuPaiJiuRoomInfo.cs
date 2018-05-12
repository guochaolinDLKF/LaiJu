//===================================================
//Author      : DRB
//CreateTime  ：9/5/2017 3:35:27 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GuPaiJiu;

public class UIItemGuPaiJiuRoomInfo : UIItemRoomInfoBase
{
    public static UIItemGuPaiJiuRoomInfo Instance;

    [SerializeField]
    private Text textUpperLimit;//爆锅上限
    [SerializeField]
    private Text ZZModelText;//坐庄模式
    [SerializeField]
    private Text GuiZeText;//规则
    [SerializeField]
    private Text BetModelText;//下注模式
    [SerializeField]
    private Text PlayerID;



    protected override void OnAwake()
    {
        base.OnAwake();
        Instance = this;
        ModelDispatcher.Instance.AddEventListener("OnRoomGameInfoChanged", OnRoomGameInfoChanged);
        ModelDispatcher.Instance.AddEventListener(ConstantGuPaiJiu.OnGuPaiRoomInfoChanged, OnRoomInfoChanged);
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();        
        ModelDispatcher.Instance.RemoveEventListener("OnRoomGameInfoChanged", OnRoomGameInfoChanged);
        ModelDispatcher.Instance.RemoveEventListener(ConstantGuPaiJiu.OnGuPaiRoomInfoChanged, OnRoomInfoChanged);
    }


    private void OnRoomGameInfoChanged(TransferData data)
    {
        RoomEntity entity = data.GetValue<RoomEntity>("Room");
        ShowLoop1(entity.currentLoop, entity.maxLoop);
    }

    private void OnRoomInfoChanged(TransferData data)
    {
        RoomEntity entity = data.GetValue<RoomEntity>("Room");
        ShowZZModel(entity.roomMode,entity,entity.betModel,entity.guDScore,entity.scoreLimit);
        //if (!entity.IsAddPanBase) return;
        ShowUpperLimit(entity.scoreLimit);
        base.ShowLoop(entity.currentLoop, entity.maxLoop, entity.isQuan);
    }


    private void ShowLoop1(int currentLoop, int maxLoop)
    {
        if (m_TextLoop != null) m_TextLoop.SafeSetText(string.Format("游戏局数:{0}/{1}", currentLoop, maxLoop));
    }

    private void ShowUpperLimit(int upperLimit)
    {
        Debug.Log(" `````````````````````````````爆锅上限······································");
        if (textUpperLimit != null) textUpperLimit.SafeSetText(string.Format("爆锅上限:{0}",upperLimit));
    }

    /// <summary>
    /// 房间模式
    /// </summary>
    /// <param name="zuoString"></param>
    /// <param name="guizi"></param>
    /// <param name="tianjiuwang"></param>
    private void ShowZZModel(RoomEntity.RoomMode RoundZhuang, RoomEntity room,RoomEntity.BetModel betModel,int guDScore,int scoreLimit)//,int playerID
    {
        switch (RoundZhuang)
        {
            case RoomEntity.RoomMode.RoundZhuang:
                ZZModelText.SafeSetText("坐庄模式:轮庄");
                break;
            case RoomEntity.RoomMode.RobZhuang:
                ZZModelText.SafeSetText("坐庄模式:抢庄");
                break;          
        }
        string GuiZi = room.enumGuiZi == EnumGuiZi.GuiZi ? "是" : "否";
        string tianjiu = room.enumTianJiuWang == EnumTianJiuWang.TianJiuWang ? "是" : "否";
        string dijiu = room.enumDiJiuWang == EnumDiJiuWang.DiJiuWang ? "是" : "否";

        if (GuiZeText != null) GuiZeText.SafeSetText(string.Format("鬼    子：{0}\n地九王：{1}\n天九王：{2}\n", GuiZi, dijiu, tianjiu));
        switch (betModel)
        {
            case RoomEntity.BetModel.NationalScore:
                if (BetModelText != null) BetModelText.SafeSetText(string.Format("固定分模式    固定分:{0}",guDScore.ToString()));
                break;
            case RoomEntity.BetModel.NoNational:
                if (BetModelText != null) BetModelText.SafeSetText(string.Format("不固定分模式  封顶分:{0}", scoreLimit.ToString()));
                break;         
        }
        if(PlayerID!=null)PlayerID.SafeSetText(string.Format("房主:{0}", room.fzName));
    }



}
