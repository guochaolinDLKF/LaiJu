//===================================================
//Author      : DRB
//CreateTime  ：8/22/2017 2:56:12 PM
//Description ：
//===================================================

//===================================================
//Author：DRB
//CreateTime：2017/8/22 14:55:40
//Remark：This code is generated for the tool
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// cfg_interactiveExpression数据管理
/// </summary>
public partial class cfg_interactiveExpressionDBModel : AbstractDBModel<cfg_interactiveExpressionDBModel, cfg_interactiveExpressionEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "cfg_interactiveExpression.drb"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override cfg_interactiveExpressionEntity MakeEntity(GameDataTableParser parse)
    {
        cfg_interactiveExpressionEntity entity = new cfg_interactiveExpressionEntity();
        entity.id = parse.GetFieldValue("id").ToInt();
        entity.code = parse.GetFieldValue("code");
        entity.image = parse.GetFieldValue("image");
        entity.sound = parse.GetFieldValue("sound");
        entity.animation = parse.GetFieldValue("animation");
        return entity;
    }
}
