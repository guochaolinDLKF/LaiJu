
//===================================================
//Author：DRB
//CreateTime：2017/10/11 13:53:40
//Remark：This code is generated for the tool
//===================================================
using UnityEngine;
using System.Collections;

/// <summary>
/// cfg_rule实体
/// </summary>
public partial class cfg_ruleEntity : AbstractEntity
{
    /// <summary>
    /// 游戏Id
    /// </summary>
    public int GameId { get; set; }

    /// <summary>
    /// 包含红中满天飞玩法
    /// </summary>
    public bool IsHongZhongFly { get; set; }

    /// <summary>
    /// 包含刮大风玩法
    /// </summary>
    public bool IsBigWind { get; set; }

    /// <summary>
    /// 不夹不能胡
    /// </summary>
    public bool IsNotJiaCantHu { get; set; }

    /// <summary>
    /// 3、7算夹
    /// </summary>
    public bool Is37Jia { get; set; }

    /// <summary>
    /// 单调算夹
    /// </summary>
    public bool IsSingleJia { get; set; }

    /// <summary>
    /// 对倒算夹
    /// </summary>
    public bool IsDoubleDoubleJia { get; set; }

    /// <summary>
    /// 可以支对
    /// </summary>
    public bool IsZhiDui { get; set; }

    /// <summary>
    /// 喜代替刻
    /// </summary>
    public bool IsXiReplaceSameTriple { get; set; }

    /// <summary>
    /// 硬幺
    /// </summary>
    public bool IsHardYao { get; set; }

    /// <summary>
    /// 软幺
    /// </summary>
    public bool IsSoftYao { get; set; }

    /// <summary>
    /// 红中可以代替19
    /// </summary>
    public bool IsHongZhongReplace19 { get; set; }

    /// <summary>
    /// 红中可以代替刻
    /// </summary>
    public bool HongZhongIsSameTriple { get; set; }

    /// <summary>
    /// 门清不能胡
    /// </summary>
    public bool IsNotUsedCantHu { get; set; }

    /// <summary>
    /// 暗杠不算开门
    /// </summary>
    public bool IsAnGangNotUsedPoker { get; set; }

    /// <summary>
    /// 手把一能胡
    /// </summary>
    public bool IsHandOnlyOneCantHu { get; set; }

    /// <summary>
    /// 胡牌必须有刻
    /// </summary>
    public bool IsMustHasSameTriple { get; set; }

    /// <summary>
    /// 胡牌必须有顺子
    /// </summary>
    public bool IsMustHasStrightTriple { get; set; }

    /// <summary>
    /// 胡牌必须有19
    /// </summary>
    public bool IsMustHas19 { get; set; }

    /// <summary>
    /// 胡牌必须包含所有花色
    /// </summary>
    public bool IsMustAllColor { get; set; }

    /// <summary>
    /// 胡牌不能清一色
    /// </summary>
    public bool IsCantSingleColor { get; set; }

    /// <summary>
    /// 不听不能胡
    /// </summary>
    public bool IsNotTingCantHu { get; set; }

    /// <summary>
    /// 七对不能胡
    /// </summary>
    public bool IsSevenDoubleCantHu { get; set; }

    /// <summary>
    /// 中发白代替19亮喜
    /// </summary>
    public bool IsZhongFaBaiReplace19LiangXi { get; set; }

    /// <summary>
    /// 一套中发白亮喜
    /// </summary>
    public bool IsZhongFaBaiLiangXi { get; set; }

    /// <summary>
    /// 东南西北亮喜
    /// </summary>
    public bool IsFengLiangXi { get; set; }

    /// <summary>
    /// 风可以吃
    /// </summary>
    public bool IsFengCanChi { get; set; }

    /// <summary>
    /// 万能牌可以吃
    /// </summary>
    public bool IsUniversalCanChi { get; set; }

    /// <summary>
    /// 飘胡
    /// </summary>
    public bool IsCanPiaoHu { get; set; }

    /// <summary>
    /// 亮喜牌型可以胡
    /// </summary>
    public bool IsLiangXiCanHu { get; set; }

}
