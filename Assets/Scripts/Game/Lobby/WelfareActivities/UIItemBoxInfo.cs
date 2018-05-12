//===================================================
//Author      : DRB
//CreateTime  ：12/2/2017 11:12:10 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemBoxInfo : UIViewBase
{
    private int index;

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btn_box":
                SendNotification("btn_box", index);
                break;
            default:
                break;
        }
    }

    public void SetBoxIndex(int index)
    {
        this.index = index;
    }

    public int GetBoxIndex()
    {
        return index;
    }
}
