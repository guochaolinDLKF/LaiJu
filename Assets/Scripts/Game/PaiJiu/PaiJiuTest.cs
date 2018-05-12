//===================================================
//Author      : DRB
//CreateTime  ：7/5/2017 11:57:03 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PaiJiuTest : MonoBehaviour {


    //DOTweenVisualManager manager;
    // Use this for initialization
    private Tweener m_bankerAni;
    void Start () {
        //manager = GetComponent<DOTweenVisualManager>();
        
        m_bankerAni = transform.DOScale(3, 0.2f).From().SetAutoKill(false).Pause();
        transform.localScale = Vector3.one;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.A))
        {
          
            m_bankerAni.Restart();
            //m_bankerAni.Restart();
        }


        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    GameManager.Instance.CreateRoom(9);
        //}


    }
}
