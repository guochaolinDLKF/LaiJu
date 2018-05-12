//===================================================
//Author      : DRB
//CreateTime  ：7/13/2017 9:01:55 PM
//Description ：
//===================================================
using UnityEngine;


public static class LPSUtil
{
    //public static float CalculateDistance(float latitude1,float longitude1,float latitude2,float longitude2)
    //{
    //    float c = Mathf.Sin(latitude1) * Mathf.Sin(latitude2) * Mathf.Cos(longitude1 - longitude2) + Mathf.Cos(latitude1) * Mathf.Cos(latitude2);
    //    float distance = 6371.004f * Mathf.Acos(c) * Mathf.PI / 180;
    //    return distance;
    //}

    public static float CalculateDistance(float lat1, float lng1, float lat2, float lng2)
    {
        float a, b;
        const float R = 6378.137f; //地球半径

        lat1 = lat1 * Mathf.PI / 180.0f;
        lat2 = lat2 * Mathf.PI / 180.0f;


        a = (lat1 - lat2);
        b = (lng1 - lng2) * Mathf.PI / 180.0f;

        float d;
        float sa2, sb2;
        sa2 = Mathf.Sin(a / 2.0f);
        sb2 = Mathf.Sin(b / 2.0f);

        d = 2 * R * Mathf.Asin(Mathf.Sqrt(sa2 * sa2 + Mathf.Cos(lat1) * Mathf.Cos(lat2) * sb2 * sb2));

        return d;
    }
}
