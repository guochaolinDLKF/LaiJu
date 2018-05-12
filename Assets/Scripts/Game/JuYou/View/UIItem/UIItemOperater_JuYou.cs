//===================================================
//Author      : WZQ
//CreateTime  ：8/10/2017 3:00:20 PM
//Description ：聚友 UI界面控制
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace JuYou
{
    public class UIItemOperater_JuYou : UIViewBase
    {
        //DRB.MahJong.UIItemOperator
        //Operater_PaiJiu
        //---------下注-----------------
        [SerializeField]
        private Transform m_PousParent;//下注项
        [SerializeField]
        private Button[] m_PousBtn;//5个下注按钮

        [SerializeField]
        private Button m_ConfirmPour;//确认下注(兜)
        [SerializeField]
        private Button m_BuDou;//不下注(不兜)
        [SerializeField]
        private Button m_CancelPour;//撤销下注

        [SerializeField]
        private Text m_ReadyPourText;//显示准备下注的分数

        private int readyPour = 0;//准备下注的分数
        private int[] pourBtnScore = new int[6] { 1, 5, 10, 50, 100, 200 };

        private int baseScore = 0;//房间底注
        private bool isQuanDou = false;
        //---------选庄-----------------
        //[SerializeField]
        //private GameObject m_ChooseBankerParent;//选择是否做庄按钮 父物体




        public override Dictionary<string, ModelDispatcher.Handler> DicNotificationInterests()
        {
            Dictionary<string, ModelDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();

            NoticeJetton(false);
            //m_PousParent.gameObject.SetActive(false);

            //m_ChooseBankerParent.SetActive(false);
            return dic;


        }

        //对View 提供初始化 ？？？
        public void UIInit()
        {
            //NoticeJetton(false);
            //m_PousParent.gameObject.SetActive(false);
            //m_ChooseBankerParent.SetActive(false);
        }

        //下注分数引用？？？
        protected override void OnBtnClick(GameObject go)
        {
            int pour=  StringUtil.ToInt(go.name);
            if(pour != 0)  Pour(pour);

            switch (go.name)
            {
                //case "PourBtn1":
                //    Pour(pourBtnScore[0]);
                //    //Pour(go.name.ToInt());
                //    break;
                //case "PourBtn2":
                //    Pour(pourBtnScore[1]);
                //    break;
                //case "PourBtn3":
                //    Pour(pourBtnScore[2]);
                //    break;
                //case "PourBtn4":
                //    Pour(pourBtnScore[3]);
                //    break;
                //case "PourBtn5":
                //    Pour(pourBtnScore[4]);
                //    break;
                //case "PourBtn6":
                //    Pour(pourBtnScore[5]);
                    //break;
                case "ConfirmPour"://确认下注
                    ConfirmPour(true);
                    break;
                case "BuDou"://不下注
                    ConfirmPour(false);
                    break;
                case "CancelPour"://撤销
                    CancelPour();
                    break;
                case "QuandouBtn"://全兜
                    isQuanDou = true;
                    Pour(baseScore);
                    break;

                //case "btnQiangZhuang"://抢庄
                //    ConfirmChooseBanker(true);

                //    break;
                //case "btnBuQiang"://不抢
                //    ConfirmChooseBanker(false);
                //    break;

                default:
                    break;
            }
        }




        #region  下注相关    显影由UIScenePaiJiuView 座位信息回调控制
        //控制下注相关 显影
        /// <summary>
        /// 通知下注（显示自己开始下注相关）(开启下注按钮)
        /// </summary>
        public void NoticeJetton(bool OnOff, SeatEntity  seat = null, int score = 0)
        {

            //= data.GetValue<bool>("OnOff");
            if (m_PousParent.gameObject.activeSelf != OnOff)
            {
                Debug.Log("设置下注相关 显影" + OnOff);
                //if (seat != null)
                //{
                //    readyPour = seat.Pour;
                //    SetReadyPourText();
                //}
                readyPour = 0;
                baseScore = score;
                isQuanDou = false;
                SetReadyPourText();
                m_PousParent.gameObject.SetActive(OnOff);
                //m_ConfirmPour.gameObject.SetActive(OnOff);
                //m_CancelPour.gameObject.SetActive(OnOff);

            }

        }

        //5个下注按钮 添加下注分数
        private void Pour(int pour)
        {
            Debug.Log("baseScore" + baseScore);
            //点击按钮之后应该是增加 准备下注分
            if (!m_ConfirmPour.gameObject.activeSelf) return;
            readyPour += pour;
            readyPour = Mathf.Clamp(readyPour, 0, baseScore);
            SetReadyPourText();
        }

        /// <summary>
        /// 确认下注的分数
        /// </summary>
        private void ConfirmPour(bool isDou)
        {        
            if (readyPour == 0 && isDou) return;//下注为0 不发送消息
            object[] obj = new object[] { readyPour,isQuanDou };
            SendNotification(ConstDefine_JuYou.ObKey_btnPour, isDou ? obj : null);        
        }

       
        /// <summary>
        /// 撤销准备下注的分数
        /// </summary>
        private void CancelPour()
        {
            isQuanDou = false;
            readyPour = 0;
            SetReadyPourText();

        }


        //显示准备下注分数
        public void SetReadyPourText()
        {
            m_ReadyPourText.SafeSetText(string.Format("{0}", readyPour.ToString()));
        }
        #endregion


        #region  选庄相关

        ////设置选庄项
        //public void ChooseBanker(bool OnOff)
        //{
        //    if (m_ChooseBankerParent.activeSelf != OnOff)
        //    {
        //        m_ChooseBankerParent.SetActive(OnOff);

        //        //动画效果？？
        //    }

        //}

        //是否坐庄按钮监听
        private void ConfirmChooseBanker(bool isChooseBanker)
        {

            object[] obj = new object[] { isChooseBanker };
            SendNotification(ConstDefine_JuYou.ObKey_btnChooseBanker, obj);

        }
        #endregion
    }
}