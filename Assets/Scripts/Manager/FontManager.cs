//===================================================
//Author      : DRB
//CreateTime  ：8/30/2017 3:54:40 PM
//Description ：
//===================================================
using UnityEngine;


public class FontManager : Singleton<FontManager> 
{
    /// <summary>
    /// 加载字体
    /// </summary>
    /// <param name="font"></param>
    /// <returns></returns>
    public Font LoadFont(string font)
    {
        string path = string.Format("download/{0}/source/font/{1}.drb",ConstDefine.GAME_NAME,font);
        Font ret = AssetBundleManager.Instance.LoadAssetBundle<Font>(path,font);
        return ret;
    }
}
