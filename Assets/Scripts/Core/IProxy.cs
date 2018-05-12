//===================================================
//Author      : DRB
//CreateTime  ：5/17/2017 3:23:47 PM
//Description ：
//===================================================
using UnityEngine;


public interface IProxy 
{
    void SendNotification(string notificationName,TransferData data);
}
