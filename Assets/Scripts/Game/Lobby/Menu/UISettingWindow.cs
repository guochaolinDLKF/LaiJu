//===================================================
//Author      : DRB
//CreateTime  ：4/12/2017 2:23:59 PM
//Description ：
//===================================================
using UnityEngine;

public class UISettingWindow : UIWindowViewBase
{

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case ConstDefine.BtnSettingViewDisband:
                SendNotification(ConstDefine.BtnSettingViewDisband);
                break;
            case ConstDefine.BtnSettingViewQuit:
                SendNotification(ConstDefine.BtnSettingViewQuit);
                break;
            case ConstDefine.BtnSettingViewShare:
                SendNotification(ConstDefine.BtnSettingViewShare);
                break;
            case ConstDefine.BtnSettingViewRule:
                SendNotification(ConstDefine.BtnSettingViewRule);
                break;
            case ConstDefine.BtnSettingViewAudio:
                SendNotification(ConstDefine.BtnSettingViewAudio);
                break;
            case ConstDefine.BtnSettingViewBind:
                SendNotification(ConstDefine.BtnSettingViewBind);
                break;
            case ConstDefine.BtnSettingViewMail:
                SendNotification(ConstDefine.BtnSettingViewMail);
                break;
            case ConstDefine.BtnSettingViewLeave:
                SendNotification(ConstDefine.BtnSettingViewLeave);
                break;
        }
        Close();
    }
}