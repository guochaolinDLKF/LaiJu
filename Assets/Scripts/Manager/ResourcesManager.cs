//===================================================
//Author      : DRB
//CreateTime  ：7/4/2016 1:23:24 AM
//Description ：资源管理器
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourcesManager : Singleton<ResourcesManager>
{
    #region Variation
    private Dictionary<string, GameObject> m_pDictionary = new Dictionary<string, GameObject>();
    #endregion

    #region Public Function
    public GameObject Load(ResourceType type,string name,bool isCache = true,bool isInstantiate = true)
    {
        GameObject go = null;
        string path = "";
        switch (type)
        {
            case ResourceType.UIScene:
                path += "UIScenes/";
                break;
            case ResourceType.UIWindow:
                path += "UIWindows/";
                break;
            case ResourceType.Prefab:
                path += "Prefabs/";
                break;
            case ResourceType.UIItem:
                path += "UIItems/";
                break;
            case ResourceType.UIAnimation:
                path += "UIAnimations/";
                break;
        }
        path += name;
        if (m_pDictionary.ContainsKey(path))
        {
            go = m_pDictionary[path];
        }
        else
        {
            go = Resources.Load<GameObject>(path);
            if (go == null)
            {
                AppDebug.LogWarning("资源" + path + "为空");
                return null;
            }
            if (isCache)
            {
                m_pDictionary.Add(path, go);
            }
        }
        if (isInstantiate)
        {
            return Object.Instantiate(go);
        }
        else
        {
            return go;
        }
    }
    #endregion
}
