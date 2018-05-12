//===================================================
//Author      : DRB
//CreateTime  ：6/23/2017 10:54:39 AM
//Description ：
//===================================================
using UnityEngine;

/// <summary>
/// 房卡支付方式
/// </summary>
public enum CardPaymentType
{
    HouseOwner = 1,
    AA = 2,
}

/// <summary>
/// 癞子排序方式
/// </summary>
public enum UniversalSortType
{
    Normal,
    Left,
    Best,
}


public class MahjongRule 
{
    /// <summary>
    /// 是否包含红中满天飞玩法
    /// </summary>
    public bool IsHongZhongFly;
    /// <summary>
    /// 是否包含刮大风玩法
    /// </summary>
    public bool IsBigWind;
    /// <summary>
    /// 是否不夹不能胡
    /// </summary>
    public bool IsNotJiaCantHu;
    /// <summary>
    /// 3、7算夹
    /// </summary>
    public bool Is37Jia;
    /// <summary>
    /// 单调算夹
    /// </summary>
    public bool IsSingleJia;
    /// <summary>
    /// 对倒算夹
    /// </summary>
    public bool IsDoubleDoubleJia;
    /// <summary>
    /// 是否可以支对
    /// </summary>
    public bool IsZhiDui;
    /// <summary>
    /// 喜代替刻
    /// </summary>
    public bool IsXiReplaceSameTriple;
    /// <summary>
    /// 是否硬幺
    /// </summary>
    public bool IsHardYao;
    /// <summary>
    /// 是否软幺
    /// </summary>
    public bool IsSoftYao;
    /// <summary>
    /// 红中是否可以代替19
    /// </summary>
    public bool IsHongZhongReplace19;
    /// <summary>
    /// 红中是否可以代替刻
    /// </summary>
    public bool HongZhongIsSameTriple;
    /// <summary>
    /// 门清不能胡
    /// </summary>
    public bool IsNotUsedCantHu;
    /// <summary>
    /// 暗杠不算开门
    /// </summary>
    public bool IsAnGangNotUsedPoker;
    /// <summary>
    /// 手把一是否能胡
    /// </summary>
    public bool IsHandOnlyOneCantHu;
    /// <summary>
    /// 胡牌是否必须有刻
    /// </summary>
    public bool IsMustHasSameTriple;
    /// <summary>
    /// 胡牌是否必须有顺子
    /// </summary>
    public bool IsMustHasStrightTriple;
    /// <summary>
    /// 胡牌是否必须有19
    /// </summary>
    public bool IsMustHas19;
    /// <summary>
    /// 胡牌是否必须包含所有花色
    /// </summary>
    public bool IsMustAllColor;
    /// <summary>
    /// 胡牌是否不能清一色
    /// </summary>
    public bool IsCantSingleColor;
    /// <summary>
    /// 不听不能胡
    /// </summary>
    public bool IsNotTingCantHu;
    /// <summary>
    /// 七对不能胡
    /// </summary>
    public bool IsSevenDoubleCantHu;
    /// <summary>
    /// 中发白代替19亮喜
    /// </summary>
    public bool IsZhongFaBaiReplace19LiangXi;
    /// <summary>
    /// 一套中发白亮喜
    /// </summary>
    public bool IsZhongFaBaiLiangXi;
    /// <summary>
    /// 东南西北亮喜
    /// </summary>
    public bool IsFengLiangXi;
    /// <summary>
    /// 风可以吃
    /// </summary>
    public bool IsFengCanChi;
    /// <summary>
    /// 万能牌可以吃
    /// </summary>
    public bool IsUniversalCanChi;
    /// <summary>
    /// 飘胡
    /// </summary>
    public bool IsCanPiaoHu;
    /// <summary>
    /// 亮喜牌型可以胡
    /// </summary>
    public bool IsLiangXiCanHu;
    /// <summary>
    /// 打癞子才能胡
    /// </summary>
    public bool isPlayedUniversalCanHu;

    /// <summary>
    /// 可以听牌
    /// </summary>
    public bool isTing;
    /// <summary>
    /// 十三幺可以胡
    /// </summary>
    public bool is13YaoCanHu;
    /// <summary>
    /// 旋风杠
    /// </summary>
    public bool isXuanFengGang;





    /// <summary>
    /// 显示墙
    /// </summary>
    public bool isDisplayWall = true;
    /// <summary>
    /// 癞子排序方式
    /// </summary>
    public UniversalSortType UniversalSortType = UniversalSortType.Best;

    public MahjongRule() { }

    public MahjongRule(cfg_ruleEntity entity)
    {
        IsHongZhongFly = entity.IsHongZhongFly;
        IsBigWind = entity.IsBigWind;
        IsNotJiaCantHu = entity.IsNotJiaCantHu;
        Is37Jia = entity.Is37Jia;
        IsSingleJia = entity.IsSingleJia;
        IsDoubleDoubleJia = entity.IsDoubleDoubleJia;
        IsZhiDui = entity.IsZhiDui;
        IsXiReplaceSameTriple = entity.IsXiReplaceSameTriple;
        IsHardYao = entity.IsHardYao;
        IsSoftYao = entity.IsSoftYao;
        IsHongZhongReplace19 = entity.IsHongZhongReplace19;
        HongZhongIsSameTriple = entity.HongZhongIsSameTriple;
        IsNotUsedCantHu = entity.IsNotUsedCantHu;
        IsAnGangNotUsedPoker = entity.IsAnGangNotUsedPoker;
        IsHandOnlyOneCantHu = entity.IsHandOnlyOneCantHu;
        IsMustHasSameTriple = entity.IsMustHasSameTriple;
        IsMustHasStrightTriple = entity.IsMustHasStrightTriple;
        IsMustHas19 = entity.IsMustHas19;
        IsMustAllColor = entity.IsMustAllColor;
        IsCantSingleColor = entity.IsCantSingleColor;
        IsNotTingCantHu = entity.IsNotTingCantHu;
        IsSevenDoubleCantHu = entity.IsSevenDoubleCantHu;
        IsZhongFaBaiReplace19LiangXi = entity.IsZhongFaBaiReplace19LiangXi;
        IsZhongFaBaiLiangXi = entity.IsZhongFaBaiLiangXi;
        IsFengLiangXi = entity.IsFengLiangXi;
        IsFengCanChi = entity.IsFengCanChi;
        IsUniversalCanChi = entity.IsUniversalCanChi;
        IsCanPiaoHu = entity.IsCanPiaoHu;
        IsLiangXiCanHu = entity.IsLiangXiCanHu;
    }
}
