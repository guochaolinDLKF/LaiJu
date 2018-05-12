//===================================================
//Author      : CZH
//CreateTime  ：9/5/2017 10:24:08 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuPaiJiu;
using proto.gp;
using UnityEngine.UI;
using DG.Tweening;

public class UIGuPaiJiuItemSeat : UIItemBase
{
    [SerializeField]
    private UIGuPaiJiuItemPlayerInfo m_PlayerInfo;
    [SerializeField]
    private GameObject m_Ready;
    [SerializeField]
    private int m_nSeatIndex = -1;
    [SerializeField]
    private Image m_betPool;
    [SerializeField]
    private GameObject m_betPool1;
    [SerializeField]
    private GameObject m_betPoolGuo;//庄家显示锅
    [SerializeField]
    private Text m_betPourGuoText;//锅显示的分数
    [SerializeField]
    private Text m_betPourText1;
    [SerializeField]
    private Text m_betPourText2;
    [SerializeField]
    private Text m_betPourText3;
    [SerializeField]
    private Text[] m_betPourXins;
    [SerializeField]
    private Text m_piaoFen;
    [SerializeField]
    private GameObject EndIamge;//组合完成的图片
    [SerializeField]
    private GameObject m_Leave;//玩家离开的显示
    [SerializeField]
    private GameObject m_Qiang;//抢庄图片
    [SerializeField]
    private GameObject m_NoQiang;//不抢图片

    private Tweener settlementScoreTween;



    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add("OnSeatInfoChanged", OnSeatInfoChanged);
        dic.Add("OnSeatGameInfoChanged", OnSeatGameInfoChanged);
        dic.Add("OnPlayerInfo", OnPlayerInfo);
        dic.Add(ConstantGuPaiJiu.OnGuPaiSetBetPour, SetBetPour);//设置 下注分数
        dic.Add(ConstantGuPaiJiu.SetSeatGold, SetSeatGold);//每次结算设置座位 Gold 分数
        dic.Add(ConstantGuPaiJiu.EndIamge, SetEndIamge);//设置玩家组合牌完成的图片
        dic.Add(ConstantGuPaiJiu.SetLeave, SetLeave);//设置玩家离开的图片
        dic.Add(ConstantGuPaiJiu.ISImageGarbBanker, OnSetQiangGarbBankerImage);//设置抢庄或者不抢的图片
        return dic;
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        gameObject.SetActive(false);        
        settlementScoreTween = m_piaoFen.rectTransform.DOMove(m_piaoFen.rectTransform.position + new Vector3(0, 45, 0), 0.5f).SetEase(Ease.Linear).SetAutoKill(false).Pause();
        m_piaoFen.gameObject.SetActive(false);
        if (m_betPoolGuo != null) m_betPoolGuo.SetActive(false);
        if (m_betPool != null) m_betPool.gameObject.SetActive(false);
        if (m_NoQiang != null) m_NoQiang.SetActive(false);
        if (m_Qiang != null) m_Qiang.SetActive(false);
        if (m_betPool1 != null)m_betPool1.SetActive(false);
    }

    private void OnSeatGameInfoChanged(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
        SetSeatInfo(seat, roomStatus);
    }

    private void OnSeatInfoChanged(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
        SetSeatInfo(seat, roomStatus);
        if (roomStatus == ROOM_STATUS.GRABBANKER) OnSetQiangGarbBankerImage(data);

    }

    private void SetSeatInfo(SeatEntity seat, ROOM_STATUS roomStatus)
    {
        if (m_nSeatIndex == seat.Index)
        {
            if (seat.PlayerId == 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
            m_PlayerInfo.SetUI(seat);
            m_Ready.gameObject.SetActive(roomStatus==ROOM_STATUS.READY&&seat.seatStatus == SEAT_STATUS.READY);
            EndIamge.gameObject.SetActive(seat.seatStatus==SEAT_STATUS.GROUPDONE||seat.isCuoPai==1);        
            SetLeave(seat.IsFocus);
            SetBetPourUI(seat,roomStatus);          
        }
    }

    /// <summary>
    /// 设置庄标记
    /// </summary>
    private void OnPlayerInfo(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        if (m_nSeatIndex == seat.Index)            
        {
            m_PlayerInfo.SetUI(seat);
            GuPaiJiuGameCtrl.Instance.ClientSendGrabBakerEnd();//客户端发送抢庄完毕
        }
        m_Qiang.SetActive(false);
        m_NoQiang.SetActive(false);
    }

    private void OnSetQiangGarbBankerImage(TransferData data)
    {      
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
        if (seat.Index== m_nSeatIndex)
        {
            switch (seat.isGrabBanker)
            {
                case 1:
                    m_Qiang.SetActive(roomStatus==ROOM_STATUS.GRABBANKER);
                    break;
                case 2:
                    m_NoQiang.SetActive(roomStatus == ROOM_STATUS.GRABBANKER);
                    break;
                default:
                    break;
            }            
        }
    }


    /// <summary>
    /// 组合完成显示组合完成的图片
    /// </summary>
    /// <param name="data"></param>
    private void SetEndIamge(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
        SetSeatInfo(seat, roomStatus);
    }


    /// <summary>
    /// 当玩家下注结束后设置分数
    /// </summary>
    /// <param name="data"></param>
    private void SetBetPour(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
        SetBetPourUI(seat,roomStatus);      
    }

    private void SetBetPourUI(SeatEntity seat, ROOM_STATUS roomStatus)
    {
        if (m_nSeatIndex == seat.Index)
        {
            if (m_betPoolGuo != null) m_betPoolGuo.SetActive(false);
            if (m_betPool != null) m_betPool.gameObject.SetActive(false);
            if (m_betPool1 != null) if (m_betPool1 != null) m_betPool1.SetActive(false);
            if (seat.IsBanker)
            {
                //if (m_betPoolGuo != null) m_betPoolGuo.SetActive(roomStatus == ROOM_STATUS.POUR || roomStatus == ROOM_STATUS.DEAL || roomStatus == ROOM_STATUS.GROUPPOKER || roomStatus == ROOM_STATUS.CHECK|| roomStatus == ROOM_STATUS.SETTLE);
                if (m_betPoolGuo != null) m_betPoolGuo.SetActive(seat.firstPour != 0);
                if (m_betPourGuoText != null) m_betPourGuoText.text = seat.firstPour.ToString();
            }
            else
            {
#if IS_BAODINGQIPAI
                if (m_betPool1 != null) m_betPool1.SetActive((roomStatus == ROOM_STATUS.POUR && seat.seatStatus == SEAT_STATUS.WAITDEAL) || roomStatus == ROOM_STATUS.DEAL || roomStatus == ROOM_STATUS.GROUPPOKER || roomStatus == ROOM_STATUS.CHECK||roomStatus == ROOM_STATUS.CUOPAI);//&& !m_betPool.gameObject.activeSelf
#else
                if (m_betPool != null ) m_betPool.gameObject.SetActive((roomStatus == ROOM_STATUS.POUR&&seat.seatStatus==SEAT_STATUS.WAITDEAL) || roomStatus == ROOM_STATUS.DEAL || roomStatus == ROOM_STATUS.GROUPPOKER || roomStatus == ROOM_STATUS.CHECK);//&& !m_betPool.gameObject.activeSelf

#endif

                SetBetPour(seat.firstPour, seat.secondPour,seat.threePour);
            }
        }
    }

    /// <summary>
    /// 设置下注的分数
    /// </summary>
    /// <param name="firstPour"></param>
    /// <param name="secondPour"></param>
    private void SetBetPour(int firstPour, int secondPour,int threePour)
    {       
        if (m_betPourText1 == null || m_betPourText2 == null||m_betPourText3==null) return;
        string textFirst = firstPour == 0 ? "" : firstPour.ToString();
        string textSecond = secondPour == 0 ? "" : secondPour.ToString();
        string textThree = threePour == 0 ? "" : threePour.ToString();
#if IS_BAODINGQIPAI
        m_betPourXins[0].text = textFirst;
        m_betPourXins[1].text = textSecond;
        m_betPourXins[2].text = textThree;
#else
         m_betPourText1.text = textFirst;
        m_betPourText2.text = textSecond;
        m_betPourText3.text = textThree;
#endif

    }

    /// <summary>
    /// 每次结算，更新座位 Gold 分数
    /// </summary>
    /// <param name="data"></param>
    private void SetSeatGold(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        bool IsPlayer = data.GetValue<bool>("IsPlayer");
        if (seat.Index== m_nSeatIndex)
        {
            if (IsPlayer) AudioEffectManager.Instance.Play(seat.eamings > 0 ? "gp_shengli" : "gp_shibai", Vector3.zero);//播放胜利或失败的音效         
            m_PlayerInfo.SetUI(seat);
            if (seat.PlayerId != 0)
                StartCoroutine(AnimationScore(seat));
        }
      
    }

    /// <summary>
    /// 飘分
    /// </summary>
    /// <param name="seat"></param>
    /// <returns></returns>
    private IEnumerator AnimationScore(SeatEntity seat)
    {
        m_piaoFen.gameObject.SetActive(true);       
        string scoreText = string.Format(seat.eamings >= 0 ? "+{0}" : "{1}", seat.eamings, seat.eamings);
        m_piaoFen.color = seat.eamings >= 0 ? Color.green : Color.red;
        m_piaoFen.text = scoreText;     
        if (settlementScoreTween != null) settlementScoreTween.Restart();       
        yield return new WaitForSeconds(5f);
        m_piaoFen.gameObject.SetActive(false);     
    }



    /// <summary>
    /// 设置玩家离开
    /// </summary>
    /// <param name="data"></param>
    /// 
    private void SetLeave(bool IsFocus)
    {
        m_Leave.SetActive(!IsFocus);
    }
    private void SetLeave(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");      
        if (seat.Index == m_nSeatIndex)
        {
            SetLeave(seat.IsFocus);          
        }
    }
}
