//===================================================
//Author      : DRB
//CreateTime  ：12/6/2017 10:17:11 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlText : MonoBehaviour {
    
	void Start () {
		
	}
	
	void Update () {
        if (Input.GetKeyUp(KeyCode.A))
        {
            AudioTest();
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            ShareTest();
        }
	}
    void AudioTest()
    {
        UIViewManager.Instance.OpenWindow(UIWindowType.AudioSetting);
    }
    void ShareTest()
    {
        UIViewManager.Instance.OpenWindow(UIWindowType.Share);
    }
}
