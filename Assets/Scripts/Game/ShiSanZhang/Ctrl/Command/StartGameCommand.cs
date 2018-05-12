//===================================================
//Author      : DRB
//CreateTime  ：12/4/2017 3:38:09 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.sss;

namespace ShiSanZhang
{
    public class StartGameCommand : IGameCommand
    {      

        private List<DEAL_POKER> m_dealPokers;

        public StartGameCommand(List<DEAL_POKER> dealPoker)
        {
            m_dealPokers = dealPoker;          
        }


        public void Execute()
        {
            RoomShiSanZhangProxy.Instance.Begin(m_dealPokers);
            if (ShiSanZhangSceneCtrl.Instance!=null)
            {
                Debug.Log("开始发牌！！！！！！！！！！！！！！！！！！！");
                ShiSanZhangSceneCtrl.Instance.Begin(RoomShiSanZhangProxy.Instance.CurrentRoom,true);
            }
            
        }

        public void Revoke()
        {
            
        }
    }
}