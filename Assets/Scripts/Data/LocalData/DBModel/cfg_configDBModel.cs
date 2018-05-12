
//===================================================
//Author：DRB
//CreateTime：2017/7/17 13:27:13
//Remark：This code is generated for the tool
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// cfg_config数据管理
/// </summary>
public partial class cfg_configDBModel : AbstractDBModel<cfg_configDBModel, cfg_configEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "cfg_config.drb"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override cfg_configEntity MakeEntity(GameDataTableParser parse)
    {
        cfg_configEntity entity = new cfg_configEntity();
        entity.id = parse.GetFieldValue("id").ToInt();
        entity.name = parse.GetFieldValue("name");
        entity.tags = parse.GetFieldValue("tags");
        entity.times = parse.GetFieldValue("times").ToInt();
        entity.figure = parse.GetFieldValue("figure").ToInt();
        entity.mode = parse.GetFieldValue("mode").ToInt();
        entity.isDiff = parse.GetFieldValue("isDiff").ToInt();
        entity.must = parse.GetFieldValue("must");
        entity.mustIsSameColor = parse.GetFieldValue("mustIsSameColor").ToInt();
        entity.exDnxb = parse.GetFieldValue("exDnxb").ToInt();
        entity.exZfb = parse.GetFieldValue("exZfb").ToInt();
        entity.MinTripleCount = parse.GetFieldValue("MinTripleCount").ToInt();
        entity.MinDoubleCount = parse.GetFieldValue("MinDoubleCount").ToInt();
        entity.MinTripletCount = parse.GetFieldValue("MinTripletCount").ToInt();
        entity.MinQuadrupleCount = parse.GetFieldValue("MinQuadrupleCount").ToInt();
        entity.allIsSameColor = parse.GetFieldValue("allIsSameColor").ToInt();
        entity.CheckUsePoker = parse.GetFieldValue("CheckUsePoker").ToInt();
        entity.MinSpriteCount = parse.GetFieldValue("MinSpriteCount").ToInt();
        entity.MaxSpriteCount = parse.GetFieldValue("MaxSpriteCount").ToInt();
        entity.isEnd = parse.GetFieldValue("isEnd").ToInt();
        entity.effect2D = parse.GetFieldValue("effect2D");
        entity.effect3D = parse.GetFieldValue("effect3D");
        entity.effectStartPos = parse.GetFieldValue("effectStartPos");
        entity.effectEndPos = parse.GetFieldValue("effectEndPos");
        entity.effectDuration = parse.GetFieldValue("effectDuration").ToFloat();
        entity.soundEffect = parse.GetFieldValue("soundEffect");
        return entity;
    }
}
