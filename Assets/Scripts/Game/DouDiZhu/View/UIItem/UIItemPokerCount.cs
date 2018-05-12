//===================================================
//Author      : DRB
//CreateTime  ：12/19/2017 5:20:52 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemPokerCount : MonoBehaviour {
    private Text pokerCountText;
	void Start () {
        pokerCountText = GetComponent<Text>();

    }
	void Update () {
		
	}
    public void SetNumber(string PokerCount)
    {
        pokerCountText.SafeSetText(PokerCount);
    }
}
