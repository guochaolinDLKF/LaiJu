
//===================================================
//Author：DRB
//CreateTime：2017/9/11 10:45:26
//Remark：This code is generated for the tool
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// cfg_pay数据管理
/// </summary>
public partial class cfg_payDBModel : AbstractDBModel<cfg_payDBModel, cfg_payEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "cfg_pay.drb"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override cfg_payEntity MakeEntity(GameDataTableParser parse)
    {
        cfg_payEntity entity = new cfg_payEntity();
        entity.id = parse.GetFieldValue("id").ToInt();
        entity.name = parse.GetFieldValue("name");
        entity.iosCode = parse.GetFieldValue("iosCode");
        entity.money = parse.GetFieldValue("money").ToInt();
        entity.amount = parse.GetFieldValue("amount").ToInt();
        entity.give = parse.GetFieldValue("give").ToInt();
        entity.isHot = parse.GetFieldValue("isHot").ToBool();
        entity.icon = parse.GetFieldValue("icon");
        return entity;
    }
}
