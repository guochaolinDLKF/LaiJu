//===================================================
//Author      : WZQ
//CreateTime  ：5/19/2017 3:12:41 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using NiuNiu;

namespace NiuNiu
{
    /// <summary>
    /// 控制8局结束 总结算                    
    /// </summary>
    public class UINiuNiuRankListView : UIWindowViewBase
    {


        public float _autoExitRoomTime = 60f;

        [SerializeField]
        private RectTransform _ConclusionClusionPanel;             //结算Panel

        [SerializeField]
        private RectTransform _mountPoint;                       //Item挂载点

        [SerializeField]
        private GameObject _itemConclusionTemplate;                //被克隆的模板template
        [SerializeField]
        private GameObject WeiXinShartBtn;                         //微信分享战绩按钮
        
        private List<NiuNiu.Seat> itemInfoList;                    //全部结算Item信息




        protected override void OnStart()
        {
            base.OnStart();

        }




        #region SetUI  显示结算具体内容
        /// <summary>
        ///开启8局结算                                            
        /// </summary>
        public void ShowConclusionClusionPanel(NiuNiu.Room room)
        {
            //设置微信分享显影
            SetWeiXinShartBtn();
            //根据人数加载结算Item
            LoadConclusionItem(room);
            //开启自动跳转场景协调程序
            OnOffExitRoom(true);

            NiuNiuWindWordAni.GameOverViewAni(_ConclusionClusionPanel);
            //_ConclusionClusionPanel.gameObject.SetActive(true);         



        }
        #endregion


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
                if(WeiXinShartBtn != null) WeiXinShartBtn.SetActive(false);
                              
            }

        }


        // 返回大厅
        void ReturnHall()
        {
            //关闭协调程序
            OnOffExitRoom(false);
            //返回大厅-

            UIDispatcher.Instance.Dispatch("btnResultViewBack_NiuNiu");
            //SceneMgr.Instance.LoadScene(SceneType.Main);

        }

        //微信分享战绩
        void WeiXinShart()
        {
            UIDispatcher.Instance.Dispatch("btnResultViewShare_NiuNiu");

        }
        #endregion



        #region  加载玩家排名Item
        void LoadConclusionItem(NiuNiu.Room room)
        {

            int num = 0;

            for (int i = 0; i < room.SeatList.Count; i++)
            {
                if (room.SeatList[i].PlayerId > 0)
                {
                    num++;
                }
            }

            if (num <= 0)
            {
                return;
            }

            if (num > 0)
            {

                itemInfoList = NiuNiuSort.Instance.SortList(room.SeatList, (int)NiuNiu.sortRule.score);

            }

            if (itemInfoList.Count <= 0)
            {
                return;
            }

            for (int i = _mountPoint.childCount - 1; i >= 0; i++)
            {
                GameObject go = _mountPoint.GetChild(i).gameObject;
                Destroy(go);
            }

            for (int i = 0; i < itemInfoList.Count; i++)
            {

                GameObject go = Instantiate(_itemConclusionTemplate, _mountPoint);
                go.SetActive(true);
                //设置其中具体信息
                go.GetComponent<NiuNiuConclusionItem>().SetConclusionItem(itemInfoList[i], (i + 1));



            }




        }
        #endregion



        #region 开关自动跳转场景协同程序
        /// <summary>
        ///  开关自动跳转场景协同程序
        /// </summary>
        /// <param name="obj"></param>
        public void OnOffExitRoom(bool onOff)
        {
            //bool onOff = (bool)obj;

            if (onOff)
            {
                StartCoroutine("ExitRoomAuto");
            }
            else
            {
                StopCoroutine("ExitRoomAuto");
            }

        }




        IEnumerator ExitRoomAuto()
        {
            yield return new WaitForSeconds(_autoExitRoomTime);

            //调用返回主界面  断网
            //SceneMgr.Instance.LoadScene(SceneType.Main);
            UIDispatcher.Instance.Dispatch("btnResultViewBack_NiuNiu");
        }
        #endregion



    }
}