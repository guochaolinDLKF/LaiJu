//===================================================
//Author      : DRB
//CreateTime  ：10/18/2017 10:19:16 AM
//Description ：滚动视图扩展
//===================================================
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 滚动视图扩展
/// </summary>
[RequireComponent(typeof(ScrollRect))]
[RequireComponent(typeof(RectTransform))]
public class ScrollViewExt : MonoBehaviour 
{
    #region Enum
    /// <summary>
    /// 滚动视图状态
    /// </summary>
    public enum ScrollState
    {
        /// <summary>
        /// 最上
        /// </summary>
        Top,
        /// <summary>
        /// 之间
        /// </summary>
        Between,
        /// <summary>
        /// 最下
        /// </summary>
        Down,
        /// <summary>
        /// 固定
        /// </summary>
        Fixed,
    }
    #endregion

    #region Delegate
    public delegate void CallBack();
    /// <summary>
    /// 拉到最下回调
    /// </summary>
    public CallBack onDropDown;
    /// <summary>
    /// 拉到最上回调
    /// </summary>
    public CallBack onDropTop;
    #endregion

    #region Member

    public ScrollRect scrollView;
    [HideInInspector]
    public RectTransform rectTransform;

    /// <summary>
    /// 下拉提示
    /// </summary>
    public GameObject DropDownTip;
    /// <summary>
    /// 上拉提示
    /// </summary>
    public GameObject DropUpTip;
    /// <summary>
    /// 滚动视图状态
    /// </summary>
    [HideInInspector]
    public ScrollState scrollState = ScrollState.Between;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        if (scrollView == null)
        {
            scrollView = GetComponent<ScrollRect>();
        }
        rectTransform = scrollView.GetComponent<RectTransform>();
    }

    private void OnDestroy()
    {
        onDropDown = null;
        onDropTop = null;
        DropDownTip = null;
        DropUpTip = null;
    }


    private void Update()
    {
        if (scrollView == null) return;
        if (rectTransform == null) return;
        if (scrollView.content == null) return;
        if (scrollView.content.sizeDelta.y > rectTransform.sizeDelta.y)
        {
            if (scrollView.verticalNormalizedPosition < Mathf.Epsilon)
            {
                if (scrollState != ScrollState.Down)
                {
                    scrollState = ScrollState.Down;
                    if (DropDownTip != null)
                    {
                        DropDownTip.SetActive(false);
                    }
                    if (onDropDown != null)
                    {
                        onDropDown();
                    }
                }
            }
            else if (scrollView.verticalNormalizedPosition > 1 - Mathf.Epsilon)
            {
                if (scrollState != ScrollState.Top)
                {
                    scrollState = ScrollState.Top;
                    if (DropUpTip != null)
                    {
                        DropUpTip.SetActive(false);
                    }
                    if (onDropTop != null)
                    {
                        onDropTop();
                    }
                }
            }
            else
            {
                if (scrollState == ScrollState.Down)
                {
                    if (DropDownTip != null)
                    {
                        DropDownTip.SetActive(true);
                    }
                }
                else if (scrollState == ScrollState.Top)
                {
                    if (DropUpTip != null)
                    {
                        DropUpTip.SetActive(true);
                    }
                }
                scrollState = ScrollState.Between;
            }
        }
        else
        {
            if (scrollState != ScrollState.Fixed)
            {
                if (DropDownTip != null)
                {
                    DropDownTip.SetActive(false);
                }
                if (DropUpTip != null)
                {
                    DropUpTip.SetActive(false);
                }
            }
            scrollState = ScrollState.Fixed;
        }
    }
    #endregion
}
