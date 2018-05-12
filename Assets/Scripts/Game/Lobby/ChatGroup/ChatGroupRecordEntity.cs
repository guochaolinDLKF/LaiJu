//===================================================
//Author      : DRB
//CreateTime  ：11/21/2017 3:27:29 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;


public class RecordEntity
{
    public int id;

    public int winer;

    public int roomId;

    public int ownerId;

    public string ownerNickname;

    public int battleId;

    public int loop;

    public int score;

    public int roomType;

    public string time;

    public int gameId;

    public string gameName;

    public List<RecordPlayerEntity> players;
}

public class RecordPlayerEntity : IComparable<RecordPlayerEntity>
{
    public int id;

    public string nickname;

    public int score;

    public string name;

    public int money;

    public int gold;

    public string avatar;

    public List<RecordPokerEntity> recordPokerList;

    public int CompareTo(RecordPlayerEntity obj)
    {
        if (obj == null) return 0;

        return obj.gold - gold;
    }
}
//战绩扑克类
public class RecordPokerEntity
{
    public int number;
    public int flower;
}
