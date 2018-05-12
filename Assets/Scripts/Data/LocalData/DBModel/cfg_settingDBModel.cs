
//===================================================
//Author：DRB
//CreateTime：2017/7/17 13:27:20
//Remark：This code is generated for the tool
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// cfg_setting数据管理
/// </summary>
public partial class cfg_settingDBModel : AbstractDBModel<cfg_settingDBModel, cfg_settingEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "cfg_setting.drb"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override cfg_settingEntity MakeEntity(GameDataTableParser parse)
    {
        cfg_settingEntity entity = new cfg_settingEntity();
        entity.id = parse.GetFieldValue("id").ToInt();
        entity.gameName = parse.GetFieldValue("gameName");
        entity.gameId = parse.GetFieldValue("gameId").ToInt();
        entity.label = parse.GetFieldValue("label");
        entity.name = parse.GetFieldValue("name");
        entity.tags = parse.GetFieldValue("tags");
        entity.mode = parse.GetFieldValue("mode").ToInt();
        entity.value = parse.GetFieldValue("value").ToInt();
        entity.cost = parse.GetFieldValue("cost").ToInt();
        entity.status = parse.GetFieldValue("status").ToInt();
        entity.init = parse.GetFieldValue("init").ToInt();
        entity.selected = parse.GetFieldValue("selected");
        entity.unselect = parse.GetFieldValue("unselect");
        return entity;
    }
}
