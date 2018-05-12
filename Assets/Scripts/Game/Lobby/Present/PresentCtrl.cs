//===================================================
//Author      : DRB
//CreateTime  ：4/17/2017 7:46:56 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using com.oegame.mahjong.protobuf;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PresentCtrl : SystemCtrlBase<PresentCtrl>, ISystemCtrl
{
    private UIPresentView m_UIPresentView;//赠送礼物界面

    private bool m_isBusy;

    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        dic.Add(ConstDefine.BtnPresentViewPresent, OnPresentViewPresentClick);
        dic.Add(ConstDefine.BtnPresentViewReturn, OnPresentViewReturnClick);
        dic.Add(ConstDefine.BtnPresentViewNext, OnPresentViewNextClick);
        return dic;
    }

    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.Present:
                UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Present, (GameObject go) =>
                {
                    m_UIPresentView = go.GetComponent<UIPresentView>();
                });
                break;
        }
    }

    #region OnPresentViewNextClick 赠送礼物界面下一步按钮点击
    /// <summary>
    /// 赠送礼物界面下一步按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnPresentViewNextClick(object[] obj)
    {
        if (string.IsNullOrEmpty(m_UIPresentView.m_InputId.text))
        {
            ShowMessage("错误提示", "请输入玩家Id");
            return;
        }
        if (m_UIPresentView.m_InputId.text.ToInt() == AccountProxy.Instance.CurrentAccountEntity.passportId)
        {
            ShowMessage("错误提示","不能送给自己");
            return;
        }
        RequestPlayerInfo(m_UIPresentView.m_InputId.text.ToInt());
    }
    #endregion

    #region OnPresentViewReturnClick 赠送礼品界面返回按钮点击
    /// <summary>
    /// 赠送礼品界面返回按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnPresentViewReturnClick(object[] obj)
    {
        m_UIPresentView.SetUI(1);
    }
    #endregion

    #region OnPresentViewPresentClick 赠送礼物界面赠送按钮点击
    /// <summary>
    /// 赠送礼物界面赠送按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnPresentViewPresentClick(object[] obj)
    {
        if (m_UIPresentView.index < 1)
        {
            ShowMessage("提示","请输入赠送数量");
            return;
        }
        Present(m_UIPresentView.m_InputId.text.ToInt(), m_UIPresentView.index);
    }
    #endregion

    #region Present 赠送
    /// <summary>
    /// 赠送
    /// </summary>
    public void Present(int toId, int amount)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        AccountEntity entity = AccountProxy.Instance.CurrentAccountEntity;
        dic["passportId"] = entity.passportId;
        dic["token"] = entity.token;
        dic["toId"] = toId;
        dic["itemId"] = 1;
        dic["amount"] = amount;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrGive, OnPresentCallBack, true, ConstDefine.HTTPFuncGive,dic);
    }
    #endregion

    #region OnPresentCallBack 赠送回调
    /// <summary>
    /// 赠送回调
    /// </summary>
    /// <param name="args"></param>
    private void OnPresentCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            ShowMessage("错误提示", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("错误", args.Value.msg);
                return;
            }
            ShowMessage("提示", "赠送成功");
            int cardCount = args.Value.data["cards"].ToString().ToInt();
            AccountProxy.Instance.SetCards(cardCount);
            //m_UIPresentView.Close();
        }
    }
    #endregion

    #region RequestPlayerInfo 获取玩家信息
    /// <summary>
    /// 获取玩家信息
    /// </summary>
    /// <param name="playerId"></param>
    private void RequestPlayerInfo(int playerId)
    {
        if (m_isBusy) return;
        m_isBusy = true;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        AccountEntity entity = AccountProxy.Instance.CurrentAccountEntity;
        dic["passportId"] = entity.passportId;
        dic["token"] = entity.token;
        dic["toId"] = playerId;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrPlayer, OnQueryPlayerInfoCallBack, true, ConstDefine.HTTPFuncPlayer, dic);
    }
    #endregion

    #region OnQueryPlayerInfoCallBack 获取玩家信息回调
    /// <summary>
    /// 获取玩家信息回调
    /// </summary>
    /// <param name="args"></param>
    private void OnQueryPlayerInfoCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        if (args.HasError)
        {
            UIViewManager.Instance.ShowMessage("错误", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                UIViewManager.Instance.ShowMessage("错误", args.Value.msg);
                return;
            }
            PlayerEntity entity = LitJson.JsonMapper.ToObject<PlayerEntity>(args.Value.data.ToJson());
            m_UIPresentView.SetUI(2, entity.id, entity.avatar, entity.nickname);
        }
    }
    #endregion
}
