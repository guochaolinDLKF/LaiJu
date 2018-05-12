//===================================================
//Author      : DRB
//CreateTime  ：6/15/2017 12:22:11 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZhaJHTest : MonoBehaviour {

    private float delay = 0.5f;
    private float firstClickTime = 0;
    private bool isTime = false;
	void Start () {
		
	}
	
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.Y))
        {
            GameCtrl.Instance.CreateRoom(21);
        }
    }

    //void OnGUI()
    //{
    //    Event e = Event.current;
    //    if (e.isMouse&&(e.clickCount==2))
    //    {
    //        GameManager.Instance.CreateRoom(7);
    //    }
    //}
}
