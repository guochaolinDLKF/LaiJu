
//===================================================
//Author：DRB
//CreateTime：2017/9/11 10:45:26
//Remark：This code is generated for the tool
//===================================================
using UnityEngine;
using System.Collections;

/// <summary>
/// cfg_pay实体
/// </summary>
public partial class cfg_payEntity : AbstractEntity
{
    /// <summary>
    /// 名称
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// 计费代码
    /// </summary>
    public string iosCode { get; set; }

    /// <summary>
    /// RMB
    /// </summary>
    public int money { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public int amount { get; set; }

    /// <summary>
    /// 赠送
    /// </summary>
    public int give { get; set; }

    /// <summary>
    /// 是否热销
    /// </summary>
    public bool isHot { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string icon { get; set; }

}
