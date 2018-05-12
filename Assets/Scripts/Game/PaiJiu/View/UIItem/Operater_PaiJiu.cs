//===================================================
//Author      : WZQ
//CreateTime  ：7/6/2017 8:08:46 PM
//Description ：玩家自己座位UI 交互
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PaiJiu;
using proto.paigow;
public class Operater_PaiJiu : UIViewBase
{
    //---------下注-----------------
    [SerializeField]
    private Transform m_PousParent;//下注项
    [SerializeField]
    private Button[] m_PousBtn;//5个下注按钮

    [SerializeField]
    private Button m_ConfirmPour;//确认下注

    [SerializeField]
    private Button m_CancelPour;//撤销下注

    [SerializeField]
    private Text m_ReadyPourText;//显示准备下注的分数

    private int readyPour = 0;//准备下注的分数
    private int baseScore = 0;//房间底注
    private bool isQuanXia = false;
   
    //---------选庄-----------------
    [SerializeField]
    private GameObject m_ChooseBankerParent;//选择是否做庄按钮 父物体
   //---------抢庄-----------------
    [SerializeField]
    private GameObject m_RobBankerParent;//抢庄父物体
    [SerializeField]
    private Button m_StartGame;//开始游戏按钮
    [SerializeField]
    private GameObject m_CutGuoItem;//切锅项

    [SerializeField]
    private GameObject m_CutPokerItem; // 切牌项


    public override Dictionary<string, ModelDispatcher.Handler> DicNotificationInterests()
    {
        Dictionary<string, ModelDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();

        NoticeJetton(false);
        //m_PousParent.gameObject.SetActive(false);
        m_ChooseBankerParent.SetActive(false);
        return dic;
    }

    //对View 提供初始化 ？？？
    public void UIInit()
    {
        NoticeJetton(false);
        //m_PousParent.gameObject.SetActive(false);
        m_ChooseBankerParent.SetActive(false);
    }

    #region OnBtnClick  按钮点击
    protected override void OnBtnClick(GameObject go)
    {

        int pour = StringUtil.ToInt(go.name);
        if (pour != 0)
        {
            Pour(pour);
            return;
        }

        switch (go.name)
        {
      
            case "ConfirmPour"://确认下注
                ConfirmPour();
                break;
            case "CancelPour"://撤销
                CancelPour();
                break;
            case "btnZuoZhuang"://坐庄
                ConfirmChooseBanker(true);
                break;
            case "btnBuZuoZhuang"://不坐庄
                ConfirmChooseBanker(false);
                break;
            case "btnQiangZhuang"://抢庄
                ConfirmRobBanker(true);
                break;
            case "btnBuQiang"://不抢庄
                ConfirmRobBanker(false);
                break;
            case "QuanXia"://全下
                isQuanXia = true;
                Pour(baseScore);
                break;
            case "btnKaiShi"://开始按钮
                SendNotification(ConstDefine_PaiJiu.ObKey_btnStartGame);
                break;
            case "btnQieGuo"://切锅
                SendNotification(ConstDefine_PaiJiu.ObKey_btnQieGuo,new object[] { true});
                break;
            case "btnBuQieGuo"://不切锅
                SendNotification(ConstDefine_PaiJiu.ObKey_btnQieGuo, new object[] { false });
                break;
            case "btnQiePai"://切牌
                ConfirmCutPoker(Seat.CutPoker.CutPoker);
                break;
            case "btnBuQie"://不切牌
                ConfirmCutPoker(Seat.CutPoker.NoCutPoker);
                break;
            default:
                break;
        }
    }
    #endregion

    #region SetUI 设置按钮显影

    public void SetUI(Room currentRoom, Seat seat, Seat BankerSeat)
    {
        ROOM_STATUS roomStatus = currentRoom.roomStatus;

        //下注项
        NoticeJetton(roomStatus == ROOM_STATUS.POUR && (BankerSeat != null) && ((seat.IsBanker && seat.Pour <= 0) || ((seat.Pour <= 0) && (!seat.IsBanker) && BankerSeat.Pour > 0)), seat, BankerSeat != null ? BankerSeat.Pour : 0);

        //选庄按钮
        ChooseBanker(roomStatus == ROOM_STATUS.CHOOSEBANKER && currentRoom.ChooseBankerSeat != null && currentRoom.ChooseBankerSeat == seat);
        StartGameRelated(currentRoom, seat);
#if IS_ZHANGJIAKOU
            CutPokerRelated(currentRoom, seat);
#endif
        //抢庄按钮
        if (m_RobBankerParent != null) m_RobBankerParent.SetActive(roomStatus == ROOM_STATUS.GRABBANKER && BankerSeat == null && seat.isGrabBanker == 3);
        IsCutGuoRelated(currentRoom, seat);

    }
    #endregion

    #region NoticeJetton 设置下注项显影  
    /// <summary>
    /// 通知下注（显示自己开始下注相关）(开启下注按钮)
    /// </summary>
    public void NoticeJetton(bool OnOff,Seat seat=null, int bankerPour = 0)
    {
       
        //是否全下
        if (m_PousParent.gameObject.activeSelf != OnOff)
        {
           Debug.Log("设置下注相关 显影" + OnOff);
            readyPour = 0;
            baseScore = bankerPour;
            isQuanXia = false;
            SetReadyPourText();
            m_PousParent.gameObject.SetActive(OnOff);
        }

    }
    
    //5个下注按钮 添加下注分数
    private void Pour(int pour)
    {
        //点击按钮之后应该是增加 准备下注分
        if (!m_ConfirmPour.gameObject.activeSelf) return;
        readyPour += pour;
        readyPour = Mathf.Clamp(readyPour, 0, baseScore);
        SetReadyPourText();
    }

    /// <summary>
    /// 确认下注的分数
    /// </summary>
    private void ConfirmPour()
    {
        if (readyPour == 0) return;//下注为0 不发送消息
        object[] obj = new object[] { readyPour, isQuanXia };
        SendNotification(ConstDefine_PaiJiu.ObKey_btnPour, obj);

    }

    /// <summary>
    /// 撤销准备下注的分数
    /// </summary>
    private void CancelPour()
    {
        isQuanXia = false;
        readyPour = 0;
        SetReadyPourText();
    }


    //显示准备下注分数
    private void SetReadyPourText()
    {
        m_ReadyPourText.SafeSetText(string.Format("{0}", readyPour.ToString()));
    }
    #endregion

    #region ChooseBanker 设置 选庄.抢庄 相关项显影
    //设置选庄项
    public void ChooseBanker(bool OnOff)
    {
        if (m_ChooseBankerParent.activeSelf != OnOff)
        {
            m_ChooseBankerParent.SetActive(OnOff);
            //动画效果？？
        }
    }

    //是否坐庄按钮监听
    private void ConfirmChooseBanker(bool isChooseBanker)
    {
        object[] obj = new object[] { isChooseBanker };
        SendNotification(ConstDefine_PaiJiu.ObKey_btnChooseBanker, obj);
    }
    //是否抢庄按钮监听
    private void ConfirmRobBanker(bool isRobBanker)
    {
        object[] obj = new object[] { (isRobBanker?1:2) };
        SendNotification(ConstDefine_PaiJiu.ObKey_btnRobBanker, obj);
    }
    #endregion

    #region StartGameRelated  设置开始游戏相关
    public void StartGameRelated(Room currRoom, Seat playerSeat)
    {
        ROOM_STATUS roomStart = currRoom.roomStatus;
        List<Seat> seatList = currRoom.SeatList;
        Debug.Log("设置开始游戏相关 roomStart: " + roomStart);
       
        if (m_StartGame != null)
        {
            bool isAllReady = (playerSeat.IsBanker && roomStart == ROOM_STATUS.READY);
            int playerSum = 0;
            
            for (int i = 0; i < seatList.Count; i++)
            {
                if (seatList[i].PlayerId > 0) playerSum++;
                if (seatList[i].PlayerId > 0 && seatList[i].seatStatus != SEAT_STATUS.SEAT_STATUS_READY)
                {
                    isAllReady = false;
                    break;
                }
            }
            if (m_StartGame != null) m_StartGame.gameObject.SetActive(isAllReady && playerSum>=2);
        }
    }
    #endregion

    #region IsCutGuoRelated 切锅相关显影
    public void IsCutGuoRelated(Room currRoom, Seat playerSeat)
    {
        ROOM_STATUS roomStatus = currRoom.roomStatus;
        //在一局打了n把之后可切锅  在结算阶段 未操作时显示
#if IS_ZHANGJIAKOU
        if (m_CutGuoItem != null) m_CutGuoItem.gameObject.SetActive(playerSeat.IsBanker && playerSeat.isCutGuo == 3 && roomStatus == ROOM_STATUS.SETTLE && currRoom.dealTime >= 4);
#elif IS_PAIJIU
        if (m_CutGuoItem != null) m_CutGuoItem.gameObject.SetActive(playerSeat.IsBanker && playerSeat.isCutGuo == 3 && roomStatus == ROOM_STATUS.SETTLE && currRoom.dealTime >= 3);
#endif
    }
    #endregion

    #region CutPokerRelated  切牌相关项
    public void CutPokerRelated(Room currRoom, Seat playerSeat)
    {
        //切牌状态 自己为当前切牌座位  切牌状态为未操作
        if (m_CutPokerItem != null) m_CutPokerItem.SetActive(currRoom.roomStatus== ROOM_STATUS.CUTPOKER && playerSeat.isCutPoker == Seat.CutPoker.IsNotOperating);
    }

    //是否切牌按钮监听
    private void ConfirmCutPoker(Seat.CutPoker isCutPoker)
    {
        object[] obj = new object[] { (int)isCutPoker };
        SendNotification(ConstDefine_PaiJiu.ObKey_btnCutPoker, obj);

    }
    #endregion
}
