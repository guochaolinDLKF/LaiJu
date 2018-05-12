//===================================================
//Author      : CZH
//CreateTime  ：6/14/2017 3:18:01 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhaJh;
using zjh.proto;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class UIZhaJHItemSeat : UIItemBase
{
    public UIZhaJHItemPlayerInfo m_PlayerInfo;

    [SerializeField]
    public GameObject m_Ready;
    [SerializeField]
    protected Transform m_UIAnimationContainer;
    [SerializeField]
    public int m_nSeatIndex = -1;
    public Image m_zhuangtai;//显示看牌的图片
    public Button btnbipai;//座位显示和谁比牌的按钮
    public Image m_ImageTime;//倒计时时间图片

    public Transform chipTran;//筹码生成位置
    public Transform chipMountTran;//筹码挂载点

    public GameObject[] pokerMounts;//扑克的挂载点和移动到的位置
    public GameObject m_pokerMount;//扑克牌的挂载点父物体

    private bool isTime = false;//控制倒计时的开关
    private float stime = 0;//倒计时剩余时间
    private float ztime = 0;//倒计时总时间
    [SerializeField]
    private GameObject VSMask;//VS比牌遮罩
    [SerializeField]
    private Transform VsTran;//比牌位移点
    [SerializeField]
    private Transform VsTran1;//比牌位移点
    [SerializeField]
    public Text SettlementScore;//结算时分数上移
    [HideInInspector]
    public bool isEnabled = false;//选择比牌的时候是否打开看牌的标记
    [SerializeField]
    private GameObject KPMask;
    [SerializeField]
    private Image Notice1;//低分警告
    [SerializeField]
    private Image Notice2;//低分警告
    [SerializeField]
    private Image SuperHomeowner;//超级房主标志

    private Tweener settlementScoreTween;

    private bool isLowScore = false;
    [SerializeField]
    private GameObject guangshu; //灯光
    private SeatEntity storageSeat = null;//搓牌的时候储存座位信息

    public Image display;//显示牌型图片

    private bool isDisplay=false;


    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(ZhaJHMethodname.OnZJHSeatInfoChanged, OnSeatInfoChanged);
        dic.Add(ZhaJHMethodname.OnZJHCloseCountTime, CloseCountTime);//关闭倒计时       
        dic.Add(ZhaJHMethodname.OnZJHSeatLookPoker,SeatLookPoker);//看牌的方法
        dic.Add(ZhaJHMethodname.OnZJHSeatInfoGold, OnSeatInfoGold);//加注和跟注更新分数
        dic.Add(ZhaJHMethodname.OnZJHSetSeatInfoOperation, SetSeatInfoOperation);//当玩家处于下注的时候重新连接
        dic.Add(ZhaJHMethodname.OnZJHSeatInfoBet, OnSeatInfoBet);
        dic.Add(ZhaJHMethodname.OnZJHTheCardAnimation, TheCardAnimation);//比牌动画
        dic.Add(ZhaJHMethodname.OnZJHCloseZhuangTai, CloseZhuangTai);
        dic.Add(ZhaJHMethodname.OnZJHOpenPoker, OpenPoker);//开牌
        dic.Add(ZhaJHMethodname.OnZJHShufflingPoker, ShufflingPoker);//搓牌
        dic.Add(ZhaJHMethodname.OnZJHSeatLowScore, SeatLowScore);//底分模式
        dic.Add(ZhaJHMethodname.OnZJHCloseShufflingPoker, CloseShufflingPoker);//搓牌关闭按钮
        dic.Add(ZhaJHMethodname.OnZJHDiscardMethod, DiscardMethod);//弃牌
        return dic;
    }



    protected override void OnAwake()
    {
        base.OnAwake();
        gameObject.SetActive(false);
        settlementScoreTween = SettlementScore.rectTransform.DOMove(SettlementScore.rectTransform.position + new Vector3(0, 80, 0), 0.5f).SetEase(Ease.Linear).SetAutoKill(false).Pause();
    }
    private void OnSeatInfoChanged(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        ENUM_ROOM_STATUS roomStatus = data.GetValue<ENUM_ROOM_STATUS>("RoomStatus");
        bool IsPlayer = data.GetValue<bool>("IsPlayer");
        RoomEntity room = data.GetValue<RoomEntity>("Room");
        RoomEntity CurrentRoom = data.GetValue<RoomEntity>("Room");
        SetSeatInfo(seat, roomStatus);
        //if (seat.pokerList.Count == 0 && seat.Index == m_nSeatIndex) ClosePoker();
        if (roomStatus == ENUM_ROOM_STATUS.IDLE || roomStatus == ENUM_ROOM_STATUS.ROOMDISSOLUTION || roomStatus == ENUM_ROOM_STATUS.SETTLEMENT || roomStatus == ENUM_ROOM_STATUS.READY)
        {
            return;
        }
        SetSeatInfoTwo(seat, roomStatus);
    }
    private void OnSeatInfoGold(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        RoomEntity room = data.GetValue<RoomEntity>("Room");
        ENUM_ROOM_STATUS roomStatus = data.GetValue<ENUM_ROOM_STATUS>("RoomStatus");
        SetSeatInfo(seat, roomStatus);
        SeatChip(seat, roomStatus);
    }
    /// <summary>
    /// 通知下注
    /// </summary>
    public void OnSeatInfoBet(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        RoomEntity room = data.GetValue<RoomEntity>("Room");
        ENUM_ROOM_STATUS roomStatus = data.GetValue<ENUM_ROOM_STATUS>("RoomStatus");
        SetSeatInfo(seat, roomStatus);
    }


    //座位 seatItem 显示
    private void SetSeatInfo(SeatEntity seat, ENUM_ROOM_STATUS roomStatus)
    {
        if (m_nSeatIndex == seat.Index)
        {
            //m_zhuangtai.enabled = false;
            if (seat.PlayerId == 0)
            {
                RoomZhaJHProxy.Instance.DicEvent("Index", seat.Index, ZhaJHMethodname.OnZJHInfoSettlement1);
                this.gameObject.SetActive(false);
            }
            else
            {
                this.gameObject.SetActive(true);
                if (SuperHomeowner != null && seat.pos == 7) SuperHomeowner.gameObject.SetActive(true);
                SettlementScore.gameObject.SetActive(false);
                if (seat.isScore && seat.profit != 0 && seat.seatStatus != ENUM_SEAT_STATUS.READY)
                {
                    if (seat.profit > 0)
                    {
                        if (seat.pos == RoomZhaJHProxy.Instance.PlayerSeat.pos)
                        {
                            AudioEffectManager.Instance.Play("zjh_sheng", Vector3.zero);//播放胜利的音乐
                        }
                        StartCoroutine(ChipMove());
                    }
                    else
                    {
                        if (seat.pos == RoomZhaJHProxy.Instance.PlayerSeat.pos)
                        {
                            AudioEffectManager.Instance.Play("zjh_bai", Vector3.zero);//播放失败的音乐
                        }
                    }

                    StartCoroutine(AnimationScore(seat));
                }
            }
            m_PlayerInfo.SetUI(seat);           
            m_Ready.gameObject.SetActive(seat.seatStatus == ENUM_SEAT_STATUS.READY);//已准备按钮                      
                                                                                    // }
            if (seat.seatStatus == ENUM_SEAT_STATUS.BET && roomStatus != ENUM_ROOM_STATUS.SETTLEMENT)
            {
                long currentTime = TimeUtil.GetTimestampMS();//获取当前时间        
                //int times = (int)Math.Ceiling((seat.systemTime - currentTime + GlobalInit.Instance.TimeDistance) * 0.001f);
                float times = (float)((seat.systemTime - currentTime + GlobalInit.Instance.TimeDistance) * 0.001f);
                if (times > 0)
                {
                    //SetImageTime(times);  
                    isTime = true;
                    stime = times;
                    ztime = times;
                }
            }
            isLowScore = seat.isLowScore;
            if (seat.seatStatus == ENUM_SEAT_STATUS.BET)
            {
                guangshu.SetActive(true);
            }
            else
            {
                guangshu.SetActive(false);
            }
        }
        else
        {
            if (seat.PlayerId != 0)
            {
                guangshu.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 移动筹码
    /// </summary>
    /// <returns></returns>
    IEnumerator ChipMove()
    {
        yield return new WaitForSeconds(0.5f);
        Tween tween;
        for (int i = 0; i < chipMountTran.childCount; i++)
        {
            chipMountTran.GetChild(i).transform.DOMove(chipTran.position, 0.8f);
            if (chipMountTran.GetChild(chipMountTran.childCount - 1))
            {
                tween = chipMountTran.GetChild(i).transform.DOMove(chipTran.position, 0.8f).OnComplete(()=> 
                {
                    for (int j = 0; j < chipMountTran.childCount; j++)
                    {
                        Destroy(chipMountTran.GetChild(j).gameObject);
                    }
                });
            }
           
        }
    }


    /// <summary>
    /// 收益结算时分数的动画
    /// </summary>
    /// <param name="seat"></param>
    private IEnumerator AnimationScore(SeatEntity seat)
    {
        SettlementScore.gameObject.SetActive(true);
        string scoreText = string.Format(seat.profit > 0 ? "+{0}" : "{1}", seat.profit, seat.profit);
        SettlementScore.color = seat.profit > 0 ? Color.green : Color.red;
        SettlementScore.text = scoreText;       
        if (settlementScoreTween != null) settlementScoreTween.Restart();    
        yield return new WaitForSeconds(4f);
        seat.isScore = false;
        SettlementScore.gameObject.SetActive(false);             
    }

    private void SeatLowScore(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        if (seat.Index == m_nSeatIndex)
        {
            isLowScore = seat.isLowScore;
        }
    }




    /// <summary>
    /// 根据下注的Item来对象实例化筹码
    /// </summary>
    /// <param name="seat"></param>
    /// <param name="roomStatus"></param>
    private void SeatChip(SeatEntity seat, ENUM_ROOM_STATUS roomStatus)
    {

        if (m_nSeatIndex == seat.Index)
        {
            Remainder(seat.betPoints);
            if (oneHundred != 0)
            {
                InstantiationChip(oneHundred, 100);
                oneHundred = 0;
            }
            if (Fifty != 0)
            {
                InstantiationChip(Fifty, 50);
                Fifty = 0;
            }
            //if (Forty!=0)
            //{
            //    InstantiationChip(Forty, 40);
            //    Forty = 0;
            //}
            if (Twenty != 0)
            {
                InstantiationChip(Twenty, 20);
                Twenty = 0;
            }
            if (ten != 0)
            {
                InstantiationChip(ten, 10);
                ten = 0;
            }
            if (five != 0)
            {
                InstantiationChip(five, 5);
                five = 0;
            }
            if (two != 0)
            {
                InstantiationChip(two, 2);
                two = 0;
            }
            if (one != 0)
            {
                InstantiationChip(one, 1);
                one = 0;
            }

        }
    }

    /// <summary>
    /// 实例化筹码显示
    /// </summary>
    /// <param name="number"></param>
    /// <param name="score"></param>
    public void InstantiationChip(float number, int score)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject go = Instantiate(ZJHPrefabManager.Instance.LoadChip(score), chipTran.position, Quaternion.identity);
            int randomx = UnityEngine.Random.Range(-230, 230);
            int randomy = UnityEngine.Random.Range(-60, 80);
            Tween magnify1 = go.transform.DOMove(new Vector3(chipMountTran.position.x + randomx, chipMountTran.position.y + randomy, 0), 0.2f);
            go.transform.SetParent(chipMountTran);
            go.transform.localScale = Vector3.one;
            //int randomx = UnityEngine.Random.Range(-5, 5);//canvas 画布模式的改变导致变动
            //int randomy = UnityEngine.Random.Range(-2, 2);
        }
    }

    /// <summary>
    /// 计算各个分数筹码的个数
    /// </summary>
    float ten = 0;
    float five = 0;
    float two = 0;
    float one = 0;
    float Twenty = 0;//二十
    //float Forty = 0;//四十
    float Fifty = 0;//五十
    float oneHundred = 0;//一百
    private float Remainder(float betPoints)
    {
        if (betPoints >= 100)
        {
            float remainderHundred = betPoints % 100;
            oneHundred = (betPoints - remainderHundred) / 100;
            return Remainder(remainderHundred);
        }
        else if (betPoints >= 50 && betPoints < 100)
        {
            float remainderFifty = betPoints % 50;
            Fifty = (betPoints - remainderFifty) / 50;
            return Remainder(remainderFifty);
        }
        //else if (betPoints >= 40 && betPoints < 50)
        //{
        //    float remainderForty = betPoints % 40;
        //    Forty = (betPoints - remainderForty) / 40;
        //    return Remainder(remainderForty);
        //}
        else if (betPoints >= 20 && betPoints < 50)
        {
            float remainderTwenty = betPoints % 20;
            Twenty = (betPoints - remainderTwenty) / 20;
            return Remainder(remainderTwenty);
        }
        else if (betPoints >= 10 && betPoints < 20)
        {
            float remainderTen = betPoints % 10;
            ten = (betPoints - remainderTen) / 10;
            return Remainder(remainderTen);
        }
        else if (betPoints >= 5 && betPoints < 10)
        {
            float remainderFive = betPoints % 5;
            five = (betPoints - remainderFive) / 5;
            return Remainder(remainderFive);
        }
        else if (betPoints >= 2 && betPoints < 5)
        {
            float remaindertwo = betPoints % 2;
            two = (betPoints - remaindertwo) / 2;
            return Remainder(remaindertwo);
        }
        else
        {
            one = betPoints;
            return one;
        }
    }


    /// <summary>
    /// 当玩家处于下注时，重连的显示按钮
    /// </summary>
    /// <param name="data"></param>
    public void SetSeatInfoOperation(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        ENUM_ROOM_STATUS roomStatus = data.GetValue<ENUM_ROOM_STATUS>("RoomStatus");
        bool IsPlayer = data.GetValue<bool>("IsPlayer");
        RoomEntity CurrentRoom = data.GetValue<RoomEntity>("Room");
        if (m_nSeatIndex == seat.Index)
        {
            if (seat.seatStatus == ENUM_SEAT_STATUS.BET && seat.seatToperateStatus != ENUM_SEATOPERATE_STATUS.Discard)
            {
                //if (seat.pos==RoomZhaJHProxy.Instance.PlayerSeat.pos)
                //{
                guangshu.SetActive(true);
                TransferData data1 = new TransferData();
                data1.SetValue("round", CurrentRoom.round);
                data1.SetValue("totalRound", CurrentRoom.totalRound);
                data1.SetValue("Fen", CurrentRoom.roomPour);
                data1.SetValue("Seat", seat);
                ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHBtnShow, data1);
                ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHHidFen, data1);
                if (RoomZhaJHProxy.Instance.btnGDD)
                {
                    ZhaJHGameCtrl.Instance.WithNotes(null);
                }
                //}
            }
        }
    }

    /// <summary>
    /// 断线重连和中途加入实例化牌
    /// </summary>
    /// <param name="seat"></param>
    /// <param name="roomStatus"></param>
    public void SetSeatInfoTwo(SeatEntity seat, ENUM_ROOM_STATUS roomStatus)
    {
        if (m_nSeatIndex == seat.Index)
        {
            if (seat.seatStatus != ENUM_SEAT_STATUS.IDLE && seat.seatStatus != ENUM_SEAT_STATUS.READY)//&& seat.seatStatus != ENUM_SEAT_STATUS.DEAL && seat.seatStatus != ENUM_SEAT_STATUS.DEAL //seat.seatStatus != ENUM_SEAT_STATUS.IDLE &&seat.seatStatus != ENUM_SEAT_STATUS.READY
            {
                string stateName = seat.seatStatus == ENUM_SEAT_STATUS.THECARD ? (seat.Index == 1 || seat.Index == 2 || seat.Index == 6) ? "img_shibaiyou" : "img_shibaizuo" : (seat.Index == 1 || seat.Index == 2 || seat.Index == 3) ? "img_qipaiyou" : "img_qipaizuo";
                string pokerStatus = (seat.Index == 1 || seat.Index == 2 || seat.Index == 6) ? "img_yikanpaiyou" : "img_yikanpaizuo";
                //#### 如果在游戏中，并且看牌了
                if (seat.pokerStatus == ENUM_POKER_STATUS.POSITIVE)
                {
                    //####如果在游戏中，看牌后弃牌了
                    if (seat.seatToperateStatus == ENUM_SEATOPERATE_STATUS.Discard)
                    {
                        //执行实例化看牌后弃牌                        
                        if (seat.pos == RoomZhaJHProxy.Instance.PlayerSeat.pos)
                        {
                            //执行看牌的方法，实例化牌
                            TransferData data = new TransferData();
                            data.SetValue("index", seat.Index);
                            data.SetValue("pokerList", seat.pokerList);
                            data.SetValue("spriteName", "failpoker");
                            // SendNotification(ZhaJHMethodname.LookPoker, data);
                            if (display != null) display.enabled = false;
                            ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHLookPoker, data);
                            m_zhuangtai.enabled = true;
                            m_zhuangtai.sprite = ZJHSpriteManager.Instance.ZJHSprite(stateName);
                        }
                        else
                        {
                            InstantiationPoker(seat, "failpoker");
                            m_zhuangtai.enabled = true;
                            m_zhuangtai.sprite = ZJHSpriteManager.Instance.ZJHSprite(stateName);
                        }
                    }
                    //####如果在游戏中，看牌后未弃牌
                    else
                    {
                        if (seat.pos == RoomZhaJHProxy.Instance.PlayerSeat.pos)
                        {
                            TransferData data = new TransferData();
                            data.SetValue("index", seat.Index);
                            data.SetValue("pokerList", seat.pokerList);
                            data.SetValue("spriteName", "normalpoker");
                            JudgingCardType(seat);
                            ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHLookPoker, data);
                        }
                        else
                        {
                            InstantiationPoker(seat, "normalpoker");
                            m_zhuangtai.enabled = true;
                            m_zhuangtai.sprite = ZJHSpriteManager.Instance.ZJHSprite(pokerStatus);
                        }
                    }
                }
                //#### 如果在游戏中，没有看牌
                else
                {
                    if (seat.seatToperateStatus == ENUM_SEATOPERATE_STATUS.Discard)
                    {
                        if (seat.pos == RoomZhaJHProxy.Instance.PlayerSeat.pos)
                        {
                            InstantiationPoker(seat, "failpoker");
                            m_zhuangtai.enabled = true;
                            m_zhuangtai.sprite = ZJHSpriteManager.Instance.ZJHSprite(stateName);                         
                        }
                        else
                        {
                            InstantiationPoker(seat, "failpoker");
                            m_zhuangtai.enabled = true;
                            m_zhuangtai.sprite = ZJHSpriteManager.Instance.ZJHSprite(stateName);
                        }
                    }
                    else
                    {
                        if (seat.pos == RoomZhaJHProxy.Instance.PlayerSeat.pos)
                        {
                            InstantiationPoker(seat, "normalpoker");
                        }
                        else
                        {
                            InstantiationPoker(seat, "normalpoker");
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 执行看牌的方法，实例化牌
    /// </summary>
    /// <param name="dataLook"></param>
    public void SeatLookPoker(TransferData dataLook)
    {
        SeatEntity seat = dataLook.GetValue<SeatEntity>("Seat");
        if (seat.Index == m_nSeatIndex)
        {
            if (seat.pos == RoomZhaJHProxy.Instance.PlayerSeat.pos)
            {
                //执行看牌的方法，实例化牌
                if (seat.seatToperateStatus == ENUM_SEATOPERATE_STATUS.Discard)
                {
                    TransferData data = new TransferData();
                    data.SetValue("index", seat.Index);
                    data.SetValue("pokerList", seat.pokerList);
                    data.SetValue("spriteName", "failpoker");
                    ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHLookPoker, data);
                }
                else
                {
                    TransferData data = new TransferData();
                    data.SetValue("index", seat.Index);
                    data.SetValue("pokerList", seat.pokerList);
                    if (stime > 10f && RoomZhaJHProxy.Instance.ShufflingPoker == 1 && (RoomZhaJHProxy.Instance.CurrentRoom.roomStatus != ENUM_ROOM_STATUS.IDLE && RoomZhaJHProxy.Instance.CurrentRoom.roomStatus != ENUM_ROOM_STATUS.SETTLEMENT))
                    {
                        data.SetValue("spriteName", "realityPoker");
                        KPMask.SetActive(true);
                        this.gameObject.transform.SetSiblingIndex(this.gameObject.transform.parent.childCount - 1);//把ITem移到最后一位
                        LookShufflingPoker(seat);
                    }
                    else
                    {
                        data.SetValue("spriteName", "normalPoker");
                        JudgingCardType(seat);
                        ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHLookPoker, data);
                    }
                }

            }
            else
            {
                if (RoomZhaJHProxy.Instance.CurrentRoom.roomStatus != ENUM_ROOM_STATUS.IDLE)
                {
                    m_zhuangtai.enabled = true;
                    string stateName = (seat.Index == 1 || seat.Index == 2 || seat.Index == 6) ? "img_yikanpaiyou" : "img_yikanpaizuo";
                    m_zhuangtai.sprite = ZJHSpriteManager.Instance.ZJHSprite(stateName);
                }
            }
        }

    }


    private void JudgingCardType(SeatEntity seat)
    {
        string imageName = string.Empty;
        Debug.Log(seat.zjhCardType + "                        牌型");
        switch (seat.zjhCardType) //判断牌型。显示相对应的图片
        {
            case ZJHCardType.ShunZi:
                imageName = "img_shunzi";
                break;
            case ZJHCardType.WithFlowers:
                imageName = "img_tonghua";
                break;
            case ZJHCardType.WithFlowersShun:
                imageName = "img_tonghuashun";
                break;
            case ZJHCardType.Bomb:
                imageName = "img_zhadan";
                break;
            case ZJHCardType.Sub:
                imageName = "img_duizi";
                break;
        }
        Debug.Log(imageName + "                                         牌型的名字");
        if (display != null && imageName != string.Empty)
        {
            display.enabled = true;
            display.sprite = ZJHSpriteManager.Instance.ZJHSprite(imageName);
        }
    }


    /// <summary>
    /// 实例化牌
    /// </summary>
    /// <param name="index"></param>
    private void InstantiationPoker(SeatEntity seat, string spriteName)
    {
        TransferData data = new TransferData();
        data.SetValue("index", seat.Index);
        data.SetValue("Seat", seat);
        data.SetValue("spriteName", spriteName);
        ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHHairPoker, data);
    }


    /// <summary>
    /// 搓牌
    /// </summary>
    private Vector3 pokerMoun1;
    private Vector3 pokerMoun2;
    private Vector3 pokerMoun0;
    private void LookShufflingPoker(SeatEntity seat)
    {
        if (seat.pokerList.Count != 0)
        {
            if (this.gameObject.activeSelf)
            {
                for (int z = 0; z < pokerMounts.Length; z++)
                {
                    for (int k = 0; k < pokerMounts[z].transform.childCount; k++)
                    {
                        Destroy(pokerMounts[z].transform.GetChild(k).gameObject);
                    }
                }
            }

            m_pokerMount.GetComponent<GridLayoutGroup>().spacing = new Vector2(-140, 0);

            pokerMoun1 = pokerMounts[1].transform.GetChild(0).transform.localPosition;
            pokerMoun2 = pokerMounts[2].transform.GetChild(0).transform.localPosition;
            pokerMoun0 = pokerMounts[0].transform.GetChild(0).transform.localPosition;

            //pokerMounts[1].transform.GetChild(0).transform.localPosition = pokerMounts[0].transform.localPosition;
            //pokerMounts[2].transform.GetChild(0).transform.localPosition = pokerMounts[0].transform.localPosition;
            seat.pokerList = RandomSortList(seat.pokerList);
            storageSeat = seat;
            for (int i = 0; i < seat.pokerList.Count; i++)
            {
                //seat.pokerList.Sort(delegate(Poker a,Poker b) { return (new System.Random()).Next(0, 3); });               
                GameObject go = ZJHPrefabManager.Instance.LoadPoker(seat.pokerList[i], null, "normalpoker");
                go.transform.SetParent(pokerMounts[i].transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }
            StartCoroutine("AutomaticOpenPoker");
        }
    }
    //搓牌打乱牌的顺序
    public List<T> RandomSortList<T>(List<T> ListT)
    {
        System.Random random = new System.Random();
        List<T> newList = new List<T>();
        foreach (T item in ListT)
        {
            newList.Insert(random.Next(newList.Count + 1), item);
        }
        return newList;
    }
    IEnumerator AutomaticOpenPoker()
    {
        yield return new WaitForSeconds(7);
        KPMask.SetActive(false);
        OpenPoker(null);
    }
    //开牌
    private void OpenPoker(TransferData data)
    {
        StopCoroutine("AutomaticOpenPoker");
        KPMask.SetActive(false);
        ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHNoMaskP);
        if (RoomZhaJHProxy.Instance.PlayerSeat.Index == m_nSeatIndex)
        {
            m_pokerMount.GetComponent<GridLayoutGroup>().spacing = new Vector2(40, 0);
        }
    }
    /// <summary>
    /// 搓牌自动开牌
    /// </summary>
    /// <param name="pokerMount"></param>
    /// <returns></returns>
    IEnumerator OpenPokerIE(GameObject pokerMount)
    {
        for (int i = 0; i <= 180;)
        {
            pokerMount.GetComponent<GridLayoutGroup>().spacing = new Vector2(-140 + i, 0);
            i += 50;
            yield return new WaitForSeconds(0.5f);
        }
    }
    //点击搓牌按钮
    private void ShufflingPoker(TransferData data)
    {
        StopCoroutine("AutomaticOpenPoker");
        KPMask.transform.GetChild(0).gameObject.SetActive(false);
        KPMask.transform.GetChild(1).gameObject.SetActive(true);
        if (RoomZhaJHProxy.Instance.PlayerSeat.Index == m_nSeatIndex)
        {
            if (this.gameObject.activeSelf)
            {
                for (int z = 0; z < pokerMounts.Length; z++)
                {
                    for (int k = 0; k < pokerMounts[z].transform.childCount; k++)
                    {
                        Destroy(pokerMounts[z].transform.GetChild(k).gameObject);
                    }
                }
            }

            for (int i = 0; i < storageSeat.pokerList.Count; i++)
            {
                GameObject go = ZJHPrefabManager.Instance.LoadPoker(storageSeat.pokerList[i], null, "realityPoker");
                go.transform.SetParent(pokerMounts[i].transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = new Vector3(2f, 2f, 2f);
                // pokerMounts[i].transform.GetChild(0).gameObject.GetComponent<UIZhaJHDrag>().enabled = true;
                go.GetComponent<UIZhaJHDrag>().enabled = true;

            }
            //for (int i = 0; i < pokerMounts.Length; i++)
            //{
            //    pokerMounts[i].transform.GetChild(0).localScale = new Vector3(3,3,3);                  
            //    pokerMounts[i].transform.GetChild(0).gameObject.GetComponent<UIZhaJHDrag>().enabled = true;              
            //}
            StartCoroutine("ShufflingPoker1");
        }
    }


    IEnumerator ShufflingPoker1()
    {
        yield return new WaitForSeconds(7);
        pokerMounts[0].transform.GetChild(0).localPosition = pokerMoun0;
        pokerMounts[1].transform.GetChild(0).localPosition = pokerMoun1 + new Vector3(200, 0, 0);
        pokerMounts[2].transform.GetChild(0).localPosition = pokerMoun2 + new Vector3(400, 0, 0);

        for (int i = 0; i < pokerMounts.Length; i++)
        {
            pokerMounts[i].transform.GetChild(0).gameObject.GetComponent<UIZhaJHDrag>().enabled = false;
        }
        yield return new WaitForSeconds(1);
        pokerMounts[0].transform.GetChild(0).localPosition = pokerMoun0;
        pokerMounts[1].transform.GetChild(0).localPosition = pokerMoun1;
        pokerMounts[2].transform.GetChild(0).localPosition = pokerMoun2;
        KPMask.SetActive(false);
        KPMask.transform.GetChild(0).gameObject.SetActive(true);
        KPMask.transform.GetChild(1).gameObject.SetActive(false);
        ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHNoMaskP);
        if (this.gameObject.activeSelf)
        {
            for (int z = 0; z < pokerMounts.Length; z++)
            {
                for (int k = 0; k < pokerMounts[z].transform.childCount; k++)
                {
                    Destroy(pokerMounts[z].transform.GetChild(k).gameObject);
                }
            }
        }
        for (int i = 0; i < storageSeat.pokerList.Count; i++)
        {
            GameObject go = ZJHPrefabManager.Instance.LoadPoker(storageSeat.pokerList[i], null, "normalPoker");
            go.transform.SetParent(pokerMounts[i].transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
        //for (int i = 0; i < pokerMounts.Length; i++)
        //{            
        //    pokerMounts[i].transform.GetChild(0).localScale = new Vector3(0.8f, 0.8f, 0.8f);          
        //}
        m_pokerMount.GetComponent<GridLayoutGroup>().spacing = new Vector2(40, 0);
    }

    /// <summary>
    /// 点击关闭按钮
    /// </summary>
    private void CloseShufflingPoker(TransferData data)
    {
        if (RoomZhaJHProxy.Instance.PlayerSeat.Index == m_nSeatIndex)
        {
            StopCoroutine("ShufflingPoker1");
            pokerMounts[0].transform.GetChild(0).localPosition = pokerMoun0;
            pokerMounts[1].transform.GetChild(0).localPosition = pokerMoun1;
            pokerMounts[2].transform.GetChild(0).localPosition = pokerMoun2;
            KPMask.SetActive(false);
            KPMask.transform.GetChild(0).gameObject.SetActive(true);
            KPMask.transform.GetChild(1).gameObject.SetActive(false);
            ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHNoMaskP);
            if (this.gameObject.activeSelf)
            {
                for (int z = 0; z < pokerMounts.Length; z++)
                {
                    for (int k = 0; k < pokerMounts[z].transform.childCount; k++)
                    {
                        Destroy(pokerMounts[z].transform.GetChild(k).gameObject);
                    }
                }
            }
            for (int i = 0; i < storageSeat.pokerList.Count; i++)
            {
                GameObject go = ZJHPrefabManager.Instance.LoadPoker(storageSeat.pokerList[i], null, "normalPoker");
                go.transform.SetParent(pokerMounts[i].transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }
            m_pokerMount.GetComponent<GridLayoutGroup>().spacing = new Vector2(40, 0);
        }
    }



    /// <summary>
    /// 倒计时
    /// </summary>
    Color color = new Color(255, 255, 255, 255);
    void Update()
    {
        if (isTime)
        {
            m_ImageTime.gameObject.SetActive(true);
            stime -= Time.deltaTime;
            m_ImageTime.fillAmount = stime / ztime;
            if (stime <= 0)
            {
                isTime = false;
                m_ImageTime.gameObject.SetActive(false);
            }
        }
        if (!isTime)
        {
            stime = 0;
            m_ImageTime.gameObject.SetActive(false);
        }

        //闪烁
        if (isLowScore)
        {
            Notice2.gameObject.SetActive(true);
            Notice1.gameObject.SetActive(true);
            if (color.a >= 254f)
            {
                color.a = 0f;
                Tweener noticeTween = Notice1.DOColor(color, 0.5f);
                noticeTween.OnComplete(() =>
                {
                    color.a = 255f;
                    noticeTween = Notice1.DOColor(color, 0.5f);
                });
            }
        }
        else
        {
            Notice2.gameObject.SetActive(false);
            Notice1.gameObject.SetActive(false);
        }

    }


    ///// <summary>
    ///// 时间倒计时
    ///// </summary>
    ///// <param name="data"></param>
    //private void SetImageTime(float times)
    //{
    //    if (m_ImageTime.gameObject.activeSelf) return;
    //    StartCoroutine("CountDown", times);
    //}
    //IEnumerator CountDown(float times)
    //{               
    //    m_ImageTime.gameObject.SetActive(true);       
    //    for (float i = times; i >= 0; i-=0.01f)
    //    {
    //        float a = i >= 0 ? i : 0;
    //        m_ImageTime.fillAmount = (float)a / times;
    //        yield return new WaitForSeconds(0.01f);
    //        if (a == 0)
    //        {
    //            m_ImageTime.gameObject.SetActive(false);
    //        }

    //    }
    //}
    /// <summary>
    /// 亮牌的时候清除牌的状态
    /// </summary>
    private void CloseZhuangTai(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        if (m_nSeatIndex == seat.Index)
        {
            m_zhuangtai.enabled = false;
        }

    }
    /// <summary>
    /// 关闭倒计时
    /// </summary>
    private void CloseCountTime(TransferData data)
    {
        isTime = false;

        //StopCoroutine("CountDown");
        //m_ImageTime.gameObject.SetActive(false);
    }

    /// <summary>
    /// 比牌动画
    /// </summary>
    /// <param name="type"></param>
    private Vector3 a;
    private Vector3 b;
    private void TheCardAnimation(TransferData data)
    {
        VSMask.SetActive(true);
        VSMask.GetComponent<UIZhaJHDoTweenVS>().DoTweenPlay();
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        SeatEntity discardSeat = data.GetValue<SeatEntity>("DiscardSeat");
        if (m_nSeatIndex == seat.Index)
        {
            a = m_pokerMount.transform.position;
            b = m_pokerMount.transform.localScale;
            this.gameObject.transform.SetSiblingIndex(this.gameObject.transform.parent.childCount - 1);//如果是当前 Item 就把对象移到父物体的最后位置
            this.m_zhuangtai.enabled = false;
            ///把比牌的两个人的座位存到 List 中，
            /// 判断当前这个座位是不是 List 中的第一位，如果是第一位将牌移到A点
            /// 否则移动到B点
            if (seat.pos == RoomZhaJHProxy.Instance.posList[0])
            {
                ///VsTran 为 A点
                if (seat.Index == 0)
                {
                    m_pokerMount.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
                    m_pokerMount.GetComponent<GridLayoutGroup>().spacing = new Vector2(-50, 0);
                    Tween magnify1 = m_pokerMount.transform.DOMove(VsTran.transform.position, 0.8f).OnComplete(() =>
                    {
                        if (display != null && display.isActiveAndEnabled)
                        {
                            display.enabled = false;
                            isDisplay = true;
                        }
                        StartCoroutine(VSMaskIEnumer(seat, discardSeat));
                    }); 
                }
                else
                {
                    m_pokerMount.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
                    Tween magnify1 = m_pokerMount.transform.DOMove(VsTran.transform.position, 0.8f).OnComplete(() =>
                    {
                        if (display != null && display.isActiveAndEnabled)
                        {
                            display.enabled = false;
                            isDisplay = true;
                        }
                        StartCoroutine(VSMaskIEnumer(seat, discardSeat));
                    }); 
                }
            }
            else
            {
                if (seat.Index == 0)
                {
                    m_pokerMount.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
                    m_pokerMount.GetComponent<GridLayoutGroup>().spacing = new Vector2(-50, 0);
                    Tween magnify1 = m_pokerMount.transform.DOMove(VsTran1.transform.position, 0.8f).OnComplete(() =>
                    {
                        if (display != null && display.isActiveAndEnabled)
                        {
                            display.enabled = false;
                            isDisplay = true;
                        }
                        StartCoroutine(VSMaskIEnumer(seat, discardSeat));
                    }); 
                }
                else
                {
                    ///VsTran1 为 B点   
                    m_pokerMount.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
                    Tweener magnify1 = m_pokerMount.transform.DOMove(VsTran1.transform.position, 0.8f).OnComplete(()=> 
                    {
                        if (display != null && display.isActiveAndEnabled)
                        {
                            display.enabled = false;
                            isDisplay = true;
                        }
                        StartCoroutine(VSMaskIEnumer(seat, discardSeat));
                    });
                }
            }          
        }
    }

    IEnumerator VSMaskIEnumer(SeatEntity seat, SeatEntity discardSeat)
    {
        yield return new WaitForSeconds(2f);
        
        RoomZhaJHProxy.Instance.LosePokerProxy(discardSeat.pos);
        Debug.Log(discardSeat.pos + "                          弃牌222222");
        yield return new WaitForSeconds(0.2f);
        Tween magnify = m_pokerMount.transform.DOMove(a, 0.8f);
        m_pokerMount.transform.DOScale(b, 0.2f);
        if (isDisplay)
        {
            display.enabled = true;
            isDisplay = false;
        }
        if (seat.Index == 0)
        {
            m_pokerMount.GetComponent<GridLayoutGroup>().spacing = new Vector2(40, 0);
        }
        yield return new WaitForSeconds(0.3f);
        VSMask.GetComponent<UIZhaJHDoTweenVS>().DoTweenEnde();
        VSMask.SetActive(false);
        if (!this.m_zhuangtai.isActiveAndEnabled && RoomZhaJHProxy.Instance.CurrentRoom.roomStatus != ENUM_ROOM_STATUS.IDLE && seat.pokerStatus == ENUM_POKER_STATUS.POSITIVE && seat.pos != RoomZhaJHProxy.Instance.PlayerSeat.pos)
        {
            this.m_zhuangtai.enabled = true;
        }
        yield return new WaitForSeconds(0.5f);
        RoomZhaJHProxy.Instance.DicEvent("isbool", false, ZhaJHMethodname.OnZJHNoMaskP);//重新把玩家 Item 排序

    }


    /// <summary>
    /// 弃牌执行的方法
    /// </summary>
    private void DiscardMethod(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        if (seat.Index == m_nSeatIndex)
        {
            string stateName = seat.seatStatus == ENUM_SEAT_STATUS.THECARD ? (seat.Index == 1 || seat.Index == 2 || seat.Index == 6) ? "img_shibaiyou" : "img_shibaizuo" : (seat.Index == 1 || seat.Index == 2 || seat.Index == 3) ? "img_qipaiyou" : "img_qipaizuo";
            if (seat.pokerStatus == ENUM_POKER_STATUS.POSITIVE)
            {
                if (seat.seatToperateStatus == ENUM_SEATOPERATE_STATUS.Discard)
                {
                    //执行实例化看牌后弃牌                        
                    if (seat.pos == RoomZhaJHProxy.Instance.PlayerSeat.pos)
                    {
                        //执行看牌的方法，实例化牌
                        TransferData data1 = new TransferData();
                        data1.SetValue("index", seat.Index);
                        data1.SetValue("pokerList", seat.pokerList);
                        data1.SetValue("spriteName", "failpoker");
                        ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHLookPoker, data1);
                        m_zhuangtai.enabled = true;
                        m_zhuangtai.sprite = ZJHSpriteManager.Instance.ZJHSprite(stateName);
                    }
                    else
                    {
                        InstantiationPoker(seat, "failpoker");
                        m_zhuangtai.enabled = true;
                        m_zhuangtai.sprite = ZJHSpriteManager.Instance.ZJHSprite(stateName);
                    }
                }              
            }
            else
            {
                if (seat.seatToperateStatus == ENUM_SEATOPERATE_STATUS.Discard)
                {
                    if (seat.pos == RoomZhaJHProxy.Instance.PlayerSeat.pos)
                    {
                        InstantiationPoker(seat, "failpoker");
                        m_zhuangtai.enabled = true;
                        m_zhuangtai.sprite = ZJHSpriteManager.Instance.ZJHSprite(stateName);
                    }
                    else
                    {
                        InstantiationPoker(seat, "failpoker");
                        m_zhuangtai.enabled = true;
                        m_zhuangtai.sprite = ZJHSpriteManager.Instance.ZJHSprite(stateName);
                    }
                }
            }
        }
    }

    private void ClosePoker()
    {
        for (int i = 0; i < pokerMounts.Length; i++)
        {
            for (int j = 0; j < pokerMounts[i].transform.childCount; j++)
            {
                Destroy(pokerMounts[i].transform.GetChild(j).gameObject);
            }
        }
    }
}
