//===================================================
//Author      : DRB
//CreateTime  ：12/2/2017 1:57:21 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGiftInfo : MonoBehaviour
{
    [SerializeField]
    private int index;
    [SerializeField]
    private GiftType giftType;
    [SerializeField]
    private string name;
    [SerializeField]
    private string img_url;

    public void SetUIGiftIndex(int index,string name, GiftType giftType,string url)
    {
        this.index = index;
        this.name = name;
        this.giftType = giftType;
        img_url = url;
    }

    public int GetUIGiftIndex()
    {
        return index;
    }
    //public void SetUIGiftType(GiftType giftType)
    //{
    //    this.giftType = giftType;
    //}

    public GiftType GetUIGiftType()
    {
        return giftType;
    }
    public string GetName()
    {
        return name;
    }
    public string GetURL()
    {
        return img_url;
    }
}
