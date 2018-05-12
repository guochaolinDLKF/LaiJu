//===================================================
//Author      : DRB
//CreateTime  ：12/6/2017 10:58:10 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShiSanZhang {
    public class ShiSanZhangPrefabManager : Singleton<ShiSanZhangPrefabManager>
    {

        /// <summary>
        /// 加载骰子
        /// </summary>
        private Dictionary<string, Sprite> diceDic = new Dictionary<string, Sprite>();


        public Sprite LoadSprite(Poker poker)
        {
            if (diceDic.ContainsKey(poker.ToString()))
            {
                return diceDic[poker.ToString()];
            }
            else
            {
                string path = string.Format("download/{0}/source/uisource/gameuisource/shisanzhangpoker.drb", ConstDefine.GAME_NAME, poker.ToString());
                string name = poker.ToString();
                Sprite sprite = AssetBundleManager.Instance.LoadSprite(path, name);
                diceDic.Add(name,sprite);
                return sprite;
            }
            
        }
    }
}
