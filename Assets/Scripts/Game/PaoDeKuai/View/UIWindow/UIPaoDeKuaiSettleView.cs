//===================================================
//Author      : WZQ
//CreateTime  ：12/1/2017 5:38:00 PM
//Description ：跑的快每局结算界面
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace PaoDeKuai
{
    public class UIPaoDeKuaiSettleView : UIWindowViewBase
    {
        [SerializeField]
        private UIItemPDKSettleSeat m_PlayerItem;

        [SerializeField]
        private UIItemPDKSettleSeat m_Template;

        [SerializeField]
        private Transform m_SeatGroup;

        [SerializeField]
        private Text m_TextCountDown;

        [SerializeField]
        private Image[] m_IsVictory;

        [SerializeField]
        private GameObject m_BtnReady;//继续游戏（准备）

        [SerializeField]
        private GameObject m_BtnResult;//查看结果

        private float m_fTimer;

        private float m_fCountDown;

        private bool m_isAutoClick;






        public void SetUI(List<SeatEntity> seatList,int playerPos,int winnertPos, bool isLastLoop, float countDown=5f)
        {
            //倒计时
            m_isAutoClick = !isLastLoop;
            m_fCountDown = countDown;
            m_fTimer = Time.time;

            m_BtnReady.SetActive(!isLastLoop);
            m_BtnResult.SetActive(isLastLoop);

            //动画
            for (int i = 0; i < m_IsVictory.Length; ++i)
            {
                m_IsVictory[0].gameObject.SetActive(winnertPos == playerPos);
                m_IsVictory[1].gameObject.SetActive( !(winnertPos == playerPos));
            }

            //Item信息
            if (m_Template == null) return;

            for (int i = 0; i < seatList.Count; i++)
            {
                if (seatList[i].PlayerId == 0) continue;

                if (seatList[i].Pos == playerPos)
                {
                    m_PlayerItem.SetUI(seatList[i]);
                }
                else
                {
                    GameObject go = Instantiate(m_Template.gameObject);
                    go.SetParent(m_SeatGroup);
                    go.GetComponent<UIItemPDKSettleSeat>().SetUI(seatList[i]);
                    go.SetActive(true);
                }
                            
            }
        }


        protected override void OnBtnClick(GameObject go)
        {
            switch (go.name)
            {
                case ConstDefine_PaoDeKuai.BtnPDKSettleViewReady:  //继续游戏点击
                    SendNotification(ConstDefine_PaoDeKuai.BtnPDKSettleViewReady); 
                    break;
                case ConstDefine_PaoDeKuai.BtnPDKSettleViewResult://查看牌局结果
                    SendNotification(ConstDefine_PaoDeKuai.BtnPDKSettleViewResult);
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
                        SendNotification(ConstDefine_PaoDeKuai.BtnPDKSettleViewReady);
                    }
                }
            }
        }


    }
}