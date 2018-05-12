
//===================================================
//Author：DRB
//CreateTime：2017/7/24 15:09:57
//Remark：This code is generated for the tool
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// cfg_commonMessage数据管理
/// </summary>
public partial class cfg_commonMessageDBModel : AbstractDBModel<cfg_commonMessageDBModel, cfg_commonMessageEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "cfg_commonMessage.drb"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override cfg_commonMessageEntity MakeEntity(GameDataTableParser parse)
    {
        cfg_commonMessageEntity entity = new cfg_commonMessageEntity();
        entity.id = parse.GetFieldValue("id").ToInt();
        entity.gameId = parse.GetFieldValue("gameId").ToInt();
        entity.message = parse.GetFieldValue("message");
        entity.AudioName = parse.GetFieldValue("AudioName");
        return entity;
    }
}
