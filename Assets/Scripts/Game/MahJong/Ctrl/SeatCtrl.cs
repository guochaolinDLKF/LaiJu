//===================================================
//Author      : DRB
//CreateTime  ：4/1/2017 11:49:36 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace DRB.MahJong
{

    public class SeatCtrl : MonoBehaviour
    {
        private enum PokerState
        {
            Normal,
            Hold,
            Show,
        }
        [SerializeField]
        private Grid3D m_WallContainer;//墙挂载点
        [SerializeField]
        private Grid3D m_HandContainer;//手牌挂载点
        [SerializeField]
        private Grid3D m_DeskTopContainer;//打出的牌挂载点
        [SerializeField]
        private Transform m_DrawContainer;//摸得牌挂载点
        [SerializeField]
        private PengCtrl[] m_PengCtrls;//碰的牌挂载点
        [SerializeField]
        private Grid3D m_BuHuaContainer;//补花挂载点
        [SerializeField]
        private Transform m_WallModel;//墙模型
        [SerializeField]
        private int m_nSeatPos;
        [SerializeField]
        private TestGrid m_JinContainer;//打金挂载点
        [SerializeField]
        private TestGrid m_BuHuaContainer_New;//补花新版挂载点
        [SerializeField]
        private TestGrid m_SwapContainer;//交换牌挂载点

        private HandCtrl m_PushHand;

        private HandCtrl m_DiceHand;

        private MaJiangCtrl m_HitPoker;

        private List<MaJiangCtrl> m_SwapPoker = new List<MaJiangCtrl>();


        private Tweener m_WallTweener;

        private static MaJiangCtrl s_CurrentMaJiang;

        private static Vector3 s_PlayPokerDestPos;//出牌动画目标点

        private List<MaJiangCtrl> m_LightMaJiang = new List<MaJiangCtrl>();//提示听牌的列表

        private bool m_isOpenDoor;//是否开门

        public int SeatPos
        {
            get { return m_nSeatPos; }
        }

#if IS_SHUANGLIAO
        private const float INIT_POKER_ANIMATION_DURATION = 0.2f;
#else
        private const float INIT_POKER_ANIMATION_DURATION = 0.5f;
#endif

        private Tweener m_HandTween;

        #region MonoBehaviour
        private void Awake()
        {
            m_WallTweener = m_WallModel.DOMove(m_WallModel.transform.position + new Vector3(0, -20, 0), 0.7f).SetEase(Ease.Linear).SetAutoKill(false).Pause();

            m_HandTween = m_HandContainer.transform.DORotate(new Vector3(-75f, 0f, 0f), INIT_POKER_ANIMATION_DURATION, RotateMode.LocalAxisAdd).SetAutoKill(false).Pause();

            string handPrefabName = "hand";
            string handPath = string.Format("download/{0}/prefab/model/{1}.drb", ConstDefine.GAME_NAME, handPrefabName);
            AssetBundleManager.Instance.LoadOrDownload(handPath, handPrefabName, (GameObject go) =>
            {
                m_PushHand = Instantiate(go).GetComponent<HandCtrl>();
                m_PushHand.gameObject.SetParent(transform);
                m_PushHand.gameObject.SetActive(false);
            });

            string diceHandPrefabName = "dicehand";
            string diceHandPath = string.Format("download/{0}/prefab/model/{1}.drb", ConstDefine.GAME_NAME, diceHandPrefabName);
            AssetBundleManager.Instance.LoadOrDownload(diceHandPath, diceHandPrefabName, (GameObject go) =>
            {
                m_DiceHand = Instantiate(go).GetComponent<HandCtrl>();
                m_DiceHand.gameObject.SetParent(transform);
                m_DiceHand.gameObject.SetActive(false);
            });

            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_DRAW_POKER, OnSeatDrawPoker);
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_PLAY_POKER, OnSeatPlayPoker);
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_OPERATE, OnSeatOperate);
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_ZHIDUI, OnSeatZhiDui);
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_ROOM_INFO_CHANGED, OnRoomInfoChanged);
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_HU, OnSeatHu);
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_MING_TI, OnSeatMingTi);
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_SWAP_POKER, OnSwapPoker);
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_SET_LACK_COLOR, OnSetLackColor);
        }

        private void OnDestroy()
        {
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_DRAW_POKER, OnSeatDrawPoker);
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_PLAY_POKER, OnSeatPlayPoker);
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_OPERATE, OnSeatOperate);
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_ZHIDUI, OnSeatZhiDui);
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_ROOM_INFO_CHANGED, OnRoomInfoChanged);
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_HU, OnSeatHu);
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_MING_TI, OnSeatMingTi);
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_SWAP_POKER, OnSwapPoker);
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_SET_LACK_COLOR, OnSeatMingTi);
        }
        #endregion

        #region Init 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="seatCount"></param>
        /// <param name="status"></param>
        public void Init(int seatCount)
        {
            if (seatCount == 2)
            {
                m_DeskTopContainer.constraintCount = 13;
                m_DeskTopContainer.transform.localPosition = new Vector3(-70f, 5f, -58f);
            }
        }
        #endregion

        #region Begin 开局
        /// <summary>
        /// 开局
        /// </summary>
        /// <param name="playerPos"></param>
        public void Begin(int playerPos, bool isHu, bool isReplay)
        {
            m_isOpenDoor = false;

            s_CurrentMaJiang = null;
            m_HitPoker = null;

            for (int i = 0; i < m_PengCtrls.Length; ++i)
            {
                m_PengCtrls[i].Reset();
            }

            if (playerPos == m_nSeatPos && !isHu && !isReplay)
            {
                m_DrawContainer.gameObject.SetLayer("PlayerHand");
                m_HandContainer.gameObject.SetLayer("PlayerHand");
            }
            else
            {
                m_DrawContainer.gameObject.SetLayer("Hand");
                m_HandContainer.gameObject.SetLayer("Hand");
            }

            if (isReplay)
            {
                SetDrawPokerState(PokerState.Show);
                SetHandPokerState(PokerState.Show);
            }
            else
            {
                if (isHu)
                {
                    if (playerPos == m_nSeatPos)
                    {
                        SetHandPokerState(PokerState.Show);
                    }
                    else
                    {
                        SetHandPokerState(PokerState.Hold);
                    }
                    SetDrawPokerState(PokerState.Show);
                }
                else
                {
                    SetDrawPokerState(PokerState.Normal);
                    SetHandPokerState(PokerState.Normal);
                }
            }

        }
        #endregion

        #region OnSeatDrawPoker 当座位摸牌时
        /// <summary>
        /// 当座位摸牌时
        /// </summary>
        /// <param name="data"></param>
        private void OnSeatDrawPoker(TransferData data)
        {
            int seatPos = data.GetValue<int>("SeatPos");
            if (seatPos == m_nSeatPos)
            {
                bool isPlayer = data.GetValue<bool>("IsPlayer");
                Poker hitPoker = data.GetValue<Poker>("HitPoker");
                bool isLast = data.GetValue<bool>("IsLast");
                bool isBuhua = data.GetValue<bool>("IsBuhua");
                MaJiangCtrl majiang = MahJongManager.Instance.DrawMaJiang(m_nSeatPos, hitPoker, isPlayer, isLast, isBuhua);
                DrawPoker(majiang, isBuhua);

                if (isPlayer)
                {
                    CheckTing();
                }
            }
        }
        #endregion

        #region OnSeatPlayPoker 当座位出牌时
        /// <summary>
        /// 当座位出牌时
        /// </summary>
        /// <param name="data"></param>
        private void OnSeatPlayPoker(TransferData data)
        {
            int seatPos = data.GetValue<int>("SeatPos");
            if (seatPos == m_nSeatPos)
            {
                AudioEffectManager.Instance.Play("chupai", Vector3.zero, false);
                Poker hitPoker = data.GetValue<Poker>("HitPoker");
                int gender = data.GetValue<int>("Gender");
                bool isTing = data.GetValue<bool>("IsTing");
                bool isBao = data.GetValue<bool>("IsBao");
                if (isTing)
                {
                    StartCoroutine(PlayTingAudio(gender));
                }
                for (int i = 0; i < m_LightMaJiang.Count; ++i)
                {
                    m_LightMaJiang[i].CloseTip();
                }
                m_LightMaJiang.Clear();
#if IS_LEPING
                if (isBao)
                {
                    AudioEffectManager.Instance.Play(gender.ToString() + "_dabao",Vector3.zero,false);
                }
                PlayAudio(hitPoker.color, hitPoker.size, gender, SystemProxy.Instance.LanguageType);
#elif IS_GUGENG
                if (isBao)
                {
                    AudioEffectManager.Instance.Play(gender.ToString() + "_dabao",Vector3.zero,false);
                }
                else
                {
                    PlayAudio(hitPoker.color, hitPoker.size, gender, SystemProxy.Instance.LanguageType);
                }
#else
                PlayAudio(hitPoker.color, hitPoker.size, gender, SystemProxy.Instance.LanguageType);
#endif

                MaJiangCtrl majiang = MahJongManager.Instance.PlayMaJiang(m_nSeatPos, hitPoker, false);
                PlayPoker(majiang, true, isBao, isBao);

                UIItemOperator.Instance.Close();
            }
        }
        #endregion

        #region PlayTingAudio 播放听牌音效
        /// <summary>
        /// 播放听牌音效
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        private IEnumerator PlayTingAudio(int gender)
        {
            yield return new WaitForSeconds(1f);
            PlayAudio(OperatorType.Ting, 0, gender, SystemProxy.Instance.LanguageType);
        }
        #endregion

        #region OnSeatOperate 当座位操作时
        /// <summary>
        /// 当座位操作时
        /// </summary>
        /// <param name="obj"></param>
        private void OnSeatOperate(TransferData data)
        {
            int seatPos = data.GetValue<int>("SeatPos");
            if (seatPos == m_nSeatPos)
            {
                PokerCombinationEntity combination = data.GetValue<PokerCombinationEntity>("Combination");
                OperatorType type = data.GetValue<OperatorType>("OperateType");
                int subType = data.GetValue<int>("SubType");
                int gender = data.GetValue<int>("Gender");
                bool isPlayer = data.GetValue<bool>("IsPlayer");
                RoomEntity.RoomStatus roomStatus = data.GetValue<RoomEntity.RoomStatus>("RoomStatus");

                PlayAudio(type, subType, gender, SystemProxy.Instance.LanguageType);

                for (int i = 0; i < m_LightMaJiang.Count; ++i)
                {
                    m_LightMaJiang[i].CloseTip();
                }
                m_LightMaJiang.Clear();

                if (type != OperatorType.Hu)
                {
                    if (type == OperatorType.DingJiang)
                    {
                        DingJiang(combination, isPlayer);
                    }
                    else
                    {
                        if (combination != null)
                        {
                            Combination3D combination3D = MahJongManager.Instance.Operate(seatPos, type, subType, combination.PokerList);
                            Operate(combination3D, type != OperatorType.Gang && (roomStatus == RoomEntity.RoomStatus.Begin || roomStatus == RoomEntity.RoomStatus.Deal));
                        }
                    }
                }
                else
                {
                    if ((subType == 2))
                    {
                        EffectManager.Instance.PlayEffectAsync("zimo", (Transform effect) =>
                        {
                            if (m_HitPoker != null)
                            {
                                effect.transform.SetParent(m_DrawContainer);
                                effect.gameObject.SetLayer("PlayerHand");
                                effect.position = m_HitPoker.transform.position;
                                effect.eulerAngles = Vector3.zero;
                            }
                        }, 4f);
                    }
                    else if (subType == 1)
                    {
                        if (s_CurrentMaJiang != null)
                        {
                            EffectManager.Instance.PlayEffectAsync("zimo", (Transform effect) =>
                            {
                                effect.transform.SetParent(s_CurrentMaJiang.transform);
                                effect.position = s_CurrentMaJiang.transform.position;
                                effect.eulerAngles = Vector3.zero;
                            }, 4f);
                        }
                        AudioEffectManager.Instance.Play("se_zimo", Vector3.zero, false);
                    }
                }

                Debug.Log(RoomMaJiangProxy.Instance.CurrentRoom.Status);
                if (RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Begin || RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Deal || (isPlayer) || type == OperatorType.Hu)
                {
                    UIItemOperator.Instance.Close();
                }

                if (isPlayer && type != OperatorType.Hu && (roomStatus == RoomEntity.RoomStatus.Begin || roomStatus == RoomEntity.RoomStatus.Deal))
                {
                    CheckTing();
                }
            }
        }
        #endregion

        #region OnSeatHu 当座位胡牌时
        /// <summary>
        /// 当座位胡牌时
        /// </summary>
        /// <param name="obj"></param>
        private void OnSeatHu(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("SeatEntity");
            RoomEntity.RoomStatus status = data.GetValue<RoomEntity.RoomStatus>("RoomStatus");
            if (seat.Pos != m_nSeatPos) return;
            if (s_CurrentMaJiang != null)
            {
                s_CurrentMaJiang.CloseTip();
            }
            for (int i = 0; i < m_LightMaJiang.Count; ++i)
            {
                m_LightMaJiang[i].CloseTip();
            }
            m_LightMaJiang.Clear();
            int subType = data.GetValue<int>("SubType");
            bool isPlayer = data.GetValue<bool>("IsPlayer");

            PlayAudio(OperatorType.Hu, subType, seat.Gender, SystemProxy.Instance.LanguageType);
//#if IS_GONGXIAN
            if (status == RoomEntity.RoomStatus.Replay)
            {
                SetDrawPokerState(PokerState.Show);
                SetHandPokerState(PokerState.Show);
            }
            else
            {
                SetDrawPokerState(PokerState.Show);
                if (isPlayer)
                {
                    SetHandPokerState(PokerState.Show);
                }
                else
                {
                    SetHandPokerState(PokerState.Hold);
                }
            }

            MaJiangCtrl ctrl = MahJongManager.Instance.Hu(seat.Pos, subType, seat.HitPoker, isPlayer);
            if (ctrl != null)
            {
                m_HitPoker = ctrl;
                m_HitPoker.gameObject.SetParent(m_DrawContainer);
                m_HitPoker.gameObject.SetLayer(m_DrawContainer.gameObject.layer);
            }

            if ((subType == 2))//自摸
            {
                EffectManager.Instance.PlayEffectAsync("zimo", (Transform effect) =>
                {
                    if (m_HitPoker != null)
                    {
                        effect.transform.SetParent(m_DrawContainer);
                        effect.gameObject.SetLayer(m_DrawContainer.gameObject.layer);
                        effect.position = m_HitPoker.transform.position;
                        effect.eulerAngles = Vector3.zero;
                    }
                }, 4f);
            }
            else if (subType == 1)//点炮
            {
                if (m_HitPoker != null)
                {
                    EffectManager.Instance.PlayEffectAsync("zimo", (Transform effect) =>
                    {
                        effect.transform.SetParent(m_DrawContainer);
                        effect.position = m_HitPoker.transform.position;
                        effect.eulerAngles = Vector3.zero;
                        effect.gameObject.SetLayer(m_DrawContainer.gameObject.layer);
                    }, 4f);
                }
                AudioEffectManager.Instance.Play("se_zimo", Vector3.zero, false);
            }
            HandSort(false);
//#else
//            if (seat.HitPoker != null)
//            {
//                MaJiangCtrl ctrl = MahJongManager.Instance.GetPoker(seat.HitPoker.index);
//                if (ctrl != null)
//                {
//                    Transform effect = EffectManager.Instance.PlayEffect("zimo");
//                    effect.transform.SetParent(m_DrawContainer.transform);
//                    effect.gameObject.SetLayer(ctrl.gameObject.layer);
//                    effect.position = ctrl.transform.position;
//                    effect.eulerAngles = Vector3.zero;
//                }
//            }
//            else
//            {
//                if ((subType == 2))
//                {
//                    EffectManager.Instance.PlayEffectAsync("zimo", (Transform effect) =>
//                    {
//                        if (m_HitPoker != null)
//                        {
//                            effect.transform.SetParent(m_DrawContainer.transform);
//                            effect.gameObject.SetLayer(m_DrawContainer.gameObject.layer);
//                            effect.position = m_HitPoker.transform.position;
//                            effect.eulerAngles = Vector3.zero;
//                        }
//                    }, 4f);
//                }
//                else if (subType == 1)
//                {
//                    if (s_PlayPokerDestPos != Vector3.zero && s_CurrentMaJiang != null)
//                    {
//                        EffectManager.Instance.PlayEffectAsync("zimo", (Transform effect) =>
//                        {
//                            effect.position = s_PlayPokerDestPos;
//                            effect.eulerAngles = Vector3.zero;
//                        }, 4f);
//                    }
//                    AudioEffectManager.Instance.Play("se_zimo", Vector3.zero, false);
//                }
//            }
//#endif

            UIItemOperator.Instance.Close();
        }
        #endregion

        #region OnSeatMingTi 明提
        /// <summary>
        /// 明提
        /// </summary>
        /// <param name="obj"></param>
        private void OnSeatMingTi(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("SeatEntity");
            if (seat == null) return;
            if (seat.Pos != m_nSeatPos) return;

            List<MaJiangCtrl> lstHand = MahJongManager.Instance.GetHand(seat.Pos);
            if (lstHand == null) return;

            for (int i = 0; i < seat.MingTiPoker.Count; ++i)
            {
                for (int j = 0; j < lstHand.Count; ++j)
                {
                    if (lstHand[j].Poker.index == seat.MingTiPoker[i].index)
                    {
                        MahJongManager.Instance.DespawnMaJiang(lstHand[j]);
                        lstHand.RemoveAt(j);
                        lstHand.Add(MahJongManager.Instance.SpawnMaJiang(seat.Pos, seat.MingTiPoker[i], LayerMask.LayerToName(m_HandContainer.gameObject.layer)));
                        break;
                    }
                }
            }

            UIItemOperator.Instance.Close();
            HandSort();
        }
        #endregion

        #region OnRoomInfoChanged 房间信息变更
        /// <summary>
        /// 房间信息变更
        /// </summary>
        /// <param name="obj"></param>
        private void OnRoomInfoChanged(TransferData data)
        {
            RoomEntity room = data.GetValue<RoomEntity>("Room");
            SeatEntity currOperator = data.GetValue<SeatEntity>("CurrentOperator");
            int playerSeatPos = data.GetValue<int>("PlayerSeatPos");
            if (room.Status == RoomEntity.RoomStatus.Begin && currOperator != null && currOperator.Pos == m_nSeatPos && playerSeatPos == m_nSeatPos)
            {
                CheckTing();
            }
        }
        #endregion

        #region OnSeatZhiDui 支对
        /// <summary>
        /// 支对
        /// </summary>
        /// <param name="data"></param>
        private void OnSeatZhiDui(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("Seat");
            bool isPlayer = data.GetValue<bool>("IsPlayer");

            if (m_nSeatPos == seat.Pos)
            {
                PlayAudio(OperatorType.ZhiDui, 0, seat.Gender, SystemProxy.Instance.LanguageType);
            }

            if (m_nSeatPos == seat.Pos)
            {
                RoomEntity.RoomStatus roomStatus = data.GetValue<RoomEntity.RoomStatus>("RoomStatus");
                List<MaJiangCtrl> hand = MahJongManager.Instance.GetHand(m_nSeatPos);
                for (int i = 0; i < seat.HoldPoker.Count; ++i)
                {
                    for (int j = 0; j < seat.HoldPoker[i].Count; ++j)
                    {
                        for (int k = 0; k < hand.Count; ++k)
                        {
                            if (hand[k].Poker.index == seat.HoldPoker[i][j].index)
                            {
                                hand[k].Hold(isPlayer, RoomMaJiangProxy.Instance.CurrentRoom.isReplay);
                                break;
                            }
                        }
                    }
                }
                HandSort();
            }

            UIItemOperator.Instance.Close();
        }
        #endregion

        #region OnSwapPoker 选择交换牌
        /// <summary>
        /// 选择交换牌
        /// </summary>
        /// <param name="obj"></param>
        private void OnSwapPoker(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("SeatEntity");
            if (seat == null) return;
            if (seat.Pos != m_nSeatPos) return;

            SetSwap(seat.SwapPoker);
        }
        #endregion

        #region SetSwap 设置交换的牌
        /// <summary>
        /// 设置交换的牌
        /// </summary>
        /// <param name="pokers"></param>
        public void SetSwap(List<Poker> pokers)
        {
            if (pokers == null || pokers.Count == 0) return;
            List<MaJiangCtrl> lst = MahJongManager.Instance.GetHand(m_nSeatPos);
            if (lst != null)
            {
                List<MaJiangCtrl> swapPoker = new List<MaJiangCtrl>();
                for (int i = lst.Count - 1; i >= 0; --i)
                {
                    if (MahJongHelper.ContainPoker(lst[i].Poker, pokers))
                    {
                        swapPoker.Add(lst[i]);
                        lst.RemoveAt(i);
                    }
                }

                for (int i = 0; i < swapPoker.Count; ++i)
                {
                    MahJongManager.Instance.DespawnMaJiang(swapPoker[i]);
                }
            }

            for (int i = 0; i < pokers.Count; ++i)
            {
                MaJiangCtrl ctrl = MahJongManager.Instance.SpawnMaJiang(m_nSeatPos, pokers[i], "Table");
                ctrl.gameObject.SetParent(m_SwapContainer.transform);
                m_SwapPoker.Add(ctrl);
                if (pokers[i].color == 0)
                {
                    ctrl.Hold(false, false);
                }
            }
            m_SwapContainer.Sort();
            HandSort();
        }
        #endregion

        #region OnSetLackColor 设置缺门
        /// <summary>
        /// 设置缺门
        /// </summary>
        /// <param name="obj"></param>
        private void OnSetLackColor(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("SeatEntity");
            if (seat == null) return;
            if (seat.Pos != m_nSeatPos) return;
            List<MaJiangCtrl> lst = MahJongManager.Instance.GetHand(m_nSeatPos);
            for (int i = 0; i < lst.Count; ++i)
            {
                if (lst[i].Poker.color == 0) continue;
                if (lst[i].Poker.color == seat.LackColor)
                {
                    lst[i].isGray = true;
                }
            }

            if (seat.IsPlayer)
            {
                UIItemOperator.Instance.Close();
            }
        }
        #endregion

        #region InitWall 初始化墙
        /// <summary>
        /// 初始化墙
        /// </summary>
        /// <param name="wall"></param>
        public void InitWall(List<MaJiangCtrl> wall, bool isPlayAnimation)
        {
            for (int i = 0; i < wall.Count; ++i)
            {
                wall[i].gameObject.SetParent(m_WallContainer.transform);
            }
            m_WallContainer.Sort();
            if (isPlayAnimation)
            {
                StartCoroutine(InitWallAnimation(wall));
            }
        }
        #endregion

        #region InitWallCoroutine 初始化墙动画
        /// <summary>
        /// 初始化墙动画
        /// </summary>
        /// <param name="wall"></param>
        /// <returns></returns>
        private IEnumerator InitWallAnimation(List<MaJiangCtrl> wall)
        {
            for (int i = 0; i < wall.Count; ++i)
            {
                wall[i].gameObject.SetActive(false);
            }

            yield return null;

            m_WallTweener.OnComplete(() =>
            {
                for (int i = 0; i < wall.Count; ++i)
                {
                    wall[i].gameObject.SetActive(true);
                }
                m_WallTweener.PlayBackwards();
            }).Restart();
        }
        #endregion

        #region DrawPoker 摸牌
        /// <summary>
        /// 摸牌
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="isPlayAnimation"></param>
        public void DrawPoker(List<MaJiangCtrl> hand, bool isPlayAnimation = false)
        {
            if (isPlayAnimation)
            {

            }
            else
            {
                for (int i = 0; i < hand.Count; ++i)
                {
                    hand[i].gameObject.SetParent(m_HandContainer.transform);
                }
                HandSort();
            }
        }
        #endregion

        #region DrawPoker 摸牌
        /// <summary>
        /// 摸牌
        /// </summary>
        /// <param name="majiang"></param>
        /// <param name="isInit"></param>
        public void DrawPoker(MaJiangCtrl majiang, bool isInit = false)
        {
            if (isInit)
            {
                majiang.gameObject.SetParent(m_HandContainer.transform);
            }
            else
            {
                majiang.gameObject.SetParent(m_DrawContainer);
                m_HitPoker = majiang;
            }
            HandSort();
        }
        #endregion

        #region PlayPoker 出牌
        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="majiang"></param>
        /// <param name="isPlayAnimation"></param>
        public void PlayPoker(MaJiangCtrl majiang, bool isPlayAnimation, bool isPlayEffect, bool isJin)
        {
#if IS_GUGENG
            if (isJin)
            {
                PlayJin(majiang, isPlayAnimation);
                return;
            }
#endif
            majiang.gameObject.SetParent(m_DeskTopContainer.transform);
            majiang.gameObject.SetLayer(m_DeskTopContainer.gameObject.layer);
            majiang.transform.localPosition = m_DeskTopContainer.GetLocalPos(majiang.transform);
            if (s_CurrentMaJiang != null)
            {
                s_CurrentMaJiang.CloseTip();
            }
            HandSort();
            s_CurrentMaJiang = majiang;
            s_CurrentMaJiang.ShowTip(false);
            m_HitPoker = null;

            s_PlayPokerDestPos = majiang.transform.position;

            if (isPlayAnimation)
            {
#if IS_SHUANGLIAO

#else
                if (SystemProxy.Instance.HasHand)
                {
                    StartCoroutine(PlayPokerAnimation(majiang, isPlayEffect));
                }
                else
                {
                    if (isPlayEffect)
                    {
                        EffectManager.Instance.PlayEffectAsync("play_universal", (Transform effect) =>
                        {
                            effect.transform.position = majiang.transform.position + new Vector3(0, 7, 0);
                        }, 1f);
                    }
                }
#endif
            }

        }
        #endregion

        #region PlayPokerAnimation 出牌动画
        /// <summary>
        /// 出牌动画
        /// </summary>
        /// <param name="majiang"></param>
        /// <returns></returns>
        private IEnumerator PlayPokerAnimation(MaJiangCtrl majiang, bool isPlayEffect)
        {
            bool FirstPushLastPlace = true;
#if IS_LEPING
            FirstPushLastPlace = false;
#endif
            HandSort();
            if (m_DeskTopContainer.transform.childCount <= m_DeskTopContainer.constraintCount && FirstPushLastPlace)
            {
                if (m_PushHand != null)
                {
                    m_PushHand.Reset();
                    m_PushHand.transform.position = majiang.transform.position;
                }
                majiang.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.27f);
                Vector3 destPos = majiang.transform.localPosition;
                majiang.gameObject.SetActive(true);
                majiang.transform.localPosition = new Vector3(majiang.transform.localPosition.x, majiang.transform.localPosition.y, majiang.transform.localPosition.z - 15f);

                if (isPlayEffect)
                {
                    EffectManager.Instance.PlayEffectAsync("play_universal", (Transform effect) =>
                    {
                        effect.transform.position = majiang.transform.position + new Vector3(0, 7, 0);
                    }, 1f);
                }

                yield return new WaitForSeconds(0.22f);

                if (majiang != null)
                {
                    majiang.transform.DOLocalMove(destPos, 0.5f);
                }
            }
            else
            {
                if (m_DiceHand != null)
                {
                    m_DiceHand.Reset();
                    m_DiceHand.transform.position = majiang.transform.position;
                }
                majiang.gameObject.SetActive(false);


                yield return new WaitForSeconds(0.4f);
                if (isPlayEffect)
                {
                    EffectManager.Instance.PlayEffectAsync("play_universal", (Transform effect) =>
                    {
                        effect.transform.position = majiang.transform.position + new Vector3(0, 7, 0);
                    }, 1f);
                }
                majiang.gameObject.SetActive(true);
            }
        }
        #endregion

        #region PlayJin 打金
        /// <summary>
        /// 打金
        /// </summary>
        /// <param name="majiang"></param>
        /// <param name="isPlayAnimation"></param>
        public void PlayJin(MaJiangCtrl majiang, bool isPlayAnimation)
        {
            majiang.gameObject.SetParent(m_JinContainer.transform);
            majiang.gameObject.SetLayer(m_JinContainer.gameObject.layer);
            m_JinContainer.Sort();
            if (s_CurrentMaJiang != null)
            {
                s_CurrentMaJiang.CloseTip();
            }
            HandSort();
            s_CurrentMaJiang = majiang;
            s_CurrentMaJiang.ShowTip(false);
            m_HitPoker = null;

            s_PlayPokerDestPos = majiang.transform.position;

            if (isPlayAnimation)
            {
                if (SystemProxy.Instance.HasHand)
                {
                    StartCoroutine(PlayJinAnimation(majiang));
                }
            }
        }

        private IEnumerator PlayJinAnimation(MaJiangCtrl majiang)
        {
            HandSort();
            if (m_DiceHand != null)
            {
                m_DiceHand.Reset();
                m_DiceHand.transform.position = majiang.transform.position;
            }
            majiang.gameObject.SetActive(false);


            yield return new WaitForSeconds(0.4f);
            majiang.gameObject.SetActive(true);
        }
        #endregion

        #region HandSort 手牌排序
        /// <summary>
        /// 手牌排序
        /// </summary>
        public void HandSort(bool isDrawPoker = false)
        {
            List<MaJiangCtrl> lst = MahJongManager.Instance.GetHand(m_nSeatPos);

            if (lst == null) return;

            for (int i = 0; i < lst.Count; ++i)
            {
                lst[i].gameObject.SetParent(m_HandContainer.transform);
                lst[i].gameObject.SetLayer(m_HandContainer.gameObject.layer);
                lst[i].transform.SetSiblingIndex(i);
            }
            //处理支对排序
            for (int i = 0; i < lst.Count; ++i)
            {
                for (int j = 0; j < RoomMaJiangProxy.Instance.GetSeatBySeatId(m_nSeatPos).HoldPoker.Count; ++j)
                {
                    if (MahJongHelper.ContainPoker(lst[i].Poker, RoomMaJiangProxy.Instance.GetSeatBySeatId(m_nSeatPos).HoldPoker[j]))
                    {
                        lst[i].transform.SetSiblingIndex(0);
                    }
                }
            }

            //处理定将排序
            for (int i = 0; i < lst.Count; ++i)
            {
                if (MahJongHelper.ContainPoker(lst[i].Poker, RoomMaJiangProxy.Instance.GetSeatBySeatId(m_nSeatPos).DingJiangPoker))
                {
                    lst[i].transform.SetSiblingIndex(0);

                    if (!RoomMaJiangProxy.Instance.CurrentRoom.isReplay)
                    {
                        lst[i].Show(m_nSeatPos == RoomMaJiangProxy.Instance.PlayerSeat.Pos);
                    }
                }
            }
            //明提排序
            for (int i = 0; i < lst.Count; ++i)
            {
                if (MahJongHelper.ContainPoker(lst[i].Poker, RoomMaJiangProxy.Instance.GetSeatBySeatId(m_nSeatPos).MingTiPoker))
                {
                    lst[i].transform.SetSiblingIndex(0);

                    if (!RoomMaJiangProxy.Instance.CurrentRoom.isReplay)
                    {
                        lst[i].Show(m_nSeatPos == RoomMaJiangProxy.Instance.PlayerSeat.Pos);
                    }
                }
            }

            if (RoomMaJiangProxy.Instance.GetSeatBySeatId(m_nSeatPos).LackColor > 0)
            {
                List<MaJiangCtrl> temp = new List<MaJiangCtrl>();
                for (int i = 0; i < lst.Count; ++i)
                {
                    if (lst[i].Poker.color == RoomMaJiangProxy.Instance.GetSeatBySeatId(m_nSeatPos).LackColor)
                    {
                        temp.Add(lst[i]);
                    }
                }
                for (int i = 0; i < temp.Count; ++i)
                {
                    temp[i].transform.SetSiblingIndex(lst.Count);
                }
            }


            if (isDrawPoker)
            {
                Debug.Log("把牌放右边");
                if (RoomMaJiangProxy.Instance.CurrentRoom.CurrentOperator != null && RoomMaJiangProxy.Instance.CurrentRoom.CurrentOperator.Pos == m_nSeatPos)
                {
                    SeatEntity seat = RoomMaJiangProxy.Instance.GetSeatBySeatId(m_nSeatPos);
                    if (seat.HitPoker != null)
                    {
                        for (int i = 0; i < lst.Count; ++i)
                        {
                            if (lst[i].Poker.index == seat.HitPoker.index)
                            {
                                lst[i].gameObject.SetParent(m_DrawContainer);
                                break;
                            }
                        }
                    }
                    else
                    {
                        lst[lst.Count - 1].gameObject.SetParent(m_DrawContainer);
                    }
                }
            }
            else
            {
                SeatEntity seat = RoomMaJiangProxy.Instance.GetSeatBySeatId(m_nSeatPos);
                if (seat.HitPoker != null)
                {
                    for (int i = 0; i < lst.Count; ++i)
                    {
                        if (lst[i].Poker.index == seat.HitPoker.index)
                        {
                            lst[i].gameObject.SetParent(m_DrawContainer);
                            break;
                        }
                    }
                }
            }

            m_HandContainer.Sort();
        }
        #endregion

        #region Operate 吃碰杠
        /// <summary>
        /// 吃碰杠
        /// </summary>
        /// <param name="combination"></param>
        public void Operate(Combination3D combination, bool isDrawPoker)
        {
            if (s_CurrentMaJiang != null)
            {
                s_CurrentMaJiang.CloseTip();
            }

            if (combination == null) return;
            if (combination.OperatorType != OperatorType.Gang || (combination.OperatorType == OperatorType.Gang && (SubOperateType)combination.SubTypeId != SubOperateType.AnGang))
            {
                m_isOpenDoor = true;
            }

            if (combination.OperatorType == OperatorType.Hu) return;

            if (combination.OperatorType == OperatorType.BuHua)
            {
                if (m_BuHuaContainer_New != null)
                {
                    combination.PokerList[0].gameObject.SetParent(m_BuHuaContainer_New.transform);
                    m_BuHuaContainer_New.Sort();
                }
                else
                {
                    combination.PokerList[0].gameObject.SetParent(m_BuHuaContainer.transform);
                    m_BuHuaContainer.Sort();
                }
                HandSort();
                return;
            }

            if (combination.OperatorType == OperatorType.BuXi)
            {
                for (int i = 0; i < m_PengCtrls.Length; ++i)
                {
                    if (m_PengCtrls[i].Combination == null) continue;
                    if (m_PengCtrls[i].Combination.OperatorType == OperatorType.LiangXi)
                    {
                        m_PengCtrls[i].SetUI(combination, m_nSeatPos);
                    }
                }
                HandSort();
                return;
            }

            for (int i = 0; i < m_PengCtrls.Length; ++i)
            {
                if (m_PengCtrls[i].Combination == combination)
                {
                    m_PengCtrls[i].SetUI(combination, m_nSeatPos);
                    break;
                }
                if (m_PengCtrls[i].Combination == null)
                {
                    m_PengCtrls[i].SetUI(combination, m_nSeatPos);
                    break;
                }
            }

#if !IS_WANGQUE
            if (m_isOpenDoor)
            {
                for (int i = 0; i < m_PengCtrls.Length; ++i)
                {
                    if (m_PengCtrls[i].Combination != null && m_PengCtrls[i].Combination.OperatorType == OperatorType.Gang)
                    {
                        m_PengCtrls[i].OpenDoor();
                    }
                }
            }
#endif

            m_HitPoker = null;
            HandSort(isDrawPoker);
        }
        #endregion

        #region DingJiang 定将
        /// <summary>
        /// 定将
        /// </summary>
        /// <param name="combination"></param>
        private void DingJiang(PokerCombinationEntity combination, bool isPlayer)
        {
            List<MaJiangCtrl> dingjiang = MahJongManager.Instance.DingJiang(m_nSeatPos, combination.PokerList);
            for (int k = 0; k < dingjiang.Count; ++k)
            {
                dingjiang[k].gameObject.SetParent(m_HandContainer.transform, false);
                dingjiang[k].gameObject.SetLayer((isPlayer && RoomMaJiangProxy.Instance.CurrentRoom.isReplay ? "PlayerHand" : "Table"));
                dingjiang[k].transform.SetSiblingIndex(0);
            }
            HandSort();
        }
        #endregion

        #region ShowSettle 显示结算
        /// <summary>
        /// 显示结算
        /// </summary>
        /// <param name="handPokers"></param>
        /// <param name="hitPoker"></param>
        public void ShowSettle(List<Poker> handPokers, Poker hitPoker)
        {
            List<MaJiangCtrl> hand = MahJongManager.Instance.GetHand(m_nSeatPos);
            if (hand == null || hand.Count == 0) return;
            hand = new List<MaJiangCtrl>(hand);
            if (m_HitPoker != null)
            {
                for (int i = 0; i < hand.Count; ++i)
                {
                    if (hand[i] == m_HitPoker)
                    {
                        hand.RemoveAt(i);
                        break;
                    }
                }
            }
            Debug.Log("数据数量" + handPokers.Count);
            Debug.Log("模型数量" + hand.Count);
            if (hand.Count != handPokers.Count)
            {
                AppDebug.ThrowError("模型数量和数据不对等");
            }
            for (int i = 0; i < handPokers.Count; ++i)
            {
                MaJiangCtrl majiang = hand[0];

                MaJiangCtrl newMajiang = MahJongManager.Instance.SpawnMaJiang(m_nSeatPos, handPokers[i], "Hand");

                newMajiang.transform.SetParent(m_HandContainer.transform);
                newMajiang.transform.localPosition = majiang.transform.position;
                newMajiang.transform.localRotation = majiang.transform.localRotation;
                newMajiang.transform.localScale = majiang.transform.localScale;

                MahJongManager.Instance.DespawnMaJiang(majiang);
                hand.Remove(majiang);

                hand.Add(newMajiang);
            }

            if (hitPoker != null && hitPoker.size != 0 && m_HitPoker != null)
            {
                MaJiangCtrl majiang = m_HitPoker;

                MaJiangCtrl newMajiang = MahJongManager.Instance.SpawnMaJiang(m_nSeatPos, hitPoker, "Hand");

                newMajiang.gameObject.SetParent(m_DrawContainer.transform);

                MahJongManager.Instance.DespawnMaJiang(majiang);
                hand.Remove(majiang);

                hand.Add(newMajiang);

//#if !IS_GONGXIAN
//                SetDrawPokerState(PokerState.Normal);
//                newMajiang.transform.localEulerAngles = new Vector3(90, 0, 0);
//                m_DrawContainer.gameObject.SetLayer("Hand");
//#endif
            }
//#if IS_GONGXIAN
            SetDrawPokerState(PokerState.Show);
//#endif
            SetHandPokerState(PokerState.Show);

            m_HandContainer.Sort();
        }
        #endregion

        #region CheckTing 检测听牌
        /// <summary>
        /// 检测听牌
        /// </summary>
        public void CheckTing()
        {
            if (RoomMaJiangProxy.Instance.CurrentRoom.isReplay) return;
            SeatEntity seat = RoomMaJiangProxy.Instance.GetSeatBySeatId(m_nSeatPos);
            if (seat == null) return;
            if (seat.isHu) return;
            for (int i = 0; i < m_LightMaJiang.Count; ++i)
            {
                m_LightMaJiang[i].CloseTip();
            }
            m_LightMaJiang.Clear();

            Dictionary<Poker, List<Poker>> dic = RoomMaJiangProxy.Instance.CheckAllTing();
            if (dic != null && dic.Count > 0)
            {
                List<MaJiangCtrl> lstHand = MahJongManager.Instance.GetHand(RoomMaJiangProxy.Instance.PlayerSeat.Pos);
                if (lstHand == null) return;
                Debug.Log(RoomMaJiangProxy.Instance.Rule.isTing);
                Debug.Log(RoomMaJiangProxy.Instance.PlayerSeat.IsTing);
                Debug.Log(RoomMaJiangProxy.Instance.PlayerSeat.isLockTing);
                if (RoomMaJiangProxy.Instance.Rule.isTing)
                {
                    UIItemOperator.Instance.ShowTing((!RoomMaJiangProxy.Instance.PlayerSeat.IsTing && !RoomMaJiangProxy.Instance.PlayerSeat.isLockTing));
                }
                foreach (var pair in dic)
                {
                    for (int i = 0; i < lstHand.Count; ++i)
                    {
                        if (pair.Key.index == lstHand[i].Poker.index)
                        {
                            lstHand[i].ShowTip(true);
                            m_LightMaJiang.Add(lstHand[i]);
                            break;
                        }
                    }

                }
            }
            else
            {
                UIItemOperator.Instance.ShowTing(false);
            }
        }
        #endregion

        public IEnumerator PlayDealAnimation()
        {
            PlayDealAnimation(null);
            yield return new WaitForSeconds(INIT_POKER_ANIMATION_DURATION * 2);
        }

        #region PlayDealnimation 播放发牌动画
        /// <summary>
        /// 播放发牌动画
        /// </summary>
        /// <param name="onComplete"></param>
        public void PlayDealAnimation(TweenCallback onComplete)
        {
            m_HandTween.OnComplete(() =>
            {
                MahJongManager.Instance.Sort(MahJongManager.Instance.GetHand(m_nSeatPos), RoomMaJiangProxy.Instance.PlayerSeat.UniversalList);
                HandSort();
                m_HandTween.PlayBackwards();
            }).OnRewind(() =>
            {
                if (onComplete != null)
                {
                    onComplete();
                }
            }).Restart();
        }
        #endregion

        #region SetUniversal 设置万能牌
        /// <summary>
        /// 设置万能牌
        /// </summary>
        /// <param name="universal"></param>
        public void SetUniversal(List<Poker> universal)
        {
            List<MaJiangCtrl> hand = MahJongManager.Instance.GetHand(m_nSeatPos);
            if (hand == null) return;
            MahJongManager.Instance.Sort(hand, universal);
            for (int i = 0; i < hand.Count; ++i)
            {
                if (hand[i].Poker != null && MahJongHelper.CheckUniversal(hand[i].Poker, universal))
                {
                    hand[i].SetUniversal(true, RoomMaJiangProxy.Instance.CurrentRoom.PokerTotalPerPlayer == 17);
                }
            }
            HandSort();
        }
        #endregion

        #region SetHandPokerState 设置手牌状态
        /// <summary>
        /// 设置手牌状态
        /// </summary>
        /// <param name="state"></param>
        private void SetHandPokerState(PokerState state)
        {
            switch (state)
            {
                case PokerState.Normal:
                    m_HandContainer.transform.localEulerAngles = new Vector3(-90, -90, 0);
                    break;
                case PokerState.Hold:
                    m_HandContainer.transform.localEulerAngles = new Vector3(0, 90, 180);
                    m_HandContainer.gameObject.SetLayer("Hand");
                    break;
                case PokerState.Show:
                    m_HandContainer.transform.localEulerAngles = new Vector3(180, 90, 180);
                    m_HandContainer.gameObject.SetLayer("Hand");
                    break;
            }
        }
        #endregion

        #region SetDrawPokerState 设置摸的牌状态
        /// <summary>
        /// 设置摸的牌状态
        /// </summary>
        /// <param name="state"></param>
        private void SetDrawPokerState(PokerState state)
        {
            switch (state)
            {
                case PokerState.Normal:
                    m_DrawContainer.transform.localEulerAngles = new Vector3(-90, -90, 0);
                    break;
                case PokerState.Hold:
                    m_DrawContainer.transform.localEulerAngles = new Vector3(180, -90, 0);
                    m_DrawContainer.gameObject.SetLayer("Hand");
                    break;
                case PokerState.Show:
                    m_DrawContainer.transform.localEulerAngles = new Vector3(0, -90, 0);
                    m_DrawContainer.gameObject.SetLayer("Hand");
                    break;
            }
        }
        #endregion


        #region PlaySwapAnimation 播放交换牌动画
        /// <summary>
        /// 播放交换牌动画
        /// </summary>
        /// <param name="swapType"></param>
        public void PlaySwapAnimation(int angles, List<Poker> pokers)
        {
            Vector3 srcAngle = m_SwapContainer.transform.parent.localEulerAngles;

            Vector3 angle = new Vector3(0, angles, 0);
            m_SwapContainer.transform.parent.DORotate(angle, 1f, RotateMode.LocalAxisAdd).OnComplete(() =>
            {
                m_SwapContainer.transform.parent.localEulerAngles = srcAngle;
                for (int i = 0; i < m_SwapPoker.Count; ++i)
                {
                    MahJongManager.Instance.DespawnMaJiang(m_SwapPoker[i]);
                }
                m_SwapPoker.Clear();

                for (int i = 0; i < pokers.Count; ++i)
                {
                    MaJiangCtrl ctrl = MahJongManager.Instance.SpawnMaJiang(m_nSeatPos, pokers[i], LayerMask.LayerToName(m_HandContainer.gameObject.layer));
                    MahJongManager.Instance.GetHand(m_nSeatPos).Add(ctrl);
                    MahJongManager.Instance.Sort(MahJongManager.Instance.GetHand(m_nSeatPos), RoomMaJiangProxy.Instance.GetSeatBySeatId(m_nSeatPos).UniversalList);
                    ctrl.gameObject.SetParent(m_HandContainer.transform);
                }
                HandSort();
            });
        }
        #endregion


        private void PlayAudio(OperatorType type, int subType, int gender, LanguageType languageType)
        {
            string audioName = type.ToString().ToLower();
            if (type == OperatorType.Hu)
            {
                if (subType == 1)
                {
                    audioName = "hu";
                }
                else if (subType == 2)
                {
                    audioName = "zimo";
                }
            }
            audioName = gender.ToString() + "_" + audioName;
            if (languageType != LanguageType.Mandarin)
            {
                audioName = string.Format("{0}/", languageType.ToString().ToLower()) + audioName;
            }
            AudioEffectManager.Instance.Play(audioName);
        }

        private void PlayAudio(int color, int size, int gender, LanguageType languageType)
        {
            string audioName = string.Format("{0}_{1}_{2}", gender, color, size);
            if (languageType != LanguageType.Mandarin)
            {
                audioName = string.Format("{0}/", languageType.ToString().ToLower()) + audioName;
            }
            AudioEffectManager.Instance.Play(audioName);
        }
    }
}
