//===================================================
//Author      : WZQ
//CreateTime  ：6/6/2017 7:04:16 PM
//Description ：单元小结窗口
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using NiuNiu;

namespace NiuNiu
{
    public class UIUnitSettlement_NiuNiu : UIWindowViewBase
    {



        private float AutoCloseTime = 4;                             //自动关闭时间

        public float _autoExitRoomTime = 60f;

        //[SerializeField]
        //private RectTransform _ConclusionClusionPanel;              //结算Panel

        [SerializeField]
        private RectTransform _mountPoint;                          //Item挂载点

        [SerializeField]
        private GameObject _itemConclusionTemplate;                //被克隆的模板template


        private List<NiuNiu.Seat> itemInfoList;                    //全部结算Item信息

        [SerializeField]
        private GameObject _ContentBG;                          //详细信息

        [SerializeField]
        private GameObject[] _VictoryFailureAni;                   //胜利失败动画





        protected override void OnStart()
        {
            base.OnStart();


            //播放音效
            AudioEffectManager.Instance.Play(NiuNiu.ConstDefine_NiuNiu.UnitSettlement_NiuNiu, Vector3.zero);


            //自动关闭该窗口
            //Destroy(gameObject, destroyTime);
            Invoke("Close", AutoCloseTime);
        }




        //开始下一局
        protected override void OnBtnClick(GameObject go)
        {
            Debug.Log("开始下一局");

            base.OnBtnClick(go);
            Close();
        }


        public void SetUI(List<NiuNiu.Seat> seatList, NiuNiu.Seat seat)
        {
            InitAni();
            //加载详细信息
            //根据人数加载结算Item
            LoadConclusionItem(seatList);


            //播放胜利失败动画
            if (seatList.Count > 0)
            {
                VictoryFailureAni(seat);

            }
            StartCoroutine("ConclusionAni");

        }

        void InitAni()
        {
            for (int i = 0; i < _VictoryFailureAni.Length; i++)
            {
                _VictoryFailureAni[i].SetActive(false);
                _VictoryFailureAni[i].transform.localScale = Vector3.zero;
            }
            _ContentBG.SetActive(false);

        }

        //成功失败动画
        void VictoryFailureAni(NiuNiu.Seat seat)
        {
            if (seat.Earnings >= 0)
            {
                _VictoryFailureAni[0].SetActive(true);
                NiuNiuWindWordAni.PopUp(_VictoryFailureAni[0].transform);
            }
            if (seat.Earnings < 0)
            {
                _VictoryFailureAni[1].SetActive(true);
                NiuNiuWindWordAni.PopUp(_VictoryFailureAni[1].transform);
            }


        }



        //详细信息动画
        IEnumerator ConclusionAni()
        {
            yield return new WaitForSeconds(1f);

            _ContentBG.SetActive(true);
            NiuNiuWindWordAni.PopUp(_ContentBG.transform);

        }





        #region  加载玩家排名Item
        void LoadConclusionItem(List<NiuNiu.Seat> seatList)
        {
            int num = 0;
            for (int i = 0; i < seatList.Count; i++)
            {
                if (seatList[i].PlayerId > 0)
                {
                    num++;
                }
            }


            if (num <= 0 || _itemConclusionTemplate == null)
            {
                return;
            }

            if (num > 0)
            {

                itemInfoList = NiuNiuSort.Instance.SortList(seatList, (int)NiuNiu.sortRule.score);

            }

            if (itemInfoList.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < _mountPoint.childCount; i++)
            {
                GameObject go = _mountPoint.GetChild(0).gameObject;
                Destroy(go);
            }

            for (int i = 0; i < itemInfoList.Count; i++)
            {

                GameObject go = Instantiate(_itemConclusionTemplate, _mountPoint);
                go.SetActive(true);
                go.transform.localScale = Vector3.one;
                //设置其中具体信息
                go.GetComponent<NiuNiuConclusionItem>().SetConclusionItem(itemInfoList[i], (i + 1));





            }




        }
        #endregion



    }
}