//===================================================
//Author      : DRB
//CreateTime  ：12/2/2017 6:06:32 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShiSanZhangPokerCtrl : MonoBehaviour {

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
        flipCardsAni = transform.DOScaleX(0, 0.1f).SetEase(Ease.Linear).SetAutoKill(false).Pause();
    }



    /// <summary>
    /// 正向翻牌
    /// </summary>
    public void FlipCardsForward()
    {
        if (isBeenPlayed) return;
        isBeenPlayed = true;


        StartCoroutine(FlipCards(true));
        //JuYou.MaJiangCtrl_JuYou
    }


    IEnumerator FlipCards(bool isForward)
    {
        yield return 0;
        //正向旋转
        flipCardsAni.OnComplete(

            () =>
            {

                positive.SetActive(isForward);
                reverse.SetActive(!isForward);
                flipCardsAni.PlayBackwards();
            }
            ).Restart();
        //transform.localEulerAngles = new Vector3(0, 90, 0);

    }

}
