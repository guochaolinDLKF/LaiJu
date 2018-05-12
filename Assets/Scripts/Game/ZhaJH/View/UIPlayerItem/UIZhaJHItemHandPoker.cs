//===================================================
//Author      : CZH
//CreateTime  ：6/30/2017 7:08:41 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIZhaJHItemHandPoker : MonoBehaviour {

    [SerializeField]
    private Image m_image;


    private void Awake()
    {

    }
    public void SetUI(Sprite sprite)
    {
        if (m_image!=null)
        {
            m_image.sprite = sprite;
        }
    }
    
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
