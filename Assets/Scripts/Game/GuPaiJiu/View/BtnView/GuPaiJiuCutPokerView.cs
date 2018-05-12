//===================================================
//Author      : DRB
//CreateTime  ：10/30/2017 2:42:53 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GuPaiJiu;
using proto.gp;

public class GuPaiJiuCutPokerView : UIBtnGuPaiJiuViewBase
{
    [SerializeField]
    private GuPaiJiuSeatCtrl[] m_Seats;  
    [SerializeField]
    private GameObject m_CutPokerObj;//切牌或者不切
    [SerializeField]
    private GameObject m_CutPokerImage;//切牌提示
    [SerializeField]
    private Text m_CutPokerText;//切牌文字  
    private ROOM_STATUS curRoomStatus;//切牌房间状态

    private bool isSendCutPoker;//是否向服务器发送切牌结束
    private bool isSeat;//切牌结束时当前客户端的话，向服务器发切牌完成
    private bool isSendNoCutPoker;//时间到向服务器发送不切牌
    private Queue<int> CutPokerAniQueue = new Queue<int>(); //切牌队列





    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(ConstantGuPaiJiu.StartCutPoker1, StartCutPoker1);
        dic.Add(ConstantGuPaiJiu.CutPokerAni, CutPokerAni);//切牌动画    
        dic.Add(ConstantGuPaiJiu.CutPokerEnd, CutPokerEnd);//切牌完毕
        dic.Add(ConstantGuPaiJiu.TellCutPoker, TellCutPoker);//通知切牌
        dic.Add(ConstantGuPaiJiu.StartCutPoker, StartCutPoker);//开始切牌
        dic.Add(ConstantGuPaiJiu.NoCutPoker, NoCutPoker);//不切
        dic.Add(ConstantGuPaiJiu.CloseCutPokerImage, CloseCutPokerImage);

        return dic;
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        if (m_CutPokerObj) m_CutPokerObj.SetActive(false);
        if (m_CutPokerImage != null) m_CutPokerImage.SetActive(false);
        if (TimeObj != null) TimeObj.SetActive(false);
    }


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case ConstantGuPaiJiu.btnGuPaiJiuViewCut://切牌
                ConfirmCutPoker(EnumCutPoker.Cut);
                break;
            case ConstantGuPaiJiu.btnGuPaiJiuViewNoCut://不切牌
                ConfirmCutPoker(EnumCutPoker.NoCut);
                break;
        }
    }



    private void TellCutPoker(TransferData data)
    {
        bool isCut = data.GetValue<bool>("isCut");
        bool isWait = data.GetValue<bool>("isWait");
        bool IsPlayer = data.GetValue<bool>("IsPlayer");
        long unixtime = data.GetValue<long>("Unixtime");
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");       
        OpenInvertedTime(unixtime);
        if (IsPlayer)
        {
            onComplete = OnCutPokerEnd;//添加委托
            isSendNoCutPoker = true;
            if (m_CutPokerObj) m_CutPokerObj.SetActive(isCut);
            m_CutPokerImage.SetActive(false);
        }
        else
        {
            if (isCut && isWait)
            {
                m_CutPokerImage.SetActive(isWait || isCut);
                string cutPokerText = string.Format("{0} 切牌中...", seat.Nickname);
                m_CutPokerText.text = cutPokerText;
            }
        }
    }

    /// <summary>
    /// 是否可以切牌
    /// </summary>
    private bool isPlayCutPokerAni = false;
    

    /// <summary>
    /// 开始切牌（选择切牌顿数）
    /// </summary>
    /// <param name="data"></param>
    private void StartCutPoker(TransferData data)
    {
        bool IsPlayer = data.GetValue<bool>("IsPlayer");
        if (m_CutPokerObj) m_CutPokerObj.SetActive(false);
        if (IsPlayer)
        {
            isSendNoCutPoker = false;
            isSendCutPoker = true;
            RoomEntity room = data.GetValue<RoomEntity>("Room");
            curRoomStatus = room.roomStatus;
            isPlayCutPokerAni = true;
        }
    }

    private void StartCutPoker1(TransferData data)
    {
        GameObject go = data.GetValue<GameObject>("Obj");
        SeatEntity seat = GetIsBanke();
        if (seat != null)
        {
            GuPaiJiuSeatCtrl currSeatCrtl = GetSeatCtrlBySeatPos(seat.Index);
            if (curRoomStatus == ROOM_STATUS.CUTPOKER && isPlayCutPokerAni&& !isCutPokerBig)
            {
                for (int i = 0; i < currSeatCrtl.pokerWall.Count; i++)
                {
                    if (go == currSeatCrtl.pokerWall[i])
                    {
                        int index = go.transform.GetSiblingIndex();
                        Debug.Log(index + "          ······ ·······     切牌顿数");
                        GuPaiJiuGameCtrl.Instance.OnGuPaiJiuClientSendCutPokerInfo(index / 2);
                        break;
                    }
                }
            }
        }
    }

    private bool isCutPokerBig = false;
    /// <summary>
    /// 播放切牌动画
    /// </summary>
    /// 
    private void CutPokerAni(TransferData data)
    {
        int dun = data.GetValue<int>("Dun");
        CutPokerAniQueue.Enqueue(dun);
        if (!isCutPokerBig)
        {
            CutPokerAni();
            isCutPokerBig = true;
        }
    }
    private void CutPokerAni()
    {
        SeatEntity seat = GetIsBanke();
        if (seat != null)
        {
            GuPaiJiuSeatCtrl currSeatCrtl = GetSeatCtrlBySeatPos(seat.Index);
            if (currSeatCrtl != null)
            {
                int dun = CutPokerAniQueue.Dequeue();
                //动画结束事件？？  
                Debug.Log(dun + "      ````````````````````````````````````````````````开始切牌动画");
                currSeatCrtl.CutPokerAni(dun, () =>
                {
                    if (CutPokerAniQueue.Count > 0)
                    {
                        CutPokerAni();
                    }
                    else
                    {
                        isCutPokerBig = false;
                        OnCutPokerEnd();
                        //SendCurPokerEnd();//客户端发送切牌结束                           
                        return;
                    }
                });
            }
        }
    }

    private void CloseCutPokerImage(TransferData data)
    {
        CloseCutPoker();
    }
    /// <summary>
    /// 切牌结束
    /// </summary>
    /// <param name="data"></param>
    public void CutPokerEnd(TransferData data)
    {
       
        CloseCutPoker();
    }
    /// <summary>
    /// 选择不切牌
    /// </summary>
    private void NoCutPoker(TransferData data)
    {
        CloseCutPoker();
    }

    private void CloseCutPoker()
    {
        CloseTime(null);
        if (m_CutPokerObj != null) m_CutPokerObj.SetActive(false);
        if (m_CutPokerImage != null) m_CutPokerImage.SetActive(false);
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


    /// 客户端发送切牌结束
    /// </summary>
    /// <param name="isCutPoker"></param>
    private void SendCurPokerEnd()
    {
        if (!isOpenTime && !isCutPokerBig)
        {
            Debug.Log("切牌完毕·········································");
            isPlayCutPokerAni = false;
            isSendCutPoker = false;           
            GuPaiJiuGameCtrl.Instance.GuPaiJiuClientCutPokerDone();//客户端发送切牌结束                
        }
    }

    /// <summary>
    /// 客户端发送切牌或者不切牌
    /// </summary>
    /// <param name="isCutPoker"></param>
    private void ConfirmCutPoker(EnumCutPoker isCutPoker)
    {       
        isSendNoCutPoker = false;
        SendNotification(ConstantGuPaiJiu.GuPaiJiuClisentSendCutPoker, (int)isCutPoker);        
    }

    /// <summary>
    /// 获得庄家座位号
    /// </summary>
    /// <returns></returns>
    private SeatEntity GetIsBanke()
    {
        RoomEntity room = RoomGuPaiJiuProxy.Instance.CurrentRoom;
        for (int i = 0; i < room.seatList.Count; i++)
        {
            if (room.seatList[i].IsBanker)
            {
                SeatEntity seat = room.seatList[i];
                return seat;
            }
        }
        return null;
    }

    #region GetSeatCtrlBySeatPos 根据座位号获取座位控制器
    /// <summary>
    /// 根据座位号获取座位控制器
    /// </summary>
    /// <param name="seatPos"></param>
    /// <returns></returns>
    private GuPaiJiuSeatCtrl GetSeatCtrlBySeatPos(int Index)
    {
        for (int i = 0; i < m_Seats.Length; ++i)
        {
            if (m_Seats[i].SeatIndex == Index)
            {
                return m_Seats[i];
            }
        }
        return null;
    }
    #endregion


    /// <summary>
    /// 时间到了检测
    /// </summary>
    protected void OnCutPokerEnd()
    {
        if (isSendCutPoker)
        {
            Debug.Log("---------------------------------------------------------------");
            SendCurPokerEnd();
        }

        if (isSendNoCutPoker)     
            ConfirmCutPoker(EnumCutPoker.NoCut);//&&isSeat             
    }
   
}
