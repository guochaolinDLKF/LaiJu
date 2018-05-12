//===================================================
//Author      : DRB
//CreateTime  ：5/4/2017 1:57:50 PM
//Description ：
//===================================================
using System.Collections.Generic;
using UnityEngine;


public class MatchHTTPEntity 
{
    public int id;

    public int gameId;

    public int player;

    public string title;

    public string subtitle;

    public string cover;

    public string detail;

    public string ipaddr;

    public int port;

    public int costNums;

    public int costType;

    public string rule;

    public string desc;

    public List<string> reward;

    public List<MatchRewardEntity> rewardSetting;
}

public class MatchRewardEntity
{
    public int begin;

    public int end;

    public int itemId;

    public int amount;
}
