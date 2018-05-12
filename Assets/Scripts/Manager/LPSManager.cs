//===================================================
//Author      : DRB
//CreateTime  ：7/14/2017 8:54:51 PM
//Description ：
//===================================================
using System;
using System.Collections;
using UnityEngine;


public class LPSManager : SingletonMono<LPSManager> 
{

    public float Latitude;

    public float Longitude;


    public void StartGPS(Action<float, float> onStart)
    {
        StartCoroutine(StartGPSCoroutine(onStart));
    }

    private IEnumerator StartGPSCoroutine(Action<float, float> onStart)
    {
        if (!Input.location.isEnabledByUser)
        {
            if (onStart != null)
            {
                onStart(0,0);
            }
            yield break;
        }
        
        Input.location.Start(10.0f, 10.0f);

        float maxWait = 10f;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            maxWait -= Time.deltaTime;
        }

        if (maxWait <= 0f)
        {
            Debug.Log("未获取到经纬度信息");
            if (onStart != null)
            {
                onStart(0f,0f);
            }
            yield break;
        }

        Latitude = Input.location.lastData.latitude;
        Longitude = Input.location.lastData.longitude;

        Debug.Log("纬度" + Latitude);
        Debug.Log("经度" + Longitude);

        if (onStart != null)
        {
            onStart(Input.location.lastData.latitude, Input.location.lastData.longitude);
        }

        Input.location.Stop();
    }
}
