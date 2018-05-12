//===================================================
//Author      : DRB
//CreateTime  ：4/27/2017 1:53:29 PM
//Description ：麻将模型代理层
//===================================================
using System.Collections.Generic;
using System.Text;
using proto.mahjong;
using UnityEngine;

namespace DRB.MahJong
{
    public class RoomMaJiangProxy : ProxyBase<RoomMaJiangProxy>
    {
        #region Notify Constant
        /// <summary>
        /// 开局
        /// </summary>
        public const string ON_BEGIN = "OnMahjongBegin";
        /// <summary>
        /// 座位方向变更
        /// </summary>
        public const string ON_SEAT_DIRECTION_CHANGED = "OnMahjongSeatDirectionChanged";
        /// <summary>
        /// 摸牌
        /// </summary>
        public const string ON_DRAW_POKER = "OnMahjongDrawPoker";
        /// <summary>
        /// 出牌
        /// </summary>
        public const string ON_PLAY_POKER = "OnMahjongPlayPoker";
        /// <summary>
        /// 吃碰杠
        /// </summary>
        public const string ON_OPERATE = "OnMahjongOperate";
        /// <summary>
        /// 听牌
        /// </summary>
        public const string ON_TING = "OnMahjongTing";
        /// <summary>
        /// 支对
        /// </summary>
        public const string ON_ZHIDUI = "OnMahjongZhidui";
        /// <summary>
        /// 胡牌
        /// </summary>
        public const string ON_HU = "OnMahjongHu";
        /// <summary>
        /// 结算
        /// </summary>
        public const string ON_SETTLE = "OnMahjongSettle";
        /// <summary>
        /// 当前操作者变更
        /// </summary>
        public const string ON_CURRENT_OPERATOR_CHANGED = "OnMahjongCurrentOperatorChanged";
        /// <summary>
        /// 座位金币变更
        /// </summary>
        public const string ON_SEAT_GOLD_CHANGED = "OnMahjongSeatGoldChanged";
        /// <summary>
        /// 座位信息清空
        /// </summary>
        public const string ON_SEAT_INFO_CLEAR = "OnMahjongSeatInfoClear";
        /// <summary>
        /// 倒计时变更
        /// </summary>
        public const string ON_COUNTDOWN_CHANGED = "OnMahjongCountDownChanged";
        /// <summary>
        /// 房间信息变更
        /// </summary>
        public const string ON_ROOM_INFO_CHANGED = "OnMahjongRoomInfoChanged";
        /// <summary>
        /// 座位信息变更
        /// </summary>
        public const string ON_SEAT_INFO_CHANGED = "OnMahjongSeatInfoChanged";

        public const string ON_COUNT_DOWN_CHANGED = "OnCountDownUpdate";
        /// <summary>
        /// 明提
        /// </summary>
        public const string ON_MING_TI = "OnMahjongMingTi";
        /// <summary>
        /// 交换牌
        /// </summary>
        public const string ON_SWAP_POKER = "OnMahjongSwapPoker";
        /// <summary>
        /// 定缺
        /// </summary>
        public const string ON_SET_LACK_COLOR = "OnMahjongLackColor";
        #endregion


        #region Variable
        /// <summary>
        /// 当前房间数据实体
        /// </summary>
        public RoomEntity CurrentRoom;
        /// <summary>
        /// 玩家座位数据实体
        /// </summary>
        public SeatEntity PlayerSeat;
        /// <summary>
        /// 房间座位数量
        /// </summary>
        public const int ROOM_SEAT_COUNT = 4;
        /// <summary>
        /// 当前游戏状态
        /// </summary>
        public MahjongGameState CurrentState;
        /// <summary>
        /// 询问吃碰杠胡的网络数据
        /// </summary>
        public List<PokerCombinationEntity> AskPokerGroup;
        /// <summary>
        /// 游戏规则
        /// </summary>
        public MahjongRule Rule;
        /// <summary>
        /// 听牌字典
        /// key:打哪张可以听
        /// value:胡什么牌
        /// </summary>
        private Dictionary<Poker, List<Poker>> m_DicTing = new Dictionary<Poker, List<Poker>>();

        public List<List<Poker>> AllCanLiangXi = new List<List<Poker>>();
        #endregion


        #region InitRoom 初始化房间数据
        public void InitRoom(OP_ROOM_INFO proto)
        {
            m_DicTing.Clear();

            int currentOperatorPos = 0;

            CurrentState = MahjongGameState.DrawPoker;

            CurrentRoom = new RoomEntity()
            {
                BaseScore = proto.baseScore,
                currentLoop = proto.loop,
                PokerAmount = proto.pokerAmount,
                PokerTotal = proto.pokerTotal,
                roomId = proto.roomId,
                matchId = proto.matchId,
                Status = (RoomEntity.RoomStatus)proto.status,
                maxLoop = proto.maxLoop,
                DisbandTime = proto.dismissTime,
                DisbandTimeMax = (int)proto.dismissMaxTime,
            };
            if (proto.luckPoker != null)
            {
                CurrentRoom.LuckPoker = new Poker(proto.luckPoker.index, proto.luckPoker.color, proto.luckPoker.size, proto.luckPoker.pos);
            }
            else
            {
                CurrentRoom.LuckPoker = null;
            }
            CurrentRoom.Config.Clear();
            for (int i = 0; i < proto.settingIdCount(); ++i)
            {
                cfg_settingEntity settingEntity = cfg_settingDBModel.Instance.Get(proto.getSettingId(i));
                if (settingEntity != null)
                {
                    CurrentRoom.gameId = settingEntity.gameId;
                    CurrentRoom.Config.Add(settingEntity);
                }
            }
            CurrentRoom.SeatList = new List<SeatEntity>();
            for (int i = 0; i < proto.seatCount(); ++i)
            {
                OP_SEAT_FULL op_seat = proto.getSeat(i);
                SeatEntity seat = new SeatEntity();
                seat.Pos = op_seat.pos;
                if (proto.seatCount() == 2 && seat.Pos == 2)
                {
                    seat.Pos = 3;
                }
                seat.PlayerId = op_seat.playerId;
                seat.Gender = op_seat.gender;
                seat.Gold = op_seat.gold;
                seat.Nickname = op_seat.nickname;
                seat.Avatar = op_seat.avatar;
                seat.IP = op_seat.ipaddr;
                seat.Status = (SeatEntity.SeatStatus)op_seat.status;
                Debug.Log(seat.Status);
                if (seat.Status == SeatEntity.SeatStatus.Operate)
                {
                    currentOperatorPos = seat.Pos;
                }
                if (seat.Status == SeatEntity.SeatStatus.Operate)
                {
                    CurrentState = MahjongGameState.Operate;
                }
                seat.IsTing = op_seat.isListen;
                seat.IsTrustee = op_seat.isTrustee;
                seat.isLockTing = op_seat.isLockListen;
                seat.Latitude = op_seat.latitude;
                seat.Longitude = op_seat.longitude;
                seat.IsFocus = !op_seat.isAfk;
                seat.isDouble = op_seat.isDouble;
                seat.DisbandState = (DisbandState)op_seat.dismiss;
                seat.isHu = seat.Status == SeatEntity.SeatStatus.Finish;
                seat.IsPlayer = AccountProxy.Instance.CurrentAccountEntity.passportId == seat.PlayerId;
                seat.LackColor = op_seat.lackColor;
                if (op_seat.hasHitPoker())
                {
                    seat.HitPoker = new Poker(op_seat.hitPoker.index, op_seat.hitPoker.color, op_seat.hitPoker.size, op_seat.hitPoker.pos);
                }
                for (int j = 0; j < op_seat.pokerCount(); ++j)
                {

                    OP_POKER protoPoker = op_seat.getPoker(j);
                    seat.PokerList.Add(new Poker(protoPoker.index, protoPoker.color, protoPoker.size, protoPoker.pos));
                }
                for (int j = 0; j < op_seat.desktopCount(); ++j)
                {

                    OP_POKER protoPoker = op_seat.getDesktop(j);
                    seat.DeskTopPoker.Add(new Poker(protoPoker.index, protoPoker.color, protoPoker.size, protoPoker.pos));
                }
                for (int j = 0; j < op_seat.usePokerGroupCount(); ++j)
                {
                    OP_POKER_GROUP protoPoker = op_seat.getUsePokerGroup(j);
                    List<Poker> lst = new List<Poker>();
                    for (int k = 0; k < protoPoker.pokerCount(); ++k)
                    {
                        OP_POKER op_poker = protoPoker.getPoker(k);
                        Poker poker = new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos);
                        lst.Add(poker);
                    }
                    if (protoPoker.typeId != ENUM_POKER_TYPE.POKER_TYPE_GANG)
                    {
                        MahJongHelper.SimpleSort(lst);
                    }
#if IS_DAZHONG || IS_WUANJUN
                if ((OperatorType)protoPoker.typeId == OperatorType.Chi)//吃的牌放中间
                {
                    for (int k = 0; k < lst.Count; ++k)
                    {
                        if (lst[k].pos != seat.Pos)
                        {
                            Poker poker = lst[k];
                            lst.Remove(poker);
                            lst.Insert(1, poker);
                        }
                    }
                }

                if ((OperatorType)protoPoker.typeId == OperatorType.Peng || (OperatorType)protoPoker.typeId == OperatorType.Gang)
                {
                    for (int k = 0; k < lst.Count; ++k)
                    {
                        if (lst[k].pos != op_seat.pos)
                        {
                            Poker poker = lst[k];
                            lst.Remove(poker);
                            if (Mathf.Abs(poker.pos - op_seat.pos) == 2)
                            {
                                lst.Insert(1, poker);
                                break;
                            }
                            if (poker.pos - op_seat.pos == 1 || poker.pos - op_seat.pos == -3)
                            {
                                lst.Insert(2, poker);
                                break;
                            }
                            if (poker.pos - op_seat.pos == -1 || poker.pos - op_seat.pos == 3)
                            {
                                lst.Insert(0, poker);
                                break;
                            }
                        }
                    }
                }
#endif
                    PokerCombinationEntity combination = new PokerCombinationEntity((OperatorType)protoPoker.typeId, (int)protoPoker.subTypeId, lst);

                    seat.UsedPokerList.Add(combination);
                }
                for (int j = 0; j < op_seat.universalCount(); ++j)
                {
                    OP_POKER op_poker = op_seat.getUniversal(j);
                    seat.UniversalList.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
                }
                seat.PokerAmount = op_seat.pokerAmount;
                seat.IsBanker = op_seat.isBanker;
                seat.IsTrustee = op_seat.isTrustee;
                seat.IsWaiver = op_seat.isWaiver;
                for (int j = 0; j < op_seat.keepPokerGroupCount(); ++j)
                {
                    OP_POKER_GROUP op_poker_group = op_seat.getKeepPokerGroup(j);
                    List<Poker> lstPoker = new List<Poker>();
                    for (int k = 0; k < op_poker_group.pokerCount(); ++k)
                    {
                        OP_POKER op_poker = op_poker_group.getPoker(k);
                        lstPoker.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
                    }
                    seat.HoldPoker.Add(lstPoker);
                }
                for (int j = 0; j < op_seat.dingPokerGroupCount(); ++j)
                {
                    OP_POKER_GROUP op_poker_group = op_seat.getDingPokerGroup(j);
                    List<Poker> lstPoker = new List<Poker>();
                    for (int k = 0; k < op_poker_group.pokerCount(); ++k)
                    {
                        OP_POKER op_poker = op_poker_group.getPoker(k);
                        lstPoker.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
                    }
                    seat.DingJiangPoker.AddRange(lstPoker);
                }

                for (int j = 0; j < op_seat.swapCount(); ++j)
                {
                    OP_POKER op_poker = op_seat.getSwap(j);
                    seat.SwapPoker.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
                }
                seat.Direction = op_seat.wind;
                if (proto.seatCount() == 2 && seat.Direction == 2)
                {
                    seat.Direction = 3;
                }
                CurrentRoom.SeatList.Add(seat);
            }

            if (CurrentRoom.SeatList.Count == 2)
            {
                for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
                {
                    for (int j = 0; j < CurrentRoom.SeatList[i].UsedPokerList.Count; ++j)
                    {
                        for (int k = 0; k < CurrentRoom.SeatList[i].UsedPokerList[j].PokerList.Count; ++k)
                        {
                            if (CurrentRoom.SeatList[i].UsedPokerList[j].PokerList[k].pos == 2)
                            {
                                CurrentRoom.SeatList[i].UsedPokerList[j].PokerList[k].pos = 3;
                            }
                        }
                    }
                }
            }

            int applyCount = 0;
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                if (CurrentRoom.SeatList[i].DisbandState == DisbandState.Apply)
                {
                    ++applyCount;
                }
            }
            if (applyCount > 1)
            {
                for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
                {
                    CurrentRoom.SeatList[i].DisbandState = DisbandState.Wait;
                }
            }

            CurrentRoom.FirstDice = new DiceEntity()
            {
                diceA = proto.diceFirstA,
                diceB = proto.diceFirstB,
                seatPos = proto.diceFirst,
            };
            CurrentRoom.SecondDice = new DiceEntity()
            {
                diceA = proto.diceSecondA,
                diceB = proto.diceSecondB,
                seatPos = proto.diceSecond,
            };

            int askLeng = proto.askPokerGroupCount();
            if (askLeng > 0)
            {
                AskPokerGroup = new List<PokerCombinationEntity>();
                for (int i = 0; i < askLeng; ++i)
                {
                    OP_POKER_GROUP op_group = proto.getAskPokerGroup(i);
                    List<Poker> lst = new List<Poker>();
                    for (int j = 0; j < op_group.pokerCount(); ++j)
                    {
                        OP_POKER op_poker = op_group.getPoker(j);
                        lst.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
                    }
                    PokerCombinationEntity combination = new PokerCombinationEntity((OperatorType)op_group.typeId, (int)op_group.subTypeId, lst);
                    AskPokerGroup.Add(combination);
                }
            }
            else
            {
                AskPokerGroup = null;
            }

            CalculateSeatIndex();

            InitConfig();

            if (currentOperatorPos == 0)
            {
                currentOperatorPos = CurrentRoom.BankerPos;
            }

            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                SeatEntity seat = CurrentRoom.SeatList[i];
                for (int j = 0; j < CurrentRoom.SeatList.Count; ++j)
                {
                    for (int k = 0; k < CurrentRoom.SeatList[j].UsedPokerList.Count; ++k)
                    {
                        for (int l = 0; l < CurrentRoom.SeatList[j].UsedPokerList[k].PokerList.Count; ++l)
                        {
                            if (CurrentRoom.SeatList[j].UsedPokerList[k].PokerList[l].pos == seat.Pos && MahJongHelper.HasPoker(CurrentRoom.SeatList[j].UsedPokerList[k].PokerList[l], seat.UniversalList))
                            {
                                seat.isPlayedUniversal = true;
                                break;
                            }
                        }
                        if (seat.isPlayedUniversal) break;
                    }
                    if (seat.isPlayedUniversal) break;
                    for (int k = 0; k < CurrentRoom.SeatList[j].DeskTopPoker.Count; ++k)
                    {
                        if (CurrentRoom.SeatList[j].DeskTopPoker[k].pos == seat.Pos && MahJongHelper.HasPoker(CurrentRoom.SeatList[j].DeskTopPoker[k], seat.UniversalList))
                        {
                            seat.isPlayedUniversal = true;
                            break;
                        }
                    }
                    if (seat.isPlayedUniversal) break;
                    for (int k = 0; k < CurrentRoom.SeatList[j].PokerList.Count; ++k)
                    {
                        if (CurrentRoom.SeatList[j].PokerList[k].pos == seat.Pos && MahJongHelper.HasPoker(CurrentRoom.SeatList[j].PokerList[k], seat.UniversalList))
                        {
                            seat.isPlayedUniversal = true;
                            break;
                        }
                    }
                    if (seat.isPlayedUniversal) break;
                }
            }

            for (int i = 0; i < CurrentRoom.Config.Count; ++i)
            {
                if (CurrentRoom.Config[i].tags.Equals("handTotal"))
                {
                    CurrentRoom.PokerTotalPerPlayer = CurrentRoom.Config[i].value;
                    break;
                }
            }

            if (CurrentRoom.LuckPoker != null && CurrentRoom.LuckPoker.color != 0)
            {
                SetLuckPoker(CurrentRoom.LuckPoker.index, CurrentRoom.LuckPoker.color, CurrentRoom.LuckPoker.size);
            }

            SetCurrentOperator(currentOperatorPos, true);
        }

        public void InitRoom(RecordReplayEntity replayEntity)
        {
            CurrentRoom = new RoomEntity();
            CurrentRoom.roomId = replayEntity.roomId;
            CurrentRoom.PokerTotal = replayEntity.pokerTotal;
            CurrentRoom.PokerAmount = CurrentRoom.PokerTotal;
            CurrentRoom.BaseScore = replayEntity.baseScore;
            CurrentRoom.LuckPoker = replayEntity.luckPoker;
            CurrentRoom.Config.Clear();
            for (int i = 0; i < replayEntity.setting.Length; ++i)
            {
                cfg_settingEntity settingEntity = cfg_settingDBModel.Instance.Get(replayEntity.setting[i]);
                if (settingEntity != null)
                {
                    CurrentRoom.Config.Add(settingEntity);
                }
            }
            CurrentRoom.SeatList = new List<SeatEntity>();
            if (replayEntity.prob != null)
            {
                CurrentRoom.Prob = replayEntity.prob.poker;
                CurrentRoom.ProbCount = replayEntity.prob.poker == null ? 0 : replayEntity.prob.poker.Count;
            }
            for (int i = 0; i < replayEntity.seat.Count; ++i)
            {
                SeatEntity seat = new SeatEntity();
                seat.Avatar = replayEntity.seat[i].avatar;
                seat.Gender = replayEntity.seat[i].gender;
                seat.Nickname = replayEntity.seat[i].nickname;
                seat.PlayerId = replayEntity.seat[i].playerId;
                seat.PokerList = replayEntity.seat[i].poker;
                CurrentRoom.PokerAmount -= seat.PokerList.Count;
                seat.Pos = replayEntity.seat[i].pos;
                if (replayEntity.seat.Count == 2 && seat.Pos == 2)
                {
                    seat.Pos = 3;
                }
                seat.IsBanker = replayEntity.banker == seat.Pos;
                seat.PokerAmount = replayEntity.seat[i].poker.Count;
                seat.Gold = replayEntity.seat[i].gold;
                seat.Settle = replayEntity.seat[i].settle;
                seat.ProbMulti = replayEntity.seat[i].multiply;
                seat.isWiner = replayEntity.seat[i].isWinner;
                seat.isLoser = replayEntity.seat[i].isLoser;
                seat.LackColor = replayEntity.seat[i].lackColor;
                if (replayEntity.seat[i].universal != null)
                {
                    for (int j = 0; j < replayEntity.seat[i].universal.Count; ++j)
                    {
                        seat.UniversalList.Add(replayEntity.seat[i].universal[j]);
                    }
                }
                seat.Direction = replayEntity.seat[i].wind;
                if (replayEntity.seat.Count == 2 && seat.Direction == 2)
                {
                    seat.Direction = 3;
                }
                CurrentRoom.SeatList.Add(seat);
            }
            CurrentRoom.isReplay = true;
            CurrentRoom.Status = RoomEntity.RoomStatus.Replay;
            CurrentRoom.FirstDice = new DiceEntity()
            {
                seatPos = replayEntity.firstDicePos,
                diceA = replayEntity.firstDiceA,
                diceB = replayEntity.firstDiceB,
            };
            CurrentRoom.SecondDice = new DiceEntity()
            {
                seatPos = replayEntity.secondDicePos,
                diceA = replayEntity.secondDiceA,
                diceB = replayEntity.secondDiceB,
            };

            for (int i = 0; i < replayEntity.seat.Count; ++i)
            {
                if (replayEntity.seat[i].incomes != null)
                {
                    for (int j = 0; j < replayEntity.seat[i].incomes.Count; ++j)
                    {
                        IncomeDetailEntity detail = new IncomeDetailEntity(replayEntity.seat[i].incomes[j].cfgId, replayEntity.seat[i].incomes[j].times);
                        SeatEntity seat = GetSeatByPlayerId(replayEntity.seat[i].playerId);
                        seat.SettleInfo.Add(detail);
                    }
                }
                if (replayEntity.seat[i].scores != null)
                {
                    for (int j = 0; j < replayEntity.seat[i].scores.Count; ++j)
                    {
                        IncomeDetailEntity detail = new IncomeDetailEntity(replayEntity.seat[i].scores[j].cfgId, replayEntity.seat[i].scores[j].times);
                        SeatEntity seat = GetSeatByPlayerId(replayEntity.seat[i].playerId);
                        seat.HuScoreDetail.Add(detail);
                    }
                }
            }

            if (replayEntity.record != null)
            {
                for (int i = 0; i < replayEntity.record.Count; ++i)
                {
                    if (replayEntity.record[i].operate == OP_ROOM_FIGHT.CODE && replayEntity.record[i].type == (int)OperatorType.Hu)
                    {
                        SeatEntity seat = GetSeatByPlayerId(replayEntity.record[i].playerId);
                        if (seat != null)
                        {
                            seat.isWiner = true;
                        }
                    }
                }
            }

            CalculateSeatIndex();

            InitConfig();

            if (CurrentRoom.LuckPoker != null && CurrentRoom.LuckPoker.color != 0)
            {
                SetLuckPoker(CurrentRoom.LuckPoker.index, CurrentRoom.LuckPoker.color, CurrentRoom.LuckPoker.size);
            }
        }
        #endregion

        #region InitConfig 初始化房间配置
        /// <summary>
        /// 初始化房间配置
        /// </summary>
        public void InitConfig()
        {
            Rule = new MahjongRule();
            int gameId = 0;
            if (CurrentRoom.Config.Count > 0)
            {
                gameId = CurrentRoom.Config[0].gameId;
            }
            if (gameId == 0) return;
            Debug.Log("当前游戏Id" + gameId.ToString());
            switch (gameId)
            {
                case 39://血战
                    Rule.isXuanFengGang = false;
                    break;
                case 40://启东
                    Rule.isXuanFengGang = false;
                    break;
                case 41://东北
                    Rule.isXuanFengGang = true;
                    Rule.IsMustHas19 = true;
                    Rule.IsHongZhongReplace19 = true;
                    Rule.IsMustAllColor = true;
                    Rule.IsMustHasSameTriple = true;
                    Rule.HongZhongIsSameTriple = true;
                    Rule.isTing = true;

                    cfg_settingEntity bimenhu = GetConfigByName("无闭门胡");
                    if (bimenhu != null)
                    {
                        Rule.IsNotUsedCantHu = true;
                    }
                    break;
                case 42://承德
                    Rule.isXuanFengGang = false;
                    Rule.isTing = true;
                    Rule.is13YaoCanHu = true;
                    break;
            }
        }
        #endregion

        #region ClearRoom 清空房间数据
        /// <summary>
        /// 清空房间数据
        /// </summary>
        public void ClearRoom()
        {
            CurrentRoom = null;
            PlayerSeat = null;
            CurrentState = MahjongGameState.None;
            AskPokerGroup = null;
            Rule = null;
            m_DicTing.Clear();
            AllCanLiangXi.Clear();
        }
        #endregion

        #region GetConfig 获取规则配置
        /// <summary>
        /// 根据标签获取规则配置
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public cfg_settingEntity GetConfigByTag(string group)
        {
            for (int i = 0; i < CurrentRoom.Config.Count; ++i)
            {
                if (group == CurrentRoom.Config[i].tags)
                {
                    return CurrentRoom.Config[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 根据文本名获取规则配置
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public cfg_settingEntity GetConfigByName(string name)
        {
            for (int i = 0; i < CurrentRoom.Config.Count; ++i)
            {
                if (name == CurrentRoom.Config[i].name)
                {
                    return CurrentRoom.Config[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 根据组名获取规则配置
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public cfg_settingEntity GetConfigByLabel(string label)
        {
            for (int i = 0; i < CurrentRoom.Config.Count; ++i)
            {
                if (label == CurrentRoom.Config[i].label)
                {
                    return CurrentRoom.Config[i];
                }
            }
            return null;
        }
        #endregion

        #region CalculateSeatIndex 计算座位的客户端序号
        /// <summary>
        /// 计算座位的客户端序号
        /// </summary>
        /// <param name="room">房间信息</param>
        private void CalculateSeatIndex()
        {
            PlayerSeat = null;
            if (CurrentRoom == null) return;
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                if (CurrentRoom.SeatList[i].PlayerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
                {
                    PlayerSeat = CurrentRoom.SeatList[i];
                    break;
                }
            }
            if (PlayerSeat == null)
            {
                PlayerSeat = CurrentRoom.SeatList[0];
            }

            for (int j = 0; j < CurrentRoom.SeatList.Count; ++j)
            {
                SeatEntity seat = CurrentRoom.SeatList[j];
                int seatIndex = seat.Pos - PlayerSeat.Pos;
                seatIndex = seatIndex < 0 ? seatIndex + ROOM_SEAT_COUNT : seatIndex;
                seat.Index = seatIndex;
                if (CurrentRoom.SeatList.Count == 2)
                {
                    if (seat.Index != 0)
                    {
                        seat.Index = 2;
                    }
                }
            }
            CurrentRoom.PlayerSeat = PlayerSeat;
        }
        #endregion

        #region EnterRoom 进入房间
        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="pbSeat"></param>
        public void EnterRoom(OP_ROOM_ENTER proto)
        {
            if (CurrentRoom == null) return;
            if (CurrentRoom.SeatList.Count == 2 && proto.pos == 2)
            {
                proto.pos = 3;
            }
            Debug.Log("座位" + proto.pos + "进入房间");
            SeatEntity seat = GetSeatBySeatId(proto.pos);
            if (seat == null) return;
            seat.PlayerId = proto.playerId;
            seat.Gold = proto.gold;
            seat.Avatar = proto.avatar;
            seat.Gender = proto.gender;
            seat.Nickname = proto.nickname;
            seat.IP = proto.ipaddr;
            seat.Pos = proto.pos;
            seat.Latitude = proto.latitude;
            seat.Longitude = proto.longitude;
            seat.IsFocus = true;
            SendSeatInfoChangeNotify(seat);
            AppDebug.Log(seat.Nickname + "进入房间");
        }
        #endregion

        #region ExitRoom 离开房间
        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="pbSeat"></param>
        public void ExitRoom(OP_ROOM_LEAVE proto)
        {
            SeatEntity seat = GetSeatByPlayerId(proto.playerId);
            if (seat == null) return;
            seat.PlayerId = 0;
            seat.Nickname = string.Empty;
            seat.Status = SeatEntity.SeatStatus.Idle;
            seat.Latitude = 0f;
            seat.Longitude = 0f;
            seat.Gold = 0;
            seat.Avatar = string.Empty;

            SendSeatInfoChangeNotify(seat);

            AppDebug.Log(seat.Nickname + "离开房间");
        }
        #endregion

        #region Ready 准备
        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="pbSeat"></param>
        public void Ready(int playerId)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            Debug.Log(seat.Nickname + "准备");
            seat.Status = SeatEntity.SeatStatus.Ready;
#if !DEBUG_MODE
        seat.IsTrustee = false;
#endif
            if (seat == PlayerSeat)
            {
                SendRoomInfoChangeNotify();
            }
            else
            {
                SendSeatInfoChangeNotify(seat);
            }
        }
        #endregion

        #region CancelReady 取消准备
        /// <summary>
        /// 取消准备
        /// </summary>
        /// <param name="playerId"></param>
        public void CancelReady(int playerId)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            Debug.Log(seat.Nickname + "取消准备");
            seat.Status = SeatEntity.SeatStatus.Idle;
#if !DEBUG_MODE
        seat.IsTrustee = false;
#endif
            if (seat == PlayerSeat)
            {
                SendRoomInfoChangeNotify();
            }
            else
            {
                SendSeatInfoChangeNotify(seat);
            }
        }
        #endregion

        #region Trustee 托管
        /// <summary>
        /// 托管
        /// </summary>
        public void Trustee(int playerId, bool isTrustee)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            seat.IsTrustee = isTrustee;
            SendSeatInfoChangeNotify(seat);
            AppDebug.Log(seat.Nickname + "托管");
        }
        #endregion

        #region Begin 开局
        /// <summary>
        /// 开局
        /// </summary>
        /// <param name="pbRoom"></param>
        public void Begin(OP_ROOM_BEGIN proto)
        {
            m_DicTing.Clear();


            CurrentRoom.PokerTotal = proto.pokerTotal;
            CurrentRoom.currentLoop = proto.loop;
            CurrentRoom.PokerAmount = proto.pokerAmount;
            CurrentRoom.BaseScore = proto.baseScore;
            Debug.Log(CurrentRoom.BaseScore);
            CurrentRoom.ObsoleteDice.Clear();
            if (proto.luckPoker != null)
            {
                CurrentRoom.LuckPoker = new Poker(proto.luckPoker.index, proto.luckPoker.color, proto.luckPoker.size, proto.luckPoker.pos);
            }
            else
            {
                CurrentRoom.LuckPoker = null;
            }

            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                SeatEntity entity = CurrentRoom.SeatList[i];
                OP_SEAT_FULL protoSeat = null;
                for (int j = 0; j < proto.seatCount(); ++j)
                {
                    if (CurrentRoom.SeatList.Count == 2 && proto.getSeat(j).pos == 2)
                    {
                        proto.getSeat(j).pos = 3;
                    }
                    if (entity.Pos == proto.getSeat(j).pos)
                    {
                        protoSeat = proto.getSeat(j);
                        break;
                    }
                }
                ResetSeat(entity);
#if !DEBUG_MODE
            entity.IsTrustee = protoSeat.isTrustee;
#endif
                entity.isDouble = protoSeat.isDouble;
                for (int j = 0; j < protoSeat.pokerCount(); ++j)
                {
                    OP_POKER protoPoker = protoSeat.getPoker(j);
                    entity.PokerList.Add(new Poker(protoPoker.index, protoPoker.color, protoPoker.size, protoPoker.pos));
                }
                for (int j = 0; j < protoSeat.universalCount(); ++j)
                {
                    OP_POKER op_poker = protoSeat.getUniversal(j);
                    entity.UniversalList.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
                }
                entity.PokerAmount = protoSeat.pokerAmount;
                entity.IsBanker = protoSeat.isBanker;
                entity.isLockTing = false;
                entity.Direction = protoSeat.wind;
                if (CurrentRoom.SeatList.Count == 2 && entity.Direction == 2)
                {
                    entity.Direction = 3;
                }
                Debug.Log("方向：" + entity.Direction.ToString());
                entity.HoldPoker.Clear();
                entity.DingJiangPoker.Clear();
                entity.LackColor = 0;
                entity.SwapPoker.Clear();
                entity.MingTiPoker.Clear();
            }

            CurrentRoom.FirstDice = new DiceEntity()
            {
                diceA = proto.diceFirstA,
                diceB = proto.diceFirstB,
                seatPos = proto.diceFirst,
            };
            CurrentRoom.SecondDice = new DiceEntity()
            {
                diceA = proto.diceSecondA,
                diceB = proto.diceSecondB,
                seatPos = proto.diceSecond,
            };

            CurrentRoom.Status = (RoomEntity.RoomStatus)proto.status;

            SendRoomInfoChangeNotify();

            TransferData data = new TransferData();
            data.SetValue("SeatPos", PlayerSeat.Pos);
            data.SetValue("SeatCount", CurrentRoom.SeatList.Count);
            data.SetValue("SeatDirection", PlayerSeat.Direction);
            SendNotification(ON_BEGIN, data);
        }
        #endregion

        #region SetLuckPoker 设置翻的牌
        /// <summary>
        /// 设置翻的牌
        /// </summary>
        /// <param name="pbPoker"></param>
        public void SetLuckPoker(int index, int color, int size)
        {
            Poker poker = new Poker(index, color, size, 0);

            Debug.Log("翻的牌是" + poker.ToString());
            CurrentRoom.LuckPoker = poker;
            --CurrentRoom.PokerAmount;
#if IS_LEPING
        for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
        {
            SeatEntity entity = CurrentRoom.SeatList[i];
            entity.UniversalList.Clear();
            Poker universal = MahJongHelper.GetNextPoker(CurrentRoom.LuckPoker);
            entity.UniversalList.Add(universal);
        }
#elif IS_GUGENG
                int universalCount = 1;
        cfg_settingEntity entity = GetConfigByTag("universalCount");
        if (entity != null)
        {
            universalCount = entity.value;
        }
        if (universalCount == 1)
        {
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                CurrentRoom.SeatList[i].UniversalList.Clear();
                if (poker.color < 4)
                {
                    CurrentRoom.SeatList[i].UniversalList.Add(new Poker(0, poker.color, 10 - poker.size));
                }
                else if (poker.color == 4)
                {
                    CurrentRoom.SeatList[i].UniversalList.Add(new Poker(0, poker.color, (poker.size < 3 ? poker.size + 2 : poker.size - 2)));
                }
                else if (poker.color == 5)
                {
                    CurrentRoom.SeatList[i].UniversalList.Add(new Poker(0, poker.color, 4 - poker.size));
                }
            }
        }
        else
        {
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                CurrentRoom.SeatList[i].UniversalList.Clear();
                if (poker.color < 4)
                {
                    CurrentRoom.SeatList[i].UniversalList.Add(new Poker(0, poker.color, Mathf.Clamp(poker.size - 1, 1, 9)));
                    CurrentRoom.SeatList[i].UniversalList.Add(new Poker(0, poker.color, Mathf.Clamp(poker.size + 1, 1, 9)));
                }
                else if (poker.color == 4)
                {
                    CurrentRoom.SeatList[i].UniversalList.Add(new Poker(0, poker.color, poker.size % 2 == 0 ? 1 : 2));
                    CurrentRoom.SeatList[i].UniversalList.Add(new Poker(0, poker.color, poker.size % 2 == 0 ? 3 : 4));
                }
                else if (poker.color == 5)
                {
                    for (int j = 1; j < 4; ++j)
                    {
                        if (j == poker.size) continue;
                        CurrentRoom.SeatList[i].UniversalList.Add(new Poker(0, poker.color, j));
                    }
                }
            }
        }
#elif IS_WANGQUE
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                CurrentRoom.SeatList[i].UniversalList.Clear();
                if (poker.color < 6)
                {
                    CurrentRoom.SeatList[i].UniversalList.Add(poker);
                }
                else
                {
                    for (int j = 1; j < 5; ++j)
                    {
                        CurrentRoom.SeatList[i].UniversalList.Add(new Poker(poker.color, j));
                    }
                }
            }
#endif

            SendRoomInfoChangeNotify(false);

        }
        #endregion

        #region DrawPoker 摸牌
        /// <summary>
        /// 摸牌
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="poker"></param>
        /// <param name="isLast"></param>
        public void DrawPoker(int playerId, Poker poker, bool isLast, bool isBuhua = false)
        {
            CurrentState = MahjongGameState.DrawPoker;
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            if (poker.color == 0 && seat == PlayerSeat)
            {
                AppDebug.ThrowError("服务器广播了一个空牌");
            }
            Debug.Log(seat.Nickname + "摸了张" + poker.ToString("{0}_{1}_{2}" + (isBuhua ? "，是补花" : "")));
            poker.pos = seat.Pos;

            if (seat.HitPoker != null || isBuhua)
            {
                seat.PokerList.Add(poker);
            }
            else
            {
                seat.HitPoker = poker;
            }

            if (!isBuhua)
            {
                SetCurrentOperator(seat.Pos);
            }

            --CurrentRoom.PokerAmount;

            TransferData data = new TransferData();
            data.SetValue("SeatPos", seat.Pos);
            data.SetValue("SeatDirection", seat.Direction);
            data.SetValue("SeatCount", CurrentRoom.SeatList.Count);
            data.SetValue("HitPoker", poker);
            data.SetValue("IsLast", isLast);
            data.SetValue("IsBuhua", isBuhua);
            data.SetValue("IsPlayer", seat == PlayerSeat);
            data.SetValue("RoomStatus", CurrentRoom.Status);
            SendNotification(ON_DRAW_POKER, data);

            SendRoomInfoChangeNotify(false);
        }
        #endregion

        #region PlayPoker 出牌
        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="poker"></param>
        public void PlayPoker(int playerId, Poker poker, bool isTing)
        {
            CurrentState = MahjongGameState.PlayPoker;
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null)
            {
                AppDebug.Log("出牌座位是空的");
                return;
            }
            poker.pos = seat.Pos;
            Debug.Log(seat.Nickname + "出牌" + poker.ToString("{0}_{1}_{2}"));
            if (isTing)
            {
                seat.IsTing = true;

                SendSeatInfoChangeNotify(seat);
                TransferData tingData = new TransferData();
                tingData.SetValue("Seat", seat);
                Debug.Log("座位" + seat.Pos + "听牌");
                SendNotification(ON_TING, tingData);
            }

            bool isExists = false;
            if (seat.HitPoker != null)
            {
                if (seat.HitPoker.index == poker.index)
                {
                    seat.HitPoker = null;
                    isExists = true;
                }
                else
                {
                    for (int i = 0; i < seat.PokerList.Count; ++i)
                    {
                        if (seat.PokerList[i].index == poker.index)
                        {
                            seat.PokerList.Remove(seat.PokerList[i]);
                            isExists = true;
                            break;
                        }
                    }
                    seat.PokerList.Add(seat.HitPoker);
                    seat.HitPoker = null;
                }
            }
            else
            {
                for (int i = 0; i < seat.PokerList.Count; ++i)
                {
                    if (seat.PokerList[i].index == poker.index)
                    {
                        seat.PokerList.Remove(seat.PokerList[i]);
                        isExists = true;
                        break;
                    }
                }
            }
            if (!isExists)
            {
                Debug.LogWarning("卧槽不存在？？？？？？？？？");
            }
            seat.DeskTopPoker.Add(poker);
            if (MahJongHelper.HasPoker(poker, seat.UniversalList))
            {
                seat.isPlayedUniversal = true;
            }

            TransferData data = new TransferData();
            data.SetValue("SeatPos", seat.Pos);
            data.SetValue("HitPoker", poker);
            data.SetValue("Gender", seat.Gender);
            data.SetValue("IsTing", isTing && !seat.isLockTing);
#if IS_LEPING
        Poker bao = MahJongHelper.GetNextPoker(CurrentRoom.LuckPoker);
        data.SetValue("IsBao", bao == null ? false : (poker.color == bao.color && poker.size == bao.size));
#elif IS_GUGENG
        bool isBao = false;
        for (int i = 0; i < seat.UniversalList.Count; ++i)
        {
            if (poker.color == seat.UniversalList[i].color && poker.size == seat.UniversalList[i].size)
            {
                isBao = true;
                break;
            }
        }
        data.SetValue("IsBao", isBao);
#else
            Poker bao = CurrentRoom.LuckPoker;
            data.SetValue("IsBao", bao == null ? false : (poker.color == bao.color && poker.size == bao.size));
#endif

            SendNotification(ON_PLAY_POKER, data);

            if (isTing)
            {
                seat.isLockTing = false;
            }
        }
        #endregion

        #region OperatePoker 碰牌
        /// <summary>
        /// 碰牌
        /// </summary>
        /// <param name="seatPos"></param>
        /// <param name="combination"></param>
        public void OperatePoker(OperatorType type, int playerId, int subType, List<Poker> lst)
        {
            CurrentState = MahjongGameState.Operate;
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            SetCurrentOperator(seat.Pos);

            Debug.Log(seat.Nickname + type);
            Debug.Log(lst == null ? 0 : lst.Count);

            PokerCombinationEntity conbination = null;
            if (lst != null && lst.Count > 0)
            {
                if (type != OperatorType.Gang)
                {
                    MahJongHelper.SimpleSort(lst);
                }

                conbination = new PokerCombinationEntity(type == OperatorType.PiaoTing ? OperatorType.Peng : type, subType, lst);

                for (int i = 0; i < lst.Count; ++i)
                {
                    if (CurrentRoom.SeatList.Count == 2 && lst[i].pos == 2)
                    {
                        lst[i].pos = 3;
                    }
                }
#if IS_DAZHONG || IS_WUANJUN
            if (type == OperatorType.Chi)//吃的牌放中间
            {
                for (int k = 0; k < lst.Count; ++k)
                {
                    if (lst[k].pos != seat.Pos)
                    {
                        Poker poker = lst[k];
                        lst.Remove(poker);
                        lst.Insert(1, poker);
                    }
                }
            }

            if (type == OperatorType.Peng || type == OperatorType.Gang)//碰的牌上左对中下右
            {
                for (int k = 0; k < lst.Count; ++k)
                {
                    if (lst[k].pos != seat.Pos)
                    {
                        Poker poker = lst[k];
                        lst.Remove(poker);
                        if (Mathf.Abs(poker.pos - seat.Pos) == 2)
                        {
                            lst.Insert(1, poker);
                            break;
                        }
                        if (poker.pos - seat.Pos == 1 || poker.pos - seat.Pos == -3)
                        {
                            lst.Insert(2, poker);
                            break;
                        }
                        if (poker.pos - seat.Pos == -1 || poker.pos - seat.Pos == 3)
                        {
                            lst.Insert(0, poker);
                            break;
                        }
                    }
                }
            }
#endif
            }
            if (type == OperatorType.Hu)
            {
                if (lst != null && lst.Count > 0 && seat.HitPoker == null)
                {
                    Debug.Log(lst[0]);
                    seat.HitPoker = lst[0];
                    for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
                    {
                        bool isBreak = false;
                        for (int j = 0; j < CurrentRoom.SeatList[i].DeskTopPoker.Count; ++j)
                        {
                            if (CurrentRoom.SeatList[i].DeskTopPoker[j].index == seat.HitPoker.index)
                            {
                                CurrentRoom.SeatList[i].DeskTopPoker.RemoveAt(j);
                                isBreak = true;
                                break;
                            }
                        }
                        if (isBreak) break;
                    }
                }
                seat.isHu = true;
            }
            else if (type == OperatorType.Jiao)
            {
                seat.IsTing = true;
            }
            else if (type == OperatorType.DingJiang)
            {
                seat.DingJiangPoker.AddRange(lst);
                for (int i = 0; i < lst.Count; ++i)
                {
                    if (!MahJongHelper.ContainPoker(lst[i], seat.PokerList))
                    {
                        for (int j = 0; j < CurrentRoom.SeatList.Count; ++j)
                        {
                            if (MahJongHelper.ContainPoker(lst[i], CurrentRoom.SeatList[j].DeskTopPoker))
                            {
                                CurrentRoom.SeatList[j].DeskTopPoker.Remove(lst[i]);
                            }
                        }
                        seat.PokerList.Add(lst[i]);
                    }
                }
            }
            else
            {
                if (type == OperatorType.ChiTing || type == OperatorType.PengTing || type == OperatorType.DingZhang || type == OperatorType.PiaoTing)
                {
                    seat.isLockTing = true;
                }

                //如果是补杠，先移除之前的碰
                if (conbination != null && type == OperatorType.Gang && subType == 5)
                {
                    for (int i = 0; i < seat.UsedPokerList.Count; ++i)
                    {
                        if (seat.UsedPokerList[i].PokerList[0].color == conbination.PokerList[0].color && seat.UsedPokerList[i].PokerList[0].size == conbination.PokerList[0].size)
                        {
                            seat.UsedPokerList.RemoveAt(i);
                            break;
                        }
                    }
                }
#if IS_LUALU
            if (conbination != null && type == OperatorType.Gang)
            {
                for (int i = 0; i < seat.UsedPokerList.Count; ++i)
                {
                    for (int j = 0; j < lst.Count; ++j)
                    {
                        if (lst[j].index == seat.UsedPokerList[i].PokerList[0].index)
                        {
                            seat.UsedPokerList.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
#endif

                if (conbination != null)
                {
                    //把碰的牌加进已使用的牌
                    seat.UsedPokerList.Add(conbination);
                }

                if (conbination != null)
                {
                    //把碰的牌移出手牌
                    for (int i = 0; i < conbination.PokerList.Count; ++i)
                    {
                        for (int j = 0; j < seat.PokerList.Count; ++j)
                        {
                            if (conbination.PokerList[i].index == seat.PokerList[j].index)
                            {
                                seat.PokerList.RemoveAt(j);
                                break;
                            }
                        }
                        if (seat.HitPoker != null)
                        {
                            if (conbination.PokerList[i].index == seat.HitPoker.index)
                            {
                                seat.HitPoker = null;
                            }
                        }
                        for (int j = 0; j < seat.DeskTopPoker.Count; ++j)
                        {
                            if (conbination.PokerList[i].index == seat.DeskTopPoker[j].index)
                            {
                                seat.DeskTopPoker.RemoveAt(j);
                                break;
                            }
                        }
                    }
                }

                if (seat.HitPoker != null)
                {
                    seat.PokerList.Add(seat.HitPoker);
                    seat.HitPoker = null;
                }
            }

            TransferData data = new TransferData();
            data.SetValue("SeatCount", CurrentRoom.SeatList.Count);
            data.SetValue("SeatPos", seat.Pos);
            data.SetValue("SeatDirection", seat.Direction);
            data.SetValue("SeatIndex", seat.Index);
            data.SetValue("OperateType", type);
            data.SetValue("SubType", subType);
            data.SetValue("Combination", conbination);
            data.SetValue("Gender", seat.Gender);
            data.SetValue("IsPlayer", seat == PlayerSeat);
            data.SetValue("RoomStatus", CurrentRoom.Status);

            SendNotification(ON_OPERATE, data);

            SendSeatInfoChangeNotify(seat);
        }
        #endregion

        #region Hu 胡牌
        /// <summary>
        /// 胡牌
        /// </summary>
        /// <param name="playerId">玩家Id</param>
        /// <param name="subType">胡牌子类型</param>
        /// <param name="lst">胡的牌</param>
        public void Hu(int playerId, int subType, List<Poker> lst)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            SetCurrentOperator(seat.Pos);
            if (lst != null && lst.Count > 0)
            {
                seat.HitPoker = lst[0];

                for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
                {
                    bool isBreak = false;
                    for (int j = 0; j < CurrentRoom.SeatList[i].DeskTopPoker.Count; ++j)
                    {
                        if (CurrentRoom.SeatList[i].DeskTopPoker[j].index == seat.HitPoker.index)
                        {
                            CurrentRoom.SeatList[i].DeskTopPoker.RemoveAt(j);
                            isBreak = true;
                            break;
                        }
                    }
                    if (isBreak) break;

                    for (int j = 0; j < CurrentRoom.SeatList[i].UsedPokerList.Count; ++j)
                    {
                        for (int k = 0; k < CurrentRoom.SeatList[i].UsedPokerList[j].PokerList.Count; ++k)
                        {
                            if (CurrentRoom.SeatList[i].UsedPokerList[j].PokerList[k].index == seat.HitPoker.index)
                            {
                                CurrentRoom.SeatList[i].UsedPokerList[j].PokerList.RemoveAt(k);
                                if (CurrentRoom.SeatList[i].UsedPokerList[j].CombinationType == OperatorType.Gang)
                                {
                                    CurrentRoom.SeatList[i].UsedPokerList[j].CombinationType = OperatorType.Peng;
                                    isBreak = true;
                                    break;
                                }
                            }
                        }
                        if (isBreak) break;
                    }
                    if (isBreak) break;
                }
            }
            else
            {
                Debug.LogWarning("这个游戏胡牌的时候没告诉客户端胡的哪张！！！！让那个弱智服务器改！！！！");
            }
            seat.isHu = true;

            TransferData data = new TransferData();
            data.SetValue("SeatEntity", seat);
            data.SetValue("SubType", subType);
            data.SetValue("RoomStatus", CurrentRoom.Status);
            data.SetValue("IsPlayer", seat == PlayerSeat);
            SendNotification(ON_HU, data);
        }
        #endregion

        #region MingTi 明提
        /// <summary>
        /// 明提
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="pokers"></param>
        public void MingTi(int playerId, List<Poker> pokers)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            seat.MingTiPoker.AddRange(pokers);

            TransferData data = new TransferData();
            data.SetValue("SeatEntity", seat);
            SendNotification(ON_MING_TI, data);
        }
        #endregion

        #region Settle 结算
        /// <summary>
        /// 结算
        /// </summary>
        /// <param name="pbRoom"></param>
        public void Settle(OP_ROOM_SETTLE proto)
        {
            CurrentRoom.Prob.Clear();
            CurrentRoom.LuckPoker = proto.luckPoker == null ? null : new Poker(proto.luckPoker.index, proto.luckPoker.color, proto.luckPoker.size, proto.luckPoker.pos);
            for (int i = 0; i < proto.probPokerCount(); ++i)
            {
                OP_POKER op_poker = proto.getProbPoker(i);
                CurrentRoom.Prob.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
            }
            for (int i = 0; i < proto.seatCount(); ++i)
            {
                OP_SEAT_FULL op_seat = proto.getSeat(i);
                if (CurrentRoom.SeatList.Count == 2 && op_seat.pos == 2)
                {
                    op_seat.pos = 3;
                }
                SeatEntity seat = GetSeatBySeatId(proto.getSeat(i).pos);
                seat.Settle = op_seat.settle;
                seat.Gold = op_seat.gold;
                seat.isLoser = op_seat.isLoser;
                seat.isWiner = op_seat.isWiner;
                seat.PokerList.Clear();
                seat.ProbMulti = op_seat.probMulti;
                seat.IsTing = op_seat.isListen;
                seat.UsedPokerList.Clear();
                for (int j = 0; j < op_seat.usePokerGroupCount(); ++j)
                {
                    OP_POKER_GROUP op_combination = op_seat.getUsePokerGroup(j);
                    List<Poker> lst = new List<Poker>();
                    for (int k = 0; k < op_combination.pokerCount(); ++k)
                    {
                        OP_POKER op_poker = op_combination.getPoker(k);
                        lst.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
                    }
                    PokerCombinationEntity combination = new PokerCombinationEntity((OperatorType)op_combination.typeId, (int)op_combination.subTypeId, lst);
                    seat.UsedPokerList.Add(combination);
                }
                for (int j = 0; j < op_seat.pokerCount(); ++j)
                {
                    OP_POKER op_Poker = op_seat.getPoker(j);
                    seat.PokerList.Add(new Poker(op_Poker.index, op_Poker.color, op_Poker.size, op_Poker.pos));
                }
                if (op_seat.hitPoker != null)
                {
                    Debug.Log("hitpoker=" + op_seat.hitPoker.color + "_" + op_seat.hitPoker.size);
                    seat.HitPoker = new Poker(op_seat.hitPoker.index, op_seat.hitPoker.color, op_seat.hitPoker.size, op_seat.hitPoker.pos);
                }
                else
                {
                    seat.HitPoker = null;
                }
                seat.SettleInfo.Clear();
                for (int j = 0; j < op_seat.incomesCount(); ++j)
                {
                    OP_POKER_SETTLE income = op_seat.getIncomes(j);
                    Poker tempPoker = null;
                    if (income.poker != null)
                    {
                        tempPoker = new Poker(income.poker.index, income.poker.color, income.poker.size, income.poker.pos);
                    }

                    IncomeDetailEntity detail = new IncomeDetailEntity(income.cfgId, income.times, tempPoker);
                    seat.SettleInfo.Add(detail);
                }
                seat.HuScoreDetail.Clear();
                for (int j = 0; j < op_seat.scoresCount(); ++j)
                {
                    OP_POKER_SETTLE income = op_seat.getScores(j);
                    Poker tempPoker = null;
                    if (income.poker != null)
                    {
                        tempPoker = new Poker(income.poker.index, income.poker.color, income.poker.size, income.poker.pos);
                    }
                    IncomeDetailEntity detail = new IncomeDetailEntity(income.cfgId, income.times, tempPoker);
                    seat.HuScoreDetail.Add(detail);
                }
                seat.TotalHuScore = op_seat.huScore;
            }
            CurrentRoom.Status = RoomEntity.RoomStatus.Settle;
            CurrentRoom.IsOver = proto.isResult;
        }
        #endregion

        #region Waiver 弃权
        /// <summary>
        /// 弃权
        /// </summary>
        /// <param name="playerId"></param>
        public void Waiver(int playerId)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat != null)
            {
                seat.IsWaiver = true;
                seat.IsTrustee = false;
                SendSeatInfoChangeNotify(seat);
            }
        }
        #endregion

        #region SetCurrentOperator 设置当前操作者
        /// <summary>
        /// 设置当前操作者
        /// </summary>
        /// <param name="seatPos"></param>
        public void SetCurrentOperator(int seatPos, bool isInit = false)
        {
            if (!isInit && (CurrentRoom.Status == RoomEntity.RoomStatus.Show || CurrentRoom.Status == RoomEntity.RoomStatus.Jiao)) return;
            SeatEntity seat = GetSeatBySeatId(seatPos);
            if (seat != null)
            {
                CurrentRoom.CurrentOperator = seat;
                TransferData data = new TransferData();
                data.SetValue("SeatPos", CurrentRoom.CurrentOperator.Pos);
                data.SetValue("SratCount", CurrentRoom.SeatList.Count);
                data.SetValue("SeatDirection", CurrentRoom.CurrentOperator.Direction);
                SendNotification(ON_CURRENT_OPERATOR_CHANGED, data);
            }
        }
        #endregion

        #region ZhiDui 支对
        /// <summary>
        /// 支对
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="pokers"></param>
        public void ZhiDui(int playerId, List<Poker> pokers)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            Debug.Log(seat.Nickname + "支对");
            seat.HoldPoker.Add(pokers);

            TransferData data = new TransferData();
            data.SetValue("Seat", seat);
            data.SetValue("IsPlayer", seat == PlayerSeat);
            data.SetValue("RoomStatus", CurrentRoom.Status);
            SendNotification(ON_ZHIDUI, data);
        }
        #endregion

        #region RollDice 摇骰子(翻宝的时候)
        /// <summary>
        /// 摇骰子(翻宝的时候)
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="dice"></param>
        public void RollDice(int playerId, int dice)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            Debug.Log(seat.Pos + "摇骰子");
            CurrentRoom.ObsoleteDice.Enqueue(new DiceEntity()
            {
                diceA = dice,
                seatPos = seat.Pos
            });
        }
        #endregion

        #region SetOnLine 设置在线状态
        /// <summary>
        /// 设置在线状态
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="isOnline"></param>
        public void SetOnLine(int playerId, bool isOnline)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            Debug.Log(seat.Nickname + (isOnline ? "上线" : "离线"));
            //seat.IsOffLine = !isOnline;
            SendSeatInfoChangeNotify(seat);
        }
        #endregion

        #region SetFocus 设置焦点状态
        /// <summary>
        /// 设置焦点状态
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="isFocus"></param>
        public void SetFocus(int playerId, bool isFocus)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            Debug.Log(seat.Nickname + (isFocus ? "切回来" : "切出去"));
            seat.IsFocus = isFocus;
            SendSeatInfoChangeNotify(seat);
        }
        #endregion

        #region SetStatus 设置房间状态
        /// <summary>
        /// 设置房间状态
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(RoomEntity.RoomStatus status)
        {
            Debug.Log("房间状态变更为" + status.ToString());
            CurrentRoom.Status = status;
            SendRoomInfoChangeNotify();
        }
        #endregion

        #region SetGold 设置金币
        /// <summary>
        /// 设置金币
        /// </summary>
        /// <param name="playerId">玩家Id</param>
        /// <param name="gold">金币数量</param>
        public void SetGold(int playerId, int gold)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            seat.Gold += gold;
            TransferData data = new TransferData();
            data.SetValue("SeatIndex", seat.Index);
            data.SetValue("ChangeGold", gold);
            data.SetValue("Gold", seat.Gold);
            SendNotification(ON_SEAT_GOLD_CHANGED, data);
        }
        #endregion

        #region SetDouble 设置双倍状态
        /// <summary>
        /// 设置双倍状态
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="isDouble"></param>
        public void SetDouble(int playerId, bool isDouble)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;

            seat.isDouble = isDouble;
            SendSeatInfoChangeNotify(seat);
        }
        #endregion

        #region SetPos 设置座位号
        /// <summary>
        /// 设置座位号
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="pos"></param>
        /// <returns>是否成功</returns>
        public bool SetPos(int playerId, int pos)
        {
            if (CurrentRoom.SeatList.Count == 2 && pos == 2)
            {
                pos = 3;
            }
            SeatEntity emptySeat = GetSeatBySeatId(pos);
            if (emptySeat == null) return false;
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return false;

            emptySeat.Pos = seat.Pos;

            SendNotification(ON_SEAT_INFO_CLEAR, null);
            Debug.Log(seat.Nickname + "换到了座位" + pos.ToString());
            seat.Pos = pos;
            CalculateSeatIndex();
            SendRoomInfoChangeNotify();
            return true;
        }
        #endregion

        #region SetSeatDisbandState 设置座位解散状态
        /// <summary>
        /// 设置座位解散状态
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="isAgree"></param>
        public void SetSeatDisbandState(int playerId, DisbandState disbandState, long disbandTime)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            seat.DisbandState = disbandState;
            if (disbandTime != 0)
            {
                CurrentRoom.DisbandTime = disbandTime;
            }
        }
        #endregion

        #region SetSwapPoker 设置要交换的牌
        /// <summary>
        /// 设置要交换的牌
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="pokers"></param>
        public void SetSwapPoker(int playerId, List<Poker> pokers)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;

            for (int i = seat.PokerList.Count - 1; i >= 0; --i)
            {
                if (MahJongHelper.ContainPoker(seat.PokerList[i], pokers))
                {
                    seat.SwapPoker.Add(seat.PokerList[i]);
                    seat.PokerList.RemoveAt(i);
                }
            }

            TransferData data = new TransferData();
            data.SetValue("SeatEntity", seat);
            SendNotification(ON_SWAP_POKER, data);
        }
        #endregion

        #region SwapPoker 交换牌
        /// <summary>
        /// 交换牌
        /// </summary>
        /// <param name="swapType"></param>
        /// <param name="pokers"></param>
        public void SwapPoker(int swapType, List<Poker> pokers)
        {
            if (CurrentRoom.SeatList.Count == 2) swapType = 2;
            switch (swapType)
            {
                case 0:
                    CurrentRoom.SeatList.Sort((a, b) =>
                    {
                        return b.Pos - a.Pos;
                    });
                    {
                        List<Poker> temp = CurrentRoom.SeatList[CurrentRoom.SeatList.Count - 1].SwapPoker;
                        for (int i = CurrentRoom.SeatList.Count - 1; i >= 1; --i)
                        {
                            Debug.Log("座位" + CurrentRoom.SeatList[i].Pos + "拿了座位" + CurrentRoom.SeatList[i - 1].Pos + "的牌");
                            CurrentRoom.SeatList[i].SwapPoker = CurrentRoom.SeatList[i - 1].SwapPoker;
                        }
                        CurrentRoom.SeatList[0].SwapPoker = temp;
                    }
                    break;
                case 1:
                    CurrentRoom.SeatList.Sort((a, b) =>
                    {
                        return a.Pos - b.Pos;
                    });
                    {
                        List<Poker> temp = CurrentRoom.SeatList[CurrentRoom.SeatList.Count - 1].SwapPoker;
                        for (int i = CurrentRoom.SeatList.Count - 1; i >= 1; --i)
                        {
                            Debug.Log("座位" + CurrentRoom.SeatList[i].Pos + "拿了座位" + CurrentRoom.SeatList[i - 1].Pos + "的牌");
                            CurrentRoom.SeatList[i].SwapPoker = CurrentRoom.SeatList[i - 1].SwapPoker;
                        }
                        CurrentRoom.SeatList[0].SwapPoker = temp;
                    }
                    break;
                case 2:
                    List<int> swapedSeatPos = new List<int>();
                    for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
                    {
                        int pos = CurrentRoom.SeatList[i].Pos;
                        if (swapedSeatPos.Contains(pos)) continue;
                        for (int j = 0; j < CurrentRoom.SeatList.Count; ++j)
                        {
                            if (Mathf.Abs(CurrentRoom.SeatList[j].Pos - pos) == 2)
                            {
                                List<Poker> temp = CurrentRoom.SeatList[j].SwapPoker;
                                CurrentRoom.SeatList[j].SwapPoker = CurrentRoom.SeatList[i].SwapPoker;
                                CurrentRoom.SeatList[i].SwapPoker = temp;
                                swapedSeatPos.Add(CurrentRoom.SeatList[i].Pos);
                                swapedSeatPos.Add(CurrentRoom.SeatList[j].Pos);
                            }
                        }
                    }
                    break;
            }

            if (CurrentRoom.isReplay)
            {
                for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
                {
                    List<Poker> swapPoker = new List<Poker>();
                    for (int j = 0; j < CurrentRoom.SeatList[i].SwapPoker.Count; ++j)
                    {
                        Poker poker = CurrentRoom.SeatList[i].SwapPoker[j];
                        if (CurrentRoom.isReplay)
                        {
                            swapPoker.Add(poker);
                        }
                        else
                        {
                            swapPoker.Add(new Poker(poker.index, 0, 0));
                        }
                    }
                    CurrentRoom.SeatList[i].SwapPoker.Clear();
                    CurrentRoom.SeatList[i].PokerList.AddRange(swapPoker);
                    CurrentRoom.SeatList[i].SwapPoker.AddRange(swapPoker);
                }
            }
            else
            {
                for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
                {
                    if (CurrentRoom.SeatList[i] == PlayerSeat) continue;
                    List<Poker> swapPoker = new List<Poker>();
                    for (int j = 0; j < CurrentRoom.SeatList[i].SwapPoker.Count; ++j)
                    {
                        Poker poker = CurrentRoom.SeatList[i].SwapPoker[j];
                        swapPoker.Add(new Poker(poker.index, 0, 0));
                    }
                    CurrentRoom.SeatList[i].SwapPoker.Clear();
                    CurrentRoom.SeatList[i].PokerList.AddRange(swapPoker);
                    CurrentRoom.SeatList[i].SwapPoker.AddRange(swapPoker);
                }

                PlayerSeat.SwapPoker.Clear();
                PlayerSeat.SwapPoker.AddRange(pokers);
                PlayerSeat.PokerList.AddRange(pokers);
            }
        }
        #endregion

        #region SetLackColor 定缺
        /// <summary>
        /// 定缺
        /// </summary>
        public void SetLackColor(int playerId, int color)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            seat.LackColor = color;

            Debug.Log(seat.Nickname + "定缺" + seat.LackColor);

            TransferData data = new TransferData();
            data.SetValue("SeatEntity", seat);
            SendNotification(ON_SET_LACK_COLOR, data);
        }
        #endregion

        #region Check 数据校验
        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="pokers"></param>
        public void Check(List<Poker> pokers)
        {
            if (PlayerSeat == null) return;
            List<Poker> hand = new List<Poker>(PlayerSeat.PokerList);

            for (int i = 0; i < pokers.Count; ++i)
            {
                if (pokers[i].color == 0 || pokers[i].size == 0)
                {
                    AppDebug.ThrowError("检测数据有误");
                    return;
                }
            }

            if (hand.Count != pokers.Count)
            {
                string str = GetPokerLog(PlayerSeat.PokerList);
                string str2 = GetPokerLog(pokers);
                string log = string.Format("数据长度有误,客户端长度:{0},服务器长度:{1}\r\n,客户端手牌:{2},\r\n服务器手牌:{3}", hand.Count, pokers.Count, str, str2);
                AppDebug.ThrowError(log);
                return;
            }


            string e = string.Empty;
            for (int i = 0; i < hand.Count; ++i)
            {
                bool isExists = false;
                for (int j = 0; j < pokers.Count; ++j)
                {
                    if (hand[i].index == pokers[j].index && hand[i].color == pokers[j].color && hand[i].size == pokers[j].size)
                    {
                        isExists = true;
                    }
                }
                if (!isExists)
                {
                    e += "客户端有一个" + hand[i].ToString("{0}_{1}_{2}") + "  ";
                }
            }

            for (int i = 0; i < pokers.Count; ++i)
            {
                bool isExists = false;
                for (int j = 0; j < hand.Count; ++j)
                {
                    if (pokers[i].index == hand[j].index && pokers[i].color == hand[j].color && pokers[i].size == hand[j].size)
                    {
                        isExists = true;
                    }
                }
                if (!isExists)
                {
                    e += "服务端有一个" + pokers[i].ToString("{0}_{1}_{2}") + "  ";
                }
            }

            if (!string.IsNullOrEmpty(e))
            {
                AppDebug.ThrowError("检查数据有误," + e);
            }
            Debug.Log("检测完毕");
        }
        #endregion

        #region GetChi 获取吃
        /// <summary>
        /// 获取吃
        /// </summary>
        /// <param name="poker"></param>
        /// <returns></returns>
        public List<List<Poker>> GetChi(Poker poker)
        {
            List<Poker> handList = new List<Poker>(PlayerSeat.PokerList);
            for (int i = 0; i < PlayerSeat.DingJiangPoker.Count; ++i)
            {
                for (int j = 0; j < handList.Count; ++j)
                {
                    if (PlayerSeat.DingJiangPoker[i].index == handList[j].index)
                    {
                        handList.Remove(handList[j]);
                        break;
                    }
                }
            }

            List<Poker> universal = PlayerSeat.UniversalList;

            List<List<Poker>> chi = new List<List<Poker>>();

            if (poker.color < 4 || (Rule.IsFengCanChi && poker.color == 5))
            {
                //检查第三张
                if (poker.size >= 3)
                {
                    List<Poker> lst = new List<Poker>();
                    Poker first = null;
                    Poker second = null;
                    for (int i = 0; i < handList.Count; ++i)
                    {
                        if (handList[i].color == poker.color && handList[i].size == poker.size - 2)
                        {
                            first = handList[i];
                        }
                        if (handList[i].color == poker.color && handList[i].size == poker.size - 1)
                        {
                            second = handList[i];
                        }
                        if (first != null && second != null)
                        {
                            if (!Rule.IsUniversalCanChi)
                            {
                                if (MahJongHelper.CheckUniversal(first, universal))
                                {
                                    break;
                                }
                                if (MahJongHelper.CheckUniversal(second, universal))
                                {
                                    break;
                                }
                            }
                            lst.Add(first);
                            lst.Add(second);
                            lst.Add(poker);
                            chi.Add(lst);
                            break;
                        }
                    }
                }
                //检查第二张
                if (poker.size > 1 && poker.size < 9)
                {
                    List<Poker> lst = new List<Poker>();
                    Poker first = null;
                    Poker third = null;
                    for (int i = 0; i < handList.Count; ++i)
                    {
                        if (handList[i].color == poker.color && handList[i].size == poker.size - 1)
                        {
                            first = handList[i];
                        }
                        if (handList[i].color == poker.color && handList[i].size == poker.size + 1)
                        {
                            third = handList[i];
                        }
                        if (first != null && third != null)
                        {
                            if (!Rule.IsUniversalCanChi)
                            {
                                if (MahJongHelper.CheckUniversal(first, universal))
                                {
                                    break;
                                }
                                if (MahJongHelper.CheckUniversal(third, universal))
                                {
                                    break;
                                }
                            }
                            lst.Add(first);
                            lst.Add(poker);
                            lst.Add(third);

                            chi.Add(lst);
                            break;
                        }
                    }
                }
                //检查第一张
                if (poker.size <= 7)
                {
                    List<Poker> lst = new List<Poker>();
                    Poker second = null;
                    Poker third = null;
                    for (int i = 0; i < handList.Count; ++i)
                    {
                        if (handList[i].color == poker.color && handList[i].size == poker.size + 1)
                        {
                            second = handList[i];
                        }
                        if (handList[i].color == poker.color && handList[i].size == poker.size + 2)
                        {
                            third = handList[i];
                        }
                        if (second != null && third != null)
                        {
                            if (!Rule.IsUniversalCanChi)
                            {
                                if (MahJongHelper.CheckUniversal(second, universal))
                                {
                                    break;
                                }
                                if (MahJongHelper.CheckUniversal(third, universal))
                                {
                                    break;
                                }
                            }

                            lst.Add(poker);
                            lst.Add(second);
                            lst.Add(third);

                            chi.Add(lst);
                            break;
                        }
                    }
                }
            }
            else if (Rule.IsFengCanChi && poker.color == 4)
            {
                Poker dong = poker.size == 1 ? poker : null;
                Poker nan = poker.size == 2 ? poker : null;
                Poker xi = poker.size == 3 ? poker : null;
                Poker bei = poker.size == 4 ? poker : null;
                int fengCount = 1;
                for (int i = 0; i < handList.Count; ++i)
                {
                    if (handList[i].color == 4 && handList[i].size == 1)
                    {
                        if (dong == null)
                        {
                            ++fengCount;
                            dong = handList[i];
                        }
                    }
                    if (handList[i].color == 4 && handList[i].size == 2)
                    {
                        if (nan == null)
                        {
                            ++fengCount;
                            nan = handList[i];
                        }
                    }
                    if (handList[i].color == 4 && handList[i].size == 3)
                    {
                        if (xi == null)
                        {
                            ++fengCount;
                            xi = handList[i];
                        }
                    }
                    if (handList[i].color == 4 && handList[i].size == 4)
                    {
                        if (bei == null)
                        {
                            ++fengCount;
                            bei = handList[i];
                        }
                    }
                }

                if (fengCount == 4)
                {
                    {//东南西
                        List<Poker> lst = new List<Poker>();
                        lst.Add(dong);
                        lst.Add(nan);
                        lst.Add(xi);
                        if (MahJongHelper.ContainPoker(poker, lst))
                        {
                            chi.Add(lst);
                        }
                    }
                    {//东南北
                        List<Poker> lst = new List<Poker>();
                        lst.Add(dong);
                        lst.Add(nan);
                        lst.Add(bei);
                        if (MahJongHelper.ContainPoker(poker, lst))
                        {
                            chi.Add(lst);
                        }
                    }
                    {//东西北
                        List<Poker> lst = new List<Poker>();
                        lst.Add(dong);
                        lst.Add(xi);
                        lst.Add(bei);
                        if (MahJongHelper.ContainPoker(poker, lst))
                        {
                            chi.Add(lst);
                        }
                    }
                    {//南西北
                        List<Poker> lst = new List<Poker>();
                        lst.Add(nan);
                        lst.Add(xi);
                        lst.Add(bei);
                        if (MahJongHelper.ContainPoker(poker, lst))
                        {
                            chi.Add(lst);
                        }
                    }
                }
                else if (fengCount == 3)
                {
                    List<Poker> lst = new List<Poker>();
                    if (dong != null)
                    {
                        lst.Add(dong);
                    }
                    if (nan != null)
                    {
                        lst.Add(nan);
                    }
                    if (xi != null)
                    {
                        lst.Add(xi);
                    }
                    if (bei != null)
                    {
                        lst.Add(bei);
                    }
                    chi.Add(lst);
                }
            }
            return chi;
        }
        #endregion

        #region GetPeng 获取碰
        /// <summary>
        /// 获取碰
        /// </summary>
        /// <returns></returns>
        public List<Poker> GetPeng(Poker poker)
        {
            int sameCount = 1;
            List<Poker> lst = new List<Poker>();
            List<Poker> handList = new List<Poker>(PlayerSeat.PokerList);
            for (int i = 0; i < PlayerSeat.DingJiangPoker.Count; ++i)
            {
                for (int j = 0; j < handList.Count; ++j)
                {
                    if (PlayerSeat.DingJiangPoker[i].index == handList[j].index)
                    {
                        handList.Remove(handList[j]);
                        break;
                    }
                }
            }
            for (int i = 0; i < handList.Count; ++i)
            {
                if (handList[i].color == poker.color && handList[i].size == poker.size)
                {
                    lst.Add(handList[i]);
                    ++sameCount;
                    if (sameCount == 3)
                    {
                        lst.Add(poker);
                        return lst;
                    }
                }
            }
            return null;
        }
        #endregion

        #region GetAnGang 获取暗杠
        /// <summary>
        /// 获取暗杠
        /// </summary>
        /// <returns></returns>
        public List<List<Poker>> GetAnGang()
        {
            List<List<Poker>> lst = new List<List<Poker>>();
            List<Poker> handList = new List<Poker>(PlayerSeat.PokerList);
            Poker hitPoker = PlayerSeat.HitPoker;
            if (hitPoker != null)
            {
                handList.Add(hitPoker);
            }
            for (int i = 0; i < handList.Count; ++i)
            {
                List<Poker> combination = new List<Poker>();
                Poker poker = handList[i];
                combination.Add(poker);
                int sameCount = 1;
                for (int j = i + 1; j < handList.Count; ++j)
                {
                    if (poker.color == handList[j].color && poker.size == handList[j].size)
                    {
                        combination.Add(handList[j]);
                        ++sameCount;
                        if (sameCount == 4)
                        {
                            lst.Add(combination);
                            break;
                        }
                    }
                }
            }
#if IS_BAODI
        if (CurrentRoom.Status == RoomEntity.RoomStatus.Show)
        {
            List<Poker> combination = new List<Poker>();
            for (int i = 0; i < handList.Count; ++i)
            {
                if (handList[i].color == 4 && !MahJongHelper.ContainPoker(handList[i], combination))
                {
                    combination.Add(handList[i]);
                }
            }
            if (combination.Count == 4)
            {
                lst.Add(combination);
            }
        }
#endif
            if (PlayerSeat.IsTing)
            {
                for (int i = lst.Count - 1; i >= 0; --i)
                {
                    List<Poker> overplus = new List<Poker>(handList);
                    for (int j = 0; j < lst[i].Count; ++j)
                    {
                        overplus.Remove(lst[i][j]);
                    }
                    List<List<CardCombination>> result = MahJongHelper.CheckTing(overplus, PlayerSeat.UniversalList, !Rule.IsSevenDoubleCantHu, Rule.IsLiangXiCanHu, Rule.is13YaoCanHu);
                    if (result == null || result.Count == 0)
                    {
                        lst.RemoveAt(i);
                    }
                }
            }
            return lst;
        }
        #endregion

        #region GetBuGang 获取补杠
        /// <summary>
        /// 获取补杠
        /// </summary>
        /// <returns></returns>
        public List<Poker> GetBuGang()
        {
            List<Poker> lst = new List<Poker>();
#if IS_LAOGUI
        List<Poker> handList = new List<Poker>();
#else
            List<Poker> handList = new List<Poker>(PlayerSeat.PokerList);
#endif

            Poker hitPoker = PlayerSeat.HitPoker;
            if (hitPoker != null)
            {
                handList.Add(hitPoker);
            }
            List<PokerCombinationEntity> pengList = PlayerSeat.UsedPokerList;
            for (int i = 0; i < pengList.Count; ++i)
            {
                if (pengList[i].CombinationType == OperatorType.Peng || pengList[i].CombinationType == OperatorType.PengTing || pengList[i].CombinationType == OperatorType.Kou)
                {
                    for (int j = 0; j < handList.Count; ++j)
                    {
                        if (handList[j].color == pengList[i].PokerList[0].color && handList[j].size == pengList[i].PokerList[0].size)
                        {
                            lst.Add(handList[j]);
                        }
                    }
                }
            }
            return lst;
        }
        #endregion

        #region GetBuGang 获取补杠
        /// <summary>
        /// 获取补杠
        /// </summary>
        /// <param name="poker"></param>
        /// <returns></returns>
        public List<Poker> GetBuGang(Poker poker)
        {
            List<Poker> lst = new List<Poker>();
            List<Poker> handList = new List<Poker>(PlayerSeat.PokerList);

            Poker hitPoker = PlayerSeat.HitPoker;
            if (hitPoker != null)
            {
                handList.Add(hitPoker);
            }
            List<PokerCombinationEntity> pengList = PlayerSeat.UsedPokerList;
            for (int i = 0; i < pengList.Count; ++i)
            {
                if (pengList[i].CombinationType == OperatorType.Peng || pengList[i].CombinationType == OperatorType.PengTing || pengList[i].CombinationType == OperatorType.Kou)
                {
                    if (poker.color == pengList[i].PokerList[0].color && poker.size == pengList[i].PokerList[0].size)
                    {
                        lst.Add(poker);
                    }
                }
            }
            return lst;
        }
        #endregion

        #region GetMingGang 获取明杠
        /// <summary>
        /// 获取明杠
        /// </summary>
        /// <returns></returns>
        public List<List<Poker>> GetMingGang(Poker poker)
        {
            if (poker == null) return null;
            List<Poker> handList = new List<Poker>(PlayerSeat.PokerList);
            List<List<Poker>> lst = new List<List<Poker>>();

            {
                List<Poker> combination = new List<Poker>();
                int sameCount = 1;
                for (int i = 0; i < handList.Count; ++i)
                {
                    if (handList[i].color == poker.color && handList[i].size == poker.size)
                    {
                        combination.Add(handList[i]);
                        ++sameCount;
                        if (sameCount == 4)
                        {
                            combination.Add(poker);
                            lst.Add(combination);
                            break;
                        }
                    }
                }
            }
            return lst;
        }
        #endregion

        #region GetChiTing 获取吃听
        /// <summary>
        /// 获取吃听
        /// </summary>
        /// <param name="poker"></param>
        /// <returns></returns>
        public List<List<Poker>> GetChiTing(Poker poker)
        {
            List<List<Poker>> ret = new List<List<Poker>>();
            List<List<Poker>> chiList = GetChi(poker);
            for (int i = 0; i < chiList.Count; ++i)
            {
                List<Poker> hand = new List<Poker>(PlayerSeat.PokerList);
                List<PokerCombinationEntity> usedPoker = new List<PokerCombinationEntity>(PlayerSeat.UsedPokerList);
                List<Poker> chi = new List<Poker>();
                for (int j = 0; j < chiList[i].Count; ++j)
                {
                    if (hand.Contains(chiList[i][j]))
                    {
                        chi.Add(chiList[i][j]);
                        hand.Remove(chiList[i][j]);
                    }
                }
                if (!MahJongHelper.ContainPoker(poker, chi))
                {
                    chi.Add(poker);
                }
                PokerCombinationEntity combination = new PokerCombinationEntity(OperatorType.Chi, 0, chi);
                usedPoker.Add(combination);

                Dictionary<Poker, List<Poker>> result = CheckAllTing(hand, usedPoker, PlayerSeat.LackColor);
                if (result != null && result.Count > 0)
                {
                    ret.Add(chiList[i]);
                }
            }

            return ret;
        }
        #endregion

        #region GetPengTing 获取碰听
        /// <summary>
        /// 获取碰听
        /// </summary>
        /// <param name="poker"></param>
        /// <returns></returns>
        public List<Poker> GetPengTing(Poker poker)
        {
            List<Poker> lstPeng = GetPeng(poker);
            List<Poker> hand = new List<Poker>(PlayerSeat.PokerList);
            List<PokerCombinationEntity> usedPoker = new List<PokerCombinationEntity>(PlayerSeat.UsedPokerList);
            List<Poker> peng = new List<Poker>();
            for (int i = 0; i < lstPeng.Count; ++i)
            {
                if (hand.Contains(lstPeng[i]))
                {
                    hand.Remove(lstPeng[i]);
                }
            }
            if (!MahJongHelper.ContainPoker(poker, peng))
            {
                peng.Add(poker);
            }
            PokerCombinationEntity combination = new PokerCombinationEntity(OperatorType.Peng, 0, peng);
            usedPoker.Add(combination);
            Dictionary<Poker, List<Poker>> result = CheckAllTing(hand, usedPoker, PlayerSeat.LackColor);
            if (result != null && result.Count > 0)
            {
                return lstPeng;
            }
            return null;
        }
        #endregion

        #region GetZhiDui 获取支对
        /// <summary>
        /// 获取支对
        /// </summary>
        /// <returns></returns>
        public List<List<Poker>> GetZhiDui(out bool isMustZhiDui)
        {
            isMustZhiDui = true;
            List<List<Poker>> ret = new List<List<Poker>>();

            List<List<CardCombination>> lst = GetTingResult();
            for (int i = 0; i < lst.Count; ++i)
            {
                int doubleCount = 0;
                for (int j = 0; j < lst[i].Count; ++j)
                {
                    if (lst[i][j].CardType == CardType.SameDouble || (lst[i][j].CardType == CardType.SameTriple && lst[i][j].LackCardIds != null && lst[i][j].LackCardIds.Count == 1))
                    {
                        ++doubleCount;
                    }
                }
                if (doubleCount >= 2)
                {
                    for (int j = 0; j < lst[i].Count; ++j)
                    {
                        if (lst[i][j].CardType == CardType.SameDouble || (lst[i][j].CardType == CardType.SameTriple && lst[i][j].LackCardIds != null && lst[i][j].LackCardIds.Count == 1))
                        {
                            bool isExists = false;
                            List<Poker> lstDui = new List<Poker>();
                            for (int k = 0; k < lst[i][j].CurrentCombination.Count; ++k)
                            {
                                for (int l = 0; l < ret.Count; ++l)
                                {
                                    if (MahJongHelper.HasPoker(lst[i][j].CurrentCombination[k], ret[l]))
                                    {
                                        isExists = true;
                                        break;
                                    }
                                }
                                lstDui.Add(lst[i][j].CurrentCombination[k]);
                            }
                            if (!isExists)
                            {
                                ret.Add(lstDui);
                            }

                        }
                    }
                }
                for (int j = 0; j < lst[i].Count; ++j)
                {
                    if ((lst[i][j].CardType & CardType.StraightTriple) != CardType.None && lst[i][j].LackCardIds != null)
                    {
                        isMustZhiDui = false;
                        break;
                    }
                }
            }
            return ret;
        }
        #endregion

        #region GetLiangXi 获取可以亮喜的牌
        /// <summary>
        /// 获取可以亮喜的牌
        /// </summary>
        /// <returns></returns>
        public List<List<Poker>> GetLiangXi()
        {
            AllCanLiangXi.Clear();

            List<Poker> hand = new List<Poker>(PlayerSeat.PokerList);
            if (PlayerSeat.HitPoker != null)
            {
                hand.Add(PlayerSeat.HitPoker);
            }
            List<Poker> lstFeng = new List<Poker>();
            List<Poker> lstZi = new List<Poker>();
            List<int> alreadyExists = new List<int>();

            int fengCount = 0;
            int ziCount = 0;
            for (int i = 0; i < hand.Count; ++i)
            {
                if (hand[i].color == 4)
                {
                    int hash = hand[i].GetHashCode();
                    if (!alreadyExists.Contains(hash))
                    {
                        alreadyExists.Add(hash);
                        ++fengCount;
                    }
                    lstFeng.Add(hand[i]);
                }
                if (hand[i].color == 5)
                {
                    int hash = hand[i].GetHashCode();
                    if (!alreadyExists.Contains(hash))
                    {
                        alreadyExists.Add(hash);
                        ++ziCount;
                    }
                    lstZi.Add(hand[i]);
                }
            }
            if (fengCount >= 3 && lstFeng.Count >= 3)
            {
                for (int i = 0; i < lstFeng.Count; ++i)
                {
                    AllCanLiangXi.Add(new List<Poker>() { lstFeng[i] });
                }
            }
            if (ziCount == 3 && lstZi.Count >= 3)
            {
                for (int i = 0; i < lstZi.Count; ++i)
                {
                    AllCanLiangXi.Add(new List<Poker>() { lstZi[i] });
                }
            }


            return AllCanLiangXi;
        }
        #endregion

        #region GetLiangXi_LuaLu 获取亮喜（撸啊撸）
        /// <summary>
        /// 获取亮喜（撸啊撸）
        /// </summary>
        /// <returns></returns>
        public List<List<Poker>> GetLiangXi_LuaLu()
        {
            List<List<Poker>> ret = new List<List<Poker>>();

            List<Poker> hand = new List<Poker>(PlayerSeat.PokerList);
            if (PlayerSeat.HitPoker != null)
            {
                hand = new List<Poker>(hand);
                hand.Add(PlayerSeat.HitPoker);
            }

            List<Poker> lst1 = new List<Poker>();
            List<Poker> lst9 = new List<Poker>();
            List<Poker> lstZi = new List<Poker>();
            List<Poker> lstFeng = new List<Poker>();

            for (int i = 0; i < hand.Count; ++i)
            {
                if (hand[i].size == 1 && hand[i].color < 4)
                {
                    if (!MahJongHelper.HasPoker(hand[i], lst1))
                    {
                        lst1.Add(hand[i]);
                    }

                }
                if (hand[i].size == 9 && hand[i].color < 4)
                {
                    if (!MahJongHelper.HasPoker(hand[i], lst9))
                    {
                        lst9.Add(hand[i]);
                    }

                }
                if (hand[i].color == 5)
                {
                    if (!MahJongHelper.HasPoker(hand[i], lstZi))
                    {
                        lstZi.Add(hand[i]);
                    }
                }
                if (hand[i].color == 4)
                {
                    if (!MahJongHelper.HasPoker(hand[i], lstFeng))
                    {
                        lstFeng.Add(hand[i]);
                    }
                }
            }
            if (lst1.Count == 3)
            {
                ret.Add(lst1);
            }
            if (lst9.Count == 3)
            {
                ret.Add(lst9);
            }
            if (lstZi.Count == 3)
            {
                ret.Add(lstZi);
            }
            if (Rule.IsFengLiangXi)
            {
                if (lstFeng.Count == 4)
                {
                    ret.Add(lstFeng);
                }
            }
            return ret;
        }
        #endregion

        #region GetDingJiang 获取定将
        /// <summary>
        /// 获取定将
        /// </summary>
        /// <returns></returns>
        public Poker GetDingJiang(Poker poker)
        {
            List<Poker> hand = new List<Poker>(PlayerSeat.PokerList);
            if (PlayerSeat.HitPoker != null)
            {
                hand = new List<Poker>(hand);
                hand.Add(PlayerSeat.HitPoker);
            }

            for (int i = 0; i < hand.Count; ++i)
            {
                if (hand[i].color == poker.color && hand[i].size == poker.size)
                {
                    return poker;
                }
            }
            return null;
        }
        #endregion

        #region CheckDingZhang 检测钉掌
        /// <summary>
        /// 检测钉掌
        /// </summary>
        /// <param name="poker"></param>
        /// <returns></returns>
        public bool CheckDingZhang(Poker poker)
        {
            List<Poker> handList = new List<Poker>(PlayerSeat.PokerList);
            handList.Add(poker);

            int universalCount = 0;
            if (PlayerSeat.UniversalList != null)
            {
                for (int i = handList.Count - 1; i >= 0; --i)
                {
                    if (MahJongHelper.CheckUniversal(handList[i], PlayerSeat.UniversalList))
                    {
                        ++universalCount;
                        handList.RemoveAt(i);
                    }
                }
            }
            return MahJongHelper.CheckSevenDouble(handList, PlayerSeat.UniversalList);
        }
        #endregion

        #region GetFengGang 获取风杠
        /// <summary>
        /// 获取风杠
        /// </summary>
        /// <returns></returns>
        public List<Poker> GetFengGang()
        {
            List<Poker> lst = new List<Poker>();
            List<Poker> handList = new List<Poker>(PlayerSeat.PokerList);
            Poker hitPoker = PlayerSeat.HitPoker;
            if (hitPoker != null)
            {
                handList = new List<Poker>(handList);
                handList.Add(hitPoker);
            }
            for (int i = 0; i < handList.Count; ++i)
            {
                if (handList[i].color == 4 && !MahJongHelper.HasPoker(handList[i], lst))
                {
                    lst.Add(handList[i]);
                }
            }

            if (lst.Count == 4)
            {
                return lst;
            }
            return null;
        }
        #endregion

        #region GetKou 获取扣牌
        /// <summary>
        /// 获取扣牌
        /// </summary>
        /// <returns></returns>
        public List<List<Poker>> GetKou()
        {
            List<List<Poker>> ret = new List<List<Poker>>();
            List<Poker> handList = new List<Poker>(PlayerSeat.PokerList);
            if (PlayerSeat.HitPoker != null)
            {
                handList.Add(PlayerSeat.HitPoker);
            }
            for (int i = 0; i < handList.Count; ++i)
            {
                int sameCount = 1;
                bool isContinue = false;
                for (int j = 0; j < ret.Count; ++j)
                {
                    if (MahJongHelper.HasPoker(handList[i], ret[j]))
                    {
                        isContinue = true;
                        break;
                    }
                }
                if (isContinue)
                {
                    continue;
                }
                List<Poker> combination = new List<Poker>();
                for (int j = i + 1; j < handList.Count; ++j)
                {
                    if (handList[i].color == handList[j].color && handList[i].size == handList[j].size)
                    {
                        combination.Add(handList[j]);
                        ++sameCount;
                        if (sameCount == 3)
                        {
                            combination.Add(handList[i]);
                            ret.Add(combination);
                            break;
                        }
                    }
                }
            }
            return ret;
        }
        #endregion

        #region GetBuXi 获取补喜
        /// <summary>
        /// 获取补喜
        /// </summary>
        public List<Poker> GetBuXi()
        {
            List<Poker> lst = new List<Poker>();
            List<Poker> handList = new List<Poker>(PlayerSeat.PokerList);
            Poker hitPoker = PlayerSeat.HitPoker;
            if (hitPoker != null)
            {
                handList.Add(hitPoker);
            }
            List<PokerCombinationEntity> pengList = PlayerSeat.UsedPokerList;
            for (int i = 0; i < pengList.Count; ++i)
            {
                if (pengList[i].CombinationType == OperatorType.LiangXi)
                {
                    for (int j = 0; j < handList.Count; ++j)
                    {
                        if (MahJongHelper.ContainPoker(handList[j], PlayerSeat.DingJiangPoker)) continue;
                        if (MahJongHelper.HasPoker(handList[j], lst)) continue;
                        if (MahJongHelper.HasPoker(handList[j], pengList[i].PokerList))
                        {
                            lst.Add(handList[j]);
                        }
                    }
                }
            }
            return lst;
        }
        #endregion

        #region BeforeCheckTing 检查是否满足听牌条件，检查碰的牌里有什么
        /// <summary>
        /// 检测听牌前的检测，检查碰的牌里有什么
        /// </summary>
        /// <param name="pokers"></param>
        /// <param name="hasPeng"></param>
        /// <param name="hasWan"></param>
        /// <param name="hasTong"></param>
        /// <param name="hasTiao"></param>
        /// <param name="has19"></param>
        /// <param name="hongzhongCount"></param>
        /// <param name="hasUsedPoker"></param>
        /// <returns></returns>
        private bool BeforeCheckTing(List<Poker> pokers, List<PokerCombinationEntity> usedPoker, out int pengCount, out bool hasWan, out bool hasTong, out bool hasTiao, out bool has19, out int hongzhongCount, out bool hasUsedPoker, out bool hasChi, out int gangCount, out int liangxiCount)
        {

            pengCount = 0;//碰的数量
            gangCount = 0;//杠的数量
            liangxiCount = 0;//亮喜数量
            hasWan = false;//是否有万
            hasTong = false;//是否有筒
            hasTiao = false;//是否有条
            has19 = false;//是否有19
            hongzhongCount = 0;//红中数量
            hasUsedPoker = false;//是否门清
            hasChi = false;//是否有吃

            //不听不能胡
            //if (Rule.IsNotTingCantHu && !isTing)
            //{
            //    return false;
            //}


            List<PokerCombinationEntity> combination = usedPoker;
            for (int j = 0; j < combination.Count; ++j)
            {
                if (combination[j].CombinationType == OperatorType.Peng || combination[j].CombinationType == OperatorType.PengTing)
                {
                    ++pengCount;
                }
                if (combination[j].CombinationType == OperatorType.Gang)
                {
                    ++gangCount;
                }
                //亮喜代替刻
                if (Rule.IsXiReplaceSameTriple && combination[j].CombinationType == OperatorType.LiangXi)
                {
                    ++liangxiCount;
                }
                if (combination[j].CombinationType == OperatorType.Chi || combination[j].CombinationType == OperatorType.ChiTing)
                {
                    hasChi = true;
                }
                for (int k = 0; k < combination[j].PokerList.Count; ++k)
                {
                    if (combination[j].PokerList[k].color == 5)
                    {
                        has19 = true;
                        ++hongzhongCount;
                    }
                    if (combination[j].CombinationType != OperatorType.LiangXi)
                    {
                        if (combination[j].PokerList[k].color == 1) hasWan = true;
                        if (combination[j].PokerList[k].color == 2) hasTong = true;
                        if (combination[j].PokerList[k].color == 3) hasTiao = true;
                    }
                    if ((combination[j].PokerList[k].size == 1 || combination[j].PokerList[k].size == 9) && combination[j].PokerList[k].color < 4) has19 = true;
                }
            }
            if (!has19)
            {
                has19 = MahJongHelper.Has19(pokers, Rule.IsHongZhongReplace19);
            }
            if (combination != null && combination.Count > 0)
            {
                if (Rule.IsAnGangNotUsedPoker)
                {
                    for (int i = 0; i < combination.Count; ++i)
                    {
                        if (combination[i].CombinationType != OperatorType.Gang || (combination[i].CombinationType == OperatorType.Gang && combination[i].SubTypeId != 4))
                        {
                            hasUsedPoker = true;
                            break;
                        }
                    }
                }
                else
                {
                    hasUsedPoker = true;
                }
            }

            for (int j = 0; j < pokers.Count; ++j)
            {
                if (pokers[j].color == 1) hasWan = true;
                if (pokers[j].color == 2) hasTong = true;
                if (pokers[j].color == 3) hasTiao = true;
            }

#if !IS_TAILAI
            //是否不能清一色
            if (Rule.IsCantSingleColor)
            {
                int colorCount = 0;
                if (hasWan) ++colorCount;
                if (hasTiao) ++colorCount;
                if (hasTong) ++colorCount;
                if (colorCount < 2)
                {
                    return false;
                }
            }
#endif



            //Debug.Log("通过BeforeCheckTing");
            return true;
        }
        #endregion

        #region FilterUnqualified 过滤掉不合格的胡牌结果 
        /// <summary>
        /// 过滤掉不合格的胡牌结果
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="has19"></param>
        /// <param name="hasPeng"></param>
        /// <param name="hongzhongCount"></param>
        private void FilterUnqualified(List<List<CardCombination>> lst, bool has19, int pengCount, int hongzhongCount, bool hasChi, int gangCount, int liangxiCount, bool hasUsedPoker, bool hasWan, bool hasTong, bool hasTiao)
        {
            //Debug.Log("开始过滤不能胡的牌------------------------------");
            //Debug.Log("过滤前数量 : " + lst.Count);
            for (int w = lst.Count - 1; w >= 0; --w)
            {
                bool isContinue = false;
                //检测钉掌的牌是否挪为他用
                for (int i = 0; i < PlayerSeat.UsedPokerList.Count; ++i)
                {
                    if (PlayerSeat.UsedPokerList[i].CombinationType == OperatorType.DingZhang)
                    {
                        for (int j = 0; j < lst[w].Count; ++j)
                        {
                            if (lst[w][j].CardType != CardType.SameDouble)
                            {
                                lst.RemoveAt(w);
                                isContinue = true;
                                break;
                            }
                        }
                    }
                    if (isContinue) break;
                }
                if (isContinue) continue;

                if (!Rule.IsSevenDoubleCantHu)
                {
                    int doubleCount = 0;
                    for (int i = 0; i < lst[w].Count; ++i)
                    {
                        if (lst[w][i].CardType == CardType.SameDouble)
                        {
                            ++doubleCount;
                        }
                    }
                    if (doubleCount == 7)
                    {
                        continue;
                    }
                }

#if !IS_TAILAI
                //是否硬幺
                if (Rule.IsHardYao)
                {
                    if (!has19)
                    {
                        lst.RemoveAt(w);
                        continue;
                    }
                }
#endif

                if (Rule.IsCanPiaoHu)//检测飘胡
                {
                    int sameTripleCount = pengCount + gangCount + liangxiCount;
                    for (int i = 0; i < lst[w].Count; ++i)
                    {
                        if (lst[w][i].CardType == CardType.SameTriple)
                        {
                            ++sameTripleCount;
                        }
                    }
                    if (sameTripleCount == 4)
                    {
                        continue;
                    }
                }



                //门清不能胡
                if (Rule.IsNotUsedCantHu && !hasUsedPoker)
                {
                    lst.RemoveAt(w);
                    continue;
                }


                //是否必须花色全
                if (Rule.IsMustAllColor)
                {
                    if (!hasWan || !hasTong || !hasTiao)
                    {
                        lst.RemoveAt(w);
                        continue;
                    }
                }
                //是否不能清一色
                if (Rule.IsCantSingleColor)
                {
                    int colorCount = 0;
                    if (hasWan) ++colorCount;
                    if (hasTiao) ++colorCount;
                    if (hasTong) ++colorCount;
                    if (colorCount < 2)
                    {
                        lst.RemoveAt(w);
                        continue;
                    }
                }
                //是否硬幺
                if (Rule.IsHardYao)
                {
                    if (!has19)
                    {
                        lst.RemoveAt(w);
                        continue;
                    }
                }

                if (PlayerSeat.DingJiangPoker.Count != 0)
                {
                    for (int i = 0; i < lst[w].Count; ++i)
                    {
                        if (lst[w][i].CardType == CardType.SameTriple)
                        {
                            lst.RemoveAt(w);
                            isContinue = true;
                            break;
                        }
                        if (lst[w][i].CardType == CardType.SameDouble)
                        {
                            for (int j = 0; j < lst[w][i].CurrentCombination.Count; ++j)
                            {
                                if (!MahJongHelper.ContainPoker(lst[w][i].CurrentCombination[j], PlayerSeat.DingJiangPoker))
                                {
                                    lst.RemoveAt(w);
                                    isContinue = true;
                                    break;
                                }
                            }
                        }
                        if (isContinue) break;
                    }
                }
                if (isContinue) continue;
                bool isHas19 = has19;
                bool isHasPeng = (pengCount + gangCount + liangxiCount) > 0;
                int ziCount = hongzhongCount;

                //计算字数量
                for (int i = 0; i < lst[w].Count; ++i)
                {
                    for (int j = 0; j < lst[w][i].CurrentCombination.Count; ++j)
                    {
                        if (lst[w][i].CurrentCombination[j].color == 5)
                        {
                            ++ziCount;
                        }
                    }
                    if (lst[w][i].LackCardIds != null)
                    {
                        for (int j = 0; j < lst[w][i].LackCardIds.Count; ++j)
                        {
                            if (lst[w][i].LackCardIds[j].color == 5)
                            {
                                ++ziCount;
                            }
                        }
                    }
                }

                if (Rule.HongZhongIsSameTriple && ziCount > 0)//如果红中可以代替刻
                {
                    isHasPeng = true;
                }

                if (Rule.IsMustHasSameTriple && !isHasPeng)//如果必须有刻
                {
                    int prevSameDoubleCount = 0;
                    for (int i = 0; i < lst[w].Count; ++i)
                    {
                        if (lst[w][i].CardType == CardType.SameTriple)
                        {
                            isHasPeng = true;
                        }
                        if (lst[w][i].CardType == CardType.SameDouble)
                        {
                            ++prevSameDoubleCount;
                        }
                        if (prevSameDoubleCount > 1)
                        {
                            isHasPeng = true;
                        }
                    }

                    if (!isHasPeng)
                    {
                        lst.RemoveAt(w);
                        continue;
                    }
                }

                if (Rule.IsMustHas19 && ziCount == 0 && !isHas19) //如果没有19 不能胡
                {
                    for (int i = 0; i < lst[w].Count; ++i)
                    {
                        if (MahJongHelper.Has19(lst[w][i].CurrentCombination, Rule.IsHongZhongReplace19))
                        {
                            isHas19 = true;
                            break;
                        }

                    }
                    if (!isHas19)
                    {
                        for (int i = 0; i < lst[w].Count; ++i)
                        {
                            if (lst[w][i].LackCardIds != null)
                            {
                                for (int j = lst[w][i].LackCardIds.Count - 1; j >= 0; --j)
                                {
                                    if ((lst[w][i].LackCardIds[j].size == 1 || lst[w][i].LackCardIds[j].size == 9) && lst[w][i].LackCardIds[j].color < 4)
                                    {
                                        isHas19 = true;
                                    }
                                    else if (Rule.IsHongZhongReplace19 && lst[w][i].LackCardIds[j].color == 5)//如果红中可以代替19
                                    {
                                        isHas19 = true;
                                    }
                                    else
                                    {
                                        lst[w][i].LackCardIds.RemoveAt(j);
                                    }
                                }
                            }
                        }
                    }
                    if (!isHas19)
                    {
                        lst.RemoveAt(w);
                        continue;
                    }
                }
                if (Rule.IsMustHasStrightTriple && !hasChi)//如果必须有顺子
                {
                    bool hasStraightTriple = false;
                    for (int i = 0; i < lst[w].Count; ++i)
                    {
                        CardCombination currCombination = lst[w][i];
                        if ((currCombination.CardType & CardType.StraightTriple) != CardType.None)
                        {
                            hasStraightTriple = true;
                            break;
                        }
                    }
                    if (!hasStraightTriple)
                    {
                        lst.RemoveAt(w);
                        continue;
                    }
                }
                if (Rule.IsNotJiaCantHu)//如果不夹不能胡
                {
                    bool hasJia = false;
                    int sameDoubleCount = 0;
                    for (int i = 0; i < lst[w].Count; ++i)
                    {
                        CardCombination currCombination = lst[w][i];
                        if (currCombination.LackCardIds != null)
                        {
                            if (currCombination.CardType == CardType.straightLackDouble)//顺子缺两张，算夹胡
                            {
                                hasJia = true;
                                break;
                            }
                            if (currCombination.CardType == CardType.StraightLackMiddle)//顺子缺中间，算夹胡
                            {
                                hasJia = true;
                                break;
                            }
                            if (currCombination.CardType == CardType.StraightLack37)//缺37
                            {
                                if (Rule.Is37Jia)//如果37夹
                                {
                                    hasJia = true;
                                }
                            }
                            if (currCombination.CardType == CardType.SameDouble)
                            {
                                if (Rule.IsSingleJia)//如果单钓夹
                                {
                                    if (currCombination.LackCardIds != null && currCombination.LackCardIds.Count > 0)
                                    {
                                        hasJia = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (currCombination.CardType == CardType.SameDouble || (currCombination.CardType == CardType.SameTriple && currCombination.LackCardIds != null && currCombination.LackCardIds.Count == 1))
                        {
                            ++sameDoubleCount;
                        }
                    }
                    if (Rule.IsDoubleDoubleJia)//鸿鹄激情场对倒算夹
                    {
                        if (sameDoubleCount == 2)
                        {
                            hasJia = true;
                        }
                    }
                    if (Rule.IsZhiDui)//如果可以支对
                    {
                        if (sameDoubleCount == 2)
                        {
                            hasJia = true;
                        }
                    }

                    if (!hasJia)
                    {
                        lst.RemoveAt(w);
                        continue;
                    }
                }
            }
        }
        #endregion

        #region GetLackPoker 从听牌结果里获取缺的牌
        /// <summary>
        /// 从听牌结果里获取缺的牌
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        private List<Poker> GetLackPoker(List<List<CardCombination>> lst)
        {
            List<Poker> lackPokers = new List<Poker>();
            if (lst == null || lst.Count == 0) return lackPokers;
            //循环所有的结果
            for (int j = 0; j < lst.Count; ++j)
            {
                //循环该结果下所有组合
                for (int k = 0; k < lst[j].Count; ++k)
                {
                    List<Poker> lackCards = lst[j][k].LackCardIds;
                    if (lackCards != null)
                    {
                        //循环该组合所有缺的牌
                        for (int l = 0; l < lackCards.Count; ++l)
                        {
                            bool isExists = false;
                            //循环已经找到的缺的牌
                            for (int m = 0; m < lackPokers.Count; ++m)
                            {
                                if (lackCards[l].color == lackPokers[m].color && lackCards[l].size == lackPokers[m].size)
                                {
                                    isExists = true;
                                    break;
                                }
                            }
                            if (!isExists)
                            {
                                lackPokers.Add(lackCards[l]);
                            }
                        }
                    }
                }
            }
            //红中满天飞
            if (Rule.IsHongZhongFly)
            {
                Poker hongzhong = new Poker(0, 5, 1, 0);
                if (!MahJongHelper.HasPoker(hongzhong, lackPokers))
                {
                    lackPokers.Add(hongzhong);
                }
            }

            //刮大风
            if (Rule.IsBigWind)
            {
                for (int i = 0; i < lst.Count; ++i)
                {
                    for (int j = 0; j < lst[i].Count; ++j)
                    {
                        if (lst[i][j].CardType == CardType.SameTriple && lst[i][j].LackCardIds == null)
                        {
                            lackPokers.Add(new Poker(lst[i][j].CurrentCombination[0]));
                        }
                    }
                }
            }
            return lackPokers;
        }
        #endregion

        #region GetTingResult 获取听牌结果
        /// <summary>
        /// 获取听牌结果
        /// </summary>
        /// <returns></returns>
        public List<List<CardCombination>> GetTingResult()
        {
            List<Poker> hand = PlayerSeat.PokerList;
            if (PlayerSeat.HitPoker != null)
            {
                hand = new List<Poker>(hand);
                hand.Add(PlayerSeat.HitPoker);
            }
            //手把一
            if (Rule.IsHandOnlyOneCantHu)
            {
                if (hand.Count < 3) return null;
            }

            for (int i = 0; i < hand.Count; ++i)
            {
                if (hand[i].color == PlayerSeat.LackColor) return null;
            }

            List<Poker> ting = new List<Poker>(hand);

            int pengCount = 0;//碰数量
            int gangCount = 0;//杠数量
            int liangxiCount = 0;//亮喜数量
            bool hasWan = false;//是否有万
            bool hasTong = false;//是否有筒
            bool hasTiao = false;//是否有条
            bool has19 = false;//是否有19
            int hongzhongCount = 0;//红中数量
            bool hasUsedPoker = false;//是否门清
            bool hasChi = false;
            if (!BeforeCheckTing(ting, PlayerSeat.UsedPokerList, out pengCount, out hasWan, out hasTong, out hasTiao, out has19, out hongzhongCount, out hasUsedPoker, out hasChi, out gangCount, out liangxiCount)) return null;

            for (int i = 0; i < PlayerSeat.UsedPokerList.Count; ++i)
            {
                if (PlayerSeat.UsedPokerList[i].CombinationType == OperatorType.DingZhang)
                {
                    for (int j = 0; j < PlayerSeat.UsedPokerList[i].PokerList.Count; ++j)
                    {
                        ting.Add(PlayerSeat.UsedPokerList[i].PokerList[j]);
                    }
                }
            }

            List<List<CardCombination>> lst = MahJongHelper.CheckTing(ting, PlayerSeat.UniversalList, !Rule.IsSevenDoubleCantHu, Rule.IsLiangXiCanHu, Rule.is13YaoCanHu);
            if (lst.Count > 0)
            {
                FilterUnqualified(lst, has19, pengCount, hongzhongCount, hasChi, gangCount, liangxiCount, hasUsedPoker, hasWan, hasTong, hasTiao);
                return lst;
            }
            return null;
        }
        #endregion

        #region CheckTingByHand 检测当前手牌听什么
        /// <summary>
        /// 检测当前手牌听什么
        /// </summary>
        /// <returns></returns>
        public List<Poker> CheckTingByHand(bool containHitPoker = true)
        {
            List<Poker> hand = new List<Poker>(PlayerSeat.PokerList);
            if (PlayerSeat.HitPoker != null && containHitPoker)
            {
                hand.Add(PlayerSeat.HitPoker);
            }
            //手把一
            if (Rule.IsHandOnlyOneCantHu)
            {
                if (hand.Count < 3) return null;
            }

            bool isExistsUniversal = false;
            for (int i = 0; i < PlayerSeat.UniversalList.Count; ++i)
            {
                Poker uni = PlayerSeat.UniversalList[i];
                for (int j = 0; j < hand.Count; ++j)
                {
                    Poker h = hand[j];
                    if (uni.color == h.color && uni.size == h.size)
                    {
                        isExistsUniversal = true;
                        break;
                    }
                }
                if (isExistsUniversal) break;
            }
            if (isExistsUniversal)
            {
                //不打癞子不能胡
                if (Rule.isPlayedUniversalCanHu && !PlayerSeat.isPlayedUniversal)
                {
                    return null;
                }
            }

            List<Poker> lackPokers = GetLackPoker(hand, PlayerSeat.UsedPokerList, PlayerSeat.LackColor);
            return lackPokers;
        }
        #endregion

        #region GetTingCombination 获取听牌组合
        /// <summary>
        /// 获取听牌组合
        /// </summary>
        /// <param name="pokers"></param>
        /// <param name="usedPoker"></param>
        /// <returns></returns>
        private List<List<CardCombination>> GetTingCombination(List<Poker> pokers, List<PokerCombinationEntity> usedPoker, int lackColor)
        {
            List<Poker> ting = new List<Poker>(pokers);
            //手把一
            if (Rule.IsHandOnlyOneCantHu)
            {
                if (ting.Count < 3) return null;
            }

            for (int i = 0; i < pokers.Count; ++i)
            {
                if (pokers[i].color == lackColor) return null;
            }

            int pengCount = 0;//碰数量
            int gangCount = 0;//杠数量
            int liangxiCount = 0;//亮喜数量
            bool hasWan = false;//是否有万
            bool hasTong = false;//是否有筒
            bool hasTiao = false;//是否有条
            bool has19 = false;//是否有19
            int hongzhongCount = 0;//红中数量
            bool hasUsedPoker = false;//是否门清
            bool hasChi = false;
            if (!BeforeCheckTing(ting, usedPoker, out pengCount, out hasWan, out hasTong, out hasTiao, out has19, out hongzhongCount, out hasUsedPoker, out hasChi, out gangCount, out liangxiCount)) return null;
            for (int i = 0; i < PlayerSeat.UsedPokerList.Count; ++i)
            {
                if (PlayerSeat.UsedPokerList[i].CombinationType == OperatorType.DingZhang)
                {
                    for (int j = 0; j < PlayerSeat.UsedPokerList[i].PokerList.Count; ++j)
                    {
                        ting.Add(PlayerSeat.UsedPokerList[i].PokerList[j]);
                    }
                }
            }

            List<List<CardCombination>> lst = MahJongHelper.CheckTing(ting, PlayerSeat.UniversalList, !Rule.IsSevenDoubleCantHu, Rule.IsLiangXiCanHu, Rule.is13YaoCanHu);

            if (lst.Count > 0)
            {
                FilterUnqualified(lst, has19, pengCount, hongzhongCount, hasChi, gangCount, liangxiCount, hasUsedPoker, hasWan, hasTong, hasTiao);
            }
            return lst;
        }
        #endregion

        #region GetLackPoker 获取所有胡的牌
        /// <summary>
        /// 获取所有胡的牌
        /// </summary>
        /// <param name="pokers"></param>
        /// <param name="usedPoker"></param>
        /// <returns></returns>
        private List<Poker> GetLackPoker(List<Poker> pokers, List<PokerCombinationEntity> usedPoker, int lackColor)
        {
            List<List<CardCombination>> lst = GetTingCombination(pokers, usedPoker, lackColor);
            return GetLackPoker(lst);
        }
        #endregion

        #region CheckAllTing 检查打一张之后是否听牌
        /// <summary>
        /// 检查打一张之后是否听牌
        /// </summary>
        /// <returns></returns>
        public Dictionary<Poker, List<Poker>> CheckAllTing()
        {
            m_DicTing.Clear();
            List<Poker> hand = new List<Poker>(PlayerSeat.PokerList);
            if (PlayerSeat.HitPoker != null)
            {
                hand = new List<Poker>(hand);
                hand.Add(PlayerSeat.HitPoker);
            }
            Dictionary<Poker, List<Poker>> dic = CheckAllTing(hand, PlayerSeat.UsedPokerList, PlayerSeat.LackColor);
            if (dic != null)
            {
                foreach (var pair in dic)
                {
                    m_DicTing.Add(pair.Key, pair.Value);
                }
            }


            return m_DicTing;
        }
        #endregion

        #region CheckAllTing 检查打一张之后是否听牌
        /// <summary>
        /// 检查打一张之后是否听牌
        /// </summary>
        /// <returns></returns>
        private Dictionary<Poker, List<Poker>> CheckAllTing(List<Poker> poker, List<PokerCombinationEntity> usedPoker, int lackColor)
        {
            Dictionary<Poker, List<Poker>> ret = new Dictionary<Poker, List<Poker>>();

            List<Poker> hand = new List<Poker>(poker);

            if (hand.Count < 2) return null;
            //手把一
            if (Rule.IsHandOnlyOneCantHu)
            {
                if (hand.Count < 3) return null;
            }
            List<Poker> overplusPoker = new List<Poker>();
            for (int q = 0; q < hand.Count; ++q)
            {
                if (MahJongHelper.ContainPoker(hand[q], PlayerSeat.DingJiangPoker)) continue;
                overplusPoker.Clear();
                Poker lack = hand[q];
                overplusPoker.AddRange(hand);
                overplusPoker.Remove(lack);

                bool isExistsUniversal = false;
                for (int i = 0; i < PlayerSeat.UniversalList.Count; ++i)
                {
                    Poker uni = PlayerSeat.UniversalList[i];
                    for (int j = 0; j < hand.Count; ++j)
                    {
                        Poker h = hand[j];
                        if (uni.color == h.color && uni.size == h.size)
                        {
                            isExistsUniversal = true;
                            break;
                        }
                    }
                    if (isExistsUniversal) break;
                }
                if (isExistsUniversal)
                {
                    //不打癞子不能胡
                    if (Rule.isPlayedUniversalCanHu && !PlayerSeat.isPlayedUniversal && !MahJongHelper.HasPoker(lack, PlayerSeat.UniversalList))
                    {
                        continue;
                    }
                }


                List<Poker> lackPokers = GetLackPoker(overplusPoker, usedPoker, PlayerSeat.LackColor);
                if (lackPokers != null && lackPokers.Count > 0)
                {
                    ret.Add(lack, lackPokers);
                }
            }
            return ret;
        }
        #endregion

        #region GetHu 获取胡牌信息
        /// <summary>
        /// 获取胡牌信息
        /// </summary>
        public List<Poker> GetHu(Poker poker)
        {
            if (!m_DicTing.ContainsKey(poker)) return null;

            return m_DicTing[poker];
        }
        #endregion

        #region GetAllTing 获取所有听牌信息
        /// <summary>
        /// 获取所有听牌信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<Poker, List<Poker>> GetAllTing()
        {
            return m_DicTing;
        }
        #endregion

        #region GetSeat 获取座位实体
        /// <summary>
        /// 根据玩家Id获取座位
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public SeatEntity GetSeatByPlayerId(int playerId)
        {
            if (CurrentRoom == null) return null;
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                if (CurrentRoom.SeatList[i].PlayerId == playerId)
                {
                    return CurrentRoom.SeatList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 根据座位号获取座位
        /// </summary>
        /// <param name="seatId"></param>
        /// <returns></returns>
        public SeatEntity GetSeatBySeatId(int seatId)
        {
            if (CurrentRoom == null) return null;
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                if (CurrentRoom.SeatList[i].Pos == seatId)
                {
                    return CurrentRoom.SeatList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 根据索引获取座位
        /// </summary>
        /// <param name="seatIndex"></param>
        /// <returns></returns>
        public SeatEntity GetSeatBySeatIndex(int seatIndex)
        {
            if (CurrentRoom == null) return null;
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                if (CurrentRoom.SeatList[i].Index == seatIndex)
                {
                    return CurrentRoom.SeatList[i];
                }
            }
            return null;
        }
        #endregion

        #region SendRoomInfoChangeNotify 发送房间信息变更消息
        /// <summary>
        /// 发送房间信息变更消息
        /// </summary>
        public void SendRoomInfoChangeNotify(bool containSeat = true)
        {
            TransferData roomData = new TransferData();
            roomData.SetValue("Room", CurrentRoom);
            roomData.SetValue("CurrentOperator", CurrentRoom.CurrentOperator);
            roomData.SetValue("PlayerSeatPos", PlayerSeat.Pos);
            SendNotification(ON_ROOM_INFO_CHANGED, roomData);

            if (!containSeat) return;
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                SendSeatInfoChangeNotify(CurrentRoom.SeatList[i]);
            }
        }
        #endregion

        #region SendSeatInfoChangeNotify 发送座位信息变更消息
        /// <summary>
        /// 发送座位信息变更消息
        /// </summary>
        /// <param name="seat"></param>
        private void SendSeatInfoChangeNotify(SeatEntity seat)
        {
            if (seat == null) return;

            TransferData data = new TransferData();
            data.SetValue("Seat", seat);
            data.SetValue("IsPlayer", seat == PlayerSeat);
            data.SetValue("RoomStatus", CurrentRoom.Status);
            data.SetValue("PlayerStatus", PlayerSeat.Status);
            SendNotification(ON_SEAT_INFO_CHANGED, data);
        }
        #endregion

        #region SetCountDown 设置倒计时
        /// <summary> 
        /// 设置倒计时
        /// </summary>
        /// <param name="countDown"></param>
        public void SetCountDown(long countDown, bool isPlayer = false)
        {
            TransferData data = new TransferData();
            data.SetValue("ServerTime", countDown);
            data.SetValue("IsPlayer", isPlayer);
            SendNotification(ON_COUNT_DOWN_CHANGED, data);
        }
        #endregion

        public string GetPokerLog(List<Poker> pokers)
        {
            if (pokers == null) return string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < pokers.Count; ++i)
            {
                sb.AppendFormat("{0}, ", pokers[i].ToString(true, true));
            }
            return sb.ToString();
        }


        #region ResetSeat 重置座位信息
        /// <summary>
        /// 重置座位信息
        /// </summary>
        /// <param name="seat"></param>
        private void ResetSeat(SeatEntity seat)
        {
            if (seat == null) return;
            seat.UniversalList.Clear();
            seat.DeskTopPoker.Clear();
            seat.PokerList.Clear();
            seat.UsedPokerList.Clear();
            seat.HitPoker = null;
            seat.Status = SeatEntity.SeatStatus.Wait;
            seat.IsTing = false;
            seat.isHu = false;
            seat.isPlayedUniversal = false;
        }
        #endregion
    }
}