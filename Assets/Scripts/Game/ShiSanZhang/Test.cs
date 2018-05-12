//===================================================
//Author      : DRB
//CreateTime  ：12/2/2017 5:44:28 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using ShiSanZhang;


public class Test : MonoBehaviour {

    public Transform[] handPoker;
    public GameObject[] pokerObj;
    public Transform Hand;

    public Transform hand1;

    private Tween dealPoker;

    public Image tupian;



    public UIItemPoker_ShiSanZhang[] m_Images;
    public UIItemPoker_ShiSanZhang[] m_Images1;
    public UIItemPoker_ShiSanZhang[] m_Images2;
    public Transform[] ve3;

    void Start()
    {
        for (int i = 0; i < m_Images.Length; i++)
        {
            float a = ((10f * (m_Images.Length - 1)) / 2) - i * 10f;
            m_Images[i].gameObject.transform.RotateAround(ve3[0].position, Vector3.forward, a);
        }
        for (int i = 0; i < m_Images1.Length; i++)
        {
            float a = ((10f * (m_Images1.Length - 1)) / 2) - i * 10f;
            m_Images1[i].gameObject.transform.RotateAround(ve3[1].position, Vector3.forward, a);
        }
        for (int i = 0; i < m_Images2.Length; i++)
        {
            float a = ((10f * (m_Images2.Length - 1)) / 2) - i * 10f;
            m_Images2[i].gameObject.transform.RotateAround(ve3[2].position, Vector3.forward, a);
        }
        //Ceshi1();
    }

    public void CeShi()
    {
        for (int i = 0; i < pokerObj.Length; i++)
        {
            pokerObj[i].transform.DOMove(handPoker[i].position, 0.3f).OnComplete(OnCompleteDoTween);
        }
    }

    public void OnCompleteDoTween()
    {
        for (int i = 0; i < pokerObj.Length; i++)
        {
            pokerObj[i].SetParent(Hand);
            pokerObj[i].gameObject.GetComponent<ShiSanZhangPokerCtrl>().FlipCardsForward();
        }


    }

    public void Onclick1()
    {
        pokerObj[0].SetParent(hand1);
        pokerObj[0].transform.localPosition = Vector3.zero;
        pokerObj[0].transform.localScale = Vector3.one;
    }

    public void OnClick()
    {
        CeShi();
    }


    public void Ceshi1()
    {
        //tupian.material.DOColor(new Color(0, 255f, 255f,255f), 10f);
        tupian.DOColor(new Color(255f,255f,255f,0f),10);
        //tupian.gameObject.transform.DOScale(new Vector3(0,0,0),10f);
    }
}
