//===================================================
//Author      : WZQ
//CreateTime  ：7/25/2017 8:21:37 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRetroactionView : UIWindowViewBase
{


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btn_retroaction":
                //SendNotification("btn_retroaction", m_InputInviteCode.text.ToInt());
                break;
        }
    }


}
