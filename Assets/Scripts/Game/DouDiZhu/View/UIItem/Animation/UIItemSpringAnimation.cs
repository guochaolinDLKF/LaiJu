//===================================================
//Author      : DRB
//CreateTime  ：1/4/2018 5:19:55 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIItemSpringAnimation : MonoBehaviour {
    [SerializeField]
    private Transform[] flowerTrans;
    private void Awake()
    {
        for (int i = 0; i < flowerTrans.Length; i++)
        {
            flowerTrans[i].localScale = Vector3.zero;
        }
    }

    #region FlowerAnimation 花的动画
    /// <summary>
    /// 花的动画
    /// </summary>
    public void FlowerAnimation()
    {
        for (int i = 0; i < flowerTrans.Length; i++)
        {
            flowerTrans[i].localScale = Vector2.zero;
            flowerTrans[i].DOScale(1,1).SetEase(Ease.InOutBack);
        }
    }
    #endregion
}
