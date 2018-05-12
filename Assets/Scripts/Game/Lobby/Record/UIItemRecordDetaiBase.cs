//===================================================
//Author      : DRB
//CreateTime  ：11/7/2017 3:15:28 PM
//Description ：
//===================================================
using UnityEngine;
using UnityEngine.UI;

public abstract class UIItemRecordDetailBase : UIItemBase 
{
    [SerializeField]
    protected Button m_btnPlayBack;

    protected int m_RecordId;

    protected override void OnAwake()
    {
        base.OnAwake();
        if (m_btnPlayBack != null)
        {
            m_btnPlayBack.onClick.AddListener(OnPlayBackClick);
        }
    }

    private void OnPlayBackClick()
    {
        SendNotification("btnRecordViewPlayBack", m_RecordId);
    }


    public virtual void SetUI(TransferData data)
    {
        m_RecordId = data.GetValue<int>("RecordId");
    }
}
