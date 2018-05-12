//===================================================
//Author      : WZQ
//CreateTime  ：11/21/2017 10:23:58 AM
//Description ：跑得快总结算
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace PaoDeKuai {
    public class UIPaoDeKuaiResult : UIWindowViewBase
    {
        [SerializeField]
        private Text m_TextRoomId;
        [SerializeField]
        private Transform m_SeatInfoContainer;

        [SerializeField]
        private Text m_TextBack;
        [SerializeField]
        private Button m_BtnShare;
        [SerializeField]
        private Text m_TextDateTime;


        protected override void OnAwake()
        {
            base.OnAwake();
            m_BtnShare.gameObject.SetActive(SystemProxy.Instance.IsInstallWeChat);
        }

        public void SetUI(RoomEntity room)
        {
            m_TextRoomId.SafeSetText(room.roomId.ToString());
            int winerIndex = 0;
            int gold = 0;
            for (int i = 0; i < room.SeatList.Count; ++i)
            {
                if (room.SeatList[i].Gold > gold)
                {
                    winerIndex = i;
                    gold = room.SeatList[i].Gold;
                }
            }
            m_TextDateTime.SafeSetText(System.DateTime.Now.ToString(ConstDefine.TIME_FORMAT));


            UIViewManager.Instance.LoadItemAsync(ConstDefine_PaoDeKuai.UIItemNameResult, (GameObject prefab) =>
            {
                for (int i = 0; i < room.SeatList.Count; ++i)
                {
                    GameObject go = Instantiate(prefab);
                    go.SetParent(m_SeatInfoContainer);
                    UIItemPaoDeKuaiResult result = go.GetComponent<UIItemPaoDeKuaiResult>();
                    result.SetUI(room.SeatList[i], winerIndex == i, 0);
                }
            });

            //m_TextBack.SafeSetText(RoomMaJiangProxy.Instance.CurrentRoom.matchId > 0 ? "继续" : "返回");
        }

        protected override void OnBtnClick(GameObject go)
        {
            base.OnBtnClick(go);
            switch (go.name)
            {
                case "btnResultViewBack":
                    SendNotification(ConstDefine_PaoDeKuai.BtnPDKResultViewBack);
                    break;
                case "btnResultViewShare":
                    SendNotification(ConstDefine_PaoDeKuai.BtnPDKResultViewShare);
                    break;
            }
        }





    }
}