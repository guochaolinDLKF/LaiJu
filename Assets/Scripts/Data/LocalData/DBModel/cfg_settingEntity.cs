
//===================================================
//Author：DRB
//CreateTime：2017/7/17 13:27:20
//Remark：This code is generated for the tool
//===================================================
using UnityEngine;
using System.Collections;

/// <summary>
/// cfg_setting实体
/// </summary>
public partial class cfg_settingEntity : AbstractEntity
{
    /// <summary>
    /// 组名
    /// </summary>
    public string gameName { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public int gameId { get; set; }

    /// <summary>
    /// 组名
    /// </summary>
    public string label { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// 标记
    /// </summary>
    public string tags { get; set; }

    /// <summary>
    /// 选择模式
    /// </summary>
    public int mode { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    public int value { get; set; }

    /// <summary>
    /// 消耗
    /// </summary>
    public int cost { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public int status { get; set; }

    /// <summary>
    /// 默认值
    /// </summary>
    public int init { get; set; }

    /// <summary>
    /// 选中事件
    /// </summary>
    public string selected { get; set; }

    /// <summary>
    /// 未选中事件
    /// </summary>
    public string unselect { get; set; }

}
