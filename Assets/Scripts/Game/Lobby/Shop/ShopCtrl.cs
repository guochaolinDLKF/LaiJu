//===================================================
//Author      : DRB
//CreateTime  ：4/7/2017 3:40:21 PM
//Description ：商城模块控制器
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCtrl : SystemCtrlBase<ShopCtrl>, ISystemCtrl
{
    private UIShopWindow m_UIShopWindow;

    private UIPaymentView m_UIPaymentView;

    private bool m_isBusy;

    private bool m_isHasOrder;//当前是否有没完成的订单

    private int m_CurrentSelectId;//当前选择商品Id

    private int m_CurrentSelectChannel;//当前选择渠道

    private bool m_isPrompted;

    private List<GoodsEntity> m_GoodsList = new List<GoodsEntity>();

#if UNITY_EDITOR
    private int m_Channel = 2;
#elif UNITY_STANDALONE_WIN
    private int m_Channel = 2;
#elif UNITY_IPHONE
    private int m_Channel = 2;
#elif UNITY_ANDROID
    private int m_Channel = 1;
#endif


    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {        
        Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        dic.Add("btnPaymentViewAlipay", OnBtnPaymentViewAlipay);//支付宝支付按钮
        dic.Add("btnPaymentViewWeChatPay", OnBtnPaymentViewWeChatPay);//微信支付按钮
        return dic;
    }

    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.Shop:
                OpenShopView();
                break;
        }
    }

    #region OpenShopView 打开商城窗口
    /// <summary>
    /// 打开商城窗口
    /// </summary>
    private void OpenShopView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Shop, (GameObject go) =>
        {
            m_UIShopWindow = go.GetComponent<UIShopWindow>();
            RequestShopConfig();
        });
    }
    #endregion

    #region OpenPaymentView 打开支付方式选择窗口
    /// <summary>
    /// 打开支付方式选择窗口
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="price"></param>
    private void OpenPaymentView(int amount, int price)
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Payment, (GameObject go) =>
        {
            m_UIPaymentView = go.GetComponent<UIPaymentView>();
            m_UIPaymentView.SetUI(amount, price);
        });
    }
    #endregion

    #region OnBtnPaymentViewWeChatPay 微信支付按钮点击
    /// <summary>
    /// 微信支付按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPaymentViewWeChatPay(object[] obj)
    {
        m_CurrentSelectChannel = 401;
        WXPayOrder(m_CurrentSelectId, m_CurrentSelectChannel);
    }
    #endregion

    #region OnBtnPaymentViewAlipay 支付宝支付按钮点击
    /// <summary>
    /// 支付宝支付按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPaymentViewAlipay(object[] obj)
    {
        m_CurrentSelectChannel = 402;
        WXPayOrder(m_CurrentSelectId, m_CurrentSelectChannel);
    }
    #endregion

    #region OnPropClick 商品点击
    /// <summary>
    /// 商品点击
    /// </summary>
    /// <param name="propId"></param>
    private void OnPropClick(int goodsId)
    {
#if IS_DAZHONG
        if (AccountProxy.Instance.CurrentAccountEntity.codebind == 0 && !m_isPrompted)
        {
            m_isPrompted = true;
            ShowMessage("提示", "您还没有绑定邀请码，是否前往绑定？（绑定后可获得房卡奖励）", MessageViewType.OkAndCancel, OnTipOkClick, null);
            return;
        }
#endif
        if (GlobalInit.Instance.PaymentType == PaymentType.Official)
        {
            WXPayOrder(goodsId, 0);
        }
        else
        {
            GoodsEntity goods = null;
            for (int i = 0; i < m_GoodsList.Count; ++i)
            {
                if (m_GoodsList[i].id == goodsId)
                {
                    goods = m_GoodsList[i];
                }
            }
            OpenPaymentView(goods.amount, goods.money);
            m_CurrentSelectId = goodsId;
        }
    }
    #endregion

    #region OnTipOkClick 绑定提示确认按钮点击
    /// <summary>
    /// 绑定提示确认按钮点击
    /// </summary>
    private void OnTipOkClick()
    {
        UIViewManager.Instance.OpenWindow(UIWindowType.Invite);
    }
    #endregion

    #region RequestShopConfig 请求商店配置
    /// <summary>
    /// 请求商店配置
    /// </summary>
    private void RequestShopConfig()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/payConfig/",OnRequestShopConfigCallBack,true,"payConfig",dic);
    }
    #endregion

    #region OnRequestShopConfigCallBack 请求商店配置回调
    /// <summary>
    /// 请求商店配置回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestShopConfigCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            ShowMessage("提示", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("提示",args.Value.msg);
                return;
            }
            TransferData data = new TransferData();
            List<TransferData> lstShop = new List<TransferData>();

            m_GoodsList.Clear();

            for (int i = 0; i < args.Value.data.Count; ++i)
            {
                LitJson.JsonData jsonData = args.Value.data[i];
                int id = jsonData["id"].ToString().ToInt();
                string name = jsonData["name"].ToString();
                string iosCode = jsonData["iosCode"].ToString();
                int money = jsonData["money"].ToString().ToInt();
                int amount = jsonData["amount"].ToString().ToInt();
                int giveCount = jsonData["give"].ToString().ToInt();
                bool isHot = jsonData["isHot"].ToString().ToBool();
                string icon = jsonData["icon"].ToString();

                TransferData shopData = new TransferData();
                shopData.SetValue("ShopId",id);
                shopData.SetValue("ShopName",name);
                shopData.SetValue("IosCode",iosCode);
                shopData.SetValue("Money",money);
                shopData.SetValue("Amount",amount);
                shopData.SetValue("GiveCount",giveCount);
                shopData.SetValue("IsHot",isHot);
                shopData.SetValue("Icon",icon);
                lstShop.Add(shopData);

                GoodsEntity goods = new GoodsEntity()
                {
                    amount = amount,
                    giveCount = giveCount,
                    icon = icon,
                    id = id,
                    iosCode = iosCode,
                    isHot = isHot,
                    money = money,
                    name = name
                };

                m_GoodsList.Add(goods);
            }
            data.SetValue("ShopList", lstShop);
            data.SetValue("BindCode", AccountProxy.Instance.CurrentAccountEntity.codebind);
            m_UIShopWindow.SetUI(data, OnPropClick, OnBtnInfoClick);
        }
    }

    private void OnBtnInfoClick(string content)
    {
        SDK.Instance.CopyTextToClipboard(content);

        UIViewManager.Instance.ShowTip("已复制到剪切板");
    }
    #endregion


   


    #region WXPayOrder 微信支付订单
    /// <summary>
    /// 微信支付订单
    /// </summary>
    /// <param name="propId"></param>
    /// <param name="channelId"></param>
    private void WXPayOrder(int propId, int channelId)
    {
        if (m_isBusy) return;
#if UNITY_IPHONE
        if (m_isHasOrder && GlobalInit.Instance.PaymentType == PaymentType.Official)
        {
            ShowMessage("提示","当前有没处理完的订单");
            return;
        }
        m_isHasOrder = true;
#endif

        m_isBusy = true;

        UIViewManager.Instance.ShowWait();
        Dictionary<string, object> dic = new Dictionary<string, object>();
        AccountEntity entity = AccountProxy.Instance.CurrentAccountEntity;
        dic["passportId"] = entity.passportId;
        dic["token"] = entity.token;
        dic["configId"] = propId;

        if (SystemProxy.Instance.IsInstallWeChat && SystemProxy.Instance.IsOpenWXLogin)
        {
            dic["channelId"] = 1;
        }
        else
        {
            dic["channelId"] = m_Channel;
        }

#if UNITY_IPHONE
        if (GlobalInit.Instance.PaymentType == PaymentType.Official)
        {
            dic["channelId"] = 2;
        }
#endif

        if (channelId != 0)
        {
            dic["channelId"] = channelId;
        }
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrWXPayOrder, OnWXPayOrderCallBack, true, ConstDefine.HTTPFuncWXPayOrder, dic);
    }
    #endregion

    #region OnWXPayOrderCallBack 微信支付订单回调
    /// <summary>
    /// 微信支付订单回调
    /// </summary>
    /// <param name="args"></param>
    private void OnWXPayOrderCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        UIViewManager.Instance.CloseWait();
        if (args.HasError)
        {
            ShowMessage("错误", "网络连接失败");
            m_isHasOrder = false;
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("错误", args.Value.msg);
                m_isHasOrder = false;
                return;
            }
            string channel = m_Channel.ToString();

            if (SystemProxy.Instance.IsInstallWeChat && SystemProxy.Instance.IsOpenWXLogin)
            {
                channel = "1";
            }
            if (m_CurrentSelectChannel != 0)
            {
                channel = m_CurrentSelectChannel.ToString();
                m_CurrentSelectChannel = 0;
            }

#if UNITY_IPHONE
            if (GlobalInit.Instance.PaymentType == PaymentType.Official)
            {
                SDK.Instance.ApplePay(args.Value.data.ToJson(), channel);
            }
            else
            {
                if (SystemProxy.Instance.IsOpenWXLogin && SystemProxy.Instance.IsInstallWeChat)
                {
                    SDK.Instance.WXPay(args.Value.data.ToJson(), channel);
                }
                else
                {
                    SDK.Instance.ApplePay(args.Value.data.ToJson(), channel);
                }
            }
#else
            SDK.Instance.WXPay(args.Value.data.ToJson(), channel);
#endif
        }
    }
    #endregion

    #region Receipt 发送收据
    /// <summary>
    /// 发送收据
    /// </summary>
    /// <param name="orderId">订单号</param>
    /// <param name="receipt">收据信息</param>
    public void Receipt(string orderId, string receipt)
    {
        if (string.IsNullOrEmpty(orderId))
        {
            m_isHasOrder = false;
            return;
        }
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["orderId"] = orderId;
        dic["receipt"] = receipt;
        PlayerPrefs.SetString("OrderId", orderId);
        PlayerPrefs.SetString("Receipt", receipt);
        UIViewManager.Instance.ShowWait();
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrVerifyPay, OnReceiptCallBack, true, ConstDefine.HTTPFuncVerifyPay, dic);
    }
    #endregion

    #region OnReceiptCallBack 发送收据回调
    /// <summary>
    /// 发送收据回调
    /// </summary>
    /// <param name="args"></param>
    private void OnReceiptCallBack(NetWorkHttp.CallBackArgs args)
    {
        UIViewManager.Instance.CloseWait();
        if (args.HasError)
        {
            ShowMessage("错误", "网络连接失败,正在重新请求服务器");
            if (PlayerPrefs.HasKey("Receipt"))
            {
                Receipt(PlayerPrefs.GetString("OrderId"), PlayerPrefs.GetString("Receipt"));
            }
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("错误", args.Value.msg);
                return;
            }
            m_isHasOrder = false;
            PlayerPrefs.DeleteKey("OrderId");
            PlayerPrefs.DeleteKey("Receipt");
            int cardsCount = args.Value.data["cards"].ToString().ToInt();
            AccountProxy.Instance.SetCards(cardsCount);
        }
    }
    #endregion
}
