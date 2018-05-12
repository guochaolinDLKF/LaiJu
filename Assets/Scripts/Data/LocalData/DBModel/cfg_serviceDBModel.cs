
//===================================================
//Author：DRB
//CreateTime：2017/7/17 13:27:18
//Remark：This code is generated for the tool
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// cfg_service数据管理
/// </summary>
public partial class cfg_serviceDBModel : AbstractDBModel<cfg_serviceDBModel, cfg_serviceEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "cfg_service.drb"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override cfg_serviceEntity MakeEntity(GameDataTableParser parse)
    {
        cfg_serviceEntity entity = new cfg_serviceEntity();
        entity.id = parse.GetFieldValue("id").ToInt();
        entity.key = parse.GetFieldValue("key");
        entity.value = parse.GetFieldValue("value");
        return entity;
    }
}
