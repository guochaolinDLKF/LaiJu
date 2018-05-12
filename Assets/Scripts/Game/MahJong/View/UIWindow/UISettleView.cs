//===================================================
//Author      : DRB
//CreateTime  ：3/17/2017 9:59:48 AM
//Description ：
//===================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DRB.MahJong
{
    public class UISettleView : UIWindowViewBase
    {
        [SerializeField]
        private Transform m_HuInfoContainer;


        [SerializeField]
        private Text m_TextCountDown;

        [SerializeField]
        private GameObject m_Page1;
        [SerializeField]
        private GameObject m_Page2;
        [SerializeField]
        private GameObject m_Page3;
        [SerializeField]
        private Button m_BtnShare;
        [SerializeField]
        private Text m_TextTaiSetting;

        [SerializeField]
        private Text m_TxtRoomId;
        [SerializeField]
        private Text m_TxtGameLoop;


        private List<UIItemSettleSeatInfo> m_Cache = new List<UIItemSettleSeatInfo>();


        protected override void OnAwake()
        {
            base.OnAwake();
            m_BtnShare.gameObject.SetActive(SystemProxy.Instance.IsInstallWeChat);
        }

        private float m_fTimer;


        private float m_fCountDown;


        private bool m_isAutoClick;

#if IS_LONGGANG
    private const float AUTO_READY_COUNT_DOWN = 10f;
#else
        private const float AUTO_READY_COUNT_DOWN = 5f;
#endif

#if DEBUG_MODE
        private const float AUTO_READY_COUNT_DOWN_COMMON = 5f;
#else
#if IS_LUALU
    private const float AUTO_READY_COUNT_DOWN_COMMON = 10f;
#else
    private const float AUTO_READY_COUNT_DOWN_COMMON = 30f;
#endif
#endif



        public void Settle(List<SeatEntity> seats, int playerSeatPos, int currentLoop, int maxLoop,
            List<Poker> prob, int matchId, Poker luckPoker, bool isQuan, bool isOver, int roomId, string huTaiCount, bool isFeng)
        {
            m_TxtGameLoop.SafeSetText(string.Format("游戏局数:{0}/{1}", currentLoop.ToString(), maxLoop.ToString()));
            m_TxtRoomId.SafeSetText("房间Id:" + roomId.ToString());

            bool isZimo = true;
            int bankerPos = 0;
            for (int i = 0; i < seats.Count; ++i)
            {
                if (seats[i].isLoser) isZimo = false;
                if (seats[i].IsBanker) bankerPos = seats[i].Pos;
            }

            if (!string.IsNullOrEmpty(huTaiCount))
            {
                m_TextTaiSetting.SafeSetText("胡台数:" + huTaiCount);
            }

            for (int i = 0; i < m_Cache.Count; ++i)
            {
                m_Cache[i].gameObject.SetActive(false);
            }

            UIViewManager.Instance.LoadItemAsync("UIItemHuInfo", (GameObject prefab) =>
             {
                 for (int i = 0; i < seats.Count; ++i)
                 {
                     SeatEntity seat = seats[i];
                     UIItemSettleSeatInfo info = null;
                     if (i < m_Cache.Count)
                     {
                         info = m_Cache[i];
                         info.gameObject.SetActive(true);
                     }
                     else
                     {
                         GameObject go = Instantiate(prefab);
                         go.SetParent(m_HuInfoContainer);
                         info = go.GetComponent<UIItemSettleSeatInfo>();
                         m_Cache.Add(info);
                     }
                     info.SetUI(seat.Pos, seat.Pos == playerSeatPos, seat.Avatar, seat.Nickname, seat.Gold, seat.Settle,
                         seat.IsBanker, seat.PokerList, seat.HitPoker, seat.UsedPokerList, seat.SettleInfo, seat.HuScoreDetail,
                         seat.isWiner, prob, seat.ProbMulti, seat.isLoser, isZimo, seat.UniversalList, seat.TotalHuScore,
                         luckPoker, seat.IsTing, seat.HoldPoker, bankerPos, isFeng, seat.DeskTopPoker, seat.Direction);
                 }
             });


            if (RoomMaJiangProxy.Instance.CurrentRoom.isReplay)
            {
                m_Page1.SetActive(false);
                m_Page2.SetActive(false);
                m_Page3.SetActive(true);
            }
            else
            {
                m_Page1.SetActive(!isOver);
                m_Page2.SetActive(isOver);
                m_Page3.SetActive(false);

                if (!isOver)
                {
                    m_isAutoClick = true;
                    m_fCountDown = matchId > 0 ? AUTO_READY_COUNT_DOWN : AUTO_READY_COUNT_DOWN_COMMON;
                    m_fTimer = Time.time;
                }
            }

        }

        protected override void OnBtnClick(GameObject go)
        {
            base.OnBtnClick(go);
            switch (go.name)
            {
                case "btnReady":
                    SendNotification("btnReady");
                    break;
                case "btnSettleViewResult":
                    SendNotification("btnSettleViewResult");
                    break;
                case "btnSettleViewShare":
                    SendNotification("btnSettleViewShare");
                    break;
                case ConstDefine.BtnSettleViewReplayOver:
                    SendNotification(ConstDefine.BtnSettleViewReplayOver);
                    break;
            }
        }

        private void Update()
        {
            if (m_isAutoClick)
            {
                if (Time.time - m_fTimer > 1f)
                {
                    m_fTimer = Time.time;
                    m_fCountDown -= 1f;
                    m_TextCountDown.SafeSetText(m_fCountDown.ToString());
                    if (m_fCountDown <= 0f)
                    {
                        m_isAutoClick = false;
                        SendNotification("btnReady");
                    }
                }
            }
        }
    }
}
