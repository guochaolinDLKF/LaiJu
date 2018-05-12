//===================================================
//Author      : DRB
//CreateTime  ：8/30/2017 12:02:47 PM
//Description ：
//===================================================
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIDisbandView : UIWindowViewBase 
{
    [SerializeField]
    private Transform m_Container;
    [SerializeField]
    private Button m_BtnAgree;
    [SerializeField]
    private Button m_BtnRefuse;
    [SerializeField]
    private Text m_ImgWaitResult;
    [SerializeField]
    private Text m_TxtApplicant;
    [SerializeField]
    private Text m_CountDown;
    [SerializeField]
    private Text m_TxtMaxCountDown;

    private float m_Timer;
    private bool m_isCountDown;


    private List<UIItemDisbandSeat> m_Cache = new List<UIItemDisbandSeat>();

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnDisbandViewAgree":
                SendNotification("btnDisbandViewAgree");
                break;
            case "btnDisbandViewRefuse":
                SendNotification("btnDisbandViewRefuse");
                break;
        }
    }

    private void Update()
    {
        if (m_isCountDown)
        {
            m_Timer -= Time.deltaTime;
            if (m_Timer <= 0)
            {
                m_isCountDown = false;
                return;
            }
            m_CountDown.SafeSetText(m_Timer.ToString("0"));
        }
    }

    public void SetUI<T>(List<T> lst,SeatEntityBase playerSeat,float countDown,int maxCountDown) where T : SeatEntityBase
    {
        if (lst == null) return;
        if (playerSeat == null) return;

        if (playerSeat.DisbandState == DisbandState.Wait)
        {
            m_BtnAgree.gameObject.SetActive(true);
            m_BtnRefuse.gameObject.SetActive(true);
            m_ImgWaitResult.gameObject.SetActive(false);
        }
        else
        {
            m_BtnAgree.gameObject.SetActive(false);
            m_BtnRefuse.gameObject.SetActive(false);
            m_ImgWaitResult.gameObject.SetActive(true);
        }

        for (int i = 0; i < lst.Count; ++i)
        {
            Debug.Log(lst[i].DisbandState);
            if (lst[i].DisbandState == DisbandState.Apply)
            {
                m_TxtApplicant.SafeSetText(lst[i].Nickname);
            }
        }

        m_TxtMaxCountDown.SafeSetText(maxCountDown.ToString() + "秒");

        for (int i = 0; i < m_Cache.Count; ++i)
        {
            m_Cache[i].gameObject.SetActive(false);
        }

        UIViewManager.Instance.LoadItemAsync("UIItemDisbandSeat",(GameObject go)=> 
        {
            for (int i = 0; i < lst.Count; ++i)
            {
                UIItemDisbandSeat item = null;
                if (i < m_Cache.Count)
                {
                    item = m_Cache[i];
                    item.gameObject.SetActive(true);
                }
                else
                {
                    go = Instantiate(go);
                    go.SetParent(m_Container, true);
                    item = go.GetComponent<UIItemDisbandSeat>();
                    m_Cache.Add(item);
                }

                item.SetUI(lst[i]);
            }
        });

        if (countDown != 0f)
        {
            m_Timer = countDown;
            m_isCountDown = true;
            m_CountDown.SafeSetText(countDown.ToString("0"));
        }
    }
}
