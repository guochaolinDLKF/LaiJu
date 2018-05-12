//===================================================
//Author      : WZQ
//CreateTime  ：5/18/2017 1:50:34 PM
//Description ：飘分显示
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace NiuNiu
{
    public class NiuNiuEarningsUI : MonoBehaviour
    {

        public RectTransform _earnings;                     //分数物体
                                                            //private DOTweenAnimation _doTweenAni;               //动画脚本

        //private Vector3 initialScale;                      //
        private Vector3 initialPos;                       //初始位置

        [SerializeField]
        private Image _symbol;                            //符号图片

        [SerializeField]
        private Image _hundredPlace;                     //百位图片

        [SerializeField]
        private Image _tensPlace;                        //十位图片

        [SerializeField]
        private Image _onesPlace;                        //个位图片

        private string pathK="";

        private Sequence mySequence;//图片动画
                                    // Use this for initialization

        void Awake()
        {
            initialPos = _earnings.localPosition;
            FlyTo(_earnings).Pause();
        }
        void Start()
        {
            //_doTweenAni = _earnings.GetComponent<DOTweenAnimation>();
            //initialScale = earnings.localScale;

            InitAni();
        }


        //播放完成后
        public void InitAni()
        {
            _earnings.gameObject.SetActive(false);
            _earnings.localScale = Vector3.zero;
            _earnings.localPosition = initialPos;
            //_earnings.localPosition = initialPos;

        }




        public void AgainPlay(int earningsValue)
        {
            //_symbol.sprite = earningsValue >= 0 ? ResourceReference.Instance.NumSymbolSprites[1] : ResourceReference.Instance.NumSymbolSprites[0];

            //string earningsStr = string.Format("{0:D2}", earningsValue);
            //char tensStr = earningsStr[earningsStr.Length - 2];
            //char onesStr = earningsStr[earningsStr.Length - 1];

            //_tensPlace.sprite = ResourceReference.Instance.NumEarningsSprites[int.Parse(tensStr.ToString())];
            //_onesPlace.sprite = ResourceReference.Instance.NumEarningsSprites[int.Parse(onesStr.ToString())];

            //_doTweenAni.DORestart();  
            //_earnings.gameObject.SetActive(true);



            //---------------------
            //if (m_goldImages == null || m_goldImages.Length < 1)
            //{
            //    return;
            //}

             pathK = string.Format("download/{0}/source/uisource/niuniu.drb", ConstDefine.GAME_NAME);

            //符号
            if (_symbol != null)
            {
                string symbolName = earningsValue >= 0 ? "img_jia" : "img_jian";

                Sprite currSprite = null;
                currSprite = AssetBundleManager.Instance.LoadSprite(pathK, symbolName);

                if (currSprite != null)
                {
                    _symbol.sprite = currSprite;
                }

            }


            string earningsStr = string.Format("{0:D3}", earningsValue);
            char hundredStr = earningsStr[earningsStr.Length - 3];
            char tensStr = earningsStr[earningsStr.Length - 2];
            char onesStr = earningsStr[earningsStr.Length - 1];

            SetSprite(_hundredPlace, earningsValue, hundredStr);
            SetSprite(_tensPlace, earningsValue, tensStr);
            SetSprite(_onesPlace, earningsValue, onesStr);

           
            _onesPlace.gameObject.SetActive(true);
            _tensPlace.gameObject.SetActive(Mathf.Abs(earningsValue) >= Mathf.Pow(10, 1));
            _hundredPlace.gameObject.SetActive(Mathf.Abs(earningsValue) >= Mathf.Pow(10, 2));


            //----------------------





            //动画 1 4 5 右上 3 6 左上  2直上
            //NiuNiuWindWordAni.FlyTo(_earnings);
            if (mySequence != null)
            {

                _earnings.gameObject.SetActive(true);
                mySequence.Restart();
               
            } 

        }


        private void  SetSprite(Image setSprite, int earningsValue , char strChar )
        {
           
            if (setSprite != null)
            {

                string onesStrName = earningsValue >= 0 ? "img_fenshu" + strChar : "jian" + strChar;
                Sprite currSprite = null;
                currSprite = AssetBundleManager.Instance.LoadSprite(pathK, onesStrName);

                if (currSprite != null)
                {
                    setSprite.sprite = currSprite;
                }

            }




        }

        /// <summary>
        ///  分数动画
        /// </summary>
        /// <param name="graphic"></param>
        public Sequence FlyTo(RectTransform graphic)
        {
            
            Vector3 pos = graphic.localPosition;

            
            RectTransform rt = graphic;
           
             mySequence = DOTween.Sequence().SetAutoKill(false);

            Tweener move1 = rt.DOLocalMoveY(rt.localPosition.y + 150f, 1).SetAutoKill(false);
            Tweener move2 = rt.DOLocalMoveY(rt.localPosition.y + 200f, 1f).SetAutoKill(false);
  
            Tween magnify1 = rt.DOScale(1f, 0.5f).SetAutoKill(false);
            Tween magnify2 = rt.DOScale(0, 0.5f).SetAutoKill(false);
            mySequence.Append(move1);
            mySequence.Join(magnify1);
        
            mySequence.AppendInterval(2);
            mySequence.Append(move2);
            mySequence.Join(magnify2);
          

            mySequence.OnComplete<Sequence>(
                   () =>
                   {
                       rt.gameObject.SetActive(false);
                       graphic.localPosition = pos;
                      
                   }
                    );
            return mySequence;

        }





    }
}