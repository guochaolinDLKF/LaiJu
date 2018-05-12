//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 9:47:27 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IntegralRecordEntityExternal
{
    public string wx;
    public IntegralRecordEntity integralRecordEntity;
}

public class IntegralRecordEntity {

    public int index;

    public bool isApply;

    public string exchangeDate;

    public string exchangeTime;

    public string exchangeGoods;

    public IntegralRecordEntity(int index_local,bool isApply_local, string exchangeData_local,string exchangeTime_local,string exchangeGoods_local)
    {
        index = index_local;
        isApply = isApply_local;
        exchangeDate = exchangeData_local;
        exchangeTime = exchangeTime_local;
        exchangeGoods = exchangeGoods_local;
    }
}
