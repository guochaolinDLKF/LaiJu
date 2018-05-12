//===================================================
//Author      : DRB
//CreateTime  ：1/30/2018 5:15:42 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIRecordDetailInfo : UIRecordDetailBase
{

    [SerializeField]
    private Text[] m_ArrNickName;
    [SerializeField]
    private Text m_txtRoomId;
    [SerializeField]
    private Text m_txtBattleId;
    [SerializeField]
    private Text m_txtMaxLoop;
    [SerializeField]
    private Text m_txtDateTime;
    [SerializeField]
    private Text m_txtGameName;
    [SerializeField]
    private Text[] m_ArrGold;
    [SerializeField]
    private Text m_txtOwnerName;
    protected override string RecordDetailPrefabName
    {
        get
        {
            if (GameType.Equals("doudizhu"))
            {
                return "UIItemRecordDetailDouDiZhu";
            }
            return "UIItemRecordDetailMahjong";
        }
    }



    public override void ShowRecordDetail(TransferData data)
    {
        GameType= data.GetValue<string>("GameType");
        base.ShowRecordDetail(data);
        List<TransferData> lstPlayer = data.GetValue<List<TransferData>>("Player");
        if (lstPlayer == null || lstPlayer.Count == 0) return;

        for (int i = 0; i < m_ArrNickName.Length; ++i)
        {
            if (i < lstPlayer.Count)
            {
                m_ArrNickName[i].gameObject.SetActive(true);
                m_ArrNickName[i].SafeSetText(lstPlayer[i].GetValue<string>("NickName"));
            }
            else
            {
                m_ArrNickName[i].gameObject.SetActive(false);
            }
        }


        int roomId = data.GetValue<int>("RoomId");
        int battleId = data.GetValue<int>("BattleId");
        int maxLoop = data.GetValue<int>("MaxLoop");
        string time = data.GetValue<string>("DateTime");
        string GameName = data.GetValue<string>("ChineseGameName");
        string OwnerName = data.GetValue<string>("OwnerName");

        m_txtRoomId.SafeSetText("房间号 " + roomId.ToString());
        m_txtBattleId.SafeSetText("牌局号 " + battleId.ToString());
        m_txtMaxLoop.SafeSetText("总局数 " + maxLoop.ToString());
        m_txtDateTime.SafeSetText("时间 " + time);
        m_txtGameName.SafeSetText("类型 " + GameName);
        m_txtOwnerName.SafeSetText("房主 " + OwnerName);

        for (int i = 0; i < m_ArrGold.Length; ++i)
        {
            if (i < lstPlayer.Count)
            {
                m_ArrGold[i].gameObject.SetActive(true);
                m_ArrGold[i].SafeSetText(lstPlayer[i].GetValue<int>("Gold").ToString());
            }
            else
            {
                m_ArrGold[i].gameObject.SetActive(false);
            }
        }


    }
}
