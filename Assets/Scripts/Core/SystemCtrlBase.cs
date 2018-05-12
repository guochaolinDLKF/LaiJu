
using System;
using System.Collections.Generic;
/// <summary>
/// 模块控制器基类
/// </summary>
/// <typeparam name="T"></typeparam>
public class SystemCtrlBase<T> : IDisposable where T : new()
{
    #region Singleton
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
    #endregion

    #region Variable
    private Dictionary<string, UIDispatcher.Handler> m_Dic;
    #endregion

    #region Constructors
    public SystemCtrlBase()
    {
        m_Dic = DicNotificationInterests();
        foreach (var pair in m_Dic)
        {
            UIDispatcher.Instance.AddEventListener(pair.Key, pair.Value);
        }
    }
    #endregion

    #region Dispose
    public virtual void Dispose()
    {
        if (m_Dic != null)
        {
            foreach (var pair in m_Dic)
            {
                UIDispatcher.Instance.RemoveEventListener(pair.Key, pair.Value);
            }
            m_Dic.Clear();
            m_Dic = null;
        }
    }
    #endregion

    #region DicNotificationInterests 所有监听的事件
    /// <summary>
    /// 所有监听的事件
    /// </summary>
    /// <returns></returns>
    public virtual Dictionary<string, UIDispatcher.Handler> DicNotificationInterests()
    {
        return new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
    }
    #endregion

    #region ShowMessage 显示窗口
    /// <summary>
    /// 显示窗口
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="message">内容</param>
    /// <param name="type">类型</param>
    /// <param name="okAction">确定回调</param>
    /// <param name="cancelAction">取消回调</param>
    protected void ShowMessage(string title, string message, MessageViewType type = MessageViewType.Ok, Action okAction = null, Action cancelAction = null, float countDown = 0.0f, AutoClickType autoType = AutoClickType.None)
    {
        UIViewManager.Instance.ShowMessage(title, message, type, okAction, cancelAction, countDown, autoType);
    }
    #endregion

    protected void ShowTip(string message)
    {
        UIViewManager.Instance.ShowTip(message);
    }
}