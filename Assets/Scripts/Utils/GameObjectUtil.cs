
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public static class GameObjectUtil 
{
    /// <summary>
    /// 获取或创建组建
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T GetOrCreatComponent<T>(this GameObject obj) where T:Component
    {
        T t = obj.GetComponent<T>();
        if (t == null)
        {
            t = obj.AddComponent<T>();
        }
        return t;
    }

    public static void SetParent(this GameObject obj,Transform parent,bool isUI = false)
    {
        if (parent == null)
        {
            AppDebug.LogWarning("设置的父节点为空！请检查引用对象");
            return;
        }
        obj.transform.SetParent(parent,!isUI);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localEulerAngles = Vector3.zero;
        //obj.SetLayer(parent.gameObject.layer);
    }

    public static void SetLayer(this GameObject obj,string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        Transform[] transArr = obj.transform.GetComponentsInChildren<Transform>();
        for (int i = transArr.Length - 1; i >= 0; --i)
        {
            transArr[i].gameObject.layer = layer;
        }
    }

    public static void SetLayer(this GameObject obj, int layer)
    {
        Transform[] transArr = obj.transform.GetComponentsInChildren<Transform>();
        for (int i = transArr.Length - 1; i >= 0; --i)
        {
            transArr[i].gameObject.layer = layer;
        }
    }

    public static void SafeSetActive(this MonoBehaviour mono, bool isActive)
    {
        if (mono != null)
        {
            mono.gameObject.SetActive(isActive);
        }
    }

    //================================UGUI扩展============================================

    /// <summary>
    /// 安全设置Text
    /// </summary>
    /// <param name="txtObj"></param>
    /// <param name="text"></param>
    public static void SafeSetText(this Text txtObj, string text, bool isAnimation = false,float duration = 0.2f,ScrambleMode scrambleMode = ScrambleMode.None)
    {
        if (txtObj != null)
        {
            if (isAnimation)
            {
                txtObj.text = "";
                txtObj.DOText(text, duration, true, scrambleMode);
            }
            else
            {
                txtObj.text = text;
            }
        }
    }

    public static void SafeSetSliderValue(this Slider sliObj, float value)
    {
        if (sliObj != null)
        {
            sliObj.value = value;
        }
    }
}