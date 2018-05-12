//===================================================
//Author      : CZH
//CreateTime  ：7/19/2017 10:41:24 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZhaJh;
using DG.Tweening;

/// <summary>
/// 小结算显示
/// </summary>

public class UIZJHSmallSettlement : UIWindowViewBase
{
    [SerializeField]
    private Text playerName;//玩家名字
    [SerializeField]
    private Text playerID;//玩家ID
    [SerializeField]
    private Text bureauNumber;//局数
    [SerializeField]
    private Text settlementProfit;//结算收益
    [SerializeField]
    private Text totalScore;//总分
    [SerializeField]
    private Button btnClose;//关闭按钮   
    [SerializeField]
    private RawImage touImage;//头像


    protected override void OnAwake()
    {
        base.OnAwake();
        Button[] btnArr = GetComponentsInChildren<Button>(true);
        for (int i = 0; i < btnArr.Length; i++)
        {
            EventTriggerListener.Get(btnArr[i].gameObject).onEnter += OnPointerEnter;
            EventTriggerListener.Get(btnArr[i].gameObject).onExit += OnPointerExit;
        }
    }

    public void SettlementUI(SeatEntity seat)
    {        
        playerName.text = "昵称："+seat.Nickname;        
        this.playerID.SafeSetText(string.Format("(ID:{0})", seat.PlayerId.ToString()));
        bureauNumber.text = seat.currentLoop.ToString();
        settlementProfit.text = seat.totalProfit.ToString();
        totalScore.text = seat.gold.ToString("0.00");
        TextureManager.Instance.LoadHead(seat.Avatar, OnAvatarLoadCallBack);
    }
    private void OnAvatarLoadCallBack(Texture2D tex)
    {
        touImage.texture = tex;
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnFanHui":
                SendNotification(ZhaJHMethodname.OnZJHReturnHall);
                break;
        }
    }

    /// <summary>
    /// 当鼠标离开的时候调用
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(GameObject go)
    {
        if (go.name == "btnFanHui")
        {
            go.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        }
    }
    /// <summary>
    /// 当鼠标进入时候调用
    /// 把按钮放大
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(GameObject go)
    {
        if (go.name == "btnFanHui")
        {
            go.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
        }
    }

}
