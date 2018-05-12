//===================================================
//Author      : CZH
//CreateTime  ：9/9/2017 12:24:11 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuPaiJiu;
using UnityEngine.UI;
using proto.gp;
using DG.Tweening;

/// <summary>
/// 组合的界面
/// </summary>
public class CombinationPokerView : UIViewBase
{
    [SerializeField]
    private GameObject btnObj;//按钮显示
    [SerializeField]
    private GameObject opneBtnObj;//开牌按钮显示
    [SerializeField]
    private GameObject CombinationObj;//控制显示
    [SerializeField]
    private GameObject GroupPokerObj;//控制显示
    [SerializeField]
    private Transform[] pokerPointArry; //牌的挂载点
    [SerializeField]
    private Transform[] groupPokerPointArry; //牌的挂载点
    [SerializeField]
    private Image m_pokerType1;//显示扑克类型图片
    [SerializeField]
    private Image m_pokerType2;//显示扑克类型图片
    [SerializeField]
    private Image m_pokerType3;//显示扑克类型图片
    [SerializeField]
    private Image m_pokerType4;//显示扑克类型图片

    [SerializeField]
    private Text m_TextCountDown;//时间
    [SerializeField]
    private Transform roomPokerPoint;//房间 Poker 挂载点
    [SerializeField]
    private GameObject m_rubPokerParent;//搓牌父物体
    [SerializeField]
    private Transform[] m_rubPoker;//搓牌，牌的挂载点

    private SeatEntity currSeat;
    private EnumPlay roomPlay;//房间模式

    private GuPaiJiu.PokerType pokerType1;//定义扑克类型枚举
    private GuPaiJiu.PokerType pokerType2;//定义扑克类型枚举

    private bool isOpenTime = false;//是否开启倒计时
    private bool isStartPokerTime = false;//是否自动开牌
    private bool isCuoPai = false;//是否搓牌
    private float m_CurrentTime;//倒计时时间
    private float m_StartPokerTime;//搓牌倒计时

    private Vector3 m_rubPokerV0;
    private Vector3 m_rubPokerV1;

    private bool isSend = true;//是否向服务器发自动配牌的消息

    private List<int> pokerList = new List<int>(); //向服务器发送牌的组合的集合  

    //private Dictionary<string, GameObject> dicImage = new Dictionary<string, GameObject>();

    private List<int> IndexList=new List<int>();//提示的集合

    private Dictionary<int, GameObject> IndexObjDic = new Dictionary<int, GameObject>();

    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(ConstantGuPaiJiu.GroupValidPoker, GroupValidPoker);
        dic.Add("OnSeatInfoChanged", OnSeatInfoChanged);
        dic.Add(ConstantGuPaiJiu.GroupEnd, GroupEnd);//组合牌结束
        dic.Add(ConstantGuPaiJiu.isSend, isSendMethod);//是否向服务器发自动发牌的消息、
        dic.Add(ConstantGuPaiJiu.OnGuPaiJiuPromptPoker, OnGuPaiJiuPromptPoker);//提示牌
        return dic;
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        m_rubPokerParent.SetActive(false);
        GroupPokerObj.SetActive(false);
        btnObj.SetActive(false);
        m_rubPokerV0 = m_rubPoker[0].transform.position;
        m_rubPokerV1 = m_rubPoker[1].transform.position;
    }


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case ConstantGuPaiJiu.OnGuPaibtnGroupComplete://点击组合完成
#if IS_BAODINGQIPAI
                OnClikComplete(ClientName.BaoDingQiPai);
#else
                OnClikComplete(ClientName.No);
#endif
                break;
            case ConstantGuPaiJiu.OnGuPaibtnGroupComplete1://点击组合完成
                OnClikComplete(ClientName.No);
                break;
            case ConstantGuPaiJiu.OnGuPaibtnAutomaticPoker://自动配牌
                OnClikAutomaticPoker();
                break;
            case ConstantGuPaiJiu.OnGuPaibtnBeginPoker://搓牌开牌按钮
                GroupValidPoker(currSeat, null);
                break;
            case ConstantGuPaiJiu.OnGuPaiJiubtnPrompt://提示
                SendPrompt();
                break;
            case "OnGuPaibtnOpenPoker":
               if(roomPlay==EnumPlay.BigPaiJiu) OpenPoker(currSeat,null);
               if(roomPlay == EnumPlay.SmallPaiJiu)ClientCuoPai();
                break;
        }
    }

    private void OnSeatInfoChanged(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        RoomEntity room = data.GetValue<RoomEntity>("Room");
        roomPlay = room.roomPlay;        
        bool isPlayer = data.GetValue<bool>("IsPlayer");
        if (isPlayer)
        {
            if (room.roomStatus==ROOM_STATUS.GROUPPOKER)
            {
                if (seat.seatStatus == SEAT_STATUS.GROUP)
                {
#if IS_BAODINGQIPAI
                    GroupPokerObj.SetActive(true);//显示组合界面     
                    InstantiationPoker(seat, groupPokerPointArry,true);


#else
                    CombinationObj.SetActive(true);//显示组合界面                 
                    OpenPoker(seat, room);
                    OpenInvertedTime(seat);
                    SetGroupRoomPoker(room);//加载已经发过的牌//加载已发过的牌
                   
#endif
                }
            }
            if (room.roomStatus== ROOM_STATUS.CUOPAI)
            {
                if (seat.isCuoPai==2)
                {
                    seat.groupTime = room.roomUnixtime;
                    RubPoker(seat,room);
                }
            }       
        }
    }


    /// <summary>
    /// 提示
    /// </summary>
    private void OnGuPaiJiuPromptPoker(TransferData data)
    {
        IndexList = data.GetValue<List<int>>("IndexList");        
        PromptPokerMove();             
    }

    /// <summary>
    /// 提示
    /// </summary>
    private void PromptPokerMove()
    {
        if (pokerList.Count != 0) pokerList.Clear();
        for (int i = 0; i < IndexList.Count; i++)
        {
#if IS_BAODINGQIPAI
            IndexObjDic[IndexList[i]].SetParent(groupPokerPointArry[i]);
            IndexObjDic[IndexList[i]].transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
#else
            IndexObjDic[IndexList[i]].SetParent(pokerPointArry[i]);
            IndexObjDic[IndexList[i]].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
#endif

            pokerList.Add(IndexObjDic[IndexList[i]].name.ToInt());
            IndexObjDic[IndexList[i]].transform.localPosition = Vector3.zero;
            
        }
#if IS_BAODINGQIPAI
        ConfirmPoker(pokerList,true);//判断牌型
#else
        ConfirmPoker(pokerList);//判断牌型
#endif

    }

    /// <summary>
    /// 向服务端发送提示消息号
    /// </summary>
    private void SendPrompt()
    {
        if (IndexList.Count==0)
        {
            GuPaiJiuGameCtrl.Instance.ClientSendPromptPoker();
        }
        else
        {
            PromptPokerMove();
        }
    }

    /// <summary>
    /// 点击开牌按钮
    /// </summary>
    private void OpenPoker(SeatEntity seat, RoomEntity room)
    {
        btnObj.SetActive(true);
        opneBtnObj.SetActive(false);
        m_rubPoker[0].transform.position = m_rubPokerV0;
        m_rubPoker[1].transform.position = m_rubPokerV1;
        m_rubPoker[0].transform.SetSiblingIndex(0);
        m_rubPoker[1].transform.SetSiblingIndex(1);
        GroupValidPoker(seat, null);
    }

   


    /// <summary>
    /// 开始组合牌
    /// </summary>
    /// <param name="data"></param>
    private void GroupValidPoker(TransferData data)
    {       
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        RoomEntity room = data.GetValue<RoomEntity>("Room");
        currSeat = seat;    
        Debug.Log("··································执行搓牌方法显示界面");
        RubPoker(seat,room);
    }

    private void GroupValidPoker(SeatEntity seat, RoomEntity room)
    {        
        if (pokerList.Count != 0) pokerList.Clear();
        isStartPokerTime = false;
        m_StartPokerTime = 0;
        m_rubPokerParent.SetActive(false);
        InstantiationPoker(seat, pokerPointArry);//实例化牌
        ConfirmPoker(pokerList);//判断牌型
    }


    /// <summary>
    /// 搓牌界面
    /// </summary>
    /// <param name="seat"></param>
    /// <param name="room"></param>
    private void RubPoker(SeatEntity seat, RoomEntity room)
    {
        CombinationObj.SetActive(true);//显示组合界面
        m_rubPokerParent.SetActive(true);
        btnObj.SetActive(false);
        opneBtnObj.SetActive(true);
        m_pokerType1.gameObject.SetActive(false);
        m_pokerType2.gameObject.SetActive(false);
        ClearParent();//清空牌的挂载点
        if (pokerList.Count != 0) pokerList.Clear();//清除牌型提示集合
        if (IndexList.Count != 0) IndexList.Clear();//清除提示牌集合
        if (IndexObjDic.Count != 0)IndexObjDic.Clear();//清除提示牌集合
        if (roomPlay == EnumPlay.BigPaiJiu) InstantiationBigRubPoker(seat,room);       
        if (roomPlay == EnumPlay.SmallPaiJiu) InstantiationSmallRubPoker(seat, room);
        SetGroupRoomPoker(room);//加载已经发过的牌
        OpenInvertedTime(seat);//开启倒计时
    }

#region 大牌九实例化搓牌
    /// <summary>
    /// 实例化搓牌的牌
    /// </summary>
    private void InstantiationBigRubPoker(SeatEntity seat, RoomEntity room)
    {
        Debug.Log("··································大牌九开始搓牌");
        isStartPokerTime = true;
        m_StartPokerTime = 0;
        m_rubPoker[0].gameObject.GetComponent<RectTransform>().sizeDelta.Set(358,389);
        m_rubPoker[1].gameObject.GetComponent<RectTransform>().sizeDelta.Set(358 , 389);        
        for (int i = 0; i < seat.pokerList.Count; i++)
        {
            if (i < 2)
            {
                LoadPoker(seat, seat.pokerList[i].Type, m_rubPoker[0]);
            }
            else
            {
                LoadPoker(seat, 0, m_rubPoker[1]);
            }
        }
        m_rubPoker[1].gameObject.GetComponent<GuPaiJiuRubCtrl>().enabled = true;
        m_rubPoker[1].gameObject.GetComponent<GuPaiJiuRubCtrl>().onComplete = (Vector3 V3) =>
        {
            m_rubPoker[1].transform.SetSiblingIndex(0);
            m_rubPoker[1].gameObject.GetComponent<GuPaiJiuRubCtrl>().enabled = false;
            m_rubPoker[1].transform.DOLocalMove(V3, 0.4f).OnComplete(() =>
            {
                for (int i = 2; i < seat.pokerList.Count; i++)
                {
                    Destroy(m_rubPoker[1].transform.GetChild(i - 2).gameObject);
                    LoadPoker(seat, seat.pokerList[i].Type, m_rubPoker[1]);
                }
                m_rubPoker[0].gameObject.GetComponent<GuPaiJiuRubCtrl>().enabled = true;
            });                    
            m_rubPoker[0].gameObject.GetComponent<GuPaiJiuRubCtrl>().onComplete = (Vector3 V31) =>
            {
                m_rubPoker[0].transform.SetSiblingIndex(0);
                m_rubPoker[0].gameObject.GetComponent<GuPaiJiuRubCtrl>().enabled = false;
                m_rubPoker[0].transform.DOLocalMove(V31, 0);               
                Debug.Log("````````````````````````````````````````````````````搓牌结束，加载配牌界面");
                m_rubPoker[1].gameObject.GetComponent<GuPaiJiuRubCtrl>().onComplete = null;
                m_rubPoker[0].gameObject.GetComponent<GuPaiJiuRubCtrl>().onComplete = null;
                OpenPoker(seat, room);
            };
        };
    }
#endregion


#region 小牌九搓牌
    private void InstantiationSmallRubPoker(SeatEntity seat, RoomEntity room)
    {
        Debug.Log("··································小牌九开始搓牌");
        isCuoPai = true;
        m_StartPokerTime = 0;
        m_rubPoker[0].gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(358 * 0.5f, 389);//.Set(368 * 0.5f, 389);
        m_rubPoker[1].gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(358 * 0.5f, 389);
        Debug.Log(m_rubPoker[0].gameObject.GetComponent<RectTransform>().sizeDelta.x+"                          宽度");
        Debug.Log(m_rubPoker[1].gameObject.GetComponent<RectTransform>().sizeDelta.y + "                          高度");
        for (int i = 0; i < 2; i++)
        {
            if (i < 1)
            {
                LoadPoker(seat, seat.pokerList[i].Type, m_rubPoker[0]);
            }
            else
            {
                LoadPoker(seat, 0, m_rubPoker[1]);
            }
        }
        m_rubPoker[1].gameObject.GetComponent<GuPaiJiuRubCtrl>().enabled = true;
        m_rubPoker[1].gameObject.GetComponent<GuPaiJiuRubCtrl>().onComplete = (Vector3 V3) =>
        {
            m_rubPoker[1].transform.SetSiblingIndex(0);
            m_rubPoker[1].gameObject.GetComponent<GuPaiJiuRubCtrl>().enabled = false;
            m_rubPoker[1].transform.DOLocalMove(V3, 0.4f).OnComplete(() =>
            {

                for (int i = 1; i < seat.pokerList.Count; i++)
                {
                    Destroy(m_rubPoker[1].transform.GetChild(i - 1).gameObject);
                    LoadPoker(seat, seat.pokerList[i].Type, m_rubPoker[1]);
                }
                m_rubPoker[0].gameObject.GetComponent<GuPaiJiuRubCtrl>().enabled = true;                                                                
            });
            m_rubPoker[0].gameObject.GetComponent<GuPaiJiuRubCtrl>().onComplete = (Vector3 V31) =>
            {                
                m_rubPoker[0].gameObject.GetComponent<GuPaiJiuRubCtrl>().enabled = false;
                m_rubPoker[0].transform.DOLocalMove(V31, 0).OnComplete(()=> 
                {
                    m_rubPoker[1].gameObject.GetComponent<GuPaiJiuRubCtrl>().onComplete = null;
                    m_rubPoker[0].gameObject.GetComponent<GuPaiJiuRubCtrl>().onComplete = null;
                    ClientCuoPai();
                });               
            };
        };
    }
#endregion

    /// <summary>
    /// 发送搓牌结束
    /// </summary>
    private void ClientCuoPai()
    {        
        isCuoPai = false;
        m_rubPoker[0].transform.SetSiblingIndex(0);
        m_rubPoker[1].transform.SetSiblingIndex(1);
        GuPaiJiuGameCtrl.Instance.ClientSendCuoPai();
    }


    private void LoadPoker(SeatEntity seat,int Pokertype,Transform tran)
    {
        GuPaiJiuPrefabManager.Instance.LoadPoker(seat.Index, Pokertype, (GameObject go) =>
        {
            go.transform.SetParent(tran);
            go.transform.localPosition = Vector3.zero;
            //go.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            go.transform.localScale = Vector3.one;
            go.name = Pokertype.ToString();
        });
    }


    /// <summary>
    /// 实例化组合实牌
    /// </summary>
    /// <param name="seat"></param>
    private void InstantiationPoker(SeatEntity seat,Transform[] pokerPointArry,bool IsBool=false)
    {      
        for (int i = 0; i < seat.pokerList.Count; i++)
        {                              
            pokerList.Add(seat.pokerList[i].Type);            
            GuPaiJiuPrefabManager.Instance.LoadPoker(seat.Index, seat.pokerList[i].Type, (GameObject go) =>
            {
                go.transform.SetParent(pokerPointArry[i]);
                go.transform.localPosition = Vector3.zero;
                //go.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                go.transform.localScale = Vector3.one;
                go.name = seat.pokerList[i].Type.ToString();
                go.transform.tag = "poker";
                go.GetComponent<GuPaiJiuCtrl>().enabled = true;
                if(IsBool) go.GetComponent<GuPaiJiuCtrl>().isBool = true;
                go.GetComponent<GuPaiJiuCtrl>().onComplete = ConfirmPoker;
                IndexObjDic.Add(seat.pokerList[i].Index, go);
            });
        }
    }

 
    /// <summary>
    /// 组合牌牌移动结束回调
    /// </summary>
    private void ConfirmPoker(bool isBool)
    {
        if (pokerList.Count != 0) pokerList.Clear();
        if (!isBool)
        {
            for (int i = 0; i < pokerPointArry.Length; i++)
            {
                pokerList.Add(pokerPointArry[i].GetChild(0).gameObject.name.ToInt());
            }
        }
        else
        {
            for (int i = 0; i < groupPokerPointArry.Length; i++)
            {
                pokerList.Add(groupPokerPointArry[i].GetChild(0).gameObject.name.ToInt());
            }
        }

        ConfirmPoker(pokerList,isBool);
    }

    /// <summary>
    /// 判断牌型
    /// </summary>
    /// <param name="pokerList"></param>
    private void ConfirmPoker(List<int> pokerList,bool isBool=false)
    {
        //判断牌型
        LookupPokerType.Instance.GetDicPokerType(pokerList, out pokerType1, out pokerType2);    
        //加载牌型图片并赋值
        if (!isBool)
        {
            if (pokerType1 != GuPaiJiu.PokerType.Kong) m_pokerType1.gameObject.SetActive(true);
            if (pokerType2 != GuPaiJiu.PokerType.Kong) m_pokerType2.gameObject.SetActive(true);
            m_pokerType1.sprite = GuPaiJiuPrefabManager.Instance.LoadSprite(pokerType1.ToString());
            m_pokerType2.sprite = GuPaiJiuPrefabManager.Instance.LoadSprite(pokerType2.ToString());
        }
        else
        {
            if (pokerType1 != GuPaiJiu.PokerType.Kong) m_pokerType3.gameObject.SetActive(true);
            if (pokerType2 != GuPaiJiu.PokerType.Kong) m_pokerType4.gameObject.SetActive(true);
            m_pokerType3.sprite = GuPaiJiuPrefabManager.Instance.LoadSprite(pokerType1.ToString());
            m_pokerType4.sprite = GuPaiJiuPrefabManager.Instance.LoadSprite(pokerType2.ToString());
        }
       
    }


    /// <summary>
    /// 组排结束后点击完成执行的方法
    /// </summary>
    private void OnClikComplete(ClientName clientName)
    {
        if (clientName == ClientName.BaoDingQiPai)
        {
            TransferData data = new TransferData();
            data.SetValue("Index", 0);          
            ModelDispatcher.Instance.Dispatch(ConstantGuPaiJiu.OnSeatCtrlGroupPoker,data);
            ClikComplete();
        }
        else
        {
            for (int i = 0; i < pokerList.Count; i++)
            {
                Debug.Log(pokerList[i] + "                                              组合完成发给服务器的牌" + i);
            }
            SendNotification(ConstantGuPaiJiu.GuPaiJiuClientSendGroupPoker, pokerList);
            isOpenTime = false;
        }       
    }


    //搓牌点击完成
    private void ClikComplete()
    {
        ClearParentPoker();
        GroupPokerObj.SetActive(true);
        for (int i = 0; i < pokerPointArry.Length; i++)
        {
            GameObject go = pokerPointArry[i].GetChild(0).gameObject;
            go.SetParent(groupPokerPointArry[i]);
            go.gameObject.GetComponent<GuPaiJiuCtrl>().isBool = true;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        }       
        CombinationObj.SetActive(false);
        ConfirmPoker(pokerList,true);
    }


    /// <summary>
    /// 组合牌结束，关闭组合界面
    /// </summary>
    /// <param name="data"></param>
    private void GroupEnd(TransferData data)
    {
        btnObj.SetActive(false);
        opneBtnObj.SetActive(true);
        GroupPokerObj.SetActive(false);
        CombinationObj.SetActive(false);
    }
    /// <summary>
    /// 加载已经发出过的牌（显示在组合牌的界面）
    /// </summary>
    /// <param name="room"></param>
    private void SetGroupRoomPoker(RoomEntity room)
    {         
       // if (room.dealSecond == 1) return;       
        for (int i = 0; i < room.roomPokerList.Count; i++)
        {
            GuPaiJiuPrefabManager.Instance.LoadPoker(0, room.roomPokerList[i].Type, (GameObject go) =>
            {
                go.transform.SetParent(roomPokerPoint);
                go.GetComponent<Image>().raycastTarget = false;
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                go.name = room.roomPokerList[i].Type.ToString();
            });
        }
    }


    /// <summary>
    /// 组合牌的时候判断挂载点的子物体是不是为空
    /// 如果不为空，则清空
    /// </summary>
    /// 
    private void ClearParentPoker()
    {
        for (int i = 0; i < groupPokerPointArry.Length; i++)
        {
            if (groupPokerPointArry[i].childCount != 0)
            {
                for (int j = 0; j < groupPokerPointArry[i].childCount; j++)
                {
                    Destroy(groupPokerPointArry[i].GetChild(j).gameObject);
                }
            }
        }
    }


    private void ClearParent()
    {
        for (int i = 0; i < pokerPointArry.Length; i++)
        {
            if (pokerPointArry[i].childCount!=0)
            {
                for (int j = 0; j < pokerPointArry[i].childCount; j++)
                {
                    Destroy(pokerPointArry[i].GetChild(j).gameObject);
                }
            }
        }
        if (roomPokerPoint.childCount != 0)
        {
            for (int i = 0; i < roomPokerPoint.childCount; i++)
            {
                Destroy(roomPokerPoint.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < m_rubPoker.Length; i++)
        {
            if (m_rubPoker[i].childCount!=0)
            {
                for (int j = 0; j < m_rubPoker[i].childCount; j++)
                {
                    Destroy(m_rubPoker[i].GetChild(j).gameObject);
                }
            }
        }
    }

    /// <summary>
    /// 开启倒计时
    /// </summary>
    private void OpenInvertedTime(SeatEntity seat)
    {            
        long currentTime = TimeUtil.GetTimestampMS();//获取当前时间                                                          
        int countTime = (int)(seat.groupTime + GlobalInit.Instance.TimeDistance - TimeUtil.GetTimestampMS());
        Debug.Log(seat.groupTime+"                              服务器时间");
        Debug.Log(TimeUtil.GetTimestampMS()+"                      客户端时间");
        Debug.Log(GlobalInit.Instance.TimeDistance+"                         服务器，客户端时间差");
        int s = Mathf.RoundToInt(countTime / 1000f)-2;
        Debug.Log(s+"                             搓牌时间");
        if (s > 0)
        {
            isOpenTime = true;
            SetTime(s);           
        }

    }

    public void SetTime(int second)
    {      
        m_CurrentTime = second;
        m_StartPokerTime = 0;
    }

    private void isSendMethod(TransferData data)
    {
        bool isSend = data.GetValue<bool>("isSend");
        this.isSend = isSend;
    }


    void Update()
    {
        if (isOpenTime)
        {
            m_CurrentTime -= Time.deltaTime;
            m_StartPokerTime += Time.deltaTime;
            if (isStartPokerTime)
            {
                if (m_StartPokerTime >= 15)
                {
                    OpenPoker(currSeat,null);
                    isStartPokerTime = false;
                    m_StartPokerTime = 0;
                    Debug.Log("··································搓牌自动开牌");
                }
            }
           
            if (m_CurrentTime <= 0)
            {
                isOpenTime = false;
                m_CurrentTime = 0;
                m_StartPokerTime = 0;
                if (isSend && roomPlay == EnumPlay.BigPaiJiu) OnClikAutomaticPoker();
                if (isCuoPai && roomPlay == EnumPlay.SmallPaiJiu) ClientCuoPai();
            }
            SetTimeCount((int)m_CurrentTime);           
        }
    }
    private void SetTimeCount(int second)
    {
        if (m_TextCountDown != null)
        {
            m_TextCountDown.SafeSetText(second.ToString("0"));
        }
    }

    /// <summary>
    /// 自动配牌
    /// </summary>
    private void OnClikAutomaticPoker()
    {
        SendNotification(ConstantGuPaiJiu.GuPaiJiuClisentSendAutoGroupPoker);    
    }


}
