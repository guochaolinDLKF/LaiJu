//===================================================
//Author      : DRB
//CreateTime  ：3/7/2017 3:09:06 PM
//Description ：选择房间模块控制器
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnterRoomType
{
    /// <summary>
    /// 默认
    /// </summary>
    None,
    /// <summary>
    /// 创建
    /// </summary>
    Create,
    /// <summary>
    /// 加入
    /// </summary>
    Join,
    /// <summary>
    /// 重连
    /// </summary>
    Renter
}

public class GameCtrl : SystemCtrlBase<GameCtrl>, ISystemCtrl
{
    #region Private Variable
    /// <summary>
    /// 加入房间窗口
    /// </summary>
    private UIJoinRoomView m_UIJoinRoomView;
    /// <summary>
    /// 创建房间窗口
    /// </summary>
    private UICreateRoomView m_UICreateRoomView;
    /// <summary>
    /// 创建房间窗口（网雀）
    /// </summary>
    private UICreateRoomView2 m_UICreateRoomView2;
    /// <summary>
    /// 我的房间窗口
    /// </summary>
    private UIMyRoomView m_UIMyRoomView;

    private List<MyRoomEntity> m_RoomList;

    private bool m_isBusy;
    /// <summary>
    /// 当前选择的游戏Id
    /// </summary>
    private int m_nCurrentSelectGame;
    /// <summary>
    /// 当前群Id
    /// </summary>
    private int m_nCurrentGroupId;
    /// <summary>
    /// 当前进入房间类型
    /// </summary>
    private EnterRoomType m_CurrentType = EnterRoomType.None;
    /// <summary>
    /// 当前加入的房间Id
    /// </summary>
    private int m_nCurrentJoinRoomId;
    /// <summary>
    /// 套接字句柄
    /// </summary>
    public int SocketHandle;

    public int CurrentGameId;

    private IGameCtrl m_CurrentGameCtrl;
    #endregion

    #region DicNotificationInterests 注册UI事件
    /// <summary>
    /// 注册UI事件
    /// </summary>
    /// <returns></returns>
    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        dic.Add(ConstDefine.BtnCreateRoomViewCreate, OnBtnCreateRoomViewCreateClick);
        dic.Add("btnCreateRoomViewCreate_WangQue", OnBtnCreateRoomViewCreateClick_WangQue);
        dic.Add(ConstDefine.BtnJoinRoomViewJoin, OnBtnJoinViewJoinClick);
        dic.Add("OnBtnMicroUp", OnBtnDouDZMicroUp);
        dic.Add("OnBtnMicroCancel", OnBtnDouDZMicroCancel);
        return dic;
    }
    #endregion

    #region OpenView
    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.JoinRoom:
                OpenJoinRoomView();
                break;
            case UIWindowType.CreateRoom:
                OpenCreateRoomView();
                break;
            case UIWindowType.MyRoom:
                OpenMyRoomView();
                break;
        }
    }
    #endregion

    #region OpenCreateRoomView 打开创建房间窗口
    /// <summary>
    /// 打开创建房间窗口
    /// </summary>
    private void OpenCreateRoomView()
    {
        m_nCurrentGroupId = 0;
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.CreateRoom, (GameObject go) =>
        {
            m_UICreateRoomView = go.GetComponent<UICreateRoomView>();
            if (m_UICreateRoomView == null)
            {
                List<cfg_gameEntity> lst = cfg_gameDBModel.Instance.GetList();
                string gameType = lst.Count > 0 ? lst[0].GameType : string.Empty;
                UICreateRoomView2 view2 = go.GetComponent<UICreateRoomView2>();
                view2.SetGame(gameType);
                return;
            }
            m_UICreateRoomView.OnSettingMenuClick = OnSettingMenuClick;
            m_UICreateRoomView.OnOptionClick = OnSettingRuleToggle;

            List<cfg_gameEntity> lstGame = cfg_gameDBModel.Instance.GetList();
            m_UICreateRoomView.CreateMenu(lstGame);
            OnSettingMenuClick(lstGame[0].id);
        });
    }
    #endregion

    #region OpenCreateRoomView 打开创建房间窗口
    /// <summary>
    /// 打开创建房间窗口
    /// </summary>
    /// <param name="gameId"></param>
    public void OpenCreateRoomView(int gameId, int groupId = 0)
    {
        m_nCurrentGroupId = groupId;
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.CreateRoom, (GameObject go) =>
        {
            m_UICreateRoomView = go.GetComponent<UICreateRoomView>();
            if (m_UICreateRoomView == null)
            {
                UICreateRoomView2 view2 = go.GetComponent<UICreateRoomView2>();
                if (gameId == 0)
                {
                    view2.SetGame(string.Empty);
                }
                else
                {
                    view2.SetGame(cfg_gameDBModel.Instance.Get(gameId).GameType);
                }
                return;
            }
            m_UICreateRoomView.OnSettingMenuClick = OnSettingMenuClick;
            m_UICreateRoomView.OnOptionClick = OnSettingRuleToggle;
            OnSettingMenuClick(gameId);
        });
    }
    #endregion

    #region OpenJoinRoomView 打开加入房间窗口
    /// <summary>
    /// 打开加入房间窗口
    /// </summary>
    private void OpenJoinRoomView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.JoinRoom, (GameObject go) =>
        {
            m_UIJoinRoomView = go.GetComponent<UIJoinRoomView>();
        });
    }
    #endregion

    #region OpenMyRoomView 打开我的房间窗口
    /// <summary>
    /// 打开我的房间窗口
    /// </summary>
    /// <param name="obj"></param>
    private void OpenMyRoomView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.MyRoom, (GameObject go) =>
        {
            m_UIMyRoomView = go.GetComponent<UIMyRoomView>();
            m_UIMyRoomView.onJoinClick = OnMyRoomJoinClick;
            m_UIMyRoomView.onInviteClick = OnMyRoomInviteClick;
            m_UIMyRoomView.onPlayerClick = OnMyRoomPlayerClick;
            m_UIMyRoomView.onUpdate = OnMyRoomUpdate;
            RequestRoomList();
        });
    }
    #endregion

    #region OnSettingMenuClick 设置菜单点击
    /// <summary>
    /// 设置菜单点击
    /// </summary>
    /// <param name="gameId"></param>
    private void OnSettingMenuClick(int gameId)
    {
        m_nCurrentSelectGame = gameId;
        List<cfg_settingEntity> lst = cfg_settingDBModel.Instance.GetRuleByGameId(gameId);
        cfg_gameEntity gameEntity = cfg_gameDBModel.Instance.Get(gameId);
        if (gameEntity == null) return;
        int payment = gameEntity.Payment;

        int playerCount = 0;
        for (int i = 0; i < lst.Count; ++i)
        {
            cfg_settingEntity entity = lst[i];
            if (entity.tags.Equals("player"))
            {
                List<cfg_settingEntity> options = cfg_settingDBModel.Instance.GetOptionsByRuleNameAndGameId(entity.gameId, entity.label);
                for (int j = 0; j < options.Count; ++j)
                {
                    if (options[j].init == 1)
                    {
                        playerCount = options[j].value;
                    }
                }
                break;
            }
        }
        bool isAA = payment == 1;
        if (payment == 2)
        {
            for (int i = 0; i < lst.Count; ++i)
            {
                cfg_settingEntity entity = lst[i];
                if (entity.tags.Equals("card"))
                {
                    List<cfg_settingEntity> options = cfg_settingDBModel.Instance.GetOptionsByRuleNameAndGameId(entity.gameId, entity.label);
                    for (int j = 0; j < options.Count; ++j)
                    {
                        if (options[j].init == 1)
                        {
                            isAA = options[j].value == 2;
                        }
                    }
                    break;
                }
            }
        }

        m_UICreateRoomView.SetContent(lst, payment, isAA, playerCount);
    }
    #endregion

    #region OnSettingRuleToggle 选择规则回调
    /// <summary>
    /// 选择规则回调
    /// </summary>
    /// <param name="optionName"></param>
    /// <param name="isOn"></param>
    private void OnSettingRuleToggle(int id)
    {
        cfg_settingDBModel.Instance.SelectOption(id);
        OnSettingMenuClick(m_nCurrentSelectGame);
    }
    #endregion

    #region OnBtnCreateRoomViewCreateClick 创建房间窗口创建按钮点击
    /// <summary>
    /// 创建房间窗口创建按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnCreateRoomViewCreateClick(object[] obj)
    {
        List<int> selectId = obj[0] as List<int>;

        if (selectId != null && selectId.Count > 0)
        {
            List<cfg_settingEntity> lst = cfg_settingDBModel.Instance.GetList();
            if (lst != null)
            {
                for (int i = 0; i < lst.Count; ++i)
                {
                    for (int j = 0; j < selectId.Count; ++j)
                    {
                        if (lst[i].id == selectId[j])
                        {
                            lst[i].init = 1;
                            lst[i].status = 1;
                            break;
                        }
                        else
                        {
                            lst[i].init = 0;
                        }
                    }
                }
            }
        }

        CreateRoom(m_nCurrentSelectGame);
    }
    #endregion

    #region OnBtnCreateRoomViewCreateClick_WangQue 网雀创建房间窗口创建按钮点击
    /// <summary>
    /// 网雀创建房间窗口创建按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnCreateRoomViewCreateClick_WangQue(object[] obj)
    {
        int gameId = (int)obj[0];
        List<int> lstRuleId = (List<int>)obj[1];

        if (lstRuleId != null && lstRuleId.Count > 0)
        {
            List<cfg_settingEntity> lst = cfg_settingDBModel.Instance.GetAllOptions();
            if (lst != null)
            {
                for (int i = 0; i < lst.Count; ++i)
                {
                    for (int j = 0; j < lstRuleId.Count; ++j)
                    {
                        if (lst[i].id == lstRuleId[j])
                        {
                            lst[i].init = 1;
                            lst[i].status = 1;
                            break;
                        }
                        else
                        {
                            lst[i].init = 0;
                        }
                    }
                }
            }
        }
        cfg_settingDBModel.Instance.SaveSetting();
        CreateRoom(gameId);
    }
    #endregion

    #region OnBtnJoinViewJoinClick 加入房间窗口加入按钮点击
    /// <summary>
    /// 加入房间窗口加入按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnJoinViewJoinClick(object[] obj)
    {
        int roomId = (int)obj[0];
        RequestJoinRoom(roomId);
    }
    #endregion

    #region OnBtnDouDZMicroCancel 语音按钮取消
    /// <summary>
    /// 语音按钮取消
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnDouDZMicroCancel(object[] obj)
    {
        ChatCtrl.Instance.CancelMicro();
    }
    #endregion

    #region OnBtnDouDZMicroUp 语音按钮抬起
    /// <summary>
    /// 语音按钮抬起
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnDouDZMicroUp(object[] obj)
    {
        ChatCtrl.Instance.SendMicro();
    }
    #endregion

    #region RequestJoinRoom 请求加入房间
    /// <summary>
    /// 请求加入房间
    /// </summary>
    /// <param name="roomId"></param>
    public void RequestJoinRoom(int roomId)
    {
        if (m_isBusy) return;
        m_isBusy = true;
        UIViewManager.Instance.ShowWait();
        Dictionary<string, object> dic = new Dictionary<string, object>();
        AccountEntity entity = AccountProxy.Instance.CurrentAccountEntity;
        dic["passportId"] = entity.passportId;
        dic["token"] = entity.token;
        dic["bundleId"] = DeviceUtil.GetBundleIdentifier();
        dic["roomId"] = roomId;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrJoin, OnRequestJoinRoomCallBack, true, ConstDefine.HTTPFuncJoin, dic);
    }
    #endregion

    #region OnJoinRoomCallBack 请求加入房间回调
    /// <summary>
    /// 请求加入房间回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestJoinRoomCallBack(NetWorkHttp.CallBackArgs args)
    {
        UIViewManager.Instance.CloseWait();
        m_isBusy = false;
        if (args.HasError)
        {
            ShowMessage("错误", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("错误提示", args.Value.msg);
                return;
            }
            LitJson.JsonData data = args.Value.data;
            JoinRoom(data["gameId"].ToString().ToInt(), data["roomId"].ToString().ToInt());
        }
    }
    #endregion

    #region RequestRebuild 请求重建房间
    /// <summary>
    /// 请求重建房间
    /// </summary>
    public void RequestRebuildRoom()
    {
        if (m_isBusy) return;
        m_isBusy = true;
        UIViewManager.Instance.ShowWait();
        Dictionary<string, object> dic = new Dictionary<string, object>();
        AccountEntity entity = AccountProxy.Instance.CurrentAccountEntity;
        dic["passportId"] = entity.passportId;
        dic["token"] = entity.token;
        dic["bundleId"] = DeviceUtil.GetBundleIdentifier();
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrRenter, OnRequestRebuildRoomCallBack, true, ConstDefine.HTTPFuncRenter, dic);
    }
    #endregion

    #region OnRebuildCallBack 请求重建回调
    /// <summary>
    /// 请求重建回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestRebuildRoomCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        UIViewManager.Instance.CloseWait();
        if (args.HasError)
        {
            ShowMessage("失败", "网络连接失败", MessageViewType.Ok, RequestRebuildRoom);
        }
        else
        {
            if (args.Value.code < 0)
            {
                if (SceneMgr.Instance.CurrentSceneType != SceneType.Main)
                {
                    SceneMgr.Instance.LoadScene(SceneType.Main);
                }
                return;
            }
            LitJson.JsonData data = args.Value.data;
            int gameId = data["gameId"].ToString().ToInt();
            int roomId = data["roomId"].ToString().ToInt();
            int matchId = 0;
            if (data.Inst_Object.ContainsKey("settingId"))
            {
                matchId = data["settingId"].ToString().ToInt();
            }

            if (matchId > 0)
            {
                MatchCtrl.Instance.RequestBattleList(matchId, roomId);
            }

            if (roomId > 0)
            {
                CurrentGameId = gameId;
                RebuildRoom();
            }
        }
    }
    #endregion




    #region 获取服务器列表
    /// <summary>
    /// 获取服务器列表
    /// </summary>
    public void RequestGameServer()
    {
        if (m_isBusy) return;
        m_isBusy = true;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["bundleId"] = DeviceUtil.GetBundleIdentifier();
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrGateway, OnRequestGameServerCallBack, true, ConstDefine.HTTPFuncGateway, dic);
    }
    #endregion

    #region OnRequestGameServerCallBack 获取服务器列表回调
    /// <summary>
    /// 获取服务器列表回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestGameServerCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        if (args.HasError)
        {
            ShowMessage("错误", "网络连接失败,请重新尝试", MessageViewType.Ok, RequestGameServer);
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("错误提示", args.Value.msg, MessageViewType.Ok);
                return;
            }
            GameProxy.Instance.GameServers = LitJson.JsonMapper.ToObject<List<GameEntity>>(LitJson.JsonMapper.ToJson(args.Value.data["gateway"]));

            if (PlayerPrefs.HasKey("Receipt"))
            {
                ShopCtrl.Instance.Receipt(PlayerPrefs.GetString("OrderId"), PlayerPrefs.GetString("Receipt"));
            }

            RequestRebuildRoom();
        }
    }
    #endregion



    #region RequestRoomList 请求房间列表
    /// <summary>
    /// 请求房间列表
    /// </summary>
    private void RequestRoomList()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/roomlist/", OnRequestRoomListCallBack, true, "roomlist", dic);
    }
    #endregion

    #region OnRequestRoomListCallBack 请求房间列表回调
    /// <summary>
    /// 请求房间列表回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestRoomListCallBack(NetWorkHttp.CallBackArgs args)
    {
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

            m_RoomList = LitJson.JsonMapper.ToObject<List<MyRoomEntity>>(args.Value.data.ToJson());

            if (m_UIMyRoomView != null)
            {
                m_UIMyRoomView.SetUI(m_RoomList, SystemProxy.Instance.IsInstallWeChat && SystemProxy.Instance.IsOpenWXLogin);
            }
        }
    }
    #endregion

    #region OnMyRoomJoinClick 我的房间界面进入按钮点击
    /// <summary>
    /// 我的房间界面进入按钮点击
    /// </summary>
    /// <param name="roomId"></param>
    private void OnMyRoomJoinClick(int roomId)
    {
        RequestJoinRoom(roomId);
    }
    #endregion

    #region OnMyRoomInviteClick 我的房间界面邀请按钮点击
    /// <summary>
    /// 我的房间界面邀请按钮点击
    /// </summary>
    /// <param name="roomId"></param>
    private void OnMyRoomInviteClick(int roomId)
    {
        MyRoomEntity entity = m_RoomList.Find(p => p.roomId == roomId);
        ShareCtrl.Instance.InviteFriend(entity.roomId, entity.gameId, entity.roomSetting);
    }
    #endregion

    #region OnMyRoomPlayerClick 我的房间界面玩家人数按钮点击
    /// <summary>
    /// 我的房间界面玩家人数按钮点击
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="container"></param>
    private void OnMyRoomPlayerClick(int roomId, Transform container)
    {
        MyRoomEntity entity = m_RoomList.Find(p => p.roomId == roomId);
        m_UIMyRoomView.SetPlayerView(entity.players, container, entity.maxPlayer);
    }
    #endregion

    #region OnMyRoomUpdate 更新我的房间界面
    /// <summary>
    /// 更新我的房间界面
    /// </summary>
    private void OnMyRoomUpdate()
    {
        RequestRoomList();
    }
    #endregion


    #region OnDisConnectCallBack 网络连接中断回调
    /// <summary>
    /// 网络连接中断回调
    /// </summary>
    private void OnDisConnectCallBack(bool isActiveClose)
    {
        Debug.Log("中断了 重建不？" + !isActiveClose);
        if (!isActiveClose)
        {
            RebuildRoom();
        }
    }
    #endregion

    #region OnReconnectCallBack 重新连接回调
    /// <summary>
    /// 重新连接回调
    /// </summary>
    private void OnReconnectCallBack()
    {
        m_CurrentGameCtrl.RebuildRoom();
    }
    #endregion











    #region CreateRoom 创建房间
    /// <summary>
    /// 创建房间
    /// </summary>
    /// <param name="gameId"></param>
    public void CreateRoom(int gameId)
    {
        m_CurrentType = EnterRoomType.Create;

        if (NetWorkSocket.Instance.Connected(SocketHandle) && CurrentGameId != gameId)
        {
            NetWorkSocket.Instance.Close(SocketHandle);
        }
        CurrentGameId = gameId;
        GameEntity entity = GameProxy.Instance.Get(gameId);
        if (entity == null)
        {
            ShowMessage("提示","此游戏暂未开放");
            return;
        }
#if DEBUG_MODE
        if (!string.IsNullOrEmpty(GlobalInit.Instance.TestIP))
        {
            entity.ipaddr = GlobalInit.Instance.TestIP;
            entity.port = GlobalInit.Instance.TestPort;
        }
#endif
        ConnectServer(EnterRoomType.Create, entity.ipaddr, entity.port);
    }
    #endregion

    #region JoinRoom 加入房间
    /// <summary>
    /// 加入房间
    /// </summary>
    /// <param name="roomId"></param>
    public void JoinRoom(int gameId, int roomId)
    {
        m_nCurrentJoinRoomId = roomId;
        m_CurrentType = EnterRoomType.Join;

        if (NetWorkSocket.Instance.Connected(SocketHandle) && CurrentGameId != gameId)
        {
            NetWorkSocket.Instance.Close(SocketHandle);
        }
        CurrentGameId = gameId;
        GameEntity entity = GameProxy.Instance.Get(gameId);
#if DEBUG_MODE
        if (!string.IsNullOrEmpty(GlobalInit.Instance.TestIP))
        {
            entity.ipaddr = GlobalInit.Instance.TestIP;
            entity.port = GlobalInit.Instance.TestPort;
        }
#endif
        ConnectServer(EnterRoomType.Join, entity.ipaddr, entity.port);
    }
    #endregion

    #region RebuildRoom 重建房间
    /// <summary>
    /// 重建房间
    /// </summary>
    public void RebuildRoom()
    {
        m_CurrentType = EnterRoomType.Renter;

        GameEntity entity = GameProxy.Instance.Get(CurrentGameId);
#if DEBUG_MODE
        if (!string.IsNullOrEmpty(GlobalInit.Instance.TestIP))
        {
            entity.ipaddr = GlobalInit.Instance.TestIP;
            entity.port = GlobalInit.Instance.TestPort;
        }
#endif
        ConnectServer(EnterRoomType.Renter, entity.ipaddr, entity.port);
    }
    #endregion

    #region QuitRoom 退出房间
    /// <summary>
    /// 退出房间
    /// </summary>
    public void QuitRoom()
    {
        if (m_CurrentGameCtrl == null) return;
        m_CurrentGameCtrl.QuitRoom();
    }
    #endregion

    #region DisbandRoom 解散房间
    /// <summary>
    /// 解散房间
    /// </summary>
    public void DisbandRoom()
    {
        if (m_CurrentGameCtrl == null) return;
        m_CurrentGameCtrl.DisbandRoom();
    }
    #endregion

    #region BeginMatch 比赛开始
    /// <summary>
    /// 比赛开始
    /// </summary>
    /// <param name="gameId"></param>
    public void BeginMatch(int gameId)
    {
        m_CurrentGameCtrl = GetGameCtrl(gameId);
    }
    #endregion

    #region OnReceiveMessage 接收聊天消息
    /// <summary>
    /// 接收聊天消息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="playerId"></param>
    /// <param name="message"></param>
    /// <param name="audioName"></param>
    public void OnReceiveMessage(ChatType type, int playerId, string message, string audioName, int toPlayerId)
    {
        if (m_CurrentGameCtrl == null) return;
        m_CurrentGameCtrl.OnReceiveMessage(type, playerId, message, audioName, toPlayerId);
    }
    #endregion

    #region GetRoomEntity 获取房间数据实体
    /// <summary>
    /// 获取房间数据实体
    /// </summary>
    /// <returns></returns>
    public RoomEntityBase GetRoomEntity()
    {
        if (m_CurrentGameCtrl == null) return new RoomEntityBase();
        return m_CurrentGameCtrl.GetRoomEntity();
    }
    #endregion




    #region GetGameCtrl 获取游戏控制器
    /// <summary>
    /// 获取游戏控制器
    /// </summary>
    /// <param name="gameId"></param>
    /// <returns></returns>
    private IGameCtrl GetGameCtrl(int gameId)
    {
        cfg_gameEntity gameEntity = cfg_gameDBModel.Instance.Get(gameId);
        if (gameEntity != null)
        {
            return GetGameCtrl(gameEntity.GameType);
        }
        return null;
    }

    /// <summary>
    /// 获取游戏控制器
    /// </summary>
    /// <param name="gameType">游戏类型</param>
    /// <returns></returns>
    private IGameCtrl GetGameCtrl(string gameType)
    {
        GameType type = (GameType)Enum.Parse(typeof(GameType), gameType, true);
        return GetGameCtrl(type);
    }

    /// <summary>
    /// 获取游戏控制器
    /// </summary>
    /// <param name="type">游戏类型</param>
    /// <returns></returns>
    private IGameCtrl GetGameCtrl(GameType type)
    {
        IGameCtrl ret = null;

        switch (type)
        {
            case GameType.Majiang:
                ret = MaJiangGameCtrl.Instance;
                break;
            case GameType.Niuniu:
                ret = NiuNiuGameCtrl.Instance;
                break;
            case GameType.Paijiu:
                ret = PaiJiuGameCtrl.Instance;
                break;
            case GameType.PaoDeKuai:
                ret = PaoDeKuaiGameCtrl.Instance;
                break;
            case GameType.Zhajinhua:
                ret = ZhaJHGameCtrl.Instance;
                break;
            case GameType.Jigala:
                ret = JuYouGameCtrl.Instance;
                break;
            case GameType.Gupai:
                ret = GuPaiJiuGameCtrl.Instance;
                break;
            case GameType.GuanDan:
                ret = GuanDanGameCtrl.Instance;
                break;
            case GameType.Doudizhu:
                ret = DouDiZhuGameCtrl.Instance;
                break;
            case GameType.ShiSanShui:
                ret = ShiSanZhangGameCtrl.Instance;
                break;
        }

        return ret;
    }
    #endregion




    #region ConnectServer 连接服务器
    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <param name="type"></param>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public void ConnectServer(EnterRoomType type, string ip, int port)
    {
        if (NetWorkSocket.Instance.Connected(SocketHandle))
        {
            OnConnectedCallBack(true);
            return;
        }
        UIViewManager.Instance.ShowWait();
        m_CurrentType = type;
        SocketHandle = NetWorkSocket.Instance.BeginConnect(ip, port, OnConnectedCallBack);
    }
    #endregion

    #region OnConnectedCallBack 连接服务器回调
    /// <summary>
    /// 连接服务器回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnConnectedCallBack(bool isSuccess)
    {
        UIViewManager.Instance.CloseWait();
        if (isSuccess)
        {
            NetWorkSocket.Instance.GetSocket(SocketHandle).OnDisConnect = OnDisConnectCallBack;//断开连接事件
            NetWorkSocket.Instance.GetSocket(SocketHandle).OnReconnect = OnReconnectCallBack;//重新连接事件
            m_CurrentGameCtrl = GetGameCtrl(CurrentGameId);
            if (m_CurrentGameCtrl == null)
            {
                throw new NotSupportedException("没有此Id:" + CurrentGameId.ToString());
            }

            if (m_CurrentType == EnterRoomType.Create)
            {
                List<int> ids = new List<int>();               
                List<cfg_settingEntity> lst = cfg_settingDBModel.Instance.GetOptionsByGameId(CurrentGameId);               
                for (int i = 0; i < lst.Count; ++i)
                {
                    if (lst[i].status == 1 && lst[i].init == 1)
                    {
                        Debug.Log(lst[i].id);
                        ids.Add(lst[i].id);
                    }
                }
                m_CurrentGameCtrl.CreateRoom(m_nCurrentGroupId, ids);
            }
            else if (m_CurrentType == EnterRoomType.Join)
            {
                m_CurrentGameCtrl.JoinRoom(m_nCurrentJoinRoomId);
            }
            else
            {
                m_CurrentGameCtrl.RebuildRoom();
            }
        }
        else
        {
            if (m_CurrentType == EnterRoomType.Renter)
            {
                UIViewManager.Instance.ShowMessage("错误提示", "网络连接失败,请重新尝试", MessageViewType.Ok, RebuildRoom, null, 20f, AutoClickType.Ok);
            }
            else
            {
                UIViewManager.Instance.ShowMessage("错误提示", "网络连接失败", MessageViewType.Ok, ExitGame);
            }
        }
    }
    #endregion

    #region ExitGame 退出游戏房间
    /// <summary>
    /// 退出游戏房间
    /// </summary>
    public void ExitGame()
    {
        if (NetWorkSocket.Instance.Connected(SocketHandle))
        {
            NetWorkSocket.Instance.SafeClose(SocketHandle);
        }
        SceneMgr.Instance.LoadScene(SceneType.Main);
    }
    #endregion
}
