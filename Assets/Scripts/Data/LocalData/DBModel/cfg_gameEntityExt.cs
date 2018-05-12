//===================================================
//Author      : DRB
//CreateTime  ：11/7/2017 4:03:40 PM
//Description ：
//===================================================
using System;
using UnityEngine;


public partial class cfg_gameEntity : IComparable<cfg_gameEntity>
{
    public int CompareTo(cfg_gameEntity other)
    {
        if (other == null) return 0;

        int a = (int)Enum.Parse(typeof(GameType),GameType, true);
        int b = (int)Enum.Parse(typeof(GameType), other.GameType, true);

        return a - b;
    }
}
