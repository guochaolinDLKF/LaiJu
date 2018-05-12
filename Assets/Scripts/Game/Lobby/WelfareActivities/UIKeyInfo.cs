//===================================================
//Author      : DRB
//CreateTime  ：12/2/2017 2:15:24 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIKeyInfo : MonoBehaviour
{

    private int keyCount;

    public void SetKeyCount(int keyCount)
    {
        this.keyCount = keyCount;
    }
    public int GetKeyCount()
    {
        return keyCount;
    }
}
