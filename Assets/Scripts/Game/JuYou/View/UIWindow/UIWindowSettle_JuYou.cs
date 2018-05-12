//===================================================
//Author      : WZQ
//CreateTime  ：8/21/2017 4:50:25 PM
//Description ：聚友每局结算
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace JuYou
{
    public class UIWindowSettle_JuYou : UIWindowViewBase
    {
        private float AutoCloseTime = 10;                             //自动关闭时间


        [SerializeField]
        private RectTransform _mountPoint;                          //Item挂载点

        [SerializeField]
        private GameObject _itemConclusionTemplate;                //被克隆的模板template

        [SerializeField]
        private Text m_TextCountDown;//倒计时

        [SerializeField]
        private GameObject m_NextGameBtn;//下一局按钮

        private float m_fTimer;
        [SerializeField]
        private float m_fCountDown = 5;
        private bool m_isAutoClick;

        protected override void OnStart()
        {
            base.OnStart();


            //播放音效
            //AudioEffectManager.Instance.Play(NiuNiu.ConstDefine_NiuNiu.UnitSettlement_NiuNiu, Vector3.zero);

            //自动关闭该窗口
            //Destroy(gameObject, destroyTime);
            //Invoke("Close", AutoCloseTime);
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
                        //发送开始下一把
                        SendNotification(ConstDefine_JuYou.ObKey_NextGame);
                    }
                }
            }
        }
     


        protected override void OnBtnClick(GameObject go)
        {

            base.OnBtnClick(go);

            switch (go.name)
            {
                case "NextGameBtn":
                    Debug.Log("开始下一局");
                    SendNotification(ConstDefine_JuYou.ObKey_NextGame);
                    break;
            }




        }

        /// <summary>
        /// 设置小结算窗口
        /// </summary>
        /// <param name="seatList"></param>
        public void Settle(RoomEntity room)
        {
            List<SeatEntity> seatList = room.SeatList;
            //m_fCountDown = countdown;
            m_isAutoClick = true;
            if (room.currentLoop ==room.maxLoop)
            {
                m_isAutoClick = false;
                m_TextCountDown.gameObject.SetActive(false);
                m_NextGameBtn.gameObject.SetActive(false);
            }
             //加载详细信息
             //根据人数加载结算Item
             LoadConclusionItem(seatList);


            //设置自动关闭

            ////播放胜利失败动画
            //if (seatList.Count > 0)
            //{
            //    VictoryFailureAni(seat);

            //}
            //StartCoroutine("ConclusionAni");

        }




        #region  加载玩家排名Item
        void LoadConclusionItem(List<SeatEntity> seatList)
        {


            for (int i = _mountPoint.childCount; i > 0; i++)
            {
                GameObject go = _mountPoint.GetChild(i - 1).gameObject;
                Destroy(go);
            }

          
            for (int i = 0; i < seatList.Count; i++)
            {
                if (seatList[i].PlayerId <= 0) continue;
                GameObject go = Instantiate(_itemConclusionTemplate, _mountPoint);
                go.SetActive(true);
                go.transform.localScale = Vector3.one;
                //设置其中具体信息
                go.GetComponent<UIItemSettleSeatInfo_JuYou>().SetUI(seatList[i]);//SetConclusionItem(itemInfoList[i], (i + 1));
               

             
            }




        }
        #endregion

   
    }
}