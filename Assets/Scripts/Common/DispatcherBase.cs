//===================================================
//Author      : DRB
//CreateTime  ：7/4/2016 10:32:43 PM
//Description ：泛型观察者基类
//===================================================
using UnityEngine;
using System.Collections.Generic;
using System;



public class DispatcherBase<T,P,K> : IDisposable where T:new() where P :class
{
    #region 单例
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

    public delegate void Handler(P obj);//声明一个委托
    //声明并实例化一个委托字典，字典的Key可以是任何类型，value必须是委托列表
    private Dictionary<K, List<Handler>> m_pDictionary = new Dictionary<K, List<Handler>>();
    /// <summary>
    /// 添加监听事件
    /// </summary>
    /// <param name="key"></param>
    /// <param name="handler"></param>
    public void AddEventListener(K key,Handler handler)
    {
        if (m_pDictionary.ContainsKey(key))//如果字典中存在这个key，则将传入的委托添加进字典中
        {

            m_pDictionary[key].Add(handler);
        }
        else//如果不存在，则实例化一个委托列表，并且将传入的委托添加进字典中
        {
            m_pDictionary[key] = new List<Handler>();
            m_pDictionary[key].Add(handler);
        }
    }
    /// <summary>
    /// 移除监听事件
    /// </summary>
    /// <param name="key"></param>
    /// <param name="handler"></param>
    public void RemoveEventListener(K key, Handler handler)
    {
        if (m_pDictionary.ContainsKey(key))
        {
            if (m_pDictionary[key].Contains(handler))
            {
                m_pDictionary[key].Remove(handler);
            }
        }

    }
    /// <summary>
    /// 执行监听事件，传入消息类型（key）和参数,当存在这个消息类型的时候就执行这个消息类型所对应的委托和委托函数中的参数
    /// </summary>
    /// <param name="key">消息类型</param>
    /// <param name="param">参数</param>
    public void Dispatch(K key,P param)
    {
        if (m_pDictionary.ContainsKey(key))
        {
            if (m_pDictionary[key] != null)
            {
                for (int i = 0; i < m_pDictionary[key].Count; ++i)
                {
                    m_pDictionary[key][i](param);
                }
            }
        }
    }
    /// <summary>
    /// 当没有参数的时候所执行的消息
    /// </summary>
    /// <param name="key"></param>
    public void Dispatch(K key)
    {
        Dispatch(key, null);
    }
    /// <summary>
    /// 销毁对象
    /// </summary>
    public void Dispose()
    {
        
    }
}
