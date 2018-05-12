
//===================================================
//Author：DRB
//CreateTime：2017/10/11 13:53:40
//Remark：This code is generated for the tool
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// cfg_rule数据管理
/// </summary>
public partial class cfg_ruleDBModel : AbstractDBModel<cfg_ruleDBModel, cfg_ruleEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "cfg_rule.drb"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override cfg_ruleEntity MakeEntity(GameDataTableParser parse)
    {
        cfg_ruleEntity entity = new cfg_ruleEntity();
        entity.id = parse.GetFieldValue("id").ToInt();
        entity.GameId = parse.GetFieldValue("GameId").ToInt();
        entity.IsHongZhongFly = parse.GetFieldValue("IsHongZhongFly").ToBool();
        entity.IsBigWind = parse.GetFieldValue("IsBigWind").ToBool();
        entity.IsNotJiaCantHu = parse.GetFieldValue("IsNotJiaCantHu").ToBool();
        entity.Is37Jia = parse.GetFieldValue("Is37Jia").ToBool();
        entity.IsSingleJia = parse.GetFieldValue("IsSingleJia").ToBool();
        entity.IsDoubleDoubleJia = parse.GetFieldValue("IsDoubleDoubleJia").ToBool();
        entity.IsZhiDui = parse.GetFieldValue("IsZhiDui").ToBool();
        entity.IsXiReplaceSameTriple = parse.GetFieldValue("IsXiReplaceSameTriple").ToBool();
        entity.IsHardYao = parse.GetFieldValue("IsHardYao").ToBool();
        entity.IsSoftYao = parse.GetFieldValue("IsSoftYao").ToBool();
        entity.IsHongZhongReplace19 = parse.GetFieldValue("IsHongZhongReplace19").ToBool();
        entity.HongZhongIsSameTriple = parse.GetFieldValue("HongZhongIsSameTriple").ToBool();
        entity.IsNotUsedCantHu = parse.GetFieldValue("IsNotUsedCantHu").ToBool();
        entity.IsAnGangNotUsedPoker = parse.GetFieldValue("IsAnGangNotUsedPoker").ToBool();
        entity.IsHandOnlyOneCantHu = parse.GetFieldValue("IsHandOnlyOneCantHu").ToBool();
        entity.IsMustHasSameTriple = parse.GetFieldValue("IsMustHasSameTriple").ToBool();
        entity.IsMustHasStrightTriple = parse.GetFieldValue("IsMustHasStrightTriple").ToBool();
        entity.IsMustHas19 = parse.GetFieldValue("IsMustHas19").ToBool();
        entity.IsMustAllColor = parse.GetFieldValue("IsMustAllColor").ToBool();
        entity.IsCantSingleColor = parse.GetFieldValue("IsCantSingleColor").ToBool();
        entity.IsNotTingCantHu = parse.GetFieldValue("IsNotTingCantHu").ToBool();
        entity.IsSevenDoubleCantHu = parse.GetFieldValue("IsSevenDoubleCantHu").ToBool();
        entity.IsZhongFaBaiReplace19LiangXi = parse.GetFieldValue("IsZhongFaBaiReplace19LiangXi").ToBool();
        entity.IsZhongFaBaiLiangXi = parse.GetFieldValue("IsZhongFaBaiLiangXi").ToBool();
        entity.IsFengLiangXi = parse.GetFieldValue("IsFengLiangXi").ToBool();
        entity.IsFengCanChi = parse.GetFieldValue("IsFengCanChi").ToBool();
        entity.IsUniversalCanChi = parse.GetFieldValue("IsUniversalCanChi").ToBool();
        entity.IsCanPiaoHu = parse.GetFieldValue("IsCanPiaoHu").ToBool();
        entity.IsLiangXiCanHu = parse.GetFieldValue("IsLiangXiCanHu").ToBool();
        return entity;
    }
}
