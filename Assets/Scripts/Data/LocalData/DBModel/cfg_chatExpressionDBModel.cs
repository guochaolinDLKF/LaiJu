
//===================================================
//Author：DRB
//CreateTime：2017/8/11 10:47:29
//Remark：This code is generated for the tool
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// cfg_chatExpression数据管理
/// </summary>
public partial class cfg_chatExpressionDBModel : AbstractDBModel<cfg_chatExpressionDBModel, cfg_chatExpressionEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "cfg_chatExpression.drb"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override cfg_chatExpressionEntity MakeEntity(GameDataTableParser parse)
    {
        cfg_chatExpressionEntity entity = new cfg_chatExpressionEntity();
        entity.id = parse.GetFieldValue("id").ToInt();
        entity.code = parse.GetFieldValue("code");
        entity.image = parse.GetFieldValue("image");
        entity.sound = parse.GetFieldValue("sound");
        return entity;
    }
}
