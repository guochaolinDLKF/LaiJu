//===================================================
//Author      : DRB
//CreateTime  ：5/9/2017 4:18:40 PM
//Description ：邮件实体
//===================================================
using System.Collections.Generic;
using UnityEngine;


public class MailEntity 
{
    public int id;

    public int type;

    public string msg;

    public int playerId;

    public string nickname;

    public string time;

    public bool isView;

    public List<Attach> attach;

}

public class Attach
{
    public int itemId;

    public int amount;
}

