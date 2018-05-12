//===================================================
//Author      : DRB
//CreateTime  ：9/8/2017 5:13:53 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuPaiJiuPrefabManager : Singleton<GuPaiJiuPrefabManager>
{
    /// <summary>
    /// 加载骰子
    /// </summary>
    private Dictionary<string, GameObject> diceDic = new Dictionary<string, GameObject>();

    public void ClosDic()
    {
        diceDic.Clear();
        dicImage.Clear();
    }
    public void LoadPrefab( string spriteName, Action<GameObject> onComplete)
    {      
        Sprite sprite = null;
        string path = string.Format("download/{0}/source/uisource/gameuisource/gupaijiudice.drb", ConstDefine.GAME_NAME);
        sprite = AssetBundleManager.Instance.LoadSprite(path, spriteName);      

        string prefabName1 = "dicesize";
        string path1 = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, prefabName1);
        AssetBundleManager.Instance.LoadOrDownload(path1, prefabName1, (GameObject go)=> 
        {
            if (diceDic.ContainsKey(spriteName))
            {
                diceDic[spriteName].gameObject.SetActive(true);
                onComplete(diceDic[spriteName].gameObject);
                diceDic.Remove(spriteName);
            }
            else
            {
                go = UnityEngine.Object.Instantiate(go);
                go.GetComponent<SetDice>().SetDiceImage(sprite);                               
                onComplete(go);
            }                                 
        });
    }

    public void PushToPool(string goName,GameObject go)
    {
        if (go == null) return;
        go.SetActive(false);
        if (diceDic.ContainsKey(goName))
        {
            diceDic[goName] = go;
        }
        else
        {
            diceDic.Add(goName, go);
        }     
    }


    /// <summary>
    /// 加载牌型图片
    /// </summary>
    private Dictionary<string, Sprite> dicImage = new Dictionary<string, Sprite>();
    public Sprite LoadSprite(string spriteName)
    {
        if (spriteName == "Kong") return null;
        Sprite sprite = null;
        string pathName = spriteName.ToLower();
        string path = string.Format("download/{0}/source/uisource/gameuisource/gupokertype.drb", ConstDefine.GAME_NAME);
        if (dicImage.ContainsKey(pathName))
        {
            return dicImage[pathName];
        }
        else
        {
            sprite = AssetBundleManager.Instance.LoadSprite(path, pathName);
            dicImage[pathName] = sprite;
            return sprite;
        }       
    }


    /// <summary>
    /// 加载牌
    /// </summary>
    /// <param name="spriteName"></param>
    /// <param name="onComplete"></param>
    public void LoadPoker(int index, int pokerType, Action<GameObject> onComplete, bool isV3 = false, bool isPoker = true)
    {
        string prefabName1 = string.Empty;
        string spriteName= string.Empty;
        string pokerPath = string.Empty;
        Vector3 tranV3 = new Vector3();
        switch (index)
        {
            case 0:
                if (isV3) tranV3 = new Vector3(0.3f, 0.3f, 0.3f);
                if (isPoker)
                {
                    prefabName1 = "uigupaijiuitempokerv";
                    pokerPath = "1";
                    spriteName = string.Format("{0}_{1}", pokerType, pokerPath);
                }
                else
                {
                    prefabName1 = "uigupaijiuitempokerh";
                    pokerPath = "2";
                    spriteName = string.Format("{0}_{1}", pokerType, pokerPath);
                }
                break;
            case 1:
                if (isV3) tranV3 = new Vector3(0.3f, 0.3f, 0.3f);
                if (isPoker)
                {
                    prefabName1 = "uigupaijiuitempokerh";
                    pokerPath = "2";
                    spriteName = string.Format("{0}_{1}", pokerType, pokerPath);
                }
                else
                {
                    prefabName1 = "uigupaijiuitempokerv";
                    pokerPath = "1";
                    spriteName = string.Format("{0}_{1}", pokerType, pokerPath);
                }
                break;
            case 2:
                if (isV3) tranV3 = new Vector3(0.3f, 0.3f, 0.3f);
                if (isPoker)
                {
                    prefabName1 = "uigupaijiuitempokerv";
                    pokerPath = "1";
                    spriteName = string.Format("{0}_{1}", pokerType, pokerPath);
                }
                else
                {
                    prefabName1 = "uigupaijiuitempokerh";
                    pokerPath = "2";
                    spriteName = string.Format("{0}_{1}", pokerType, pokerPath);
                }
                break;
            case 3:
                if (isV3) tranV3 = new Vector3(0.3f, 0.3f, 0.3f);
                if (isPoker)
                {
                    prefabName1 = "uigupaijiuitempokerh";
                    pokerPath = "2";
                    spriteName = string.Format("{0}_{1}", pokerType, pokerPath);
                }
                else
                {
                    prefabName1 = "uigupaijiuitempokerv";
                    pokerPath = "1";
                    spriteName = string.Format("{0}_{1}", pokerType, pokerPath);
                }                             
                break;
        }

        Sprite sprite = null;
        string path = string.Format("download/{0}/source/uisource/gameuisource/gupoker/poker{1}.drb", ConstDefine.GAME_NAME,pokerPath);
        sprite = AssetBundleManager.Instance.LoadSprite(path, spriteName);
   
        string path1 = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, prefabName1);
        AssetBundleManager.Instance.LoadOrDownload(path1, prefabName1, (GameObject go) =>
        {
            go = UnityEngine.Object.Instantiate(go);
            onComplete(go);
            if (isV3) go.transform.localScale = tranV3;
            go.GetComponent<GuPaiJiuCtrl>().SetUI(sprite);               
        });
    }


    public void LoadPoker(int index, Action<GameObject> onComplete)
    {
        string prefabName1 = string.Empty;
        switch (index)
        {
            case 0:
                prefabName1 = "uiGupaijiuitempokerxv";
                break;
            case 1:
                prefabName1 = "uiGupaijiuitempokerxh";
                break;
            case 2:
                prefabName1 = "uiGupaijiuitempokerxv";
                break;
            case 3:
                prefabName1 = "uiGupaijiuitempokerxh";
                break;
        }
        string path1 = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, prefabName1);
        AssetBundleManager.Instance.LoadOrDownload(path1, prefabName1, (GameObject go) =>
        {
            go = UnityEngine.Object.Instantiate(go);
            onComplete(go);           
        });

    }
    /// <summary>
    /// 加载筹码
    /// </summary>
    /// <param name="name"></param>
    /// <param name="onComplete"></param>
    public void LoadChip(string name, Action<GameObject> onComplete)
    {
        const string prefabName = "uiitemgupaijiuchip"; 
        string prefabPath = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, prefabName);
        GameObject go = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(prefabPath, prefabName);

        Sprite sprite = null;
        string spriteNameK = string.Format("g_chouma{0}", name);
        string pathK = string.Format("download/{0}/source/uisource/gupaijiuroom.drb", ConstDefine.GAME_NAME);
        sprite = AssetBundleManager.Instance.LoadSprite(pathK, spriteNameK);
        go = UnityEngine.Object.Instantiate(go);       
        go.GetComponent<Image>().sprite = sprite;
        onComplete(go);
    }



}
