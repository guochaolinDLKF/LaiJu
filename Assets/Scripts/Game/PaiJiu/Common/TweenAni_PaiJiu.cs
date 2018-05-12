//===================================================
//Author      : WZQ
//CreateTime  ：7/21/2017 9:48:26 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class TweenAni_PaiJiu 
{

    /// <summary>
    ///  分数动画
    /// </summary>
    /// <param name="graphic"></param>
    public static void FlyTo(Transform rt, Action  action)
    {
        //Vector3 pos = graphic.localPosition;

        rt.gameObject.SetActive(true);
        
        //Color c = graphic.color;
        //c.a = 0;
        //graphic.color = c;
        Sequence mySequence = DOTween.Sequence();

  
        Tweener move1 = rt.DOLocalMoveY(rt.localPosition.y + 150f, 1);
        Tweener move2 = rt.DOLocalMoveY(rt.localPosition.y + 200f, 1f);


       
        Tween magnify1 = rt.DOScale(1f, 0.5f);
        Tween magnify2 = rt.DOScale(0, 0.5f);
        mySequence.Append(move1);
        mySequence.Join(magnify1);
       
        mySequence.AppendInterval(2);
        mySequence.Append(move2);
        mySequence.Join(magnify2);

      
        mySequence.OnComplete(

            () =>
            {
                if (action != null) action();
            }


               //() =>
               //{
               //    rt.gameObject.SetActive(false);
               //    graphic.localPosition = pos;
               //}
               );
        //---------------------------------------------



    }

    /// <summary>
    /// 牌型动画
    /// </summary>
    /// <param name="rt"></param>
    public static void PokerTypeAni(Transform rt)
    {

        rt.localScale = Vector3.one * 3;
        Sequence mySequence = DOTween.Sequence();

        Tween magnify1 = rt.DOScale(1f, 0.5f);
        mySequence.Append(magnify1);
    }


    /// <summary>
    ///  弃牌动画 {未实现}
    /// </summary>
    /// <param name="rt"></param>
    /// <param name="endPos"></param>
    /// <param name="action"></param>
    public static void DiscardPoker(Transform rt, Vector3 endPos, Action action)
    {
        Sequence mySequence = DOTween.Sequence();

        Tweener moveTo = rt.DOLocalMove(endPos , 2).SetEase(Ease.InSine);
        //Tweener rotateTo = rt.DOLocalRotate(rt.localPosition.y + 200f, 1f);

    }

    /// <summary>
    /// 金币流动动画 ( tran 起点 终点 改为回调)
    /// </summary>
    /// <param name="rt"></param>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="isAutoOff"></param>
    public static void GoldFlowingAni(Transform rt, Transform goldImage, Vector3 startPos, Vector3 endPos, Action action=null)
    {
        rt.position = startPos;
        rt.localScale =Vector3.one*0.5f;
        Sequence mySequence = DOTween.Sequence();

        Tween magnify1 = rt.DOMove(endPos, 1);

        Tween Scale1 = rt.DOScale(1, 0.5f);
        Tween Scale2 = rt.DOScale(0.5f, 0.5f);
        //Tween magnify2=rt.do
        mySequence.Append(Scale1);
        mySequence.Join(magnify1).SetEase(Ease.InQuint);
        mySequence.Append(Scale2);
        mySequence.OnComplete<Sequence>(
           () =>
           {
               if (action != null) action();
           }

            );


   
        //if (isAutoOff)
        //{

        //    mySequence.OnComplete<Sequence>(
        //       () =>
        //       {
        //           if (goldImage != null) goldImage.localPosition = Vector3.zero;
        //           ItemPool_NiuNiu.Instance.PushToPool(rt.gameObject);
        //           //rt.gameObject.SetActive(false)
        //       }

        //        );

        //}



    }



}
