//===================================================
//Author      : DRB
//CreateTime  ：7/5/2016 11:34:37 PM
//Description ：泛型单例基类
//===================================================
using UnityEngine;
using System.Collections;
using System;

public class Singleton<T> : IDisposable where T : new()
{
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

    public static void CreateInstance()
    {
        if (instance == null)
        {
            instance = new T();
        }
    }

    public static T GetInstance()
    {
        if (instance == null)
        {
            instance = new T();
        }
        return instance;
    }

    public virtual void Dispose()
    {
        
    }
}