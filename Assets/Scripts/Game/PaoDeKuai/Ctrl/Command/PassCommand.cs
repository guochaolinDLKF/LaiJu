//===================================================
//Author      : DRB
//CreateTime  ：12/2/2017 11:39:04 AM
//Description ：
//===================================================

using UnityEngine;
using proto.pdk;
namespace PaoDeKuai {
    public class PassCommand : IGameCommand {

        private PDK_PASS m_DataEntity;

        public PassCommand(PDK_PASS data)
        {
            m_DataEntity = data;
        }


        public void Execute()
        {
            RoomPaoDeKuaiProxy.Instance.Pass(m_DataEntity);
            //if (PaoDeKuaiSceneCtrl.Instance != null)
            //{
            //    PaoDeKuaiSceneCtrl.Instance.(RoomPaoDeKuaiProxy.Instance.CurrentRoom.CurrAlreadyPlayPos, RoomPaoDeKuaiProxy.Instance.CurrentRoom.RecentlyPlayPoker.Pokers);
            //}
        }

        public void Revoke()
        {

        }
    }
}