//===================================================
//Author      : DRB
//CreateTime  ：9/13/2016 12:33:27 AM
//Description ：特效管理器
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DRBPool;
using System;

public class EffectManager : Singleton<EffectManager>
{
    private SpawnPool m_EffectPool;

    private Dictionary<string, Transform> m_EffectDic = new Dictionary<string, Transform>();


    public void Init(MonoBehaviour mono)
    {
        m_EffectPool = PoolManager.Pools.Create("Effect");
    }

    /// <summary>
    /// 清空特效
    /// </summary>
    public void Clear()
    {
        m_EffectDic.Clear();
        m_EffectPool.DespawnAll();
        m_EffectPool = null;
    }

    #region LoadEffectAsync 异步加载特效
    /// <summary>
    /// 异步加载特效
    /// </summary>
    /// <param name="effectName"></param>
    /// <returns></returns>
    private void LoadEffectAsync(string effectName, Action<GameObject> onComplete)
    {
        string path = string.Format("download/{0}/effect/{1}.drb", ConstDefine.GAME_NAME, effectName);
        AssetBundleManager.Instance.LoadOrDownload(path, effectName, (GameObject go) =>
        {
            if (onComplete != null)
            {
                onComplete(go);
            }
        });
    }
    #endregion

    #region LoadEffect 同步加载特效
    /// <summary>
    /// 同步加载特效
    /// </summary>
    /// <param name="effectName"></param>
    /// <returns></returns>
    private GameObject LoadEffect(string effectName)
    {
        string path = string.Format("download/{0}/effect/{1}.drb", ConstDefine.GAME_NAME, effectName);
        return AssetBundleManager.Instance.LoadAssetBundle<GameObject>(path,effectName);
    }
    #endregion

    public Transform PlayEffect(string effectName, float delayDestroy = 0f)
    {
        if (!m_EffectDic.ContainsKey(effectName))
        {
            GameObject go = LoadEffect(effectName);
            if (go == null) return null;
            m_EffectDic[effectName] = go.transform;
            PrefabPool prefabPool = new PrefabPool(m_EffectDic[effectName]);
            prefabPool.PreloadAmount = 0;//预加载数量
            prefabPool.IsCullDespawned = true;//是否开启缓存池自动清理
            prefabPool.CullAbove = 1;//缓存池自动清理 但是始终保留对象的个数
            prefabPool.CullDelay = 5;//多长时间清理一次   秒
            prefabPool.CullMaxPerPass = 2;//每次清理几个
            m_EffectPool.CreatePrefabPool(prefabPool);
        }

        Transform effect = m_EffectPool.Spawn(m_EffectDic[effectName]);
        if (delayDestroy > 0f)
        {
            DestroyEffect(effect, delayDestroy);
        }
        return effect;
    }

    /// <summary>
    /// 播放特效
    /// </summary>
    /// <param name="effectName"></param>
    /// <returns></returns>
    public void PlayEffectAsync(string effectName, Action<Transform> onBeginPlay, float delayDestroy = 0f)
    {
        if (!m_EffectDic.ContainsKey(effectName))
        {
            LoadEffectAsync(effectName, (GameObject go) =>
            {
                if (go != null)
                {
                    m_EffectDic[effectName] = go.transform;
                    PrefabPool prefabPool = new PrefabPool(m_EffectDic[effectName]);
                    prefabPool.PreloadAmount = 0;//预加载数量
                    prefabPool.IsCullDespawned = true;//是否开启缓存池自动清理
                    prefabPool.CullAbove = 1;//缓存池自动清理 但是始终保留对象的个数
                    prefabPool.CullDelay = 5;//多长时间清理一次   秒
                    prefabPool.CullMaxPerPass = 2;//每次清理几个
                    m_EffectPool.CreatePrefabPool(prefabPool);
                    Transform effect = m_EffectPool.Spawn(m_EffectDic[effectName]);
                    if (onBeginPlay != null)
                    {
                        onBeginPlay(effect);
                    }
                    if (delayDestroy > 0f)
                    {
                        DestroyEffect(effect, delayDestroy);
                    }
                }
            });
        }
        else
        {
            Transform effect = m_EffectPool.Spawn(m_EffectDic[effectName]);
            if (onBeginPlay != null)
            {
                onBeginPlay(effect);
            }
            if (delayDestroy > 0f)
            {
                DestroyEffect(effect, delayDestroy);
            }
        }

        Debug.Log("播放特效" + effectName);
    }

    /// <summary>
    /// 销毁特效
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="time"></param>
    public void DestroyEffect(Transform trans, float delay)
    {
        if (trans == null) return;
        m_EffectPool.StartCoroutine(DestroyEffectCoroutine(trans, delay));
        //m_EffectPool.Despawn(trans, delay);
    }

    /// <summary>
    /// 延迟销毁
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator DestroyEffectCoroutine(Transform trans, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (trans == null) yield break;
        trans.gameObject.SetLayer(0);
        m_EffectPool.Despawn(trans);
    }



}
