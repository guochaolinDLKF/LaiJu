//===================================================
//Author      : DRB
//CreateTime  ：6/21/2017 5:55:17 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZJHSpriteManager:Singleton<ZJHSpriteManager>
{

    public Sprite ZJHSprite(string strName)
    {
        Sprite sprite = null;
        string editorName = strName;
        string editorPath = string.Format("download/{0}/source/uisource/zhajhpicture.drb", ConstDefine.GAME_NAME);
        sprite = AssetBundleManager.Instance.LoadSprite(editorPath, editorName);
        return sprite;

    }
}
