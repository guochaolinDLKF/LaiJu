//===================================================
//Author      : DRB
//CreateTime  ：7/6/2017 7:32:02 PM
//Description ：开局命令
//===================================================
using proto.mahjong;
using UnityEngine;

namespace DRB.MahJong
{
    public class BeginCommand : IGameCommand
    {

        private OP_ROOM_BEGIN m_Begin;

        public BeginCommand(OP_ROOM_BEGIN proto)
        {
            m_Begin = proto;
        }

        public void Execute()
        {
            RoomMaJiangProxy.Instance.Begin(m_Begin);
#if UNITY_IPHONE || UNITY_ANDROID
            GameUtil.Shake();
#endif
            if (MaJiangSceneCtrl.Instance != null)
            {
                MaJiangSceneCtrl.Instance.Begin(RoomMaJiangProxy.Instance.CurrentRoom, true, false);
            }

            RoomMaJiangProxy.Instance.SetCountDown(0);
        }

        public void Revoke()
        {

        }
    }
}
