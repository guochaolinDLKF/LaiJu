//===================================================
//Author      : DRB
//CreateTime  ：11/30/2017 7:51:45 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WelfareActivitiesEntity  {
    public int treasureKeyNum;
    public List<int> lstBoxIndex;

    public int surPlusTimeCount;
    public int totalTimeCount;
    public List<lotteryWheelEntity> lstLotteryWheelEntity;
    public WelfareActivitiesEntity(int keyNum, int surPlusTimeCount, int totalTimeCount, List<lotteryWheelEntity> lstLotteryWheelEntity)
    {
        treasureKeyNum = keyNum;
        this.surPlusTimeCount = surPlusTimeCount;
        this.totalTimeCount = totalTimeCount;
        this.lstLotteryWheelEntity = lstLotteryWheelEntity;
    }
}
public class TreasureWindowEntity
{
    public int treasureKeyNum;
    public List<int> lstBoxIndex;
    public TreasureWindowEntity(int treasureKeyNum, List<int> lstBoxIndex)
    {
        this.treasureKeyNum = treasureKeyNum;
        this.lstBoxIndex = lstBoxIndex;
    }
}
public class LotteryWheelWindowEntity
{
    public int useable;
    public int total;
    public string ac_cj;
    public string ac_bg;
    public bool is_deduct;
    public int deduct_num;
    public List<lotteryWheelEntity> prize;
}

public class lotteryWheelEntity
{
    public int id;

    public string name;

    public string img_url;

    public GiftType type;
}

public class TreasureEntity
{
    public string name;
    
    public string img_url;
    
    public int type;

    //public int giftCount;

    //public int keyNum;

    //public bool IsCanOpenTreasure;

    //public int treasureIndex;
}
public class LotteryWheelGiftCallBackEntity
{
    public int id;

    public int useable;

    public int total;

    //public GiftType giftType;

    //public int giftCount;

    //public string message;
}
public class TreasureKeyNum
{
    public int key_num;
}

public class AgreeMentEntity
{
    public string content;
}

public class AgentServiceEntity
{
    public string agentWX;
}
