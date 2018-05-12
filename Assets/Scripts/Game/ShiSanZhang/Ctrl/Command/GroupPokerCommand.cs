//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 5:52:43 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShiSanZhang
{
    public class GroupPokerCommand : IGameCommand
    {


        public void Execute()
        {
            RoomShiSanZhangProxy.Instance.GroupPokerProxy();
        }

        public void Revoke()
        {
            throw new NotImplementedException();
        }
    }
}
