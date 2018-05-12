//===================================================
//Author      : WZQ
//CreateTime  ：7/26/2017 7:16:50 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaiJiu
{
    public class UIRankerListView_PaiJiu : UIWindowViewBase
    {

        public float _autoExitRoomTime = 60f;

        [SerializeField]
        private RectTransform _ConclusionClusionPanel;             //结算Panel

        [SerializeField]
        private RectTransform _mountPoint;                         //Item挂载点

        [SerializeField]
        private GameObject _itemConclusionTemplate;                //被克隆的模板template
        [SerializeField]
        private GameObject WeiXinShartBtn;                         //微信分享战绩按钮

        private List<PaiJiu.Seat> itemInfoList;                    //全部结算Item信息




        protected override void OnStart()
        {
            base.OnStart();

        }




        #region SetUI  显示结算具体内容
        /// <summary>
        ///开启8局结算                                            
        /// </summary>
        public void SetUI(PaiJiu.Room room)
        {
            //设置微信分享显影
            SetWeiXinShartBtn();
            //根据人数加载结算Item
            LoadConclusionItem(room);



            //开启自动跳转场景协调程序？？？
            //OnOffExitRoom(true);

            //--------动画？？？-------------
            //NiuNiuWindWordAni.GameOverViewAni(_ConclusionClusionPanel);
                



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
                if (WeiXinShartBtn != null) WeiXinShartBtn.SetActive(false);

            }

        }


        // 返回大厅
        void ReturnHall()
        {
            //关闭协调程序
            //OnOffExitRoom(false);

            //返回大厅-
            UIDispatcher.Instance.Dispatch(ConstDefine_PaiJiu.ObKey_btnResultViewBack);
           
            

        }

        //微信分享战绩
        void WeiXinShart()
        {
            UIDispatcher.Instance.Dispatch(ConstDefine_PaiJiu.ObKey_btnResultViewShare);

        }
        #endregion



        #region  加载玩家排名Item
        void LoadConclusionItem(PaiJiu .Room room)
        {

            itemInfoList = room.SeatList;
            for (int i = 0; i < _mountPoint.childCount; i++)
            {
                GameObject go = _mountPoint.GetChild(0).gameObject;
                Destroy(go);
            }

            for (int i = 0; i < itemInfoList.Count; i++)
            {
                if (itemInfoList[i].PlayerId <= 0) continue;
                GameObject go = Instantiate(_itemConclusionTemplate, _mountPoint);
                go.SetActive(true);
                //设置其中具体信息
                go.GetComponent<UIItemConclusion_PaiJiu>().SetUI(itemInfoList[i]);



            }


        }
        #endregion



        //#region 开关自动跳转场景协同程序
        ///// <summary>
        /////  开关自动跳转场景协同程序
        ///// </summary>
        ///// <param name="obj"></param>
        //public void OnOffExitRoom(bool onOff)
        //{
        //    //bool onOff = (bool)obj;

        //    if (onOff)
        //    {
        //        StartCoroutine("ExitRoomAuto");
        //    }
        //    else
        //    {
        //        StopCoroutine("ExitRoomAuto");
        //    }

        //}




        //IEnumerator ExitRoomAuto()
        //{
        //    yield return new WaitForSeconds(_autoExitRoomTime);

        //    //调用返回主界面  断网
        //    //SceneMgr.Instance.LoadScene(SceneType.Main);
        //    UIDispatcher.Instance.Dispatch("btnResultViewBack_NiuNiu");
        //}
        //#endregion

    }
}