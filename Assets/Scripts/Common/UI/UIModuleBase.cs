//===================================================
//Author      : DRB
//CreateTime  ：5/20/2017 4:19:40 PM
//Description ：UI模块基类
//===================================================
using System.Collections.Generic;
using UnityEngine;


public class UIModuleBase : MonoBehaviour 
{
    protected Dictionary<string, ModelDispatcher.Handler> m_Dic;
    void Awake()
    {
        m_Dic = DicNotificationInterests();
        foreach (var pair in m_Dic)
        {
            ModelDispatcher.Instance.AddEventListener(pair.Key, pair.Value);
        }

        OnAwake();
    }

    private void Start()
    {
        OnStart();
    }

    private void OnDestroy()
    {
        if (m_Dic != null)
        {
            foreach (var pair in m_Dic)
            {
                ModelDispatcher.Instance.RemoveEventListener(pair.Key, pair.Value);
            }
            m_Dic.Clear();
            m_Dic = null;
        }
        BeforeOnDestroy();
    }

    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void BeforeOnDestroy() { }

    #region DicNotificationInterests 所有监听的事件
    /// <summary>
    /// 所有监听的事件
    /// </summary>
    /// <returns></returns>
    public virtual Dictionary<string, ModelDispatcher.Handler> DicNotificationInterests()
    {
        return new Dictionary<string, ModelDispatcher.Handler>();
    }
    #endregion

    protected void AddListener(string notificationName, ModelDispatcher.Handler action)
    {
        if (m_Dic == null)
        {
            m_Dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        }
        if (m_Dic.ContainsKey(notificationName)) return;
        m_Dic.Add(notificationName, action);
        ModelDispatcher.Instance.AddEventListener(notificationName, action);
    }

    protected void RemoveListener(string notificationName, ModelDispatcher.Handler action)
    {
        if (m_Dic == null) return;
        if (!m_Dic.ContainsKey(notificationName)) return;
        m_Dic.Remove(notificationName);
        ModelDispatcher.Instance.RemoveEventListener(notificationName, action);
    }

    protected virtual void SendNotification(string notificationName, params object[] param)
    {
        UIDispatcher.Instance.Dispatch(notificationName, param);
    }
}
