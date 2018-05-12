//===================================================
//Author      : DRB
//CreateTime  ：12/2/2017 2:16:20 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILotteryWheelInfo : MonoBehaviour
{
    private int giftIndex;

    private int surPlusTime;

    private int surPlusTotalTime;

    //private string message;

    //private int giftCount;

    private GiftType giftType;

    public void SetTime(int surPlusTime, int surPlusTotalTime)
    {
        this.surPlusTime = surPlusTime;
        this.surPlusTotalTime = surPlusTotalTime;
    }
    public void SetGiftCallBackData(int giftIndex, string message, int surPlusTime, int surPlusTotalTime, int giftCount, GiftType giftType)
    {
        this.giftIndex = giftIndex;
        this.surPlusTime = surPlusTime;
        this.surPlusTotalTime = surPlusTotalTime;
        //this.message = message;
        //this.giftCount = giftCount;
        //this.giftType = giftType;
    }
    public void SetGiftCallBackData(int giftIndex, int surPlusTime, int surPlusTotalTime, GiftType giftType)
    {
        this.giftIndex = giftIndex;
        this.surPlusTime = surPlusTime;
        this.surPlusTotalTime = surPlusTotalTime;
        this.giftType = giftType;
    }
    public void SetGiftCallBackData(int giftIndex, int surPlusTime, int surPlusTotalTime)
    {
        this.giftIndex = giftIndex;
        this.surPlusTime = surPlusTime;
        this.surPlusTotalTime = surPlusTotalTime;
    }
    public int GetGiftIndex()
    {
        return giftIndex;
    }



    public int GetSurPlusTime()
    {
        return surPlusTime;
    }
    public int GetSurPlusTotalTime()
    {
        return surPlusTotalTime;
    }
    //public string GetMessage()
    //{
    //    return message;
    //}
    //public GiftType GetGiftType()
    //{
    //    return giftType;
    //}
    //public int GetGiftCount()
    //{
    //    return giftCount;
    //}

}
