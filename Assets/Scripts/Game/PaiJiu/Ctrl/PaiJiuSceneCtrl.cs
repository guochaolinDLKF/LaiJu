//===================================================
//Author      : WZQ
//CreateTime  ：7/4/2017 11:13:43 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaiJiu;
using proto.paigow;
using DG.Tweening;
public class PaiJiuSceneCtrl : SceneCtrlBase
{
    private PaiJiuSceneCtrl() { }

    private static PaiJiuSceneCtrl instance;

    public static PaiJiuSceneCtrl Instance
    {
        get
        {
            return instance;
        }
    }

    #region Variable
    private UIScenePaiJiuView m_UIScenePaiJiu3DView;//场景UI
    [SerializeField]
    private SeatCtrl_PaiJiu[] m_Seats;//座位(3d场景座位控制器)
    [SerializeField]
    private Transform[] m_DiceContainer;//骰子挂载点
    [SerializeField]
    private Transform m_DiceHandContainer;//按骰子的手挂载点
    [SerializeField]
    private Transform m_EffectContainer;//特效挂载点

#if IS_ZHANGJIAKOU
    private const float DEAL_ANIMATION_DURATION = 0.3f;//摸牌动画时间
#else
     private const float DEAL_ANIMATION_DURATION = 0.1f;//摸牌动画时间
#endif
#if IS_ZHANGJIAKOU
    private const float PLAYER_OPEN_POKER_DURATION = 5f;//玩家手动开牌时间
#else
    private const float PLAYER_OPEN_POKER_DURATION = 3f;//玩家手动开牌时间
#endif
    private const float ROLL_DICE_ANIMATION_DURATION = 2.0f;//摇骰子时间
    const int countPerTimes = 2;//每次抓几张
    //[SerializeField]
    //private Transform m_PoolMaJiangContainer;//牌墙挂载点  改为存储点

    private List<MaJiangCtrl_PaiJiu> wallList = new List<MaJiangCtrl_PaiJiu>();//剩余牌墙 牌List
    private GameObject hand = null;
    //private IGameAI m_AI;//游戏AI（暂无）

    /// <summary>
    /// 发牌动画时间
    /// </summary>
    //#if IS_SHUANGLIAO
    //        private const float DEAL_ANIMATION_DURATION = 0.1f;
    //#else
    //    private const float DEAL_ANIMATION_DURATION = 0.2f;
    //#endif

    //    /// <summary>
    //    /// 摇骰子动画时间
    //    /// </summary>
    //#if IS_SHUANGLIAO
    //        private const float ROLL_DICE_ANIMATION_DURATION = 1.5f;
    //#else
    //    private const float ROLL_DICE_ANIMATION_DURATION = 2.0f;
    //#endif

    #endregion


    #region Override MonoBehaviour
    protected override void OnAwake()
    {
        base.OnAwake();
        instance = this;
        for (int i = 0; i < m_Seats.Length; ++i)
        {
            //m_Seats[i].Init(RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count, RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus);
            m_Seats[i].Init(RoomPaiJiuProxy.Instance.CurrentRoom.SeatList.Count, RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus, RoomPaiJiuProxy.Instance.PlayerSeat.Pos); 
        }

        //销毁加载界面
       if(DelegateDefine.Instance.OnSceneLoadComplete != null) DelegateDefine.Instance.OnSceneLoadComplete();



        //注册改变场景的事件
        //NetDispatcher.Instance.AddEventListener(OP_ROOM_FIGHT_PASS.CODE, OnServerReturnPass);//服务器返回过
        //NetDispatcher.Instance.AddEventListener(OP_ROOM_FIGHT_WAIT.CODE, OnServerReturnOperateWait);//服务器返回吃碰杠胡等待
    }

    //游戏切入切出
    private void OnApplicationPause(bool isPause)
    {
        if (!isPause)
        {
            if (!NetWorkSocket.Instance.Connected(GameCtrl.Instance.SocketHandle))
            {
                //if (RoomMaJiangProxy.Instance.CurrentRoom.Status != RoomEntity.RoomStatus.Replay)
                //{
                PaiJiuGameCtrl.Instance.RebuildRoom();
                //}
            }
        }
    }

    protected override void OnStart()
    {
        base.OnStart();
        if (DelegateDefine.Instance.OnSceneLoadComplete != null)
        {
            DelegateDefine.Instance.OnSceneLoadComplete();
        }

        GameObject go = UIViewManager.Instance.LoadSceneUIFromAssetBundle(UIViewManager.SceneUIType.PaiJiu3D,
            () => {

                UIDispatcher.Instance.Dispatch(ConstDefine_PaiJiu.ObKey_SetEnterRoomView);

            }
            
            
            );



        m_UIScenePaiJiu3DView = go.GetComponent<UIScenePaiJiuView>();

       
        UIItemPaiJiuRoomInfo.Instance.SetUI(RoomPaiJiuProxy.Instance.CurrentRoom.roomId, 0);
        //摄像机
        DRB.MahJong. CameraCtrl.Instance.SetPos(RoomPaiJiuProxy.Instance.PlayerSeat.Pos, RoomPaiJiuProxy.Instance.CurrentRoom.SeatList.Count);
        //CompassCtrl.Instance.SetNormal();               //-------------------------------------------<指南针>-----------------------------

        //设置房间UI   由模型层发消息
        RoomPaiJiuProxy.Instance.SendRoomInfoChangeNotify();



        //未使用
        MahJongManager_PaiJiu.Instance.Init(() =>
        {
          // 如果当前房间已经开始，重建房间信息
             ReBuildRoom(RoomPaiJiuProxy.Instance.CurrentRoom);

        }
        );


        //设置BGM
        AudioBackGroundManager.Instance.Play("bgm_paijiu");
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        MahJongManager_PaiJiu.Instance.Init(null);
        //销毁注册的事件
        //NetDispatcher.Instance.RemoveEventListener(OP_ROOM_FIGHT_PASS.CODE, OnServerReturnPass);//服务器返回过
        //NetDispatcher.Instance.RemoveEventListener(OP_ROOM_FIGHT_WAIT.CODE, OnServerReturnOperateWait);//服务器返回吃碰杠胡等待
    }
    #endregion



    #region Rebuild 重建房间
    /// <summary>
    /// 重建房间
    /// </summary>
    /// <param name="room"></param>
    private void ReBuildRoom(PaiJiu.Room room)
    {
        //1.DEAL 发牌中  2.SETTLE 结算中  3.IDLE 空闲中  4.POUR 下注中 5.LOOP 看牌中 6.READY 准备中 7.DISMISS 解散中 8.CHOOSEBANKER 选庄中  9.GRABBANKER 抢庄中 10.CUTPOKER 切牌中 11.BEGIN 游戏开始（开局）

        //轮庄 1未使用    必定有牌：SETTLE  LOOP  必定无牌 IDLE READY  CHOOSEBANKER  可能有 POUR remainMahjong > 0   DISMISS remainMahjong > 0
        //抢庄 1未使用    必定有牌：SETTLE  LOOP POUR  必定无牌 IDLE READY  GRABBANKER  可能有   DISMISS remainMahjong > 0
        if (RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus== ROOM_STATUS.BEGIN|| RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus== ROOM_STATUS.CUTPOKER

            || RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus == ROOM_STATUS.SETTLE || RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus == ROOM_STATUS.LOOP
          ||( RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus == ROOM_STATUS.POUR && RoomPaiJiuProxy.Instance.CurrentRoom.remainMahjong > 0) || (RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus == ROOM_STATUS.DISMISS && RoomPaiJiuProxy.Instance.CurrentRoom.remainMahjong > 0)
          )
        {
          
            Begin(room, false);
        }


        //====================确认当前游戏状态=====================
        RoomPaiJiuProxy.Instance.SendRoomInfoChangeNotify();
    }

    #endregion

    #region RobEnd 抢庄成功动画
    /// <summary>
    /// 抢庄结束成功
    /// </summary>
    public void RobEnd(Seat seat)
    {

        System.Action OnComplete = () => {
#if IS_ZHANGJIAKOU
            //关闭抢庄状态
            ModelDispatcher.Instance.Dispatch(ConstDefine_PaiJiu.ObKey_SetRobBanker);
#endif
            //显示庄
            TransferData data = new TransferData();
            data.SetValue("seat", seat);
            PaiJiuGameCtrl.Instance.ClientSendRobComplete();
            ModelDispatcher.Instance.Dispatch(ConstDefine_PaiJiu.ObKey_SetBankerAni, data);

        };

        //===================摇骰子=====================
        StartCoroutine(RollDice(0, 0, RoomPaiJiuProxy.Instance.CurrentRoom.currentDice.diceA, RoomPaiJiuProxy.Instance.CurrentRoom.currentDice.diceB, OnComplete));

    }
    #endregion

    #region 开局相关 

    #region Begin 开局发牌等
    /// <summary>
    /// 开局
    /// </summary>
    /// <param name="room"></param>
    /// <param name="isPlayAnimation"></param>
    public void Begin(PaiJiu.Room room, bool isPlayAnimation/*, bool isReplay*/)
    {
        //CompassCtrl.Instance.SetNormal(); //指南针控制
        //UIItemTingTip.Instance.Close();
        //m_UISceneMaJiang3DView.Begin();       
        //m_UIScenePaiJiu3DView.               //设置开局的

        //====================初始化墙====================
        if (wallList.Count==0)      //如果牌墙为空  初始化墙  
        {
            InitWall(isPlayAnimation);
        }

        DrawPoker(room, isPlayAnimation);
    }

    /// <summary>
    /// 只开局 不发牌
    /// </summary>
    public void Begin(bool isPlayAnimation)
    {
        //清空全部
        ClealAllScenePoker();
        InitWall(isPlayAnimation, () =>
        {
            //开局完成
            PaiJiuGameCtrl.Instance.ClientSendBeginComplete();
        });
    }
    #endregion


    /// <summary>
    ///  发牌
    /// </summary>
    /// <param name="room"></param>
    /// <param name="isPlayAnimation"></param>
    private void DrawPoker(PaiJiu.Room room, bool isPlayAnimation)
    {

        //===================摸牌=======================
        if (!isPlayAnimation)
        {
            for (int i = 0; i < room.SeatList.Count; i++)
            {
                PaiJiu.Seat seat = room.SeatList[i];
                //桌面上的牌
                for (int j = 0; j < seat.TablePokerList.Count; j++)
                {
                    AppDebug.Log(string.Format("重连房间座位{0}已出的牌{1}", seat.Pos, (seat.TablePokerList[j].type + "_" + seat.TablePokerList[j].size)));
                    MaJiangCtrl_PaiJiu majiang = MahJongManager_PaiJiu.Instance.DrawMaJiang(seat.Pos, seat.TablePokerList[j]);
                }

                //清空摸到手里的已出过的牌
                if (seat.TablePokerList.Count > 0) GetSeatCtrlBySeatPos(seat.Pos).ClearHandPoker();
                //AppDebug.Log(string.Format("------------------------------------------------重连房间座位手牌长度{0}", seat.PokerList.Count));


                //手里的牌
                for (int j = 0; j < seat.PokerList.Count; ++j)
                {
                    //if (seat == RoomPaiJiuProxy.Instance.PlayerSeat) AppDebug.Log(string.Format("---PlayerSeat---------重连房间座位手牌{0}", seat.PokerList[j].type + "_" + seat.PokerList[j].size));
                    MaJiangCtrl_PaiJiu majiang = null;
                    majiang = MahJongManager_PaiJiu.Instance.DrawMaJiang(seat.Pos, seat.PokerList[j]);

                    //显示手牌 
                    if (seat == RoomPaiJiuProxy.Instance.PlayerSeat)
                    {
                        m_UIScenePaiJiu3DView.DrawPoker(majiang);
                    }
                    else
                    {
                        GetSeatCtrlBySeatPos(seat.Pos).DrawPoker(majiang);
                    }
                    //DRB.MahJong.MaJiangSceneCtrl
                }

            }
#if IS_ZHANGJIAKOU
            // 如果有牌墙具体信息 则翻开牌墙
            if (room.pokerWall.Count > 0) DrawMaJiangWall(room.pokerWall, false);
#endif


        }
        else
        {
            //清空手牌
            for (int i = 0; i < room.SeatList.Count; i++)
            {
                SeatCtrl_PaiJiu seatCtrl = GetSeatCtrlBySeatPos(room.SeatList[i].Pos);
                if (seatCtrl != null) seatCtrl.ClearHandPoker();
            }
            StartCoroutine(BeginAnimation(/*isReplay*/));
        }

    }





    private void InitWall(bool isPlayAnimation, System.Action OnComplete = null)
    {
        Debug.Log("开始生成牌墙");
        ////清空牌墙
        //MahJongManager_PaiJiu.Instance.ClearWall();

        //根据数量加载  加载到挂载点
        AppDebug.Log(RoomPaiJiuProxy.Instance.CurrentRoom.remainMahjong);
        int PokerTotalNum = RoomPaiJiuProxy.Instance.CurrentRoom.mahJongSum;

        wallList = MahJongManager_PaiJiu.Instance.Rebuild(PokerTotalNum);

        //庄家挡板出现牌墙   
        //WallManager_PaiJiu.Instance.InitWall(wallList, isPlayAnimation);
        GetSeatCtrlBySeatPos(RoomPaiJiuProxy.Instance.BankerSeat.Pos).InitWall(wallList, isPlayAnimation, OnComplete);
       


    }
    #endregion


    public void DrawMaJiangWall(List<Poker> pokerWall,bool isPlayAnimation)
    {
        wallList = MahJongManager_PaiJiu.Instance.DrawMaJiangWall(pokerWall);

        GetSeatCtrlBySeatPos(RoomPaiJiuProxy.Instance.BankerSeat.Pos).DrawMaJiangWall(wallList, isPlayAnimation);


    }









    #region DrawPokerAnimation 开局动画
    /// <summary>
    /// 开局动画
    /// </summary>
    /// <param name="isReplay"></param>
    /// <returns></returns>
    private IEnumerator BeginAnimation(/*bool isReplay*/)
    {
            yield return new WaitForSeconds(0.7f);
        //===================摇骰子=====================
        StartCoroutine(RollDice(0,0,RoomPaiJiuProxy.Instance.CurrentRoom.currentDice.diceA, RoomPaiJiuProxy.Instance.CurrentRoom.currentDice.diceB));
        yield return new WaitForSeconds(ROLL_DICE_ANIMATION_DURATION);
        int loopCount = RoomPaiJiuProxy.Instance.PlayerSeat.PokerList.Count/ countPerTimes;   //===================根据手牌长度设定摸牌循环次数============count/countPerTimes=========
    
        int firstGivePos = RoomPaiJiuProxy.Instance.CurrentRoom.currentDice.seatPos;
        //按顺序发牌 
        for (int i = 0; i < loopCount; ++i)
        {
            for (int j = 0; j < RoomPaiJiuProxy.Instance.CurrentRoom.SeatList.Count; ++j)
            {
                int seatPos = (firstGivePos + j - 1) % RoomPaiJiuProxy.Instance.CurrentRoom.SeatList.Count;
                PaiJiu.Seat seat = RoomPaiJiuProxy.Instance.CurrentRoom.SeatList[seatPos];
                if (seat.PlayerId <= 0) continue;

                for (int k = 0; k < countPerTimes; ++k)
                {

                    //发牌入手
                    MaJiangCtrl_PaiJiu majiang = MahJongManager_PaiJiu.Instance.DrawMaJiang(seat.Pos, seat.PokerList[(i* countPerTimes)+ k]);

                    //显示手牌
                    if (seat == RoomPaiJiuProxy.Instance.PlayerSeat)
                    {
                        m_UIScenePaiJiu3DView.DrawPoker(majiang);
                    }
                    else
                    {
                        GetSeatCtrlBySeatPos(seat.Pos).DrawPoker(majiang);
                    }

                }
#if IS_ZHANGJIAKOU
                AudioEffectManager.Instance.Play(ConstDefine_PaiJiu.FaPai_paijiu, Vector3.zero);        //播放声音
#endif
                yield return new WaitForSeconds(DEAL_ANIMATION_DURATION);
            }


        }
#if IS_ZHANGJIAKOU
        // 如果有牌墙具体信息 则翻开牌墙
        if (RoomPaiJiuProxy.Instance.CurrentRoom.pokerWall.Count > 0) DrawMaJiangWall(RoomPaiJiuProxy.Instance.CurrentRoom.pokerWall, true);
#endif
        yield return new WaitForSeconds(PLAYER_OPEN_POKER_DURATION);
        PaiJiuGameCtrl.Instance.ClientSendGetSettle();


    }
    #endregion

    #region RollDice 摇骰子
    /// <summary>
    /// 摇骰子 只使用第二次摇骰子
    /// </summary>
    /// <param name="firstDiceA">第一次摇</param>
    /// <param name="firstDiceB"></param>
    /// <param name="secondDiceA">第二次摇</param>
    /// <param name="secondDiceB"></param>
    private IEnumerator RollDice(int firstDiceA, int firstDiceB, int secondDiceA, int secondDiceB, System.Action OnComplete = null)
    {
        int bankerPos = RoomPaiJiuProxy.Instance.BankerSeat.Pos;

        if (bankerPos == 2 && RoomPaiJiuProxy.Instance.CurrentRoom.SeatList.Count == 2)
        {
            bankerPos = 3;
        }
        //==================手动画动画==========================
        if (SystemProxy.Instance.HasHand)
        {
            if (hand == null)
            {
                string handPrefabName = "dicehand";
                string handPath = string.Format("download/{0}/prefab/model/{1}.drb", ConstDefine.GAME_NAME, handPrefabName);
                AssetBundleManager.Instance.LoadOrDownload(handPath, handPrefabName, (GameObject go) =>
                {
                    hand = Instantiate(go);
                    hand.SetParent(m_DiceHandContainer);
                    hand.transform.localEulerAngles = new Vector3(0, (bankerPos - 1) * (-90f), 0);
                });

            }
            else
            {
                hand.transform.localEulerAngles = new Vector3(0, (bankerPos - 1) * (-90f), 0);
                hand.SetActive(true);
            }
                    

            yield return new WaitForSeconds(0.5f);
        }


        //==================骰子动画==========================
        AudioEffectManager.Instance.Play("rolldice", Vector3.zero, false);
        string prefabName = "dice";
        string path = string.Format("download/{0}/prefab/model/{1}.drb", ConstDefine.GAME_NAME, prefabName);
        AssetBundleManager.Instance.LoadOrDownload(path, prefabName, (GameObject go) =>
        {
            go = Instantiate(go);
            DRB.MahJong. DiceCtrl ctrl = go.GetComponent<DRB.MahJong.DiceCtrl>();
            go.SetParent(m_DiceContainer[0]);
            go.transform.localPosition = GameUtil.GetRandomPos(go.transform.position, 1f);
            ctrl.Roll(firstDiceA, secondDiceA);
        });

        AssetBundleManager.Instance.LoadOrDownload(path, prefabName, (GameObject go) =>
        {
            go = Instantiate(go);
            DRB.MahJong.DiceCtrl ctrl2 = go.GetComponent<DRB.MahJong.DiceCtrl>();
            go.SetParent(m_DiceContainer[1]);
            go.transform.localPosition = GameUtil.GetRandomPos(go.transform.position, 1f);
            ctrl2.Roll(firstDiceB, secondDiceB);
        });

        if (OnComplete != null)
        {
            yield return new WaitForSeconds(2f);
            OnComplete();
        }
    }
    #endregion


    #region   CutPoker  切牌
    private bool isPlayCutPokerAni = false;

    private Queue<int> CutPokerAniQueue = new Queue<int>(); //切牌队列

    /// <summary>
    ///  切牌  参数为要切的第几墩
    /// </summary>
    /// <param name="dun"></param>
    public void CutPoker(int dun)
    {
        CutPokerAniQueue.Enqueue(dun);

        if (isPlayCutPokerAni) return;
        isPlayCutPokerAni = true;
        CutPokerAni();

        //wallList
        //MahJongManager_PaiJiu

    }


    private void CutPokerAni()
    {

       SeatCtrl_PaiJiu  seatCtrl =  GetSeatCtrlBySeatPos(RoomPaiJiuProxy.Instance.BankerSeat.Pos);

        if (seatCtrl != null)
        {
            int dun = CutPokerAniQueue.Dequeue();

            //动画结束事件？？  
            seatCtrl.CutPokerAni(wallList , dun,  ()=> {

                if (CutPokerAniQueue.Count > 0)
                {
                    CutPokerAni();
                }
                else
                {
                    isPlayCutPokerAni = false;
                    // 发送结束消息
                    PaiJiuGameCtrl.Instance.ClientSendCutPokerComplete();
                    return;
                }

            });

        }
        


        


    }
    #endregion


    #region SetHandPokerStatus 变更手牌状态 翻牌  
    /// <summary>
    ///  变更手牌状态 翻牌
    /// </summary>
    /// <param name="seatPos"></param>
    public void SetHandPokerStatus(int seatPos ,Seat seat)
    {
        //显示手牌
        if (seatPos == RoomPaiJiuProxy.Instance.PlayerSeat.Pos)
        {
            m_UIScenePaiJiu3DView.SetHandPokerStatus(seat);
        }
        else
        {
            //MahJongManager_PaiJiu 更改数据 
            // 更改预制
            AppDebug.Log(string.Format("结算开牌座位：{0}", seatPos));
            GetSeatCtrlBySeatPos(seatPos).SetPokerStatus(seat);
        }
    }
    #endregion

    #region Settle  结算
    public void Settle(bool loopEnd)
    {
        //1 翻开所有手牌  
        for (int i = 0; i < RoomPaiJiuProxy.Instance.CurrentRoom.SeatList.Count; i++)
        {
            SetHandPokerStatus(RoomPaiJiuProxy.Instance.CurrentRoom.SeatList[i].Pos, RoomPaiJiuProxy.Instance.CurrentRoom.SeatList[i]);
        }
        //ui处理
        m_UIScenePaiJiu3DView.EverytimeSettle(RoomPaiJiuProxy.Instance.CurrentRoom);    
    }
    #endregion

    #region NextGame 准备开始下一次   
    /// <summary>
    /// 准备开始下一次
    /// </summary>
    public void NextGame()
    {
        //清空场景
        PaiJiu.Room Currentroom = RoomPaiJiuProxy.Instance.CurrentRoom;
        //1 将用过的牌放入桌子   
        if (Currentroom.loopEnd) MahJongManager_PaiJiu.Instance.ClearWall();
        for (int i = 0; i < Currentroom.SeatList.Count; i++)
        {
            if (Currentroom.SeatList[i].PlayerId > 0)
            {
                //弃牌
                if (Currentroom.SeatList[i] == RoomPaiJiuProxy.Instance.PlayerSeat)
                {
                    m_UIScenePaiJiu3DView.ClearHandPoker();
                }
            
                SeatCtrl_PaiJiu seatCtrl = GetSeatCtrlBySeatPos(Currentroom.SeatList[i].Pos);
                seatCtrl.ClearHandPoker();

                //如果是一局结束 清空打过的牌 
                if(Currentroom.loopEnd) seatCtrl.ClearDeskTopContainer();

                //如果是一局结束 清空牌墙
            }
        }
    }
    #endregion

    #region ClealAllScenePoker 清空场景中全部Poker  
    /// <summary>
    /// 清空场景中全部Poker
    /// </summary>
    private void ClealAllScenePoker()
    {
        //清空场景
        PaiJiu.Room Currentroom = RoomPaiJiuProxy.Instance.CurrentRoom;
        //1 将用过的牌放入桌子   
        MahJongManager_PaiJiu.Instance.ClearWall();
        for (int i = 0; i < Currentroom.SeatList.Count; i++)
        {
            if (Currentroom.SeatList[i].PlayerId > 0)
            {
               

                SeatCtrl_PaiJiu seatCtrl = GetSeatCtrlBySeatPos(Currentroom.SeatList[i].Pos);

                if (seatCtrl != null)
                {
                    //弃牌
                    if (Currentroom.SeatList[i] == RoomPaiJiuProxy.Instance.PlayerSeat)
                    {
                        m_UIScenePaiJiu3DView.ClearHandPoker();
                    }
                    else
                    {
                        //清空手牌
                        seatCtrl.ClearHandPoker();
                    }
                //清空打过的牌 
                seatCtrl.ClearDeskTopContainer();

                }
            }

        }


    }
    #endregion



    #region  ChooseBanker 选庄时场景处理
    /// <summary>
    /// 选庄时场景处理
    /// </summary>
    public void ChooseBanker()
    {
        //清空场景
        PaiJiu.Room Currentroom = RoomPaiJiuProxy.Instance.CurrentRoom;
       
         MahJongManager_PaiJiu.Instance.ClearWall();

        for (int i = 0; i < Currentroom.SeatList.Count; i++)
        {
            if (Currentroom.SeatList[i].PlayerId > 0)
            {
                SeatCtrl_PaiJiu seatCtrl = GetSeatCtrlBySeatPos(Currentroom.SeatList[i].Pos);
                seatCtrl.ClearHandPoker();
                seatCtrl.ClearDeskTopContainer();
            }
        }
    }
    #endregion

    #region GetSeatCtrlBySeatPos 根据座位号获取座位控制器
    /// <summary>
    /// 根据座位号获取座位控制器
    /// </summary>
    /// <param name="seatPos"></param>
    /// <returns></returns>
    private SeatCtrl_PaiJiu GetSeatCtrlBySeatPos(int seatPos)
    {
        for (int i = 0; i < m_Seats.Length; ++i)
        {
            if (m_Seats[i].SeatPos == seatPos)
            {
                return m_Seats[i];
            }
        }
        return null;
    }
    #endregion

    #region OnPlayerClick 场景物体点击
    protected override void OnPlayerClick()
    {
        base.OnPlayerClick();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Table"));
        if (hits != null && hits.Length > 0)
        {         
            MaJiangCtrl_PaiJiu ctrl = hits[0].collider.gameObject.GetComponent<MaJiangCtrl_PaiJiu>();
            if (ctrl == null) return;

#if IS_ZHANGJIAKOU
            //发送切牌信息
            if (RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus == ROOM_STATUS.CUTPOKER && RoomPaiJiuProxy.Instance.PlayerSeat.isCutPoker == Seat.CutPoker.CutPoker)
            {
                Debug.Log("点击牌墙 可以切牌 ");
                for (int i = 0; i < wallList.Count; i++)
                {
                    if (ctrl == wallList[i])
                    {
                        int index = ctrl.transform.GetSiblingIndex();

                        PaiJiuGameCtrl.Instance.OnBtnClientSendCutPokerInfo(index / 2);
                        break;
                    }

                }

            }

#endif

        }
    }
    #endregion
}
