//===================================================
//Author      : WZQ
//CreateTime  ：10/17/2017 4:07:27 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NiuNiu
{
    public class UIRubPokerView_NiuNiu : UIWindowViewBase
    {
        [SerializeField]
        private UIItemBook_NiuNiu m_Book;
        [SerializeField]
        private Sprite m_backPokerSprite;
        [SerializeField]
        private Image[] m_otherPoker;

        private Seat playerSeat=null;
        private System.Action action = null;
        public void SetUI(Seat seat,System.Action action = null)
        {
            if (seat.PokerList.Count <= 0)
            {
                Close();
                return;
            }
            playerSeat = seat;
            this.action = action;
            string pathK = string.Format("download/{0}/source/uisource/gameuisource/poker/RealityPoker.drb", ConstDefine.GAME_NAME);
            Sprite currSprite = null;

            for (int i = 0; i < seat.PokerList.Count - 1; i++)
            {
                currSprite = LoadSprite(seat.PokerList[i],true);
                if (currSprite != null && i < m_otherPoker.Length) m_otherPoker[i].sprite = currSprite;
            }


            currSprite = LoadSprite(seat.PokerList[seat.PokerList.Count - 1],false);
            Sprite[] bookPages = new Sprite[2] { currSprite == null ? m_backPokerSprite : currSprite, m_backPokerSprite };
            int currentPage = 2;

            m_Book.InitBook(bookPages, currentPage, transform.GetComponent<Canvas>());
        }


        private Sprite LoadSprite(Poker poker,bool isSmallPoker)
        {
            string pathK = string.Format("download/{0}/source/uisource/gameuisource/poker/RealityPoker.drb", ConstDefine.GAME_NAME);

            Poker pokerItem = poker;
            if (isSmallPoker)
            {
                pathK = string.Format("download/{0}/source/uisource/gameuisource/poker.drb", ConstDefine.GAME_NAME);

            }
            else
            {
                int size = poker.size;
                if (size == 1) size = 14;
                else if (size == 14) size = 99;
                else if (size == 15) size = 100;
                pokerItem = new Poker(poker.index, size, poker.color, niuniu.proto.NN_ENUM_POKER_STATUS.POKER_STATUS_UPWARD);
            }

            Sprite currSprite = null;
            string pokerName = "16_1";
            pokerName = pokerItem.size == 0 ? "16_1" : pokerItem.ToString();

            currSprite = null;
            currSprite = AssetBundleManager.Instance.LoadSprite(pathK, pokerName);
            return currSprite;
        }


        #region 搓牌 取消 点击事件
        protected override void OnBtnClick(GameObject go)
        {
            base.OnBtnClick(go);

            switch (go.name)
            {
                case "ReturnBtn":
                   
                    break;
               
            }
        }
        #endregion


        public void RubPokerAniComplete()
        {
            if (action != null) action();
        }
    }
}