//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 8:01:21 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShiSanZhang
{
    public class CreateRoomCommand : IGameCommand
    {

        private RoomEntity m_Room;

        public CreateRoomCommand(RoomEntity room)
        {
            m_Room = room;
        }

        public void Execute()
        {           
            //RoomShiSanZhangProxy.Instance.InitRoom(m_Room);
        }

        public void Revoke()
        {
            throw new NotImplementedException();
        }
    }
}
