//===================================================
//Author      : DRB
//CreateTime  ：12/6/2017 7:06:55 PM
//Description ：斗地主结算界面
//===================================================
using UnityEngine;
using UnityEngine.UI;

namespace DRB.DouDiZhu
{
    public class UISettleView : UIWindowViewBase
    {
        [SerializeField]
        private Image img_isWinnerTitle;
        [SerializeField]
        private Image img_isLoserTitle;
        [SerializeField]
        private Text roomIdText;
        [SerializeField]
        private Text roomSettleLoopText;
        [SerializeField]
        public Text roomLoopText;
        [SerializeField]
        private Text dateText;
        [SerializeField]
        private Transform infoContainer;
        [SerializeField]
        private UIResultView_DouDZ m_UIResultView;

        private int m_loop;
        private int m_maxLoop;
        protected override void OnBtnClick(GameObject go)
        {
            base.OnBtnClick(go);
            switch (go.name)
            {
                case "btnDouDiZhuViewReady":
                    SendNotification("btnDouDiZhuViewReady");
                    ShowResultView();
                    Close();
                    break;
                case "btnClose":
                    SendNotification("btnDouDiZhuViewReady");
                    Close();
                    break;
                case "btnSettleViewShowPokerOpen":
                    SendNotification("btnDouDiZhuViewReady");
                    SendNotification("btnSettleViewShowPokerOpen");
                    Close();
                    break;

            }
        }
        public void SetUI(RoomEntity room/*,Text roomLoop*/)
        {
            RoomEntity currentRoom = room;

            //if (roomLoopText == null)
            //{
            //    roomLoopText = roomLoop;
            //}

            int bet = 0;
            bool isPlayer = false;
            for (int i = 0; i < currentRoom.SeatList.Count; i++)
            {
                if (currentRoom.SeatList[i].IsBanker)
                {
                    bet = currentRoom.SeatList[i].bet;
                    break;
                }
            }
            
            for (int i = 0; i < currentRoom.SeatList.Count; i++)
            {
                SeatEntity seatEntity = currentRoom.SeatList[i];

                if (seatEntity.IsPlayer)
                {
                    if (seatEntity.isWiner)
                    {
                        img_isWinnerTitle.SafeSetActive(true);
                        img_isLoserTitle.SafeSetActive(false);
                        AudioEffectManager.Instance.Play("doudizhu/" + "win");
                    }
                    else
                    {
                        img_isWinnerTitle.SafeSetActive(false);
                        img_isLoserTitle.SafeSetActive(true);
                        AudioEffectManager.Instance.Play("doudizhu/" + "lose");
                    }
                }

                UIItemSettleSeatInfo_DouDZ uiitemSettleInfo = UIPoolManager.Instance.Spawn("UIItemSettleInfo_DouDiZhu").GetComponent<UIItemSettleSeatInfo_DouDZ>();

                if (uiitemSettleInfo.transform != null)
                {
                    uiitemSettleInfo.transform.SetParent(infoContainer);
                }
                else
                {
                    Debug.LogWarning("空");
                }

                if (uiitemSettleInfo.transform.position.z != 0)
                {
                    uiitemSettleInfo.transform.localPosition = Vector3.zero;
                }
                uiitemSettleInfo.transform.localScale = Vector3.one;
                uiitemSettleInfo.SetUI(seatEntity, bet, currentRoom.OwnerID == seatEntity.PlayerId, isPlayer, currentRoom.Times);
               
            }
            roomIdText.SafeSetText(currentRoom.roomId.ToString());
            roomSettleLoopText.SafeSetText("游戏局数：" + currentRoom.currentLoop + "/" + currentRoom.maxLoop);
            m_loop = currentRoom.currentLoop;
            m_maxLoop = currentRoom.maxLoop;
            roomLoopText.SafeSetText(currentRoom.currentLoop + "/" + currentRoom.maxLoop);
            dateText.SafeSetText(System.DateTime.Today.ToString());
        }
        public void SetResultView(UIResultView_DouDZ uiResultView)
        {
            m_UIResultView = uiResultView;
            m_UIResultView.SafeSetActive(false);
        }
        private void ShowResultView()
        {
            if (m_UIResultView != null)
            {
                m_UIResultView.SafeSetActive(true);
            }
            m_UIResultView = null;
        }
    }
}
