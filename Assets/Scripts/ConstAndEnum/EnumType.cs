#region GameType 游戏类型
/// <summary>
/// 游戏类型
/// </summary>
public enum GameType
{
    /// <summary>
    /// 麻将
    /// </summary>
    Majiang,
    /// <summary>
    /// 牛牛
    /// </summary>
    Niuniu,
    /// <summary>
    /// 扎金花
    /// </summary>
    Zhajinhua,
    /// <summary>
    /// 骨牌
    /// </summary>
    Gupai,
    /// <summary>
    /// 牌九
    /// </summary>
    Paijiu,
    /// <summary>
    /// 斗地主
    /// </summary>
    Doudizhu,
    /// <summary>
    /// 挤旮旯
    /// </summary>
    Jigala,
    /// <summary>
    /// 跑得快
    /// </summary>
    PaoDeKuai,
    /// <summary>
    /// 掼蛋
    /// </summary>
    GuanDan,
    /// <summary>
    /// 十三张
    /// </summary>
    ShiSanShui,
    /// <summary>
    /// 棋牌游戏
    /// </summary>
    QiPai = 64,
}
#endregion

#region MessageViewType 消息窗口类型
/// <summary>
/// 消息窗口类型
/// </summary>
public enum MessageViewType
{
    None,
    Ok,
    OkAndCancel,
}
#endregion

#region UIWindowType 窗口UI类型
/// <summary>
/// 窗口UI类型
/// </summary>
public enum UIWindowType
{
    None,
    Login,//登陆
    JoinRoom,//加入房间
    CreateRoom,//创建房间
    Rule,//规则
    Share,//分享
    Shop,//商城
    Payment,//支付方式
    Setting,//设置
    MainMenu,//主界面菜单
    Match,//比赛
    Service,//客服
    Record,//战绩
    OwnerRecord,//个人战绩
    Invite,//邀请码
    Settle,//结算
    Chat,//聊天
    ChatGroup,//群友会
    ChatRoomDetail,//群友会房间详情
    ChatGroupApply,//群友会申请列表
    ChatGroupCards,//群友会设置房卡
    Present,//赠送
    Micro,//录音
    Notice,//公告
    Ranking,//排行榜
    SeatInfo,//座位信息
    AudioSetting,//音乐设置
    Result,//牌局结算
    MatchDetail,//比赛详情
    MatchTip,//比赛提示
    MatchWait,//比赛等待
    MatchRankList,//比赛排行榜
    PlayerInfo,//角色信息
    Bind,//绑定手机
    Mail,//邮件
    AgreeMent,//用户协议
    UserAgreement,//用户使用协议
    MyRoom,//我的房间
    Disband,//解散房间
    WelfareActivities,//福利活动（大转盘，开宝箱）
    Integral,//积分
    RealName,//实名制
    AgentService,//代理微信


    RankList_NiuNiu,  //牛牛结算排行榜
    UnitSettlement_NiuNiu, //牛牛每小局结算
    ADH_NiuNiu,//牛牛计数同意解散人数
    Rule_NiuNiu,//牛牛规则
    RubPoker_NiuNiu,//牛牛搓牌

    UIZJHWait,//炸金花等待进入房间
    UIZJHRecord,//炸金花离开房间的时候小结算
    UIZJHTotalSettlement,//炸金花普通房游戏结束总结算
    ADH_ZJH,//炸金花解散房间显示

    UnitSettlement_PaiJiu,//牌九每局小结算
    RankList_PaiJiu,//牌九总结算
    Retroaction,//牌九反馈
    GuPaiJiuResult,//骨牌总结算
    GuPaiJiuSmallResult,//骨牌小结算

    Settle_PaoDeKuai,//跑得快小结算
    Result_PaoDeKuai,//跑得快总结算
    JiPaiQi,//记牌器
}
#endregion

#region ResourceType 资源类型
/// <summary>
/// 资源类型
/// </summary>
public enum ResourceType
{
    UIScene,
    UIWindow,
    Prefab,
    UIItem,
    Sound,
    UIAnimation
}
#endregion

#region LanguageType 语言类型
/// <summary>
/// 语言类型
/// </summary>
public enum LanguageType
{
    /// <summary>
    /// 普通话
    /// </summary>
    Mandarin,
    /// <summary>
    /// 福鼎方言
    /// </summary>
    FuDing,
}
#endregion

#region ContainerType 容器类型
/// <summary>
/// 容器类型
/// </summary>
public enum ContainerType
{
    Center,
    TopRight,
    Left,
}
#endregion

#region UIWindowShowStyleType 窗口显示风格类型
/// <summary>
/// 窗口显示风格类型
/// </summary>
public enum UIWindowShowStyleType
{
    Normal,
    CenterToBig,
    FromTop,
    FromDown,
    FromLeft,
    FromRight
}
#endregion

#region UIWindowPersistenceType 窗口持久化类型
/// <summary>
/// 窗口持久化类型
/// </summary>
public enum UIWindowPersistenceType
{
    /// <summary>
    /// 一次，关闭即销毁
    /// </summary>
    Once = 0,
    /// <summary>
    /// 切换场景时销毁
    /// </summary>
    LoadSceneDestroy = 1,
    /// <summary>
    /// 切换场景时隐藏
    /// </summary>
    LoadSceneHide = 2,
    /// <summary>
    /// 常驻
    /// </summary>
    LongStay = 3,
}
#endregion

#region UIScenePersistenceType 场景UI持久化类型
/// <summary>
/// 场景UI持久化类型
/// </summary>
public enum UIScenePersistenceType
{
    /// <summary>
    /// 加载场景销毁
    /// </summary>
    LoadSceneDestroy,
    /// <summary>
    /// 加载场景隐藏
    /// </summary>
    LoadSceneHide,
}
#endregion

#region SceneType 场景类型
/// <summary>
/// 场景类型
/// </summary>
public enum SceneType
{
    None,
    Login,
    Loading,
    Init,
    Main,
    MaJiang3D,
    NiuNiu2D,
    DLandlord,
    ZhaJH,
    PaiJiu3D,
    DouDZ,
    JuYou3D,
    GuPaiJiu,
    GuanDan2D,
    PaoDeKuai2D,
    ShiSanZhang,
}
#endregion

#region SpriteType 图片类型
/// <summary>
/// 图片类型
/// </summary>
public enum SpriteType
{
    MaJiang,
    Dice,
    Direction,
    Game,
    Chat,
    Room,
}
#endregion

#region MatchStatus 比赛状态
/// <summary>
/// 比赛状态
/// </summary>
public enum MatchStatus
{
    /// <summary>
    /// 没有比赛
    /// </summary>
    None,
    /// <summary>
    /// 报名
    /// </summary>
    Apply,
    /// <summary>
    /// 进行中
    /// </summary>
    Begin,
    /// <summary>
    /// 结算
    /// </summary>
    Settle,
}
#endregion

#region ChatType 聊天类型
/// <summary>
/// 聊天类型
/// </summary>
public enum ChatType
{
    /// <summary>
    /// 文字消息
    /// </summary>
    Message,
    /// <summary>
    /// 表情
    /// </summary>
    Expression,
    /// <summary>
    /// 互动表情
    /// </summary>
    InteractiveExpression,
    /// <summary>
    /// 语音
    /// </summary>
    MicroPhone,
}
#endregion

#region AudioType 声音类型
/// <summary>
/// 声音类型
/// </summary>
public enum AudioType
{
    /// <summary>
    /// 背景音乐
    /// </summary>
    BGM,
    /// <summary>
    /// 音效
    /// </summary>
    SoundEffect,
    /// <summary>
    /// 语音
    /// </summary>
    Micro,
}
#endregion

#region AILevel AI级别
/// <summary>
/// AI级别
/// </summary>
public enum AILevel
{
    /// <summary>
    /// 稍微有点脑子的
    /// </summary>
    Normal,
    /// <summary>
    /// 没有脑子的
    /// </summary>
    Silly,
    /// <summary>
    /// 聪明的
    /// </summary>
    Clever,
    /// <summary>
    /// 最优的
    /// </summary>
    //Best,
}
#endregion