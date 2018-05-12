//===================================================
//Author      : DRB
//CreateTime  ：9/8/2017 5:32:21 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetDice : MonoBehaviour {

    [SerializeField]
    private Image diceImage;
	
	void Start () {
		
	}
	
	
	void Update () {
		
	}
    public void SetDiceImage(Sprite sprite)
    {
        diceImage.sprite = sprite;
    }
}
