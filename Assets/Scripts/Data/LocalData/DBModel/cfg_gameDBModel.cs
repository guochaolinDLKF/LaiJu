
//===================================================
//Author：DRB
//CreateTime：2017/10/12 11:11:12
//Remark：This code is generated for the tool
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// cfg_game数据管理
/// </summary>
public partial class cfg_gameDBModel : AbstractDBModel<cfg_gameDBModel, cfg_gameEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "cfg_game.drb"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override cfg_gameEntity MakeEntity(GameDataTableParser parse)
    {
        cfg_gameEntity entity = new cfg_gameEntity();
        entity.id = parse.GetFieldValue("id").ToInt();
        entity.GameName = parse.GetFieldValue("GameName");
        entity.GameType = parse.GetFieldValue("GameType");
        entity.SceneName = parse.GetFieldValue("SceneName");
        entity.BGM = parse.GetFieldValue("BGM");
        entity.Icon = parse.GetFieldValue("Icon");
        entity.Payment = parse.GetFieldValue("Payment").ToInt();
        return entity;
    }
}
