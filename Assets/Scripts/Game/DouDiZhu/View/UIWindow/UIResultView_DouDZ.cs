//===================================================
//Author      : DRB
//CreateTime  ：12/18/2017 9:58:49 AM
//Description ：斗地主结算界面窗口视图
//===================================================
using System.Collections.Generic;
using com.oegame.mahjong.protobuf;
using proto.mahjong;
using UnityEngine;
using UnityEngine.UI;
using DRB.DouDiZhu;

namespace DRB.DouDiZhu
{
    public class UIResultView_DouDZ : UIWindowViewBase
    {
        [SerializeField]
        private Text roomIdText;
        [SerializeField]
        private Text roomLoopText;
        [SerializeField]
        private Text dateText;
        [SerializeField]
        private Transform infoContainer;
        protected override void OnBtnClick(GameObject go)
        {
            base.OnBtnClick(go);
            switch (go.name)
            {
                case "btnResultViewBack":
                    SendNotification("btnDouDiZhuResultViewBack");
                    break;
                case "btnResultViewShare":
                    SendNotification("btnDouDiZhuResultViewShare");
                    break;
            }
        }
        public void SetUI(RoomEntity room)
        {
            RoomEntity currentRoom = room;
            for (int i = 0; i < currentRoom.SeatList.Count; i++)
            {
                SeatEntity seatEntity = currentRoom.SeatList[i];

                bool isWiner = true;
                for (int j = 0; j < currentRoom.SeatList.Count; j++)
                {
                    if (currentRoom.SeatList[i].totalScore < currentRoom.SeatList[j].totalScore)
                    {
                        isWiner = false;
                        break;
                    }
                }
                UIItemResult_DouDZ uiItemResult = UIPoolManager.Instance.Spawn("UIItemResultInfo_DouDiZhu").GetComponent<UIItemResult_DouDZ>();
                uiItemResult.transform.SetParent(infoContainer);
                if (uiItemResult.transform.position.z != 0)
                {
                    uiItemResult.transform.localPosition = Vector3.zero;
                }
                uiItemResult.transform.localScale = Vector3.one;
                uiItemResult.SetUI(seatEntity,isWiner, currentRoom.OwnerID == seatEntity.PlayerId, seatEntity.IsPlayer);
            }
            roomIdText.SafeSetText(currentRoom.roomId.ToString());
            roomLoopText.SafeSetText("游戏局数：" + /*currentRoom.currentLoop + "/" +*/ currentRoom.maxLoop);
            dateText.SafeSetText(System.DateTime.Now.ToString());
        }
    }
}