//===================================================
//Author      : DRB
//CreateTime  ：5/17/2017 3:23:16 PM
//Description ：
//===================================================
using System;
using UnityEngine;


public class ProxyBase<T> : Singleton<T>, IProxy where T:new()
{
    public virtual void SendNotification(string notificationName, TransferData data)
    {
        ModelDispatcher.Instance.Dispatch(notificationName,data);
    }
}
