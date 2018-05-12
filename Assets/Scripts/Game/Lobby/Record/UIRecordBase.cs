//===================================================
//Author      : DRB
//CreateTime  ：11/7/2017 2:14:22 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIRecordBase : UIItemBase 
{

    [SerializeField]
    protected Transform m_RecordContainer;
    [SerializeField]
    protected InputField m_inputBattleId;
    [SerializeField]
    protected Button m_btnSeeRecord;


    public string GameType;

    protected abstract string RecordPrefabName { get; }


    private List<UIItemRecordBase> m_Cache = new List<UIItemRecordBase>();

    protected override void OnAwake()
    {
        base.OnAwake();


        if (m_btnSeeRecord != null)
        {
            m_btnSeeRecord.onClick.AddListener(OnSeeRecordClick);
        }
    }

    private void OnSeeRecordClick()
    {
        SendNotification("btnRecordViewSeeRecord", m_inputBattleId.text.ToInt());
    }

    public virtual void ShowRecord(TransferData data)
    {
        for (int i = 0; i < m_Cache.Count; ++i)
        {
            UIPoolManager.Instance.Despawn(m_Cache[i].transform);
        }
        m_Cache.Clear();
        List<TransferData> lstRecord = data.GetValue<List<TransferData>>("Record");
        for (int i = 0; i < lstRecord.Count; ++i)
        {
            UIItemRecordBase item = UIPoolManager.Instance.Spawn(RecordPrefabName).GetComponent<UIItemRecordBase>();
            item.gameObject.SetParent(m_RecordContainer);
            item.SetUI(lstRecord[i]);
            m_Cache.Add(item);
        }
    }


}
