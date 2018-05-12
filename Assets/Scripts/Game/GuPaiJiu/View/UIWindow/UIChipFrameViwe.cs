//===================================================
//Author      : CZH
//CreateTime  ：9/5/2017 3:59:27 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuPaiJiu;
using proto.gp;
using UnityEngine.UI;

public class UIChipFrameViwe : UIViewBase
{
    [SerializeField]
    private GameObject m_ChipFrame;//下注筹码框 
    [SerializeField]
    private GameObject m_ChipFrame1;//保定棋牌的下注筹码框 
    [SerializeField]
    private GameObject m_betPool;//注码池
    [SerializeField]
    private GameObject m_betPool1;//注码池
    [SerializeField]
    private Slider m_betSlider;//下注拉条
    [SerializeField]
    private Text m_betSlidetText;//下注拉条数值显示
    [SerializeField]
    private Toggle[] m_petToggles;//下注筹码
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
    private Toggle m_textToggle1;//撤回筹码1
    [SerializeField]
    private Toggle m_textToggle2;//撤回筹码2
    [SerializeField]
    private Toggle m_textToggle3;//撤回筹码3
    [SerializeField]
    private Transform[] m_chipTrans;//生成筹码挂载点
    [SerializeField]
    private Toggle[] m_Toggles;
    [SerializeField]
    private Text[] m_betTexts; //保定棋牌的下注分
    private int ZhuangScore;//庄家剩余分数

    private int textPour1=0;
    private int textPour2=0;
    private int textPour3 = 0;
  
    private int firstPour; //一道
    private int secondPour;//二道
    private int threePour;//三道

    private int sumScore;



    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add("OnSeatInfoChanged", OnSeatInfoChanged);
        dic.Add("OnSeatGameInfoChanged", OnSeatGameInfoChanged);
        dic.Add(ConstantGuPaiJiu.OnGuPaiTellBetInfoChanged, TellBetInfoChanged);//通知下注
        dic.Add(ConstantGuPaiJiu.OnGuPaiSetBetPour, SetBetPour);
        return dic;
    }

    protected override void OnAwake()
    {
        base.OnAwake();        
        m_ChipFrame.SetActive(false);
        m_betPool.SetActive(false);
        m_betPoolGuo.SetActive(false);
        m_ChipFrame1.SetActive(false);
        m_betPool1.SetActive(false);
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case ConstantGuPaiJiu.OnGuPaibtnhand://头
                SetBetPour(1);                
                break;
            case ConstantGuPaiJiu.OnGuPaibtnavenue://二道
                SetBetPour(2);
                break;
            case ConstantGuPaiJiu.OnGuPaibtnthree://三道
                SetBetPour(3);
                break;
            case ConstantGuPaiJiu.OnGuPaibtnComplete://完成
                SetComplete();                            
                break;
            case ConstantGuPaiJiu.OnGuPaibtnWithdraw://撤回
                SetWithdraw();
                break;
            case ConstantGuPaiJiu.OnGuPaibtnDetermine://确定
                BetQueDing();
                break;
        }
    }

    /// <summary>
    ///通知玩家下注
    /// </summary>
    private void TellBetInfoChanged(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");       
        ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
        //SetPourScore(seat,roomStatus);
        SetPool(seat, roomStatus);
        SetChipFrame(seat);
    }

    /// <summary>
    /// 断线重连和创建房间用
    /// </summary>
    private void OnSeatInfoChanged(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
        bool IsPlayer = data.GetValue<bool>("IsPlayer");     
#if IS_BAODINGQIPAI
        if (seat.IsBanker)
        {
            ZhuangScore = seat.firstPour;
            m_betSlider.maxValue += seat.firstPour;           
        } 
#else
        if (seat.IsBanker) ZhuangScore = seat.firstPour;
#endif

        if (IsPlayer)
        {
            SetPourScore(seat, roomStatus);
        }
    }

    private void OnSeatGameInfoChanged(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
        bool IsPlayer = data.GetValue<bool>("IsPlayer");
        if (seat.IsBanker) ZhuangScore = seat.firstPour;       
        if (IsPlayer)
        {
            SetPourScore(seat, roomStatus);
        }
    }


    /// <summary>
    /// 玩家下注后显示分数
    /// </summary>
    /// <param name="data"></param>
    private void SetBetPour(TransferData data)
    {        
        SetPourPool(data);
    }

    /// <summary>
    /// 下注完成，设置下注分数
    /// </summary>
    /// <param name="data"></param>
    private void SetPourPool(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        bool IsPlayer = data.GetValue<bool>("IsPlayer");
        ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
        if (seat.IsBanker) ZhuangScore = seat.firstPour;         
        if (IsPlayer)
        {
            SetPourScore(seat,roomStatus);
        }
    }

    /// <summary>
    /// 设置下注分数
    /// </summary>
    /// <param name="seat"></param>
    /// <param name="roomStatus"></param>
    private void SetPourScore(SeatEntity seat, ROOM_STATUS roomStatus)
    {
        if (m_betPoolGuo != null) m_betPoolGuo.SetActive(false);
        if (m_betPool != null) m_betPool.gameObject.SetActive(false);
        if (m_betPool1 != null) m_betPool1.gameObject.SetActive(false);
        if (seat.IsBanker)
        {            
            if (m_betPoolGuo != null) m_betPoolGuo.SetActive(roomStatus == ROOM_STATUS.POUR || roomStatus == ROOM_STATUS.DEAL || roomStatus == ROOM_STATUS.GROUPPOKER || roomStatus == ROOM_STATUS.CHECK || roomStatus == ROOM_STATUS.SETTLE);
            if (m_betPourGuoText != null) m_betPourGuoText.text = seat.firstPour.ToString();
        }
        else
        {
#if IS_BAODINGQIPAI
            if (m_betPool1 != null) m_betPool1.gameObject.SetActive(roomStatus == ROOM_STATUS.POUR || roomStatus == ROOM_STATUS.DEAL || roomStatus == ROOM_STATUS.GROUPPOKER || roomStatus == ROOM_STATUS.CHECK|| roomStatus == ROOM_STATUS.CUOPAI);
#else
            if (m_betPool != null) m_betPool.gameObject.SetActive(roomStatus == ROOM_STATUS.POUR || roomStatus == ROOM_STATUS.DEAL || roomStatus == ROOM_STATUS.GROUPPOKER || roomStatus == ROOM_STATUS.CHECK);
#endif

            SetBetPour(seat.firstPour, seat.secondPour,seat.threePour);
            SetChipFrame(seat);
        }
    }


    /// <summary>
    /// 设置下注筹码框的显示
    /// </summary>
    /// <param name="seat"></param>
#if IS_BAODINGQIPAI
    private void SetChipFrame(SeatEntity seat)
    {
        m_ChipFrame1.SetActive(seat.seatStatus == SEAT_STATUS.POUR && !seat.IsBanker);
        SetBetSlider(seat);
    }
#else
     private void SetChipFrame(SeatEntity seat)
    {
        m_ChipFrame.SetActive(seat.seatStatus == SEAT_STATUS.POUR && !seat.IsBanker);
        ClosToggle();//把所有的 Toggle 设置成 false
    }
#endif

    /// <summary>
    /// 设置下注 Slider
    /// </summary>
    private void SetBetSlider(SeatEntity seat)
    {
        m_betSlider.value = 0;
        m_betSlider.maxValue = ZhuangScore-seat.firstPour- seat.secondPour-seat.threePour;
        m_betSlider.onValueChanged.AddListener( BetSlider);
    }

    int b = 0;
    private void BetSlider(float a)
    {        
        if ( m_betSlider.value - b!=0)
        {
            int n = Mathf.CeilToInt((m_betSlider.value - b)*0.2f);            
            m_betSlider.value = b + 5* n;
            b = (int)m_betSlider.value;           
            m_betSlidetText.text = m_betSlider.value.ToString();
        }                 
    }

    /// <summary>
    /// 下注确定
    /// </summary>
    private void BetQueDing()
    {
        for (int i = 0; i < m_Toggles.Length; i++)
        {
            if (m_Toggles[i].isOn)
            {
                m_Toggles[i].isOn = false;
                m_betTexts[i].text = m_betSlider.value.ToString();
                sumScore += (int)m_betSlider.value;
                if (i+1>2)
                {
                    m_Toggles[0].isOn = true;
                    sumScore = 0;
                    firstPour = m_betTexts[0].text.ToInt();
                    secondPour = m_betTexts[1].text.ToInt();
                    threePour = m_betTexts[2].text.ToInt();
                    SendNotification(ConstantGuPaiJiu.GuPaiJiuClientSendBottomPour, firstPour, secondPour, threePour);
                    return;
                }
                m_Toggles[i + 1].isOn = true;
                m_betSlider.value = 0;
                m_betSlider.maxValue = ZhuangScore - sumScore;
                break;
            }
        }     
    }

    /// <summary>
    /// 设置头道显示
    /// </summary>
    /// <param name="seat"></param>
    /// <param name="roomStatus"></param>
#if IS_BAODINGQIPAI
    private void SetPool(SeatEntity seat, ROOM_STATUS roomStatus)
    {
        m_betPool1.SetActive(roomStatus == ROOM_STATUS.POUR);
        m_betTexts[0].text = "";
        m_betTexts[1].text = "";
        m_betTexts[2].text = "";

    }
#else
     private void SetPool(SeatEntity seat, ROOM_STATUS roomStatus)
    {
        m_betPool.SetActive(roomStatus == ROOM_STATUS.POUR);
        ClostChip1();
        ClostChip2();
        ClostChip3();
    }
#endif

    //下注按钮点击
    private void SetBetPour(int pour)
    {
        for (int i = 0; i < m_petToggles.Length; i++)
        {
            if (m_petToggles[i].isOn)
            {
                if (i < 3)
                {
                    if ((textPour1 + textPour2+ textPour3 + m_petToggles[i].gameObject.name.ToInt()) > ZhuangScore)
                    {
                        UIViewManager.Instance.ShowMessage("提示", "下注不能超过锅底,请重新下注", MessageViewType.Ok);
                        return;
                    }
                }
                else
                {
                    if (textPour1 + textPour2+ textPour3 + ZhuangScore > ZhuangScore)
                    {
                        UIViewManager.Instance.ShowMessage("提示", "下注不能超过锅底,请重新下注", MessageViewType.Ok);
                        return;
                    }
                }
                if (pour == 1)
                {
                    m_textToggle1.isOn = true;
                    m_textToggle2.isOn = false;
                    m_textToggle3.isOn = false;
                    if (i == m_petToggles.Length - 1)
                    {
                        textPour1 += ZhuangScore;
                    }
                    else
                    {
                        textPour1 += m_petToggles[i].gameObject.name.ToInt();
                        LoadChip(m_petToggles[i].gameObject.name, 1);
                    }
                    m_betPourText1.text = textPour1.ToString();
                }
                else if(pour == 2)
                {
                    m_textToggle1.isOn = false;
                    m_textToggle2.isOn = true;
                    m_textToggle3.isOn = false;
                    if (i == m_petToggles.Length - 1)
                    {
                        textPour2 += ZhuangScore;
                    }
                    else
                    {
                        textPour2 += m_petToggles[i].gameObject.name.ToInt();
                        LoadChip(m_petToggles[i].gameObject.name, 2);
                    }
                    m_betPourText2.text = textPour2.ToString();
                }
                else if(pour == 3)
                {
                    m_textToggle1.isOn = false;
                    m_textToggle2.isOn = false;
                    m_textToggle3.isOn = true;
                    if (i == m_petToggles.Length - 1)
                    {
                        textPour3 += ZhuangScore;
                    }
                    else
                    {
                        textPour3 += m_petToggles[i].gameObject.name.ToInt();
                        LoadChip(m_petToggles[i].gameObject.name, 3);
                    }
                    m_betPourText3.text = textPour3.ToString();
                }
            }
        }
    }

    /// <summary>
    /// 加载筹码
    /// </summary>
    private void LoadChip(string chipName, int pour)
    {
        GuPaiJiuPrefabManager.Instance.LoadChip(chipName,(GameObject go)=> 
        {
            if (pour==1)
            {
                go.transform.SetParent(m_chipTrans[0]);
                go.transform.localPosition = Vector3.zero + new Vector3(Random.Range(1,30), Random.Range(1, 30),0);
                go.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            }
            else if(pour==2)
            {
                go.transform.SetParent(m_chipTrans[1]);
                go.transform.localPosition = Vector3.zero + new Vector3(Random.Range(1, 30), Random.Range(1, 30), 0);
                go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            else if (pour==3)
            {
                go.transform.SetParent(m_chipTrans[2]);
                go.transform.localPosition = Vector3.zero + new Vector3(Random.Range(1, 30), Random.Range(1, 30), 0);
                go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        });
    }

    //设置下注的分数
    private void SetBetPour(int firstPour, int secondPour,int threePour)
    {        
        string textFirst = firstPour == 0 ? "" : firstPour.ToString();       
        string textSecond = secondPour == 0 ? "" : secondPour.ToString();
        string textThree = threePour == 0 ? "" : threePour.ToString();
#if IS_BAODINGQIPAI
        m_betTexts[0].text = textFirst;
        m_betTexts[1].text = textSecond;
        m_betTexts[2].text = textThree;

#else
        if (firstPour == 0) ClostChip1();
        if (secondPour == 0) ClostChip2();
        if (threePour == 3) ClostChip3();
        m_betPourText1.text = textFirst;
        m_betPourText2.text = textSecond;
        m_betPourText3.text = textThree;
#endif

    }

    /// <summary>
    /// 完成按钮
    /// </summary>
    private void SetComplete()
    {
        textPour1 = 0;
        textPour2 = 0;
        textPour3 = 0;
        firstPour = m_betPourText1.text.ToInt();
        secondPour = m_betPourText2.text.ToInt();
        threePour = m_betPourText3.text.ToInt();    
        SendNotification(ConstantGuPaiJiu.GuPaiJiuClientSendBottomPour, firstPour, secondPour,threePour);       
    }
    /// <summary>
    /// 撤回点击
    /// </summary>
    private void SetWithdraw()
    {
        textPour1 = 0;
        ClostChip1();
        textPour2 = 0;
        ClostChip2();
        textPour3 = 0;
        ClostChip3();
    }


    /// <summary>
    /// 清空筹码
    /// </summary>
    private void ClostChip1()
    {
        m_betPourText1.text = "";
        if (m_chipTrans[0].childCount != 0)
        {
            for (int i = 0; i < m_chipTrans[0].childCount; i++)
            {
                Destroy(m_chipTrans[0].GetChild(i).gameObject);
            }
        }
       
    }
    private void ClostChip2()
    {
        m_betPourText2.text = "";
        if (m_chipTrans[1].childCount != 0)
        {
            for (int i = 0; i < m_chipTrans[1].childCount; i++)
            {
                Destroy(m_chipTrans[1].GetChild(i).gameObject);
            }
        }
    }
    private void ClostChip3()
    {
        m_betPourText3.text = "";
        if (m_chipTrans[2].childCount != 0)
        {
            for (int i = 0; i < m_chipTrans[2].childCount; i++)
            {
                Destroy(m_chipTrans[2].GetChild(i).gameObject);
            }
        }
    }
    private void ClosToggle()
    {
        for (int i = 0; i < m_petToggles.Length; i++)
        {
            m_petToggles[i].isOn = false;
            
        }
    }



}
