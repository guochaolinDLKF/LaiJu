//===================================================
//Author      : DRB
//CreateTime  ：7/4/2016 1:23:24 AM
//Description ：UI层级管理器
//===================================================

using UnityEngine;
using System.Collections;

/// <summary>
/// UI层级管理器
/// </summary>
public class LayerUIManager : Singleton<LayerUIManager> 
{

    /// <summary>
    /// 默认层级
    /// </summary>
    private const int DEFAULT_LAYER = 50;

    /// <summary>
    /// UIPanel层级深度
    /// </summary>
    private int m_UIViewLayer = DEFAULT_LAYER;

    #region Public Function
    /// <summary>
    /// 重置
    /// </summary>
    public void Reset()
    {
        m_UIViewLayer = DEFAULT_LAYER;
    }

    /// <summary>
    /// 检查窗口数量 如果没有打开的窗口 重置
    /// </summary>
    public void CheckOpenWindow()
    {
        --m_UIViewLayer;
        if (UIViewUtil.Instance.OpenWindowCount == 0)
        {
            Reset();
        }
    }

    /// <summary>
    /// 设置层级
    /// </summary>
    public void SetLayer(GameObject obj)
    {
        ++m_UIViewLayer;
        Canvas m_Canvas = obj.GetComponent<Canvas>();
        m_Canvas.overrideSorting = true;
        m_Canvas.sortingOrder = m_UIViewLayer;
    }
    #endregion
}