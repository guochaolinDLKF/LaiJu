//===================================================
//Author      : DRB
//CreateTime  ：7/4/2017 7:35:17 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSeatMask : MonoBehaviour {

    private static ItemSeatMask instence;

    public static ItemSeatMask Instence
    {
        get
        {
            if (instence==null)
            {
                instence = new ItemSeatMask();
            }
            return instence;
        }     
    }
    void Awake()
    {
        instence = this;
    }

    public void OnBtnClik()
    {
        if (gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }                      
        RoomZhaJHProxy.Instance.DicEvent("isbool",false, ZhaJHMethodname.OnZJHHidBP);
        RoomZhaJHProxy.Instance.DicEvent("isbool", false, ZhaJHMethodname.OnZJHNoMaskP);
    }
}
