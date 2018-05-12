//===================================================
//Author      : WZQ
//CreateTime  ：7/24/2017 7:57:53 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaiJiu
{
    public class UIUnitSettlement_PaiJiu : UIWindowViewBase
    {
        private float AutoCloseTime = 10;                             //自动关闭时间


        [SerializeField]
        private RectTransform _mountPoint;                          //Item挂载点

        [SerializeField]
        private GameObject _itemConclusionTemplate;                //被克隆的模板template

        private System.Action OnComplete = null;
            
        protected override void OnStart()
        {
            base.OnStart();
            //播放音效
            //AudioEffectManager.Instance.Play( ConstDefine_PaiJiu.AuidoVictorySettle_paijiu, Vector3.zero);

            Invoke("Close", AutoCloseTime);
        }


        protected override void OnBtnClick(GameObject go)
        {

            base.OnBtnClick(go);

            switch (go.name)
            {
                case "NextGameBtn":

                    Debug.Log("开始下一局");
                    Close();
                    
                    break;
            }


           

        }



        public override void Close()
        {
            base.Close();
            if (OnComplete != null) OnComplete();

        }

        /// <summary>
        /// 设置小结算窗口
        /// </summary>
        /// <param name="seatList"></param>
        public void SetUI(List<Seat> seatList, System.Action onComplete = null)
        {

            OnComplete = onComplete;

#if IS_ZHANGJIAKOU
            for (int i = 0; i < seatList.Count; i++)
            {
                if (seatList[i].PlayerId > 0 && seatList[0].Index == 0)
                {
                    AudioEffectManager.Instance.Play(seatList[i].LoopEarnings >= 0 ? ConstDefine_PaiJiu.AuidoVictorySettle_paijiu : ConstDefine_PaiJiu.AudioFailureSettle_paijiu, Vector3.zero);
                    break;
                }
            }
#endif
            //根据人数加载结算Item
            LoadConclusionItem(seatList);
        }




        #region  加载玩家排名Item
        void LoadConclusionItem(List<Seat> seatList)
        {
          

            for (int i = _mountPoint.childCount ; i > 0; i++)
            {
                GameObject go = _mountPoint.GetChild( i - 1 ).gameObject;
                Destroy(go);
            }

            for (int i = 0; i < seatList.Count; i++)
            {
                if (seatList[i].PlayerId <= 0) continue;
                GameObject go = Instantiate(_itemConclusionTemplate, _mountPoint);
                go.SetActive(true);
                go.transform.localScale = Vector3.one;
                //设置其中具体信息
                go.GetComponent<UIItemConclusion_PaiJiu>().SetUI(seatList[i]);

            }




        }
        #endregion



    }
}