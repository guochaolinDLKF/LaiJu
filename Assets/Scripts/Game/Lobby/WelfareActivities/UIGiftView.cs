//===================================================
//Author      : DRB
//CreateTime  ：1/16/2018 1:09:27 PM
//Description ：
//===================================================
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGiftView : UIViewBase
{
    [SerializeField]
    private Transform giftTrans;
    [SerializeField]
    private Image rotateLight;
    [SerializeField]
    private Image giftShadding;
    [SerializeField]
    private DOTweenAnimation giftMoveAnimation;
    [SerializeField]
    private DOTweenAnimation lightBgAnimation;
    [SerializeField]
    private Image shaddingImage;
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        if (go.name == "btnMakeSure")
        {
            rotateLight.SafeSetActive(false);
            giftShadding.SafeSetActive(false);
            giftMoveAnimation.SafeSetActive(false);
            giftTrans.gameObject.SetActive(false);
            lightBgAnimation.SafeSetActive(false);
        }
    }
    public void SetShadding(bool isShow)
    {
        if (isShow)
        {
            shaddingImage.color = Color.white;
        }
        else
        {
            shaddingImage.color = Color.clear;
        }
    }
}
