//===================================================
//Author      : DRB
//CreateTime  ：6/6/2017 1:47:42 PM
//Description ：扑克牌管理器
//===================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZhaJh;


public class ZJHPrefabManager : Singleton<ZJHPrefabManager>
{

    public GameObject LoadPoker(Poker poker, Action<Sprite> onComplete, string spriteName)
    {
        const string prefabName = "uizjhitempoker";// "UIZJHItemPoker";
        string prefabPath = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, prefabName);
        GameObject go = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(prefabPath, prefabName);

        Sprite sprite = null;
        string spriteNameK = poker == null ? "16_1" : poker.ToString();
        string pathK = string.Format("download/{0}/source/uisource/gameuisource/poker/{1}.drb", ConstDefine.GAME_NAME, spriteName);
        sprite = AssetBundleManager.Instance.LoadSprite(pathK, spriteNameK);
        go = UnityEngine.Object.Instantiate(go);
        go.GetComponent<UIZhaJHItemHandPoker>().SetUI(sprite);

        return go;

    }


    /// <summary>
    /// 发牌的时候实例化牌
    /// </summary>
    /// <param name="poker"></param>
    /// <param name="onComplete"></param>
    /// <param name="spriteName"></param>
    /// <returns></returns>
    public GameObject LoadPokerFP(Poker poker, Action<Sprite> onComplete, string spriteName)
    {
        const string prefabName = "uizjhitempoker";// "UIZJHItemPoker";
        string prefabPath = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, prefabName);
        GameObject go = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(prefabPath, prefabName);

        Sprite sprite = null;
        string spriteNameK = poker == null ? "16_1" : poker.ToString();
        string pathK = string.Format("download/{0}/source/uisource/gameuisource/poker/{1}.drb", ConstDefine.GAME_NAME, spriteName);
        sprite = AssetBundleManager.Instance.LoadSprite(pathK, spriteNameK);
        go.GetComponent<UIZhaJHItemHandPoker>().SetUI(sprite);

        return go;

    }

    /// <summary>
    /// 加载结算界面
    /// </summary>
    /// <param name="seat"></param>
    /// <param name="onComplete"></param>
    /// <returns></returns>
    public GameObject LoadInfo(string playerId, Action<Sprite> onComplete, float profit)
    {
        const string prefabName = "uizjhiteminfo"; // "UIZJHItemInfo"
        string prefabPath = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, prefabName);
        GameObject go = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(prefabPath, prefabName);

        go = UnityEngine.Object.Instantiate(go);
        go.GetComponent<UIZhaJHItemJSInfo>().Assignment(playerId.ToString(), profit);
        return go;
    }

    /// <summary>
    /// 实例化筹码
    /// </summary>
    /// <param name="chip"></param>
    /// <returns></returns>
    public GameObject LoadChip(int chip)
    {
        const string prefabName = "uizjhchipanimation"; //"UIZJHChipAnimation"
        string prefabPath = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, prefabName);
        GameObject go = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(prefabPath, prefabName);

        Sprite sprite = null;
        string spriteNameK = string.Format("img_chouma{0}fen", chip.ToString());
        string pathK = string.Format("download/{0}/source/uisource/zhajhpicture.drb", ConstDefine.GAME_NAME);
        sprite = AssetBundleManager.Instance.LoadSprite(pathK, spriteNameK);
        //go = UnityEngine.Object.Instantiate(go);
        go.GetComponent<UIZhaJHItemHandPoker>().SetUI(sprite);

        return go;
    }

    /// <summary>
    /// 申请加入房间
    /// </summary>
    /// <param name="player"></param>
    /// <param name="SetUp"></param>
    /// <returns></returns>
    public GameObject LoadApplyJoinRoom(PlayerEntityZjh player, Action<GameObject> SetUp)
    {
        const string prefabName = "uizjhitemappyjoin";
        string prefabPath = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, prefabName);
        GameObject go = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(prefabPath, prefabName);
        go = UnityEngine.Object.Instantiate(go);
        go.GetComponent<UIZhaJHItemBtnTiShi>().PromptSwitch(player);
        SetUp(go);
        return go;
    }

   

    /// <summary>
    /// 加载结算界面
    /// </summary>
    /// <param name="seat"></param>
    /// <param name="onComplete"></param>
    /// <returns></returns>
    public void LoadSendBill(int id, float gold, string avatar, Action<GameObject> onComplete, string name)
    {
        string prefabName = name; // "UIZJHItemPlayerBill"
        string prefabPath = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, prefabName);
        GameObject go = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(prefabPath, prefabName);

        go = UnityEngine.Object.Instantiate(go);
        if (onComplete != null) onComplete(go);
        go.GetComponent<UIZhaJHItemBill>().SetUI(id, gold, avatar);
    }




    //加载战绩详情的牌
    public GameObject RecordLoadPoker(int size,int color, Action<Sprite> onComplete, string spriteName)
    {
        const string prefabName = "uizjhitempoker";
        string prefabPath = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, prefabName);
        GameObject go = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(prefabPath, prefabName);

        Sprite sprite = null;
        //poker == null ? "16_1" : poker.ToString();
        
        string spriteNameK = string.Format("{0}_{1}",size,color);        
        string pathK = string.Format("download/{0}/source/uisource/gameuisource/poker/{1}.drb", ConstDefine.GAME_NAME, spriteName);
        sprite = AssetBundleManager.Instance.LoadSprite(pathK, spriteNameK);
        go = UnityEngine.Object.Instantiate(go);
        go.GetComponent<UIZhaJHItemHandPoker>().SetUI(sprite);

        return go;

    }

    

}
