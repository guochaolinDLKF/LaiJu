//===================================================
//Author      : WZQ
//CreateTime  ：8/29/2017 3:49:16 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using niuniu.proto;
namespace NiuNiu
{
    public class PokerCtrl : MonoBehaviour
    {
        public GameObject positive;//正面图片
        public GameObject reverse;//背面图片
        private Image positiveImage;
        private bool isBeenPlayed = false;

        private Tweener flipCardsAni;
        void Awake()
        {
            positiveImage = positive.GetComponent<Image>();
            positive.SetActive(false);
            reverse.SetActive(true);
            flipCardsAni = transform.DOScaleX(0, 0.08f).SetEase(Ease.Linear).SetAutoKill(false).Pause(); 
        }

        //void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.A))
        //    {

        //        FlipCardsForward();
        //    }

        //    if (Input.GetKeyDown(KeyCode.S))
        //    {

        //        FlipCardsReverse();
        //    }


        //}



        #region  设置单张扑克UI (参数 大小 花色   扑克位置索引)
        /// <summary>
        /// 设置单张扑克UI (参数 大小 花色   扑克位置索引)
        /// </summary>
        public void SetPokerSprite(NiuNiu.Poker poker)
        {

            //AssetBundle更换图片
            string pokerName = poker.ToString();
            if (poker.status != NN_ENUM_POKER_STATUS.POKER_STATUS_UPWARD) pokerName = "16_1";
            if (string.Compare(pokerName, positiveImage.sprite.name) != 0)
            {

                if (poker.size == 0) pokerName = "16_1";
                Sprite currSprite = null;
                string pathK = string.Format("download/{0}/source/uisource/gameuisource/poker.drb", ConstDefine.GAME_NAME);
                currSprite = AssetBundleManager.Instance.LoadSprite(pathK, pokerName);

                if (currSprite != null)
                {
                    positiveImage.sprite = currSprite;
                }

            }
             

            if (poker.status == NN_ENUM_POKER_STATUS.POKER_STATUS_UPWARD)
            {
               FlipCardsForward();
                //m_ShowPokerAni[pokerIndex].Restart();
            }
            //else
            //{
            //    _pokersList[pokerIndex].transform.localPosition = UIPokerHide;
            //}




        }
        #endregion










        public void InitFlipCardsAni()
        {
            isBeenPlayed = false;
            positive.SetActive(false);
            reverse.SetActive(true);
        }
        /// <summary>
        /// 正向翻牌
        /// </summary>
      public  void FlipCardsForward()
        {
            if (isBeenPlayed) return;
            isBeenPlayed = true;

           
            StartCoroutine(FlipCards(true));
            //JuYou.MaJiangCtrl_JuYou
        }

        /// <summary>
        /// 反向翻牌
        /// </summary>
        void FlipCardsReverse()
        {

            StartCoroutine(FlipCards(false));
        }

        IEnumerator FlipCards(bool isForward)
        {
            yield return 0;
            //正向旋转
            flipCardsAni.OnComplete(

                () => {

                    positive.SetActive(isForward);
                    reverse.SetActive(!isForward);
                    flipCardsAni.PlayBackwards();
                }
                ).Restart();
            //transform.localEulerAngles = new Vector3(0, 90, 0);
          
        }


    }
}