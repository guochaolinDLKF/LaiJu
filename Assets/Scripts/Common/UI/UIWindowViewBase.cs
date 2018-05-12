//===================================================
//Author      : DRB
//CreateTime  ：7/5/2016 11:34:37 PM
//Description ：窗口UI视图基类
//===================================================

using UnityEngine;
using System;
using UnityEngine.UI;

public class UIWindowViewBase : UIViewBase
{
    /// <summary>
    /// 挂点类型
    /// </summary>
    [SerializeField]
    public ContainerType containerType = ContainerType.Center;

    /// <summary>
    /// 打开方式
    /// </summary>
    [SerializeField]
    public UIWindowShowStyleType showStyle = UIWindowShowStyleType.Normal;

    /// <summary>
    /// 打开或关闭动画效果持续时间
    /// </summary>
    [SerializeField]
    public float duration = 0.2f;

    /// <summary>
    /// 窗口持久化类型
    /// </summary>
    public UIWindowPersistenceType persistenceType = UIWindowPersistenceType.Once;

    /// <summary>
    /// 窗口名称
    /// </summary>
    [HideInInspector]
    public string WindowName;

    /// <summary>
    /// 下一个要打开的窗口
    /// </summary>
    private UIWindowType m_NextOpenUIType;

    [HideInInspector]
    public bool isShow = true;

    [SerializeField]
    private bool m_hasMask;
    [SerializeField]
    private Color m_MaskColor = new Color(0f, 0f, 0f, 0.6f);

    [SerializeField]
    private bool m_MaskCanClose = true;

    private GameObject m_Mask;



    protected override void OnStart()
    {
        base.OnStart();
        RectTransform rect = GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.sizeDelta = new Vector2(1,1);
        }
        if (m_hasMask)
        {
            if (UIViewManager.Instance.CurrentUIScene.CurrentCanvas == null) return;
            m_Mask = new GameObject("Mask(Instance)");
            m_Mask.SetParent(transform,true);
            m_Mask.transform.SetAsFirstSibling();
            //m_Mask.transform.SetSiblingIndex(transform.parent.GetSiblingIndex());
            Image mask = m_Mask.AddComponent<Image>();
            mask.color = m_MaskColor;
            if (m_MaskCanClose)
            {
                Button btn = m_Mask.AddComponent<Button>();
                btn.onClick.AddListener(OnMaskClick);
            }
            mask.rectTransform.anchorMax = Vector2.one;
            mask.rectTransform.anchorMin = Vector2.zero;
            mask.rectTransform.sizeDelta = new Vector2(5000f,5000f);
        }
    }

    protected virtual void OnMaskClick()
    {
        Close();
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        if (go.name.Equals("btnClose", StringComparison.CurrentCultureIgnoreCase))
        {
            Close();
        }
    }

    #region Hide 隐藏窗口
    /// <summary>
    /// 隐藏窗口
    /// </summary>
    public virtual void Hide()
    {
        isShow = false;
        gameObject.SetActive(false);
    }
    #endregion

    #region Show 显示窗口
    /// <summary>
    /// 显示窗口
    /// </summary>
    public virtual void Show()
    {
        isShow = true;
        gameObject.SetActive(true);
    }
    #endregion

    #region Close 关闭窗口
    /// <summary>
    /// 关闭窗口
    /// </summary>
    public virtual void Close()
    {
        UIViewUtil.Instance.CloseWindow(WindowName);
    }
    #endregion

    #region CloseOpenNext 关闭并且打开下一个窗口
    /// <summary>
    /// 关闭并且打开下一个窗口
    /// </summary>
    /// <param name="nextType"></param>
    public virtual void CloseOpenNext(UIWindowType nextType)
    {
        this.Close();
        if (nextType == UIWindowType.None) return;
        UIViewUtil.Instance.NextOpenViewName = nextType.ToString();
    }

    public virtual void CloseOpenNext(string nextName)
    {
        if (string.IsNullOrEmpty(nextName)) return;
        UIViewUtil.Instance.NextOpenViewName = nextName;
    }
    #endregion

    #region BeforeOnDestroy 销毁之前
    /// <summary>
    /// 销毁之前
    /// </summary>
    protected override void BeforeOnDestroy()
    {
        if (m_Mask != null)
        {
            Destroy(m_Mask);
        }
    }
    #endregion
}