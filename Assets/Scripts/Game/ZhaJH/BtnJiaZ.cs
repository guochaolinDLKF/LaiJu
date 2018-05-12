//===================================================
//Author      : DRB
//CreateTime  ：7/4/2017 9:47:10 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnJiaZ : MonoBehaviour {
    [SerializeField]
    private  Image jiazhuMask;  //加注遮罩
    [SerializeField]
    private Image jiazhubgGJ1;
    [SerializeField]
    private Image jiazhubgGJ2;
    [SerializeField]
    private Image jiazhuBg;   
    [SerializeField]
    private Image xuepinBg;//血拼

    //void Awake()
    //{
    //    if (jiazhuBg != null) jiazhuBg.gameObject.SetActive(false);
    //    if (jiazhubgGJ1 != null) jiazhubgGJ1.gameObject.SetActive(false);
    //    if (jiazhubgGJ2 != null) jiazhubgGJ2.gameObject.SetActive(false);
    //    if (xuepinBg != null) xuepinBg.gameObject.SetActive(false);
    //}

    void Start () {
		
	}
	
	
	void Update () {
		
	}
    public void BtnOnClik()
    {
        if (jiazhuMask != null) jiazhuMask.gameObject.SetActive(false);
        if (jiazhuBg != null) jiazhuBg.gameObject.SetActive(false);
        if (jiazhubgGJ1 != null) jiazhubgGJ1.gameObject.SetActive(false);
        if (jiazhubgGJ2 != null) jiazhubgGJ2.gameObject.SetActive(false);
        if (xuepinBg != null) xuepinBg.gameObject.SetActive(false);               
    }
}
