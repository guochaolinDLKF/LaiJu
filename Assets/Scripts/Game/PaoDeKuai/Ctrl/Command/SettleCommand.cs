//===================================================
//Author      : DRB
//CreateTime  ：12/1/2017 10:13:27 AM
//Description ：
//===================================================
using UnityEngine;
using proto.pdk;
namespace PaoDeKuai
{
    public class SettleCommand : IGameCommand
    {
        private PDK_GAME_OVER m_DataEntity;

        public SettleCommand(PDK_GAME_OVER data)
        {
            m_DataEntity = data;
        }


        public void Execute()
        {
            RoomPaoDeKuaiProxy.Instance.Settle(m_DataEntity);
            if (PaoDeKuaiSceneCtrl.Instance != null)
            {
                PaoDeKuaiSceneCtrl.Instance.Settle();
            }
        }

        public void Revoke()
        {

        }

    }
}