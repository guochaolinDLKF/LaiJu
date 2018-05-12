//===================================================
//Author      : WZQ
//CreateTime  ：8/22/2017 4:03:32 PM
//Description ：聚友 总结算排行榜窗口
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace JuYou
{
    public class UIWindowResult_JuYou : UIWindowViewBase
    {
        
        [SerializeField]
        private RectTransform _mountPoint;                         //Item挂载点

        [SerializeField]
        private GameObject _itemConclusionTemplate;                //被克隆的模板template
        [SerializeField]
        private GameObject WeiXinShartBtn;                         //微信分享战绩按钮

        private List<SeatEntity> itemInfoList;                    //全部结算Item信息

        [SerializeField]
        private Text m_TextCountDown;//倒计时

        private float m_fTimer;
        [SerializeField]
        private float m_fCountDown = 60;
        private bool m_isAutoClick;


        protected override void OnStart()
        {
            base.OnStart();

        }

        #region SetUI  显示结算具体内容
        /// <summary>
        ///开启8局结算                                            
        /// </summary>
        public void SetUI(RoomEntity room)
        {
            m_isAutoClick = true;


            //设置微信分享显影
            SetWeiXinShartBtn();
            //根据人数加载结算Item
            LoadConclusionItem(room);


        }
        #endregion
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
                       
                        ReturnHall();
                    }
                }
            }
        }

        #region 结算排名界面 点击事件
        protected override void OnBtnClick(GameObject go)
        {
            base.OnBtnClick(go);

            switch (go.name)
            {
                case "ReturnBtn":
                    ReturnHall();
                    break;
                case "WeiXinShartBtn":
                    WeiXinShart();
                    break;
            }
        }

        private void SetWeiXinShartBtn()
        {
            if (!SystemProxy.Instance.IsInstallWeChat || !SystemProxy.Instance.IsOpenWXLogin)
            {
                if (WeiXinShartBtn != null) WeiXinShartBtn.SetActive(false);

            }

        }


        // 返回大厅
        void ReturnHall()
        {
            //返回大厅-
            UIDispatcher.Instance.Dispatch(ConstDefine_JuYou.ObKey_btnResultViewBack);

        }

        //微信分享战绩
        void WeiXinShart()
        {
            UIDispatcher.Instance.Dispatch(ConstDefine_JuYou.ObKey_btnResultViewShare);

        }
        #endregion



        #region  加载玩家排名Item
        void LoadConclusionItem(RoomEntity room)
        {

            itemInfoList = room.SeatList;
            for (int i = 0; i < _mountPoint.childCount; i++)
            {
                GameObject go = _mountPoint.GetChild(0).gameObject;
                Destroy(go);
            }

            int goldMin = 0;
            int goldMax = 0;
            for (int i = 0; i < itemInfoList.Count; i++)
            {
                if (itemInfoList[i].PlayerId <= 0) continue;
                if (itemInfoList[i].Gold < goldMin) goldMin = itemInfoList[i].Gold;
                if (itemInfoList[i].Gold > goldMax) goldMax = itemInfoList[i].Gold;
            }


            for (int i = 0; i < itemInfoList.Count; i++)
            {
                if (itemInfoList[i] == null || itemInfoList[i].PlayerId <= 0) continue;
                GameObject go = Instantiate(_itemConclusionTemplate, _mountPoint);
                go.SetActive(true);
                //设置其中具体信息
                go.GetComponent<UIItemSettleSeatInfo_JuYou>().SetUI(itemInfoList[i]);
                if (itemInfoList[i].Gold <= goldMin) go.GetComponent<UIItemSettleSeatInfo_JuYou>().SetRichMan(true);
                if (itemInfoList[i].Gold >= goldMax) go.GetComponent<UIItemSettleSeatInfo_JuYou>().SetDaYingJia(true);
                
            }


        }
        #endregion

    }
}