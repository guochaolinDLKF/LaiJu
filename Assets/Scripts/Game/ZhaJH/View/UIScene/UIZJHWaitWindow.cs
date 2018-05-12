//===================================================
//Author      : DRB
//CreateTime  ：7/17/2017 4:30:49 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 高级房等待界面显示及里面的点击事件
/// </summary>
public class UIZJHWaitWindow : UIWindowViewBase
{   
    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(ZhaJHMethodname.OnZJHWithdrawEnterRoom, WithdrawEnterRoom);       
        return dic;
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case ZhaJHMethodname.btnWithdraw:                
                SendNotification(ZhaJHMethodname.btnWithdraw);
                WithdrawEnterRoom(null);
                break;           
        }
    }
    /// <summary>
    /// 高级房服务器返回取消进入房间的方法
    /// </summary>
    /// <param name="obj"></param>
    private void WithdrawEnterRoom(TransferData obj)
    {
        this.gameObject.SetActive(false);
    }
   
}
