//===================================================
//Author      : DRB
//CreateTime  ：3/8/2017 11:24:32 AM
//Description ：消息提示窗口视图
//===================================================

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public enum AutoClickType
{
    None,
    Ok,
    Cancel
}

/// <summary>
/// 消息提示窗口视图
/// </summary>
public class UIMessageView : MonoBehaviour
{
    #region Private Variable
    /// <summary>
    /// 标题
    /// </summary>
    [SerializeField]
    private Text lblTitle;
    /// <summary>
    /// 内容
    /// </summary>
    [SerializeField]
    private Text lblMessage;
    /// <summary>
    /// 确定按钮
    /// </summary>
    [SerializeField]
    private Button btnOk;
    /// <summary>
    /// 取消按钮
    /// </summary>
    [SerializeField]
    private Button btnCancel;
    /// <summary>
    /// 确定按钮文本框
    /// </summary>
    [SerializeField]
    private Text m_TextOk;
    /// <summary>
    /// 取消按钮文本框
    /// </summary>
    [SerializeField]
    private Text m_TextCancel;
    /// <summary>
    /// 确定按钮回调
    /// </summary>
    public Action OnOkClickHandler;
    /// <summary>
    /// 取消按钮回调
    /// </summary>
    public Action OnCancelHandler;
    /// <summary>
    /// 倒计时
    /// </summary>
    private float m_fCountDown;
    /// <summary>
    /// 自动点击类型
    /// </summary>
    private AutoClickType m_AutoType;
    #endregion

    #region MonoBehaviour

    void Awake()
    {
        EventTriggerListener.Get(btnOk.gameObject).onClick = BtnOkClickCallBack;
        EventTriggerListener.Get(btnCancel.gameObject).onClick = BtnCancelClickCallBack;
    }

    private void Update()
    {
        if (m_AutoType != AutoClickType.None)
        {
            m_fCountDown -= Time.deltaTime;
            if (m_fCountDown < 0)
            {
                m_TextCancel.SafeSetText("取消");
                m_TextOk.SafeSetText("确定");
                switch (m_AutoType)
                {
                    case AutoClickType.Cancel:
                        BtnCancelClickCallBack(btnCancel.gameObject);
                        break;
                    case AutoClickType.Ok:
                        BtnOkClickCallBack(btnOk.gameObject);
                        break;
                }
            }
            switch (m_AutoType)
            {
                case AutoClickType.Cancel:
                    m_TextCancel.SafeSetText(string.Format("取消({0})", m_fCountDown.ToString("0")));
                    break;
                case AutoClickType.Ok:
                    m_TextOk.SafeSetText(string.Format("确定({0})", m_fCountDown.ToString("0")));
                    break;
            }

        }
    }

    #endregion

    #region Private Function
    #region BtnOkClickCallBack 确认按钮点击
    /// <summary>
    /// 确认按钮点击
    /// </summary>
    /// <param name="go"></param>
    private void BtnOkClickCallBack(GameObject go)
    {
        AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);
        Close();
        if (OnOkClickHandler != null) OnOkClickHandler();
    }
    #endregion

    #region BtnCancelClickCallBack 取消按钮点击
    /// <summary>
    /// 取消按钮点击
    /// </summary>
    /// <param name="go"></param>
    private void BtnCancelClickCallBack(GameObject go)
    {
        AudioEffectManager.Instance.Play("btnclose", Vector3.zero, false);
        Close();
        if (OnCancelHandler != null) OnCancelHandler();
    }
    #endregion

    #region Close 关闭窗口
    /// <summary>
    /// 关闭窗口
    /// </summary>
    private void Close()
    {
        m_AutoType = AutoClickType.None;
        m_fCountDown = 0.0f;
        gameObject.transform.localPosition = new Vector3(0, 5000, 0);
    }
    #endregion
    #endregion

    #region Public Function
    #region Show 显示窗口
    /// <summary>
    /// 显示窗口
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="message">内容</param>
    /// <param name="type">类型</param>
    /// <param name="okAction">确定回调</param>
    /// <param name="cancelAction">取消回调</param>
    public void Show(string title, string message,float countDown = 0f,AutoClickType autoType = AutoClickType.None, MessageViewType type = MessageViewType.Ok, Action okAction = null, Action cancelAction = null)
    {
        gameObject.transform.localPosition = Vector3.zero;
        lblTitle.text = title;
        lblMessage.text = message;

        switch (type)
        {
            case MessageViewType.Ok:
                btnOk.transform.localPosition = new Vector3(0, btnOk.transform.localPosition.y, 0);
                btnCancel.gameObject.SetActive(false);
                btnOk.gameObject.SetActive(true);
                break;
            case MessageViewType.OkAndCancel:
                btnOk.transform.localPosition = new Vector3(-218, btnOk.transform.localPosition.y, 0);
                btnCancel.gameObject.SetActive(true);
                btnOk.gameObject.SetActive(true);
                break;
            case MessageViewType.None:
                btnCancel.gameObject.SetActive(false);
                btnOk.gameObject.SetActive(false);
                break;
        }
        m_AutoType = autoType;
        m_fCountDown = countDown;
        switch (autoType)
        {
            case AutoClickType.None:
                m_TextOk.SafeSetText("确定");
                m_TextCancel.SafeSetText("取消");
                break;
            case AutoClickType.Cancel:
                m_TextOk.SafeSetText("确定");
                break;
            case AutoClickType.Ok:
                m_TextCancel.SafeSetText("取消");
                break;
        }

        OnOkClickHandler = okAction;
        OnCancelHandler = cancelAction;
    }
    #endregion
    #endregion
}