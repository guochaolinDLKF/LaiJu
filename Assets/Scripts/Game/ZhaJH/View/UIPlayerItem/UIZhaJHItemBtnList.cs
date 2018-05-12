//===================================================
//Author      : CZH
//CreateTime  ：6/16/2017 7:24:43 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zjh.proto;
using ZhaJh;

public class UIZhaJHItemBtnList : UIViewBase
{   
    public Toggle[] m_togGendaodi;
    public Image m_genZhuHui;
    public Image m_jiaZhuHui;
    public Image m_kanPaiHui;
    public Image m_biPaiHui;
    public Image m_qiPaiHui;
    public Image m_xuePinHui;

    [SerializeField]
    private Image jiazhubgGJ1;//高级 1 分
    [SerializeField]
    private Image jiazhubgGJ2;// 高级 2 分
    [SerializeField]
    private Image jiazhuBg; //普通
    [SerializeField]
    private Image xuepinBg;//血拼

    [SerializeField]
    private Button HidJiaZ;//遮罩Button

    public UIZhaJHItemSeat[] m_Seat;
    [SerializeField]
    private Image zhezhao;
    [SerializeField]
    private Text genScero; //跟注分数
    [SerializeField]
    private Text bpScero;  //比牌分数

    private float Fen = 0;

    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(ZhaJHMethodname.OnZJHBtnShow, BtnShow);
        dic.Add(ZhaJHMethodname.OnZJHHidFen, HidFen);
        dic.Add(ZhaJHMethodname.OnZJHHidBP, HidBP);
        dic.Add(ZhaJHMethodname.OnZJHHidKP, HidKP); //点击看牌按钮让按钮编灰色               
        dic.Add(ZhaJHMethodname.OnZJHBtnQP, BtnQP);//弃牌的时候用的方法
        dic.Add(ZhaJHMethodname.OnZJHNoMaskP, NoMaskP);//取消比牌的遮罩
        dic.Add(ZhaJHMethodname.OnZJHBtnGDD, BtnGDD);
        return dic;
    }
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "jiazhu":
                Choice();
                break;
            case "kanpai":
                if (RoomZhaJHProxy.Instance.PlayerSeat.pokerStatus == ENUM_POKER_STATUS.POSITIVE) return;  //round == 0 || round == 1 ||
                SendNotification(ZhaJHButtonConstant.btnZJHLookPoker);
                break;
            case "bipai":
                MaskP();
                BPPeople();
                //ShowBP(true);                               
                break;
            case "genzhu":
                SendNotification(ZhaJHButtonConstant.btnZJHWithNotes);
                break;
            case "qipai":
                SendNotification(ZhaJHButtonConstant.btnZJHLosePoker);
                break;
            case "xuepin":
                Shopping();
                break;
        }
    }
    /// <summary>
    /// 加注
    /// </summary>
    private void Choice()
    {
        if (RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId != RoomMode.Senior)
        {            
            jiazhuBg.gameObject.SetActive(true);
            jiazhubgGJ1.gameObject.SetActive(false);
            jiazhubgGJ2.gameObject.SetActive(false);
        }
        else if (RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId == RoomMode.Senior && RoomZhaJHProxy.Instance.CurrentRoom.scores == 1)
        {
            jiazhuBg.gameObject.SetActive(false);
            jiazhubgGJ1.gameObject.SetActive(true);
            jiazhubgGJ2.gameObject.SetActive(false);
        }
        else
        {
            jiazhuBg.gameObject.SetActive(false);
            jiazhubgGJ1.gameObject.SetActive(false);
            jiazhubgGJ2.gameObject.SetActive(true);
        }
        HidJiaZ.gameObject.SetActive(true);
    }
    /// <summary>
    /// 血拼
    /// </summary>
    private void Shopping()
    {
        xuepinBg.gameObject.SetActive(true);
        HidJiaZ.gameObject.SetActive(true);
    }


    public void BPPeople()
    {
        int cout = 0;
        // RoomZhaJHProxy.Instance.CurrentRoom.seatList
        for (int i = 0; i < RoomZhaJHProxy.Instance.CurrentRoom.seatList.Count; i++)
        {
            if ((RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.THECARD || RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.BET || RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.WAIT || RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.DEAL) && RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].seatToperateStatus != ENUM_SEATOPERATE_STATUS.Discard)
            {
                cout++;
            }
        }
        if (cout == 2)
        {
            for (int i = 0; i < RoomZhaJHProxy.Instance.CurrentRoom.seatList.Count; i++)
            {
                if ((RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.WAIT || RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.THECARD || RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.DEAL) && RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].seatToperateStatus != ENUM_SEATOPERATE_STATUS.Discard)
                {
                    RoomZhaJHProxy.Instance.theCardPos = RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].pos;
                    UIDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHThanPoker);
                }
            }
        }
        else
        {            
            ShowBP(true);
            zhezhao.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 是否跟到底
    /// </summary>
    public void TolggeEvent()
    {
        RoomZhaJHProxy.Instance.btnGDD = m_togGendaodi[0].isOn;     
    }

    #region 王牌 HidFen() 方法
    /// <summary>
    /// 隐藏加注按钮
    /// </summary>      
    public void HidFen(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        if (seat == null) return;
        if (seat==RoomZhaJHProxy.Instance.PlayerSeat)
        {            
            float fen = data.GetValue<float>("Fen");
            int totalRound = data.GetValue<int>("totalRound");            
            float jZF = RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId != RoomMode.Senior || (RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId == RoomMode.Senior && RoomZhaJHProxy.Instance.CurrentRoom.scores==2) ? 20 : 10;
            if (fen == jZF)
            {
                RoomZhaJHProxy.Instance.CurrentRoom.jiazhuScroc = false;
                m_jiaZhuHui.gameObject.SetActive(true);              
            }
            else
            {
                if (fen == 50)
                {                   
                    m_jiaZhuHui.gameObject.SetActive(true);
                }
                else if (fen == 100)
                {                   
                    m_jiaZhuHui.gameObject.SetActive(true);
                    m_xuePinHui.gameObject.SetActive(true);
                }
                else
                {                                  
                    m_jiaZhuHui.gameObject.SetActive(false);
                }
            }
        }       
    }
    #endregion

    #region 点击看牌按钮后，让看牌按钮变灰色
    /// <summary>
    /// 点击看牌按钮后，让看牌按钮变灰色
    /// </summary>
    /// <param name="data"></param>
    public void HidKP(TransferData data)
    {
        m_kanPaiHui.gameObject.SetActive(true);
        genScero.text = (Fen * 2).ToString();
        bpScero.text = (Fen * 2).ToString();
    }
    #endregion




    #region 王牌 BtnShow() 方法
    public void BtnShow(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        if (seat==RoomZhaJHProxy.Instance.PlayerSeat)
        {
            int totalRound = data.GetValue<int>("totalRound");
            float fen = data.GetValue<float>("Fen");
            Debug.Log(fen+"                                 下注分数11111");
            Fen = fen;
            int round = seat.seatRound;
            if (round == 0)
            {
                round = 1;
            }           
            if (round <= RoomZhaJHProxy.Instance.SureWheelNumber)
            {
                m_kanPaiHui.gameObject.SetActive(true);
                m_biPaiHui.gameObject.SetActive(true);
                if (seat.pokerStatus == ENUM_POKER_STATUS.POSITIVE)
                {
                    genScero.text = (fen * 2).ToString();
                    bpScero.text = (fen * 2).ToString();                  
                }
                else
                {
                    genScero.text = fen.ToString();
                    bpScero.text = fen.ToString();                   
                }
            }
            else
            {
                if (seat.pokerStatus == ENUM_POKER_STATUS.POSITIVE)
                {
                    genScero.text = (fen * 2).ToString();
                    bpScero.text= (fen * 2).ToString();
                    m_kanPaiHui.gameObject.SetActive(true);
                }
                else
                {
                    genScero.text = fen.ToString();
                    bpScero.text = fen.ToString();
                    m_kanPaiHui.gameObject.SetActive(false);
                }
                if (JudgeLow(seat, fen) && RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId == RoomMode.Senior)
                {
                    m_biPaiHui.gameObject.SetActive(true);
                }
                else
                {
                    m_biPaiHui.gameObject.SetActive(false);
                }
            }
            if (seat.isLowScore)
            {
                m_qiPaiHui.gameObject.SetActive(true);
            }
            else
            {
                m_qiPaiHui.gameObject.SetActive(false);
            }

            if ((totalRound - round) < 2 && RoomZhaJHProxy.Instance.isShopping == 1 && RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId != RoomMode.Senior)
            {
                m_xuePinHui.gameObject.SetActive(false);
            }
            else
            {
                m_xuePinHui.gameObject.SetActive(true);
            }
            m_genZhuHui.gameObject.SetActive(false);
        }
        else
        {
            BtnQP(null);
        }     
    }
    #endregion


    /// <summary>
    /// 判断是不是低分模式
    /// </summary>
    /// <returns></returns>
    private bool JudgeLow(SeatEntity seat, float fen)
    {
        if (seat.pokerStatus == ENUM_POKER_STATUS.OPPOSITE)
        {
            if (seat.gold - fen > 50) return false;
            return true;
        }
        else
        {
            if ((seat.gold - 2 * fen) > 50) return false;
            return true;
        }
    }

    private void BtnGDD(TransferData data)
    {
        return;
    }
    #region 王牌 BtnQP() 方法
    public void BtnQP(TransferData data)
    {     
        m_kanPaiHui.gameObject.SetActive(true);
        m_biPaiHui.gameObject.SetActive(true);
        m_qiPaiHui.gameObject.SetActive(true);
        m_genZhuHui.gameObject.SetActive(true);
        m_jiaZhuHui.gameObject.SetActive(true);
        m_xuePinHui.gameObject.SetActive(true);
    }
    #endregion




    /// <summary>
    /// 显示和谁比牌的按钮
    /// </summary>
    private void ShowBP(bool isbool)
    {
        if (!isbool) zhezhao.gameObject.SetActive(isbool);
        int index = 0;
        for (int i = 0; i < RoomZhaJHProxy.Instance.CurrentRoom.seatList.Count; i++)
        {
            if (RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.WAIT || RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.THECARD|| RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.DEAL)
            {
                if (RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].seatToperateStatus != ENUM_SEATOPERATE_STATUS.Discard&& !RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].isLowScore)
                {
                    index = RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].Index;
                    for (int j = 0; j < m_Seat.Length; j++)
                    {
                        if (index == m_Seat[j].m_nSeatIndex)
                        {                            
                            if (m_Seat[j].btnbipai == null) continue;
                            m_Seat[j].btnbipai.gameObject.SetActive(isbool);                           
                            //if (m_Seat[j].m_zhuangtai.enabled)
                            //{
                            //    m_Seat[j].m_zhuangtai.enabled = !isbool;
                            //    m_Seat[j].isEnabled = true;
                            //}
                            //if (m_Seat[j].isEnabled)
                            //{
                            //    m_Seat[j].m_zhuangtai.enabled = !isbool;
                            //}
                        }
                    }
                }
            }
        }
    }



    private void HidBP(TransferData data)
    {
        bool isbool = data.GetValue<bool>("isbool");
        ShowBP(isbool);
    }

    /// <summary>
    /// 选择比牌人遮罩
    /// </summary>  
    private void MaskP()
    {
        for (int i = 0; i < RoomZhaJHProxy.Instance.CurrentRoom.SeatCount; i++)
        {
            if (RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].seatToperateStatus == ENUM_SEATOPERATE_STATUS.Discard || RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.IDLE|| RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].isLowScore)
            {
                for (int j = 0; j < m_Seat.Length; j++)
                {
                    if (RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].Index == m_Seat[j].m_nSeatIndex)
                    {
                        m_Seat[j].transform.SetSiblingIndex(0);
                       // break;
                    }
                }
            }
        }
    }
    /// <summary>
    /// 取消比牌遮罩重新排序Item
    /// </summary>
    private void NoMaskP(TransferData data)
    {
        zhezhao.transform.SetSiblingIndex(0);
        for (int i = 0; i < m_Seat.Length; i++)
        {
            m_Seat[i].transform.SetSiblingIndex(i + 1);
            m_Seat[i].isEnabled = false;
        }
    }






}
