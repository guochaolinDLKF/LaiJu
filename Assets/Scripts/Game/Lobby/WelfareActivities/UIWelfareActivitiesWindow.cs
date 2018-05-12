//===================================================
//Author      : DRB
//CreateTime  ：11/30/2017 6:51:23 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GiftType
{
    Key,
    RoomCard,
    Integral,
    Again,
    Null,
    Other,
}
public enum InterfaceType
{
    Treasure,
    LotteryWheel,
}
public class UIWelfareActivitiesWindow : UIWindowViewBase
{
    #region Variable
    [SerializeField]
    private Transform lotteryWheel;
    [SerializeField]
    private Transform treasure;
    #endregion

    #region OnBtnClick 按钮事件
    /// <summary>
    /// 按钮事件
    /// </summary>
    /// <param name="go"></param>
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btn_treasure":
                //ChangeInterface(InterfaceType.Treasure);
                if (!treasure.gameObject.activeSelf)
                {
                    SendNotification("btn_treasure");
                }
                break;
            case "btn_lotteryWheel":
                if (!lotteryWheel.gameObject.activeSelf)
                {
                    SendNotification("btn_lotteryWheel");
                }
                //ChangeInterface(InterfaceType.LotteryWheel);
                break;
            case "btn_treasureRule":
                SendNotification("btn_treasureRule");
                break;
            case "btn_lotteryWheelRule":
                SendNotification("btn_lotteryWheelRule");
                break;
            case "btn_beginDraw":
                if (!lotteryWheel.GetComponent<UILotteryWheelView>().GetIsStartTrun())
                {
                    SendNotification("btn_beginDraw");
                }
                break;
            case "btn_lotteryWheelRecord":
                SendNotification("btn_lotteryWheelRecord");
                break;
            case "btn_treasureRecord":
                SendNotification("btn_treasureRecord");
                break;
        }
    }
    #endregion

    #region ChangeInterface 切换界面
    /// <summary>
    /// 切换界面
    /// </summary>
    /// <param name="str"></param>
    public void ChangeInterface(InterfaceType type)
    {
        if (type == InterfaceType.Treasure)
        {
            if (!treasure.gameObject.activeSelf)
            {
                lotteryWheel.gameObject.SetActive(false);
                treasure.gameObject.SetActive(true);
            }
        }
        else if (type == InterfaceType.LotteryWheel)
        {
            if (!lotteryWheel.gameObject.activeSelf)
            {
                lotteryWheel.gameObject.SetActive(true);
                treasure.gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region SetUI 设置UI
    /// <summary>
    /// 设置UI
    /// </summary>
    /// <param name="data"></param>
    public void SetUI(TransferData data)
    {
        SetTreasure(data);
        SetLotteryWheel(data);
    }
    /// <summary>
    /// 设置宝箱钥匙
    /// </summary>
    /// <param name="data"></param>
    public void SetTreasure(TransferData data)
    {
        treasure.gameObject.GetComponent<UITreasureView>().SetTreasure(data);
    }
    /// <summary>
    /// 设置转盘数
    /// </summary>
    /// <param name="data"></param>
    public void SetLotteryWheel(TransferData data)
    {
        lotteryWheel.gameObject.GetComponent<UILotteryWheelView>().SetLotteryWheelUI(data);
    }
    public void SetLotteryWheelSruPlus(int count)
    {
        lotteryWheel.gameObject.GetComponent<UILotteryWheelView>().SetSurPlusTimeText(count);
    }
    public void SetTreasureKey(int count)
    {
        TransferData data = new TransferData();
        data.SetValue("treasureKeyNum",count);
        treasure.gameObject.GetComponent<UITreasureView>().SetTreasure(data);
    }

    #endregion

    public void PlayerBoxAnimation(int index)
    {
        treasure.gameObject.GetComponent<UITreasureView>().PlayerBoxAnimation(index);
    }
    
    public void StartTurn(TransferData data)
    {
        lotteryWheel.GetComponent<UILotteryWheelView>().StartTurn(data);
    }
    //public string LotteryWheelGiftMessage()
    //{
    //    return lotteryWheel.GetComponent<UILotteryWheelInfo>().GetMessage();
    //}
    //public void ChangeRoomCardText(int count)
    //{
    //    roomCardText.SafeSetText(count.ToString());
    //}
    //public GiftType LotteryWheelGiftType()
    //{
    //    return lotteryWheel.GetComponent<UILotteryWheelInfo>().GetGiftType();
    //}
    //public int LotteryWheelGiftCount()
    //{
    //    return lotteryWheel.GetComponent<UILotteryWheelInfo>().GetGiftCount();
    //}
    public void OnLotteryWheelComplete()
    {
        SendNotification("OnLotteryComplete", lotteryWheel.GetComponent<UILotteryWheelInfo>().GetSurPlusTime());
    }
    public GiftType GetTargetGiftType()
    {
        return lotteryWheel.GetComponent<UILotteryWheelView>().GetTargetGiftType();
    }
    public string GetTargetGifName()
    {
        return lotteryWheel.GetComponent<UILotteryWheelView>().GetTargetGiftName();
    }
}
