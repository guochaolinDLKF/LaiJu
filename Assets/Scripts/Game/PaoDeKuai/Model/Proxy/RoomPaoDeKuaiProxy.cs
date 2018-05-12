//===================================================
//Author      : WZQ
//CreateTime  ：11/13/2017 3:59:41 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaoDeKuai;
using proto.pdk;
public class RoomPaoDeKuaiProxy : ProxyBase<RoomPaoDeKuaiProxy>
{
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
    /// 当前操作的座位
    /// </summary>
    public SeatEntity CurrentOperator { get; private set; }

    /// <summary>
    /// 提示实体
    /// </summary>
    public HintPokersEntity Currhint;

    ///// <summary>
    ///// 当前游戏状态
    ///// </summary>
    //public MahjongGameState CurrentState;

    ///// <summary>
    ///// 游戏规则
    ///// </summary>
    //public MahjongRule Rule;





    #endregion

    #region InitRoom 初始化房间数据
    public void InitRoom(ROOM_INFO protoRoom)
    {
        //ROOM_INFO protoRoom = proto.roomInfo;
        CurrentRoom = new RoomEntity()
        {
            //BaseScore = proto.baseScore,
            roomId = protoRoom.roomId,
            ownerId = protoRoom.ownerId,
            currentLoop = protoRoom.loop,
            //matchId = protoRoom.matchId,//比赛场ID
            Status = (RoomEntity.RoomStatus)protoRoom.roomStatus,
            maxLoop = protoRoom.maxLoop,
            DisbandStartTime = protoRoom.dismissTime,
            DisbandTime = protoRoom.dismissMaxTime,
            DisbandTimeMax = (int)(protoRoom.dismissMaxTime - protoRoom.dismissTime),

        };
        CurrentRoom.HistoryPoker = new List<Poker>();

        CurrentRoom.SeatList = new List<SeatEntity>();

        //收到数据存到模型基类
        CurrentRoom.Config.Clear();
        for (int i = 0; i < protoRoom.settingIdCount(); ++i)
        {
            cfg_settingEntity settingEntity = cfg_settingDBModel.Instance.Get(protoRoom.getSettingId(i));

            if (settingEntity != null)
            {
                CurrentRoom.Config.Add(settingEntity);
            }
        }

        //获得当前游戏模式
        for (int i = 0; i < CurrentRoom.Config.Count; i++)
        {
            if (CurrentRoom.Config[i].tags.Equals("mode"))
            {
                //CurrentRoom.roomModel = (Room.RoomModel)CurrentRoom.Config[i].value;

            }
        }
        for (int i = 0; i < CurrentRoom.Config.Count; i++)
        {
            if (CurrentRoom.Config[i].tags.Equals("roomMode"))
            {
                //CurrentRoom.superModel = (Room.SuperModel)CurrentRoom.Config[i].value;
                //Debug.Log("服务器发送的房间 普通高级场模式为为：" + CurrentRoom.Config[i].value);
            }
        }



        Debug.Log("房间ID为：" + protoRoom.roomId);
        Debug.Log("服务器发送的房间座位长度为：" + protoRoom.getSeatInfoList().Count);

        CurrentRoom.SeatCount = protoRoom.getSeatInfoList().Count;

        CurrentRoom.SeatList = new List<SeatEntity>();

        for (int i = 0; i < CurrentRoom.SeatCount; ++i)
        {
            SEAT_INFO protoSeat = protoRoom.getSeatInfo(i);
            SeatEntity seat = new SeatEntity();
           

            //是否同意解散
            if (protoSeat.hasSeatStatus())
            {
                seat.DisbandState = (DisbandState)protoSeat.dismiss_status;
            }

            //庄
            //seat.IsBanker = protoSeat.isBanker;
            //玩家ID
            seat.PlayerId = protoSeat.playerId;
            //房主
            seat.isLandlord = seat.PlayerId== protoRoom .ownerId;
            //pos
            seat.Pos = protoSeat.pos;
            //昵称
            seat.Nickname = protoSeat.nickname;
            //头像
            seat.Avatar = protoSeat.avatar;
            //性别
            seat.Gender = protoSeat.gender;
            //是否准备
            seat.IsReady = protoSeat.isReady;

            //已有金币
            seat.Gold = protoSeat.gold;

            //座位状态
            seat.Status= (SeatEntity.SeatStatus)protoSeat.seatStatus;
            ////本局收益
            //seat.Earnings = protoSeat.nn_earnings;
            ////是否是胜利者
            //seat.Winner = protoSeat.isWiner;

            ////纬度
            //if (protoSeat.hasLatitude()) seat.Latitude = protoSeat.latitude;

            ////经度
            //if (protoSeat.hasLongitude()) seat.Longitude = protoSeat.longitude;


            //手牌类型


            seat.pokerList = new List<Poker>();
            //具体手牌
            if (protoSeat.hasPokerInfo())
            {
                List<POKER_INFO> prPokerList= protoSeat.getPokerInfoList();
              Debug.Log("prPokerList：" + prPokerList.Count);
                for (int j = 0; j < prPokerList.Count; ++j)
                {
                    seat.pokerList.Add(new Poker(prPokerList[j].index, prPokerList[j].size, prPokerList[j].color));
                }
                seat.HandPockerNum = prPokerList.Count;
            }

            CurrentRoom.SeatList.Add(seat);
            if (seat.Status == SeatEntity.SeatStatus.Operate) CurrentRoom.OperateSeat = seat;
        }

        CalculateSeatIndex();

        //上家出牌
        List<Poker> recentlyPlayPoker = new List<Poker>();
        List<POKER_INFO> leftPoker = new List<POKER_INFO>();
        if (protoRoom.hasLeftPoker()) leftPoker = protoRoom.leftPoker.getPokerInfoList();
        for (int i = 0; i < leftPoker.Count; ++i)
        {
            recentlyPlayPoker.Add(new Poker(leftPoker[i].index, leftPoker[i].size, leftPoker[i].color));
        }
        int leftPokerPos = protoRoom.hasLeftPoker() ? protoRoom.leftPoker.pos : 0;
        CurrentRoom.CurrAlreadyPlayPos = leftPokerPos;
        CurrentRoom.RecentlyPlayPoker = new CombinationPokersEntity(leftPokerPos, recentlyPlayPoker, PokersType.None, 0);
        PaoDeKuaiHelper.CheckPokerType(CurrentRoom.RecentlyPlayPoker);
        Currhint = new HintPokersEntity(CurrentRoom.RecentlyPlayPoker);

        SeatEntity leftseat = GetSeatBySeatPos(leftPokerPos);
        if (leftseat != null)
        {
            if (recentlyPlayPoker.Count > 0)
                leftseat.pokerList.AddRange(recentlyPlayPoker);
        }

        SetSeatPass(CurrentRoom.RecentlyPlayPoker.Pos, CurrentRoom.OperateSeat);

        //if (proto.nn_room.hasUnixtime())
        //{
        //    CurrentRoom.serverTime = proto.nn_room.unixtime;//（时间戳）
        //}
        

    }
    #endregion

    #region EnterRoom 进入房间
    /// <summary>
    /// 进入房间
    /// </summary>
    /// <param name="pbSeat"></param>
    public void EnterRoom(PDK_ENTER_ROOM proto)
    {
        if (CurrentRoom == null) return;
        //if (CurrentRoom.SeatList.Count == 2 && proto.pos == 2)
        //{
        //    proto.pos = 3;
        //}
        Debug.Log("座位" + proto.pos + "进入房间");
        SeatEntity seat = GetSeatBySeatPos(proto.pos);
        if (seat == null) return;
        seat.PlayerId = proto.playerId;
        seat.isLandlord = seat.PlayerId == CurrentRoom.ownerId;
        seat.Gold = 0;
        //seat.Gold = proto.gold;
        seat.Avatar = proto.avatar;
        seat.Gender = proto.gender;
        seat.Nickname = proto.nickname;
        //seat.IP = proto.ipaddr;
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
    public void ExitRoom(PDK_LEAVE proto)
    {
        SeatEntity seat = GetSeatByPlayerId(proto.playerId);
        if (seat == null) return;
        seat.PlayerId = 0;
        seat.isLandlord = false;
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


    #region 
    #endregion

    #region Ready 准备
    /// <summary>
    /// 准备
    /// </summary>
    /// <param name="proto"></param>
    public void Ready(PDK_READY proto)
    {
        SeatEntity seat = GetSeatBySeatPos(proto.pos);
        if (seat == null) return;
        seat.IsReady = true;
        seat.Status = SeatEntity.SeatStatus.Ready;
        SendSeatInfoChangeNotify(seat);
    }
    #endregion

    #region  Begin 开局发牌
    /// <summary>
    /// 开局发牌
    /// </summary>
    public void Begin(PDK_BEGIN proto)
    {
        ++CurrentRoom.currentLoop;

        //黑桃3的座位
        CurrentRoom.SpadesThreePos = proto.pos;

        List<SEAT_INFO> protoSeatList= proto.getSeatInfoList();

        for (int i = 0; i < protoSeatList.Count; ++i)
        {
            SeatEntity seat = GetSeatBySeatPos(protoSeatList[i].pos);
            if (seat == null) continue;
            Debug.Log(string.Format("Pos:{0}  Count:{1}", seat.Pos, protoSeatList[i].getPokerInfoList().Count));
            seat.HandPockerNum = protoSeatList[i].HandPocker;
            ResetSeat(seat);

            seat.Status = proto.pos == seat.Pos ? SeatEntity.SeatStatus.Operate : SeatEntity.SeatStatus.Wait;
            

            List<POKER_INFO> prPokerList = protoSeatList[i].getPokerInfoList();
            Debug.Log("发牌位置" + seat.Pos);
            //手牌
            for (int j = 0; j < prPokerList.Count; ++j)
            {
                Debug.Log(string.Format("index:{0} size:{1} color:{2}", prPokerList[j].index, prPokerList[j].size, prPokerList[j].color));

                seat.pokerList.Add(new Poker(prPokerList[j].index, prPokerList[j].size, prPokerList[j].color));
            }
            PaoDeKuaiHelper.Sort(seat.pokerList);
        }

        CurrentRoom.Status =  RoomEntity.RoomStatus.Begin;


        SendRoomInfoChangeNotify();

    }
    #endregion

    #region  PlayPoker 出牌
    /// <summary>
    /// 出牌
    /// </summary>
    /// <param name="proto"></param>
    public PokersType PlayPoker(PDK_OPERATE proto)
    {
        SeatEntity seat = GetSeatBySeatPos(proto.pos);
        if (seat == null) return PokersType.None;
        CurrentRoom.OperateSeat = null;
          CurrentRoom.CurrAlreadyPlayPos = proto.pos;
        //CurrentRoom.RecentlyPlayPoker.Reset();
        //CurrentRoom.CurrPlayPoker.Clear();
        seat.Status = SeatEntity.SeatStatus.Wait;

        List<Poker> playPoker = new List<Poker>();
        List<POKER_INFO> prPlayPokers = proto.getPokerInfoList();

        for (int i = 0; i < prPlayPokers.Count; i++)
        {
            for (int j = 0; j < seat.pokerList.Count; ++j)
            {
                if (prPlayPokers[i].index == seat.pokerList[j].index)
                {
                    seat.pokerList[j].SetPaker(prPlayPokers[i].size, prPlayPokers[i].color);
                    playPoker.Add(seat.pokerList[j]);
                    CurrentRoom.HistoryPoker.Add(seat.pokerList[j]);//已出牌
                    //CurrentRoom.CurrPlayPoker.Add(seat.pokerList[j]);
                    seat.pokerList.RemoveAt(j);
                    break;
                }
            }
        }
        PaoDeKuaiHelper.Sort(playPoker);
     
        CurrentRoom.RecentlyPlayPoker=new CombinationPokersEntity (proto.pos, playPoker, PokersType.None,0);
        PaoDeKuaiHelper.CheckPokerType(CurrentRoom.RecentlyPlayPoker);
        Debug.Log(string.Format("出牌玩家：{0} 检测牌型：{1}",seat.PlayerId, CurrentRoom.RecentlyPlayPoker.PokersType.ToString()));

       
        //是自己出牌
        Currhint.Reset();
        if (proto.pos == PlayerSeat.Pos)
        {
            //清空上家牌
            Currhint.Others.Reset();
        }
        else
        {
            Currhint.Others = CurrentRoom.RecentlyPlayPoker;
        }

        if (seat == PlayerSeat) SendOperateStateChangeNotify(seat);
        SendSeatInfoChangeNotify(seat);
        SetCountDown(0, seat == PlayerSeat, seat.Index,true);
        HistoryPokerChanged();
        return CurrentRoom.RecentlyPlayPoker.PokersType;
    }
    #endregion

    #region NoticeSeatOperate  通知某座位操作
    /// <summary>
    /// 通知某座位操作
    /// </summary>
    /// <param name="proto"></param>
    public void NoticeSeatOperate(PDK_NEXT_PLAYER proto)
    {
        SeatEntity seat = GetSeatBySeatPos(proto.pos);
        if (seat == null) return;

        CurrentRoom.OperateSeat = seat;
        SetSeatPass(CurrentRoom.RecentlyPlayPoker.Pos, CurrentRoom.OperateSeat);
        if (seat.Pos == CurrentRoom.CurrAlreadyPlayPos)
        {
            //某人继续出牌
            CurrentRoom.RecentlyPlayPoker.Reset();
        }
      
            seat.Status = SeatEntity.SeatStatus.Operate;

        if (proto.pos == PlayerSeat.Pos)
        {
           
            //重置提示
            Currhint.Reset();
        }

        
        if (seat == PlayerSeat) SendOperateStateChangeNotify(seat);
        SendRoomInfoChangeNotify();
        //SendSeatInfoChangeNotify(seat);

        SetCountDown(0,seat==PlayerSeat,seat.Index);
    }
    #endregion

    #region Pass  过
    /// <summary>
    /// 过
    /// </summary>
    public void Pass(PDK_PASS proto)
    {
        SeatEntity seat = GetSeatBySeatPos(proto.pos);
        if (seat == null) return;
        seat.IsPass = true;
        seat.Status = SeatEntity.SeatStatus.Wait;
        SetCountDown(0, seat == PlayerSeat, seat.Index,true);
        SendSeatInfoChangeNotify(seat);
    }
    #endregion

    #region Settle 结算
    /// <summary>
    /// 结算
    /// </summary>
    /// <param name="proto"></param>
    public void Settle(PDK_GAME_OVER proto)
    {
        //proto.winnert_pos;
        Debug.Log("通知小结算");
        List<SEAT_SETTING> prSeatList = proto.getSeatSettingList();

        CurrentRoom.WinnertPos = proto.winnert_pos;
        CurrentRoom.Status = RoomEntity.RoomStatus.Settle;
        for (int i = 0; i < prSeatList.Count; ++i)
        {
            for (int j = 0; j < CurrentRoom.SeatList.Count; ++j)
            {
                if (prSeatList[i].pos==CurrentRoom.SeatList[j].Pos)
                {
                    Debug.Log(string.Format("Pos:{0} Gold{1}", prSeatList[i].pos, prSeatList[i].gold));
                    CurrentRoom.SeatList[j].Earnings=  prSeatList[i].gold- CurrentRoom.SeatList[j].Gold;
                    //CurrentRoom.SeatList[j].Gold = prSeatList[i].gold;

                    List<POKER_INFO> prPokerList = prSeatList[i].getPokerInfoList();
                    //牌
                    UpdatePokerInfo(prPokerList, CurrentRoom.SeatList[j].pokerList);
                    break;
                }
            }
        }

        for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
        {
            SetGold(CurrentRoom.SeatList[i].PlayerId, CurrentRoom.SeatList[i].Earnings);
        }
        SendRoomInfoChangeNotify();


    }

    public void UpdatePokerInfo(List<POKER_INFO> prSeatList,List<Poker> pokerList)
    {
        for (int i = 0; i < prSeatList.Count; ++i)
        {
            for (int j = 0; j < pokerList.Count; ++j)
            {
                if (pokerList[j].index == prSeatList[i].index)
                {
                    pokerList[j].SetPaker(prSeatList[i].size, prSeatList[i].color);
                    break;
                }
            }
        }

    }

    #endregion

    #region RoomStatusChangeNotify 房间状态变化
    /// <summary>
    /// 房间状态变化
    /// </summary>
    /// <param name="roomStatus"></param>
    public void RoomStatusChangeNotify(RoomEntity.RoomStatus roomStatus)
    {
        Debug.Log("房间状态变更为" + roomStatus.ToString());
        CurrentRoom.Status = roomStatus;
        SendRoomInfoChangeNotify();    
    }
    #endregion


    #region CheakHint 提示
    /// <summary>
    /// 提示
    /// </summary>
    /// <returns></returns>
    public List<Poker> CheakHint()
    {
        //List<Poker> pokerList = PaoDeKuaiHelper.CopyPokerList(PlayerSeat.pokerList);
        PaoDeKuaiHelper.HintPoker(Currhint,PlayerSeat.pokerList);

        Debug.Log("本次提示牌：");
        List<Poker> pokers = Currhint.CurrHint.Pokers;
        if (pokers != null)
        {
            for (int i = 0; i < pokers.Count; ++i)
            {
                Debug.Log(pokers[i].ToChinese());
            }
        }


        return Currhint.CurrHint.Pokers;

    }
    #endregion

    #region CheckCanPlayPoker 检测能否出牌
    /// <summary>
    /// 检测能否出牌
    /// </summary>
    /// <param name="toPokers"></param>
    /// <returns></returns>
    public bool CheckCanPlayPoker(List<Poker> toPokers )
    {
        if (toPokers==null) return false;
        Debug.Log("检测能否出牌:");
        for (int i = 0; i < toPokers.Count; i++)
        {
            Debug.Log("检测能否出牌:" +toPokers[i].ToChinese());
        }



        if (PlayerSeat.CheckThree())
        {

            if (!PaoDeKuaiHelper.CheckThree(toPokers))
            {
                Debug.Log("第一手出的牌必须带有黑桃3");
                return false;
            }
        }

        //检测
        CombinationPokersEntity combinationPokers = new CombinationPokersEntity(PlayerSeat.Pos,toPokers, PokersType.None,0);
        PaoDeKuaiHelper.CheckPokerType(combinationPokers);

        Debug.Log("检测能否出牌:" + combinationPokers.PokersType.ToString());

      


        if (combinationPokers.PokersType == PokersType.None) return false;

        if (combinationPokers.PokersType == PokersType.Three && PlayerSeat.pokerList.Count>3)
        {
            return false;
        }


        if (CurrentRoom.RecentlyPlayPoker.Pokers.Count ==0 )
        {
            //没有上家牌
            return true;
        }
        else
        {
            if (combinationPokers.PokersType== CurrentRoom.RecentlyPlayPoker.PokersType)
            {
                return combinationPokers.CurrSize > CurrentRoom.RecentlyPlayPoker.CurrSize;
            }
            else 
            {
                return combinationPokers.PokersType == PokersType.Bomb;
            }
        }
    }
    #endregion

    #region NextLoopGame 下一局
    /// <summary>
    /// 下一局
    /// </summary>
    public void NextLoopGame()
    {
        CurrentRoom.Status = RoomEntity.RoomStatus.Show;
        CurrentRoom.SpadesThreePos = 0;
        CurrentRoom.CurrAlreadyPlayPos = 0;
        CurrentRoom.RecentlyPlayPoker.Reset();
        CurrentRoom.HistoryPoker.Clear();
        //CurrentRoom.
        Currhint.Reset();
        for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
        {
            CurrentRoom.SeatList[i].HandPockerNum = 0;
            CurrentRoom.SeatList[i].Earnings = 0;
            CurrentRoom.SeatList[i].pokerList.Clear();
            CurrentRoom.SeatList[i].RoundPoker.Clear();
        }
        HistoryPokerChanged();
    }
    #endregion

    /// <summary>
    /// 设置座位是否Pass
    /// </summary>
    /// <param name="alreadyPlayPos"></param>
    /// <param name="currPos"></param>
    public void SetSeatPass(int playPos,SeatEntity operateSeat)
    {
        if (playPos == 0 || operateSeat == null|| playPos == operateSeat.Pos)
        {
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                CurrentRoom.SeatList[i].IsPass = false;
            }
            return;
        }
        int currPos = operateSeat.Pos;

        for (int j = 0; j < CurrentRoom.SeatList.Count; ++j)
        {
            CurrentRoom.SeatList[j].IsPass = (playPos > currPos && (CurrentRoom.SeatList[j].Pos > playPos || CurrentRoom.SeatList[j].Pos < currPos)) ||
                 (playPos < currPos && (CurrentRoom.SeatList[j].Pos > playPos && CurrentRoom.SeatList[j].Pos < currPos));
        }




    }

    #region ClearRoom 清空房间数据
    /// <summary>
    /// 清空房间数据
    /// </summary>
    public void ClearRoom()
    {
        CurrentRoom = null;
        PlayerSeat = null;
        CurrentOperator = null;
        Currhint = null;
        //CurrentState = MahjongGameState.None;
        //Rule = null;

        //RecentlyPlayPoker = null;

    }
    #endregion

    #region ResetSeat 重置座位信息
    /// <summary>
    /// 重置座位信息
    /// </summary>
    /// <param name="seat"></param>
    private void ResetSeat(SeatEntity seat)
    {
        if (seat == null) return;
        //seat.UniversalList.Clear();
        seat.DeskTopPoker.Clear();
        seat.pokerList.Clear();
        seat.RoundPoker.Clear();
        //seat.UsedPokerList.Clear();
        seat.Status = SeatEntity.SeatStatus.Wait;
        seat.HandPockerNum = 0;
        //seat.isPlayedUniversal = false;
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
            seatIndex = seatIndex < 0 ? seatIndex + CurrentRoom.SeatList.Count : seatIndex;
            seat.Index = seatIndex;
            if (CurrentRoom.SeatList.Count == 3)
            {
                if (seat.Index == 2)
                {
                    seat.Index = 3;
                }
            }
        }
    }
    #endregion

    #region SetSeatDisbandState 设置座位解散状态
    /// <summary>
    /// 设置座位解散状态
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="isAgree"></param>
    public void SetSeatDisbandState(int playerId, DisbandState disbandState, long disbandStartTime = 0,long dismissEndTime=0)
    {
        SeatEntity seat = GetSeatByPlayerId(playerId);
        if (seat == null) return;
        seat.DisbandState = disbandState;
        if (disbandStartTime != 0)
        {
            CurrentRoom.DisbandStartTime = disbandStartTime;
            CurrentRoom.DisbandTime = dismissEndTime;
            CurrentRoom.DisbandTimeMax = (int)(dismissEndTime - disbandStartTime);
        }
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
    public SeatEntity GetSeatBySeatPos(int seatPos)
    {
        if (CurrentRoom == null) return null;
        for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
        {
            if (CurrentRoom.SeatList[i].Pos == seatPos)
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
        roomData.SetValue("CurrentOperator", CurrentOperator);
        roomData.SetValue("PlayerSeatPos", PlayerSeat.Pos);
        SendNotification(ConstDefine_PaoDeKuai.ON_ROOM_INFO_CHANGED, roomData);

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
        SendNotification(ConstDefine_PaoDeKuai.ON_SEAT_INFO_CHANGED, data);
    }
    #endregion

    #region SetCountDown 设置倒计时
    /// <summary> 
    /// 设置倒计时
    /// </summary>
    /// <param name="countDown"></param>
    public void SetCountDown(long countDown, bool isPlayer = false, int seatIndex = 0, bool isClose = false)
    {
        Debug.Log("设置倒计时");
        TransferData data = new TransferData();
        data.SetValue("ServerTime", countDown);
        data.SetValue("IsPlayer", isPlayer);
        data.SetValue("IsClose", isClose);
        data.SetValue("SeatIndex", seatIndex);
        SendNotification(ConstDefine_PaoDeKuai.ON_COUNT_DOWN_CHANGED, data);
    }
    #endregion



    #region  SendOperateStateChangeNotify 玩家操作状态变更
    /// <summary> 
    /// 玩家操作状态变更
    /// </summary>
    /// <param name="countDown"></param>
    public void SendOperateStateChangeNotify(SeatEntity seat)
    {
        if (seat != PlayerSeat) return;
        TransferData data = new TransferData();
        //data.SetValue("Seat", seat);
        data.SetValue("RoomStatus", CurrentRoom.Status);
        data.SetValue("IsPlayer", seat == PlayerSeat);
        data.SetValue("PlayerStatus", PlayerSeat.Status);
        SendNotification(ConstDefine_PaoDeKuai.ON_OperateState_CHANGED, data);
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
        SendNotification(ConstDefine_PaoDeKuai.ON_SEAT_GOLD_CHANGED, data);
    }
    #endregion

    public void HistoryPokerChanged()
    {
        TransferData data = new TransferData();
        data.SetValue("HistoryPoker", CurrentRoom.HistoryPoker);
        SendNotification(ConstDefine_PaoDeKuai.ON_HistoryPoker_CHANGED, data);

    }

    ////获得poker长度
    //public string GetPokerLog(List<Poker> pokers)
    //{
    //    if (pokers == null) return string.Empty;
    //    StringBuilder sb = new StringBuilder();
    //    for (int i = 0; i < pokers.Count; ++i)
    //    {
    //        sb.AppendFormat("{0}, ", pokers[i].ToString(true, true));
    //    }
    //    return sb.ToString();
    //}



}
