//===================================================
//Author      : DRB
//CreateTime  ：5/4/2017 8:00:18 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMatchTipView : UIWindowViewBase 
{

    [SerializeField]
    private Text m_TextCurrentPlayerCount;
    [SerializeField]
    private Text m_TextMaxPlayerCount;
    [SerializeField]
    private Text m_TextLessPlayerCount;

    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(NotificationDefine.ON_APPLY_INFO_CHANGED,OnApplyInfoChanged);
        return dic;
    }

    private void OnApplyInfoChanged(TransferData obj)
    {
        int currentPlayerCount = obj.GetValue<int>("CurrentPlayerCount");
        int maxPlayerCount = obj.GetValue<int>("MaxPlayerCount");
        SetUI(currentPlayerCount, maxPlayerCount);
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case ConstDefine.BtnMatchTipViewBowout:
                SendNotification(ConstDefine.BtnMatchTipViewBowout);
                break;
        }
    }
    public void SetUI(int currentPlayerCount,int maxPlayerCount)
    {
        m_TextCurrentPlayerCount.SafeSetText(currentPlayerCount.ToString());
        m_TextMaxPlayerCount.SafeSetText(maxPlayerCount.ToString());
        m_TextLessPlayerCount.SafeSetText((maxPlayerCount - currentPlayerCount).ToString());
    }

}
