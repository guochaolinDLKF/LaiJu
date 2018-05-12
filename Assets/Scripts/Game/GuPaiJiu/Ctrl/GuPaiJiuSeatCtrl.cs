//===================================================
//Author      : DRB
//CreateTime  ：9/7/2017 1:40:41 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuPaiJiu;
using proto.gp;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class GuPaiJiuSeatCtrl : MonoBehaviour
{
    [SerializeField]
    private int m_nSeatIndex;
    [SerializeField]
    private Transform ShuffleTran;//洗牌挂载点
    [SerializeField]
    private Grid3D dealTran;//牌墙挂载点
    [SerializeField]
    private Transform[] m_HandContainer;//手牌挂载点  
    [SerializeField]
    private Image m_pokerType1;//显示扑克类型图片
    [SerializeField]
    private Image m_pokerType2;//显示扑克类型图片

    private GuPaiJiu.PokerType pokerType1;//定义扑克牌型枚举
    private GuPaiJiu.PokerType pokerType2;//定义扑克牌型枚举

    private bool isShowType=false;
    [HideInInspector]
    public  List<GameObject> pokerWall = new List<GameObject>();//牌墙
    //private  Action<List<GameObject>> onComplete;//获得牌墙的方法

    private List<int> pokerList = new List<int>(); //判断牌型的集合

    private Queue<UIAnimation> aniQue = new Queue<UIAnimation>();//洗牌动画队列
   
    private List<string> MusicList = new List<string>();
  
    //获取Index
    public int SeatIndex
    {
        get { return m_nSeatIndex; }
    }

    //获取牌墙挂载点
    public Grid3D DealTran
    {
        get
        {
            return dealTran;
        }      
    }

    //获取手牌挂载点
    public Transform[] HandContainer
    {
        get
        {
            return m_HandContainer;
        }      
    }

 
    private void Awake()
    {
        ModelDispatcher.Instance.AddEventListener(ConstantGuPaiJiu.SetSeatGold, SetSeatGold);
        ModelDispatcher.Instance.AddEventListener("OnSeatInfoChanged", OnSeatInfoChanged);
        ModelDispatcher.Instance.AddEventListener("OnSeatGameInfoChanged", OnSeatGameInfoChanged);
        ModelDispatcher.Instance.AddEventListener(ConstantGuPaiJiu.CloseHandContainer, CloseHandContainer);//每次结算清空牌
        ModelDispatcher.Instance.AddEventListener(ConstantGuPaiJiu.DrawPoker, DrawPoker);
        ModelDispatcher.Instance.AddEventListener("LoopCloseDeal", LoopCloseDeal);
#if IS_BAODINGQIPAI
        ModelDispatcher.Instance.AddEventListener(ConstantGuPaiJiu.OnSeatCtrlGroupPoker, OnSeatCtrlGroupPoker);
#endif

    }
    private void OnDestroy()
    {
        ModelDispatcher.Instance.RemoveEventListener(ConstantGuPaiJiu.SetSeatGold, SetSeatGold);
        ModelDispatcher.Instance.RemoveEventListener("OnSeatInfoChanged", OnSeatInfoChanged);
        ModelDispatcher.Instance.RemoveEventListener("OnSeatGameInfoChanged", OnSeatGameInfoChanged);
        ModelDispatcher.Instance.RemoveEventListener(ConstantGuPaiJiu.CloseHandContainer, CloseHandContainer);//每次结算清空牌
        ModelDispatcher.Instance.RemoveEventListener(ConstantGuPaiJiu.DrawPoker, DrawPoker);//翻牌
        ModelDispatcher.Instance.RemoveEventListener("LoopCloseDeal", LoopCloseDeal);
#if IS_BAODINGQIPAI
        ModelDispatcher.Instance.RemoveEventListener(ConstantGuPaiJiu.OnSeatCtrlGroupPoker, OnSeatCtrlGroupPoker);
#endif

    }


    #region 洗牌、发牌动画
    /// <summary>
    /// 洗牌动画
    /// </summary>
    /// <param name="room"></param>
    public void ShuffleAnimation(RoomEntity room, Action<List<GameObject>> onComplete)
    {
        StartCoroutine(ShuffleAnimationTor(room));
        //this.onComplete = onComplete;
    }
    IEnumerator ShuffleAnimationTor(RoomEntity room)
    {
        Debug.Log("          ``````````````````洗牌了``````````````````````                    ");
        string prefabName = string.Empty;
        prefabName = this.m_nSeatIndex == 0 ? "currentshuffleanimation" : "shuffleanimation";
        string path = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, prefabName);
        AssetBundleManager.Instance.LoadOrDownload(path, prefabName, (GameObject go) =>
        {
            Debug.Log("          ``````````````````开始加载洗牌动画``````````````````````                    ");
            if (ShuffleTran != null)
            {               
                if (aniQue.Count!=0)
                {
                    aniQue.Peek().gameObject.SetActive(true);
                }
                else
                {
                    Debug.Log("          ``````````````````加载洗牌动画``````````````````````                    ");
                    go = Instantiate(go);
                    go.SetParent(ShuffleTran);                   
                    aniQue.Enqueue(go.GetComponent<UIAnimation>());
                }              
               // shuffleList.Add(go.GetComponent<UIAnimation>());
            }
            Debug.Log("          ``````````````````加载洗牌动画结束``````````````````````                    ");
        });
        yield return new WaitForSeconds(1.5f);
        if (aniQue.Count != 0)
        {
            aniQue.Peek().gameObject.SetActive(false);
        }       
        Debug.Log("          ```````````````````````洗牌结束了````````````````````````````````     ");
        LoadDealEmptyPoker(room);
    }
    /// <summary>
    /// 加载发牌空牌
    /// </summary>
    private void LoadDealEmptyPoker(RoomEntity room)
    {        
        if (dealTran.gameObject.transform.childCount != 0) return;
        LoadDealEmptyPoker(room.TotalPokerNum);
        Invoke("ClisentSendPokerWallEnd",1f); //延迟1s发送生成牌墙完毕
    }
    //客户端发送生成牌墙完毕
    private void ClisentSendPokerWallEnd()
    {
        UIDispatcher.Instance.Dispatch(ConstantGuPaiJiu.GuPaiJiuClisentSendPokerWallEnd);
    }
    #endregion


    /// <summary>
    /// 加载牌墙
    /// </summary>
    /// <param name="number"></param>
    private void LoadDealEmptyPoker(int number)
    {
        if (pokerWall.Count != 0) pokerWall.Clear();
        for (int i = 0; i < number; i++)
        {
            GuPaiJiuPrefabManager.Instance.LoadPoker(m_nSeatIndex, (GameObject go) =>
            {               
                go.SetParent(dealTran.transform);
                go.AddComponent<EventTriggerListener>().onClick += (GameObject go1) =>
                {
                    if (go.tag== "pokerwall")
                    {
                        TransferData data = new TransferData();                       
                        data.SetValue("Obj", go1);
                        ModelDispatcher.Instance.Dispatch(ConstantGuPaiJiu.StartCutPoker1, data);
                    }
                };
                go.tag = "pokerwall";
                pokerWall.Add(go);
            });
        }
        dealTran.Sort();      
       // onComplete(pokerWall);
    }

    /// <summary>
    /// 正常游戏中改变座位信息
    /// </summary>
    /// <param name="data"></param>
    private void OnSeatGameInfoChanged(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        RoomEntity room = data.GetValue<RoomEntity>("Room");
        ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
        bool IsPlayer = data.GetValue<bool>("IsPlayer");
        if (m_nSeatIndex == seat.Index)
        {
            if (roomStatus == ROOM_STATUS.READY)//当房间处于空闲，清空庄下面的发牌挂载点和房间挂载点
            {
               // LoopCloseDeal(seat,IsPlayer);
            }
        }
    }


    /// <summary>
    /// 翻牌实例化牌
    /// </summary>
    /// <param name="data"></param>
    private void DrawPoker(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");              
        if (seat.Index== m_nSeatIndex)
        {           
            int dun = data.GetValue<int>("Index");         
            List<Poker> DrawPokerList = data.GetValue<List<Poker>>("DrawPokerList");
            switch (dun)
            {
                case 0:
                    LoadDrawPoker(0, DrawPokerList);
                    break;
                case 1:
                    LoadDrawPoker(2, DrawPokerList);
                    break;
            }
        }
    }

    private void LoadDrawPoker(int index,List<Poker> DrawPokerList)
    {
        for (int i = 0; i < DrawPokerList.Count; i++)
        {         
            LoadPoker(m_nSeatIndex, DrawPokerList[i].Type, m_HandContainer[i+index],0, false,true,null);
        }     
    }



    /// <summary>
    /// 设置座位牌
    /// </summary>
    /// <param name="data"></param>
    private void OnSeatInfoChanged(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        RoomEntity room = data.GetValue<RoomEntity>("Room");
        bool isPlayer = data.GetValue<bool>("IsPlayer");
        if (m_nSeatIndex == seat.Index)
        {
            if (room.roomStatus != ROOM_STATUS.IDLE && room.roomStatus != ROOM_STATUS.READY && room.roomStatus != ROOM_STATUS.GRABBANKER && room.roomStatus != ROOM_STATUS.GRABBANKERDONE)
            {
                if (seat.IsBanker) LoadDealEmptyPoker(room.RemainPokerNum);
                if (room.roomStatus == ROOM_STATUS.CHECK)
                {
                    if (seat.drawPokerList.Count == 0)
                    {
                        SeatCtrlBreak(seat);
                    }
                    else
                    {
                        SeatCtrlBreak(seat, true);
                    }
                }
                else if (room.roomStatus == ROOM_STATUS.GROUPPOKER)
                {
                    //SeatCtrlBreak(seat);
                }
                else
                {
                    SeatCtrlBreak(seat);
                }
            }
        }
    }

    //组合牌
    private void OnSeatCtrlGroupPoker(TransferData data)
    {
        int index = data.GetValue<int>("Index");
        if (index== m_nSeatIndex)
        {
            CloseHandContainer();
        }
    }




    /// <summary>
    /// 断线重连实例化牌
    /// </summary>
    private void SeatCtrlBreak(SeatEntity seat,bool isSeat=false)
    {
        CloseHandContainer();
        for (int i = 0; i < seat.pokerList.Count; i++)
        {
            pokerList.Add(seat.pokerList[i].Type);
            if (!isSeat)
            {
                LoadPoker(seat.Index, seat.pokerList[i].Type, m_HandContainer[i], i, false, true, seat);
            }
            else
            {
                LoadPoker(seat.Index, seat.pokerList[i].Type, m_HandContainer[i], i, false, true, null);
            }
        }
        if (!isSeat)
        {
            ConfirmPoker(pokerList);
        }
    }



    /// <summary>
    /// 组合牌结束，自己实例化自己的牌
    /// </summary>
    public void SeatCtrlGroupEnd(SeatEntity seat, ROOM_STATUS roomStatus)
    {
        if ((seat.seatStatus == SEAT_STATUS.SETTLE || seat.seatStatus== SEAT_STATUS.GROUPDONE||seat.isCuoPai==1)&& roomStatus != ROOM_STATUS.IDLE)
        {
            isShowType = true;
        }
        if (pokerList.Count != 0) pokerList.Clear();
        CloseHandContainer();
        for (int i = 0; i < seat.pokerList.Count; i++)
        {
            pokerList.Add(seat.pokerList[i].Type);
            LoadPoker(seat.Index, seat.pokerList[i].Type, m_HandContainer[i],i, false, true,seat);            
        }
        ConfirmPoker(pokerList);
    }

    public void SetCtrlJieSuanPoker(SeatEntity seat, ROOM_STATUS roomStatus)
    {
        StartCoroutine(SetCtrlJieSuanTor(seat,roomStatus));
    }

    IEnumerator SetCtrlJieSuanTor(SeatEntity seat, ROOM_STATUS roomStatus)
    {
        if ((seat.seatStatus == SEAT_STATUS.SETTLE || seat.seatStatus == SEAT_STATUS.GROUPDONE || seat.isCuoPai == 1) && roomStatus != ROOM_STATUS.IDLE)
        {
            isShowType = true;
        }
        if (pokerList.Count != 0) pokerList.Clear();
        for (int i = 0; i < seat.pokerList.Count; i++)
        {
            pokerList.Add(seat.pokerList[i].Type);
            if (m_nSeatIndex != 0) CloseHandContainer(m_HandContainer[i]);
            if (m_nSeatIndex != 0) LoadPoker(seat.Index, seat.pokerList[i].Type, m_HandContainer[i], i, false, true, seat);           
            if (i == 1)
            {
               if(m_nSeatIndex!=0) ConfirmPoker(pokerList);
                if (seat.PlayerId != 0)
                {
                    string MusicName = string.Format("gp_{0}_{1}", pokerType1, seat.Gender);               
                    AudioEffectManager.Instance.Play(MusicName, Vector3.zero);
                }                
                yield return new WaitForSeconds(1f);
            }
            else if (i == 3)
            {
                if (m_nSeatIndex != 0) ConfirmPoker(pokerList);
                if (seat.PlayerId != 0)
                {
                    string MusicName = string.Format("gp_{0}_{1}", pokerType2, seat.Gender);                  
                    AudioEffectManager.Instance.Play(MusicName, Vector3.zero);
                }               
            }
        }
       // ConfirmPoker(pokerList);
    }





    /// <summary>
    /// 加载牌
    /// </summary>
    /// <param name="seat"></param>
    /// <param name="poker"></param>
    /// <param name="handContainer"></param>
    /// <param name="isV3"></param>
    /// <param name="isPoker"></param>
    private void LoadPoker(int index,int pokerType,Transform handContainer,int dun, bool isV3 = false, bool isPoker = true,SeatEntity seat=null)
    {
        GuPaiJiuPrefabManager.Instance.LoadPoker(index, pokerType, (GameObject go) =>
        {
            go.transform.SetParent(handContainer);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = new Vector3(0.45f,0.45f,0.45f);
            go.name = pokerType.ToString();
            if (seat!=null|| pokerType==0)
            {
                go.AddComponent<EventTriggerListener>().onClick += (GameObject go1) =>
                {
                    GuPaiJiuGameCtrl.Instance.GuPaiJiuClisentSendDrawPoker(seat, dun / 2);
                };
            }          
        }, isV3, isPoker);
    }


    /// <summary>
    /// 判断牌型
    /// </summary>
    /// <param name="pokerList"></param>
    private void ConfirmPoker(List<int> pokerList)
    {
        if (!isShowType) return;
        for (int i = 0; i < pokerList.Count; i++)
        {
            Debug.Log(pokerList[i]);
        }
        //判断牌型
        LookupPokerType.Instance.GetDicPokerType(pokerList, out pokerType1, out pokerType2);
        Debug.Log(pokerType1+"                      牌型");
        if (pokerType1 == GuPaiJiu.PokerType.Kong && pokerType2 == GuPaiJiu.PokerType.Kong) return;
        if(pokerType1 != GuPaiJiu.PokerType.Kong)  m_pokerType1.gameObject.SetActive(true);
        if (pokerType2 != GuPaiJiu.PokerType.Kong) m_pokerType2.gameObject.SetActive(true);
#if IS_CHUANTONGPAIJIU
        if (MusicList.Count != 0) MusicList.Clear();
        MusicList.Add(pokerType1.ToString().ToLower());
        MusicList.Add(pokerType2.ToString().ToLower());
#endif
        //加载牌型图片并赋值
        m_pokerType1.sprite = GuPaiJiuPrefabManager.Instance.LoadSprite(pokerType1.ToString());
        m_pokerType2.sprite = GuPaiJiuPrefabManager.Instance.LoadSprite(pokerType2.ToString());
    }



    private void CloseHandContainer(TransferData data)
    {
        CloseHandContainer();
    }

    private void CloseHandContainer(Transform tran)
    {
        if (tran.childCount>0)
        {
            for (int i = 0; i < tran.childCount; i++)
            {
                Destroy(tran.GetChild(i).gameObject);
            }
        }
    }



    /// <summary>
    /// 清空手牌挂载点
    /// </summary>
    private void CloseHandContainer()
    {
        if (m_HandContainer.Length>0)
        {
            for (int i = 0; i < m_HandContainer.Length; i++)
            {
                if (m_HandContainer[i].childCount != 0)
                {
                    for (int j = 0; j < m_HandContainer[i].childCount; j++)
                    {
                        Destroy(m_HandContainer[i].GetChild(j).gameObject);
                    }
                }
            }
        }    
        m_pokerType1.gameObject.SetActive(false);
        m_pokerType2.gameObject.SetActive(false);
    }

    /// <summary>
    /// 清空发牌挂载点
    /// </summary>
    private void CloseDealTran()
    {

        if (dealTran.gameObject.transform.childCount != 0)
        {
            for (int j = 0; j < dealTran.gameObject.transform.childCount; j++)
            {
                Destroy(dealTran.gameObject.transform.GetChild(j).gameObject);
            }
        }
    }

    /// <summary>
    /// 每局结算，清空发牌挂载点和房间挂载点
    /// </summary>
    private void LoopCloseDeal(TransferData data)//SeatEntity seat,bool IsPlayer
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        RoomEntity room = data.GetValue<RoomEntity>("Room");
        ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
        bool IsPlayer = data.GetValue<bool>("IsPlayer");
        CloseDealTran();
        if (IsPlayer)
        {
            ModelDispatcher.Instance.Dispatch(ConstantGuPaiJiu.CloseRoomPokerTran);//清空房间挂载点
        }
       
    }

    /// <summary>
    /// 每次结算清空桌面
    /// </summary>
    /// <param name="data"></param>
    private void SetSeatGold(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");       
        if (m_nSeatIndex == seat.Index)
        {
            bool IsPlayer = data.GetValue<bool>("IsPlayer");
            RoomEntity room = data.GetValue<RoomEntity>("Room");
            StartCoroutine(SetSettle(seat, IsPlayer, room));
        }
    }
    //每次结算后清空挂载点，通知服务器开始下一次
    IEnumerator SetSettle(SeatEntity seat,bool IsPlayer,RoomEntity room)
    {
#if IS_BAODINGQIPAI
            yield return new WaitForSeconds(13);
#else
             yield return new WaitForSeconds(5);
#endif
        //CloseHandContainer();      
        if (IsPlayer)
        {
            TransferData data = new TransferData();
            data.SetValue("Room",room);
            if (room.dealSecond != (room.roomPlay==EnumPlay.BigPaiJiu?2:4)) { ModelDispatcher.Instance.Dispatch(ConstantGuPaiJiu.LoadRoomPoker, data); }//加载房间的桌面的牌          
        }
    }

    /// <summary>
    /// 结算后通过此方法开始报牌音乐
    /// </summary>
    public List<string> PlayMusic(SeatEntity seat)
    {
        //StartCoroutine(PlayMusicTor(seat));
        return MusicList;
    }
    IEnumerator PlayMusicTor(SeatEntity seat)
    {        
        Debug.Log(MusicList.Count + "                                     播放音乐的长度");
        for (int i = 0; i < MusicList.Count; i++)
        {
            string MusicName = string.Format("gp_{0}_{1}", MusicList[i], seat.Gender);          
            AudioEffectManager.Instance.Play(MusicName, Vector3.zero);
            yield return new WaitForSeconds(0.3f);
        }
    }

    /// <summary>
    /// 切牌动画
    /// </summary>
    /// <param name="wallList"></param>
    /// <param name="dun"></param>
    /// <param name="onComplete"></param>
    public void CutPokerAni(int dun, System.Action onComplete)
    {

        GameObject pokerOne = pokerWall[(2 * dun)];
        GameObject pokerTwe = pokerWall[(2 * dun + 1)];

        //改变顺序  选择距离远的切
        bool tou = dun < (pokerWall.Count / 4); //是否距离头部近       
        Debug.Log(string.Format("是否距离头部近:{0}", tou));
        pokerOne.transform.SetSiblingIndex(tou ? (pokerOne.transform.parent.childCount - 1) : 0);
        pokerTwe.transform.SetSiblingIndex(tou ? (pokerOne.transform.parent.childCount - 1) : 1);

        pokerWall.Remove(pokerOne);
        pokerWall.Remove(pokerTwe);
        pokerWall.Insert(tou ? (pokerWall.Count) : 0, pokerOne);
        pokerWall.Insert(tou ? (pokerWall.Count) : 1, pokerTwe);

        //上移
        pokerOne.transform.DOLocalMove(pokerOne.transform.localPosition + new Vector3(0, 120, 0), 0.1f).OnComplete(() =>
        {
            //移动至头尾
            pokerOne.transform.DOLocalMove(dealTran.GetPos(tou ? (pokerWall.Count - 2) : 0) + new Vector3(0, 120, 0), 0.3f).OnComplete(() =>
            {
                //全部归位
                for (int i = 0; i < pokerWall.Count; i++)
                {
                    if (i == pokerWall.Count - 1)
                    {
                        pokerWall[i].transform.DOLocalMove(dealTran.GetPos(i), 0.2f).OnComplete(() =>
                        {
                            if (onComplete != null) onComplete();
                        });

                    }
                    else
                    {
                        pokerWall[i].transform.DOLocalMove(dealTran.GetPos(i), 0.2f);
                    }
                }
            });

        });
        pokerTwe.transform.DOLocalMove(pokerTwe.transform.localPosition + new Vector3(0, 120, 0), 0.1f).OnComplete(() =>
        {
            pokerTwe.transform.DOLocalMove(dealTran.GetPos(tou ? (pokerWall.Count - 1) : 1) + new Vector3(0, 120, 0), 0.3f);
        });
    }


}




