
//===================================================
//Author：DRB
//CreateTime：2017/7/17 13:27:13
//Remark：This code is generated for the tool
//===================================================
using UnityEngine;
using System.Collections;

/// <summary>
/// cfg_config实体
/// </summary>
public partial class cfg_configEntity : AbstractEntity
{
    /// <summary>
    /// 名称
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// 标记
    /// </summary>
    public string tags { get; set; }

    /// <summary>
    /// 几番
    /// </summary>
    public int times { get; set; }

    /// <summary>
    /// 计算方式
    /// </summary>
    public int figure { get; set; }

    /// <summary>
    /// 输赢模式
    /// </summary>
    public int mode { get; set; }

    /// <summary>
    /// 是否匹配
    /// </summary>
    public int isDiff { get; set; }

    /// <summary>
    /// 必须包含的牌(;分割牌,|表示或者,_分割花色和大小)
    /// </summary>
    public string must { get; set; }

    /// <summary>
    /// 包含的牌是否花色相同
    /// </summary>
    public int mustIsSameColor { get; set; }

    /// <summary>
    /// 剔除东南西北
    /// </summary>
    public int exDnxb { get; set; }

    /// <summary>
    /// 剔除中发白
    /// </summary>
    public int exZfb { get; set; }

    /// <summary>
    /// 最小刻
    /// </summary>
    public int MinTripleCount { get; set; }

    /// <summary>
    /// 最小对
    /// </summary>
    public int MinDoubleCount { get; set; }

    /// <summary>
    /// 最小顺
    /// </summary>
    public int MinTripletCount { get; set; }

    /// <summary>
    /// 最小杠
    /// </summary>
    public int MinQuadrupleCount { get; set; }

    /// <summary>
    /// 全部花色
    /// </summary>
    public int allIsSameColor { get; set; }

    /// <summary>
    /// 包含吃碰杠牌
    /// </summary>
    public int CheckUsePoker { get; set; }

    /// <summary>
    /// 最小癞
    /// </summary>
    public int MinSpriteCount { get; set; }

    /// <summary>
    /// 最大癞
    /// </summary>
    public int MaxSpriteCount { get; set; }

    /// <summary>
    /// 摸牌来源
    /// </summary>
    public int isEnd { get; set; }

    /// <summary>
    /// 2D特效
    /// </summary>
    public string effect2D { get; set; }

    /// <summary>
    /// 3D特效
    /// </summary>
    public string effect3D { get; set; }

    /// <summary>
    /// 特效起始位置
    /// </summary>
    public string effectStartPos { get; set; }

    /// <summary>
    /// 特效目标位置
    /// </summary>
    public string effectEndPos { get; set; }

    /// <summary>
    /// 特效持续时间
    /// </summary>
    public float effectDuration { get; set; }

    /// <summary>
    /// 音效
    /// </summary>
    public string soundEffect { get; set; }

}
