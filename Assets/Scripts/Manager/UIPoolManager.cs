//===================================================
//Author      : DRB
//CreateTime  ：9/27/2017 2:11:57 PM
//Description ：UI对象池管理器
//===================================================
using System.Collections.Generic;
using DRBPool;
using UnityEngine;


public class UIPoolManager : SingletonMono<UIPoolManager> 
{
    private SpawnPool m_Pool;


    protected override void OnAwake()
    {
        base.OnAwake();
        m_Pool = PoolManager.Pools.Create("UIItem");
        m_Pool.transform.SetParent(transform);
    }

    /// <summary>
    /// 回收
    /// </summary>
    /// <param name="instance"></param>
    public void Despawn(Transform instance)
    {
        m_Pool.Despawn(instance);
    }

    /// <summary>
    /// 生成
    /// </summary>
    /// <param name="prefabName"></param>
    public Transform Spawn(string prefabName)
    {
        if (!m_Pool.prefabPools.ContainsKey(prefabName))
        {
            GameObject go = UIViewManager.Instance.LoadItem(prefabName);
            if (go == null)
            {
                AppDebug.ThrowError("没有该预制体:" + prefabName);
            }
            Transform prefab = go.transform;
            PrefabPool pool = new PrefabPool(prefab);
            pool.PreloadAmount = 5;
            pool.IsCullDespawned = true;
            pool.CullAbove = 20;
            pool.CullDelay = 20;
            pool.CullMaxPerPass = 1;
            m_Pool.CreatePrefabPool(pool);
        }
        
        return m_Pool.Spawn(prefabName);
    }
}
