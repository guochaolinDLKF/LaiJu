//===================================================
//Author      : DRB
//CreateTime  ：7/5/2016 11:34:37 PM
//Description ：Mono泛型单例基类
//===================================================

using UnityEngine;
using System.Collections;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    #region 单例
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                DontDestroyOnLoad(obj);
                instance = obj.GetOrCreatComponent<T>();
            }
            return instance;
        }
    }
    #endregion

    public static void CreateInstance()
    {
        if (instance == null)
        {
            GameObject obj = new GameObject(typeof(T).Name);
            DontDestroyOnLoad(obj);
            instance = obj.GetOrCreatComponent<T>();
        }
    }

    void Awake()
    {
        OnAwake();
    }

    void Start()
    {
        OnStart();
    }

    void OnDestroy()
    {
        BeforeOnDestroy();
    }

    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void BeforeOnDestroy() { }


}