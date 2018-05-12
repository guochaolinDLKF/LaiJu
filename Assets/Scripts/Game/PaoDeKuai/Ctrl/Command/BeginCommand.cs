//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 5:05:44 PM
//Description ：开局指令
//===================================================

using UnityEngine;
using proto.pdk;
namespace PaoDeKuai
{
    public class BeginCommand : IGameCommand
    {
        private PDK_BEGIN m_DataEntity;

        public BeginCommand(PDK_BEGIN data)
        {
            m_DataEntity = data;
        }
        public void Execute()
        {
            RoomPaoDeKuaiProxy.Instance.Begin(m_DataEntity);
            if (PaoDeKuaiSceneCtrl.Instance != null)
            {
                PaoDeKuaiSceneCtrl.Instance.Begin(RoomPaoDeKuaiProxy.Instance.CurrentRoom, true, false);
            }

        }

        public void Revoke()
        {

        }
        
    }
}