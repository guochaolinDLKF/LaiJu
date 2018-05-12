//===================================================
//Author      : DRB
//CreateTime  ：1/9/2018 3:25:09 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
    [SerializeField]
    private float shakeFrequency;
    [SerializeField]
    private float shakeTime;

    private Vector3 StartPos;
    [SerializeField]
    private float timer;
    [SerializeField]
    private float timerTotal;

    void Start () {
        StartPos = new Vector3(0, 1, -10);
    }
    public void StartShake(float shakeTotalTime)
    {
        shakeTime = shakeTotalTime;
    }
	
	void Update () {
        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;
            float ran = Random.Range(-shakeFrequency,shakeFrequency);
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                transform.localPosition = StartPos + new Vector3(ran, ran, ran);
                timer = timerTotal;
            }
        }
        else
        {
            transform.localPosition = StartPos;
        }
	}
}
