
//===================================================
//Author：DRB
//CreateTime：2017/7/17 13:27:21
//Remark：This code is generated for the tool
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// cfg_share数据管理
/// </summary>
public partial class cfg_shareDBModel : AbstractDBModel<cfg_shareDBModel, cfg_shareEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "cfg_share.drb"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override cfg_shareEntity MakeEntity(GameDataTableParser parse)
    {
        cfg_shareEntity entity = new cfg_shareEntity();
        entity.id = parse.GetFieldValue("id").ToInt();
        entity.type = parse.GetFieldValue("type").ToInt();
        entity.title = parse.GetFieldValue("title");
        entity.content = parse.GetFieldValue("content");
        entity.url = parse.GetFieldValue("url");
        entity.isReward = parse.GetFieldValue("isReward").ToInt();
        return entity;
    }
}
