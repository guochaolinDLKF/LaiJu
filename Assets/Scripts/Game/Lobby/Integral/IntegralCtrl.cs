//===================================================
//Author      : DRB
//CreateTime  ：11/28/2017 2:09:35 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegralCtrl : SystemCtrlBase<IntegralCtrl>, ISystemCtrl
{
    #region  Variable

    private UIIntegralWindow m_UIIntegralWindow;

    private UIExchangeRecordWindow m_UIExchangeRecordWindow;

    private UIPlayerReceiveRecordInfoView m_UIPlayerReceiveRecordInfoView;

    private PrizeType prizeType;

    #endregion

    #region Dictionary And OpenView
    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        
        dic.Add("btn_integralRule", OnBtnIntegralRule);
        dic.Add("btn_integralRecord", OnBtnIntegralRecord);
        dic.Add("btn_receive", OnBtnIntegralRecordReceive);
        dic.Add("btn_spendGift", OnBtnIntegralSpendGiftReceive);
        dic.Add("btn_SendPlayerMessage", OnBtnSendPlayerMessage);

        return dic;
    }
    
    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.Integral:
                OnBtnIntegral();
                break;
            default:
                break;
        }

    }
    #endregion

    #region 按钮
    #region OnBtnSendPlayerMessage 发送玩家信息按钮
    /// <summary>
    /// 发送玩家信息按钮
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnSendPlayerMessage(object[] obj)
    {
        RequestSendMessage(obj[0], obj[1], obj[2],obj[3],obj[4]);
    }
    #endregion

    #region OnBtnIntegralSpendGiftReceive 积分兑换礼品按钮
    /// <summary> 
    /// 积分兑换礼品按钮
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnIntegralSpendGiftReceive(object[] obj)
    {
        RequestSpendIntegralExchangeGift(obj[0]);
        //SpendIntegralExchangeGiftlCallBackTest();
    }
    #endregion

    #region OnBtnIntegralRecordReceive 礼物申请领取按钮
    /// <summary>
    /// 礼物申请领取按钮
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnIntegralRecordReceive(object[] obj)
    {
        int index = (int)obj[0];
        if (prizeType == (PrizeType)obj[1])
        {
            OnRequestGiftReceive(index);
            //m_UIPlayerReceiveRecordInfoView = m_UIExchangeRecordWindow.OpenPrompt();
        }
    }
    #endregion

    #region OnBtnIntegral 打开积分界面按钮
    /// <summary>
    /// 打开积分界面按钮
    /// </summary>
    private void OnBtnIntegral()
    {
        m_UIIntegralWindow = UIViewUtil.Instance.LoadWindow("IntegralSpend").GetComponent<UIIntegralWindow>();
        RequestIntegralInfo();
        //IntegralText();
    }
    #endregion

    #region OnBtnIntegralRule 打开积分规则界面按钮
    /// <summary>
    /// 打开积分规则界面按钮
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnIntegralRule(object[] obj)
    {
        UIViewUtil.Instance.LoadWindow("IntegralRule");
    }
    #endregion

    #region OnBtnIntegralRecord 打开积分兑换界面按钮
    /// <summary>
    /// 打开积分兑换界面按钮
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnIntegralRecord(object[] obj)
    {
        m_UIExchangeRecordWindow = UIViewUtil.Instance.LoadWindow("IntegralRecord").GetComponent<UIExchangeRecordWindow>();
        RequestExchangeRecordInfo();
        //IntegralRecordTest();
    }
    #endregion

    #endregion

    #region 测试

    #region IntegralText 测试积分兑换功能
    /// <summary>
    /// 测试积分兑换功能
    /// </summary>
    private void IntegralText()
    {
        IntegralEntityExternal integralEntityExternal;
        List<IntegralEntity> lstIntegralEntityTest = new List<IntegralEntity>();
        IntegralEntity integralEntity001 = new IntegralEntity();
        integralEntity001.id = 0;
        integralEntity001.img_url = "http://img.mp.itc.cn/upload/20161120/dcc2dd640a604aaea092e74f6855dd4b_th.jpeg";
        integralEntity001.name = "钢之炼金术师";
        integralEntity001.need_score = 9099;
        IntegralEntity integralEntity002 = new IntegralEntity();
        integralEntity002.id = 1;
        integralEntity002.img_url = "http://a4.topitme.com/o/201012/17/12925697494423.jpg";
        integralEntity002.name = "樱木和流川枫";
        integralEntity002.need_score = 9999;
        lstIntegralEntityTest.Add(integralEntity001);
        lstIntegralEntityTest.Add(integralEntity002);
        integralEntityExternal = new IntegralEntityExternal();
        integralEntityExternal.score = 10086;
        integralEntityExternal.prize = lstIntegralEntityTest;

        IntegralEntityExternal integralEntityAndNum = integralEntityExternal;

        TransferData data = new TransferData();
        List<TransferData> lstIntegralEntityData = new List<TransferData>();

        List<IntegralEntity> lstIntegralEntity = integralEntityAndNum.prize;

        for (int i = 0; i < lstIntegralEntity.Count; i++)
        {
            TransferData integralEntity = new TransferData();
            integralEntity.SetValue("integralEntity", lstIntegralEntity[i]);
            lstIntegralEntityData.Add(integralEntity);
        }
        data.SetValue("lstData", lstIntegralEntityData);
        data.SetValue("Count", integralEntityAndNum.score);

        if (m_UIIntegralWindow)
        {
            m_UIIntegralWindow.SetUI(data);
        }
    }
    #endregion

    #region  IntegralRecordTest  积分记录功能测试
    /// <summary>
    /// 积分记录功能
    /// </summary>
    private void IntegralRecordTest()
    {

        List<ExchangeRecordEntity> lstIntegralRecordEntity = new List<ExchangeRecordEntity>();
        ExchangeRecordEntity integralRecordEntity01 = new ExchangeRecordEntity();
        integralRecordEntity01.id = 0;
        integralRecordEntity01.add_time = "17:24";
        integralRecordEntity01.date = "2017.12.28";
        integralRecordEntity01.name = "樱木花道全国大赛限量典藏版";
        integralRecordEntity01.status = 1;
        ExchangeRecordEntity integralRecordEntity02 = new ExchangeRecordEntity();

        integralRecordEntity02.id = 1;
        integralRecordEntity02.add_time = "07:24";
        integralRecordEntity02.date = "2017.08.14";
        integralRecordEntity02.name = "钢之炼金术师FA限量典藏版";
        integralRecordEntity02.status = 1;
        lstIntegralRecordEntity.Add(integralRecordEntity01);
        lstIntegralRecordEntity.Add(integralRecordEntity02);
        
        List<ExchangeRecordEntity> integralRecordEntity = lstIntegralRecordEntity;

        List<TransferData> lstIntegralEntityData = new List<TransferData>();
        TransferData data = new TransferData();

        for (int i = 0; i < integralRecordEntity.Count; i++)
        {
            TransferData recordData = new TransferData();
            recordData.SetValue("integralRecordEntity", integralRecordEntity[i]);
            lstIntegralEntityData.Add(recordData);
            recordData.SetValue("PrizeType", prizeType);
        }

        data.SetValue("lstRecord", lstIntegralEntityData);

        if (m_UIExchangeRecordWindow)
        {
            m_UIExchangeRecordWindow.SetUI(data);
        }
    }
    #endregion

    #region SendMessageCallBack 接收消息后显示的微信号测试
    /// <summary>
    /// 接收消息后显示的微信号
    /// </summary>
    private void SendMessageCallBack()
    {
        string wxCustonmerService = "Shy";

        TransferData data = new TransferData();

        data.SetValue("wxCustonmerService", wxCustonmerService);

        UIViewUtil.Instance.LoadWindow("ApplySuccessdMessage").GetComponent<UIApplySuccessdMessageView>().SetUI(data);

        if (m_UIExchangeRecordWindow)
        {
            m_UIExchangeRecordWindow.SetExchangeRecord(data);
        }

    }
    #endregion

    #endregion

    #region 发送消息与回调消息

    #region RequestIntegralInfo 请求积分兑换相关
    /// <summary>
    /// 请求积分兑换相关
    /// </summary>
    private void RequestIntegralInfo()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/scorePrize/", OnRequestIntegralCallBack, true, "scorePrize", dic);
    }
    #endregion

    #region OnRequestIntegralCallBack 请求积分兑换相关回调
    /// <summary>
    /// 请求客服信息回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestIntegralCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            ShowMessage("提示", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("提示", args.Value.msg);
                return;
            }
            if (args.Value.data == null || args.Value.data.Count == 0) return;

            IntegralEntityExternal integralEntityAndNum = LitJson.JsonMapper.ToObject<IntegralEntityExternal>(args.Value.data.ToJson());

            TransferData data = new TransferData();

            List<TransferData> lstIntegralEntityData = new List<TransferData>();

            List<IntegralEntity> lstIntegralEntity = integralEntityAndNum.prize;

            for (int i = 0; i < lstIntegralEntity.Count; i++)
            {
                TransferData integralEntity = new TransferData();
                integralEntity.SetValue("integralEntity", lstIntegralEntity[i]);
                lstIntegralEntityData.Add(integralEntity);
            }
            data.SetValue("lstData", lstIntegralEntityData);
            data.SetValue("Count", integralEntityAndNum.score);

            if (m_UIIntegralWindow)
            {
                m_UIIntegralWindow.SetUI(data);
            }
        }
    }
    #endregion

    //=================================

    #region RequestExchangeRecordInfo 请求花费积分换取礼物相关
    /// <summary>
    /// 请求花费积分换取礼物相关
    /// </summary>
    private void RequestSpendIntegralExchangeGift(object obj)
    {
        int index = (int)obj;

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["id"] = index;
        
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/scoreExchange/", OnRequestSpendIntegralExchangeGiftlCallBack, true, "scoreExchange", dic);
    }
    #endregion

    #region OnRequestSpendIntegralExchangeGiftlCallBack 请求花费积分换取礼物相关相关回调
    /// <summary>
    /// 请求花费积分换取礼物相关相关回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestSpendIntegralExchangeGiftlCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            ShowMessage("提示", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("提示", args.Value.msg);
                return;
            }
            ShowMessage("", "兑换成功\n\n请在兑换记录中领取您的豪礼");
            RequestIntegralInfo();
        }
    }
    #endregion

    //=================================

    #region RequestExchangeRecordInfo 请求兑换记录相关
    /// <summary>
    /// 请求兑换记录相关
    /// </summary>
    private void RequestExchangeRecordInfo()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["prize_type"] = (int)PrizeType.Integral;
        prizeType = PrizeType.Integral;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/prizeRecord/", OnExchangeRecordCallBack, true, "prizeRecord", dic);
    }
    #endregion

    #region OnRequestIntegralCallBack 请求兑换记录相关回调
    /// <summary>
    /// 请求兑换记录相关回调
    /// </summary>
    /// <param name="args"></param>
    private void OnExchangeRecordCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            ShowMessage("提示", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("提示", args.Value.msg);
                return;
            }
            if (args.Value.data == null || args.Value.data.Count == 0) return;

            List<ExchangeRecordEntity> integralRecordEntity = LitJson.JsonMapper.ToObject<List<ExchangeRecordEntity>>(args.Value.data.ToJson());

            List<TransferData> lstIntegralEntityData = new List<TransferData>();

            TransferData data = new TransferData();

            for (int i = 0; i < integralRecordEntity.Count; i++)
            {
                TransferData recordData = new TransferData();
                recordData.SetValue("ExchangeRecordEntity", integralRecordEntity[i]);
                lstIntegralEntityData.Add(recordData);
                recordData.SetValue("PrizeType", prizeType);
            }

            data.SetValue("lstRecord", lstIntegralEntityData);

            if (m_UIExchangeRecordWindow)
            {
                m_UIExchangeRecordWindow.SetUI(data);
            }
        }
    }
    #endregion

    //=================================
    
    #region  OnRequestFictitiousGiftRecord 礼物申请领取
    /// <summary>
    /// 礼物申请领取
    /// </summary>
    private void OnRequestGiftReceive(int id)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["id"] = id;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/getPrize/", OnRequestGiftReceiveCallBack, true, "getPrize", dic);
    }
    #endregion

    #region OnRequestGiftReceiveCallBack 礼物申请领取回调
    /// <summary>
    /// 礼物申请领取回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestGiftReceiveCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            ShowMessage("提示", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("提示", args.Value.msg);
                return;
            }
            if (args.Value.data == null || args.Value.data.Count == 0) return;

            ExchangeRecordCallBackEntity exchangeRecordCallBackEntity = LitJson.JsonMapper.ToObject<ExchangeRecordCallBackEntity>(args.Value.data.ToJson());

            int status = exchangeRecordCallBackEntity.status;

            string address = exchangeRecordCallBackEntity.contact_address;

            string name = exchangeRecordCallBackEntity.contact_name;

            string phone = exchangeRecordCallBackEntity.contact_phone;

            int index = exchangeRecordCallBackEntity.id;

            Action requst = null;
            requst = RequestExchangeRecordInfo;
            if (status == 1)
            {
                ShowMessage("", "领取成功", MessageViewType.Ok, requst);
                AccountCtrl.Instance.RequestCards();
            }
            else if (status == 2)
            {
                if (!m_UIPlayerReceiveRecordInfoView)
                {
                    m_UIPlayerReceiveRecordInfoView = m_UIExchangeRecordWindow.OpenPrompt(name, phone, address, index, requst,PrizeType.Integral);
                }
            }
        }
    }
    #endregion

    //==================================


    #region RequestSendMessage 请求发送获得礼品玩家的相关信息
    /// <summary>
    /// 请求发送获得礼品玩家的相关信息
    /// </summary>
    private void RequestSendMessage(object name, object telephone, object address, object index,object prizeType)
    {
        PrizeType type = (PrizeType)prizeType;
        if (type != PrizeType.Integral) return;
        string sentName = (string)name;
        string sendTelephone = (string)telephone;
        string sendAddress = (string)address;
        int sendIndex = (int)index;
        if (sentName == "")
        {
            ShowTip("请输入姓名");
            return;
        }
        if (sendTelephone == "")
        {
            ShowTip("请输入电话");
            return;
        }
        else
        {
            if (!sendTelephone.Regex(ConstDefine.TelNumRegex))
            {
                ShowTip("请输入正确的手机号码");
                return;
            }
        }
        if (sendAddress == "")
        {
            ShowTip("请输入地址");
            return;
        }
        if (m_UIPlayerReceiveRecordInfoView)
        {
            m_UIPlayerReceiveRecordInfoView.Close();
        }

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["id"] = sendIndex;
        dic["contact_name"] = sentName;
        dic["contact_phone"] = sendTelephone;
        dic["contact_address"] = sendAddress;

        //SendMessageCallBack();
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/apply_get/", OnRequestSendMessageCallBack, true, "apply_get", dic);
    }

    #endregion

    #region OnRequestSendMessageCallBack  请求发送获得礼品玩家的相关信息回调
    /// <summary>
    /// 请求发送获得礼品玩家的相关信息回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestSendMessageCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            ShowMessage("提示", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("提示", args.Value.msg);
                return;
            }
            //if (args.Value.data == null || args.Value.data.Count == 0) return;

            ExchangeRecordCallBackEntity exchangeRecordCallBackEntity = LitJson.JsonMapper.ToObject<ExchangeRecordCallBackEntity>(args.Value.data.ToJson()); ;

            string wxCustonmerService = exchangeRecordCallBackEntity.wx_kf;

            TransferData data = new TransferData();

            data.SetValue("wxCustonmerService", wxCustonmerService);

            data.SetValue("id", exchangeRecordCallBackEntity.id);

            if (m_UIExchangeRecordWindow)
            {
                m_UIExchangeRecordWindow.SetExchangeRecord(data);
            }
            UIViewUtil.Instance.LoadWindow("ApplySuccessdMessage").GetComponent<UIApplySuccessdMessageView>().SetUI(data);

            RequestExchangeRecordInfo();
        }
    }
    #endregion

    #endregion
}
