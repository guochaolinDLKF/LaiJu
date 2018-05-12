//===================================================
//Author      : DRB
//CreateTime  ：4/7/2017 4:53:18 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class  UIRecordView : UIWindowViewBase
{
    [SerializeField]
    private UIRecordBase[] m_ArrRecord;
    [SerializeField]
    private UIRecordDetailBase[] m_ArrRecordDetail;





    protected override void OnAwake()
    {
        base.OnAwake();
        m_ArrRecordDetail[0].SetCloseAction(
            () => {
                m_ArrRecord[0].gameObject.SetActive(true);
            }
            );
   
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        //for (int i = 0; i < m_btnGame.Length; ++i)
        //{
        //    if (go == m_btnGame[i].gameObject)
        //    {
        //        SendNotification("btnRecordViewGame", go.name);
        //        break;
        //    }
        //}
    }


    public void ShowRecord(TransferData data)
    {
        string gameType = data.GetValue<string>("GameType");
        m_ArrRecord[0].gameObject.SetActive(true);
        m_ArrRecord[0].ShowRecord(data);
        //for (int i = 0; i < m_ArrRecord.Length; ++i)
        //{
        //    if (m_ArrRecord[i].GameType.Equals(gameType))
        //    {
        //        m_ArrRecord[i].gameObject.SetActive(true);
        //        m_ArrRecord[i].ShowRecord(data);
        //    }
        //    else
        //    {
        //        m_ArrRecord[i].gameObject.SetActive(false);
        //    }
        //}
        for (int i = 0; i < m_ArrRecordDetail.Length; ++i)
        {
            m_ArrRecordDetail[i].gameObject.SetActive(false);
        }
    }

    public void ShowRecordDetail(TransferData data)
    {
        string gameType = data.GetValue<string>("GameType");
        m_ArrRecordDetail[0].gameObject.SetActive(true);
        m_ArrRecordDetail[0].ShowRecordDetail(data);
        //for (int i = 0; i < m_ArrRecordDetail.Length; ++i)
        //{
        //    if (m_ArrRecordDetail[i].GameType.Equals(gameType))
        //    {
        //        m_ArrRecordDetail[i].gameObject.SetActive(true);
        //        m_ArrRecordDetail[i].ShowRecordDetail(data);
        //    }
        //    else
        //    {
        //        m_ArrRecordDetail[i].gameObject.SetActive(false);
        //    }
        //}
        for (int i = 0; i < m_ArrRecord.Length; ++i)
        {
            m_ArrRecord[i].gameObject.SetActive(false);
        }

    }



}
