//===================================================
//Author      : DRB
//CreateTime  ：3/31/2017 10:37:14 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using DRB.MahJong;
using UnityEngine;

public class PokerConfig
{
    /// <summary>
    /// 符合条件的牌
    /// </summary>
    public List<Poker> Pokers;
}

public partial class cfg_configEntity
{
    /// <summary>
    /// 必须包含的牌
    /// </summary>
    private List<PokerConfig> mustList;
    public List<PokerConfig> Must
    {
        get
        {
            if (mustList == null)
            {
                mustList = new List<PokerConfig>();
                if (must.Equals("0")) return mustList;

                string[] arr = must.Split(';');
                for (int i = 0; i < arr.Length; ++i)
                {
                    if (string.IsNullOrEmpty(arr[i])) continue;
                    PokerConfig pokerConfig = new PokerConfig();
                    pokerConfig.Pokers = new List<Poker>();
                    string[] arr1 = arr[i].Split('|');
                    for (int j = 0; j < arr1.Length; ++j)
                    {
                        string[] arr2 = arr1[j].Split('_');
                        if (arr2.Length == 2)
                        {
                            Poker poker = new Poker(0,arr2[0].ToInt(), arr2[1].ToInt());
                            pokerConfig.Pokers.Add(poker);
                        }
                    }
                    mustList.Add(pokerConfig);
                }

            }
            return mustList;
        }
    }

    public bool MustIsSameColor
    {
        get { return mustIsSameColor == 1; }
    }

    public bool AllIsSameColor
    {
        get { return allIsSameColor == 1; }
    }


}
