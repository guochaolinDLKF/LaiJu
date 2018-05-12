//===================================================
//Author      : DRB
//CreateTime  ：7/5/2016 11:32:12 PM
//Description ：自定义常量类
//===================================================

using DRB.Common;
/// <summary>
/// 自定义常量类
/// </summary>
public class ConstDefine
{
    //=======================================登陆窗口按钮======================================
    public const string BtnGetIdentifyingCode = "btnGetIdentifyingCode";
    public const string BtnLogin = "btnLogin";

    //=======================================主界面===========================================
    public const string BtnRule = "btn_rule";
    public const string BtnShare = "btn_share";
    public const string BtnRecord = "btn_record";
    public const string BtnSevice = "btn_sevice";
    public const string BtnSetting = "btn_setting";
    public const string BtnGame = "btn_game"; 
    public const string BtnShop = "btn_shop";
    public const string BtnPresent = "btn_present";
    public const string BtnMatch = "btn_Match";
    public const string BtnInvite = "btn_invite";
    public const string BtnMessage = "btn_message";
    public const string BtnMyRoom = "btn_MyRoom";
    public const string BtnRealName = "btn_realname";


    //=======================================选择房间窗口按钮==================================
    public const string BtnSelectRoomViewCreateRoom = "btnSelectRoomViewCreateRoom";
    public const string BtnSelectRoomViewJoinRoom = "btnSelectRoomViewJoinRoom";
    public const string BtnSelectRoomViewBack = "btnSelectRoomViewBack";
    public const string BtnSelectRoomViewRefresh = "btnSelectRoomViewRefresh";


    //=======================================创建房间窗口按钮==================================
    public const string BtnCreateRoomViewCreate = "btnCreateRoomViewCreate";
    public const string BtnCreateRoomViewBack = "btnCreateRoomViewBack";

    //==================================窗口返回按钮=======================================
    public const string BtnRecordViewBack = "btnRecordViewBack";
    public const string BtnServiceViewBack = "btnServiceViewBack";
    public const string BtnShareViewBack = "btnShareViewBack";
    public const string BtnShopViewBack = "btnShopViewBack";

    //======================================加入房间按钮======================================
    public const string BtnJoinRoomViewJoin = "btnJoinRoomViewJoin";

    //======================================游戏场景按钮======================================
    public const string BtnGameViewRule = "btnMaJiangViewRule";
    public const string BtnGameViewChat = "btnMaJiangViewChat";
    public const string BtnGameViewShare = "btnMaJiangViewShare";

    public const string BtnMaJiangViewAuto = "btnMaJiangViewAuto";
    public const string BtnMaJiangViewCancelAuto = "btnMaJiangViewCancelAuto";
    public const string BtnMaJiangViewReady = "btnMaJiangViewReady";
    public const string BtnMaJiangViewCancelReady = "btnMaJiangViewCancelReady";

    //斗地主
    public const string btnDLandlordViewOpenStart = "btnDLandlordViewOpenStart ";//明牌开始
    public const string btnDLandlordViewReady = "btnDLandlordViewReady";//开始游戏
    public const string btnDLandlordViewChat = "btnDLandlordViewChat";//聊天按钮
    public const string btnDLandlordViewShare = "btnDLandlordViewShare";//邀请微信好友
    public const string btnDLandlordViewAuto = "btnDLandlordViewAuto";//托管
    public const string btnDLandlordViewCancelAuto = "btnDLandlordViewCancelAuto";//取消托管
    public const string btnDLandlordViewPlay = "btnDLandlordViewPlay";//出牌
    public const string btnDLandlordViewPrompt = "btnDLandlordViewPrompt";//提示
    public const string btnDLandlordViewNoPlay = "btnDLandlordViewNoPlay";//不出
    public const string btnFen1 = "btnFen1";
    public const string btnFen2 = "btnFen1";
    public const string btnFen3 = "btnFen3";


    //====================================分享窗口按钮=======================================
    public const string BtnShareFriend = "btnShareFriend";
    public const string BtnShareMoments = "btnShareMoments";

    //=====================================设置窗口按钮======================================
    public const string BtnSettingViewRule = "btnSettingViewRule";
    public const string BtnSettingViewQuit = "btnSettingViewQuit";
    public const string BtnSettingViewDisband = "btnSettingViewDisband";
    public const string BtnSettingViewShare = "btnSettingViewShare";
    public const string BtnSettingViewChangeUser = "btnSettingViewChangeUser";
    public const string BtnSettingViewAudio = "btnSettingViewAudio";
    public const string BtnSettingViewBind = "btnSettingViewBind";
    public const string BtnSettingViewMail = "btnSettingViewMail";
    public const string BtnSettingViewLeave = "btnSettingViewLeave";

    //======================================结算界面按钮===================================
    public const string BtnSettleViewReplayOver = "btnSettleViewReplayOver";


    //=====================================比赛按钮========================================
    public const string BtnMatchTipViewBowout = "btnMatchTipViewBowout";



    //======================================消息按钮========================================
    public const string BtnMailViewNotice = "btnMailViewNotice";
    public const string BtnMailViewMail = "btnMailViewMail";

    //======================================送礼按钮========================================
    public const string BtnPresentViewPresent = "btnPresentViewPresent";
    public const string BtnPresentViewReturn = "btnPresentViewReturn";
    public const string BtnPresentViewNext = "btnPresentViewNext";

    //=======================================Http接口======================================
    public const string HTTPAddrGuest = "passport/guest/";
    public const string HTTPFuncGuest = "guest";
    public const string HTTPAddrGive = "Game/give/";
    public const string HTTPFuncGive = "give";
    public const string HTTPAddrWXCheck = "weixin/wxcheck/";
    public const string HTTPFuncWXCheck = "wxcheck";
    public const string HTTPAddrWXLogin = "weixin/wxlogin/";
    public const string HTTPFuncWXLogin = "wxlogin";
    public const string HTTPAddrGateway = "game/gateway/";
    public const string HTTPFuncGateway = "gateway";
    public const string HTTPAddrMatch = "game/match/";
    public const string HTTPFuncMatch = "match";
    public const string HTTPAddrRecord = "game/record/";
    public const string HTTPFuncRecord = "record";
    public const string HTTPAddrBattle = "game/battle/";
    public const string HTTPFuncBattle = "battle";
    public const string HTTPAddrPlayer = "game/player/";
    public const string HTTPFuncPlayer = "player";
    public const string HTTPAddrWXPayOrder = "pay/order/";
    public const string HTTPFuncWXPayOrder = "order";
    public const string HTTPAddrCards = "game/cards/";
    public const string HTTPFuncCards = "cards";
    public const string HTTPAddrJoin = "game/enter/";
    public const string HTTPFuncJoin = "enter";
    public const string HTTPAddrRenter = "game/renter/";
    public const string HTTPFuncRenter = "renter";
    public const string HTTPAddrInform = "game/inform/";
    public const string HTTPFuncInform = "inform";
    public const string HTTPAddrNotice = "game/notice/";
    public const string HTPPFuncNotice = "notice";
    public const string HTTPAddrShared = "game/shared/";
    public const string HTTPFuncShared = "shared";
    public const string HTTPAddrBindCode = "passport/bindcode/";
    public const string HTTPFuncBindCode = "bindCode";
    public const string HTTPAddrVerifyCode = "passport/verifyCode/";
    public const string HTTPFuncVerifyCode = "verifyCode";
    public const string HTTPAddrBindPhone = "passport/bindphone/";
    public const string HTTPFuncBindPhone = "bindPhone";
    public const string HTTPAddrUnBindPhone = "passport/unbindPhone/";
    public const string HTTPFuncUnBindPhone = "unbindPhone";
    public const string HTTPAddrMail = "game/mail/";
    public const string HTTPFuncMail = "mail";
    public const string HTTPAddrRanking = "game/ranking/";
    public const string HTTPFuncRanking = "ranking";
    public const string HTTPAddrScore = "game/score/";
    public const string HTTPFuncScore = "score";



    public const string HTTPAddrPlayerInfo = "agent/player/";
    public const string HTTPFuncPlayerInfo = "player";
    public const string HTTPAddrRecharge = "agent/pay/";
    public const string HTTPFuncRecharge = "pay";
    public const string HTTPAddrLog = "agent/log/";
    public const string HTTPFuncLog = "log";

    public const string HTTPAddrInit = "game/init/";
    public const string HTTPFuncInit = "init";
    public const string HTTPAddrRelogin = "passport/relogin/";
    public const string HTTPFuncRelogin = "relogin";
    public const string HTTPAddrUrlBind = "agent/urlbind/";
    public const string HTTPFuncUrlBind = "urlbind";
    public const string HTTPAddrCodeBind = "agent/codebind/";
    public const string HTTPFuncCodeBind = "codebind";
    public const string HTTPAddrVerifyPay = "apple/verifyPay/";
    public const string HTTPFuncVerifyPay = "verifyPay";


    //=======================================登录校验常量===================================
    /// <summary>
    /// 用户名正则
    /// </summary>
    public const string AccountRegex = @"^([a-zA-Z0-9]|[_]){6,16}$";
    /// <summary>
    /// 手机号正则
    /// </summary>
    public const string TelNumRegex = @"^[1]+[3,5,8,7]+\d{9}";
    /// <summary>
    /// 验证码正则
    /// </summary>
    public const string VerifyCodeRegex = @"^\d{4,11}$";


    //===========================================分割符========================================
    public const string DATE_TIME_FORMAT = "yyyy-MM-dd";
    public const string TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

    /// <summary>
    /// 值分割符
    /// </summary>
    public const char ValueSeparator = ';';
    /// <summary>
    /// 坐标分隔符
    /// </summary>
    public const char PositionSeparator = '_';
    /// <summary>
    /// 项分割符
    /// </summary>
    public static readonly char[] ItemSeparator = { '\r','\n'};
    /// <summary>
    /// 字符串正负号格式化
    /// </summary>
    public const string STRING_FORMAT_SIGN = "+#;-#;0";

    /// <summary>
    /// 版本信息
    /// </summary>
    public static readonly Version VERSION = new Version("0.0.0");


    /// <summary>
    /// 服务器地址
    /// </summary>

#if !OUTER_NET//内网
    #if DEBUG_MODE
        public const string WebUrl = "http://192.168.1.165/";
#else
        public const string WebUrl = "http://test.api.wqmajiang.com/";
#endif
#else//外网
#if IS_SHUANGLIAO//双辽
    public const string WebUrl = "http://api.shuangliaomajiang.com/";
#elif IS_LONGGANG//龙港
    public const string WebUrl = "http://api.cloud-making.com/";
#elif IS_TAILAI//泰来
    public const string WebUrl = "http://api.bihushouyou.com/";
#elif IS_LEPING//乐平
    public const string WebUrl = "http://api.lepingmj.com/";
#elif IS_HONGHU//鸿鹄
    public const string WebUrl = "http://api.honghu.wqmajiang.com/";
#elif IS_WUANJUN//武安郡
    public const string WebUrl = "http://api.wuanjun.wqmajiang.com/";
#elif IS_PAIJIU//牌九
    public const string WebUrl = "http://api.sxxwpj.com/";
#elif IS_LUALU//撸啊撸
    public const string WebUrl = "http://api.tslal.com/";
#elif IS_DAZHONG//大众
    public const string WebUrl = "http://api.majiang0518.com/";
#elif IS_LAOGUI//老鬼
    public const string WebUrl = "http://api.laogui.wqmajiang.com/";
#elif IS_CANGZHOU//沧州
    public const string WebUrl = "http://api.krn3.cn/";
#elif IS_JUYOU//聚友
    public const string WebUrl = "http://api.juyou.wqmajiang.com/";
#elif IS_GONGXIAN//珙县
    public const string WebUrl = "http://api.gx.wqmajiang.com/";
#elif IS_ZHANGJIAKOU//张家口
    public const string WebUrl = "http://api.zjkqp.wqmajiang.com/";
#elif IS_BAODINGQIPAI//保定棋牌
    public const string WebUrl = "";
#elif IS_CHUANTONGPAIJIU
    public const string WebUrl = "http://api.ctpj.wqmajiang.com/";
#elif IS_WANGQUE//网雀
    public const string WebUrl = "http://ipv4.api.wqmajiang.com/";
#elif IS_GUGENG
    public const string WebUrl = "http://api.gugeng.wqmajiang.com/";
#elif IS_BAODING
    public const string WebUrl = "http://api.xingyun.wqmajiang.com/";
#elif IS_LAIJU
	public const string WebUrl = "http://49.4.1.168/";
#else//其他
    public const string WebUrl = "http://apple.wqmajiang.com/";
#endif
#endif

    /// <summary>
    /// 游戏名
    /// </summary>
#if IS_SHUANGLIAO
    public const string GAME_NAME = "shuangliao";
#elif IS_LONGGANG
    public const string GAME_NAME = "longgang";

#elif IS_TAILAI
    public const string GAME_NAME = "tailai";
#elif IS_LEPING
    public const string GAME_NAME = "leping";
#elif IS_HONGHU
    public const string GAME_NAME = "honghu";
#elif IS_LUALU
    public const string GAME_NAME = "lualu";
#elif IS_BAODI
    public const string GAME_NAME = "baodi";
#elif IS_PAIJIU
    public const string GAME_NAME = "paijiu";
#elif IS_WUANJUN
    public const string GAME_NAME = "wuanjun";
#elif IS_LAOGUI
    public const string GAME_NAME = "laogui";
#elif IS_JUYOU
    public const string GAME_NAME = "juyou";
#elif IS_DAZHONG
    public const string GAME_NAME = "dazhong";
#elif IS_WANGPAI
    public const string GAME_NAME = "wangpai";
#elif IS_HAOPAI
    public const string GAME_NAME = "haopai";
#elif IS_GONGXIAN
    public const string GAME_NAME = "gongxian";
#elif IS_ZHANGJIAKOU
    public const string GAME_NAME = "zhangjiakou";
#elif IS_CANGZHOU
    public const string GAME_NAME = "cangzhou";
#elif IS_WANGQUE
    public const string GAME_NAME = "wangque";
#elif IS_BAODING
    public const string GAME_NAME = "baoding";
#elif IS_GUGENG
    public const string GAME_NAME = "gugeng";
#elif IS_BAODINGQIPAI
    public const string GAME_NAME = "baodingqipai";
#elif IS_CHUANTONGPAIJIU
    public const string GAME_NAME = "chuantongpaijiu";
#elif IS_LAIJU
    public const string GAME_NAME = "laiju";
#elif IS_ZHIZUNWANPAI
    public const string GAME_NAME = "zhizunwanpai";
#elif IS_ZHENJIANG
    public const string GAME_NAME = "zhenjiang";
#else
    public const string GAME_NAME = "暂未添加该游戏";
#endif


    public const string SIGN = "mj12321jm";

}
