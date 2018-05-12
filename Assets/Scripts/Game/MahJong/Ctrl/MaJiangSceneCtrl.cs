//===================================================
//Author      : DRB
//CreateTime  ：4/1/2017 11:54:40 AM
//Description ：麻将场景控制器
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using proto.mahjong;

namespace DRB.MahJong
{
    public class MaJiangSceneCtrl : SceneCtrlBase
    {
        #region Singleton
        public static MaJiangSceneCtrl Instance;
        #endregion

        #region Variable
        private UISceneMaJiangView m_UISceneMaJiang3DView;//场景UI
        [SerializeField]
        private SeatCtrl[] m_Seats;//座位
        [SerializeField]
        private Transform[] m_DiceContainer;//骰子挂载点
        [SerializeField]
        private Transform m_DiceHandContainer;//按骰子的手挂载点
        [SerializeField]
        private Transform m_EffectContainer;//特效挂载点

        [SerializeField]
        private Transform[] m_Table;//桌子模型

        private MaJiangCtrl m_Temp;//拖拽临时麻将
        private Vector2 m_DragBeginPos;//拖拽开始坐标点

        /// <summary>
        /// 发牌动画时间
        /// </summary>
        private const float DEAL_ANIMATION_DURATION = 0.1f;

        /// <summary>
        /// 摇骰子动画时间
        /// </summary>
#if IS_SHUANGLIAO
        private const float ROLL_DICE_ANIMATION_DURATION = 1.5f;
#else
        private const float ROLL_DICE_ANIMATION_DURATION = 2.0f;
#endif


        private Queue<Poker> m_ListLuckPoker = new Queue<Poker>();//换宝队列
        private Queue<DiceEntity> m_ListDice = new Queue<DiceEntity>();//骰子队列
        private bool m_isPlaying = false;//是否正在播放换宝动画

        private bool m_isBegining;//是否正在播放开局动画

        #endregion

        #region GetSeatCtrlBySeatPos 根据座位号获取座位控制器
        /// <summary>
        /// 根据座位号获取座位控制器
        /// </summary>
        /// <param name="seatPos"></param>
        /// <returns></returns>
        private SeatCtrl GetSeatCtrlBySeatPos(int seatPos)
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

        #region Override MonoBehaviour
        protected override void OnAwake()
        {
            base.OnAwake();

            Instance = this;

            if (DelegateDefine.Instance.OnSceneLoadComplete != null)
            {
                DelegateDefine.Instance.OnSceneLoadComplete();
            }
            ModelDispatcher.Instance.AddEventListener(SystemProxy.ON_TABLE_COLOR_CHANGED, OnTableColorChanged);
            ModelDispatcher.Instance.AddEventListener(SystemProxy.ON_POKER_COLOR_CHANGED, OnPokerColorChanged);

            AudioBackGroundManager.Instance.Play("bgm_majiang");

#if !IS_GONGXIAN
            SetTableColor(SystemProxy.Instance.TableColor);
#endif
            SetPokerColor(SystemProxy.Instance.PokerColor);
        }

        private void OnApplicationPause(bool isPause)
        {
            if (!isPause)
            {
                if (!NetWorkSocket.Instance.Connected(GameCtrl.Instance.SocketHandle))
                {
                    if (!RoomMaJiangProxy.Instance.CurrentRoom.isReplay)
                    {
                        GameCtrl.Instance.RebuildRoom();
                    }
                }
            }

        }

        private void OnApplicationFocus(bool focus)
        {
            MaJiangGameCtrl.Instance.ClientSendFocus(focus);
        }

        protected override void OnStart()
        {
            base.OnStart();

            GameObject go = UIViewManager.Instance.LoadSceneUIFromAssetBundle(UIViewManager.SceneUIType.MaJiang3D);
            m_UISceneMaJiang3DView = go.GetComponent<UISceneMaJiangView>();
            m_AI = new GameMahJongAI();

            //初始化麻将管理器
            MahJongManager.Instance.Init();
            Init();
        }

        protected override void BeforeOnDestroy()
        {
            base.BeforeOnDestroy();

            ModelDispatcher.Instance.RemoveEventListener(SystemProxy.ON_TABLE_COLOR_CHANGED, OnTableColorChanged);
            ModelDispatcher.Instance.RemoveEventListener(SystemProxy.ON_POKER_COLOR_CHANGED, OnPokerColorChanged);

            MahJongManager.Instance.Dispose();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (!m_isBegining)
            {
                if (MaJiangGameCtrl.Instance.CommandQueue.Count > 0)
                {
                    try
                    {
                        IGameCommand command = MaJiangGameCtrl.Instance.CommandQueue.Dequeue();
                        command.Execute();
                    }
                    catch (Exception e)
                    {
                        LogSystem.LogError(e.ToString());
                        MaJiangGameCtrl.Instance.RebuildRoom();
                    }
                }
            }
        }
        #endregion

        #region Init 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            for (int i = 0; i < m_Seats.Length; ++i)
            {
                m_Seats[i].Init(RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count);
            }

            string param = string.Empty;
            cfg_settingEntity limit = RoomMaJiangProxy.Instance.GetConfigByTag("limit");
            if (limit != null)
            {
                param = string.Format("{0}:{1}", limit.label, limit.name);
            }
            UIItemMahJongRoomInfo.Instance.SetUI(RoomMaJiangProxy.Instance.CurrentRoom.roomId, RoomMaJiangProxy.Instance.CurrentRoom.BaseScore, param);

            UIItemMahJongRoomInfo.Instance.SetRoomConfig(RoomMaJiangProxy.Instance.CurrentRoom.Config);
            
            UIItemMahJongRoomInfo.Instance.SetGameName(RoomMaJiangProxy.Instance.CurrentRoom.GameName);

            CompassCtrl.Instance.SetNormal();

            CameraCtrl.Instance.SetPokerTotal(RoomMaJiangProxy.Instance.CurrentRoom.PokerTotalPerPlayer);

            RoomMaJiangProxy.Instance.SendRoomInfoChangeNotify();


            if (RoomMaJiangProxy.Instance.PlayerSeat == null)
            {
                AppDebug.ThrowError("玩家座位是空的，玩个毛线");
            }

            CameraCtrl.Instance.SetPos(RoomMaJiangProxy.Instance.PlayerSeat.Pos, RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count);

            if (RoomMaJiangProxy.Instance.CurrentRoom.isReplay)
            {
                StartCoroutine(RecordReplay());
                return;
            }
            else
            {
                ReBuildRoom(RoomMaJiangProxy.Instance.CurrentRoom);
            }

            if (RoomMaJiangProxy.Instance.CurrentRoom.matchId > 0)
            {
                MaJiangGameCtrl.Instance.ClientSendReady();
            }
        }
        #endregion

        #region OnTableColorChanged 桌子颜色变更回调
        /// <summary>
        /// 桌子颜色变更回调
        /// </summary>
        /// <param name="data"></param>
        private void OnTableColorChanged(TransferData data)
        {
            string color = data.GetValue<string>("TableColor");
            if (string.IsNullOrEmpty(color)) return;

            SetTableColor(color);
        }
        #endregion

        #region OnPokerColorChanged 麻将颜色变更回调
        /// <summary>
        /// 麻将颜色变更回调
        /// </summary>
        /// <param name="obj"></param>
        private void OnPokerColorChanged(TransferData data)
        {
            string color = data.GetValue<string>("PokerColor");
            if (string.IsNullOrEmpty(color)) return;

            SetPokerColor(color);
        }
        #endregion

        #region SetTableColor 设置桌面颜色
        /// <summary>
        /// 设置桌面颜色
        /// </summary>
        /// <param name="color"></param>
        private void SetTableColor(string color)
        {
            if (m_Table != null)
            {
                string path = string.Format("download/{0}/source/modelsource/materials/table_{1}.drb", ConstDefine.GAME_NAME, color);
                string name = string.Format("table_{0}", color);
                AssetBundleManager.Instance.LoadOrDownload(path, name, (Material mat) =>
                {
                    if (mat == null) return;
                    for (int i = 0; i < m_Table.Length; ++i)
                    {
                        m_Table[i].GetComponent<Renderer>().material = mat;
                    }
                }, 0);
            }
        }
        #endregion

        #region SetPokerColor 设置麻将颜色
        /// <summary>
        /// 设置麻将颜色
        /// </summary>
        /// <param name="color"></param>
        private void SetPokerColor(string color)
        {
            MahJongManager.Instance.SetPokerColor(color);
        }
        #endregion

        #region Rebuild 重建房间
        /// <summary>
        /// 重建房间
        /// </summary>
        /// <param name="room"></param>
        private void ReBuildRoom(RoomEntity room)
        {
            Debug.Log(RoomMaJiangProxy.Instance.CurrentRoom.Status);
            if (RoomMaJiangProxy.Instance.CurrentRoom.Status != RoomEntity.RoomStatus.Ready && RoomMaJiangProxy.Instance.CurrentRoom.Status != RoomEntity.RoomStatus.Settle)
            {
                Begin(room, false, false);
            }

            //====================确认当前游戏状态=====================
            for (int i = 0; i < room.SeatList.Count; ++i)
            {
                if (room.SeatList[i].Status == SeatEntity.SeatStatus.Fight && room.SeatList[i].Pos == RoomMaJiangProxy.Instance.PlayerSeat.Pos)
                {

                    if (RoomMaJiangProxy.Instance.AskPokerGroup != null && RoomMaJiangProxy.Instance.AskPokerGroup.Count > 0)
                    {
                        IGameCommand command = new AskOperateCommand(RoomMaJiangProxy.Instance.AskPokerGroup, 0);
                        MaJiangGameCtrl.Instance.CommandQueue.Enqueue(command);
                    }

                }

                if (room.SeatList[i].DisbandState == DisbandState.Apply)
                {
                    MaJiangGameCtrl.Instance.OpenDisbandView();
                }
            }

            RoomMaJiangProxy.Instance.SendRoomInfoChangeNotify();
        }

        #endregion

        public void CheckTing()
        {
            GetSeatCtrlBySeatPos(RoomMaJiangProxy.Instance.PlayerSeat.Pos).CheckTing();
        }

        #region 开局相关 
        #region Begin 开局
        /// <summary>
        /// 开局
        /// </summary>
        /// <param name="room"></param>
        /// <param name="isPlayAnimation"></param>
        public void Begin(RoomEntity room, bool isPlayAnimation, bool isReplay)
        {
            CompassCtrl.Instance.SetNormal();
            UIItemTingTip.Instance.Close();
            m_UISceneMaJiang3DView.Begin(isPlayAnimation);
            for (int i = 0; i < m_Seats.Length; ++i)
            {
                bool isHu = false;
                if (RoomMaJiangProxy.Instance.GetSeatBySeatId(m_Seats[i].SeatPos) != null)
                {
                    isHu = RoomMaJiangProxy.Instance.GetSeatBySeatId(m_Seats[i].SeatPos).isHu;
                }
                Debug.Log(isHu);
                m_Seats[i].Begin(RoomMaJiangProxy.Instance.PlayerSeat.Pos, isHu, RoomMaJiangProxy.Instance.CurrentRoom.isReplay);
            }
            //====================初始化墙====================
            AppDebug.Log("第二个摇骰子的人是" + room.SecondDice.seatPos);

            if (RoomMaJiangProxy.Instance.CurrentRoom.currentLoop == 1)
            {
                CheckIP(RoomMaJiangProxy.Instance.CurrentRoom.SeatList);
            }

            InitWall(isPlayAnimation);
            //===================摸牌=======================
            if (!isPlayAnimation)
            {
                PlayLuckPokerAnimation(isPlayAnimation);

                MahJongHelper.Sort(RoomMaJiangProxy.Instance.PlayerSeat.PokerList, RoomMaJiangProxy.Instance.PlayerSeat.UniversalList, RoomMaJiangProxy.Instance.Rule.UniversalSortType);
                for (int i = 0; i < room.SeatList.Count; ++i)
                {
                    SeatEntity seat = room.SeatList[i];


                    //桌面上的牌
                    for (int j = 0; j < seat.DeskTopPoker.Count; ++j)
                    {
                        MaJiangCtrl majiang = MahJongManager.Instance.DrawMaJiang(seat.Pos, seat.DeskTopPoker[j], seat == RoomMaJiangProxy.Instance.PlayerSeat, false, false);
                        majiang = MahJongManager.Instance.PlayMaJiang(seat.Pos, seat.DeskTopPoker[j]);
                        bool isJin = false;
                        if (MahJongHelper.HasPoker(seat.DeskTopPoker[j], seat.UniversalList))
                        {
                            isJin = true;
                        }
                        GetSeatCtrlBySeatPos(seat.Pos).PlayPoker(majiang, isPlayAnimation, false, isJin);
                    }

                    List<MaJiangCtrl> majiangs = new List<MaJiangCtrl>();
                    //手里的牌
                    for (int j = 0; j < seat.PokerList.Count; ++j)
                    {

                        MaJiangCtrl majiang = null;
                        Poker drawPoker = seat.PokerList[j];
                        for (int k = 0; k < seat.DingJiangPoker.Count; ++k)
                        {
                            if (seat.PokerList[j].index == seat.DingJiangPoker[k].index)
                            {
                                drawPoker = seat.DingJiangPoker[k];
                            }
                        }
                        majiang = MahJongManager.Instance.DrawMaJiang(seat.Pos, drawPoker, seat == RoomMaJiangProxy.Instance.PlayerSeat, false, false);
                        majiangs.Add(majiang);
                    }
                    GetSeatCtrlBySeatPos(seat.Pos).DrawPoker(majiangs);

                    //碰的牌
                    for (int j = 0; j < seat.UsedPokerList.Count; ++j)
                    {
                        PokerCombinationEntity combination = seat.UsedPokerList[j];
                        for (int k = 0; k < seat.UsedPokerList[j].PokerList.Count; ++k)
                        {
                            if (seat.UsedPokerList[j].CombinationType == OperatorType.Gang && k == 3)
                            {
                                MahJongManager.Instance.DrawMaJiang(seat.Pos, combination.PokerList[k], seat == RoomMaJiangProxy.Instance.PlayerSeat, true, false);
                            }
                            else
                            {
                                MahJongManager.Instance.DrawMaJiang(seat.Pos, combination.PokerList[k], seat == RoomMaJiangProxy.Instance.PlayerSeat, false, false);
                            }
                        }
                        Combination3D Combination3D = MahJongManager.Instance.Operate(seat.Pos, combination.CombinationType, combination.SubTypeId, combination.PokerList);
                        GetSeatCtrlBySeatPos(seat.Pos).Operate(Combination3D, false);
                    }

                    //摸的牌(第14张)
                    if (seat.HitPoker != null)
                    {
                        MaJiangCtrl majiang = MahJongManager.Instance.DrawMaJiang(seat.Pos, seat.HitPoker, seat == RoomMaJiangProxy.Instance.PlayerSeat, false, true);
                        GetSeatCtrlBySeatPos(seat.Pos).DrawPoker(majiang);
                    }

                    if (room.Status == RoomEntity.RoomStatus.Swap && seat.SwapPoker != null && seat.SwapPoker.Count > 0)
                    {
                        GetSeatCtrlBySeatPos(seat.Pos).SetSwap(seat.SwapPoker);
                    }
                }

                if (RoomMaJiangProxy.Instance.PlayerSeat.Status == SeatEntity.SeatStatus.Operate || RoomMaJiangProxy.Instance.PlayerSeat.HitPoker != null)
                {
                    GetSeatCtrlBySeatPos(RoomMaJiangProxy.Instance.PlayerSeat.Pos).CheckTing();
                }
                else
                {
                    List<Poker> ting = RoomMaJiangProxy.Instance.CheckTingByHand();
                    UIItemTingTip.Instance.Show(null, ting);
                }

                if (RoomMaJiangProxy.Instance.CurrentRoom.CurrentOperator != null)
                {
                    m_UISceneMaJiang3DView.SetOperating(RoomMaJiangProxy.Instance.CurrentRoom.CurrentOperator.Index);
                    CompassCtrl.Instance.SetCurrent(RoomMaJiangProxy.Instance.CurrentRoom.CurrentOperator.Pos, room.SeatList.Count, RoomMaJiangProxy.Instance.CurrentRoom.CurrentOperator.Direction);
                }
            }
            else
            {
                StartCoroutine(BeginAnimation(isReplay));
            }
        }
        #endregion

        #region InitWall 初始化牌墙
        /// <summary>
        /// 初始化牌墙
        /// </summary>
        /// <param name="wall">所有牌</param>
        /// <param name="playerCount">玩家数量</param>
        /// <param name="bankerPos">庄家座位索引</param>
        /// <param name="isPlayAnimation">是否播放动画</param>
        private void InitWall(bool isPlayAnimation)
        {
            int playerCount = m_Seats.Length;
            int bankerPos = RoomMaJiangProxy.Instance.CurrentRoom.BankerPos;
            if (bankerPos == 2 && RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count == 2)
            {
                bankerPos = 3;
            }
            List<MaJiangCtrl> wall = MahJongManager.Instance.Rebuild(RoomMaJiangProxy.Instance.CurrentRoom.PokerTotal, RoomMaJiangProxy.Instance.CurrentRoom.SecondDice.seatPos, RoomMaJiangProxy.Instance.CurrentRoom.SecondDice.diceA, RoomMaJiangProxy.Instance.CurrentRoom.SecondDice.diceB, m_Seats.Length, bankerPos);
            if (!RoomMaJiangProxy.Instance.Rule.isDisplayWall) return;
            int tableMaJiangCount = wall.Count / playerCount;
            if (tableMaJiangCount % 2 == 0)
            {
                int index = 0;
                for (int i = bankerPos - 1; i < playerCount + bankerPos - 1; ++i)
                {
                    List<MaJiangCtrl> lst = new List<MaJiangCtrl>();
                    int endIndex = tableMaJiangCount + index;
                    for (int j = index; j < endIndex; ++j, ++index)
                    {
                        lst.Add(wall[j]);
                    }
                    int seatIndex = bankerPos - i - 1 < 0 ? bankerPos - i - 1 + m_Seats.Length : bankerPos - 1 - i;
                    m_Seats[seatIndex].InitWall(lst, isPlayAnimation);
                }
            }
            else
            {
                int beginIndex = 0;
                for (int i = bankerPos - 1; i < playerCount + bankerPos - 1; ++i)
                {
                    List<MaJiangCtrl> lst = new List<MaJiangCtrl>();
                    int endIndex = 0;
                    if (i == bankerPos - 1)
                    {
                        endIndex = tableMaJiangCount + beginIndex + playerCount - 1;
                        for (int j = beginIndex; j < endIndex; ++j)
                        {
                            lst.Add(wall[j]);
                        }
                    }
                    else
                    {
                        endIndex = tableMaJiangCount + beginIndex - 1;
                        for (int j = beginIndex; j < endIndex; ++j)
                        {
                            lst.Add(wall[j]);
                        }
                    }
                    beginIndex += endIndex - beginIndex;
                    int seatIndex = bankerPos - i - 1 < 0 ? bankerPos - i - 1 + m_Seats.Length : bankerPos - 1 - i;
                    m_Seats[seatIndex].InitWall(lst, isPlayAnimation);
                }
            }
        }
        #endregion

        #region DrawPokerAnimation 开局动画
        /// <summary>
        /// 开局动画
        /// </summary>
        /// <param name="isReplay"></param>
        /// <returns></returns>
        private IEnumerator BeginAnimation(bool isReplay)
        {
            m_isBegining = true;
            yield return new WaitForSeconds(0.7f);
            //===================摇骰子=====================
            yield return StartCoroutine(RollDice(RoomMaJiangProxy.Instance.CurrentRoom.BankerPos, RoomMaJiangProxy.Instance.CurrentRoom.FirstDice.diceA, RoomMaJiangProxy.Instance.CurrentRoom.FirstDice.diceB));
#if IS_WANGQUE
            yield return StartCoroutine(RollDice(RoomMaJiangProxy.Instance.CurrentRoom.BankerPos, RoomMaJiangProxy.Instance.CurrentRoom.SecondDice.diceA, RoomMaJiangProxy.Instance.CurrentRoom.SecondDice.diceB));
#endif

#if IS_LONGGANG || IS_LEPING
            yield return StartCoroutine(RollDice(RoomMaJiangProxy.Instance.CurrentRoom.BankerPos, RoomMaJiangProxy.Instance.CurrentRoom.SecondDice.diceA, RoomMaJiangProxy.Instance.CurrentRoom.SecondDice.diceB));
#endif
            //===================发牌=====================
            yield return StartCoroutine(PlayDealAnimation());
            //===================翻宝=====================
#if !IS_LEPING
#if IS_GUGENG
            AudioEffectManager.Instance.Play("selectUniversal");
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(RollDice(RoomMaJiangProxy.Instance.CurrentRoom.BankerPos, RoomMaJiangProxy.Instance.CurrentRoom.SecondDice.diceA, RoomMaJiangProxy.Instance.CurrentRoom.SecondDice.diceB));
#endif
            PlayLuckPokerAnimation();
#endif

            yield return new WaitForSeconds(1.2f);

            if (isReplay)
            {
                for (int i = 0; i < RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count; ++i)
                {
                    MahJongManager.Instance.Sort(MahJongManager.Instance.GetHand(RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i].Pos), RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i].UniversalList);
                    GetSeatCtrlBySeatPos(RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i].Pos).HandSort();
                }
            }

            m_isBegining = false;
        }
        #endregion

        #region PlayDealAnimation 播放发牌动画
        /// <summary>
        /// 播放发牌动画
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlayDealAnimation()
        {
            AudioEffectManager.Instance.Play("fapai", Vector3.zero, false);
            const int countPerTimes = 4;//每次抓几张
            int pokerCount = RoomMaJiangProxy.Instance.CurrentRoom.SeatList[0].PokerList.Count;
            if (pokerCount == 0) yield break;
            int loopCount = Mathf.FloorToInt(pokerCount / countPerTimes);
            int overplusCount = pokerCount % countPerTimes;

            for (int i = 0; i < loopCount; ++i)
            {
                for (int j = 0; j < RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count; ++j)
                {
                    SeatEntity seat = RoomMaJiangProxy.Instance.CurrentRoom.SeatList[j];
                    for (int k = 0; k < countPerTimes; ++k)
                    {
                        int index = i * countPerTimes + k;
                        MaJiangCtrl majiang = MahJongManager.Instance.DrawMaJiang(seat.Pos, seat.PokerList[index], seat == RoomMaJiangProxy.Instance.PlayerSeat, false, false);
                        GetSeatCtrlBySeatPos(seat.Pos).DrawPoker(majiang, true);
                    }
                    yield return new WaitForSeconds(DEAL_ANIMATION_DURATION);
                }
            }

            yield return null;

            for (int j = 0; j < RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count; ++j)
            {
                SeatEntity seat = RoomMaJiangProxy.Instance.CurrentRoom.SeatList[j];
                for (int k = 0; k < overplusCount; ++k)
                {
                    MaJiangCtrl majiang = MahJongManager.Instance.DrawMaJiang(seat.Pos, seat.PokerList[loopCount * countPerTimes + k], seat == RoomMaJiangProxy.Instance.PlayerSeat, false, false);
                    GetSeatCtrlBySeatPos(seat.Pos).DrawPoker(majiang, true);
                }
                yield return new WaitForSeconds(DEAL_ANIMATION_DURATION);
            }
            yield return null;

            if (!RoomMaJiangProxy.Instance.CurrentRoom.isReplay)
            {
                yield return StartCoroutine(GetSeatCtrlBySeatPos(RoomMaJiangProxy.Instance.PlayerSeat.Pos).PlayDealAnimation());
                List<Poker> ting = RoomMaJiangProxy.Instance.CheckTingByHand();
                UIItemTingTip.Instance.Show(null, ting);
            }
            else
            {
                GetSeatCtrlBySeatPos(RoomMaJiangProxy.Instance.PlayerSeat.Pos).HandSort();
            }
        }
        #endregion

        #region RollDice 摇骰子
        /// <summary>
        /// 摇骰子
        /// </summary>
        /// <param name="DiceA"></param>
        /// <param name="DiceB"></param>
        public IEnumerator RollDice(int seatPos, int DiceA, int DiceB)
        {
            if (seatPos == 0) yield break;
            if (DiceA == 0 && DiceB == 0) yield break;
            if (SystemProxy.Instance.HasHand)
            {
                GameObject hand = MahJongManager.Instance.SpawnHand_Fang();
                hand.SetParent(m_DiceHandContainer);
                hand.transform.localEulerAngles = new Vector3(0, (seatPos - 1) * -90f, 0);
                yield return new WaitForSeconds(0.5f);
            }

            AudioEffectManager.Instance.Play("rolldice", Vector3.zero, false);

            Coroutine coroutine = null;
            if (DiceA != 0)
            {
                GameObject dice1 = MahJongManager.Instance.SpawnDice();
                DiceCtrl ctrl = dice1.GetComponent<DiceCtrl>();
                dice1.SetParent(m_DiceContainer[0]);
                dice1.transform.localPosition = GameUtil.GetRandomPos(dice1.transform.position, 1f);
                coroutine = StartCoroutine(ctrl.RollAnimation(DiceA));
            }

            if (DiceB != 0)
            {
                GameObject dice2 = MahJongManager.Instance.SpawnDice();
                DiceCtrl ctrl2 = dice2.GetComponent<DiceCtrl>();
                dice2.SetParent(m_DiceContainer[1]);
                dice2.transform.localPosition = GameUtil.GetRandomPos(dice2.transform.position, 1f);
                coroutine = StartCoroutine(ctrl2.RollAnimation(DiceB));
            }
            yield return coroutine;
        }
        #endregion
        #endregion

        #region RecordReplay 战绩回放
        /// <summary>
        /// 战绩回放
        /// </summary>
        /// <returns></returns>
        private IEnumerator RecordReplay()
        {
            Begin(RoomMaJiangProxy.Instance.CurrentRoom, true, true);

            yield return new WaitForSeconds(10f);

            RecordReplayEntity entity = RecordProxy.Instance.CurrentReplayEntity;
            for (int i = 0; i < entity.record.Count; ++i)
            {
                IGameCommand command = null;
                RecordOperate operate = entity.record[i];
                switch (operate.operate)
                {
                    case OP_ROOM_GET_POKER.CODE:
                        command = new DrawPokerCommand(operate.playerId, operate.poker[0], operate.isLast, false, 0);
                        break;
                    case OP_ROOM_OPERATE.CODE:
                        command = new PlayPokerCommand(operate.playerId, operate.poker[0], false);
                        break;
                    case OP_ROOM_FIGHT.CODE:
                        command = new OperateCommand(operate.playerId, (OperatorType)operate.type, operate.subType, operate.poker, 0);
                        break;
                    case OP_ROOM_RANDOM_DICE.CODE:
                        command = new RollDiceCommand(operate.playerId, operate.type);
                        break;
                    case OP_ROOM_LUCK_POKER.CODE:
                        command = new LuckPokerCommand(new Poker(operate.poker[0].index, operate.poker[0].color, operate.poker[0].size));
                        break;
                    case OP_ROOM_CHANGE_LUCK.CODE:
                        command = new ChangeLuckPokerCommand(null, new Poker(operate.poker[0].index, operate.poker[0].color, operate.poker[0].size));
                        break;
                    case OP_ROOM_SHOW_LUCK.CODE:
                        command = new OperateCommand(operate.playerId, (OperatorType)operate.type, operate.subType, operate.poker, 0);
                        break;
                    case OP_ROOM_SWAP_SETTING.CODE:
                        command = new SetSwapPokerCommand(operate.playerId, operate.poker);
                        break;
                    case OP_ROOM_SWAP_BEGIN.CODE:
                        command = new SwapPokerCommand(operate.type, operate.poker);
                        break;
                }
                if (command != null)
                {
                    MaJiangGameCtrl.Instance.CommandQueue.Enqueue(command);
                }

                yield return new WaitForSeconds(1f);
            }
            StartCoroutine(PlaySettleAnimation(ShowSettle));
        }
        #endregion

        #region 摸宝相关
        #region PlayLuckPokerAnimation 播放翻宝（癞子）动画
        /// <summary>
        /// 播放翻宝（癞子）动画
        /// </summary>
        public void PlayLuckPokerAnimation(bool isPlayAnimation = true, DiceEntity dice = null)
        {
#if IS_LONGGANG
            if (RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker != null && RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker.color > 0)
            {
                Debug.LogWarning("几次阿卧槽");
                MaJiangCtrl majiang = MahJongManager.Instance.SpawnMaJiang(RoomMaJiangProxy.Instance.PlayerSeat.Pos, RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker, "Table");
                if (isPlayAnimation)
                {
                    majiang.transform.DOMove(CameraCtrl.Instance.CenterContainer.transform.position, 1.0f);
                    majiang.transform.DORotate(CameraCtrl.Instance.CenterContainer.transform.eulerAngles, 1.0f).OnComplete(() =>
                    {
                        MahJongManager.Instance.DespawnMaJiang(majiang);
                        UIItemMahJongRoomInfo.Instance.SetLuckPoker(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker);
                        GetSeatCtrlBySeatPos(RoomMaJiangProxy.Instance.PlayerSeat.Pos).SetUniversal(RoomMaJiangProxy.Instance.PlayerSeat.UniversalList);
                    });
                }
                else
                {
                    MahJongManager.Instance.DespawnMaJiang(majiang);
                    UIItemMahJongRoomInfo.Instance.SetLuckPoker(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker);
                    GetSeatCtrlBySeatPos(RoomMaJiangProxy.Instance.PlayerSeat.Pos).SetUniversal(RoomMaJiangProxy.Instance.PlayerSeat.UniversalList);
                }
            }

#elif IS_HONGHU
            if (isPlayAnimation)
            {
                if (RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker != null)
                {
                    MaJiangCtrl majiang = MahJongManager.Instance.GetWallFirstMahJong(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker);
                    majiang.transform.DOMove(CameraCtrl.Instance.CenterContainer.transform.position, 1.0f);
                    majiang.transform.DORotate(new Vector3(CameraCtrl.Instance.CenterContainer.transform.eulerAngles.x, CameraCtrl.Instance.CenterContainer.transform.eulerAngles.y, CameraCtrl.Instance.CenterContainer.transform.eulerAngles.z - 180), 1.0f).OnComplete(() =>
                    {
                        MahJongManager.Instance.DespawnMaJiang(majiang);
                        UIItemMahJongRoomInfo.Instance.SetLuckPoker(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker);
                    });
                }
            }
            else
            {
                UIItemMahJongRoomInfo.Instance.SetLuckPoker(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker);
            }

#elif IS_LEPING
            if (isPlayAnimation)
            {
                if (RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker != null && RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker.color > 0)
                {
                    MaJiangCtrl majiang = MahJongManager.Instance.HoldLuckPoker(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker);
                    Vector3 srcPos = majiang.transform.position;
                    Vector3 srcRot = majiang.transform.eulerAngles;

                    majiang.transform.DOMove(CameraCtrl.Instance.CenterContainer.transform.position, 1.0f);
                    majiang.transform.DORotate(CameraCtrl.Instance.CenterContainer.transform.eulerAngles, 1.0f).OnComplete(() =>
                    {
                        majiang.transform.position = srcPos;
                        majiang.transform.eulerAngles = new Vector3(srcRot.x, srcRot.y, srcRot.z + 180);
                        UIItemMahJongRoomInfo.Instance.SetLuckPoker(MahJongHelper.GetNextPoker(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker));
                        GetSeatCtrlBySeatPos(RoomMaJiangProxy.Instance.PlayerSeat.Pos).SetUniversal(RoomMaJiangProxy.Instance.PlayerSeat.UniversalList);
                    });
                }
            }
            else
            {
                if (RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker != null && RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker.color > 0)
                {
                    MaJiangCtrl majiang = MahJongManager.Instance.HoldLuckPoker(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker);
                    Vector3 srcRot = majiang.transform.eulerAngles;
                    majiang.transform.position = majiang.transform.position;
                    majiang.transform.eulerAngles = new Vector3(srcRot.x, srcRot.y, srcRot.z + 180);
                    UIItemMahJongRoomInfo.Instance.SetLuckPoker(MahJongHelper.GetNextPoker(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker));
                    GetSeatCtrlBySeatPos(RoomMaJiangProxy.Instance.PlayerSeat.Pos).SetUniversal(RoomMaJiangProxy.Instance.PlayerSeat.UniversalList);
                }
            }
#elif IS_GUGENG
            if (RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker != null && RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker.color > 0)
            {
                MaJiangCtrl majiang = MahJongManager.Instance.HoldLuckPoker_GuGeng(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker, (RoomMaJiangProxy.Instance.CurrentRoom.SecondDice.diceTotal - 1) * 2);

                if (isPlayAnimation)
                {
                    Vector3 srcPos = majiang.transform.position;
                    Vector3 srcRot = majiang.transform.eulerAngles;
                    majiang.transform.DOMove(CameraCtrl.Instance.CenterContainer.transform.position, 1.0f);
                    majiang.transform.DORotate(CameraCtrl.Instance.CenterContainer.transform.eulerAngles, 1.0f).OnComplete(() =>
                    {
                        majiang.transform.position = srcPos;
                        majiang.transform.eulerAngles = new Vector3(srcRot.x, srcRot.y, srcRot.z + 180);
                        UIItemMahJongRoomInfo.Instance.SetUniversal(RoomMaJiangProxy.Instance.PlayerSeat.UniversalList);
                        GetSeatCtrlBySeatPos(RoomMaJiangProxy.Instance.PlayerSeat.Pos).SetUniversal(RoomMaJiangProxy.Instance.PlayerSeat.UniversalList);
                    });
                }
                else
                {
                    Vector3 srcRot = majiang.transform.eulerAngles;
                    majiang.transform.eulerAngles = new Vector3(srcRot.x, srcRot.y, srcRot.z + 180);
                    UIItemMahJongRoomInfo.Instance.SetUniversal(RoomMaJiangProxy.Instance.PlayerSeat.UniversalList);
                    GetSeatCtrlBySeatPos(RoomMaJiangProxy.Instance.PlayerSeat.Pos).SetUniversal(RoomMaJiangProxy.Instance.PlayerSeat.UniversalList);
                }
            }
#elif IS_TAILAI
            if (dice == null) return;
            PlayChangeLuckPokerAnimation(new Poker(0, 0, 0), dice);
#elif IS_WANGQUE
            if (RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker != null && RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker.color > 0)
            {
                MaJiangCtrl majiang = MahJongManager.Instance.GetWallLastMahJong(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker);

                if (isPlayAnimation)
                {
                    Vector3 srcPos = majiang.transform.position;
                    Vector3 srcRot = majiang.transform.eulerAngles;
                    majiang.transform.DOMove(CameraCtrl.Instance.CenterContainer.transform.position, 1.0f);
                    majiang.transform.DORotate(CameraCtrl.Instance.CenterContainer.transform.eulerAngles, 1.0f).OnComplete(() =>
                    {
                        majiang.transform.position = srcPos;
                        majiang.transform.eulerAngles = new Vector3(srcRot.x, srcRot.y, srcRot.z + 180);
                        UIItemMahJongRoomInfo.Instance.SetLuckPoker(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker);
                        GetSeatCtrlBySeatPos(RoomMaJiangProxy.Instance.PlayerSeat.Pos).SetUniversal(RoomMaJiangProxy.Instance.PlayerSeat.UniversalList);
                    });
                }
                else
                {
                    Vector3 srcRot = majiang.transform.eulerAngles;
                    majiang.transform.eulerAngles = new Vector3(srcRot.x, srcRot.y, srcRot.z + 180);
                    UIItemMahJongRoomInfo.Instance.SetLuckPoker(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker);
                    GetSeatCtrlBySeatPos(RoomMaJiangProxy.Instance.PlayerSeat.Pos).SetUniversal(RoomMaJiangProxy.Instance.PlayerSeat.UniversalList);
                }
            }
#endif
        }
        #endregion

        #region RollDice 泰来摸宝摇骰子
        /// <summary>
        /// 泰来摸宝摇骰子
        /// </summary>
        /// <param name="dice"></param>
        /// <returns></returns>
        private IEnumerator RollDice(DiceEntity dice)
        {
            if (dice == null) yield break;
            int bankerPos = dice.seatPos;
            if (bankerPos == 2 && RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count == 2)
            {
                bankerPos = 3;
            }
            if (SystemProxy.Instance.HasHand)
            {
                string handPrefabName = "dicehand";
                string handPath = string.Format("download/{0}/prefab/model/{1}.drb", ConstDefine.GAME_NAME, handPrefabName);
                AssetBundleManager.Instance.LoadOrDownload(handPath, handPrefabName, (GameObject go) =>
                {
                    GameObject hand = Instantiate(go);
                    hand.SetParent(m_DiceHandContainer);
                    hand.transform.localEulerAngles = new Vector3(0, (bankerPos - 1) * -90f, 0);
                });

                yield return new WaitForSeconds(0.5f);
            }

            yield return StartCoroutine(RollDice(dice.seatPos,dice.diceA,dice.diceB));
        }
        #endregion

        #region PlayChangeLuckPokerAnimation 播放换宝动画
        /// <summary>
        /// 播放换宝动画
        /// </summary>
        public void PlayChangeLuckPokerAnimation(Poker poker, DiceEntity dice = null)
        {
            m_ListLuckPoker.Enqueue(poker);
            m_ListDice.Enqueue(dice);
#if IS_TAILAI
            StartCoroutine(PlayChangeLuckPokerAnimation_TaiLai());
#else
            StartCoroutine(PlayChangeLuckPokerAnimation());
#endif
        }
        #endregion

        #region PlayChangeLuckPokerAnimation 换宝动画
        /// <summary>
        /// 换宝动画
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlayChangeLuckPokerAnimation()
        {
            if (m_isPlaying) yield break;
            if (m_ListLuckPoker.Count <= 0)
            {
                yield break;
            }
            m_isPlaying = true;
            Poker poker = m_ListLuckPoker.Dequeue();
            UIItemMahJongRoomInfo.Instance.SetLuckPoker(poker);
            Debug.Log("换宝了");
            if (RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker != null)
            {
                MaJiangCtrl majiang = MahJongManager.Instance.GetWallLastMahJong(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker);

                majiang.transform.DOMove(CameraCtrl.Instance.CenterContainer.transform.position, 1.0f);
                majiang.transform.DORotate(new Vector3(CameraCtrl.Instance.CenterContainer.transform.eulerAngles.x, CameraCtrl.Instance.CenterContainer.transform.eulerAngles.y, CameraCtrl.Instance.CenterContainer.transform.eulerAngles.z - 180), 1.0f).OnComplete(() =>
                {
                    MahJongManager.Instance.DespawnMaJiang(majiang);
                    UIItemMahJongRoomInfo.Instance.SetLuckPoker(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker);
                    m_isPlaying = false;
                    StartCoroutine(PlayChangeLuckPokerAnimation());
                });
            }
        }
        #endregion

        #region PlayChangeLuckPokerAnimation_TaiLai 泰来换宝动画
        /// <summary>
        /// 泰来换宝动画
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlayChangeLuckPokerAnimation_TaiLai()
        {
            if (m_isPlaying) yield break;
            if (m_ListLuckPoker.Count <= 0)
            {
                yield break;
            }
            m_isPlaying = true;
            yield return new WaitForSeconds(1f);
            Poker poker = m_ListLuckPoker.Dequeue();
            DiceEntity dice = m_ListDice.Dequeue();
            if (dice == null) yield break;
            yield return StartCoroutine(RollDice(dice));
            UIItemMahJongRoomInfo.Instance.SetLuckPoker(poker);
            if (RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker != null)
            {
                MaJiangCtrl majiang = MahJongManager.Instance.HoldLuckPoker_TaiLai(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker, dice.diceA);
                majiang.transform.DOMove(CameraCtrl.Instance.CenterContainer.transform.position, 1.0f);
                majiang.transform.DORotate(new Vector3(CameraCtrl.Instance.CenterContainer.transform.eulerAngles.x, CameraCtrl.Instance.CenterContainer.transform.eulerAngles.y, CameraCtrl.Instance.CenterContainer.transform.eulerAngles.z - 180), 1.0f).OnComplete(() =>
                {
                    MahJongManager.Instance.DespawnMaJiang(majiang);
                    m_isPlaying = false;
                    UIItemMahJongRoomInfo.Instance.SetLuckPoker(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker);
                    StartCoroutine(PlayChangeLuckPokerAnimation_TaiLai());
                });
            }
        }
        #endregion
        #endregion

        #region 结算相关
        #region Settle 结算
        /// <summary>
        /// 结算
        /// </summary>
        /// <param name="pbRoom"></param>
        public void Settle(List<SeatEntity> lstWinerSeat)
        {
            UIItemTingTip.Instance.Close();
            UIItemMahJongRoomInfo.Instance.SetLuckPoker(RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker);

            if (lstWinerSeat != null)
            {
                for (int i = 0; i < lstWinerSeat.Count; ++i)
                {
                    MahJongHelper.Sort(lstWinerSeat[i].PokerList, lstWinerSeat[i].UniversalList, RoomMaJiangProxy.Instance.Rule.UniversalSortType);
                    GetSeatCtrlBySeatPos(lstWinerSeat[i].Pos).ShowSettle(lstWinerSeat[i].PokerList, lstWinerSeat[i].HitPoker);
                }
//#if !IS_GONGXIAN
//                if (lstWinerSeat.Count > 1)
//                {
//                    m_UISceneMaJiang3DView.PlayUIAnimation(UIAnimationType.UIAnimation_YiPaoDuoXiang);
//                }
//#endif
            }

            if (lstWinerSeat == null || lstWinerSeat.Count == 0)
            {
                m_UISceneMaJiang3DView.PlayUIAnimation(UIAnimationType.UIAnimation_LiuJu);
            }

            StartCoroutine(PlaySettleAnimation(ShowSettle));
        }
        #endregion

        #region PlaySettleAnimation 播放结算动画
        /// <summary>
        /// 播放结算动画
        /// </summary>
        /// <param name="onAnimationCpmplete"></param>
        /// <returns></returns>
        private IEnumerator PlaySettleAnimation(Action onAnimationCpmplete)
        {
            yield return StartCoroutine(PlaySettleEffect());

            yield return StartCoroutine(ProbAnimation());

            if (onAnimationCpmplete != null)
            {
                onAnimationCpmplete();
            }
        }
        #endregion

        #region PlaySettleEffect 结算特效动画
        /// <summary>
        /// 结算特效动画
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlaySettleEffect()
        {
            for (int i = 0; i < RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count; ++i)
            {
                if (RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i].isWiner)
                {
                    SeatEntity seat = RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i];
                    for (int j = 0; j < seat.SettleInfo.Count; ++j)
                    {
                        cfg_configEntity cfgEntity = cfg_configDBModel.Instance.Get(seat.SettleInfo[j].typeId);
                        if (cfgEntity == null) continue;

                        if (!string.IsNullOrEmpty(cfgEntity.effect3D) || !string.IsNullOrEmpty(cfgEntity.effect2D) || !string.IsNullOrEmpty(cfgEntity.soundEffect))
                        {
                            yield return new WaitForSeconds(1f);
                            //播放3D特效
                            if (!string.IsNullOrEmpty(cfgEntity.effect3D))
                            {
                                EffectManager.Instance.PlayEffectAsync(cfgEntity.effect3D, (Transform effect) =>
                                {
                                    effect.position = cfgEntity.effectStartPos.ToVector3();
                                    effect.localRotation = Quaternion.identity;
                                    if (!string.IsNullOrEmpty(cfgEntity.effectEndPos))
                                    {
                                        effect.DOMove(cfgEntity.effectEndPos.ToVector3(), cfgEntity.effectDuration).SetEase(Ease.Linear).SetAutoKill(true);
                                    }
                                }, cfgEntity.effectDuration);
                            }
                            //播放2D特效
                            if (!string.IsNullOrEmpty(cfgEntity.effect2D))
                            {
                                m_UISceneMaJiang3DView.PlayUIAnimation(cfgEntity.effect2D);
                            }
                            //播放音效
                            if (!string.IsNullOrEmpty(cfgEntity.soundEffect))
                            {
                                AudioEffectManager.Instance.Play(string.Format("{0}_{1}", seat.Gender.ToString(), cfgEntity.soundEffect), Vector3.zero, false);
                                AudioEffectManager.Instance.Play(string.Format("se_{0}", cfgEntity.soundEffect), Vector3.zero, false);
                            }

                            //yield return new WaitForSeconds(cfgEntity.effectDuration);
                        }
                    }
                }
            }
#if IS_HONGHU || IS_TAILAI
            yield return new WaitForSeconds(4f);
#else
            yield return new WaitForSeconds(2f);
#endif
            yield return null;
        }
        #endregion

        #region ProbAnimation 抓马动画
        /// <summary>
        /// 抓马动画
        /// </summary>
        private IEnumerator ProbAnimation()
        {
            List<Poker> prob = RoomMaJiangProxy.Instance.CurrentRoom.Prob;

            bool hasWiner = false;
            for (int i = 0; i < RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count; ++i)
            {
                SeatEntity seat = RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i];
                if (seat.isWiner)
                {
                    hasWiner = true;
                    break;
                }
            }

            cfg_settingEntity entity = RoomMaJiangProxy.Instance.GetConfigByTag("prob");
            if (entity != null && hasWiner)
            {
                int expectProbCount = entity.value;
                if ((prob == null && expectProbCount > 0) || (prob != null && expectProbCount > prob.Count))
                {
                    m_UISceneMaJiang3DView.PlayUIAnimation(UIAnimationType.UIAnimation_WuMa);
                    yield return new WaitForSeconds(1f);
                }
                if (expectProbCount == -1 && prob.Count == 0)
                {
                    m_UISceneMaJiang3DView.PlayUIAnimation(UIAnimationType.UIAnimation_WuMa);
                    yield return new WaitForSeconds(1f);
                }
            }


            //抓马
            if (prob == null || prob.Count == 0)
            {
                yield break;
            }

            List<MaJiangCtrl> lstProb = new List<MaJiangCtrl>(prob.Count);
            for (int i = 0; i < prob.Count; ++i)
            {
                MaJiangCtrl ctrl = MahJongManager.Instance.DrawMaJiang(0, prob[i], false, true, false);
                ctrl.gameObject.SetLayer("Table");
                ctrl.transform.localScale = Vector3.one;
                lstProb.Add(ctrl);
            }

            int probRule = entity == null ? 0 : entity.value;


            yield return new WaitForSeconds(0.5f);

            AudioEffectManager.Instance.Play("begin_prob", Vector3.zero, false);

            yield return new WaitForSeconds(2f);

            if (lstProb.Count == 1 && probRule == -1)
            {
                float angleY = (RoomMaJiangProxy.Instance.PlayerSeat.Pos - 1) * -90;
                if (RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count == 2 && RoomMaJiangProxy.Instance.PlayerSeat.Pos == 2)
                {
                    angleY -= 90;
                }
                m_EffectContainer.localEulerAngles = new Vector3(m_EffectContainer.localEulerAngles.x, angleY, m_EffectContainer.localEulerAngles.z);

                EffectManager.Instance.PlayEffectAsync("effect_prob", (Transform trans) =>
                {
                    trans.gameObject.SetParent(m_EffectContainer);
                });

                AudioEffectManager.Instance.Play("prob", Vector3.zero, false);
                lstProb[0].transform.SetParent(m_EffectContainer);
                lstProb[0].transform.localEulerAngles = Vector3.zero;
                lstProb[0].transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                lstProb[0].transform.position = m_EffectContainer.position - new Vector3(0, 10, 0);
                lstProb[0].transform.DOMove(m_EffectContainer.position + new Vector3(0, 10, 0), 1f).SetEase(Ease.InOutElastic);

#if IS_LAOGUI
                if ((lstProb[0].Poker.color == 5 && lstProb[0].Poker.size == 1) || (lstProb[0].Poker.color < 4 && lstProb[0].Poker.size >= 6))
                {
                    yield return new WaitForSeconds(3f);
                    string audioName = "10";
                    AudioEffectManager.Instance.Play(audioName, Vector3.zero, false);
                }
#else
                if ((lstProb[0].Poker.color == 5 && lstProb[0].Poker.size == 1) || (lstProb[0].Poker.color < 4 && lstProb[0].Poker.size == 1) || (lstProb[0].Poker.color < 4 && lstProb[0].Poker.size >= 6))
                {
                    yield return new WaitForSeconds(3f);
                    string audioName = "10";
                    AudioEffectManager.Instance.Play(audioName, Vector3.zero, false);
                }
#endif

            }
            else
            {
                for (int i = 0; i < prob.Count; ++i)
                {
                    lstProb[i].transform.DOMove(CameraCtrl.Instance.ProbContainers[i].transform.position, 1.0f);
                    lstProb[i].transform.DORotate(CameraCtrl.Instance.ProbContainers[i].transform.eulerAngles, 1.0f);
                    if (prob.Count > 1)
                    {
#if IS_WANGQUE
                        if ((lstProb[i].Poker.color < 4 && (lstProb[i].Poker.size == 1 || lstProb[i].Poker.size == 5 || lstProb[i].Poker.size == 9)))
                        {
                            lstProb[i].transform.DOScale(1.3f, 1f).SetLoops(6, LoopType.Yoyo);
                        }
                        if (RoomMaJiangProxy.Instance.PlayerSeat.UniversalList != null && lstProb[i].Poker.color == 5 && lstProb[i].Poker.size == 1)
                        {
                            lstProb[i].transform.DOScale(1.3f, 1f).SetLoops(6, LoopType.Yoyo);
                        }
#endif
                        yield return new WaitForSeconds(1.0f);
                    }
                }
            }

            yield return new WaitForSeconds(3f);
        }
        #endregion

        #region ShowSettle 显示结算信息
        /// <summary>
        /// 显示结算信息
        /// </summary>
        private void ShowSettle()
        {
            if (RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Settle 
                || RoomMaJiangProxy.Instance.CurrentRoom.isReplay 
                || RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Ready)
            {
                UIItemMahJongRoomInfo.Instance.SetLuckPoker(null);
                UIItemMahJongRoomInfo.Instance.SetUniversal(null);
                AudioEffectManager.Instance.Play(RoomMaJiangProxy.Instance.PlayerSeat.isWiner ? "win" : "lose", Vector3.zero, false);
                RoomMaJiangProxy.Instance.SendRoomInfoChangeNotify();
                UIViewManager.Instance.OpenWindow(UIWindowType.Settle);
            }
        }
        #endregion
        #endregion

        #region PlaySwapAnimation 播放交换牌动画
        /// <summary>
        /// 播放交换牌动画
        /// </summary>
        public void PlaySwapAnimation(int swapType)
        {
            int seatCount = RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count;
            if (seatCount == 2) swapType = 2;
            for (int i = 0; i < m_Seats.Length; ++i)
            {
                SeatEntity seat = RoomMaJiangProxy.Instance.GetSeatBySeatId(m_Seats[i].SeatPos);
                if (seat == null) continue;
                int angles = 0;
                switch (swapType)
                {
                    case 0:
                        angles = 90;
                        if (seatCount == 3 && seat.Pos == 1)
                        {
                            angles *= 2;
                        }
                        break;
                    case 1:
                        angles = -90;
                        if (seatCount == 3 && seat.Pos == 3)
                        {
                            angles *= 2;
                        }
                        break;
                    case 2:
                        angles = 180;
                        break;
                }

                m_Seats[i].PlaySwapAnimation(angles, seat.SwapPoker);
            }
        }
        #endregion


        #region OnPlayerClick 玩家点击
        /// <summary>
        /// 玩家点击
        /// </summary>
        protected override void OnPlayerClick()
        {
            base.OnPlayerClick();

            //如果点击的是UI 不做处理
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = CameraCtrl.Instance.CurrentHandPokerCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit[] hitArr = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("PlayerHand"));
            if (hitArr.Length > 0)
            {
                MaJiangCtrl ctrl = hitArr[0].collider.gameObject.GetComponent<MaJiangCtrl>();
                if (ctrl == null) return;
                AudioEffectManager.Instance.Play("dianpai", Vector3.zero, false);


                for (int q = 0; q < RoomMaJiangProxy.Instance.PlayerSeat.HoldPoker.Count; ++q)
                {
                    if (MahJongHelper.HasPoker(ctrl.Poker, RoomMaJiangProxy.Instance.PlayerSeat.HoldPoker[q]))
                    {
                        List<MaJiangCtrl> hand = MahJongManager.Instance.GetHand(RoomMaJiangProxy.Instance.PlayerSeat.Pos);
                        for (int i = 0; i < hand.Count; ++i)
                        {
                            if (MahJongHelper.ContainPoker(hand[i].Poker, RoomMaJiangProxy.Instance.PlayerSeat.HoldPoker[q]))
                            {
                                hand[i].Hold(true, false);
                            }
                        }
                        return;
                    }
                }

                MaJiangGameCtrl.Instance.OnPokerClick(ctrl);
                return;
            }

            ray = CameraCtrl.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);

            hitArr = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Table"));
            if (hitArr.Length > 0)
            {
                MaJiangCtrl ctrl = hitArr[0].collider.gameObject.GetComponent<MaJiangCtrl>();
                if (ctrl == null) return;
                AudioEffectManager.Instance.Play("dianpai", Vector3.zero, false);

                for (int q = 0; q < RoomMaJiangProxy.Instance.PlayerSeat.UsedPokerList.Count; ++q)
                {
                    if (RoomMaJiangProxy.Instance.PlayerSeat.UsedPokerList[q].CombinationType == OperatorType.Kou)
                    {
                        if (MahJongHelper.ContainPoker(ctrl.Poker, RoomMaJiangProxy.Instance.PlayerSeat.UsedPokerList[q].PokerList))
                        {
                            List<MaJiangCtrl> lst = MahJongManager.Instance.GetUsedPoker(ctrl);
                            if (lst != null)
                            {
                                for (int i = 0; i < lst.Count; ++i)
                                {
                                    lst[i].transform.localEulerAngles += new Vector3(180f, 0f, 0f);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region OnFingerBeginDrag 手势拖拽开始
        /// <summary>
        /// 手势拖拽开始
        /// </summary>
        protected override void OnFingerBeginDrag()
        {
            base.OnFingerBeginDrag();

            //如果点击的是UI 不做处理
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = CameraCtrl.Instance.CurrentHandPokerCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit[] hitArr = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("PlayerHand"));
            if (hitArr.Length > 0)
            {
                MaJiangCtrl ctrl = hitArr[0].collider.gameObject.GetComponent<MaJiangCtrl>();
                if (ctrl != null)
                {
                    if (m_Temp != null)
                    {
                        MahJongManager.Instance.DespawnMaJiang(m_Temp);
                        m_Temp = null;
                    }

                    if (MahJongHelper.ContainPoker(ctrl.Poker, RoomMaJiangProxy.Instance.PlayerSeat.DingJiangPoker)) return;
                    for (int i = 0; i < RoomMaJiangProxy.Instance.PlayerSeat.HoldPoker.Count; ++i)
                    {
                        if (MahJongHelper.ContainPoker(ctrl.Poker, RoomMaJiangProxy.Instance.PlayerSeat.HoldPoker[i])) return;
                    }

                    for (int q = 0; q < RoomMaJiangProxy.Instance.PlayerSeat.HoldPoker.Count; ++q)
                    {
                        if (MahJongHelper.HasPoker(ctrl.Poker, RoomMaJiangProxy.Instance.PlayerSeat.HoldPoker[q]))
                        {
                            return;
                        }
                    }
                    m_DragBeginPos = ctrl.transform.position;
                    m_Temp = MahJongManager.Instance.SpawnMaJiang(RoomMaJiangProxy.Instance.PlayerSeat.Pos, ctrl.Poker, "PlayerHand");
                    m_Temp.Init(ctrl.Poker, false);
                    m_Temp.gameObject.SetLayer(LayerMask.NameToLayer("PlayerHand"));
                    m_Temp.transform.position = ctrl.transform.position;
                    m_Temp.transform.rotation = ctrl.transform.rotation;
                    m_Temp.transform.localScale = ctrl.transform.lossyScale;
                }
            }
        }
        #endregion

        #region OnFingerDrag 手势拖拽中
        /// <summary>
        /// 手势拖拽中
        /// </summary>
        /// <param name="screenPos"></param>
        protected override void OnFingerDrag(Vector2 screenPos)
        {
            base.OnFingerDrag(screenPos);
            if (m_Temp == null) return;
            Camera camera = CameraCtrl.Instance.CurrentHandPokerCamera;
            bool isX = RoomMaJiangProxy.Instance.PlayerSeat.Pos % 2 == 1;
            Vector3 del = camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));
            m_Temp.transform.position = new Vector3(del.x, del.y, del.z);
        }
        #endregion

        #region OnFingerEndDrag 手势拖拽结束
        /// <summary> 
        /// 手势拖拽结束
        /// </summary>
        protected override void OnFingerEndDrag()
        {
            base.OnFingerEndDrag();

            if (m_Temp == null) return;
            if (m_Temp.transform.position.y - m_DragBeginPos.y > 3f)
            {
                MaJiangGameCtrl.Instance.ClientSendPlayPoker(m_Temp.Poker);
                UIItemTingTip.Instance.Show(m_Temp, RoomMaJiangProxy.Instance.GetHu(m_Temp.Poker));
            }
            m_DragBeginPos = Vector3.zero;

            if (m_Temp != null)
            {
                MahJongManager.Instance.DespawnMaJiang(m_Temp);
                m_Temp = null;
            }
        }
        #endregion
    }
}
