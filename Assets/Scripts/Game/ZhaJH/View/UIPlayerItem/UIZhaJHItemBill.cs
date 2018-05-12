//===================================================
//Author      : DRB
//CreateTime  ：7/27/2017 12:14:20 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhaJh;
using UnityEngine.UI;

public class UIZhaJHItemBill : MonoBehaviour {
    [SerializeField]
    private Text textID;
    [SerializeField]
    private Text textGold;
    [SerializeField]
    private RawImage touImage;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetUI(int id,float gold,string avatar)
    {
        TextureManager.Instance.LoadHead(avatar, OnAvatarLoadCallBack);
        textID.SafeSetText("ID:"+ id.ToString());
        textGold.SafeSetText(gold.ToString("0.00"));        
    }

    private void OnAvatarLoadCallBack(Texture2D tex)
    {
        if (touImage!=null)
        {
            touImage.texture = tex;
        }       
    }
}
