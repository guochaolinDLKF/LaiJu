//===================================================
//Author      : CZH
//CreateTime  ：7/25/2017 1:56:40 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ddz.proto;
using System;
using DG.Tweening;

namespace DRB.DouDiZhu
{
    public class UISceneDouDZView : UISceneGameViewBase
    {
        public static UISceneDouDZView Instance;
        [SerializeField]
        private Button m_btnReady;//准备按钮
        [SerializeField]
        private Button m_btnShowPokerReady;//明牌准备按钮
        [SerializeField]
        private GameObject m_PlayPokerMenu;
        [SerializeField]
        private Image m_imgCantPlayPoker;
        [SerializeField]
        private Image m_imgPlayPoker;//出牌的图片
        [SerializeField]
        private Image m_imgPass;//过的图片
        [SerializeField]
        private Image m_imgTip;//提示的图片
        [SerializeField]
        private Image m_imgTrustee;//托管的图片
        [SerializeField]
        private Image m_imgShowPoker;//明牌的图片
        [SerializeField]
        private Text m_txtBaseScore;//底分
        [SerializeField]
        private Text m_txtTimes;//倍数
        [SerializeField]
        private Text m_txtRoomId;//房间Id
        [SerializeField]
        private Text m_txtLoop;//局数
        [SerializeField]
        private Button[] m_arrScore;
        [SerializeField]
        private Image[] m_arrCantScore;
        [SerializeField]
        private Button m_btnYaobuqi;//要不起的按钮
        [SerializeField]
        private Transform m_BasePokerContainer;
        [SerializeField]
        private Transform m_AnimationContainer;//动画挂载点
        [SerializeField]
        private UIItemSeat[] lstUIItemSeat;
        [SerializeField]
        private DOTweenAnimation springAnimation;

        private UISettleView m_UISettleView;

        private UIResultView_DouDZ m_UIResultView;

        private List<UIItemPoker> m_BasePokerList = new List<UIItemPoker>();

        private int m_loop;
        private int m_maxLoop;

        /// <summary>
        /// 开始拖拽牌的时候
        /// </summary>
        public bool isCanUsePoingerHandler;
        protected override void OnAwake()
        {
            base.OnAwake();
            Instance = this;
            m_btnYaobuqi.onClick.AddListener(OnPassClick);
        }

        private void OnPassClick()
        {
            SendNotification("btnDouDiZhuViewPass");
        }

        public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
        {
            Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
            dic.Add(RoomProxy.ON_INIT, OnInit);
            dic.Add(RoomProxy.ON_READY, OnReady);
            dic.Add(RoomProxy.ON_ROOM_INFO_CHANGED, OnRoomInfoChanged);
            dic.Add(RoomProxy.ON_ASK_BET, OnAskBet);
            dic.Add(RoomProxy.ON_SEAT_BET, OnBet);
            dic.Add(RoomProxy.ON_SET_BANKER, OnSetBanker);
            dic.Add(RoomProxy.ON_ASK_PLAY_POKER, OnAskPlayPoker);
            dic.Add(RoomProxy.ON_PLAY_POKER, OnPlayPoker);
            dic.Add(RoomProxy.ON_SEAT_PASS, OnPass);
            dic.Add(RoomProxy.ON_BEGIN, OnBegin);
            dic.Add(RoomProxy.ON_PLUS_TIMES, OnPlusTimes);
            dic.Add(RoomProxy.ON_TRUSTEE, OnTrustee);
            dic.Add(RoomProxy.ON_SETTLE, OnSeatSettle);
            dic.Add(RoomProxy.ON_RESULT,OnSeatResult);
            return dic;
        }

        protected override void OnBtnClick(GameObject go)
        {
            base.OnBtnClick(go);
            switch (go.name)
            {
                case "btnDouDiZhuViewReady":
                    SendNotification("btnDouDiZhuViewReady");
                    break;
                case "btnDouDiZhuViewPlayPoker"://出牌
                    SendNotification("btnDouDiZhuViewPlayPoker");
                    break;
                case "btnDouDiZhuViewPass"://过
                    SendNotification("btnDouDiZhuViewPass");
                    break;
                case "btnDouDiZhuViewTip"://提示
                    SendNotification("btnDouDiZhuViewTip");
                    break;
                case "btnDouDiZhuViewTrustee"://托管
                    SendNotification("btnDouDiZhuViewTrustee");
                    break;
                case "btnDouDiZhuCancelTrustee"://取消托管
                    SendNotification("btnDouDiZhuCancelTrustee");
                    break;
                case "btnDouDiZhuViewShowPoker"://明牌开始
                    SendNotification("btnDouDiZhuViewShowPoker");
                    SendNotification("btnDouDiZhuViewReady");
                    break;
            }

            for (int i = 0; i < m_arrScore.Length; ++i)
            {
                if (go == m_arrScore[i].gameObject)
                {
                    SendNotification("btnDouDiZhuViewBet", go.name.ToInt());
                }
            }
        }

        #region OnInit 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="obj"></param>
        private void OnInit(TransferData data)
        {
            RoomEntity room = data.GetValue<RoomEntity>("Room");
            if (room == null) return;

            for (int i = 0; i < m_BasePokerList.Count; ++i)
            {
                UIPoolManager.Instance.Despawn(m_BasePokerList[i].transform);
            }
            m_BasePokerList.Clear();

            for (int i = 0; i < room.basePoker.Count; ++i)
            {
                UIItemPoker item = SpawnPoker(room.basePoker[i]);
                item.gameObject.SetParent(m_BasePokerContainer);
                m_BasePokerList.Add(item);
            }

            for (int i = 0; i < room.SeatList.Count; ++i)
            {
                if (room.SeatList[i].IsPlayer)
                {
                    bool mustPlay = data.GetValue<bool>("MustPlay");
                    bool canPlay = data.GetValue<bool>("CanPlay");
                    SetPlayPokerMenuActive(room.SeatList[i].status == SeatEntity.SeatStatus.PlayPoker, !mustPlay, !canPlay);
                    SetBetMenuActive(room.SeatList[i].status == SeatEntity.SeatStatus.Bet, room.baseScore);
                    m_btnReady.SafeSetActive(room.SeatList[i].status == SeatEntity.SeatStatus.Idle && room.roomStatus == RoomEntity.RoomStatus.Idle);
                    m_btnShowPokerReady.SafeSetActive(room.SeatList[i].status == SeatEntity.SeatStatus.Idle && room.roomStatus == RoomEntity.RoomStatus.Idle);
                }
            }

            m_imgTrustee.SafeSetActive(room.PlayerSeat.IsTrustee);

            SetShowPokerImage(false);

            m_txtBaseScore.SafeSetText(room.baseScore.ToString());
        }
        #endregion

        #region OnReady 准备
        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="obj"></param>
        private void OnReady(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("SeatEntity");

            if (seat == null) return;
            if (!seat.IsPlayer) return;
            for (int i = 0; i < m_BasePokerList.Count; ++i)
            {
                UIPoolManager.Instance.Despawn(m_BasePokerList[i].transform);
            }
            m_BasePokerList.Clear();

            m_txtBaseScore.SafeSetText("0");
            m_txtTimes.SafeSetText("1");

            SetPlayPokerMenuActive(false);

            m_btnReady.SafeSetActive(seat.status == SeatEntity.SeatStatus.Idle);
            m_btnShowPokerReady.SafeSetActive(seat.status == SeatEntity.SeatStatus.Idle);
        }
        #endregion

        #region OnBegin 开局
        /// <summary>
        /// 开局
        /// </summary>
        /// <param name="obj"></param>
        private void OnBegin(TransferData data)
        {
            m_loop = data.GetValue<int>("Loop");
            //m_loop += 1;
            m_txtLoop.SafeSetText(string.Format("{0}/{1}", m_loop, m_maxLoop));
        }
        #endregion

        #region OnRoomInfoChanged 房间信息变更
        /// <summary>
        /// 房间信息变更
        /// </summary>
        /// <param name="obj"></param>
        private void OnRoomInfoChanged(TransferData data)
        {
            int roomId = data.GetValue<int>("RoomId");
            int baseScore = data.GetValue<int>("BaseScore");
            int currentLoop = data.GetValue<int>("CurrentLoop");
            int maxLoop = data.GetValue<int>("MaxLoop");
            int times = data.GetValue<int>("Times");

            m_txtBaseScore.SafeSetText(baseScore.ToString());
            m_txtRoomId.SafeSetText(roomId.ToString());
            m_txtLoop.SafeSetText(string.Format("{0}/{1}", currentLoop, maxLoop));
            m_txtTimes.SafeSetText(times.ToString());
            m_loop = currentLoop;
            m_maxLoop = maxLoop;
        }
        #endregion

        #region OnAskBet 询问下注
        /// <summary>
        /// 询问下注
        /// </summary>
        /// <param name="obj"></param>
        private void OnAskBet(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("SeatEntity");
            if (seat == null) return;
            if (!seat.IsPlayer) return;
            int baseScore = data.GetValue<int>("BaseScore");
            SetBetMenuActive(true, baseScore);
            //SetPlayPokerMenuActive(seat.status == SeatEntity.SeatStatus.PlayPoker);
        }
        #endregion

        #region OnBet 下注
        /// <summary>
        /// 下注
        /// </summary>
        /// <param name="obj"></param>
        private void OnBet(TransferData data)
        {
            RoomEntity room = data.GetValue<RoomEntity>("Room");
            if (room == null) return;
            SeatEntity seat = data.GetValue<SeatEntity>("SeatEntity");
            if (seat == null) return;
            SetBetMenuActive(false, 0);

            m_txtBaseScore.SafeSetText(room.baseScore.ToString());

        }
        #endregion

        #region OnSetBanker 设置地主
        /// <summary>
        /// 设置地主
        /// </summary>
        /// <param name="obj"></param>
        private void OnSetBanker(TransferData data)
        {
            List<Poker> pokers = data.GetValue<List<Poker>>("PokerList");
            if (pokers == null) return;

            for (int i = 0; i < m_BasePokerList.Count; ++i)
            {
                UIPoolManager.Instance.Despawn(m_BasePokerList[i].transform);
            }
            m_BasePokerList.Clear();

            for (int i = 0; i < pokers.Count; ++i)
            {
                UIItemPoker item = SpawnPoker(pokers[i]);
                item.gameObject.SetParent(m_BasePokerContainer);
                m_BasePokerList.Add(item);
            }
            //if (m_loop == 0)
            ////{
            //m_loop += 1;
            //m_txtLoop.SafeSetText(string.Format("{0}/{1}", m_loop, m_maxLoop));
            //}
        }
        #endregion

        #region OnAskPlayPoker 询问出牌
        /// <summary>
        /// 询问出牌
        /// </summary>
        /// <param name="obj"></param>
        private void OnAskPlayPoker(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("SeatEntity");

            Deck prevDeck = data.GetValue<Deck>("PreviourDeck");

            bool isDelaySendPass = data.GetValue<bool>("IsDelaySendPass");

            if (seat == null) return;
            if (!seat.IsPlayer) return;

            bool mustPlay = data.GetValue<bool>("MustPlay");
            bool canPlay = data.GetValue<bool>("CanPlay");
            
            if (mustPlay || canPlay)
            {
                if (prevDeck != null)
                {
                    if (prevDeck.type == DeckType.A && seat.pokerList.Count == 1)
                    {
                        SendNotification("DouDiZhuViewAskLastPokersComplete");
                    }
                }
                else
                {
                    if (seat.pokerList.Count == 1)
                    {
                        SendNotification("DouDiZhuViewAskLastPokersComplete");
                    }
                }
            }

            SetPlayPokerMenuActive(true, !mustPlay, !canPlay);

            if (isDelaySendPass)
            {
                StartCoroutine(DelayClientSendPass(seat.unixtime));
            }
        }
        #endregion

        private IEnumerator DelayClientSendPass(int delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            SendNotification("btnDouDiZhuViewPass");
        }

        #region OnPlayPoker 出牌
        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="obj"></param>
        private void OnPlayPoker(TransferData data)
        {
            SetPlayPokerMenuActive(false);
        }
        #endregion

        #region OnPlusTimes 翻倍
        /// <summary>
        /// 翻倍
        /// </summary>
        /// <param name="data"></param>
        private void OnPlusTimes(TransferData data)
        {
            RoomEntity room = data.GetValue<RoomEntity>("Room");
            if (room == null) return;

            m_txtTimes.SafeSetText(room.Times.ToString());
        }
        #endregion

        #region OnPass 过
        /// <summary>
        /// 过
        /// </summary>
        /// <param name="data"></param>
        private void OnPass(TransferData data)
        {
            SetPlayPokerMenuActive(false);
        }
        #endregion


        #region OnTrustee 托管
        /// <summary>
        /// 托管
        /// </summary>
        /// <param name="data"></param>
        private void OnTrustee(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("SeatEntity");
            if (seat == null) return;
            if (!seat.IsPlayer) return;
            m_imgTrustee.SafeSetActive(seat.IsTrustee);
        }
        #endregion

        #region OnSeatSettle 打开结算界面以及动画
        /// <summary>
        /// 打开结算界面以及动画
        /// </summary>
        /// <param name="obj"></param>
        private void OnSeatSettle(TransferData data)
        {
            RoomEntity settleRoom = data.GetValue<RoomEntity>("Room");

            bool isSpring = false;

            for (int i = 0; i < settleRoom.SeatCount; i++)
            {
                if (settleRoom.SeatList[i].isSpring)
                {
                    isSpring = true;
                    break;
                }
            }

            float waitTime = 1;

            if (isSpring)
            {
                waitTime += 2f;
                if (springAnimation != null)
                {
                    springAnimation.gameObject.SetActive(true);
                    springAnimation.transform.GetComponent<UIItemSpringAnimation>().FlowerAnimation();
                    springAnimation.DORestart();
                }

                //Action OpenSettleView = (() =>
                //{
                //    m_UISettleView = UIViewUtil.Instance.LoadWindow("Settle_DouDiZhu").GetComponent<UISettleView>();
                //    m_UISettleView.SetUI(settleRoom/*,UISceneDouDZView.Instance.GetTxtLoop()*/);
                //});

                //GetOrCreatUIItemAnimation().ShowPokersAnimation("DouDiZhu/UI_Spring_DouDiZhu", m_AnimationContainer.position, m_AnimationContainer.position, AnimationType.Spring, 1.5f, DG.Tweening.Ease.Linear,OpenSettleView);
            }
            //m_UISettleView = UIViewUtil.Instance.LoadWindow("Settle_DouDiZhu").GetComponent<UISettleView>();
            //m_UISettleView.SetUI(settleRoom/*,UISceneDouDZView.Instance.GetTxtLoop()*/);
            //m_UISettleView.SafeSetActive(false);
            //StartCoroutine(openSettelView(waitTime));

        }
        #endregion
        private IEnumerator openSettelView(float waitTime )
        {
            yield return new WaitForSeconds(waitTime);
            m_UISettleView.SafeSetActive(true);
        }

        private void OnSeatResult(TransferData data)
        {
            RoomEntity resultRoom = data.GetValue<RoomEntity>("Room");

            bool isSpring = false;

            for (int i = 0; i < resultRoom.SeatCount; i++)
            {
                if (resultRoom.SeatList[i].isSpring)
                {
                    isSpring = true;
                    break;
                }
            }

            float waitTime = 0;

            if (isSpring)
            {
                waitTime = 1.5f;
                if (springAnimation != null)
                {
                    springAnimation.gameObject.SetActive(true);
                    springAnimation.transform.GetComponent<UIItemSpringAnimation>().FlowerAnimation();
                    springAnimation.DORestart();
                }

                //Action OpenSettleView = (() =>
                //{
                //    m_UISettleView = UIViewUtil.Instance.LoadWindow("Settle_DouDiZhu").GetComponent<UISettleView>();
                //    m_UISettleView.SetUI(settleRoom/*,UISceneDouDZView.Instance.GetTxtLoop()*/);
                //});

                //GetOrCreatUIItemAnimation().ShowPokersAnimation("DouDiZhu/UI_Spring_DouDiZhu", m_AnimationContainer.position, m_AnimationContainer.position, AnimationType.Spring, 1.5f, DG.Tweening.Ease.Linear,OpenSettleView);
            }
            //m_UIResultView = UIViewUtil.Instance.LoadWindow("Result_DouDiZhu").GetComponent<UIResultView_DouDZ>();
            //m_UIResultView.SetUI(resultRoom);
            //m_UIResultView.SafeSetActive(false);
            //if (m_UISettleView != null)
            //{
            //    m_UISettleView.SetResultView(m_UIResultView);
            //}
            //StartCoroutine(openResultView(waitTime, resultRoom));
        }

        #region OpenSettleView 打开结算界面
        /// <summary>
        /// 打开结算界面
        /// </summary>
        private IEnumerator openResultView(float waitTime, RoomEntity resultRoom)
        {
            yield return new WaitForSeconds(waitTime);
            //m_UIResultView.SafeSetActive(true);
        }
        #endregion


        #region SetPlayPoker 设置可以出牌
        /// <summary>
        /// 设置可以出牌
        /// </summary>
        /// <param name="canPlayPoker"></param>
        public void SetPlayPoker(bool canPlayPoker)
        {
            m_imgCantPlayPoker.SafeSetActive(!canPlayPoker);
        }
        #endregion

        #region SetPlayPokerMenuActive 设置出牌菜单显示
        /// <summary>
        /// 设置出牌菜单显示
        /// </summary>
        /// <param name="isActive"></param>
        private void SetPlayPokerMenuActive(bool isActive, bool passActive = true, bool isYaobuqi = false)
        {
            m_PlayPokerMenu.SetActive(isActive);

            //m_imgPass.SafeSetActive(passActive);

            m_btnYaobuqi.SafeSetActive(isYaobuqi);
            m_imgCantPlayPoker.SafeSetActive(!isYaobuqi);
            m_imgPlayPoker.SafeSetActive(!isYaobuqi);
            m_imgPass.SafeSetActive(!isYaobuqi);
            m_imgTip.SafeSetActive(!isYaobuqi);

            if (!passActive)
            {
                m_btnYaobuqi.SafeSetActive(false);
                m_imgCantPlayPoker.SafeSetActive(true);
                m_imgPlayPoker.SafeSetActive(true);
                m_imgPass.SafeSetActive(false);
                m_imgTip.SafeSetActive(true);
            }

            if (!isActive) m_imgCantPlayPoker.SafeSetActive(false);
        }
        #endregion

        #region SetBetMenuActive 设置下注菜单显示
        /// <summary>
        /// 设置下注菜单显示
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="baseScore"></param>
        public void SetBetMenuActive(bool isActive, int baseScore)
        {
            for (int i = 0; i < m_arrScore.Length; ++i)
            {
                m_arrScore[i].SafeSetActive(isActive);
            }
            for (int i = 0; i < m_arrCantScore.Length; ++i)
            {
                m_arrCantScore[i].SafeSetActive(isActive ? m_arrCantScore[i].name.ToInt() <= baseScore : false);
            }
        }
        #endregion

        #region SpawnPoker 生成牌
        /// <summary>
        /// 生成牌
        /// </summary>
        /// <param name="poker"></param>
        /// <returns></returns>
        private UIItemPoker SpawnPoker(Poker poker)
        {
            Transform trans = UIPoolManager.Instance.Spawn("UIItemPoker_DouDiZhu");
            UIItemPoker item = trans.GetComponent<UIItemPoker>();
            item.Init(poker);
            return item;
        }
        #endregion

        #region Reset 进入房间时重置状态
        /// <summary>
        /// 进入房间时重置状态
        /// </summary>
        public void Reset()
        {
            m_loop = 0;
            m_maxLoop = 0;
            m_txtBaseScore.SafeSetText("");//底分
            m_txtTimes.SafeSetText("");//倍数
            m_txtRoomId.SafeSetText("");//房间Id
            m_txtLoop.SafeSetText("");//局数
            m_PlayPokerMenu.SetActive(false);
            m_imgTrustee.SafeSetActive(false);

            for (int i = 0; i < m_BasePokerList.Count; ++i)
            {
                UIPoolManager.Instance.Despawn(m_BasePokerList[i].transform);
            }
            m_BasePokerList.Clear();
            for (int i = 0; i < lstUIItemSeat.Length; i++)
            {
                lstUIItemSeat[i].Reset();
            }
        }
        #endregion

        #region SetShowPokerImage 设置明牌的激活状态
        /// <summary>
        /// 设置明牌的激活状态
        /// </summary>
        /// <param name="isActive"></param>
        public void SetShowPokerImage(bool isActive)
        {
            m_imgShowPoker.SafeSetActive(isActive);
        }
        #endregion

        #region GetOrCreatUIItemAnimation 在动画生成点增加或者获取脚本
        /// <summary>
        /// 在动画生成点增加或者获取脚本
        /// </summary>
        /// <returns></returns>
        private UIItemAnimation GetOrCreatUIItemAnimation()
        {
            UIItemAnimation uiitemAnimation = null;

            if (m_AnimationContainer != null)
            {
                if (m_AnimationContainer.gameObject.GetComponent<UIItemAnimation>() == null)
                {
                    uiitemAnimation = m_AnimationContainer.gameObject.AddComponent<UIItemAnimation>();
                }
                else
                {
                    uiitemAnimation = m_AnimationContainer.gameObject.GetComponent<UIItemAnimation>();
                }
            }
            else
            {
                Debug.LogError("动画挂载点是空的");
            }

            return uiitemAnimation;
        }
        #endregion
    }
}

