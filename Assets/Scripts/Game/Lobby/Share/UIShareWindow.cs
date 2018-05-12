//===================================================
//Author      : DRB
//CreateTime  ：4/7/2017 6:02:25 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShareWindow : UIWindowViewBase
{
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case ConstDefine.BtnShareFriend:
                SendNotification(ConstDefine.BtnShareFriend);
                break;
            case ConstDefine.BtnShareMoments:
                SendNotification(ConstDefine.BtnShareMoments);
                break;
            case ConstDefine.BtnShareViewBack:
                SendNotification(ConstDefine.BtnShareViewBack);
                break;
        }
    }

}
