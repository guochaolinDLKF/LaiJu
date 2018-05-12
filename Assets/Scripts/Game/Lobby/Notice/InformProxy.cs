//===================================================
//Author      : DRB
//CreateTime  ：5/12/2017 11:15:04 AM
//Description ：
//===================================================
using System.Collections.Generic;
using UnityEngine;


public class InformProxy : ProxyBase<InformProxy>
{

    public int UnreadMailCount { get; private set; }

    public List<MailEntity> AllMails { get; private set; }
  
    public string Notice { get; private set; }
   
    public void SetUnreadMailCount(int mailCount)
    {
        if (UnreadMailCount == mailCount) return;
        UnreadMailCount = mailCount;
        TransferData data = new TransferData();
        data.SetValue("MailCount", UnreadMailCount);
        SendNotification("OnMailCountChange",data);
    }

    public void SetMail(List<MailEntity> lst)
    {
        AllMails = lst;
        SetUnreadMailCount(0);
    }

    public void SetNotice(string notice)
    {
        Notice = notice;
    }

}
