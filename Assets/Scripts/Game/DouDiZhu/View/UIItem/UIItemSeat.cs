//===================================================
//Author      : DRB
//CreateTime  ：11/28/2017 3:27:23 PM
//Description ：
//===================================================
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DRB.DouDiZhu
{
    public class UIItemSeat : UIItemBase
    {
        #region Variable
        [SerializeField]
        private RawImage m_imgHead;//头像
        [SerializeField]
        private Text m_txtNickname;//昵称
        [SerializeField]
        private Text m_txtTotalScore;//总得分数目
        [SerializeField]
        private UITextureNumber m_txtOverPlus;//牌剩余数量
        [SerializeField]
        private UIItemPokerCount m_PokerCount;
        [SerializeField]
        private Image m_imgReady;//已准备图标
        [SerializeField]
        private Transform m_HandContainer;//手牌挂载点
        [SerializeField]
        private Transform m_PlayPokerContainer;//打出的牌挂载点
        [SerializeField]
        private Transform m_AnimationContainer;//动画挂载点
        [SerializeField]
        private Transform m_SSAnimationContainer;//炸弹动画挂载点
        [SerializeField]
        private Transform m_CountDownContainer;//倒计时挂载点
        [SerializeField]
        private Transform m_arrScoreParent;//押注的分的父物体
        [SerializeField]
        private Image[] m_arrScore;//押注的分
        [SerializeField]
        private int m_Index;//座位索引
        [SerializeField]
        private int m_Pos;//座位索引
        [SerializeField]
        private int m_gender;//性别
        [SerializeField]
        private Image m_imgPass;//过的图标
        [SerializeField]
        private Image m_imgBanker;//庄家图标
        [SerializeField]
        private Image m_imgPlayer;//闲家图标
        [SerializeField]
        private UIItemCountDown m_countDown;//倒计时

        public static UIItemSeat PlayerSeat;


        [SerializeField]
        private DOTweenAnimation ABCEDIconAnimation;
        [SerializeField]
        private DOTweenAnimation AAABBBCDIconAnimation;
        [SerializeField]
        private DOTweenAnimation AABBCCIconAnimation;
        [SerializeField]
        private DOTweenAnimation SSAnimation;
        [SerializeField]
        private UIAnimation SSSmokeAnimation;
        [SerializeField]
        private DOTweenPath BombAnimation;
        [SerializeField]
        private UIAnimation bombUIAnimation;
        [SerializeField]
        private DOTweenAnimation airPlaneAnimation;

        [SerializeField]
        private List<UIItemPoker> m_HandList = new List<UIItemPoker>();
        [SerializeField]
        private List<UIItemPoker> m_PlayPokers = new List<UIItemPoker>();
        #endregion

        #region MonoBehaviour
        protected override void OnAwake()
        {
            base.OnAwake();
            if (m_Index == 0)
            {
                PlayerSeat = this;
            }
            m_imgPass.SafeSetActive(false);
        }
        #endregion

        #region Dictionary
        public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
        {
            Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
            dic.Add(RoomProxy.ON_INIT, OnInit);
            dic.Add(RoomProxy.ON_READY, OnReady);
            dic.Add(RoomProxy.ON_SEAT_INFO_CHANGED, OnSeatInfoChanged);
            dic.Add(RoomProxy.ON_SEAT_BET, OnSeatBet);
            dic.Add(RoomProxy.ON_SET_BANKER, OnSetBanker);
            dic.Add(RoomProxy.ON_BEGIN, OnBegin);
            dic.Add(RoomProxy.ON_SHOW_POKER, OnShowPoker);
            dic.Add(RoomProxy.ON_ASK_PLAY_POKER, OnAskPlayPoker);
            dic.Add(RoomProxy.ON_PLAY_POKER, OnPlayPoker);
            dic.Add(RoomProxy.ON_SEAT_PASS, OnSeatPass);
            dic.Add(RoomProxy.ON_SETTLE, OnSeatSettle);
            dic.Add(RoomProxy.ON_RESULT, OnSeatResult);

            return dic;
        }
        #endregion

        #region OnInit 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="obj"></param>
        private void OnInit(TransferData data)
        {
            RoomEntity room = data.GetValue<RoomEntity>("Room");

            if (room == null) return;
            for (int i = 0; i < room.SeatList.Count; ++i)
            {
                SeatEntity seat = room.SeatList[i];
                if (seat.Index != m_Index) continue;
                m_gender = seat.Gender;
                m_Pos = seat.Pos;
                for (int j = 0; j < m_HandList.Count; ++j)
                {
                    UIPoolManager.Instance.Despawn(m_HandList[j].transform);
                }
                m_HandList.Clear();
                for (int j = 0; j < m_PlayPokers.Count; ++j)
                {
                    UIPoolManager.Instance.Despawn(m_PlayPokers[j].transform);
                }
                m_PlayPokers.Clear();
                m_txtTotalScore.SafeSetText(seat.totalScore.ToString());
                for (int j = 0; j < seat.pokerList.Count; ++j)
                {
                    UIItemPoker item = SpawnPoker(seat.pokerList[j]);
                    item.gameObject.SetParent(m_HandContainer);
                    m_HandList.Add(item);
                }

                HandSort();
                for (int j = 0; j < m_HandList.Count; j++)
                {
                    m_HandList[j].transform.SetSiblingIndex(0);
                }

                if (m_txtOverPlus != null)
                {
                    m_txtOverPlus.SetNumber(m_HandList.Count.ToString());
                }
                if (m_PokerCount != null)
                {
                    m_PokerCount.SetNumber(m_HandList.Count.ToString());
                }

                if (seat.PreviourPoker != null)
                {
                    for (int j = seat.PreviourPoker.Count - 1; j >= 0; --j)
                    {
                        UIItemPoker item = SpawnPoker(seat.PreviourPoker[j]);
                        item.gameObject.SetParent(m_PlayPokerContainer);
                        m_PlayPokers.Add(item);
                        item.isBanker = seat.IsBanker;
                    }
                }
                m_imgReady.SafeSetActive(room.roomStatus == RoomEntity.RoomStatus.Idle && seat.status == SeatEntity.SeatStatus.Ready);

                m_imgBanker.SafeSetActive(seat.IsBanker);
                m_imgPlayer.SafeSetActive(!seat.IsBanker);

                if (m_arrScoreParent)
                {
                    m_arrScoreParent.gameObject.SetActive(true);
                }

                for (int j = 0; j < m_arrScore.Length; ++j)
                {
                    m_arrScore[j].SafeSetActive(room.roomStatus == RoomEntity.RoomStatus.Bet && m_arrScore[j].name.ToInt() == seat.bet);
                }

                m_imgPass.SafeSetActive(room.roomStatus == RoomEntity.RoomStatus.Gaming && seat.PreviourPoker == null && seat.status == SeatEntity.SeatStatus.Wait);

                if (seat.status == SeatEntity.SeatStatus.PlayPoker)
                {
                    m_countDown.SafeSetActive(true);
                    m_countDown.transform.SetParent(m_CountDownContainer);
                    m_countDown.transform.localPosition = Vector3.zero;
                    m_countDown.SetCountDown(seat.unixtime);
                }
            }
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
            if (seat.Index != m_Index) return;

            m_imgReady.SafeSetActive(seat.status == SeatEntity.SeatStatus.Ready);

            for (int i = 0; i < m_HandList.Count; ++i)
            {
                UIPoolManager.Instance.Despawn(m_HandList[i].transform);
            }
            m_HandList.Clear();
            for (int i = 0; i < m_PlayPokers.Count; ++i)
            {
                UIPoolManager.Instance.Despawn(m_PlayPokers[i].transform);
            }
            m_PlayPokers.Clear();

            m_imgPass.SafeSetActive(false);
        }
        #endregion

        #region OnSeatInfoChanged 座位信息变更
        /// <summary>
        /// 座位信息变更
        /// </summary>
        /// <param name="data"></param>
        private void OnSeatInfoChanged(TransferData data)
        {
            int seatIndex = data.GetValue<int>("SeatIndex");

            if (seatIndex != m_Index) return;

            int playerId = data.GetValue<int>("PlayerId");

            if (playerId == 0)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            string avatar = data.GetValue<string>("Avatar");
            string nickName = data.GetValue<string>("Nickname");
            int totalScore = data.GetValue<int>("TotalScore");

            RoomEntity.RoomStatus roomStatus = data.GetValue<RoomEntity.RoomStatus>("RoomStatus");
            TextureManager.Instance.LoadHead(avatar, (Texture2D tex) =>
            {
                if (m_imgHead != null && tex != null)
                {
                    m_imgHead.texture = tex;
                }
            });
            m_txtNickname.SafeSetText(nickName);
            m_txtTotalScore.SafeSetText(totalScore.ToString());
        }
        #endregion

        #region OnSeatBet 下注
        /// <summary>
        /// 下注
        /// </summary>
        /// <param name="obj"></param>
        private void OnSeatBet(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("SeatEntity");
            if (seat == null) return;
            if (seat.Index != m_Index) return;

            if (m_arrScoreParent)
            {
                m_arrScoreParent.gameObject.SetActive(true);
            }

            for (int i = 0; i < m_arrScore.Length; ++i)
            {
                m_arrScore[i].SafeSetActive(m_arrScore[i].name.ToInt() == seat.bet);
            }

            string audioSuffix;

            if (m_gender == 1)
            {
                audioSuffix = "Man";
            }
            else
            {
                audioSuffix = "Woman";
            }

            PlayAudio("bet" + seat.bet + audioSuffix);
        }
        #endregion

        #region OnSetBanker 设置地主
        /// <summary>
        /// 设置地主
        /// </summary>
        /// <param name="obj"></param>
        private void OnSetBanker(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("SeatEntity");
            if (seat == null) return;
            if (seat.Index == m_Index)
            {
                List<Poker> pokers = data.GetValue<List<Poker>>("PokerList");
                if (pokers == null) return;

                if (seat.IsPlayer)
                {
                    for (int i = 0; i < pokers.Count; ++i)
                    {
                        UIItemPoker item = SpawnPoker(pokers[i]);
                        item.gameObject.SetParent(m_HandContainer);
                        m_HandList.Add(item);
                    }
                }
                else
                {
                    for (int i = 0; i < pokers.Count; ++i)
                    {
                        UIItemPoker item = SpawnPoker(new Poker(pokers[i].index, 0, 0));
                        item.gameObject.SetParent(m_HandContainer);
                        m_HandList.Add(item);
                    }
                }

                HandSort();

                for (int j = 0; j < m_HandList.Count; j++)
                {
                    m_HandList[j].transform.SetSiblingIndex(0);
                }
                if (m_txtOverPlus != null)
                {
                    m_txtOverPlus.SetNumber(m_HandList.Count.ToString());
                }
                if (m_PokerCount != null)
                {
                    m_PokerCount.SetNumber(m_HandList.Count.ToString());
                }
            }

            if (m_arrScoreParent)
            {
                m_arrScoreParent.gameObject.SetActive(true);
            }

            m_imgBanker.SafeSetActive(seat.Index == m_Index);
            m_imgPlayer.SafeSetActive(seat.Index != m_Index);

            for (int i = 0; i < m_arrScore.Length; ++i)
            {
                m_arrScore[i].SafeSetActive(false);
            }
        }
        #endregion

        #region OnBegin 开局
        /// <summary>
        /// 开局
        /// </summary>
        /// <param name="obj"></param>
        private void OnBegin(TransferData data)
        {
            RoomEntity room = data.GetValue<RoomEntity>("Room");
            if (room == null) return;

            for (int i = 0; i < room.SeatList.Count; ++i)
            {
                SeatEntity seat = room.SeatList[i];
                if (seat.Index == m_Index)
                {
                    for (int j = 0; j < m_HandList.Count; ++j)
                    {
                        UIPoolManager.Instance.Despawn(m_HandList[j].transform);
                    }
                    m_HandList.Clear();

                    m_gender = seat.Gender;

                    for (int j = 0; j < m_PlayPokers.Count; ++j)
                    {
                        UIPoolManager.Instance.Despawn(m_PlayPokers[j].transform);
                    }
                    m_PlayPokers.Clear();

                    List<GameObject> lstPokerTrans = new List<GameObject>();

                    for (int j = 0; j < seat.pokerList.Count; ++j)
                    {
                        UIItemPoker item = SpawnPoker(seat.pokerList[j]);
                        lstPokerTrans.Add(item.gameObject);
                        item.gameObject.SetParent(m_HandContainer);
                        item.gameObject.SetActive(false);
                        m_HandList.Add(item);
                    }

                    HandSort();

                    StartCoroutine(PlaySetHandPokerActive(lstPokerTrans));

                    if (m_txtOverPlus != null)
                    {
                        m_txtOverPlus.SetNumber(m_HandList.Count.ToString());
                    }
                    if (m_PokerCount != null)
                    {
                        m_PokerCount.SetNumber(m_HandList.Count.ToString());
                    }

                    m_imgReady.SafeSetActive(false);

                    m_imgBanker.SafeSetActive(false);
                    m_imgPlayer.SafeSetActive(true);

                    PlayAudio("deal");
                }
            }
        }
        #endregion

        #region OnShowPoker 明牌
        /// <summary>
        /// 明牌
        /// </summary>
        /// <param name="data"></param>
        private void OnShowPoker(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("SeatEntity");

            if (seat.Index != m_Index) return;

            if (seat.Index == 0) return;

            for (int i = 0; i < seat.pokerList.Count; ++i)
            {
                for (int j = 0; j < m_HandList.Count; ++j)
                {
                    if (seat.pokerList[i].index == m_HandList[j].Poker.index)
                    {
                        UIPoolManager.Instance.Despawn(m_HandList[i].transform);
                        break;
                    }
                }
            }

            List<GameObject> lstPokerTrans = new List<GameObject>();

            for (int i = 0; i < seat.pokerList.Count; ++i)
            {
                UIItemPoker item = SpawnPoker(seat.pokerList[i]);
                lstPokerTrans.Add(item.gameObject);
                item.gameObject.SetActive(false);
                item.gameObject.SetParent(m_HandContainer);
                m_HandList.Add(item);
            }
            StopAllCoroutines();

            StartCoroutine(PlaySetHandPokerActive(lstPokerTrans));
        }
        #endregion

        #region OnAskPlayPoker 询问出牌
        /// <summary>
        /// 询问出牌
        /// </summary>
        /// <param name="data"></param>
        private void OnAskPlayPoker(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("SeatEntity");
            if (seat == null) return;
            if (seat.Index != m_Index) return;

            for (int i = 0; i < m_PlayPokers.Count; ++i)
            {
                UIPoolManager.Instance.Despawn(m_PlayPokers[i].transform);
            }
            m_PlayPokers.Clear();

            m_countDown.SafeSetActive(true);
            m_countDown.transform.SetParent(m_CountDownContainer);
            m_countDown.transform.localPosition = Vector3.zero;
            m_countDown.SetCountDown(seat.unixtime);

            m_imgPass.SafeSetActive(false);
        }
        #endregion

        #region OnPlayPoker 出牌
        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="data"></param>
        private void OnPlayPoker(TransferData data)
        {
            int seatIndex = data.GetValue<int>("SeatIndex");

            bool isBanker = data.GetValue<bool>("IsBanker");

            bool isBiger = data.GetValue<bool>("isBiger");

            if (seatIndex != m_Index) return;

            m_countDown.SetCountDown(0);

            for (int i = 0; i < m_PlayPokers.Count; ++i)
            {
                UIPoolManager.Instance.Despawn(m_PlayPokers[i].transform);
            }
            m_PlayPokers.Clear();

            for (int i = 0; i < m_HandList.Count; ++i)
            {
                m_HandList[i].isSelect = false;
            }

            Deck deck = data.GetValue<Deck>("Deck");

            for (int i = 0; i < deck.pokers.Count; ++i)
            {
                for (int j = 0; j < m_HandList.Count; ++j)
                {
                    if (m_HandList[j].Poker.index == deck.pokers[i].index)
                    {
                        m_HandList[j].ClearAction();
                        m_PlayPokers.Add(m_HandList[j]);
                        m_HandList.RemoveAt(j);
                        break;
                    }
                }
            }

            if (m_PlayPokers.Count != deck.pokers.Count)
            {
                Debug.LogError("出牌数量不对!!!");
            }

            PlayPokerAnimation(deck, isBanker);

            PokerIconAndBombAnimation(deck);

            if (m_txtOverPlus != null)
            {
                m_txtOverPlus.SetNumber(m_HandList.Count.ToString());
            }
            if (m_PokerCount != null)
            {
                m_PokerCount.SetNumber(m_HandList.Count.ToString());
            }

            PlayPokerAudio(deck, isBiger);
        }
        #endregion

        #region OnSeatPass 过
        /// <summary>
        /// 过
        /// </summary>
        /// <param name="obj"></param>
        private void OnSeatPass(TransferData data)
        {
            int seatIndex = data.GetValue<int>("SeatIndex");
            if (seatIndex != m_Index) return;

            for (int i = 0; i < m_HandList.Count; ++i)
            {
                m_HandList[i].isSelect = false;
            }

            m_imgPass.SafeSetActive(true);

            string audioSuffix;

            if (m_gender == 1)
            {
                audioSuffix = "Man";
            }
            else
            {
                audioSuffix = "Woman";
            }

            PlayAudio("pass" + audioSuffix);
        }
        #endregion

        #region OnSeatSettle 结算
        /// <summary>
        /// 结算
        /// </summary>
        /// <param name="obj"></param>
        private void OnSeatSettle(TransferData data)
        {
            RoomEntity CurrentRoom = data.GetValue<RoomEntity>("Room");
            for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
            {
                if (CurrentRoom.SeatList[i].Pos == m_Pos)
                {
                    m_txtTotalScore.SafeSetText(CurrentRoom.SeatList[i].totalScore.ToString());
                    break;
                }
            }
        }
        #endregion

        #region OnSeatResult 总结算
        /// <summary>
        /// 总结算
        /// </summary>
        /// <param name="obj"></param>
        private void OnSeatResult(TransferData data)
        {
            RoomEntity CurrentRoom = data.GetValue<RoomEntity>("Room");
        }
        #endregion

        #region SpawnPoker 生成牌
        /// <summary>
        /// 生成牌
        /// </summary>
        /// <param name="poker"></param>
        /// <returns></returns>
        private UIItemPoker SpawnPoker(Poker poker, bool isAddAction = true)
        {
            Transform trans = UIPoolManager.Instance.Spawn("UIItemPoker_DouDiZhu");
            UIItemPoker item = trans.GetComponent<UIItemPoker>();
            item.isBanker = false;
            item.Init(poker);
            //item.onClick = OnPokerClick;
            if (isAddAction)
            {
                item.OnPointerDown = OnHandDown;
                item.OnPointerUp = OnHandUp;
                item.OnPointerEnter = OnHandEnter;
            }
            return item;
        }
        #endregion

        #region HandSort 手牌排序
        /// <summary>
        /// 手牌排序
        /// </summary>
        private void HandSort()
        {
            if (m_HandList == null || m_HandList.Count == 0) return;

            m_HandList.Sort();

            for (int i = 0; i < m_HandList.Count; ++i)
            {
                m_HandList[i].transform.SetSiblingIndex(i);
            }
        }
        #endregion

        #region OnSelectPoker 选择牌
        /// <summary>
        /// 选择牌
        /// </summary>
        /// <param name="obj"></param>
        public void SelectPoker(List<Poker> pokers)
        {
            for (int i = 0; i < m_HandList.Count; ++i)
            {
                bool isExists = false;
                for (int j = 0; j < pokers.Count; ++j)
                {
                    if (m_HandList[i].Poker.index == pokers[j].index)
                    {
                        isExists = true;
                        break;
                    }
                }
                m_HandList[i].isSelect = isExists;
            }
        }
        #endregion

        #region SelectVariable

        private bool m_isDown;

        private List<UIItemPoker> m_SelectPoker = new List<UIItemPoker>();

        private int m_StartSelectPokerIndex;

        private int m_TargetSelectPokerIndex;

        //private UIItemPoker m_EndSelectPoker;
        #endregion

        #region OnHandDown 当按下鼠标时 
        /// <summary>
        /// 当按下鼠标时
        /// </summary>
        /// <param name="item"></param>
        private void OnHandDown(UIItemPoker item)
        {
            if (!m_HandList.Contains(item)) return;
            m_isDown = true;
            m_SelectPoker.Clear();
            m_SelectPoker.Add(item);
            for (int i = 0; i < m_HandList.Count; i++)
            {
                if (m_HandList[i].Poker.color == item.Poker.color && m_HandList[i].Poker.size == item.Poker.size)
                {
                    m_StartSelectPokerIndex = i;
                    break;
                }
            }
            item.isGray = true;
        }
        #endregion

        #region OnHandUp 鼠标抬起时
        /// <summary>
        /// 鼠标抬起时
        /// </summary>
        /// <param name="item"></param>
        private void OnHandUp(UIItemPoker item)
        {
            m_isDown = false;

            for (int i = 0; i < m_SelectPoker.Count; ++i)
            {
                m_SelectPoker[i].isGray = false;
            }

            SendNotification("OnDouDiZhuPokerClickUp", m_SelectPoker);
        }
        #endregion

        #region OnHandEnter 当鼠标进入时
        /// <summary>
        /// 当鼠标进入时
        /// </summary>
        /// <param name="item"></param>

        private void OnHandEnter(UIItemPoker item)
        {
            if (!m_isDown) return;
            if (!m_HandList.Contains(item)) return;
            m_SelectPoker.Clear();
            for (int i = 0; i < m_HandList.Count; i++)
            {
                if (m_HandList[i].Poker.color == item.Poker.color && m_HandList[i].Poker.size == item.Poker.size)
                {
                    m_TargetSelectPokerIndex = i;
                    //m_EndSelectPoker = m_HandList[i];
                    break;
                }
            }

            if (m_StartSelectPokerIndex > m_TargetSelectPokerIndex)
            {
                for (int i = m_TargetSelectPokerIndex; i <= m_StartSelectPokerIndex; i++)
                {
                    m_SelectPoker.Add(m_HandList[i]);
                }
            }
            else
            {
                for (int i = m_StartSelectPokerIndex; i <= m_TargetSelectPokerIndex; i++)
                {
                    m_SelectPoker.Add(m_HandList[i]);
                }
            }

            for (int i = 0; i < m_HandList.Count; i++)
            {
                bool isExist = false;
                for (int j = 0; j < m_SelectPoker.Count; j++)
                {
                    if (m_HandList[i].Poker.color == m_SelectPoker[j].Poker.color && m_HandList[i].Poker.size == m_SelectPoker[j].Poker.size)
                    {
                        isExist = true;
                        m_SelectPoker[j].isGray = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    m_HandList[i].isGray = false;
                }
            }
        }
        #endregion

        #region PlayPokerAudio 播放出牌声音
        /// <summary>
        /// 播放出牌声音
        /// </summary>
        /// <param name="deck"></param>
        private void PlayPokerAudio(Deck deck, bool isBiger = false)
        {
            string audioName = null;

            string audioSound = null;

            if (deck == null)
            {
                audioName = "pass";
            }
            else
            {
                audioName = deck.type.ToString();

                switch (deck.type)
                {
                    case DeckType.A:
                        audioName = deck.mainPoker.size.ToString();
                        break;
                    case DeckType.AA:
                        audioName = deck.mainPoker.size.ToString();
                        break;
                    case DeckType.AAABBBCD:
                    case DeckType.AAABBBCCDD:
                    case DeckType.AAABBB:
                        audioName = DeckType.AAABBBCD.ToString();
                        PlayAudio("airEffectAudio");
                        break;
                    case DeckType.SS:
                        PlayAudio("SSSoundEffect");
                        break;
                    case DeckType.AAAA:
                        PlayAudio("AAAABombEffectAudio");
                        break;
                    default:
                        audioName = deck.type.ToString();
                        break;
                }
            }

            string audioSuffix;

            if (m_gender == 1)
            {
                audioSuffix = "Man";
            }
            else
            {
                audioSuffix = "Woman";
            }
            if (deck.type == DeckType.AA)
            {
                audioSound = audioName + audioSuffix + "_pair";
            }
            else
            {
                audioSound = audioName + audioSuffix;
            }
            if (deck.type != DeckType.AA && deck.type != DeckType.A && deck.type != DeckType.AAAA && deck.type != DeckType.SS)
            {
                if (isBiger)
                {
                    int ran = UnityEngine.Random.Range(0, 2);
                    if (ran == 0)
                    {
                        audioSound = "dashang" + audioSuffix;
                    }
                    else
                    {
                        audioSound = "guanshang" + audioSuffix;
                    }
                }
            }

            //PlayAudio(DeckType.SS.ToString() + audioSuffix);

            //PlayAudio("SSSoundEffect");

            //PlayAudio("AAAABombEffectAudio");


            //audioName = DeckType.AAABBBCD.ToString();

            //PlayAudio(audioName + audioSuffix);

            //PlayAudio("airEffectAudio");

            PlayAudio(audioSound);

            PlayAudio("ddz_hitPoker");
        }
        #endregion

        #region PlayAudio 播放声音
        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="audioName"></param>
        private void PlayAudio(string audioName)
        {
            AudioEffectManager.Instance.Play("doudizhu/" + audioName);
        }
        #endregion

        #region PlayPokerAnimation 播放出牌动画
        /// <summary>
        /// 播放出牌动画
        /// </summary>
        /// <param name="deviation"></param>
        private void PlayPokerAnimation(Deck deck, bool isBanker = false)
        {
            if (deck.type == DeckType.AABBCC || deck.type == DeckType.ABCDE)
            {
                m_PlayPokerContainer.GetComponent<GridLayoutGroup>().enabled = false;

                for (int i = m_PlayPokers.Count - 1; i >= 0; --i)
                {
                    m_PlayPokers[i].Init(deck.pokers[i]);

                    UIItemPoker uiItemPoker = m_PlayPokers[i];

                    if (uiItemPoker != null)
                    {
                        uiItemPoker.image.DOColor(new Color(1, 1, 1, 0), 0.2f).OnComplete(() =>
                        {
                            uiItemPoker.image.color = Color.white;
                        });

                        float deviationMultiple = m_PlayPokers.Count % 2 == 1 ? (m_PlayPokers.Count / 2) : (m_PlayPokers.Count / 2 - 0.5f);

                        uiItemPoker.MovePoker(new Vector3(0, 20, 0), 0.2f, (UIItemPoker item) =>
                        {
                            float deviation = (m_PlayPokerContainer.GetComponent<GridLayoutGroup>().cellSize.x + m_PlayPokerContainer.GetComponent<GridLayoutGroup>().spacing.x);
                            item.gameObject.SetParent(m_PlayPokerContainer);
                            item.isBanker = isBanker;
                            item.gameObject.transform.localPosition = new Vector3(-deviationMultiple * deviation, 0, 0);
                            //Debug.LogWarning(uiItemPoker.Poker.ToString());
                            item.gameObject.transform.SetSiblingIndex(item.gameObject.transform.parent.childCount - 1);
                            Tween tweener = item.gameObject.transform.DOLocalMoveX(item.gameObject.transform.localPosition.x + deviation * item.gameObject.transform.GetSiblingIndex(), 0.5f);
                            tweener.SetEase(Ease.InOutExpo);
                            //Vector3 ScaleVector = new Vector3(0.8f, 0.8f, 0.8f);
                            //Tweener tweenerScale = item.gameObject.transform.DOScale(ScaleVector, 0.1f);
                            //tweenerScale.SetEase(Ease.OutBack);
                            item.gameObject.transform.GetComponent<RectTransform>().sizeDelta = m_PlayPokerContainer.GetComponent<GridLayoutGroup>().cellSize;
                        });
                    }
                }
            }
            else
            {
                m_PlayPokerContainer.GetComponent<GridLayoutGroup>().enabled = true;

                for (int i = 0; i < m_PlayPokers.Count; ++i)
                {
                    m_PlayPokers[i].Init(deck.pokers[i]);

                    UIItemPoker uiItemPoker = m_PlayPokers[i];

                    if (m_Index != 0)
                    {
                        uiItemPoker.transform.SetSiblingIndex(0);
                    }

                    if (m_PlayPokers[i] != null)
                    {
                        uiItemPoker.image.DOColor(new Color(1, 1, 1, 0), 0.2f).OnComplete(() =>
                        {
                            uiItemPoker.image.color = Color.white;
                        });

                        uiItemPoker.MovePoker(new Vector3(0, 20, 0), 0.2f, (UIItemPoker item) =>
                        {
                            item.gameObject.SetParent(m_PlayPokerContainer);
                            item.isBanker = isBanker;
                            item.gameObject.transform.localPosition = Vector3.zero;
                            Vector3 ScaleVector = new Vector3(1f, 1f, 1);
                            Tweener tweenerScale = item.gameObject.transform.DOScale(ScaleVector, 0.1f);
                            tweenerScale.SetEase(Ease.OutBack);
                        });
                    }
                }
            }
        }
        #endregion

        #region PlaySetHandPokerActive 设置手牌激活状态（开局动画）
        /// <summary>
        /// 设置手牌激活状态
        /// </summary>
        /// <param name="lstTrans"></param>
        /// <returns></returns>
        private System.Collections.IEnumerator PlaySetHandPokerActive(List<GameObject> lstTrans)
        {
            for (int j = 0; j < lstTrans.Count; ++j)
            {
                lstTrans[j].transform.SetSiblingIndex(0);
                lstTrans[j].SetActive(true);

                if (m_Pos == 1)
                {
                    if (j == lstTrans.Count - 1)
                    {
                        if (m_arrScoreParent)
                        {
                            m_arrScoreParent.gameObject.SetActive(true);
                        }
                        //SendNotification("OnSetHandPokerActiveComplete");
                    }
                    else
                    {
                        if (m_arrScoreParent)
                        {
                            m_arrScoreParent.gameObject.SetActive(false);
                        }
                        //SendNotification("OnSetHandPokerActiveNotComplete");
                    }
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
        #endregion

        #region PokerIconAndBombAnimation 播放特效动画
        /// <summary>
        /// 播放特效动画
        /// </summary>
        /// <param name="deck"></param>
        private void PokerIconAndBombAnimation(Deck deck)
        {
            switch (deck.type)
            {
                case DeckType.AABBCC:
                    DoAnimation(AABBCCIconAnimation);
                    break;
                case DeckType.ABCDE:
                    DoAnimation(ABCEDIconAnimation);
                    break;
                case DeckType.AAABBB:
                case DeckType.AAABBBCD:
                case DeckType.AAABBBCCDD:
                    DoAnimation(AAABBBCDIconAnimation);
                    DoAnimation(airPlaneAnimation);
                    break;
                case DeckType.AAAA:
                    bombUIAnimation.SafeSetActive(true);
                    BombAnimation.gameObject.SetActive(true);
                    BombAnimation.tween.Restart();
                    bombUIAnimation.transform.SetParent(BombAnimation.transform);
                    bombUIAnimation.transform.localPosition = Vector3.zero;
                    break;
                case DeckType.SS:
                    DoAnimation(SSAnimation);
                    break;
            }

            #region Old
            //switch (deck.type)
            //{
            //    case DeckType.AABBCC:
            //        if (name == "UIItemSeat1")
            //        {
            //            GetOrCreatUIItemAnimation().ShowPokersAnimation("uiicon/UI_IconAABBCC", m_AnimationContainer.position + new Vector3(1.5f, 0, 0), m_AnimationContainer.position + new Vector3(-1.5f, 0, 0));
            //        }
            //        else
            //        {
            //            GetOrCreatUIItemAnimation().ShowPokersAnimation("uiicon/UI_IconAABBCC", m_AnimationContainer.position + new Vector3(-1.5f, 0, 0), m_AnimationContainer.position + new Vector3(1.5f, 0, 0));
            //        }
            //        break;
            //    case DeckType.ABCDE:
            //        if (name == "UIItemSeat1")
            //        {

            //            GetOrCreatUIItemAnimation().ShowPokersAnimation("uiicon/UI_IconABCDE", m_AnimationContainer.position + new Vector3(1.5f, 0, 0), m_AnimationContainer.position + new Vector3(-1.5f, 0, 0));
            //        }
            //        else
            //        {
            //            GetOrCreatUIItemAnimation().ShowPokersAnimation("uiicon/UI_IconABCDE", m_AnimationContainer.position + new Vector3(-1.5f, 0, 0), m_AnimationContainer.position + new Vector3(1.5f, 0, 0));
            //        }
            //        break;
            //    case DeckType.AAABBB:
            //    case DeckType.AAABBBCD:
            //    case DeckType.AAABBBCCDD:
            //        GetOrCreatUIItemAnimation().LoadAnimation("UIRoomAnimation_Plane");
            //        if (name == "UIItemSeat1")
            //        {
            //            GetOrCreatUIItemAnimation().ShowPokersAnimation("uiicon/UI_IconAir", m_AnimationContainer.position + new Vector3(1.5f, 0, 0), m_AnimationContainer.position + new Vector3(-1.5f, 0, 0));
            //        }
            //        else
            //        {
            //            GetOrCreatUIItemAnimation().ShowPokersAnimation("uiicon/UI_IconAir", m_AnimationContainer.position + new Vector3(-1.5f, 0, 0), m_AnimationContainer.position + new Vector3(1.5f, 0, 0));
            //        }
            //        break;
            //    case DeckType.AAAA:
            //        GetOrCreatUIItemAnimation().LoadAnimation("DouDiZhu/UI_BombAnimation", m_Index, m_SSAnimationContainer);
            //        break;
            //    case DeckType.SS:
            //        GetOrCreatUIItemAnimation().ShowPokersAnimation("DouDiZhu/UI_SSBomb_DouDiZhu", m_SSAnimationContainer.position, m_SSAnimationContainer.position + new Vector3(0, 10, 0));
            //        GetOrCreatUIItemAnimation().ShowPokersAnimation("DouDiZhu/UI_SSBombSmoke_DouDiZhu", m_SSAnimationContainer.position, m_SSAnimationContainer.position, AnimationType.SSBombSmoke, 0.38f);
            //        break;
            //}
            #endregion

            //DoAnimation(AAABBBCDIconAnimation);
            //DoAnimation(airPlaneAnimation);

            //DoAnimation(SSAnimation);

            //bombUIAnimation.SafeSetActive(true);
            //BombAnimation.gameObject.SetActive(true);
            //BombAnimation.tween.Restart();
            //bombUIAnimation.transform.SetParent(BombAnimation.transform);
            //bombUIAnimation.transform.localPosition = Vector3.zero;
        }
        #endregion

        #region DoAnimation 播放dotweenAnimation动画
        /// <summary>
        /// 播放dotweenAnimation动画
        /// </summary>
        /// <param name="animation"></param>
        private void DoAnimation(DOTweenAnimation animation)
        {
            if (animation != null)
            {
                animation.gameObject.SetActive(true);
                animation.tween.Restart();
            }
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

        #region Reset 进房间时将内容置空
        /// <summary>
        /// 进房间时将内容置空
        /// </summary>
        public void Reset()
        {
            m_txtNickname.SafeSetText("");//昵称
            m_txtTotalScore.SafeSetText("");//总得分数目
            m_PokerCount.SetNumber("0");
            for (int i = 0; i < m_HandList.Count; ++i)
            {
                UIPoolManager.Instance.Despawn(m_HandList[i].transform);
            }
            m_HandList.Clear();
            for (int i = 0; i < m_PlayPokers.Count; ++i)
            {
                UIPoolManager.Instance.Despawn(m_PlayPokers[i].transform);
            }
            m_PlayPokers.Clear();

            m_countDown.SetCountDown(0);
            m_countDown.SafeSetActive(false);
        }
        #endregion
    }
}
