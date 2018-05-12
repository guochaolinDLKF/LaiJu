//===================================================
//Author      : DRB
//CreateTime  ：5/2/2017 12:04:34 PM
//Description ：战绩回放数据
//===================================================
using System.Collections.Generic;
using DRB.MahJong;
using UnityEngine;

/// <summary>
/// 战绩回放数据
/// </summary>
public class RecordReplayEntity 
{
    public int[] setting;

    public int roomId;

    public int banker;

    public int loop;

    public int pokerTotal;

    public int baseScore;

    public int firstDicePos;

    public int firstDiceA;

    public int firstDiceB;

    public int secondDicePos;

    public int secondDiceA;

    public int secondDiceB;

    public Poker luckPoker;

    public List<Poker> universal;

    public List<RecordSeat> seat;

    public List<RecordOperate> record;

    public RecordProb prob;

}

public class RecordSeat
{
    public int pos;

    public int atPos;

    public int playerId;

    public string nickname;

    public string avatar;

    public int gender;

    public int gold;

    public int settle;

    public int multiply;

    public int wind;

    public bool isWinner;

    public bool isLoser;

    public int lackColor;

    public List<Poker> poker;

    public List<Poker> universal;

    public List<RecordIncome> incomes;

    public List<RecordIncome> scores;
}


/// <summary>
/// 战绩回放结算
/// </summary>
public class RecordIncome
{
    public int playerId;

    //public int type;

    public int cfgId;

    public int times;
}

/// <summary>
/// 战绩回放操作
/// </summary>
public class RecordOperate
{
    public int operate;

    public int type;

    public int subType;

    public int playerId;

    public bool isLast;

    public List<Poker> poker;
}

/// <summary>
/// 战绩回放抓马
/// </summary>
public class RecordProb
{
    public int playerId;

    public int prob;

    public int multiply;

    public List<Poker> poker;
}
