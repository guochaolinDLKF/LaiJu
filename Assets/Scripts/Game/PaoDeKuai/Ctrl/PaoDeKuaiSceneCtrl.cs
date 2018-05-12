//===================================================
//Author      : WZQ
//CreateTime  ：11/13/2017 3:58:39 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PaoDeKuai
{
    public class PaoDeKuaiSceneCtrl : SceneCtrlBase
    {

        #region Singleton
        public static PaoDeKuaiSceneCtrl Instance;
        #endregion

        #region Variable
        private UIScenePaoDeKuaiView m_UIScenePaoDeKuaiView;//场景UI
        //[SerializeField]
        //private SeatCtrl[] m_Seats;//座位

           

        [SerializeField]
        private Transform m_EffectContainer;//特效挂载点


        //private Vector2 m_DragBeginPos;//拖拽开始坐标点

        /// <summary>
        /// 发牌动画时间
        /// </summary>
        private const float DEAL_ANIMATION_DURATION = 0.1f;



        private bool m_isBegining;//是否正在播放开局动画

        #endregion

        #region Override MonoBehaviour
        protected override void OnAwake()
        {
            base.OnAwake();

            Instance = this;

            //加载完毕回调
            if (DelegateDefine.Instance.OnSceneLoadComplete != null)
            {
                DelegateDefine.Instance.OnSceneLoadComplete();
            }

            AudioBackGroundManager.Instance.Play("bgm_paodekuai");



        }

        private void OnApplicationPause(bool isPause)
        {
            if (!isPause)
            {
                if (!NetWorkSocket.Instance.Connected(GameCtrl.Instance.SocketHandle))
                {
                    //if (RoomMaJiangProxy.Instance.CurrentRoom.Status != RoomEntity.RoomStatus.Replay)
                    //{
                    GameCtrl.Instance.RebuildRoom();
                    //}
                }
            }

        }

        private void OnApplicationFocus(bool focus)
        {
            //焦点切换
            //MaJiangGameCtrl.Instance.ClientSendFocus(focus);
        }

        protected override void OnStart()
        {
            base.OnStart();

            GameObject go = UIViewManager.Instance.LoadSceneUIFromAssetBundle(UIViewManager.SceneUIType.PaoDeKuai2D);
            m_UIScenePaoDeKuaiView = go.GetComponent<UIScenePaoDeKuaiView>();
            //m_AI = new GameMahJongAI();

            //初始化管理器
            PrefabManager.Instance.Init();
            Init();
        }

        protected override void BeforeOnDestroy()
        {
            base.BeforeOnDestroy();

            PrefabManager.Instance.Dispose();
        }


        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (PaoDeKuaiGameCtrl.Instance.CommandQueue.Count > 0)
            {
                IGameCommand command = PaoDeKuaiGameCtrl.Instance.CommandQueue.Dequeue();
                command.Execute();
            }
            if (!m_IsSelectedPoker && Input.GetMouseButtonDown(0))
            {
                StartSelectedPoker();
            }
            if (m_IsSelectedPoker && Input.GetMouseButtonUp(0))
            {
                EndSelectedPoker();
            }
        }
        #endregion

        private void Init()
        {
            ResetDragPoker();
            //for (int i = 0; i < m_Seats.Length; ++i)
            //{
            //    m_Seats[i].Init(RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count);
            //}

            //string param = string.Empty;
            //cfg_settingEntity limit = RoomPaoDeKuaiProxy.Instance.GetConfigByTag("limit");
            //if (limit != null)
            //{
            //    param = string.Format("{0}:{1}", limit.label, limit.name);
            //}
            UIItemPaoDeKuaiRoomInfo.Instance.SetUI(RoomPaoDeKuaiProxy.Instance.CurrentRoom.roomId, RoomPaoDeKuaiProxy.Instance.CurrentRoom.BaseScore);

            //UIItemMahJongRoomInfo.Instance.SetRoomConfig(RoomMaJiangProxy.Instance.CurrentRoom.Config);

            //CompassCtrl.Instance.SetNormal();

            //CameraCtrl.Instance.SetPokerTotal(RoomMaJiangProxy.Instance.CurrentRoom.PokerTotalPerPlayer);

            RoomPaoDeKuaiProxy.Instance.SendRoomInfoChangeNotify();


            //if (RoomMaJiangProxy.Instance.PlayerSeat == null)
            //{
            //    AppDebug.ThrowError("玩家座位是空的，玩个毛线");
            //}

            //CameraCtrl.Instance.SetPos(RoomMaJiangProxy.Instance.PlayerSeat.Pos, RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count);

            if (RoomPaoDeKuaiProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Replay)
            {
                //StartCoroutine(RecordReplay());
                //return;
            }
            else
            {
                ReBuildRoom(RoomPaoDeKuaiProxy.Instance.CurrentRoom);
            }

            //if (RoomMaJiangProxy.Instance.CurrentRoom.matchId > 0)
            //{
            //    MaJiangGameCtrl.Instance.ClientSendReady();
            //}


        }

        #region Rebuild 重建房间
        /// <summary>
        /// 重建房间
        /// </summary>
        /// <param name="room"></param>
        private void ReBuildRoom(RoomEntity room)
        {
            Debug.Log(RoomPaoDeKuaiProxy.Instance.CurrentRoom.Status);
            if (RoomPaoDeKuaiProxy.Instance.CurrentRoom.Status != RoomEntity.RoomStatus.Ready && RoomPaoDeKuaiProxy.Instance.CurrentRoom.Status != RoomEntity.RoomStatus.Settle)
            {
              
                Begin(room, false, false);
            }

            //====================确认当前游戏状态=====================
            for (int i = 0; i < room.SeatList.Count; ++i)
            {
                //if (room.SeatList[i].Status == SeatEntity.SeatStatus.Fight && room.SeatList[i].Pos == RoomMaJiangProxy.Instance.PlayerSeat.Pos)
                //{

                //    if (RoomMaJiangProxy.Instance.AskPokerGroup != null && RoomMaJiangProxy.Instance.AskPokerGroup.Count > 0)
                //    {
                //        IGameCommand command = new AskOperateCommand(RoomMaJiangProxy.Instance.AskPokerGroup, 0);
                //        MaJiangGameCtrl.Instance.CommandQueue.Enqueue(command);
                //    }

                //}

                if (room.SeatList[i].DisbandState == DisbandState.Agree)
                {
                    PaoDeKuaiGameCtrl.Instance.OpenDisbandView();
                    break;
                }
            }

            RoomPaoDeKuaiProxy.Instance.SendRoomInfoChangeNotify();
        }

        #endregion



        #region 
        #endregion
        #region Begin 开局

        /// <summary>
        /// 开局
        /// </summary>
        /// <param name="room">房间信息</param>
        /// <param name="isPlayAnimation">是否播放动画</param>
        /// <param name="isReplay">是否是回放</param>
        public void Begin(RoomEntity room, bool isPlayAnimation, bool isReplay)
        {

            //加载牌墙
           List<PokerCtrl>  wall =  PrefabManager.Instance.Rebuild(RoomPaoDeKuaiProxy.Instance.CurrentRoom.PokerTotal);

            //发牌
            for (int i = 0; i < room.SeatCount; i++)
            {
                if (room.SeatList[i] == null || room.SeatList[i].PlayerId == 0) continue;

                Debug.Log(string.Format("DrawMaJiang  Pos:{0}  Count:{1}", room.SeatList[i].Pos, room.SeatList[i].pokerList.Count));

                bool isPlayer = room.SeatList[i] == RoomPaoDeKuaiProxy.Instance.PlayerSeat;
                for (int j = 0; j < room.SeatList[i].pokerList.Count; j++)
                {
                    PrefabManager.Instance.DrawPoker(room.SeatList[i].Pos, room.SeatList[i].pokerList[j], isPlayer,true);
                }
                PrefabManager.Instance.SortHandPokers(room.SeatList[i].Pos);
            }



            m_UIScenePaoDeKuaiView.Begin(room.Status, room.SeatList, isPlayAnimation, RoomPaoDeKuaiProxy.Instance.GetSeatBySeatPos(room.SpadesThreePos),
                () =>
                {

                    SeatEntity seat = RoomPaoDeKuaiProxy.Instance.GetSeatBySeatPos(room.CurrAlreadyPlayPos);
                    if (seat != null)
                    {
                        PaoDeKuaiGameCtrl.Instance.MockBroadcastPlayPoker(room.CurrAlreadyPlayPos, room.RecentlyPlayPoker.Pokers);

                    }
                    for (int i = 0; i < room.SeatList.Count; i++)
                    {
                        if (room.SeatList[i].Status == SeatEntity.SeatStatus.Operate)
                        {
                            //判断当前操作座位让其出牌
                            PaoDeKuaiGameCtrl.Instance.MockBroadcastSeatOperate(room.SeatList[i].Pos);
                            break;
                        }
                    }


                }
                );



        }
        #endregion



        #region PlayPoker 出牌
        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="pokers"></param>
        public void PlayPoker(SeatEntity seat,List<Poker> pokers, PokersType pokerType)
        {

            List<PokerCtrl> playPoker= PrefabManager.Instance.PlayPokers(seat.Pos,pokers, seat == RoomPaoDeKuaiProxy.Instance.PlayerSeat );

            m_UIScenePaoDeKuaiView.PlayPokers(seat.Index,  playPoker,  pokerType);


        }
        #endregion


        #region  SeatOperateNotice   通知座位操作      
        /// <summary>
        /// 通知座位操作
        /// </summary>
        /// <param name="pos"></param>
        public void SeatOperateNotice(int pos)
        {
            if (pos==RoomPaoDeKuaiProxy.Instance.CurrentRoom.CurrAlreadyPlayPos)
            {
                //移除已经出的牌
                PrefabManager.Instance.ClearCurrPlayPoker();
               

            }
        }
        #endregion



        #region Pass 过
        /// <summary>
        /// 过
        /// </summary>
        /// <param name="Pos"></param>
        public void Pass(int Pos)
        {
            //m_UISceneMaJiang3DView
        }
        #endregion

        /// <summary>
        /// 更具玩家操作反馈信息
        /// </summary>
        /// <param name="operateFeedbackType"></param>
        public void OperateFeedback(OperateFeedbackType operateFeedbackType)
        {
            m_UIScenePaoDeKuaiView.playerOperateFeedback(operateFeedbackType);

        }

        #region 
        #endregion


        #region     Hint   提示
        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="pokers"></param>
        public void Hint(List<Poker> pokers)
        {
            if (pokers == null || pokers.Count == 0)
            {
                m_UIScenePaoDeKuaiView.playerOperateFeedback( OperateFeedbackType.NoBigPoker);
                return;
            }
            m_CurrLiftUpPoker.Clear();
            List<PokerCtrl> handPokes = PrefabManager.Instance.GetHand(RoomPaoDeKuaiProxy.Instance.PlayerSeat.Pos);

            for (int i = 0; i < handPokes.Count; ++i)
            {
                handPokes[i].SetHold(false);
            }

            for (int i = 0; i < pokers.Count; ++i)
            {
                for (int j = 0; j < handPokes.Count; ++j)
                {
                    if (pokers[i].index == handPokes[j].Poker.index)
                    {
                        
                        handPokes[j].SetHold(true);
                        m_CurrLiftUpPoker.Add(pokers[i]);
                        break;
                    }
                }

            }
        }
        #endregion

        #region Settle 本局结算
        /// <summary>
        /// 本局结算
        /// </summary>
        public void Settle()
        {
            ////摊开 全部手牌
            //List<SeatEntity> seatlist = RoomPaoDeKuaiProxy.Instance.CurrentRoom.SeatList;
            //for (int i = 0; i < seatlist.Count; ++i)
            //{
            //    if (seatlist[i] == RoomPaoDeKuaiProxy.Instance.PlayerSeat) continue;

            //    List<PokerCtrl>  pokers = PrefabManager.Instance.GetHand(seatlist[i].Pos);
            //    m_UISceneMaJiang3DView
            //}


            StartCoroutine(SettleAni());

            //m_UIScenePaoDeKuaiView.Settle(RoomPaoDeKuaiProxy.Instance.CurrentRoom.SeatList,
            //    () => {
            //        PaoDeKuaiGameCtrl.Instance.OpenView(UIWindowType.Settle_PaoDeKuai);
            //    }
            //    );
        }

        IEnumerator SettleAni()
        {
            yield return StartCoroutine(m_UIScenePaoDeKuaiView.Settle(RoomPaoDeKuaiProxy.Instance.CurrentRoom.SeatList,null));
            PaoDeKuaiGameCtrl.Instance.OpenView(UIWindowType.Settle_PaoDeKuai);
            RoomPaoDeKuaiProxy.Instance.RoomStatusChangeNotify( RoomEntity.RoomStatus.Ready);
        }


        #endregion



        #region NextLoopGame 下一局
        /// <summary>
        /// 下一局
        /// </summary>
        public void NextLoopGame()
        {
            PrefabManager.Instance.ClearTable();
        }
        #endregion





        private PokerCtrl m_StartSelectedPoker;//初始选中的牌
        private PokerCtrl m_EndSelectedPoker; //结束选中的牌
        private bool m_IsSelectedPoker;
        private List<PokerCtrl> m_CurrSelectedPoker = new List<PokerCtrl>();//当前拖拽选中的牌
        private List<Poker> m_CurrLiftUpPoker = new List<Poker>();//当前抬起的牌
        public List<Poker> CurrLiftUpPoker { get { return m_CurrLiftUpPoker; } }
        #region OnFingerBeginDrag 手势拖拽开始
        /// <summary>
        /// 手势拖拽开始
        /// </summary>
        protected override void OnFingerBeginDrag()
        {
            base.OnFingerBeginDrag();

            Debug.Log("OnFingerBeginDrag 手势拖拽开始");
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
            Debug.Log("OnFingerDrag 手势拖拽中");

            MiddleSelectedPoker();
        }
        #endregion

        #region OnFingerEndDrag 手势拖拽结束
        /// <summary> 
        /// 手势拖拽结束
        /// </summary>
        protected override void OnFingerEndDrag()
        {
            base.OnFingerEndDrag();
            Debug.Log("OnFingerEndDrag 手势拖拽结束");

        }
        #endregion

        #region SelectedPoker 选牌
        //开始选牌
        private void StartSelectedPoker()
        {
            
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                Ray ray = m_UIScenePaoDeKuaiView.CurrentCamare.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Debug.Log("-------------Ray ray -----------------");
                    if (hit.transform.gameObject.layer != 1 << LayerMask.NameToLayer("PlayerHand")) return;
                }
                PokerCtrl ctrl = GetRayHitPoker();
                if (ctrl != null)
                {
                    //ResetDragPoker();

                    m_EndSelectedPoker = null; //结束选中的牌
                    m_CurrSelectedPoker.Clear(); //当前拖拽选中的牌
                    //获得第一张牌
                    m_StartSelectedPoker = ctrl;
                    m_CurrSelectedPoker.Add(ctrl);
                    m_IsSelectedPoker = true;
                    ctrl.SetSelected(true);
                }
            
        }

        //选牌中
        private void MiddleSelectedPoker()
        {
            if (m_IsSelectedPoker && m_StartSelectedPoker != null)
            {
                //计算拖拽时选中的牌
                PokerCtrl ctrl = GetRayHitPoker();
                if (ctrl != null)
                {
                    m_CurrSelectedPoker.Clear();
                    m_EndSelectedPoker = ctrl;

                    int startIndex = PrefabManager.Instance.GetIndex(RoomPaoDeKuaiProxy.Instance.PlayerSeat.Pos, m_StartSelectedPoker);
                    int endIndex = PrefabManager.Instance.GetIndex(RoomPaoDeKuaiProxy.Instance.PlayerSeat.Pos, m_EndSelectedPoker);

                    List<PokerCtrl> playerSeatPoker = PrefabManager.Instance.GetHand(RoomPaoDeKuaiProxy.Instance.PlayerSeat.Pos);
                    int minIndex = endIndex - startIndex > 0 ? startIndex : endIndex;
                    int maxIndex = endIndex + startIndex - minIndex;
                    for (int i = minIndex; i <= maxIndex; ++i)
                    {
                        m_CurrSelectedPoker.Add(playerSeatPoker[i]);

                    }
                    //设置拖拽选中状态
                    for (int i = 0; i < playerSeatPoker.Count; ++i)
                    {
                        playerSeatPoker[i].SetSelected(false);
                    }
                    for (int i = 0; i < m_CurrSelectedPoker.Count; ++i)
                    {
                        m_CurrSelectedPoker[i].SetSelected(true);
                    }

                }


            }
        }

        //结束选牌
        private void EndSelectedPoker()
        {
           
                for (int i = 0; i < m_CurrSelectedPoker.Count; ++i)
                {
                    m_CurrSelectedPoker[i].SetSelected(false);
                }

                List<Poker> selectedPoker = new List<Poker>();
                for (int i = 0; i < m_CurrSelectedPoker.Count; ++i)
                {
                    selectedPoker.Add(m_CurrSelectedPoker[i].Poker);
                }
 
                List<Poker> straightInSelected = PaoDeKuaiHelper.GetPokersInStraight(selectedPoker);
                List<PokerCtrl> playerSeatPoker = PrefabManager.Instance.GetHand(RoomPaoDeKuaiProxy.Instance.PlayerSeat.Pos);

                if (straightInSelected != null)
                {
                    for (int i = 0; i < playerSeatPoker.Count; ++i)
                    {
                        playerSeatPoker[i].SetHold(PaoDeKuaiHelper.PokerIsInList(playerSeatPoker[i].Poker, straightInSelected));

                    }
                }
                else
                {
                    for (int i = 0; i < m_CurrSelectedPoker.Count; ++i)
                    {
                        m_CurrSelectedPoker[i].SetHold();
                    }
                }

                ResetDragPoker();
                for (int i = 0; i < playerSeatPoker.Count; ++i)
                {
                    if (playerSeatPoker[i].IsHold) m_CurrLiftUpPoker.Add(playerSeatPoker[i].Poker);
                }

        }
        private PokerCtrl GetRayHitPoker()
        {
            PokerCtrl ctrl = null;
            Vector3 ScreenmousePos = m_UIScenePaoDeKuaiView.CurrentCamare.ScreenToWorldPoint(Input.mousePosition);
       
            RaycastHit2D[] hitArr = Physics2D.RaycastAll(new Vector2(ScreenmousePos.x, ScreenmousePos.y), new Vector2(ScreenmousePos.x, ScreenmousePos.y-1f ), 1, 1 << LayerMask.NameToLayer("PlayerHand"));
  
            if (hitArr.Length > 0)
            {
                ctrl = hitArr[0].collider.gameObject.GetComponent<PokerCtrl>();
             
                for (int i = 1; i < hitArr.Length; ++i)
                {
                   PokerCtrl ctrlTemp=  hitArr[i].collider.gameObject.GetComponent<PokerCtrl>();
                  
                    if (ctrl.Poker.size > ctrlTemp.Poker.size ||(ctrl.Poker.size == ctrlTemp.Poker.size&& ctrl.Poker.color > ctrlTemp.Poker.color))
                    {
                        ctrl = ctrlTemp;
                    }
                }
            }
            return ctrl;

        }
       
        private void ResetDragPoker()
        {
            m_StartSelectedPoker = null;//初始选中的牌
            m_EndSelectedPoker = null; //结束选中的牌
            m_IsSelectedPoker = false;
            m_CurrSelectedPoker.Clear(); //当前拖拽选中的牌
            m_CurrLiftUpPoker.Clear(); //当前抬起的牌

        }
        #endregion
    }
}