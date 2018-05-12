//===================================================
//Author      : WZQ
//CreateTime  ：7/7/2017 5:53:26 PM
//Description ：挂载在麻将上 控制该单个麻将
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
namespace PaiJiu{
    public class MaJiangCtrl_PaiJiu : MonoBehaviour
    {

        /// <summary>
        /// 数据
        /// </summary>
        [SerializeField]
        private PaiJiu.Poker m_Poker;
        public PaiJiu.Poker Poker
        {
            get { return m_Poker; }
            private set { m_Poker = value; }
        }

       

        [SerializeField]
        private bool isBeenPlayed = false;


        private Tweener tweener;
        private Vector3 UIPokerShow = new Vector3(180, 0, 0);
        private Vector3 UIPokerHide = Vector3.zero;


        private void Awake()
        {
           
             tweener = transform.DOLocalRotate(UIPokerShow, 0.7f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetAutoKill(false).Pause();
           
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(7f, 5f, 10f);


        }

        public void Init(PaiJiu.Poker poker)
        {
            isBeenPlayed = false;
            Poker = poker;
            //SetPokerStatus(false);
        }


       

        /// <summary>
        /// 设置牌状态      -------------------服务器广播消息后  再由View调用 播放动画-------------------------------
        /// </summary>
        /// <param name="isPlayAnimation"></param>
        public void SetPokerStatus(bool isPlayAnimation)
        {
           
            //是否已经播放过动画
            if (isBeenPlayed) return;

            if (isPlayAnimation)
            {
                if (m_Poker.status == proto.paigow.PAIGOW_STATUS.SHOW)
                {
                   
                    isBeenPlayed = true;
                    AppDebug.Log(string.Format("翻开牌{0}",m_Poker.ToChinese()));

                    tweener.OnComplete(

                        () => {
                            transform.localEulerAngles = m_Poker.status == proto.paigow.PAIGOW_STATUS.SHOW ? UIPokerShow : UIPokerHide;
                            AppDebug.Log("Poker旋转动画播放完毕");
                        }
                        
                       
                        ).Restart();
                   
                }
                //else
                //{
                //    tweener.PlayBackwards();
                //}


            }
            else
            {
                transform.localEulerAngles = m_Poker.status == proto.paigow.PAIGOW_STATUS.SHOW ? UIPokerShow : UIPokerHide;
                if (m_Poker.status == proto.paigow.PAIGOW_STATUS.SHOW) isBeenPlayed = true;
                AppDebug.Log(string.Format("transform.localEulerAngles{0}", transform.localEulerAngles));
            }



        }




       

       
     

        protected  void SendNotification(string notificationName, params object[] param)
        {
            UIDispatcher.Instance.Dispatch(notificationName, param);
        }


    }






}