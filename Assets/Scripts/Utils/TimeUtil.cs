//===================================================
//Author      : DRB
//CreateTime  ：3/8/2017 10:24:20 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class TimeUtil
{

    private const long TIME_STAMP_FROM_0_TO_1970 = 621355968000000000;

    #region GetTimestamp 获取时间戳(秒)
    /// <summary>
    /// 获取时间戳 定义为从格林尼治时间1970年01月01日00时00分00秒起至现在的总秒数
    /// </summary>
    /// <returns></returns>
    public static long GetTimestamp()
    {
        return (DateTime.Now.ToUniversalTime().Ticks - TIME_STAMP_FROM_0_TO_1970) / 10000000;
    }
    #endregion

    #region GetTimestamp 获取时间戳(毫秒)
    /// <summary>
    /// 获取时间戳 定义为从格林尼治时间1970年01月01日00时00分00秒起至现在的总毫秒数
    /// </summary>
    /// <returns></returns>
    public static long GetTimestampMS()
    {
        return (DateTime.Now.ToUniversalTime().Ticks - TIME_STAMP_FROM_0_TO_1970) / 10000;
    }
    #endregion

    public static DateTime GetCSharpTime(long timestamp)
    {
        timestamp *= 10000;
        timestamp += TIME_STAMP_FROM_0_TO_1970;
        DateTime dt = new DateTime(timestamp, DateTimeKind.Utc);
        return dt.ToLocalTime();
    }

    //public static DateTime GetCSharpTime(long timestamp)
    //{
    //    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
    //    TimeSpan toNow = new TimeSpan(timestamp);
    //    return dtStart.Add(toNow);
    //}


    public static string GetLocalTime()
    {
        return DateTime.Now.ToString("HH:mm");
    }
}
