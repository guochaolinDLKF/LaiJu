//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 4:43:20 PM
//Description ：准备指令
//===================================================
using UnityEngine;
using proto.pdk;
namespace PaoDeKuai
{
    public class ReadyCommand : IGameCommand
    {

        private PDK_READY m_DataEntity;

        public ReadyCommand(PDK_READY data)
        {
            m_DataEntity = data;
        }


        public void Execute()
        {
            RoomPaoDeKuaiProxy.Instance.Ready(m_DataEntity);
        }

        public void Revoke()
        {

        }
    }
}