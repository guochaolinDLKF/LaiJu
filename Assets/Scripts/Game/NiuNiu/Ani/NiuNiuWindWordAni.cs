//===================================================
//Author      : WZQ
//CreateTime  ：5/22/2017 1:52:43 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace NiuNiu
{
    public class NiuNiuWindWordAni
    {



        /// <summary>
        ///  分数动画
        /// </summary>
        /// <param name="graphic"></param>
        public static void FlyTo(RectTransform graphic)
        {
            Vector3 pos = graphic.localPosition;

            graphic.gameObject.SetActive(true);
            RectTransform rt = graphic;
            //Color c = graphic.color;
            //c.a = 0;
            //graphic.color = c;
            Sequence mySequence = DOTween.Sequence();

            //int height = Mathf.CeilToInt(1334 / camera.aspect);




            Tweener move1 = rt.DOLocalMoveY(rt.localPosition.y + 100f, 1);
            Tweener move2 = rt.DOLocalMoveY(rt.localPosition.y + 200f, 1f);
       

            //Tweener move1 = rt.DOMoveY(rt.position.y + 50, 1f);
            //Tweener move2 = rt.DOMoveY(rt.position.y + 100, 1f);

            //Tweener alpha1 = graphic.DOColor(new Color(c.r, c.g, c.b, 1), 0.5f);
            //Tweener alpha2 = graphic.DOColor(new Color(c.r, c.g, c.b, 0), 0.5f);
            Tween magnify1 = rt.DOScale(1f, 0.5f);
            Tween magnify2 = rt.DOScale(0, 0.5f);
            mySequence.Append(move1);
            mySequence.Join(magnify1);
            //mySequence.Join(alpha1);
            mySequence.AppendInterval(2);
            mySequence.Append(move2);
            mySequence.Join(magnify2);
            //mySequence.Join(alpha2);

            mySequence.OnComplete<Sequence>(
                   () =>
                   {
                       rt.gameObject.SetActive(false);
                       graphic.localPosition = pos;
                   }
                    );
            //---------------------------------------------



        }


        /// <summary>
        /// 弹出效果
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="onOff"></param>
        public static void PopUp(Transform rt, bool onOff = true)
        {

            rt.gameObject.SetActive(true);
            rt.localScale = Vector3.zero;

            Sequence mySequence = DOTween.Sequence();
            //Tween magnify1 = rt.DOScale(1.5f, 0.5f);
            Tween magnify2 = rt.DOScale(1, 0.4f);
            //mySequence.Append(magnify1);
            mySequence.Append(magnify2);

            if (!onOff)
            {

                mySequence.OnComplete<Sequence>(
                   () => rt.gameObject.SetActive(false)
                    );

            }


        }


        /// <summary>
        ///  游戏结束Panel 弹出
        /// </summary>
        /// <param name="rt"></param>
        public static void GameOverViewAni(Transform rt)
        {

            rt.localScale = Vector3.zero;
            rt.localPosition = Vector3.zero;
            rt.gameObject.SetActive(true);
            DOTweenAnimation ani = rt.GetComponent<DOTweenAnimation>();

            if (ani != null)
            {
                ani.DOPlay();
            }
            else
            {
                Sequence mySequence = DOTween.Sequence();
                Tween magnify1 = rt.DOScale(1f, 0.5f);
                mySequence.Append(magnify1);


            }



        }

        //牌型动画
        public static void PokerTypeAni(RectTransform rt)
        {
            //rt.gameObject.SetActive(true);
            rt.localScale = Vector3.one*3;

            Sequence mySequence = DOTween.Sequence();

            Tween magnify1 = rt.DOScale(1f, 0.5f);    
            mySequence.Append(magnify1);         

            //if (!onOff)
            //{

            //    mySequence.OnComplete<Sequence>(
            //       () => rt.gameObject.SetActive(false)
            //        );

            //}


        }



        /// <summary>
        /// 金币流动动画 ( tran 起点 终点 是否放入对象池)
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="isAutoOff"></param>
        public static void GoldFlowingAni(Transform rt,Transform goldImage,Vector3 startPos, Vector3 endPos,bool isAutoOff=true)
        {
            rt.position = startPos;
            Sequence mySequence = DOTween.Sequence();

            Tween magnify1 = rt.DOMove(endPos, 0.6f);
            //Tween magnify2=rt.do
            mySequence.Append(magnify1);


            if (isAutoOff)
            {

                mySequence.OnComplete<Sequence>(
                   () =>
                   {
                  if(goldImage!=null)  goldImage.localPosition = Vector3.zero;
                  ItemPool_NiuNiu.Instance.PushToPool(rt.gameObject);
                   //rt.gameObject.SetActive(false)
                   }

                    );

            }



        }



        //确认庄归属时动画
        public static void ConfirmBankerAni(Transform rt)
        {
           
            rt.localScale = Vector3.one * 3;

            Sequence mySequence = DOTween.Sequence();

            Tween magnify1 = rt.DOScale(1f, 0.5f);
            
            mySequence.Append(magnify1);

       


        }













    }
}