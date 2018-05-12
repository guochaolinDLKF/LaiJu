//===================================================
//Author      : DRB
//CreateTime  ：11/30/2017 6:13:00 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelfareActivitiesCtrl : SystemCtrlBase<WelfareActivitiesCtrl>, ISystemCtrl
{
    #region Variable And OpenView
    private UIWelfareActivitiesWindow m_UIWelfareActivitiesWindow;

    private UIExchangeRecordWindow m_UIExchangeRecordWindow;

    private UIPlayerReceiveRecordInfoView m_UIPlayerReceiveRecordInfoView;

    private PrizeType prizeType;

    private int drawTime;

    private bool isUseCard;

    private int spendCardDrawCount;
    
    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.WelfareActivities:
                OnBtnOpenWelfareActivities();
                break;
            default:
                break;
        }
    }
    #endregion

    #region Dictionary
    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();

        //dic.Add("btn_welfareActivities", OnBtnOpenWelfareActivities);
        dic.Add("btn_treasure", OnBtnOpenTreasure);
        dic.Add("btn_lotteryWheel", OnBtnOpenLotteryWheel);
        dic.Add("btn_treasureRule", OnBtnOpenTreasureRule);
        dic.Add("btn_lotteryWheelRule", OnBtnOpenlotteryWheelRule);
        dic.Add("btn_treasureRecord", OnBtnOpenTreasureRecord);
        dic.Add("btn_lotteryWheelRecord", OnBtnOpenLotteryWheelRecord);
        dic.Add("btn_box", OnBtnPressBox);
        dic.Add("btn_beginDraw", OnBtnDrawGift);
        dic.Add("OnLotteryComplete", OnLotteryComplete);
        dic.Add("btn_receive",OnBtnReceive);
        dic.Add("btn_SendPlayerMessage", OnBtnSendPlayerMessage);
        return dic;
    }
    #endregion

    #region 按钮与事件

    #region OnBtnOpenWelfareActivities 打开福利活动界面
    /// <summary>
    /// 打开福利活动界面
    /// </summary>
    private void OnBtnOpenWelfareActivities()
    {
        if (!m_UIWelfareActivitiesWindow)
        {
            m_UIWelfareActivitiesWindow = UIViewUtil.Instance.LoadWindow("WelfareActivities").GetComponent<UIWelfareActivitiesWindow>();
            //RequestWelfareActivitiesInfo();
            //测试
            //WelfareActivitiesWindow();
            //OnBtnOpenTreasure(null);
            OnBtnOpenLotteryWheel(null);
        }
    }
    #endregion

    #region OnBtnOpenTreasureRule 打开福利宝箱规则页面
    /// <summary>
    /// 打开福利宝箱规则页面
    /// </summary>
    private void OnBtnOpenTreasureRule(object[] obj)
    {
        UIViewUtil.Instance.LoadWindow("TreasureRule").GetComponent<UIWelfareActivitiesWindow>();
    }
    #endregion

    #region OnBtnOpenTreasure 打开宝箱活动界面
    /// <summary> 
    /// 打开宝箱活动界面
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnOpenTreasure(object[] obj)
    {
        if (m_UIWelfareActivitiesWindow)
        {
            m_UIWelfareActivitiesWindow.ChangeInterface(InterfaceType.Treasure);
            RequestTreasureInfo();
            //TreasureWindow();
        }
    }
    #endregion

    #region OnBtnPressBox 开启宝箱
    /// <summary>
    /// 开启宝箱
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPressBox(object[] obj)
    {
        int boxIndex = int.Parse(obj[0].ToString());
        RequestPressBox(boxIndex);
        //测试
        //PressBox();
    }
    #endregion

    #region OnBtnOpenTreasureRecord 打开宝箱兑奖界面
    /// <summary>
    /// 打开宝箱兑奖界面
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnOpenTreasureRecord(object[] obj)
    {
        m_UIExchangeRecordWindow = UIViewUtil.Instance.LoadWindow("TreasureRecord").GetComponent<UIExchangeRecordWindow>();
        CallRequestExchangeTreasureRecordInfo();
    }
    #endregion

    #region OnBtnOpenLotteryWheel 打开大转盘活动界面
    /// <summary>
    /// 打开大转盘活动界面
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnOpenLotteryWheel(object[] obj)
    {
        if (m_UIWelfareActivitiesWindow)
        {
            m_UIWelfareActivitiesWindow.ChangeInterface(InterfaceType.LotteryWheel);
            RequestLotteryWheelInfo();
            //LotteryWheelWindow();
        }
    }
    #endregion

    #region OnBtnOpenlotteryWheelRule 打开大转盘规则页面
    /// <summary>
    /// 打开大转盘规则页面
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnOpenlotteryWheelRule(object[] obj)
    {
        UIViewUtil.Instance.LoadWindow("LotteryWheelRule").GetComponent<UIWelfareActivitiesWindow>();
    }
    #endregion

    #region OnBtnDrawGift 开始抽奖
    /// <summary>
    /// 开始抽奖
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnDrawGift(object[] obj)
    {
            if (/*drawTime < 3 && drawTime > 0*/isUseCard)
            { 
                ShowMessage("提示", "是否使用" + spendCardDrawCount + "张房卡，抽奖一次", MessageViewType.OkAndCancel, RequestDrawGift);
            return;
            }
        RequestDrawGift();
        //RequestDrawGift();
        //测试
        //DrawGift();
    }
    #endregion

    #region OnBtnOpenLotteryWheelRecord 打开大转盘兑奖界面
    /// <summary>
    /// 打开大转盘兑奖界面
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnOpenLotteryWheelRecord(object[] obj)
    {
        m_UIExchangeRecordWindow = UIViewUtil.Instance.LoadWindow("LotteryWheelRecord").GetComponent<UIExchangeRecordWindow>();
        CallRequestExchangeLotteryWheelRecordInfo();
    }
    #endregion
    
    #region OnBtnReceive 点击兑换按钮
    /// <summary>
    /// 点击兑换按钮
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnReceive(object[] obj)
    {
        int index = (int)obj[0];
        if (prizeType == (PrizeType)obj[1])
        {
            OnRequestGiftReceive(index);
        }
    }
    #endregion

    #region OnBtnSendPlayerMessage 发送玩家兑奖地址信息
    /// <summary>
    /// 发送玩家兑奖地址信息
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnSendPlayerMessage(object[] obj)
    {
        RequestSendInfo(obj[0], obj[1], obj[2], obj[3],obj[4]);
    }
    #endregion

    #region OnLotteryComplete大转盘结束
    /// <summary>
    /// 大转盘结束
    /// </summary>
    /// <param name="obj"></param>
    private void OnLotteryComplete(object[] obj)
    {
        if (m_UIWelfareActivitiesWindow)
        {
            RequestLotteryWheelInfo();
            //Action action = new Action(RequestLotteryWheelInfo);
            //if (m_UIWelfareActivitiesWindow.GetTargetGiftType() == GiftType.Again)
            //{
            //    ShowMessage("", "恭喜你获得再来一次", MessageViewType.Ok, action);
            //}
            //else if (m_UIWelfareActivitiesWindow.GetTargetGiftType() == GiftType.Null)
            //{
            //    m_UIWelfareActivitiesWindow.SetLotteryWheelSruPlus(int.Parse(obj[0].ToString()));
            //    ShowMessage("", m_UIWelfareActivitiesWindow.GetTargetGifName());
            //}
            //else
            //{
            //    m_UIWelfareActivitiesWindow.SetLotteryWheelSruPlus(int.Parse(obj[0].ToString()));
            //    ShowMessage("", "恭喜你,获得" + m_UIWelfareActivitiesWindow.GetTargetGifName() + "，请在积分兑换处换取礼品");
            //}
        }
    }
    #endregion

    #endregion
    
    #region 测试

    #region WelfareActivitiesWindow 福利活动界面钥匙与次数的设置
    /// <summary>
    /// 福利活动界面钥匙与次数的设置
    /// </summary>
    private void WelfareActivitiesWindow()
    {
        List<lotteryWheelEntity> lstLotteryWheelEntity = new List<lotteryWheelEntity>();
        for (int i = 0; i < 12; i++)
        {
            lotteryWheelEntity lotteryWheelEntity = new lotteryWheelEntity();
            lotteryWheelEntity.id = i;
            lotteryWheelEntity.name = "GGGGQQQQQ";
            lotteryWheelEntity.img_url = "https://ss1.bdstatic.com/70cFuXSh_Q1YnxGkpoWK1HF6hhy/it/u=3725680684,3018861305&fm=27&gp=0.jpg";
            lotteryWheelEntity.type = GiftType.Null;
            lstLotteryWheelEntity.Add(lotteryWheelEntity);
        }
        WelfareActivitiesEntity welfareActivitiesEntity = new WelfareActivitiesEntity(3, 3, 10, lstLotteryWheelEntity);

        TransferData data = new TransferData();
        
        data.SetValue("treasureKeyNum", welfareActivitiesEntity.treasureKeyNum);
        data.SetValue("surPlusTimeCount", welfareActivitiesEntity.surPlusTimeCount);
        data.SetValue("lstBoxIndex", welfareActivitiesEntity.lstBoxIndex);
        data.SetValue("totalTimeCount", welfareActivitiesEntity.totalTimeCount);
        data.SetValue("lstLotteryWheelEntity", welfareActivitiesEntity.lstLotteryWheelEntity);

        if (m_UIWelfareActivitiesWindow)
        {
            m_UIWelfareActivitiesWindow.SetUI(data);
        }
    }
    #endregion

    #region TreasureWindow 打开宝箱窗口测试
    /// <summary>
    /// 打开宝箱窗口测试
    /// </summary>
    private void TreasureWindow()
    {
        List<int> lstBoxIndex = new List<int>();
        lstBoxIndex.Add(1);
        lstBoxIndex.Add(2);
        lstBoxIndex.Add(3);

        TreasureWindowEntity treasureWindowEntity = new TreasureWindowEntity(3, lstBoxIndex);

        TransferData data = new TransferData();

        data.SetValue("treasureKeyNum", treasureWindowEntity.treasureKeyNum);
        data.SetValue("lstBoxIndex", treasureWindowEntity.lstBoxIndex);

        if (m_UIWelfareActivitiesWindow)
        {
            m_UIWelfareActivitiesWindow.SetTreasure(data);
        }
    }
    #endregion

    #region LotteryWheelWindow 打开大转盘窗口测试
    /// <summary>
    /// 打开大转盘窗口测试
    /// </summary>
    private void LotteryWheelWindow()
    {
        LotteryWheelWindowEntity lotteryWheelWindowEntity = new LotteryWheelWindowEntity();
        lotteryWheelWindowEntity.useable = 5;
        lotteryWheelWindowEntity.total = 10;
        List<lotteryWheelEntity> lstLotteryWheelEntity = new List<lotteryWheelEntity>();

        for (int i = 0; i < 12; i++)
        {
            lstLotteryWheelEntity[i].id = i;
            lstLotteryWheelEntity[i].img_url = "https://ss1.bdstatic.com/70cFuXSh_Q1YnxGkpoWK1HF6hhy/it/u=3725680684,3018861305&fm=27&gp=0.jpg";
            lstLotteryWheelEntity[i].name = i + "_name";
            lstLotteryWheelEntity[i].type = GiftType.Again;
        }
        lotteryWheelWindowEntity.prize = lstLotteryWheelEntity;

        TransferData data = new TransferData();

        data.SetValue("surPlusTimeCount", lotteryWheelWindowEntity.useable);
        data.SetValue("totalTimeCount", lotteryWheelWindowEntity.total);
        data.SetValue("lstLotteryWheelEntity", lotteryWheelWindowEntity.prize);

        if (m_UIWelfareActivitiesWindow)
        {
            m_UIWelfareActivitiesWindow.SetLotteryWheel(data);
        }
    }
    #endregion

    #region DrawGift 开始抽奖测试
    /// <summary>
    /// 开始抽奖测试
    /// </summary>
    private void DrawGift()
    {
        LotteryWheelGiftCallBackEntity lotteryWheelGiftEntity = new LotteryWheelGiftCallBackEntity();
        lotteryWheelGiftEntity.id = 1;
        lotteryWheelGiftEntity.useable = 2;
        lotteryWheelGiftEntity.total = 5;

        TransferData data = new TransferData();

        data.SetValue("giftIndex", lotteryWheelGiftEntity.id);
        data.SetValue("time", lotteryWheelGiftEntity.useable);
        data.SetValue("timeTotal", lotteryWheelGiftEntity.total);
        //data.SetValue("giftCallBackNum", lotteryWheelGiftEntity.giftCount);
        //data.SetValue("message", lotteryWheelGiftEntity.message);
        //data.SetValue("giftType",lotteryWheelGiftEntity.giftType);

        if (m_UIWelfareActivitiesWindow)
        {
            m_UIWelfareActivitiesWindow.StartTurn(data);
        }
    }
    #endregion

    #endregion
    
    #region 刷新界面

    #region CallRequestExchangeLotteryWheelRecordInfo 刷新大转盘界面
    /// <summary>
    /// 刷新大转盘界面
    /// </summary>
    private void CallRequestExchangeLotteryWheelRecordInfo()
    {
        RequestExchangeRecordInfo(PrizeType.LotteryWheel);
        prizeType = PrizeType.LotteryWheel;
    }
    #endregion

    #region CallRequestExchangeTreasureRecordInfo 刷新宝箱界面
    /// <summary>
    /// 刷新宝箱界面
    /// </summary>
    private void CallRequestExchangeTreasureRecordInfo()
    {
        RequestExchangeRecordInfo(PrizeType.Treasure);
        prizeType = PrizeType.Treasure;
    }
    #endregion

    #endregion

    #region  发送与回复消息

    #region  请求与回调福利活动界面相关（老）
    /// <summary>
    /// 请求福利活动界面相关
    /// </summary>
    private void RequestWelfareActivitiesInfo()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/service/", OnRequestWelfareActivitiesCallBack, true, "service", dic);
    }

    /// <summary>
    /// 请求福利活动界面回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestWelfareActivitiesCallBack(NetWorkHttp.CallBackArgs args)
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

            WelfareActivitiesEntity welfareActivitiesEntity = LitJson.JsonMapper.ToObject<WelfareActivitiesEntity>(args.Value.data.ToJson());

            TransferData data = new TransferData();

            data.SetValue("treasureKeyNum", welfareActivitiesEntity.treasureKeyNum);
            data.SetValue("surPlusTimeCount", welfareActivitiesEntity.surPlusTimeCount);
            data.SetValue("lstBoxIndex", welfareActivitiesEntity.lstBoxIndex);
            data.SetValue("totalTimeCount", welfareActivitiesEntity.totalTimeCount);
            data.SetValue("lstLotteryWheelEntity", welfareActivitiesEntity.lstLotteryWheelEntity);

            if (m_UIWelfareActivitiesWindow)
            {
                m_UIWelfareActivitiesWindow.SetUI(data);
            }
        }
    }
    #endregion

    #region  请求与回调福利宝箱界面相关
    /// <summary>
    /// 请求福利宝箱界面相关
    /// </summary>
    private void RequestTreasureInfo()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/boxKeyNum/", OnRequestTreasureCallBack, true, "boxKeyNum", dic);
    }

    /// <summary>
    /// 请求福利宝箱界面回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestTreasureCallBack(NetWorkHttp.CallBackArgs args)
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

            //TreasureWindowEntity treasureWindowEntity = LitJson.JsonMapper.ToObject<TreasureWindowEntity>(args.Value.data.ToJson());
            TreasureKeyNum TreasureKyeNum = LitJson.JsonMapper.ToObject<TreasureKeyNum>(args.Value.data.ToJson());

            TransferData data = new TransferData();

            //data.SetValue("treasureKeyNum", treasureWindowEntity.treasureKeyNum);
            //data.SetValue("lstBoxIndex", treasureWindowEntity.lstBoxIndex);

            data.SetValue("treasureKeyNum", TreasureKyeNum.key_num);

            if (m_UIWelfareActivitiesWindow)
            {
                //m_UIWelfareActivitiesWindow.SetUI(data);
                m_UIWelfareActivitiesWindow.SetTreasure(data);
            }
        }
    }
    #endregion
    
    #region  请求与回调大转盘界面相关
    /// <summary>
    /// 请求大转盘界面相关
    /// </summary>
    private void RequestLotteryWheelInfo()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/turntable/", OnRequestLotteryWheelCallBack, true, "turntable", dic);
    }

    /// <summary>
    /// 请求大转盘界面回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestLotteryWheelCallBack(NetWorkHttp.CallBackArgs args)
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

            LotteryWheelWindowEntity lotteryWheelWindowEntitiy = LitJson.JsonMapper.ToObject<LotteryWheelWindowEntity>(args.Value.data.ToJson());
            isUseCard = lotteryWheelWindowEntitiy.is_deduct;
            spendCardDrawCount = lotteryWheelWindowEntitiy.deduct_num;

            TransferData data = new TransferData();

            drawTime = lotteryWheelWindowEntitiy.useable;

            data.SetValue("surPlusTimeCount", lotteryWheelWindowEntitiy.useable);
            data.SetValue("totalTimeCount", lotteryWheelWindowEntitiy.total);
            data.SetValue("lstLotteryWheelEntity", lotteryWheelWindowEntitiy.prize);
            data.SetValue("BgURL", lotteryWheelWindowEntitiy.ac_bg);
            data.SetValue("buttonURL", lotteryWheelWindowEntitiy.ac_cj);

            if (m_UIWelfareActivitiesWindow)
            {
                m_UIWelfareActivitiesWindow.SetLotteryWheel(data);
            }
        }
    }
    #endregion

    #region 请求与回调按下福利箱子按钮 
    /// <summary>
    /// 请求按下福利箱子按钮
    /// </summary>
    private void RequestPressBox(int boxIndex)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/open_box/", OnRequestPressBoxCallBack, true, "open_box", dic);
    }
    /// <summary>
    /// 请求按下福利箱子按钮回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestPressBoxCallBack(NetWorkHttp.CallBackArgs args)
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

            TreasureEntity treasureEntity = LitJson.JsonMapper.ToObject<TreasureEntity>(args.Value.data.ToJson());

            TransferData data = new TransferData();

            //data.SetValue("treasureKeyNum", treasureEntity.keyNum);

            if (m_UIWelfareActivitiesWindow)
            {
                //m_UIWelfareActivitiesWindow.SetTreasure(data);
                if (treasureEntity.type == 4)
                {
                    ShowMessage("", "空空如也");
                }
                else
                {
                    ShowMessage("", "恭喜您，您已获得" + treasureEntity.name);
                }
            }
            RequestTreasureInfo();
        }
    }

    #endregion

    #region 请求与回调大转盘开始按钮
    /// <summary>
    /// 请求按下大转盘按钮
    /// </summary>
    private void RequestDrawGift()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/turn/", OnRequestDrawGiftCallBack, true, "turn", dic);
    }
    /// <summary>
    /// 请求按下大转盘按钮回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestDrawGiftCallBack(NetWorkHttp.CallBackArgs args)
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

            LotteryWheelGiftCallBackEntity lotteryWheelGiftEntity = LitJson.JsonMapper.ToObject<LotteryWheelGiftCallBackEntity>(args.Value.data.ToJson());

            TransferData data = new TransferData();

            data.SetValue("giftIndex", lotteryWheelGiftEntity.id);
            data.SetValue("time", lotteryWheelGiftEntity.useable);
            data.SetValue("timeTotal", lotteryWheelGiftEntity.total);
            //data.SetValue("giftType", lotteryWheelGiftEntity.giftType);
            //data.SetValue("giftCallBackNum", lotteryWheelGiftEntity.giftCount);
            //data.SetValue("message", lotteryWheelGiftEntity.message);

            if (m_UIWelfareActivitiesWindow)
            {
                m_UIWelfareActivitiesWindow.StartTurn(data);
            }
        }
    }

    #endregion
    
    #region RequestExchangeRecordInfo 请求兑换记录相关
    /// <summary>
    /// 请求兑换记录相关
    /// </summary>
    private void RequestExchangeRecordInfo(PrizeType type)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["prize_type"] = (int)type;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/prizeRecord/", OnExchangeRecordCallBack, true, "prizeRecord", dic);
    }
    #endregion
    
    #region OnExchangeRecordCallBack 请求兑换记录相关回调
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

            List<ExchangeRecordEntity> exchangeRecordEntity = LitJson.JsonMapper.ToObject<List<ExchangeRecordEntity>>(args.Value.data.ToJson());

            List<TransferData> lstIntegralEntityData = new List<TransferData>();

            TransferData data = new TransferData();

            for (int i = 0; i < exchangeRecordEntity.Count; i++)
            {
                TransferData recordData = new TransferData();
                recordData.SetValue("ExchangeRecordEntity", exchangeRecordEntity[i]);
                recordData.SetValue("PrizeType", prizeType);
                lstIntegralEntityData.Add(recordData);
            }

            data.SetValue("lstRecord", lstIntegralEntityData);

            if (m_UIExchangeRecordWindow)
            {
                m_UIExchangeRecordWindow.SetUI(data);
            }
        }
    }
    #endregion

    #region  OnRequestFictitiousGiftRecord 虚拟礼物申请领取
    /// <summary>
    /// 虚拟礼物申请领取
    /// </summary>
    private void OnRequestGiftReceive(int id)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["id"] = id;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/getPrize/", OnRequestGiftReceiveCallBack, true, "getPrize", dic);
    }

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

            if (prizeType == PrizeType.LotteryWheel)
            {
                requst = CallRequestExchangeLotteryWheelRecordInfo;
            }
            else
            {
                requst = CallRequestExchangeTreasureRecordInfo; ;
            }

            if (status == 1)
            {               
                ShowMessage("", "领取成功",MessageViewType.Ok, requst);
                AccountCtrl.Instance.RequestCards();
            }
            else if (status == 2)
            {
                if (!m_UIPlayerReceiveRecordInfoView)
                {
                    m_UIPlayerReceiveRecordInfoView = m_UIExchangeRecordWindow.OpenPrompt(name,phone,address, index, requst,PrizeType.Treasure);
                }
            }
        }
    }
    #endregion

    #region RequestSendMessage 请求发送获得礼品玩家的相关信息
    /// <summary>
    /// 请求发送获得礼品玩家的相关信息
    /// </summary>
    private void RequestSendInfo(object name, object telephone, object address,object index,object prizeType)
    {
        PrizeType type = (PrizeType)prizeType;
        if (type != PrizeType.Treasure) return;
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
        
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/apply_get/", OnRequestSendInfoCallBack, true, "apply_get", dic);
    }

    #endregion

    #region OnRequestSendInfoCallBack 请求发送获得礼品玩家的相关信息回调
    /// <summary>
    /// 请求发送获得礼品玩家的相关信息回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestSendInfoCallBack(NetWorkHttp.CallBackArgs args)
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

            //RequestExchangeLotteryWheelRecordInfo(PrizeType.LotteryWheel);
            if (prizeType == PrizeType.LotteryWheel)
            {
                CallRequestExchangeLotteryWheelRecordInfo();
            }
            else
            {
                CallRequestExchangeTreasureRecordInfo(); ;
            }
        }
    }

    #endregion

    #endregion
}
