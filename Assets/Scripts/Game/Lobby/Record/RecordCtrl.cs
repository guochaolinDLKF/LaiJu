//===================================================
//Author      : DRB
//CreateTime  ：4/7/2017 3:40:21 PM
//Description ：战绩模块控制器
//===================================================
using System.Collections.Generic;
using UnityEngine;

public class RecordCtrl : SystemCtrlBase<RecordCtrl>, ISystemCtrl
{
    #region Variable
    private UIRecordView m_UIRecordView;//战绩窗口

    private bool m_isBusy;//是否操作繁忙

    private string m_CurrentGameType;

    private int m_CurrentRequestGameId;
    #endregion

    #region DicNotificationInterests 事件注册
    /// <summary>
    /// 事件注册
    /// </summary>
    /// <returns></returns>
    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        dic.Add("btnRecordViewSeeRecord", OnBtnRecordViewSeeRecordClick);//查看战绩
        dic.Add("btnRecordViewDetail", OnBtnRecordViewDetailClick);//查看详情
        dic.Add("btnRecordViewPlayBack", OnBtnRecordViewPlayBackClick);//回放
        dic.Add("btnRecordViewGame", btnRecordViewGameClick);//游戏
        return dic;
    }
    #endregion

    #region ISystemCtrl
    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.Record:
                OpenRecordView();
                break;
        }
    }
    #endregion

    #region OpenRecordView 打开战绩窗口
    /// <summary>
    /// 打开战绩窗口
    /// </summary>
    private void OpenRecordView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Record, (GameObject go) =>
        {
            m_UIRecordView = go.GetComponent<UIRecordView>();
            List<cfg_gameEntity> lst = cfg_gameDBModel.Instance.GetList();
            List<cfg_gameEntity> newList = new List<cfg_gameEntity>(lst);
            newList.Sort();
            RequestRecord(m_CurrentRequestGameId>0? m_CurrentRequestGameId: newList[0].id);
        });
    }
    #endregion

    #region btnRecordViewGameClick 游戏按钮点击
    /// <summary>
    /// 游戏按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void btnRecordViewGameClick(object[] obj)
    {
        int gameID= (int)obj[0];
        cfg_gameEntity game = cfg_gameDBModel.Instance.Get(gameID);
        if (game == null) return;
        RequestRecord(game.id);
    }
    #endregion

    #region OnBtnRecordViewSeeRecordClick 战绩窗口查看战绩按钮点击
    /// <summary>
    /// 战绩窗口查看战绩按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnRecordViewSeeRecordClick(object[] obj)
    {
        int battleId = (int)obj[0];
        if (battleId == 0)
        {
            ShowMessage("提示", "请输入正确的战绩号");
            return;
        }

        RequestRecordByBattleId(m_CurrentRequestGameId, battleId);
    }
    #endregion

    #region OnSeeDetailClick 查看详情按钮点击
    /// <summary>
    /// 查看详情按钮点击
    /// </summary>
    /// <param name="battleId"></param>
    private void OnBtnRecordViewDetailClick(object[] obj)
    {
        int battleId = (int)obj[0];
        if (battleId == 0)
        {
            ShowMessage("提示", "请输入正确的战绩号");
            return;
        }

        TestRecord record = RecordProxy.Instance.GetRecord(battleId);
        if (record == null) return;

        cfg_gameEntity gameEntity = cfg_gameDBModel.Instance.Get(m_CurrentRequestGameId);
        if (gameEntity == null) return;
        string gameType = gameEntity.GameType;

        TransferData data = new TransferData();
        data.SetValue("GameType", gameType);
        data.SetValue("BattleId", battleId);
        data.SetValue("RoomId", record.roomId);
        data.SetValue("GameId", record.gameId);
        data.SetValue("MaxLoop", record.detail.Count);
        data.SetValue("DateTime", record.time);
        data.SetValue("ChineseGameName", gameEntity.GameName);
        data.SetValue("OwnerName", record.ownerName);


        List<TransferData> lstPlayers = new List<TransferData>();
        List<TransferData> lst = new List<TransferData>();
        for (int i = 0; i < record.Players.Count; ++i)
        {
            TransferData playerData = new TransferData();
            playerData.SetValue("NickName", record.Players[i].nickname);
            playerData.SetValue("Gold", record.Players[i].gold);
            lstPlayers.Add(playerData);
        }
        data.SetValue("Player", lstPlayers);
        for (int i = 0; i < record.detail.Count; ++i)
        {
            TestRecordDetail detail = record.detail[i];
            TransferData recordData = new TransferData();
            recordData.SetValue("RecordId", detail.id);
            recordData.SetValue("Loop", detail.loop);
            recordData.SetValue("DateTime", detail.time);
            List<TransferData> lstPlayer = new List<TransferData>();
            for (int j = 0; j < detail.players.Count; ++j)
            {
                TransferData playerData = new TransferData();
                playerData.SetValue("NickName", detail.players[j].nickname);
                playerData.SetValue("Gold", detail.players[j].gold);
                playerData.SetValue("Avatar", detail.players[j].avatar);
                List<TransferData> lstPoker = new List<TransferData>();
                for (int k = 0; k < detail.players[j].poker.Count; ++k)
                {
                    TransferData pokerData = new TransferData();
                    pokerData.SetValue("Color", detail.players[j].poker[k].color);
                    pokerData.SetValue("Size", detail.players[j].poker[k].size);
                    lstPoker.Add(pokerData);
                }
                playerData.SetValue("Pokers", lstPoker);
                lstPlayer.Add(playerData);
            }
            recordData.SetValue("Player", lstPlayer);
            lst.Add(recordData);
        }
        data.SetValue("RecordDetail", lst);
        m_UIRecordView.ShowRecordDetail(data);
    }
    #endregion

    #region OnReplayClick 回放按钮点击
    /// <summary>
    /// 回放按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnRecordViewPlayBackClick(object[] obj)
    {
        int recordId = (int)obj[0];
        Replay(recordId);
    }
    #endregion

    #region Replay 请求回放
    /// <summary>
    /// 请求回放
    /// </summary>
    /// <param name="id"></param>
    private void Replay(int id)
    {
        if (m_isBusy) return;
        m_isBusy = true;
        UIViewManager.Instance.ShowWait();
        Dictionary<string, object> dic = new Dictionary<string, object>();
        AccountEntity entity = AccountProxy.Instance.CurrentAccountEntity;
        dic["passportId"] = entity.passportId;
        dic["token"] = entity.token;
        dic["id"] = id;
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrBattle, OnReplayCallBack, true, ConstDefine.HTTPFuncBattle, dic);
    }
    #endregion

    #region OnReplayCallBack 请求回放回调
    /// <summary>
    /// 请求回放回调
    /// </summary>
    /// <param name="args"></param>
    private void OnReplayCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
        UIViewManager.Instance.CloseWait();
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
            RecordProxy.Instance.CurrentReplayEntity = LitJson.JsonMapper.ToObject<RecordReplayEntity>(LitJson.JsonMapper.ToJson(args.Value.data));

            MaJiangGameCtrl.Instance.Replay(RecordProxy.Instance.CurrentReplayEntity);
        }
    }
    #endregion


    #region RequestRecord 请求战绩
    /// <summary>
    /// 请求战绩
    /// </summary>
    private void RequestRecord(int gameId)
    {
        if (m_isBusy) return;
        m_isBusy = true;
        m_CurrentRequestGameId = gameId;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["gameId"] = gameId;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/record/", RequestRecordCallBack, true, "record", dic);
    }
    #endregion

    #region RequestRecordCallBack 请求战绩回调
    /// <summary>
    /// 请求战绩回调
    /// </summary>
    /// <param name="args"></param>
    private void RequestRecordCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
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

            LitJson.JsonData jsonData = args.Value.data;
            List<TestRecord> lstRecord = LitJson.JsonMapper.ToObject<List<TestRecord>>(jsonData.ToJson());

            RecordProxy.Instance.AllRecord = lstRecord;

            cfg_gameEntity gameEntity = cfg_gameDBModel.Instance.Get(m_CurrentRequestGameId);
            if (lstRecord.Count>0)
            {
              gameEntity = cfg_gameDBModel.Instance.Get(lstRecord[0].gameId);
            }
            if (gameEntity == null) return;
            string gameType = gameEntity.GameType;

            TransferData data = new TransferData();
            data.SetValue("GameType", gameType);
            data.SetValue("GameId", gameEntity.id);

            List<TransferData> lst = new List<TransferData>();
            for (int i = 0; i < lstRecord.Count; ++i)
            {
                TestRecord record = lstRecord[i];
                TransferData recordData = new TransferData();
                recordData.SetValue("Index", i + 1);
                recordData.SetValue("BattleId", record.battleId);
                recordData.SetValue("RoomId", record.roomId);
                recordData.SetValue("GameId", record.gameId);
                recordData.SetValue("MaxLoop", record.detail.Count);
                recordData.SetValue("RoomType", record.roomType);
                recordData.SetValue("OwnerName", record.ownerName);
                recordData.SetValue("DateTime", record.time);
                List<TransferData> lstPlayer = new List<TransferData>();
                for (int j = 0; j < record.Players.Count; ++j)
                {
                    TestPlayer player = record.Players[j];
                    TransferData playerData = new TransferData();
                    playerData.SetValue("NickName", player.nickname);
                    playerData.SetValue("Gold", player.gold);
                    playerData.SetValue("PlayerID", player.id);
                    playerData.SetValue("IsPlayer", player.id== AccountProxy.Instance.CurrentAccountEntity.passportId);
                    lstPlayer.Add(playerData);
                }
                recordData.SetValue("Player", lstPlayer);
                lst.Add(recordData);
            }
            data.SetValue("Record", lst);
            m_UIRecordView.ShowRecord(data);
        }
    }
    #endregion

    #region RequestRecordByBattleId 根据牌局Id请求战绩
    /// <summary>
    /// 根据牌局Id请求战绩
    /// </summary>
    /// <param name="gameId"></param>
    /// <param name="battleId"></param>
    private void RequestRecordByBattleId(int gameId, int battleId)
    {
        if (m_isBusy) return;
        m_isBusy = true;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
        dic["gameId"] = gameId;
        dic["battleId"] = battleId;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/recordDetail/", RequestRecordByBattleIdCallBack, true, "recordDetail", dic);
    }
    #endregion

    #region RequestRecordByBattleIdCallBack 根据牌局Id请求战绩回调
    /// <summary>
    /// 根据牌局Id请求战绩回调
    /// </summary>
    /// <param name="args"></param>
    private void RequestRecordByBattleIdCallBack(NetWorkHttp.CallBackArgs args)
    {
        m_isBusy = false;
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

            LitJson.JsonData jsonData = args.Value.data;
            if (jsonData.Count == 0)
            {
                ShowMessage("提示", "无此牌局战绩");
                return;
            }
            List<TestRecord> lstRecord = LitJson.JsonMapper.ToObject<List<TestRecord>>(jsonData.ToJson());

            RecordProxy.Instance.AllRecord = lstRecord;

            cfg_gameEntity gameEntity = cfg_gameDBModel.Instance.Get(m_CurrentRequestGameId);
            if (gameEntity == null) return;
            string gameType = gameEntity.GameType;

            TransferData data = new TransferData();
            data.SetValue("GameType", gameType);
            List<TransferData> lst = new List<TransferData>();
            for (int i = 0; i < lstRecord.Count; ++i)
            {
                TestRecord record = lstRecord[i];
                TransferData recordData = new TransferData();
                recordData.SetValue("Index", i + 1);
                recordData.SetValue("BattleId", record.battleId);
                recordData.SetValue("RoomId", record.roomId);
                recordData.SetValue("GameId", record.gameId);
                recordData.SetValue("MaxLoop", record.detail.Count);
                recordData.SetValue("RoomType", record.roomType);
                recordData.SetValue("OwnerName", record.ownerName);
                recordData.SetValue("DateTime", record.time);
                List<TransferData> lstPlayer = new List<TransferData>();
                for (int j = 0; j < record.Players.Count; ++j)
                {
                    TestPlayer player = record.Players[j];
                    TransferData playerData = new TransferData();
                    playerData.SetValue("NickName", player.nickname);
                    playerData.SetValue("Gold", player.gold);
                    playerData.SetValue("PlayerID", player.id);
                    playerData.SetValue("IsPlayer", player.id == AccountProxy.Instance.CurrentAccountEntity.passportId);
                    lstPlayer.Add(playerData);
                }
                recordData.SetValue("Player", lstPlayer);
                lst.Add(recordData);
            }
            data.SetValue("Record", lst);
            m_UIRecordView.ShowRecord(data);
        }
    }
    #endregion




}
