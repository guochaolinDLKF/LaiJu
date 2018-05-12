//===================================================
//Author      : WZQ
//CreateTime  ：11/24/2017 2:34:23 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PaoDeKuai
{
    public class UIItemPaoDeKuaiRoomInfo : UIItemRoomInfoBase
    {
        public static UIItemPaoDeKuaiRoomInfo Instance;


        protected override void OnAwake()
        {
            base.OnAwake();
            Instance = this;
            ModelDispatcher.Instance.AddEventListener(ConstDefine_PaoDeKuai.ON_ROOM_INFO_CHANGED, OnRoomInfoChanged);
        }

        protected override void BeforeOnDestroy()
        {
            base.BeforeOnDestroy();
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_PaoDeKuai.ON_ROOM_INFO_CHANGED, OnRoomInfoChanged);
        }

        /// <summary>
        /// 房间信息变更
        /// </summary>
        /// <param name="obj"></param>
        private void OnRoomInfoChanged(TransferData data)
        {
            RoomEntity room = data.GetValue<RoomEntity>("Room");
           
         
            base.ShowLoop(room.currentLoop, room.maxLoop, room.isQuan);
        }








    }
}