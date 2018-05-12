//===================================================
//Author      : DRB
//CreateTime  ：3/8/2017 10:48:01 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordEntity
{
    public string password;

    public PasswordEntity() { }

    public PasswordEntity(string password)
    {
        this.password = password;
    }
}

public class AccountEntity
{
    public int passportId;

    public string token;

    public string nickname;

    public int gender;

    public string avatar;

    public int cards;

    public bool shared;

    public string phone;

    public string ipaddr;

    public int identity;

    public int urlbind;

    public int codebind;

    public int first_pay;
}