//===================================================
//Author      : DRB
//CreateTime  ：3/17/2017 5:52:25 PM
//Description ：
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransferData
{
    public TransferData()
    {
        m_PutValues = new Dictionary<string, object>();
    }

    private IDictionary<string, object> m_PutValues;
    public IDictionary<string, object> PutValues
    {
        get { return m_PutValues; }
    }

    /// <summary>
    /// 存值
    /// </summary>
    /// <typeparam name="TM"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetValue<TM>(string key, TM value)
    {
        m_PutValues[key] = value;
    }

    /// <summary>
    /// 取值
    /// </summary>
    /// <typeparam name="TM"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public TM GetValue<TM>(string key)
    {
        if (m_PutValues.ContainsKey(key))
        {
            return (TM)m_PutValues[key];
        }
        return default(TM);
    }
}
