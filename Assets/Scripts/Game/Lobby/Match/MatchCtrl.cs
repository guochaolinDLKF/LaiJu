//===================================================
//Author      : DRB
//CreateTime  ：4/7/2017 3:40:21 PM
//Description ：比赛模块控制器
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using com.oegame.mahjong.protobuf;
using proto.mahjong;
using UnityEngine;

public class MatchCtrl : SystemCtrlBase<MatchCtrl>, ISystemCtrl
{
    #region Variable
    private UIMatchView m_UIBattleView;//比赛界面
    
    private UIMatchDetailView m_UIMatchDetailView;//比赛详情窗口
    
    private UIMatchTipView m_UIMatchTipView;//比赛报名等待窗口

    private UIMatchWaitView m_UIMatchWaitView;//比赛结束等待窗口

    private UIMatchRankListView m_UIMatchRankListView;//比赛排行榜窗口

    private MatchHTTPEntity m_CurrentSelectMatchEntity;//当前选择的比赛实体

    private bool m_isWait = false;//是否等待中

    private bool m_isBusy = false;//是否操作繁忙

    private bool m_isRebuild = false;//是否断线重连
    #endregion

    #region Constructors
    public MatchCtrl():base()
    {
        NetDispatcher.Instance.AddEventListener(OP_MATCH_APPLY.CODE, OnServerReturnEnter);
        NetDispatcher.Instance.AddEventListener(OP_MATCH_UNAPPLY.CODE, OnServerReturnBowout);
        NetDispatcher.Instance.AddEventListener(OP_MATCH_STATUS.CODE, OnServerBroadcastEnterStatus);
        NetDispatcher.Instance.AddEventListener(OP_MATCH_BEGIN.CODE, OnServerBroadcastBegin);
        NetDispatcher.Instance.AddEventListener(OP_MATCH_SETTLE.CODE, OnServerBroadcastSettle);
        NetDispatcher.Instance.AddEventListener(OP_MATCH_RESULT.CODE, OnServerBroadcastResult);
        NetDispatcher.Instance.AddEventListener(OP_MATCH_WAIT.CODE, OnServerBroadcastWait);
        NetDispatcher.Instance.AddEventListener(OP_MATCH_EXISTS.CODE, OnServerReturnExists);
    }
    #endregion

    #region DicNotificationInterests 注册视图层事件
    public override Dictionary<string, UIDispatcher.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        dic.Add(ConstDefine.BtnMatchTipViewBowout, OnBtnMatchTipViewBowoutClick);
        dic.Add("btnMatchViewMatchRecord", OnBtnMatchTipViewMatchRecordClick);
        dic.Add("btnMatchRankListViewBack", OnBtnMatchRankListViewBackClick);
        dic.Add("btnMatchRankListViewShare", OnBtnMatchRankListViewShareClick);
        return dic;
    }
    #endregion

    #region Dispose
    public override void Dispose()
    {
        base.Dispose();
        NetDispatcher.Instance.RemoveEventListener(OP_MATCH_APPLY.CODE, OnServerReturnEnter);
        NetDispatcher.Instance.RemoveEventListener(OP_MATCH_UNAPPLY.CODE, OnServerReturnBowout);
        NetDispatcher.Instance.RemoveEventListener(OP_MATCH_STATUS.CODE, OnServerBroadcastEnterStatus);
        NetDispatcher.Instance.RemoveEventListener(OP_MATCH_BEGIN.CODE, OnServerBroadcastBegin);
        NetDispatcher.Instance.RemoveEventListener(OP_MATCH_SETTLE.CODE, OnServerBroadcastSettle);
        NetDispatcher.Instance.RemoveEventListener(OP_MATCH_RESULT.CODE, OnServerBroadcastResult);
        NetDispatcher.Instance.RemoveEventListener(OP_MATCH_WAIT.CODE, OnServerBroadcastWait);
        NetDispatcher.Instance.RemoveEventListener(OP_MATCH_EXISTS.CODE, OnServerReturnExists);
    }
    #endregion

    #region ISystemCtrl
    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.Match:
                UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Match,(GameObject go)=> 
                {
                    m_UIBattleView = go.GetComponent<UIMatchView>();
                    m_UIBattleView.OnEnterClick = OnEnterClick;
                    m_UIBattleView.OnSeeDetailClick = OnSeeDetailClick;
                    m_UIBattleView.SetCards(AccountProxy.Instance.CurrentAccountEntity.cards);
                    AudioBackGroundManager.Instance.Play("bgm_match");
                    RequestBattleList(0,0);
                });
                break;
            case UIWindowType.MatchWait:
                OpenMatchWaitView();
                break;
            case UIWindowType.MatchRankList:
                OpenMatchRankListView();
                break;
        }
    }
    #endregion

    private int m_CurrentId;

    private int m_CurrentRoomId;

    #region RequestBattleList 请求比赛列表
    /// <summary>
    /// 请求比赛列表
    /// </summary>
    public void RequestBattleList(int settingId,int roomId = 0)
    {
        if (m_isBusy) return;
        m_isBusy = true;
        m_CurrentId = settingId;
        m_CurrentRoomId = roomId;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrMatch, OnRequestBattleListCallBack, true, ConstDefine.HTTPFuncMatch, dic);
    }
    #endregion

    #region OnRequestBattleListCallBack 请求比赛列表回调
    /// <summary>
    /// 请求比赛列表回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestBattleListCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        if (args.HasError)
        {
            ShowMessage("错误", args.ErrorMsg);
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("错误", args.Value.msg);
                return;
            }
            string json = args.Value.data.ToJson();
            List<MatchHTTPEntity> lst = null;
            if (!string.IsNullOrEmpty(json))
            {
                lst = LitJson.JsonMapper.ToObject<List<MatchHTTPEntity>>(json);
            }
            if (m_UIBattleView != null)
            {
                m_UIBattleView.SetUI(lst);
            }

            if (m_CurrentId != 0)
            {
                m_isRebuild = true;
                Debug.Log(m_CurrentId);
                for (int i = 0; i < lst.Count; ++i)
                {
                    if (m_CurrentId == lst[i].id)
                    {
                        m_CurrentSelectMatchEntity = lst[i];
                    }
                    Debug.Log(lst[i].id);
                }

                m_CurrentId = 0;
                if (m_CurrentRoomId == 0)
                {
                    if (NetWorkSocket.Instance.Connected(GameCtrl.Instance.SocketHandle))
                    {
                        NetWorkSocket.Instance.Close(GameCtrl.Instance.SocketHandle);
                    }
                    UIViewManager.Instance.ShowWait();
                    GameCtrl.Instance.SocketHandle = NetWorkSocket.Instance.BeginConnect(m_CurrentSelectMatchEntity.ipaddr, m_CurrentSelectMatchEntity.port, onConnectedCallBack);
                }
            }
        }
    }
    #endregion

    #region OpenMatchDetailView 打开比赛详情窗口
    /// <summary>
    /// 打开比赛详情窗口
    /// </summary>
    /// <param name="entity"></param>
    private void OpenMatchDetailView(MatchHTTPEntity entity)
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.MatchDetail, (GameObject go) =>
        {
            m_UIMatchDetailView = go.GetComponent<UIMatchDetailView>();
            m_UIMatchDetailView.SetUI(entity);
        });
    }
    #endregion

    #region OpenMatchRankListView 打开比赛排行榜窗口
    /// <summary>
    /// 打开比赛排行榜窗口
    /// </summary>
    private void OpenMatchRankListView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.MatchRankList, (GameObject go) =>
        {
            m_UIMatchRankListView = go.GetComponent<UIMatchRankListView>();
            m_UIMatchRankListView.SetUI(MatchProxy.Instance.CurrentMatch.Ranking, m_CurrentSelectMatchEntity);
        });
    }
    #endregion

    #region OpenMatchWaitView 打开比赛等待结束窗口
    /// <summary>
    /// 打开比赛等待结束窗口
    /// </summary>
    private void OpenMatchWaitView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.MatchWait, (GameObject go) =>
        {
            m_UIMatchWaitView = go.GetComponent<UIMatchWaitView>();
            m_UIMatchWaitView.SetUI(MatchProxy.Instance.CurrentMatch.WaitCount, MatchProxy.Instance.CurrentMatch.RiseCount);
        });
        m_isWait = true;
    }
    #endregion

    public void StartNext()
    {
        if (!MatchProxy.Instance.CurrentMatch.IsOut)
        {
            if (MatchProxy.Instance.CurrentMatch.IsOver)
            {
                //显示最终结算界面
                OpenMatchRankListView();
            }
            else
            {
                //提示等待
                OpenMatchWaitView();
            }
        }
        else
        {
            //提示已经被淘汰
            ShowMessage("提示", "您已被淘汰", MessageViewType.Ok, ExitGame);
        }
    }

    #region OpenMatchTipView 打开比赛等待开始窗口
    /// <summary>
    /// 打开比赛等待开始窗口
    /// </summary>
    /// <param name="currentPlayerCount"></param>
    /// <param name="maxPlayerCount"></param>
    private void OpenMatchTipView(int currentPlayerCount,int maxPlayerCount)
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.MatchTip,(GameObject go)=> 
        {
            m_UIMatchTipView = go.GetComponent<UIMatchTipView>();
            m_UIMatchTipView.SetUI(currentPlayerCount, maxPlayerCount);
        });
    }
    #endregion

    #region OnEnterClick 报名按钮点击
    /// <summary>
    /// 报名按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnEnterClick(MatchHTTPEntity entity)
    {
        Enter(entity);
    }
    #endregion

    #region OnSeeDetailClick 查看详情按钮点击
    /// <summary>
    /// 查看详情按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnSeeDetailClick(MatchHTTPEntity entity)
    {
        OpenMatchDetailView(entity);
    }
    #endregion

    #region OnBtnMatchTipViewMatchRecordClick 比赛记录按钮点击
    /// <summary>
    /// 比赛记录按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnMatchTipViewMatchRecordClick(object[] obj)
    {
    }
    #endregion

    #region OnBtnMatchRankListViewBackClick 比赛排行榜界面返回按钮点击
    /// <summary>
    /// 比赛排行榜界面返回按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnMatchRankListViewBackClick(object[] obj)
    {
        m_UIMatchRankListView.Close();
        if (NetWorkSocket.Instance.Connected(GameCtrl.Instance.SocketHandle))
        {
            NetWorkSocket.Instance.SafeClose(GameCtrl.Instance.SocketHandle);
        }
        SceneMgr.Instance.LoadScene(SceneType.Main);
    }
    #endregion

    #region OnBtnMatchTipViewBowoutClick 比赛提示窗口退出按钮点击
    /// <summary>
    /// 比赛提示窗口退出按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnMatchTipViewBowoutClick(object[] obj)
    {
        Bowout();
    }
    #endregion

    #region OnBtnMatchRankListViewShareClick 比赛排行榜界面分享按钮点击
    /// <summary>
    /// 比赛排行榜界面分享按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnMatchRankListViewShareClick(object[] obj)
    {
        if (m_UIMatchRankListView == null) return;
        m_UIMatchRankListView.StartCoroutine(ShareCtrl.Instance.ScreenCapture(OnScreenCaptureComplete));
    }
    #endregion

    #region OnScreenCaptureComplete 分享截屏回调
    /// <summary>
    /// 分享截屏回调
    /// </summary>
    /// <param name="tex"></param>
    private void OnScreenCaptureComplete(Texture2D tex)
    {
        string path = LocalFileManager.Instance.LocalFilePath + "share/";
        if (!IOUtil.DirectoryExists(path))
        {
            IOUtil.CreateDirectory(path);
        }
        IOUtil.Write(path + "record.jpg", tex.EncodeToJPG());
        Debug.Log("分享完了");
        ShareCtrl.Instance.ShareTexture(WXShareType.WXSceneSession, path + "record.jpg");
    }
    #endregion

    #region Enter 报名
    /// <summary>
    /// 报名
    /// </summary>
    public void Enter(MatchHTTPEntity entity)
    {
        m_CurrentSelectMatchEntity = entity;
        if (NetWorkSocket.Instance.Connected(GameCtrl.Instance.SocketHandle))
        {
            NetWorkSocket.Instance.Close(GameCtrl.Instance.SocketHandle);
        }
        UIViewManager.Instance.ShowWait();
        GameCtrl.Instance.SocketHandle = NetWorkSocket.Instance.BeginConnect(entity.ipaddr, entity.port, onConnectedCallBack);
    }
    
    /// <summary>
    /// 连接服务器回调
    /// </summary>
    /// <param name="isSuccess"></param>
    private void onConnectedCallBack(bool isSuccess)
    {
        UIViewManager.Instance.CloseWait();
        if (isSuccess)
        {
            if (m_isRebuild)
            {
                NetWorkSocket.Instance.Send(null, OP_MATCH_EXISTS_GET.CODE, GameCtrl.Instance.SocketHandle);
            }
            else
            {
                UIViewManager.Instance.ShowWait();
                OP_MATCH_APPLY_GET proto = new OP_MATCH_APPLY_GET();
                proto.settingId = m_CurrentSelectMatchEntity.id;
                NetWorkSocket.Instance.Send(proto.encode(), OP_MATCH_APPLY_GET.CODE, GameCtrl.Instance.SocketHandle);
            }
        }
        else
        {
            ShowMessage("错误", "网络连接异常");
        }
        m_isRebuild = false;
        m_CurrentId = 0;
    }
    #endregion

    #region Bowout 取消报名
    /// <summary>
    /// 取消报名
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="matchId"></param>
    public void Bowout()
    {
        NetWorkSocket.Instance.Send(null, OP_MATCH_UNAPPLY_GET.CODE, GameCtrl.Instance.SocketHandle);
        UIViewManager.Instance.ShowWait();
    }
    #endregion

    #region ExitGame 退出比赛
    /// <summary>
    /// 退出比赛
    /// </summary>
    private void ExitGame()
    {
        GameCtrl.Instance.ExitGame();
    }
    #endregion

    #region OnServerReturnEnter 服务器返回报名消息
    /// <summary>
    /// 服务器返回报名消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnEnter(byte[] data)
    {
        UIViewManager.Instance.CloseWait();
        OP_MATCH_APPLY proto = OP_MATCH_APPLY.decode(data);

        MatchProxy.Instance.Apply(proto.matchId);

        if (proto.count == proto.total) return;
        OpenMatchTipView(proto.count, proto.total);
    }
    #endregion

    #region OnServerReturnBowout 服务器返回取消报名
    /// <summary>
    /// 服务器返回取消报名
    /// </summary>
    /// <param name="data"></param>
    private void OnServerReturnBowout(byte[] data)
    {
        UIViewManager.Instance.CloseWait();

        MatchProxy.Instance.Bowout();

        if (NetWorkSocket.Instance.Connected(GameCtrl.Instance.SocketHandle))
        {
            NetWorkSocket.Instance.SafeClose(GameCtrl.Instance.SocketHandle);
        }

        if (m_UIMatchTipView != null)
        {
            m_UIMatchTipView.Close();
        }
    }
    #endregion

    #region OnServerBroadcastEnterStatus 服务器广播报名人数
    /// <summary>
    /// 服务器广播报名人数
    /// </summary>
    /// <param name="data"></param>
    private void OnServerBroadcastEnterStatus(byte[] data)
    {
        OP_MATCH_STATUS proto = OP_MATCH_STATUS.decode(data);


        MatchProxy.Instance.UpdateEnterInfo(proto.count, proto.total);
    }
    #endregion

    #region OnServerBroadcastBegin 服务器广播比赛开始
    /// <summary>
    /// 服务器广播比赛开始
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastBegin(byte[] obj)
    {
        AppDebug.Log("服务器广播比赛开始");
        OP_MATCH_BEGIN proto = OP_MATCH_BEGIN.decode(obj);
        MatchProxy.Instance.BeginMatch();
        GameCtrl.Instance.BeginMatch(m_CurrentSelectMatchEntity.gameId);
        MatchProxy.Instance.UpdateRiseCount(proto.promoted);
        m_isWait = false;
    }
    #endregion

    #region OnServerBroadcastSettle 服务器广播结算
    /// <summary>
    /// 服务器广播结算
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastSettle(byte[] obj)
    {
        Debug.Log("服务器广播比赛结算");

        OP_MATCH_SETTLE proto = OP_MATCH_SETTLE.decode(obj);
        MatchProxy.Instance.Settle(proto.isOut, proto.number);
        if (proto.isOut)
        {
            ShowMessage("提示",string.Format("您已被淘汰{0}{1}{2}",string.IsNullOrEmpty(proto.rewardDesc)?"":",", proto.rewardDesc,string.Format(",排名第{0}",proto.number)) ,MessageViewType.Ok, ExitGame);
        }
        m_isWait = false;
    }
    #endregion

    #region OnServerBroadcastResult 服务器广播比赛结束
    /// <summary>
    /// 服务器广播比赛结束
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastResult(byte[] obj)
    {
        Debug.Log("服务器广播比赛结束");

        OP_MATCH_RESULT proto = OP_MATCH_RESULT.decode(obj);
        MatchProxy.Instance.Result(proto.getRankingList());
        Debug.Log("结算数据个数" + proto.rankingCount());
        if (m_isWait)
        {
            m_isWait = false;
            OpenMatchRankListView();
        }
    }
    #endregion

    #region OnServerBroadcastWait 服务器广播等待信息
    /// <summary>
    /// 服务器广播等待信息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastWait(byte[] obj)
    {
        OP_MATCH_WAIT proto = OP_MATCH_WAIT.decode(obj);
        MatchProxy.Instance.UpdateWaitInfo(proto.waitCount);
        
        if (proto.waitCount == 0)
        {
            ShowMessage("提示", "正在收拾麻将桌，请稍候...", MessageViewType.None);
        }
    }
    #endregion

    #region OnServerReturnExists 服务器返回比赛是否存在消息
    /// <summary>
    /// 服务器返回比赛是否存在消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnExists(byte[] obj)
    {
        Debug.Log("服务器返回比赛是否存在");
        OP_MATCH_EXISTS proto = OP_MATCH_EXISTS.decode(obj);
        if (proto.isExists)
        {
            if (proto.status == ENUM_MATCH_STATUS.MATCH_STATUS_SETTLE)
            {
                ShowMessage("提示", "请等待当前比赛轮次结束", MessageViewType.None);
            }
            else if (proto.status == ENUM_MATCH_STATUS.MATCH_STATUS_APPLY)
            {
                OpenMatchTipView(proto.count, proto.total);
            }
        }
        else
        {
            NetWorkSocket.Instance.SafeClose(GameCtrl.Instance.SocketHandle);
        }
    }
    #endregion
}
