//===================================================
//Author      : DRB
//CreateTime  ：5/8/2017 3:26:35 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMatchWaitView : UIWindowViewBase 
{
    [SerializeField]
    private Text m_TextWaitCount;
    [SerializeField]
    private Text m_TextRiseCount;


    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, ModelDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(NotificationDefine.ON_WAIT_COUNT_CHANGED, OnWaitCountChanged);
        return dic;
    }

    /// <summary>
    /// 等待数量更新
    /// </summary>
    /// <param name="obj"></param>
    private void OnWaitCountChanged(TransferData obj)
    {
        int waitCount = obj.GetValue<int>("WaitCount");

        if (waitCount == 0)
        {
            Close();
            return;
        }
        m_TextWaitCount.SafeSetText(waitCount.ToString());

    }

    public void SetUI(int waitCount,int riseCount)
    {
        m_TextWaitCount.SafeSetText(waitCount.ToString());
        m_TextRiseCount.SafeSetText(riseCount.ToString());
    }

}
