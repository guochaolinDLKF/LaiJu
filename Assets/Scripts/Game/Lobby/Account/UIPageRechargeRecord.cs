//===================================================
//Author      : DRB
//CreateTime  ：6/9/2017 11:35:10 AM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPageRechargeRecord : UIItemBase 
{
    [SerializeField]
    private Text m_TextAmount;
    [SerializeField]
    private Transform m_Container;
    [SerializeField]
    private Button m_ButtonAllDay;//全部
    [SerializeField]
    private Button m_ButtonToday;//今天
    [SerializeField]
    private Button m_ButtonYesterday;//昨天
    [SerializeField]
    private Button m_ButtonCurrentMonth;//本月
    [SerializeField]
    private Button m_ButtonPreviourMonth;//上月

    public InputField InputUserId;
    private List<UIItemRechargeRecord> m_List = new List<UIItemRechargeRecord>();
    [HideInInspector]
    public string StartDateTime = "1970-01-01";
    [HideInInspector]
    public string EndDateTime = "2999-12-31";


    public Action OnDateClick;
    private void Start()
    {
        m_ButtonAllDay.onClick.AddListener(OnBtnAllDayClick);
        m_ButtonToday.onClick.AddListener(OnBtnTodayClick);
        m_ButtonYesterday.onClick.AddListener(OnBtnYesterdayClick);
        m_ButtonCurrentMonth.onClick.AddListener(OnBtnCurrentMonthClick);
        m_ButtonPreviourMonth.onClick.AddListener(OnBtnPreviourMonthClick);
    }

    private void OnBtnAllDayClick()
    {
        StartDateTime = DateTime.MinValue.ToString(ConstDefine.DATE_TIME_FORMAT);
        StartDateTime = "1970-1-1";
        EndDateTime = DateTime.MaxValue.ToString(ConstDefine.DATE_TIME_FORMAT);
        EndDateTime = "2200-12-31";

        SendNotification("btnPlayerInfoViewQuery");
    }

    private void OnBtnTodayClick()
    {
        StartDateTime = DateTime.Now.ToString(ConstDefine.DATE_TIME_FORMAT);
        EndDateTime = StartDateTime;

        SendNotification("btnPlayerInfoViewQuery");
    }

    private void OnBtnYesterdayClick()
    {
        StartDateTime = DateTime.Now.AddDays(-1).ToString(ConstDefine.DATE_TIME_FORMAT);
        EndDateTime = StartDateTime;

        SendNotification("btnPlayerInfoViewQuery");
    }

    private void OnBtnCurrentMonthClick()
    {
        DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        StartDateTime = currentMonth.ToString(ConstDefine.DATE_TIME_FORMAT);
        EndDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString(ConstDefine.DATE_TIME_FORMAT);

        SendNotification("btnPlayerInfoViewQuery");
    }

    private void OnBtnPreviourMonthClick()
    {
        DateTime previourMonth = DateTime.Now.AddMonths(-1);
        StartDateTime = new DateTime(previourMonth.Year, previourMonth.Month,1).ToString(ConstDefine.DATE_TIME_FORMAT);
        EndDateTime = new DateTime(previourMonth.Year, previourMonth.Month, DateTime.DaysInMonth(previourMonth.Year, previourMonth.Month)).ToString(ConstDefine.DATE_TIME_FORMAT);

        SendNotification("btnPlayerInfoViewQuery");
    }

    public void SetUI(List<RechargeRecordEntity> lst)
    {
        for (int i = 0; i < m_List.Count; ++i)
        {
            m_List[i].gameObject.SetActive(false);
        }
        if (lst == null || lst.Count == 0)
        {
            InputUserId.text = string.Empty;
            m_TextAmount.SafeSetText(string.Empty);
            return;
        }
        m_TextAmount.SafeSetText(string.Format("合计：{0}",lst.Count.ToString()));
        for (int i = 0; i < lst.Count; ++i)
        {
            UIItemRechargeRecord item = null;
            RechargeRecordEntity entity = lst[i];
            if (i < m_List.Count)
            {
                item = m_List[i];
                item.gameObject.SetActive(true);
            }
            else
            {
                UIViewManager.Instance.LoadItemAsync("UIItemRechargeRecord",(GameObject go)=> 
                {
                    go = Instantiate(go);
                    go.SetParent(m_Container);
                    item = go.GetComponent<UIItemRechargeRecord>();
                    m_List.Add(item);
                });
            }
            item.SetUI(entity.add_time,entity.nickname,entity.toId,entity.amount,entity.old_amount);
        }
    }
}
