//===================================================
//Author      : DRB
//CreateTime  ：11/7/2017 2:38:54 PM
//Description ：
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIItemRecordBase : UIItemBase 
{
    [SerializeField]
    protected Button m_btnSeeDetail;

    protected int m_BattleId;

    protected override void OnAwake()
    {
        base.OnAwake();
        if (m_btnSeeDetail != null)
        {
            m_btnSeeDetail.onClick.AddListener(OnSeeDetailClick);
        }
    }

    private void OnSeeDetailClick()
    {       
        SendNotification("btnRecordViewDetail", m_BattleId);
    }

    public virtual void SetUI(TransferData data)
    {
        m_BattleId = data.GetValue<int>("BattleId");
    }
}
