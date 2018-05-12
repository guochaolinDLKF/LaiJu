//===================================================
//Author      : DRB
//CreateTime  ：11/7/2017 3:24:52 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIRecordDetailBase : UIItemBase 
{
    [SerializeField]
    protected Transform m_RecordDetailContainer;
    [SerializeField]
    protected Button m_btnBack;

    public string GameType;

    protected abstract string RecordDetailPrefabName { get; }

    private List<UIItemRecordDetailBase> m_Cache = new List<UIItemRecordDetailBase>();

    protected Action m_OnCloseAction;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_btnBack.onClick.AddListener(OnBackClick);
    }

    protected virtual void OnBackClick()
    {
        gameObject.SetActive(false);
        if (m_OnCloseAction != null) m_OnCloseAction();
    }

    public virtual void ShowRecordDetail(TransferData data)
    {
        for (int i = 0; i < m_Cache.Count; ++i)
        {
            UIPoolManager.Instance.Despawn(m_Cache[i].transform);
        }
        m_Cache.Clear();
        List<TransferData> lstRecord = data.GetValue<List<TransferData>>("RecordDetail");
        for (int i = 0; i < lstRecord.Count; ++i)
        {
            UIItemRecordDetailBase item = UIPoolManager.Instance.Spawn(RecordDetailPrefabName).GetComponent<UIItemRecordDetailBase>();
            item.gameObject.SetParent(m_RecordDetailContainer);
            item.SetUI(lstRecord[i]);
            m_Cache.Add(item);
        }
    }

    public void SetCloseAction(Action onCloseAction)
    {
        if (onCloseAction != null) m_OnCloseAction = onCloseAction;
    }
}
