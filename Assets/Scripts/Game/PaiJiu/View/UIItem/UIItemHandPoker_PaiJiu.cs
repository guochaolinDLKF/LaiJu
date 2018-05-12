//===================================================
//Author      : WZQ
//CreateTime  ：7/7/2017 6:15:44 PM
//Description ：手牌UIItem  控制3D交互
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace PaiJiu
{
    public class UIItemHandPoker_PaiJiu : UIItemBase
    {
        [HideInInspector]
        public MaJiangCtrl_PaiJiu Majiang;
        [SerializeField]
        private Button m_Button;

        private Tweener tweener;

        private Vector3 UIPokerShow = Vector3.zero;
        private Vector3 UIPokerHide= new Vector3(-120, 0, 0);
        private void Awake()
        {
            m_Button.onClick.AddListener(OnBtnClick);
            tweener = transform.DOLocalRotate(UIPokerHide, 0.7f).SetEase(Ease.Linear).SetAutoKill(false).Pause();
        }
        private void OnBtnClick()
        {
            //if (m_DragBeginPos != Vector3.zero) return;
            if (Majiang.Poker.status == proto.paigow.PAIGOW_STATUS.SHOW) return;
            AppDebug.Log("点击翻牌");
            List<UIItemHandPoker_PaiJiu> openPokerList = new List<UIItemHandPoker_PaiJiu>() { this };
            SendNotification(ConstDefine_PaiJiu.ObKey_SetPokerStatus, openPokerList);
        }


        public void SetUI(MaJiangCtrl_PaiJiu majiang)
        {
            Majiang = majiang;
            majiang.gameObject.SetParent(this.transform);
            //majiang.transform.SetAsFirstSibling();

            majiang.SetPokerStatus(false);


            //SetPokerStatus(false);
        }



        /// <summary>
        /// 设置UI牌状态     
        /// </summary>
        /// <param name="isPlayAnimation"></param>
        //public void SetPokerStatus(Seat seat, bool isPlayAnimation)
        //{

        //}





    }
}