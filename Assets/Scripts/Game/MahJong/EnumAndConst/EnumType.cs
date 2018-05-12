//===================================================
//Author      : DRB
//CreateTime  ：12/8/2017 10:59:25 AM
//Description ：
//===================================================

namespace DRB.MahJong
{
    #region MahjongGameState 麻将游戏状态
    /// <summary>
    /// 麻将游戏状态
    /// </summary>
    public enum MahjongGameState
    {
        None,
        DrawPoker,
        PlayPoker,
        AskOperate,
        Operate,
        Settle,
    }
    #endregion

    #region OperatorType 操作类型
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperatorType
    {
        None = 0,
        Chi = 1,
        Peng = 2,
        Gang = 3,
        Hu = 4,
        BuHua = 5,
        ZhiDui = 6,
        ChiTing = 7,
        PengTing = 8,
        LiangXi = 9,
        DingZhang = 10,
        FengGang = 11,
        DingJiang = 12,
        Kou = 13,
        BuXi = 14,
        PiaoTing = 15,
        Pao = 16,
        Jiao = 17,
        KouBD = 18,
        MingTi = 19,

        Pass = 64,
        Wan = 65,
        Tong = 66,
        Tiao = 67,
        Ting = 128,
        Cancel = 256,
        Ok = 512,
    }
    #endregion

    #region SubOperateType 操作子类型
    /// <summary>
    /// 操作子类型
    /// </summary>
    public enum SubOperateType
    {
        None,
        DianPao,
        ZiMo,
        MingGang,
        AnGang,
        BuGang
    }
    #endregion

    #region UIAnimationType UI动画类型
    /// <summary>
    /// UI动画类型
    /// </summary>
    public enum UIAnimationType
    {
        UIAnimation_Chi,//吃
        UIAnimation_Peng,//碰
        UIAnimation_Hu,//胡
        UIAnimation_Gang,//杠
        UIAnimation_ZiMo,//自摸
        UIAnimation_LiuJu,//刘局
        UIAnimation_GSKH,//杠上开花
        UIAnimation_YiPaoDuoXiang,//一炮多响
        UIAnimation_WuMa,//无马
        UIAnimation_BuHua,//补花
        UIAnimation_Ting,//听牌
        UIAnimation_ChiTing,//吃听
        UIAnimation_PengTing,//碰听
        UIAnimation_LiangXi,//亮喜
        UIAnimation_ZhiDui,//支对
        UIAnimation_DingZhang,//钉掌
        UIAnimation_PiaoTing,//飘听
        UIAnimation_Kou,//扣
        UIAnimation_DingJiang,//定将
        UIAnimation_BuXi,//补喜
        UIAnimation_Pao,//跑
        UIAnimation_Jiao,//叫
    }
    #endregion
}
