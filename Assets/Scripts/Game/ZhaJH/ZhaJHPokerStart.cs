//===================================================
//Author      : DRB
//CreateTime  ：7/8/2017 3:24:41 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZhaJHPokerStart : MonoBehaviour {

    private static ZhaJHPokerStart instance;
    [SerializeField]
    private GameObject clone;
    [HideInInspector]
    public bool isPoker;
    public static ZhaJHPokerStart Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ZhaJHPokerStart();
            }
            return instance;
        }
        
    }
    void Awake()
    {
        instance = this;
    }

    private int a ;

    public void ClonePoker()
    {
        for (int i = 0; i < 21; i++)
        {
            GameObject go = Instantiate(clone,this.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;          
            go.SetActive(true);
        }       
        StartCoroutine("ClonePokerTime");
        
    }

    IEnumerator ClonePokerTime()
    {
        a = 0;
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).gameObject.activeSelf)
            {
                GameObject go = Instantiate(ZJHPrefabManager.Instance.LoadPokerFP(null, null, "normalpoker"));
                go.transform.SetParent(this.gameObject.transform.GetChild(i).transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
                this.transform.GetChild(i).transform.localPosition = new Vector3(348 - a, 0, 0);
                a += 30;               
                yield return new WaitForSeconds(0.01f);
            }
        }
        yield return new WaitForSeconds(0.2f);
        isPoker = false;

       
    }
   
}
