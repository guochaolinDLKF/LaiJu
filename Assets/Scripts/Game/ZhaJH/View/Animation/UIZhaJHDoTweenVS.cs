//===================================================
//Author      : DRB
//CreateTime  ：7/20/2017 8:45:13 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIZhaJHDoTweenVS : MonoBehaviour {

   [SerializeField]
    private GameObject vS_V;
    [SerializeField]
    private GameObject vS_S;
    [SerializeField]
    private GameObject vsGuang;

    private Vector3 VTran;
    private Vector3 STran;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //public void DoTweenPlay()
    //{
    //    DOTweenAnimation[] V= vS_V.GetComponents<DOTweenAnimation>();
    //    DOTweenAnimation[] S = vS_S.GetComponents<DOTweenAnimation>();        
    //    for (int i = 0; i < V.Length; i++)
    //    {            
    //        V[i].DOPlayForward();
    //        S[i].DOPlayForward();
    //    }

    //}

    public void DoTweenPlay()
    {
        StartCoroutine("VS");       
    }

    public void DoTweenEnde()
    {        
        vS_V.transform.localPosition = VTran;
        vS_S.transform.localPosition = STran;
        vS_V.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
        vS_S.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        vsGuang.SetActive(false);
    }

    IEnumerator VS()
    {
        yield return null;
        VTran = vS_V.transform.localPosition;
        STran = vS_S.transform.localPosition;        
        Tweener V = vS_V.transform.DOLocalMove(new Vector3(-51, 38, 0), 0.4f);
        vS_V.transform.DOScale(Vector3.one,0.4f);
        Tweener S = vS_S.transform.DOLocalMove(new Vector3(46, 16, 0), 0.4f);
        vS_S.transform.DOScale(Vector3.one, 0.4f);
        V.OnComplete(()=> 
        {
            vsGuang.SetActive(true);
        });
    }
}
