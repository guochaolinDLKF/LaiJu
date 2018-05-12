//===================================================
//Author      : DRB
//CreateTime  ：7/10/2017 2:49:55 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;


public enum RankingListType
{
    Count = 1,
    Score = 2,
}

public class RankingCtrl : SystemCtrlBase<RankingCtrl>, ISystemCtrl
{
    private UIRankingView m_UIRankingView;

    private RankingListType m_CurrentType = RankingListType.Score;

    private bool m_isBusy;

    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        dic.Add("BtnScoreList", OnBtnScoreListClick);
        dic.Add("BtnCountList", OnBtnCountListClick);

        return dic;
    }

    /// <summary>
    /// 场次排行榜按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnCountListClick(object[] obj)
    {
        RequestRankingList(RankingListType.Score);
        //RequestOwnerRanking(RankingListType.Score);
    }

    /// <summary>
    /// 积分排行榜按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnScoreListClick(object[] obj)
    {
        RequestRankingList(RankingListType.Count);
        //RequestOwnerRanking(RankingListType.Count);
    }

    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.Ranking:
                OpenRankingView();
                break;
        }
    }

    private void OpenRankingView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Ranking, (GameObject go) =>
         {
             m_UIRankingView = go.GetComponent<UIRankingView>();

#if IS_BAODING
             RequestRankingList(RankingListType.Score);
             RequestOwnerRanking(RankingListType.Score);
#else
             RequestRankingList(RankingListType.Count);
#endif
         });
    }


    #region RequestRankingList 获取排行列表
    /// <summary>
    /// 获取排行列表
    /// </summary>
    /// <param name="type"></param>
    private void RequestRankingList(RankingListType type)
    {
        if (m_isBusy)
        {
            return;
        }
        m_isBusy = true;
        m_CurrentType = type;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["type"] = (int)type;
        dic["top"] = 10;
        dic["gameId"] = cfg_gameDBModel.Instance.GetList()[0].id;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrRanking, OnRequestRankingListCallBack, true, ConstDefine.HTTPFuncRanking, dic);
    }
    #endregion

    #region OnRequestRankingListCallBack 获取排行列表回调
    /// <summary>
    /// 获取排行列表回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestRankingListCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        if (args.HasError)
        {
            ShowMessage("错误", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("提示", args.Value.msg);
                return;
            }

            List<RankingEntity> lst = LitJson.JsonMapper.ToObject<List<RankingEntity>>(args.Value.data.ToJson());

            TransferData data = new TransferData();
            List<TransferData> lstRanking = new List<TransferData>();
            for (int i = 0; i < lst.Count; ++i)
            {
                TransferData rankingData = new TransferData();
                rankingData.SetValue("Top", lst[i].top);
                rankingData.SetValue("PlayerId", lst[i].playerId);
                rankingData.SetValue("NickName", lst[i].nickname);
                rankingData.SetValue("Avatar", lst[i].avatar);
                rankingData.SetValue("Score", lst[i].score);
                lstRanking.Add(rankingData);
            }
            data.SetValue("Ranking", lstRanking);
            data.SetValue("RankingType", m_CurrentType);
            if (m_UIRankingView != null)
            {
                m_UIRankingView.SetUI(data);
            }
        }
    }
    #endregion

    #region RequestOwnerRanking 获取个人排行
    /// <summary>
    /// 获取个人排行
    /// </summary>
    private void RequestOwnerRanking(RankingListType type)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["type"] = (int)type;
        dic["gameId"] = cfg_gameDBModel.Instance.GetList()[0].id;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/ranktop/", RequestOwnerRankingCallBack, true, "ranktop", dic);
    }
    #endregion

    #region RequestOwnerRankingCallBack 获取个人排行回调
    /// <summary>
    /// 获取个人排行回调
    /// </summary>
    /// <param name="args"></param>
    private void RequestOwnerRankingCallBack(NetWorkHttp.CallBackArgs args)
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

            RankingEntity entity = LitJson.JsonMapper.ToObject<RankingEntity>(args.Value.data.ToJson());
            if (entity == null) return;

            TransferData data = new TransferData();
            data.SetValue("Score", entity.value);
            m_UIRankingView.SetOwnerRanking(data);
        }
    }
    #endregion

}
