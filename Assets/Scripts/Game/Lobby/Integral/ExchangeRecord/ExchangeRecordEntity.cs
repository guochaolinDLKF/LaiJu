//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 9:47:27 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PrizeType
{
    Null,
    LotteryWheel,
    Treasure,
    Integral,
}
public class ExchangeRecordCallBackEntity
{
    public string wx_kf;

    public int status;

    public int id;

    public string contact_name;

    public string contact_address;

    public string contact_phone;
}


public class ExchangeRecordEntity {

    public int id;

    public int status;

    public string date;

    public string add_time;

    public string name;
}
