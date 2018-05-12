//===================================================
//Author      : CZH
//CreateTime  ：7/25/2017 2:38:10 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using ddz.proto;
using DRB.DouDiZhu;

public class DouDiZhuGameCtrl : SystemCtrlBase<DouDiZhuGameCtrl>, IGameCtrl, ISystemCtrl
{
    private UISettleView m_UISettleView;

    private UIResultView_DouDZ m_UIResultView;

    public List<Poker> SelectPoker = new List<Poker>();

    public List<Deck> Tip;

    public int TipIndex;

    public Queue<IGameCommand> CommandQueue = new Queue<IGameCommand>();

    #region Constructor
    public DouDiZhuGameCtrl()
    {
        NetDispatcher.Instance.AddEventListener(DDZ_ROOM_CREATE.CODE, OnServerReturnCreateRoom);//创建房间
        NetDispatcher.Instance.AddEventListener(DDZ_ROOM_RECREATE.CODE, OnServerReturnRebuild);//重建房间
        NetDispatcher.Instance.AddEventListener(DDZ_ROOM_ENTER.CODE, OnServerBroadcastEnter);//进入房间
        NetDispatcher.Instance.AddEventListener(DDZ_ROOM_LEAVE.CODE, OnServerBroadcastLeave);//离开房间 
        NetDispatcher.Instance.AddEventListener(DDZ_ROOM_READY.CODE, OnServerBroadcastReady); //准备
        NetDispatcher.Instance.AddEventListener(DDZ_ROOM_SHOW_POKER_GAME.CODE, OnServerBroadcastShowPoker);//明牌
        NetDispatcher.Instance.AddEventListener(DDZ_ROOM_DEAL.CODE, OnServerBroadcastBegin);//开局
        NetDispatcher.Instance.AddEventListener(DDZ_ROOM_INFORMQIANG.CODE, OnServerAskBet2);//询问下注
        NetDispatcher.Instance.AddEventListener(DDZ_ROOM_QIANG.CODE, OnServerBroadcastBet);//下注
        NetDispatcher.Instance.AddEventListener(DDZ_ROOM_INFORMBANKER.CODE, OnServerBroadcastBanker);//广播地主
        NetDispatcher.Instance.AddEventListener(DDZ_ROOM_INFORMPLAYPOKER.CODE, OnServerAskPlayPoker);//询问出牌
        NetDispatcher.Instance.AddEventListener(DDZ_ROOM_PLAYPOKER.CODE, OnServerBroadcastPlayPoker);//出牌
        NetDispatcher.Instance.AddEventListener(DDZ_ROOM_PASS.CODE, OnServerBroadcastPass);//过
        NetDispatcher.Instance.AddEventListener(DDZ_ROOM_AGAINDEAL.CODE, OnServerBroadcastReDeal);//重新发牌
        NetDispatcher.Instance.AddEventListener(DDZ_ROOM_SETTLE.CODE, OnServerBroadcastSettle);//小结算
        NetDispatcher.Instance.AddEventListener(DDZ_ROOM_TOTALSETTLE.CODE, OnServerBroadcastResult);//牌局结束

        NetDispatcher.Instance.AddEventListener(DDZ_ROOM_APPLYDISMISS.CODE, OnServerBroadcastDisbandRoom);//解散
    }
    #endregion

    #region Dispose
    public override void Dispose()
    {
        base.Dispose();
    }
    #endregion

    #region DicNotificationInterests
    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        dic.Add("btnDouDiZhuViewReady", OnReadyClick);//准备按钮
        dic.Add("OnDouDiZhuPokerClickUp", OnPokerClickUp);//牌点击
        dic.Add("btnDouDiZhuViewPlayPoker", OnPlayPokerClick);//出牌按钮
        dic.Add("DouDiZhuViewAskLastPokersComplete", OnPlayLastPoker);//自动出最后一张牌
        dic.Add("btnDouDiZhuViewPass", OnPassClick);//过按钮
        dic.Add("btnDouDiZhuViewBet", OnBetClick);//下注按钮
        dic.Add("btnDouDiZhuViewTip", OnTipClick);//提示按钮
        dic.Add("btnDouDiZhuViewTrustee", OnTrusteeClick);//托管按钮
        dic.Add("btnDouDiZhuCancelTrustee", OnCancelTrusteeClick);//取消托管按钮
        dic.Add("btnDouDiZhuResultViewBack", OnResultBack);//回到大堂
        dic.Add("btnSettleViewShowPokerOpen", OnShowPokerClick);//明牌开始
        dic.Add("btnDouDiZhuViewShowPoker", OnShowPokerClick);//明牌
        //dic.Add("OnSetHandPokerActiveComplete", OnHandPokerActiveComplete);//当发完所有的牌
        //dic.Add("OnSetHandPokerActiveNotComplete", OnHandPokerActiveNotComplete);//当发完所有的牌    
        dic.Add("btnDouDiZhuResultViewShare", OnResultViewShareClick);//分享按钮

        return dic;
    }






    #endregion

    #region Override IGameCtrl
    #region CreateRoom 创建房间
    /// <summary>
    /// 创建房间
    /// </summary>
    public void CreateRoom(int groupId, List<int> settingIds)
    {
        ClientSendCreateRoom(groupId, settingIds);
    }
    #endregion

    #region DisbandRoom 解散房间
    /// <summary>
    /// 解散房间
    /// </summary>
    public void DisbandRoom()
    {
        if (RoomProxy.Instance.CurrentRoom.matchId > 0)
        {
            UIViewManager.Instance.ShowMessage("提示", "比赛场不能解散房间");
        }
        else
        {
            UIViewManager.Instance.ShowMessage("提示", "是否解散房间", MessageViewType.OkAndCancel, ClientSendApplyDisbandRoom);
        }
    }
    #endregion

    #region JoinRoom 进入房间
    /// <summary>
    /// 进入房间
    /// </summary>
    /// <param name="roomId"></param>
    public void JoinRoom(int roomId)
    {
        ClientSendJoinRoom(roomId);
    }
    #endregion

    #region OnReceiveMessage接收聊天消息
    /// <summary>
    /// 接收聊天消息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="playerId"></param>
    /// <param name="message"></param>
    public void OnReceiveMessage(ChatType type, int playerId, string message, string audioName, int toPlayerId)
    {
        SeatEntityBase seat = RoomProxy.Instance.GetSeatByPlayerId(playerId);
        if (type == ChatType.Expression)//表情
        {
            if (!string.IsNullOrEmpty(audioName))
            {
                AudioEffectManager.Instance.Play(string.Format("{0}", audioName), Vector3.zero, false);
            }
            UIItemChat.Instance.ShowExpression(seat.Index, message);
        }
        else if (type == ChatType.InteractiveExpression)//互动表情
        {
            SeatEntityBase toSeat = RoomProxy.Instance.GetSeatByPlayerId(toPlayerId);

            UIItemChat.Instance.ShowInteractiveExpression(seat.Index, message, toSeat.Index, audioName);
        }
        else if (type == ChatType.MicroPhone)//语音
        {
            UIItemChat.Instance.ShowMicroPhone(seat.Index);
        }
        else if (type == ChatType.Message)//文字
        {
            if (!string.IsNullOrEmpty(audioName))
            {
                AudioEffectManager.Instance.Play(string.Format("{0}_{1}", seat.Gender, audioName), Vector3.zero, false);
            }
            UIItemChat.Instance.ShowMessage(seat.Index, message);
        }
    }
    #endregion

    #region GetRoomEntity 获取房间数据实体
    /// <summary>
    /// 获取房间数据实体
    /// </summary>
    /// <returns></returns>
    public RoomEntityBase GetRoomEntity()
    {
        return RoomProxy.Instance.CurrentRoom;
    }
    #endregion

    #region QuitRoom 离开房间
    /// <summary>
    /// 离开房间
    /// </summary>
    public void QuitRoom()
    {
        DDZ_ROOM_LEAVE proto = new DDZ_ROOM_LEAVE();
        proto.playerId = RoomProxy.Instance.CurrentRoom.PlayerSeat.PlayerId;
        NetWorkSocket.Instance.Send(null, DDZ_ROOM_LEAVE.CODE, GameCtrl.Instance.SocketHandle);
        GameCtrl.Instance.ExitGame();
    }
    #endregion

    #region RebuildRoom 重建房间
    /// <summary>
    /// 重建房间
    /// </summary>
    public void RebuildRoom()
    {
        RoomProxy.Instance.Reset();
        ClientSendRebuild();
    }
    #endregion
    #endregion

    #region Override ISystemCtrl
    /// <summary>
    /// 控制器接口
    /// </summary>
    /// <param name="type"></param>
    public void OpenView(UIWindowType type)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region OpenSettleView 打开结算界面
    /// <summary>
    /// 打开结算界面
    /// </summary>
    public void OpenSettleView(RoomEntity room)
    {
        //RoomEntity settleRoom = room;

        //bool isSpring = false;

        //for (int i = 0; i < settleRoom.SeatCount; i++)
        //{
        //    if (settleRoom.SeatList[i].isSpring)
        //    {
        //        isSpring = true;
        //        break;
        //    }
        //}

        //float waitTime = 0;

        //if (isSpring)
        //{
        //    waitTime = 1.5f;
        //}

        //m_UISettleView = UIViewUtil.Instance.LoadWindow("Settle_DouDiZhu").GetComponent<UISettleView>();
        //m_UISettleView.SetUI(settleRoom/*,UISceneDouDZView.Instance.GetTxtLoop()*/);
        //m_UISettleView.StartCoroutine(openSettelView(waitTime));

        //===========================================================================================================
        RoomEntity settleRoom = room;

        bool isSpring = false;

        for (int i = 0; i < settleRoom.SeatCount; i++)
        {
            if (settleRoom.SeatList[i].isSpring)
            {
                isSpring = true;
                break;
            }
        }

        float waitTime = 1;

        if (isSpring)
        {
            waitTime += 2f;
        }
        m_UISettleView = UIViewUtil.Instance.LoadWindow("Settle_DouDiZhu").GetComponent<UISettleView>();
        m_UISettleView.SetUI(settleRoom/*,UISceneDouDZView.Instance.GetTxtLoop()*/);
        m_UISettleView.StartCoroutine(openSettelView(waitTime));
        //m_UISettleView.SafeSetActive(false);
        m_UISettleView.transform.localScale = Vector3.zero;


    }
    #endregion
    private System.Collections.IEnumerator openSettelView(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //m_UISettleView.SafeSetActive(true);
        m_UISettleView.transform.localScale = Vector3.one;
    }

    #region OpenResultView 打开总结算界面
    /// <summary>
    /// 打开总结算界面
    /// </summary>
    public void OpenResultView(RoomEntity room)
    {
        //    RoomEntity resultRoom = room;
        //    bool isSpring = false;

        //    for (int i = 0; i < resultRoom.SeatCount; i++)
        //    {
        //        if (resultRoom.SeatList[i].isSpring)
        //        {
        //            isSpring = true;
        //            break;
        //        }
        //    }

        //    float waitTime = 0;

        //    if (isSpring)
        //    {
        //        waitTime = 1.5f;
        //    }
        //    m_UIResultView = UIViewUtil.Instance.LoadWindow("Result_DouDiZhu").GetComponent<UIResultView_DouDZ>();
        //    m_UIResultView.SetUI(resultRoom);
        //    m_UIResultView.StartCoroutine(openResultView(waitTime));
        //====================================================================

        RoomEntity resultRoom = room;

        bool isSpring = false;

        for (int i = 0; i < resultRoom.SeatCount; i++)
        {
            if (resultRoom.SeatList[i].isSpring)
            {
                isSpring = true;
                break;
            }
        }

        float waitTime = 0;

        if (isSpring)
        {
            waitTime = 1.5f;
        }
        m_UIResultView = UIViewUtil.Instance.LoadWindow("Result_DouDiZhu").GetComponent<UIResultView_DouDZ>();
        m_UIResultView.SetUI(resultRoom);
        m_UIResultView.StartCoroutine(openResultView(waitTime));
        m_UIResultView.SafeSetActive(false);
        if (m_UISettleView != null)
        {
            m_UISettleView.SetResultView(m_UIResultView);
        }
    }
    #endregion
    #region openResultView 延迟打开总结算界面
    /// <summary>
    /// 延迟打开总结算界面
    /// </summary>
    private System.Collections.IEnumerator openResultView(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        m_UIResultView.SafeSetActive(true);
    }
    #endregion


    #region OnReadyClick 准备按钮点击
    /// <summary>
    /// 准备按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnReadyClick(object[] obj)
    {
        ClientSendReady();
    }
    #endregion

    #region OnPokerClick 牌点击
    /// <summary>
    /// 牌点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnPokerClickUp(object[] obj)
    {
        if (RoomProxy.Instance.CurrentRoom == null) return;
        List<UIItemPoker> pokers = obj[0] as List<UIItemPoker>;
        if (pokers == null) return;

        //===============================================



        List<Poker> lstChoicePokers = new List<Poker>();

        for (int i = 0; i < pokers.Count; i++)
        {
            lstChoicePokers.Add(pokers[i].Poker);
        }
        List<Deck> lstDeck = DouDiZhuHelper.GetAllDeck(lstChoicePokers);

        Deck choiceDeck = DouDiZhuHelper.Check(lstChoicePokers);

        Deck rightDect = null;

        if (RoomProxy.Instance.GetPreviourDeck() != null && RoomProxy.Instance.GetPreviourDeck().type != DeckType.A && RoomProxy.Instance.GetPreviourDeck().type != DeckType.AA)
        {
            Debug.LogWarning(RoomProxy.Instance.GetPreviourDeck().type);
            if (choiceDeck != null && (choiceDeck.type == DeckType.SS || choiceDeck.type == DeckType.AAAA))
            {
                rightDect = choiceDeck;
            }
            else
            {
                for (int i = 0; i < lstDeck.Count; i++)
                {
                    if (lstDeck[i] > RoomProxy.Instance.GetPreviourDeck())
                    {
                        rightDect = lstDeck[i];
                        break;
                    }
                }
            }
        }
        else
        {
            List<Poker> lst = new List<Poker>();
            for (int i = 0; i < pokers.Count; i++)
            {
                lst.Add(pokers[i].Poker);
            }

            if (DouDiZhuHelper.GetLongestABCED(lst) != null)
            {
                rightDect = DouDiZhuHelper.GetLongestABCED(lst);
            }
        }

        if (rightDect != null)
        {
            for (int i = pokers.Count - 1; i >= 0; i--)
            {
                bool isExits = false;
                for (int j = 0; j < rightDect.pokers.Count; j++)
                {
                    if (pokers[i].Poker.color == rightDect.pokers[j].color && pokers[i].Poker.size == rightDect.pokers[j].size)
                    {
                        isExits = true;
                        break;
                    }
                }
                if (!isExits)
                {
                    pokers.Remove(pokers[i]);
                }
            }
        }

        //===============================================

        for (int i = 0; i < pokers.Count; ++i)
        {
            bool isExits = false;
            for (int j = 0; j < RoomProxy.Instance.CurrentRoom.PlayerSeat.pokerList.Count; ++j)
            {
                if (RoomProxy.Instance.CurrentRoom.PlayerSeat.pokerList[j].index == pokers[i].Poker.index)
                {
                    isExits = true;
                    break;
                }
            }
            if (!isExits) continue;

            isExits = false;
            for (int j = 0; j < SelectPoker.Count; ++j)
            {
                if (SelectPoker[j].index == pokers[i].Poker.index)
                {
                    isExits = true;
                    SelectPoker.RemoveAt(j);
                    break;
                }
            }
            if (!isExits)
            {
                SelectPoker.Add(pokers[i].Poker);
            }

            pokers[i].isSelect = !pokers[i].isSelect;
        }

        CheckPlayPoker();
    }
    #endregion

    #region OnPlayPokerClick 出牌按钮点击
    /// <summary>
    /// 出牌按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnPlayPokerClick(object[] obj)
    {
        List<Poker> lst = new List<Poker>();
        for (int i = 0; i < SelectPoker.Count; ++i)
        {
            lst.Add(SelectPoker[i]);
        }
        ClientSendPlayPoker(lst);
    }
    #endregion


    private void OnPlayLastPoker(object[] obj)
    {
        List<Poker> lst = new List<Poker>();
        for (int i = 0; i < RoomProxy.Instance.CurrentRoom.PlayerSeat.pokerList.Count; ++i)
        {
            lst.Add(RoomProxy.Instance.CurrentRoom.PlayerSeat.pokerList[i]);
        }
        ClientSendPlayPoker(lst);
    }

    #region OnPassClick 过按钮点击
    /// <summary>
    /// 过按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnPassClick(object[] obj)
    {
        ClientSendPass();
    }
    #endregion

    #region OnBetClick 下注按钮点击
    /// <summary>
    /// 下注按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBetClick(object[] obj)
    {
        int bet = (int)obj[0];
        ClientSendBet(bet);
    }
    #endregion

    #region OnTipClick 提示按钮点击
    /// <summary>
    /// 提示按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnTipClick(object[] obj)
    {
        if (RoomProxy.Instance.CurrentRoom.PlayerSeat == null) return;

        if (Tip == null || Tip.Count == 0)
        {
            Deck prevDeck = RoomProxy.Instance.GetPreviourDeck();
            //if (prevDeck != null && prevDeck.type == DeckType.A)
            //{
            //    Tip = DouDiZhuHelper.GetAStrongerDeck(prevDeck, RoomProxy.Instance.CurrentRoom.PlayerSeat.pokerList);
            //    TipIndex = 0;
            //}
            //else
            //{
            //    Tip = DouDiZhuHelper.GetStrongerDeck(prevDeck, RoomProxy.Instance.CurrentRoom.PlayerSeat.pokerList);
            //    TipIndex = 0;
            //}
            Tip = DouDiZhuHelper.GetStrongerDeck(prevDeck, RoomProxy.Instance.CurrentRoom.PlayerSeat.pokerList);
            TipIndex = 0;

            if (Tip == null || Tip.Count == 0)
            {
                ClientSendPass();
                return;
            }
        }

        SelectPoker.Clear();
        SelectPoker.AddRange(Tip[TipIndex].pokers);
        DRB.DouDiZhu.UIItemSeat.PlayerSeat.SelectPoker(Tip[TipIndex].pokers);

        CheckPlayPoker();

        ++TipIndex;
        if (TipIndex == Tip.Count) TipIndex = 0;
    }
    #endregion

    #region OnTrusteeClick 托管按钮点击
    /// <summary>
    /// 托管按钮点击
    /// </summary>
    private void OnTrusteeClick(object[] obj)
    {
        if (RoomProxy.Instance.CurrentRoom.PlayerSeat == null) return;
        RoomProxy.Instance.SetTrustee(RoomProxy.Instance.CurrentRoom.PlayerSeat.PlayerId, !RoomProxy.Instance.CurrentRoom.PlayerSeat.IsTrustee);
    }
    #endregion

    #region OnCancelTrusteeClick 取消托管按钮点击
    /// <summary>
    /// 托管按钮点击
    /// </summary>
    private void OnCancelTrusteeClick(object[] obj)
    {
        if (RoomProxy.Instance.CurrentRoom.PlayerSeat == null) return;
        RoomProxy.Instance.SetTrustee(RoomProxy.Instance.CurrentRoom.PlayerSeat.PlayerId, false);
    }
    #endregion

    #region OnResultBack 返回大厅
    /// <summary>
    /// 返回大厅
    /// </summary>
    /// <param name="obj"></param>
    private void OnResultBack(object[] obj)
    {
        Debug.LogWarning("???????返回大厅?????????");
        QuitRoom();
    }
    #endregion

    #region OnShowPoker 明牌
    /// <summary>
    /// 明牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnShowPokerClick(object[] obj)
    {
        ClientSendShowPoker();
        //========================明牌测试==================
        //List<Poker> lstPoker = new List<Poker>();
        //for (int i = 0; i < RoomProxy.Instance.CurrentRoom.SeatList[1].pokerList.Count; i++)
        //{
        //    Poker poker = new Poker(RoomProxy.Instance.CurrentRoom.SeatList[1].pokerList[i].index,2,2);
        //    lstPoker.Add(poker);
        //}

        //IGameCommand command = new ShowPokerCommand(RoomProxy.Instance.CurrentRoom.SeatList[1].PlayerId, lstPoker);
        //CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnHandPokerActiveComplete 当发完所有的牌
    /// <summary>
    /// 当发完所有的牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnHandPokerActiveComplete(object[] obj)
    {
        UISceneDouDZView.Instance.SetShowPokerImage(false);
    }
    #endregion

    #region OnHandPokerActiveNotComplete 当没有发完所有的牌
    /// <summary>
    /// 当发完所有的牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnHandPokerActiveNotComplete(object[] obj)
    {
        UISceneDouDZView.Instance.SetShowPokerImage(true);
    }
    #endregion

    #region OnResultViewShareClick 当结算界面按下分享按钮
    /// <summary>
    /// 当结算界面按下分享按钮
    /// </summary>
    /// <param name="obj"></param>
    private void OnResultViewShareClick(object[] obj)
    {
        m_UIResultView.StartCoroutine(ShareCtrl.Instance.ScreenCapture(OnScreenCaptureComplete));
    }
    #endregion

    #region OnScreenCaptureComplete 截屏完成回调
    /// <summary>
    /// 截屏完成回调
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
        AppDebug.Log("分享完了啊");
        ShareCtrl.Instance.ShareTexture(WXShareType.WXSceneSession, path + "record.jpg");
    }
    #endregion

    #region CheckPlayPoker 检测可以出牌
    /// <summary>
    /// 检测可以出牌
    /// </summary>
    public void CheckPlayPoker()
    {
        if (RoomProxy.Instance.CurrentRoom.PlayerSeat.status == SeatEntity.SeatStatus.PlayPoker)
        {
            Deck deck = DouDiZhuHelper.Check(SelectPoker);
            if (deck == null)
            {
                UISceneDouDZView.Instance.SetPlayPoker(false);
                return;
            }
            if (deck.type == DeckType.AAABBBCD) Debug.Log("11111111");
            Deck prevDeck = RoomProxy.Instance.GetPreviourDeck();
            //Debug.LogWarning("prevDeck:__" + prevDeck.type);
            //Debug.LogWarning("prevDeck:__" + prevDeck.mainPoker);
            if (deck <= prevDeck)
            {
                UISceneDouDZView.Instance.SetPlayPoker(false);
                return;
            }
            UISceneDouDZView.Instance.SetPlayPoker(true);
        }
    }
    #endregion

    #region Trustee 托管
    /// <summary>
    /// 托管
    /// </summary>
    public void Trustee()
    {
        bool mustPlay = false;
        Deck prevDeck = RoomProxy.Instance.GetPreviourDeck();
        if (prevDeck == null)
        {
            List<Poker> tempPoker = new List<Poker>();
            tempPoker.Add(RoomProxy.Instance.CurrentRoom.PlayerSeat.pokerList[0]);
            ClientSendPlayPoker(tempPoker);
            return;
        }
        else
        {
            if (prevDeck.type == DeckType.A)
            {
                if (Tip == null || Tip.Count == 0)
                {

                    Tip = DouDiZhuHelper.GetAStrongerDeck(prevDeck,RoomProxy.Instance.CurrentRoom.PlayerSeat.pokerList);

                    if (Tip == null || Tip.Count == 0)
                    {
                        ClientSendPass();
                        return;
                    }
                }

                SelectPoker.Clear();
                SelectPoker.AddRange(Tip[0].pokers);
                DRB.DouDiZhu.UIItemSeat.PlayerSeat.SelectPoker(Tip[0].pokers);

                CheckPlayPoker();

                List<Poker> lst = new List<Poker>();
                for (int i = 0; i < SelectPoker.Count; ++i)
                {
                    lst.Add(SelectPoker[i]);
                }
                ClientSendPlayPoker(lst);
                return;
            }
            
        }
        ClientSendPass();
    }
    #endregion

    #region NormalAITrustee 稍微有脑子的托管
    /// <summary>
    /// 稍微有脑子的托管
    /// </summary>
    public void NormalAITrustee()
    {
        bool mustPlay = false;
        Deck prevDeck = RoomProxy.Instance.GetPreviourDeck();
        if (prevDeck == null)
        {
            mustPlay = true;
        }
        if (mustPlay)
        {
            List<Poker> tempPoker = new List<Poker>();
            tempPoker.Add(RoomProxy.Instance.CurrentRoom.PlayerSeat.pokerList[0]);
            ClientSendPlayPoker(tempPoker);
            return;
        }
        OnTipClick(null);
        OnPlayPokerClick(null);
        //ClientSendPass();
    }
    #endregion




    #region OnServerReturnCreateRoom 服务器广播创建房间
    /// <summary>
    /// 服务器广播创建房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnCreateRoom(byte[] obj)
    {
        DDZ_ROOM_CREATE proto = DDZ_ROOM_CREATE.decode(obj);
        RoomEntity room = new RoomEntity()
        {
            roomId = proto.room.roomId,
            currentLoop = proto.room.loop,
            Times = proto.room.times,
            SeatCount = proto.room.seatListCount(),
            baseScore = proto.room.baseScore,
            gameId = GameCtrl.Instance.CurrentGameId,
            groupId = 0,
            maxLoop = proto.room.maxLoop,
            matchId = 0,
            OwnerID = proto.room.ownerId,
            currentQiangPlayerId = proto.room.currentQiangPlayerId,
            SeatList = new List<SeatEntity>(),
        };
        for (int i = 0; i < proto.room.settingIdCount(); ++i)
        {
            cfg_settingEntity settingEntity = cfg_settingDBModel.Instance.Get(proto.room.getSettingId(i));
            room.Config.Add(settingEntity);
        }
        for (int i = 0; i < proto.room.basePokerListCount(); ++i)
        {
            DDZ_POCKER op_poker = proto.room.getBasePokerList(i);
            room.basePoker.Add(new Poker(op_poker.index, op_poker.color, op_poker.size));
        }
        Debug.Log("房间状态" + proto.room.status);
        if (proto.room.status == ROOM_STATUS.DEAL || proto.room.status == ROOM_STATUS.PLAYPOKER)
        {
            room.roomStatus = RoomEntity.RoomStatus.Gaming;
        }
        else if (proto.room.status == ROOM_STATUS.QIANG)
        {
            room.roomStatus = RoomEntity.RoomStatus.Bet;
        }
        int currentOperatorPos = proto.room.currentPlayPokerPos;
        for (int i = 0; i < proto.room.seatListCount(); ++i)
        {
            DDZ_SEAT op_seat = proto.room.getSeatList(i);
            SeatEntity seat = new SeatEntity();
            seat.PlayerId = op_seat.playerId;//玩家ID
            if (seat.PlayerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
            {
                seat.IsPlayer = true;
            }
            seat.Nickname = op_seat.nickname;
            seat.Avatar = op_seat.avatar;
            seat.Gender = op_seat.gender;
            seat.Gold = op_seat.gold;
            seat.Pos = op_seat.pos;
            seat.bet = op_seat.pour;
            seat.IsBanker = op_seat.isBanker;
            if (op_seat.status == SEAT_STATUS.READY && room.roomStatus == RoomEntity.RoomStatus.Idle)
            {
                seat.status = SeatEntity.SeatStatus.Ready;
            }
            else if (room.roomStatus == RoomEntity.RoomStatus.Gaming)
            {
                seat.status = SeatEntity.SeatStatus.Wait;
            }
            if (currentOperatorPos == seat.Pos)
            {
                if (room.roomStatus == RoomEntity.RoomStatus.Bet)
                {
                    seat.status = SeatEntity.SeatStatus.Bet;
                }
                else if (room.roomStatus == RoomEntity.RoomStatus.Gaming)
                {
                    seat.status = SeatEntity.SeatStatus.PlayPoker;
                }
            }
            Debug.Log(seat.Nickname + "状态是" + seat.status.ToString());
            seat.pokerList = new List<Poker>();
            for (int j = 0; j < op_seat.pokerListCount(); ++j)
            {
                DDZ_POCKER protoPoker = op_seat.getPokerList(j);
                seat.pokerList.Add(new Poker(protoPoker.index, protoPoker.color, protoPoker.size));
            }

            if (op_seat.lastPlayPokerListCount() > 0)
            {
                seat.PreviourPoker = new List<Poker>();
                for (int j = 0; j < op_seat.lastPlayPokerListCount(); ++j)
                {
                    DDZ_POCKER op_poker = op_seat.getLastPlayPokerList(j);
                    Debug.Log(op_poker.size);
                    seat.PreviourPoker.Add(new Poker(op_poker.index, op_poker.color, op_poker.size));
                }
            }
            room.SeatList.Add(seat);
        }

        IGameCommand command = new CreateRoomCommand(room);
        CommandQueue.Enqueue(command);

        if (SceneMgr.Instance.CurrentSceneType != SceneType.DouDZ)
        {
            SceneMgr.Instance.LoadScene(SceneType.DouDZ);
        }
    }
    #endregion

    #region OnServerReturnRebuild 服务器返回重建房间
    /// <summary>
    /// 服务器返回重建房间
    /// </summary>
    /// <param name="bytes"></param>
    private void OnServerReturnRebuild(byte[] bytes)
    {
        DDZ_ROOM_RECREATE proto = DDZ_ROOM_RECREATE.decode(bytes);
        RoomEntity room = new RoomEntity()
        {
            roomId = proto.room.roomId,
            currentLoop = proto.room.loop,
            Times = proto.room.times,
            SeatCount = proto.room.seatListCount(),
            baseScore = proto.room.baseScore,
            gameId = GameCtrl.Instance.CurrentGameId,
            groupId = 0,
            maxLoop = proto.room.maxLoop,
            matchId = 0,
            OwnerID = proto.room.ownerId,
            currentQiangPlayerId = proto.room.currentQiangPlayerId,
            SeatList = new List<SeatEntity>(),
        };
        for (int i = 0; i < proto.room.settingIdCount(); ++i)
        {
            cfg_settingEntity settingEntity = cfg_settingDBModel.Instance.Get(proto.room.getSettingId(i));
            room.Config.Add(settingEntity);
        }
        for (int i = 0; i < proto.room.basePokerListCount(); ++i)
        {
            DDZ_POCKER op_poker = proto.room.getBasePokerList(i);
            room.basePoker.Add(new Poker(op_poker.index, op_poker.color, op_poker.size));
        }
        if (proto.room.status == ROOM_STATUS.DEAL || proto.room.status == ROOM_STATUS.PLAYPOKER)
        {
            room.roomStatus = RoomEntity.RoomStatus.Gaming;

            if (proto.room.status == ROOM_STATUS.DEAL)
            {
                ClientSendDealAnimationComplete();
            }
        }
        else if (proto.room.status == ROOM_STATUS.QIANG)
        {
            room.roomStatus = RoomEntity.RoomStatus.Bet;
        }

        int currentOperatorPos = proto.room.currentPlayPokerPos;

        for (int i = 0; i < proto.room.seatListCount(); ++i)
        {
            DDZ_SEAT op_seat = proto.room.getSeatList(i);
            SeatEntity seat = new SeatEntity();
            seat.PlayerId = op_seat.playerId;//玩家ID
            if (seat.PlayerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
            {
                seat.IsPlayer = true;
            }
            seat.Nickname = op_seat.nickname;
            seat.Avatar = op_seat.avatar;
            seat.Gender = op_seat.gender;
            seat.Gold = op_seat.gold;
            seat.totalScore = op_seat.totalEarnings;
            seat.Pos = op_seat.pos;
            seat.bet = op_seat.pour;
            if (room.baseScore < seat.bet)
            {
                room.baseScore = seat.bet;
            }
            seat.IsBanker = op_seat.isBanker;
            if (op_seat.status == SEAT_STATUS.READY && room.roomStatus == RoomEntity.RoomStatus.Idle)
            {
                seat.status = SeatEntity.SeatStatus.Ready;
            }
            else if (room.roomStatus == RoomEntity.RoomStatus.Gaming)
            {
                seat.status = SeatEntity.SeatStatus.Wait;
            }
            if (currentOperatorPos == seat.Pos)
            {
                if (room.roomStatus == RoomEntity.RoomStatus.Bet)
                {
                    seat.status = SeatEntity.SeatStatus.Bet;
                }
                else if (room.roomStatus == RoomEntity.RoomStatus.Gaming)
                {
                    seat.status = SeatEntity.SeatStatus.PlayPoker;
                    seat.unixtime = (int)(proto.room.unixtime + GlobalInit.Instance.TimeDistance - TimeUtil.GetTimestampMS()) / 1000;
                }
            }
            Debug.Log(seat.Nickname + "状态是" + seat.status.ToString());
            seat.pokerList = new List<Poker>();
            for (int j = 0; j < op_seat.pokerListCount(); ++j)
            {
                DDZ_POCKER protoPoker = op_seat.getPokerList(j);
                seat.pokerList.Add(new Poker(protoPoker.index, protoPoker.color, protoPoker.size));
            }

            if (op_seat.lastPlayPokerListCount() > 0)
            {
                seat.PreviourPoker = new List<Poker>();
                for (int j = 0; j < op_seat.lastPlayPokerListCount(); ++j)
                {
                    DDZ_POCKER op_poker = op_seat.getLastPlayPokerList(j);
                    Debug.Log(op_poker.size);
                    seat.PreviourPoker.Add(new Poker(op_poker.index, op_poker.color, op_poker.size));
                }
            }
            room.SeatList.Add(seat);
        }

        IGameCommand command = new CreateRoomCommand(room);
        CommandQueue.Enqueue(command);

        if (SceneMgr.Instance.CurrentSceneType != SceneType.DouDZ)
        {
            SceneMgr.Instance.LoadScene(SceneType.DouDZ);
        }

        if (room.roomStatus == RoomEntity.RoomStatus.Bet)
        {
            if (room.currentQiangPlayerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
            {
                IGameCommand command2 = new AskBetCommand(room.currentQiangPlayerId);
                CommandQueue.Enqueue(command2);
            }
        }
    }
    #endregion

    #region OnServerBroadcastEnter 服务器广播进入房间
    /// <summary>
    /// 服务器广播进入房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastEnter(byte[] obj)
    {
        DDZ_ROOM_ENTER proto = DDZ_ROOM_ENTER.decode(obj);

        IGameCommand command = new EnterRoomCommand(proto.playerId, proto.gold, proto.avatar, proto.gender, proto.nickname, proto.pos);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastLeave 服务器广播离开房间
    /// <summary>
    /// 服务器广播离开房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastLeave(byte[] obj)
    {
        DDZ_ROOM_LEAVE proto = DDZ_ROOM_LEAVE.decode(obj);

        IGameCommand command = new QuitRoomCommand(proto.playerId);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastReady 服务器广播准备
    /// <summary>
    /// 服务器广播准备
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastReady(byte[] obj)
    {
        DDZ_ROOM_READY proto = DDZ_ROOM_READY.decode(obj);
        IGameCommand command = new ReadyCommand(proto.playerId, true);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastBegin 服务器广播开局
    /// <summary>
    /// 服务器广播开局
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastBegin(byte[] obj)
    {
        DDZ_ROOM_DEAL proto = DDZ_ROOM_DEAL.decode(obj);

        List<SeatEntity> lstSeat = new List<SeatEntity>();
        for (int i = 0; i < proto.seatCount(); ++i)
        {
            List<Poker> lst = new List<Poker>();
            for (int j = 0; j < proto.getSeat(i).pokerListCount(); ++j)
            {
                DDZ_POCKER op_poker = proto.getSeat(i).getPokerList(j);
                lst.Add(new Poker(op_poker.index, op_poker.color, op_poker.size));
            }

            SeatEntity seat = new SeatEntity();
            seat.PlayerId = proto.getSeat(i).playerId;
            seat.pokerList = lst;

            lstSeat.Add(seat);
        }

        IGameCommand command = new BeginCommand(lstSeat, proto.loop);
        CommandQueue.Enqueue(command);

        ClientSendDealAnimationComplete();
    }
    #endregion

    #region OnServerAskBet 服务器询问叫地主
    /// <summary>
    /// 服务器询问叫地主
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastBanker(byte[] obj)
    {
        DDZ_ROOM_INFORMBANKER proto = DDZ_ROOM_INFORMBANKER.decode(obj);

        List<Poker> pokers = new List<Poker>();
        for (int i = 0; i < proto.basePokerListCount(); ++i)
        {
            DDZ_POCKER op_poker = proto.getBasePokerList(i);
            pokers.Add(new Poker(op_poker.index, op_poker.color, op_poker.size));
        }
        IGameCommand command = new BankerCommand(proto.playerId, pokers);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerAskBet2 服务器询问抢地主
    /// <summary>
    /// 服务器询问抢地主
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerAskBet2(byte[] obj)
    {
        DDZ_ROOM_INFORMQIANG proto = DDZ_ROOM_INFORMQIANG.decode(obj);

        IGameCommand command = new AskBetCommand(proto.playerId);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastBet 服务器广播叫地主
    /// <summary>
    /// 服务器广播叫地主
    /// </summary>
    /// <param name="bytes"></param>
    private void OnServerBroadcastBet(byte[] bytes)
    {
        DDZ_ROOM_QIANG proto = DDZ_ROOM_QIANG.decode(bytes);

        IGameCommand command = new BetCommand(proto.playerId, proto.pour);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerAskPlayPoker 服务器询问出牌
    /// <summary>
    /// 服务器询问出牌
    /// </summary>
    /// <param name="bytes"></param>
    private void OnServerAskPlayPoker(byte[] bytes)
    {
        DDZ_ROOM_INFORMPLAYPOKER proto = DDZ_ROOM_INFORMPLAYPOKER.decode(bytes);
       int unixTime= (int)(proto.unixtime + GlobalInit.Instance.TimeDistance - TimeUtil.GetTimestampMS())/1000;
        TipIndex = 0;
        IGameCommand command = new AskPlayPokerCommand(proto.playerId, unixTime);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastPlayPoker 服务器广播出牌
    /// <summary>
    /// 服务器广播出牌
    /// </summary>
    /// <param name="bytes"></param>
    private void OnServerBroadcastPlayPoker(byte[] bytes)
    {
        DDZ_ROOM_PLAYPOKER proto = DDZ_ROOM_PLAYPOKER.decode(bytes);

        List<Poker> lst = new List<Poker>();
        for (int i = 0; i < proto.pokerListCount(); ++i)
        {
            DDZ_POCKER op_poker = proto.getPokerList(i);
            Poker poker = new Poker(op_poker.index, op_poker.color, op_poker.size);
            lst.Add(poker);
        }
        Deck deck = DouDiZhuHelper.Check(lst);
        //Debug.LogWarning(deck.type);
        //Debug.LogWarning(proto.pokerListType);

        //if (deck.type == DeckType.AAABBBCD)
        //{
        //    if (proto.pokerListType != POKERLIST_TYPE.AAABBBCD)
        //    {
        //        Debug.LogError("牌型错误");
        //    }
        //}
        //else if (deck.type == DeckType.AAAABBCC)
        //{
        //    if (proto.pokerListType != POKERLIST_TYPE.AAAABBCC)
        //    {
        //        Debug.LogError("牌型错误");
        //    }
        //}
        //if (deck == null)
        //{
        //    Debug.LogWarning("====================================================");
        //    for (int i = 0; i < lst.Count; i++)
        //    {
        //        Debug.LogWarning(lst[i].ToString());
        //    }
        //}
        //else
        //{
        //    Debug.LogWarning("---------------------------------------------------");
        //    for (int i = 0; i < lst.Count; i++)
        //    {
        //        Debug.LogWarning(lst[i].ToString());
        //    }
        //}
        IGameCommand command = new PlayPokerCommand(proto.playerId, deck);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastPass 服务器广播过
    /// <summary>
    /// 服务器广播过
    /// </summary>
    /// <param name="bytes"></param>
    private void OnServerBroadcastPass(byte[] bytes)
    {
        DDZ_ROOM_PASS proto = DDZ_ROOM_PASS.decode(bytes);

        IGameCommand command = new PassCommand(proto.playerId);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastReDeal 服务器广播重新发牌
    /// <summary>
    /// 服务器广播重新发牌
    /// </summary>
    /// <param name="bytes"></param>
    private void OnServerBroadcastReDeal(byte[] bytes)
    {
        IGameCommand command = new ReDealCommand();
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastSettle 服务器广播结算
    /// <summary>
    /// 服务器广播结算
    /// </summary>
    /// <param name="bytes"></param>
    private void OnServerBroadcastSettle(byte[] bytes)
    {
        DDZ_ROOM_SETTLE proto = DDZ_ROOM_SETTLE.decode(bytes);

        RoomEntity room = new RoomEntity()
        {
            roomId = proto.room.roomId,
            currentLoop = proto.room.loop,
            Times = proto.room.times,
            SeatCount = proto.room.seatListCount(),
            baseScore = proto.room.baseScore,
            gameId = GameCtrl.Instance.CurrentGameId,
            groupId = 0,
            maxLoop = proto.room.maxLoop,
            matchId = 0,
            OwnerID = proto.room.ownerId,
            SeatList = new List<SeatEntity>(),
        };
        for (int i = 0; i < proto.room.getSeatListList().Count; ++i)
        {
            DDZ_SEAT op_seat = proto.room.getSeatList(i);
            SeatEntity seat = new SeatEntity();
            seat.PlayerId = op_seat.playerId;//玩家ID
            if (seat.PlayerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
            {
                seat.IsPlayer = true;
            }
            seat.Nickname = op_seat.nickname;
            seat.Avatar = op_seat.avatar;
            seat.Gender = op_seat.gender;
            seat.Gold = op_seat.gold;
            seat.Pos = op_seat.pos;
            seat.bet = op_seat.pour;
            seat.IsBanker = op_seat.isBanker;
            seat.isWiner = op_seat.isWiner;
            seat.score = op_seat.loopEarnings;
            seat.totalScore = op_seat.totalEarnings;
            seat.isSpring = op_seat.isSpring;
            seat.pokerList = new List<Poker>();
            for (int j = 0; j < op_seat.pokerListCount(); ++j)
            {
                DDZ_POCKER protoPoker = op_seat.getPokerList(j);
                seat.pokerList.Add(new Poker(protoPoker.index, protoPoker.color, protoPoker.size));
            }

            if (op_seat.lastPlayPokerListCount() > 0)
            {
                seat.PreviourPoker = new List<Poker>();
                for (int j = 0; j < op_seat.lastPlayPokerListCount(); ++j)
                {
                    DDZ_POCKER op_poker = op_seat.getLastPlayPokerList(j);
                    Debug.Log(op_poker.size);
                    seat.PreviourPoker.Add(new Poker(op_poker.index, op_poker.color, op_poker.size));
                }
            }
            room.SeatList.Add(seat);
        }

        IGameCommand command = new SettleCommand(room);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastResult 服务器广播结束牌局
    /// <summary>
    /// 服务器广播总结算
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastResult(byte[] bytes)
    {
        DDZ_ROOM_TOTALSETTLE proto = DDZ_ROOM_TOTALSETTLE.decode(bytes);

        RoomEntity room = new RoomEntity()
        {
            roomId = proto.room.roomId,
            currentLoop = proto.room.loop,
            Times = proto.room.times,
            SeatCount = proto.room.seatListCount(),
            baseScore = proto.room.baseScore,
            gameId = GameCtrl.Instance.CurrentGameId,
            groupId = 0,
            maxLoop = proto.room.maxLoop,
            matchId = 0,
            OwnerID = proto.room.ownerId,
            SeatList = new List<SeatEntity>(),
        };

        for (int i = 0; i < proto.room.getSeatListList().Count; ++i)
        {
            DDZ_SEAT op_seat = proto.room.getSeatList(i);
            SeatEntity seat = new SeatEntity();
            seat.PlayerId = op_seat.playerId;//玩家ID
            if (seat.PlayerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
            {
                seat.IsPlayer = true;
            }
            seat.Nickname = op_seat.nickname;
            seat.Avatar = op_seat.avatar;
            seat.Gender = op_seat.gender;
            seat.Gold = op_seat.gold;
            seat.Pos = op_seat.pos;
            seat.bet = op_seat.pour;
            seat.IsBanker = op_seat.isBanker;
            seat.isWiner = op_seat.isWiner;
            seat.score = op_seat.loopEarnings;
            seat.totalScore = op_seat.totalEarnings;
            seat.isSpring = op_seat.isSpring;
            seat.pokerList = new List<Poker>();
            for (int j = 0; j < op_seat.pokerListCount(); ++j)
            {
                DDZ_POCKER protoPoker = op_seat.getPokerList(j);
                seat.pokerList.Add(new Poker(protoPoker.index, protoPoker.color, protoPoker.size));
            }

            if (op_seat.lastPlayPokerListCount() > 0)
            {
                seat.PreviourPoker = new List<Poker>();
                for (int j = 0; j < op_seat.lastPlayPokerListCount(); ++j)
                {
                    DDZ_POCKER op_poker = op_seat.getLastPlayPokerList(j);
                    Debug.Log(op_poker.size);
                    seat.PreviourPoker.Add(new Poker(op_poker.index, op_poker.color, op_poker.size));
                }
            }
            room.SeatList.Add(seat);
        }
        IGameCommand command = new ResultCommand(room);
        CommandQueue.Enqueue(command);

    }
    #endregion

    #region OnServerBroadcastShowPoker 服务器广播明牌
    /// <summary>
    /// 服务器广播明牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastShowPoker(byte[] obj)
    {
        DDZ_ROOM_SHOW_POKER_GAME proto = DDZ_ROOM_SHOW_POKER_GAME.decode(obj);
        List<Poker> lstPoker = new List<Poker>();
        for (int j = 0; j < proto.seat.pokerListCount(); ++j)
        {
            DDZ_POCKER protoPoker = proto.seat.getPokerList(j);
            lstPoker.Add(new Poker(protoPoker.index, protoPoker.color, protoPoker.size));
        }
        IGameCommand command = new ShowPokerCommand(proto.seat.playerId, lstPoker);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastDisbandRoom 服务器广播解散房间
    /// <summary>
    /// 服务器广播解散房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastDisbandRoom(byte[] obj)
    {
        DDZ_ROOM_APPLYDISMISS proto = DDZ_ROOM_APPLYDISMISS.decode(obj);

        if (proto.hasIsSucceed())
        {
            if (proto.isSucceed)
            {
                Debug.Log("哈哈哈哈哈哈哈哈哈");
                UIViewManager.Instance.ShowMessage("提示", "房间已解散", MessageViewType.Ok, SeeResult);
            }
            else
            {
                SeatEntity seat = RoomProxy.Instance.GetSeatByPlayerId(proto.playerId);
                string nickName = "有人";
                if (seat != null)
                {
                    nickName = seat.Nickname;
                }
                UIViewManager.Instance.ShowMessage("提示", string.Format("{0}不同意解散房间", nickName), MessageViewType.Ok);
            }
        }
        else
        {
            SeatEntity seat = RoomProxy.Instance.GetSeatByPlayerId(proto.playerId);
            string nickName = "有人";
            if (seat != null)
            {
                nickName = seat.Nickname;
            }
            UIViewManager.Instance.ShowMessage("提示", string.Format("{0}发起解散房间，是否同意", nickName), MessageViewType.OkAndCancel, ClientSendAgreeDisbandRoom, ClientSendRefuseDisbandRoom, 10f, AutoClickType.Cancel);
        }
    }
    #endregion



    #region SeeResult 查看牌局总结算信息
    /// <summary>
    /// 查看牌局总结算信息
    /// </summary>
    private void SeeResult()
    {
        //GameCtrl.Instance.ExitGame();
        if (m_UIResultView == null)
        {
            GameCtrl.Instance.ExitGame();
            return;
        }
        else
        {
            m_UIResultView.SafeSetActive(true);
        }
        //OpenResultView();
    }
    #endregion

    #region ClientSendAgreeDisbandRoom 客户端发送同意解散房间
    /// <summary>
    /// 客户端发送同意解散房间
    /// </summary>
    private void ClientSendAgreeDisbandRoom()
    {
        DDZ_ROOM_REPLYDISMISS_GET proto = new DDZ_ROOM_REPLYDISMISS_GET();
        proto.isDismiss = true;
        NetWorkSocket.Instance.Send(proto.encode(), DDZ_ROOM_REPLYDISMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendRefuseDisbandRoom 客户端发送拒绝解散房间
    /// <summary>
    /// 客户端发送拒绝解散房间
    /// </summary>
    private void ClientSendRefuseDisbandRoom()
    {
        DDZ_ROOM_REPLYDISMISS_GET proto = new DDZ_ROOM_REPLYDISMISS_GET();
        proto.isDismiss = false;
        NetWorkSocket.Instance.Send(proto.encode(), DDZ_ROOM_REPLYDISMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendCreateRoom 客户端发送创建房间消息
    /// <summary> 
    /// 客户端发送创建房间消息
    /// </summary>
    private void ClientSendCreateRoom(int groupId, List<int> settingIds)
    {
        DDZ_ROOM_CREATE_GET proto = new DDZ_ROOM_CREATE_GET();
        for (int i = 0; i < settingIds.Count; ++i)
        {
            proto.addSettingId(settingIds[i]);
        }
        proto.clubId = groupId;
        NetWorkSocket.Instance.Send(proto.encode(), DDZ_ROOM_CREATE.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendJoinRoom 客户端发送加入房间
    /// <summary>
    /// 客户端发送加入房间
    /// </summary>
    private void ClientSendJoinRoom(int roomId)
    {
        DDZ_ROOM_ENTER_GET proto = new DDZ_ROOM_ENTER_GET();
        proto.roomId = roomId;
        NetWorkSocket.Instance.Send(proto.encode(), DDZ_ROOM_ENTER.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendRebuild 客户端发送重建房间
    /// <summary>
    /// 客户端发送重建房间
    /// </summary>
    private void ClientSendRebuild()
    {
        NetWorkSocket.Instance.Send(null, DDZ_ROOM_RECREATE_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendReady 客户端发送准备
    /// <summary>
    /// 客户端发送准备
    /// </summary>
    private void ClientSendReady()
    {
        NetWorkSocket.Instance.Send(null, DDZ_ROOM_READY.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendApplyDisbandRoom 客户端发送请求解散房间
    /// <summary>
    /// 客户端发送请求解散房间
    /// </summary>
    private void ClientSendApplyDisbandRoom()
    {
        //NetWorkSocket.Instance.Send(null, DDZ_ROOM_DISMISS_SUCCEED_GET.CODE, GameCtrl.Instance.SocketHandle);
        NetWorkSocket.Instance.Send(null, DDZ_ROOM_APPLYDISMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendBet 客户端发送下注
    /// <summary>
    /// 客户端发送下注
    /// </summary>
    /// <param name="bet"></param>
    private void ClientSendBet(int bet)
    {
        DDZ_ROOM_QIANG_GET proto = new DDZ_ROOM_QIANG_GET();
        proto.pour = bet;
        NetWorkSocket.Instance.Send(proto.encode(), DDZ_ROOM_QIANG_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendPlayPoker 客户端发送出牌消息
    /// <summary>
    /// 客户端发送出牌消息
    /// </summary>
    /// <param name="pokers"></param>
    private void ClientSendPlayPoker(List<Poker> pokers)
    {
        DDZ_ROOM_PLAYPOKER_GET proto = new DDZ_ROOM_PLAYPOKER_GET();
        for (int i = 0; i < pokers.Count; ++i)
        {
            Poker poker = pokers[i];
            DDZ_POCKER op_poker = new DDZ_POCKER();
            op_poker.index = poker.index;
            op_poker.color = poker.color;
            op_poker.size = poker.size;
            proto.addPokerList(op_poker);
        }
        Deck deck = DouDiZhuHelper.Check(pokers);
        if (deck.type == DeckType.AAABBBCD)
        {
            proto.pokerListType = POKERLIST_TYPE.AAABBBCD;
        }
        else if (deck.type == DeckType.AAAABBCC)
        {
            proto.pokerListType = POKERLIST_TYPE.AAAABBCC;
        }
        NetWorkSocket.Instance.Send(proto.encode(), DDZ_ROOM_PLAYPOKER_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendPass 客户端发送过
    /// <summary>
    /// 客户端发送过
    /// </summary>
    public void ClientSendPass()
    {
        NetWorkSocket.Instance.Send(null, DDZ_ROOM_PASS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendDealAnimationComplete 客户端发送发牌动画结束
    /// <summary>
    /// 客户端发送发牌动画结束
    /// </summary>
    private void ClientSendDealAnimationComplete()
    {
        NetWorkSocket.Instance.Send(null, DDZ_ROOM_DEALDONE_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendShowPoker 客户端发送明牌
    /// <summary>
    /// 客户端发送明牌
    /// </summary>
    public void ClientSendShowPoker()
    {
        NetWorkSocket.Instance.Send(null, DDZ_ROOM_SHOWPOKER.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion
}
