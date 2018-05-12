
public class ZhaJHconsts  {
    /// <summary>
    /// 创建房间
    /// </summary>
    public const int OP_ZJH_ROOM_CREATE = 401001;
    /// <summary>
    /// 进入房间
    /// </summary>
    public const int OP_ZJH_ROOM_ENTER = 401002;
    /// <summary>
    /// 离开房间
    /// </summary>
    public const int OP_ZJH_ROOM_LEAVE = 401003;
    /// <summary>
    /// 准备
    /// </summary>
    public const int OP_ZJH_ROOM_READY = 401004;
    /// <summary>
    /// 开始游戏
    /// </summary>
    public const int OP_ZJH_ROOM_BEGIN = 401005;
    /// <summary>
    /// 断线重连
    /// </summary>
    public const int OP_ZJH_ROOM_RECREATE = 401006;
    /// <summary>
    /// 申请解散房间
    /// </summary>
    public const int OP_ZJH_ROOM_APPLY_DISMISS = 401007;
    /// <summary>
    /// 解散房间答复
    /// </summary>
    public const int OP_ZJH_ROOM_REPLY_DISMISS = 401008;
    /// <summary>
    /// 解散房间成功
    /// </summary>
    public const int OP_ZJH_ROOM_DISMISS_SUCCEED = 401009;
    /// <summary>
    /// 解散房间失败
    /// </summary>
    public const int OP_ZJH_ROOM_DISMISS_FAIL = 401010;
    /// <summary>
    /// 看牌
    /// </summary>
    public const int OP_ZJH_ROOM_OPEN_LOOK_POCKER = 401011;
    /// <summary>
    /// 下一家下注
    /// </summary>
    public const int OP_ZJH_ROOM_NEXT_POUR = 401012;
    /// <summary>
    /// 得到房间解散结果
    /// </summary>
    public const int OP_ZJH_ROOM_DISSOLVE = 401013;
    /// <summary>
    /// 通知开始下一局
    /// </summary>
    public const int OP_ZJH_ROOM_NEXT_GAME = 401014;
    /// <summary>
    /// 通知房间房局结束
    /// </summary>
    public const int OP_ZJH_ROOM_GAME_OVER = 401015;
    /// <summary>
    /// 发牌
    /// </summary>
    public const int OP_ZJH_ROOM_DEAL = 401016;
    /// <summary>
    /// 跟注
    /// </summary>
    public const int OP_ZJH_ROOM_FOLLOW_POUR = 401017;
    /// <summary>
    /// 加注
    /// </summary>
    public const int OP_ZJH_ROOM_ADD_POUR = 401018;
    /// <summary>
    /// 比牌
    /// </summary>
    public const int OP_ZJH_ROOM_COMPARE_POKER = 401019;
    /// <summary>
    /// 弃牌
    /// </summary>
    public const int OP_ZJH_ROOM_LOSE_POKER = 401020;
}

public class ZhaJHButtonConstant
{
    //======================================游戏场景按钮======================================
    public const string btnZhaJHViewReady = "btnZhaJHViewReady";//准备
    public const string btnZhaJHViewShare = "btnZhaJHViewShare";
    public const string btnZhaJHViewChat = "btnZhaJHViewChat";//聊天
    public const string btnZhaJHViewLicensing = "btnZhaJHViewLicensing";
    public const string btnZJHWithNotes = "btnZJHWithNotes";//加注
    public const string btnZJHLookPoker = "btnZJHLookPoker";//看牌
    public const string btnZJHLosePoker = "btnZJHLosePoker";
   // public const string tolShowGDD = "ShowGDD";//跟到底的开关

    public const string btnZhaJHViewLightPoker = "btnZhaJHViewLightPoker";//亮牌按钮
}

public class ZhaJHMethodname
{
    public const string OnZJHDealLookEnd = "OnZJHDealLookEnd";//发牌结束，通知玩家下注
    public const string HairPoker = "HairPoker";//生成牌
    public const string AlreadyPrepared = "AlreadyPrepared";//已经发牌，隐藏已准备图片
    public const string OnZJHLicensingShow = "OnZJHLicensingShow";//是庄，显示发牌按钮
    public const string OnZJHBtnShow = "OnZJHBtnShow";//显示或者隐藏 看牌、跟注 等按钮  
    public const string OnZJHHidFen = "OnZJHHidFen";//隐藏已选过 加注按钮的分数
    public const string HidLookPoker = "HidLookPoker";//隐藏看牌按钮
    public const string OnZJHLookPoker = "OnZJHLookPoker";//看牌方法
    public const string LookPokerSign = "LookPokerSign";//非客户端显示看牌的图片
    public const string OnZJHHidBP = "OnZJHHidBP";//隐藏比牌按钮
    public const string OnZJHHidKP = "OnZJHHidKP";
    public const string OnZJHBtnQP = "OnZJHBtnQP";
    public const string OnZJHBtnGDD = "OnZJHBtnGDD";
    public const string OnZJHHidFenG = "OnZJHHidFenG";
    public const string OnZJHHidImage = "OnZJHHidImage";


    public const string SetImageTime = "SetImageTime";//倒计时

    public const string OnZJHCloseCountTime = "OnZJHCloseCountTime";//关闭倒计时
    public const string SetSeatInfoFen = "SetSeatInfoFen";//增加分数。方法在玩家 Item 上

    public const string OnZJHNoMaskP = "OnZJHNoMaskP";//重新排序ItemSeat  的方法





    //------------------------------------高级房------------------------------------------
    // public const string PromptSwitch = "PromptSwitch";//高级房通知房主有人申请加入房间

    public const string OnZJHLookupPlayer = "OnZJHLookupPlayer";//高级房房主是否同意玩家进入

    public const string OnZJHCloneApplyJoin = "OnZJHCloneApplyJoin";//高级房生成申请预制体

    public const string OnZJHAgreeRefuse = "OnZJHAgreeRefuse";//向服务器发送同意或者拒绝玩家进入房间

    public const string btnWithdraw = "btnWithdraw";//高级房取消申请进入房间按钮

    public const string OnZJHWithdrawEnterRoom = "OnZJHWithdrawEnterRoom";//服务器返回取消进入房间成功的方法，脚本在 UIZJHWaitWindow 上，脚本在预制体上

    public const string RefuseToEnterRoom = "RefuseToEnterRoom";//服务器返回拒绝玩家进入的方法


    //=========================================================================================================================================

    public const string OnZJHSeatInfoChanged = "OnZJHSeatInfoChanged";//更改座位信息
    public const string OnZJHRoomInfoChanged = "OnZJHRoomInfoChanged";//房间信息变更
    public const string OnZJHSeatLookPoker = "OnZJHSeatLookPoker";//看牌方法
    public const string OnZJHSeatInfoGold = "OnZJHSeatInfoGold";
    public const string OnZJHSetSeatInfoOperation = "OnZJHSetSeatInfoOperation";
    public const string OnZJHSeatInfoBet = "OnZJHSeatInfoBet";
    public const string OnZJHTheCardAnimation = "OnZJHTheCardAnimation";
    public const string OnZJHCloseZhuangTai = "OnZJHCloseZhuangTai";
    public const string OnZJHOpenPoker = "OnZJHOpenPoker";
    public const string OnZJHShufflingPoker = "OnZJHShufflingPoker";
    public const string OnZJHSeatLowScore = "OnZJHSeatLowScore";
    public const string OnZJHCloseShufflingPoker = "OnZJHCloseShufflingPoker";
    public const string OnZJHDiscardMethod = "OnZJHDiscardMethod";
    public const string OnZJHSettlementInterfaceView = "OnZJHSettlementInterfaceView";
    public const string OnZJHTotalSettlementView = "OnZJHTotalSettlementView";
    public const string OnZJHApplyForDissolution = "OnZJHApplyForDissolution";
    public const string OnZJHHideApplyForDissolution = "OnZJHHideApplyForDissolution";
    public const string OnZJHAutomaticReady = "OnZJHAutomaticReady";
    public const string OnZJHCloseIEtor = "OnZJHCloseIEtor";
    public const string OnZJHShoppingImageTweenMethod = "OnZJHShoppingImageTweenMethod";
    public const string OnZJHliangfen = "OnZJHliangfen";
    public const string OnZJHThanPoker = "OnZJHThanPoker";
    public const string OnZJHReturnHall = "OnZJHReturnHall";
    public const string OnZJHBtnMicroUp = "OnZJHBtnMicroUp";
    public const string OnZJHBtnMicroCancel = "OnZJHBtnMicroCancel";
    public const string OnZJHHairPoker = "OnZJHHairPoker";
    public const string OnZJHInfoSettlement = "OnZJHInfoSettlement";
    public const string OnZJHInfoSettlement1 = "OnZJHInfoSettlement1";
    public const string OnZJHHairPokerAni = "OnZJHHairPokerAni";
    public const string OnZJHCleantRoom = "OnZJHCleantRoom";


}