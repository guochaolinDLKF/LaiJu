//===================================================
//Author      : DRB
//CreateTime  ：12/2/2017 4:37:11 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShiSanZhang
{
    public class UIItemRoomInfo_ShiSanZhang : UIItemRoomInfoBase
    {
        public static UIItemRoomInfo_ShiSanZhang Instance;





        protected override void OnAwake()
        {
            base.OnAwake();
            Instance = this;
            ModelDispatcher.Instance.AddEventListener(RoomShiSanZhangProxy.ON_ROOM_INFO_CHANGED, OnRoomInfoChanged);
        }

        protected override void BeforeOnDestroy()
        {
            base.BeforeOnDestroy();
            ModelDispatcher.Instance.RemoveEventListener(RoomShiSanZhangProxy.ON_ROOM_INFO_CHANGED, OnRoomInfoChanged);
        }

        /// <summary>
        /// 房间信息变更
        /// </summary>
        /// <param name="obj"></param>
        private void OnRoomInfoChanged(TransferData data)
        {
            RoomEntity room = data.GetValue<RoomEntity>("Room");
            //if (m_TextBaseScore.text.Contains("底"))
            //{
            //    m_TextBaseScore.SafeSetText("底    分:" + room.BaseScore.ToString());
            //}
            base.ShowLoop(room.currentLoop, room.maxLoop, room.isQuan);
            base.SetUI(room.roomId,room.BaseScore);
        }

    }
}
