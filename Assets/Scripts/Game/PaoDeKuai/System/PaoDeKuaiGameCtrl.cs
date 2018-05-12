//===================================================
//Author      : WZQ
//CreateTime  ：11/13/2017 3:51:48 PM
//Description ：跑得快
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.pdk;
using PaoDeKuai;
public class PaoDeKuaiGameCtrl : SystemCtrlBase<PaoDeKuaiGameCtrl>, IGameCtrl, ISystemCtrl
{
   
       #region Variable
        private UIPaoDeKuaiSettleView m_UISettleView;//小结算

        private UIPaoDeKuaiResult m_UIResultView;//总结算

       private UISeatInfoView m_UISeatInfoView;

      private UIDisbandView m_UIDisbandView;//解散界面
    /// <summary>
    /// 当前进入房间类型
    /// </summary>
    private EnterRoomType m_CurrentType = EnterRoomType.None;
    //    /// <summary>
    //    /// 当前加入的房间Id
    //    /// </summary>
    //    private int m_nCurrentJoinRoomId;
    /// <summary>
    /// 游戏结果
    /// </summary>
    private PDK_GAME_OVER m_Result;


    public Queue<IGameCommand> CommandQueue = new Queue<IGameCommand>();//命令队列
        #endregion

    #region DicNotificationInterests 注册UI事件
    /// <summary>
    /// 注册UI事件
    /// </summary>
    /// <returns></returns>
    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        dic.Add(ConstDefine_PaoDeKuai.BtnPDKSettleViewReady, OnBtnSettleViewReadyClick);//结算界面准备按钮
        dic.Add(ConstDefine_PaoDeKuai.BtnPDKSettleViewResult, OnBtnSettleViewResultClick);//结算界面查看结果按钮
        dic.Add(ConstDefine_PaoDeKuai.BtnPDKResultViewBack, OnBtnResultViewBack);//结束界面返回按钮
        dic.Add(ConstDefine_PaoDeKuai.BtnPDKResultViewShare, OnBtnResultViewShareClick);//结束界面分享按钮
        //dic.Add("btnSettleViewShare", OnBtnResultViewShareClick);//结算界面分享按钮
        //dic.Add(ConstDefine.BtnSettleViewReplayOver, OnBtnResultViewReplayOverClick);
        dic.Add(ConstDefine.BtnMaJiangViewAuto, OnBtnPDKViewAutoClick);//托管按钮点击
        //dic.Add(ConstDefine.BtnMaJiangViewCancelAuto, OnBtnMaJiangViewCancelAutoClick);//取消托管按钮点击
        dic.Add(ConstDefine_PaoDeKuai.BtnPDKViewShare , OnBtnPDKViewShareClick);//分享按钮点击
        dic.Add(ConstDefine_PaoDeKuai.BtnPDKViewReady, OnBtnPDKViewReadyClick);//准备按钮点击
        //dic.Add(ConstDefine.BtnMaJiangViewCancelReady, OnBtnMaJiangViewCancelReadyClick);//取消准备按钮点击
      


        dic.Add(ConstDefine_PaoDeKuai.BtnPDKViewBuQiang, OnBtnPDKViewBuQiangGuanClick);//不抢关按钮点击
        dic.Add(ConstDefine_PaoDeKuai.BtnPDKViewQiang, OnBtnPDKViewQianGuanClick);//抢关按钮点击
        dic.Add(ConstDefine_PaoDeKuai.BtnPDKViewBuChu, OnBtnPDKViewBuChuClick);//不出牌按钮点击
        dic.Add(ConstDefine_PaoDeKuai.BtnPDKViewTiShi, OnBtnPDKViewTiShiClick);//提示按钮点击
        dic.Add(ConstDefine_PaoDeKuai.BtnPDKViewChuPai, OnBtnPDKViewChuPaiClick);//出牌按钮点击
        dic.Add(ConstDefine_PaoDeKuai.BtnPDKViewJiPaiQi, OnBtnPDKViewJiPaiQiClick);//记牌器按钮点击

        dic.Add(ConstDefine_PaoDeKuai.BtnPDKViewHeadClick, OnHeadClick);//头像按钮点击
        //dic.Add("OnBtnChangeSeatClick", OnBtnChangeSeatClick);//换座位按钮点击
        dic.Add("btnDisbandViewAgree", OnBtnDisbandViewAgree);//解散房间界面同意按钮
        dic.Add("btnDisbandViewRefuse", OnBtnDisbandViewRefuse);//解散房间界面拒绝按钮
        return dic;

}
    #endregion

    #region Constructors
    public PaoDeKuaiGameCtrl()
    {

        NetDispatcher.Instance.AddEventListener(PDK_CREATE_ROOM.CODE, OnServerReturnCreateRoom);//服务器返回创建麻将房间消息
        NetDispatcher.Instance.AddEventListener(PDK_RECREATE.CODE, OnServerReturnRebuild);//服务器返回服务器返回重建房间消息
        NetDispatcher.Instance.AddEventListener(PDK_ENTER_ROOM.CODE, OnServerBroadcastEnter);//服务器广播玩家进入消息
        NetDispatcher.Instance.AddEventListener(PDK_LEAVE.CODE, OnServerBroadcastLeave);//服务器广播玩家离开消息
        NetDispatcher.Instance.AddEventListener(PDK_READY.CODE, OnServerBroadcastReady);//服务器广播玩家准备消息
        //NetDispatcher.Instance.AddEventListener(OP_ROOM_UNREADY.CODE, OnServerBroadcastCancelReady);//服务器广播玩家取消准备消息
        //NetDispatcher.Instance.AddEventListener(OP_ROOM_TRUSTEE.CODE, OnServerBroadcastTrustee);//服务器广播玩家托管消息
        //NetDispatcher.Instance.AddEventListener(OP_MATCH_WAIVER.CODE, OnServerBroadcastWaiver);//服务器广播玩家弃权消息
        
        NetDispatcher.Instance.AddEventListener(PDK_APPLY_DISMISS.CODE, OnServerBroadcastApplyDisband);//服务器广播解散房间
        NetDispatcher.Instance.AddEventListener(PDK_ANSWER_DIMISS.CODE, OnServerBroadcastAnswerDimiss); //服务器广播答复解散房间
        NetDispatcher.Instance.AddEventListener(PDK_DIMISS_SUCCEED.CODE, OnServerBroadcastDimissSucceed); //服务器广播解散房间成功
        NetDispatcher.Instance.AddEventListener(PDK_DIMISS_DEFEAT.CODE, OnServerBroadcastDimissDefeat); //服务器广播解散房间失败
        //NetDispatcher.Instance.AddEventListener(OP_ROOM_RECREATE.CODE, OnServerReturnRebuild); //服务器返回重建房间
        NetDispatcher.Instance.AddEventListener(PDK_BEGIN.CODE, OnServerBroadcastBegin);//服务器广播开局消息
        //NetDispatcher.Instance.AddEventListener(PDK_OPERATE.CODE, OnServerBroadcastDrawPoker);//服务器广播摸牌消息
        NetDispatcher.Instance.AddEventListener(PDK_OPERATE.CODE, OnServerBroadcastPlayPoker);//服务器广播出牌消息
        NetDispatcher.Instance.AddEventListener(PDK_NEXT_PLAYER.CODE, OnServerBroadcastSeatOperate);//服务器通知座位操作
        NetDispatcher.Instance.AddEventListener(PDK_PASS.CODE, OnServerBroadcastPass);//服务器广播过
        NetDispatcher.Instance.AddEventListener(PDK_BOMB.CODE, OnServerBroadcastBombGoldChange);//服务器通知炸弹加分 
        NetDispatcher.Instance.AddEventListener(PDK_GAME_OVER.CODE, OnServerBroadcastSettle);//服务器通知本局结算

    }
    #endregion

    #region Dispose
    public override void Dispose()
    {
        base.Dispose();
        //NetDispatcher.Instance.RemoveEventListener(OP_ROOM_RESULT.CODE, OnServerReturnResult);//服务器返回结果消息
        NetDispatcher.Instance.RemoveEventListener(PDK_CREATE_ROOM.CODE, OnServerReturnCreateRoom);//服务器返回创建麻将房间消息
        NetDispatcher.Instance.RemoveEventListener(PDK_RECREATE.CODE, OnServerReturnRebuild);//服务器返回服务器返回重建房间消息
        NetDispatcher.Instance.RemoveEventListener(PDK_ENTER_ROOM.CODE, OnServerBroadcastEnter);//服务器广播玩家进入消息
        NetDispatcher.Instance.RemoveEventListener(PDK_LEAVE.CODE, OnServerBroadcastLeave);//服务器广播玩家离开消息
        NetDispatcher.Instance.RemoveEventListener(PDK_READY.CODE, OnServerBroadcastReady);//服务器广播玩家准备消息
        //NetDispatcher.Instance.RemoveEventListener(OP_ROOM_UNREADY.CODE, OnServerBroadcastCancelReady);//服务器广播玩家取消准备消息
        //NetDispatcher.Instance.RemoveEventListener(OP_ROOM_TRUSTEE.CODE, OnServerBroadcastTrustee);//服务器广播玩家托管消息
        //NetDispatcher.Instance.RemoveEventListener(OP_MATCH_WAIVER.CODE, OnServerBroadcastWaiver);//服务器广播玩家弃权消息


        NetDispatcher.Instance.RemoveEventListener(PDK_APPLY_DISMISS.CODE, OnServerBroadcastApplyDisband);//服务器广播解散房间(新版)
         NetDispatcher.Instance.RemoveEventListener(PDK_ANSWER_DIMISS.CODE, OnServerBroadcastAnswerDimiss); //服务器广播答复解散房间
        NetDispatcher.Instance.RemoveEventListener(PDK_DIMISS_SUCCEED.CODE, OnServerBroadcastDimissSucceed); //服务器广播解散房间成功
        NetDispatcher.Instance.RemoveEventListener(PDK_DIMISS_DEFEAT.CODE, OnServerBroadcastDimissDefeat); //服务器广播解散房间失败

        //NetDispatcher.Instance.RemoveEventListener(OP_ROOM_RECREATE.CODE, OnServerReturnRebuild); //服务器返回重建房间
        NetDispatcher.Instance.RemoveEventListener(PDK_BEGIN.CODE, OnServerBroadcastBegin);//服务器广播开局消息
        //NetDispatcher.Instance.RemoveEventListener(OP_ROOM_GET_POKER.CODE, OnServerBroadcastDrawPoker);//服务器广播摸牌消息
        NetDispatcher.Instance.RemoveEventListener(PDK_OPERATE.CODE, OnServerBroadcastPlayPoker);//服务器广播出牌消息
        NetDispatcher.Instance.RemoveEventListener(PDK_NEXT_PLAYER.CODE, OnServerBroadcastSeatOperate);//服务器通知座位操作
        NetDispatcher.Instance.RemoveEventListener(PDK_BOMB.CODE, OnServerBroadcastBombGoldChange);//服务器通知炸弹加分  
        NetDispatcher.Instance.RemoveEventListener(PDK_GAME_OVER.CODE, OnServerBroadcastSettle);//服务器通知本局结算
        NetDispatcher.Instance.RemoveEventListener(PDK_PASS.CODE, OnServerBroadcastPass);//服务器广播过
    }
    #endregion

    #region Override ISystemCtrl
    #region OpenView 打开窗口
    /// <summary>
    /// 打开窗口
    /// </summary>
    /// <param name="type"></param>
    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.Settle_PaoDeKuai:
                OpenSettleView();
                break;
            case UIWindowType.Result_PaoDeKuai:
                OpenResultView();
                break;
            case UIWindowType.JiPaiQi:
                OpenJiPaiQi();
                break;
        }
    }
    #endregion

    #region OpenSettleView 打开结算界面
    /// <summary>
    /// 打开结算界面
    /// </summary>
    private void OpenSettleView()
    {
        if (m_UIResultView != null) return;
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Settle_PaoDeKuai, (GameObject go) =>
        {
            m_UISettleView = go.GetComponent<UIPaoDeKuaiSettleView>();
            bool isLastLoop = RoomPaoDeKuaiProxy.Instance.CurrentRoom.currentLoop == RoomPaoDeKuaiProxy.Instance.CurrentRoom.maxLoop;
            m_UISettleView.SetUI(RoomPaoDeKuaiProxy.Instance.CurrentRoom.SeatList, RoomPaoDeKuaiProxy.Instance.PlayerSeat.Pos, RoomPaoDeKuaiProxy.Instance.CurrentRoom.WinnertPos, isLastLoop);
        });
    }
    #endregion

    #region OpenResultView 打开结束界面
    /// <summary>
    /// 打开结束界面
    /// </summary>
    private void OpenResultView()
    {
        if (m_UISettleView != null) m_UISettleView.Close();
        if (m_Result == null) return;
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Result_PaoDeKuai, (GameObject go) =>
        {
            m_UIResultView = go.GetComponent<UIPaoDeKuaiResult>();
            m_UIResultView.SetUI(RoomPaoDeKuaiProxy.Instance.CurrentRoom);
            m_Result = null;
        });
    }
    #endregion

    private void OpenJiPaiQi()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.JiPaiQi, (GameObject go) =>
        {
            UIPDKJiPaiQi jipaiqi = go.GetComponent<UIPDKJiPaiQi>();
            jipaiqi.SetUI(RoomPaoDeKuaiProxy.Instance.CurrentRoom.HistoryPoker);
            
        });

    }




    #region OpenDisbandView 打开解散界面
    /// <summary>
    /// 打开解散界面
    /// </summary>
    public void OpenDisbandView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Disband, (GameObject go) =>
        {
            m_UIDisbandView = go.GetComponent<UIDisbandView>();
            Debug.Log(RoomPaoDeKuaiProxy.Instance.CurrentRoom.DisbandTime);
            Debug.Log(TimeUtil.GetTimestampMS());
            Debug.Log(GlobalInit.Instance.TimeDistance);
            m_UIDisbandView.SetUI(RoomPaoDeKuaiProxy.Instance.CurrentRoom.SeatList, RoomPaoDeKuaiProxy.Instance.PlayerSeat, (RoomPaoDeKuaiProxy.Instance.CurrentRoom.DisbandTime - TimeUtil.GetTimestampMS() + GlobalInit.Instance.TimeDistance) / 1000, RoomPaoDeKuaiProxy.Instance.CurrentRoom.DisbandTimeMax / 1000);
        });
    }
    #endregion
    #endregion

    #region Override IGameCtrl
    #region CreateRoom 创建房间
    /// <summary>
    /// 创建房间
    /// </summary>
    public void CreateRoom(int groupId, List<int> settingIds)
    {
        ClientSendCreateRoom(groupId,  settingIds);
    }
    #endregion

    #region JoinRoom 加入房间
    /// <summary>
    /// 加入房间
    /// </summary>
    /// <param name="roomId"></param>
    public void JoinRoom(int roomId)
    {
        ClientSendJoinRoom(roomId);
    }
    #endregion

    #region RebuildRoom 重建房间
    /// <summary>
    /// 重建房间
    /// </summary>
    public void RebuildRoom()
    {
        RoomPaoDeKuaiProxy.Instance.ClearRoom();
        ClientSendRebuild();
    }
    #endregion

    #region QuitRoom 退出房间
    /// <summary>
    /// 退出房间
    /// </summary>
    public void QuitRoom()
    {
        //if (RoomMaJiangProxy.Instance.CurrentRoom.matchId > 0)
        //{
        //    ShowMessage("提示", "是否退赛", MessageViewType.OkAndCancel, ClientSendLeaveRoom);
        //    return;
        //}
        //if ((RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Ready && (RoomMaJiangProxy.Instance.CurrentRoom.currentLoop == 1 || RoomMaJiangProxy.Instance.CurrentRoom.currentLoop == 0)) || RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Replay)
        //{
        //    ClientSendLeaveRoom();
        //}
        //else
        //{
        //    DisbandRoom();
        //}
    }
    #endregion

    #region DisbandRoom 解散房间
    /// <summary>
    /// 解散房间
    /// </summary>
    public void DisbandRoom()
    {
        if (RoomPaoDeKuaiProxy.Instance.CurrentRoom.matchId > 0)
        {
            UIViewManager.Instance.ShowMessage("提示", "比赛场不能解散房间");
        }
        else
        {
            UIViewManager.Instance.ShowMessage("提示", "是否解散房间", MessageViewType.OkAndCancel, ClientSendApplyDisbandRoom);
        }
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
        SeatEntity seat = RoomPaoDeKuaiProxy.Instance.GetSeatByPlayerId(playerId);
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
            SeatEntity toSeat = RoomPaoDeKuaiProxy.Instance.GetSeatByPlayerId(toPlayerId);

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
        return RoomPaoDeKuaiProxy.Instance.CurrentRoom;
    }
    #endregion
    #endregion

    #region SeeResult 查看牌局总结算信息
    /// <summary>
    /// 查看牌局总结算信息
    /// </summary>
    private void SeeResult()
    {
        if (m_Result == null)
        {
            ExitGame();
            return;
        }
        OpenResultView();
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

    #region ExitGame 退出本局游戏
    /// <summary>
    /// 退出本局游戏
    /// </summary>
    private void ExitGame()
    {
        GameCtrl.Instance.ExitGame();
    }
    #endregion



    //    #region 按钮点击
    //    #region OnPokerClick 手牌点击
    //    /// <summary>
    //    /// 手牌点击
    //    /// </summary>
    //    /// <param name="param"></param>
    //    public void OnPokerClick(MaJiangCtrl handPoker)
    //    {
    //        if (IsLiangXi)
    //        {
    //            if (m_CurrentSelect != null)
    //            {
    //                handPoker.transform.localPosition = new Vector3(handPoker.transform.localPosition.x, handPoker.transform.localPosition.y, 0f);
    //                m_CurrentSelect = null;
    //            }

    //            if (m_LiangXiSelect.Contains(handPoker))
    //            {
    //                m_LiangXiPoker.Remove(handPoker.Poker);
    //                m_LiangXiSelect.Remove(handPoker);
    //                handPoker.transform.localPosition = new Vector3(handPoker.transform.localPosition.x, handPoker.transform.localPosition.y, 0f);
    //            }
    //            else
    //            {
    //                for (int i = 0; i < RoomMaJiangProxy.Instance.AllCanLiangXi.Count; ++i)
    //                {
    //                    if (MahJongHelper.ContainPoker(handPoker.Poker, RoomMaJiangProxy.Instance.AllCanLiangXi[i]))
    //                    {
    //                        m_LiangXiPoker.Add(handPoker.Poker);
    //                        m_LiangXiSelect.Add(handPoker);
    //                        handPoker.transform.localPosition = new Vector3(handPoker.transform.localPosition.x, handPoker.transform.localPosition.y, 2f);
    //                        break;
    //                    }
    //                }

    //            }
    //            UIItemOperator.Instance.ShowOK(MahJongHelper.CheckLiangXi(m_LiangXiPoker));
    //            return;
    //        }

    //        if (MahJongHelper.ContainPoker(handPoker.Poker, RoomMaJiangProxy.Instance.PlayerSeat.DingJiangPoker))
    //        {
    //            return;
    //        }


    //        if (m_CurrentSelect == null)
    //        {
    //            m_CurrentSelect = handPoker;
    //            handPoker.transform.localPosition = new Vector3(handPoker.transform.localPosition.x, handPoker.transform.localPosition.y, 2f);
    //            if (!RoomMaJiangProxy.Instance.PlayerSeat.IsTing)
    //            {
    //                UIItemTingTip.Instance.Show(m_CurrentSelect, RoomMaJiangProxy.Instance.GetHu(m_CurrentSelect.Poker));
    //            }
    //        }
    //        else
    //        {
    //            if (m_CurrentSelect == handPoker)
    //            {
    //                if (m_CurrentSelect != null)
    //                {
    //                    m_CurrentSelect.transform.localPosition = new Vector3(m_CurrentSelect.transform.localPosition.x, m_CurrentSelect.transform.localPosition.y, 0f);
    //                    m_CurrentSelect = null;
    //                }
    //#if !IS_LUALU
    //                ClientSendPlayPoker(handPoker.Poker);
    //#endif
    //            }
    //            else
    //            {
    //                m_CurrentSelect.transform.localPosition = new Vector3(m_CurrentSelect.transform.localPosition.x, m_CurrentSelect.transform.localPosition.y, 0f);
    //                m_CurrentSelect = handPoker;
    //                handPoker.transform.localPosition = new Vector3(handPoker.transform.localPosition.x, handPoker.transform.localPosition.y, 2f);
    //                if (!RoomMaJiangProxy.Instance.PlayerSeat.IsTing)
    //                {
    //                    UIItemTingTip.Instance.Show(m_CurrentSelect, RoomMaJiangProxy.Instance.GetHu(m_CurrentSelect.Poker));
    //                }
    //            }
    //        }
    //    }
    //    #endregion

    //    #region OnOperatorClick 操作按钮点击
    //    /// <summary>
    //    /// 操作按钮点击
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private void OnOperatorClick(object[] obj)
    //    {
    //        OperatorType type = (OperatorType)obj[0];

    //        Debug.Log("点击了按钮" + type.ToString());

    //        if (type == OperatorType.Ting)
    //        {
    //            IsTingByPlayPoker = true;
    //            return;
    //        }
    //        else if (type == OperatorType.Cancel)
    //        {
    //            IsTingByPlayPoker = false;
    //            IsLiangXi = false;
    //            for (int i = 0; i < m_LiangXiSelect.Count; ++i)
    //            {
    //                m_LiangXiSelect[i].transform.localPosition = new Vector3(m_LiangXiSelect[i].transform.localPosition.x, m_LiangXiSelect[i].transform.localPosition.y, 0f);
    //            }
    //            m_LiangXiSelect.Clear();
    //            m_LiangXiPoker.Clear();

    //            List<MaJiangCtrl> lstHand = MahJongManager.Instance.GetHand(RoomMaJiangProxy.Instance.PlayerSeat.Pos);
    //            for (int i = 0; i < lstHand.Count; ++i)
    //            {
    //                lstHand[i].isGray = false;
    //            }
    //            return;
    //        }
    //        else if (type == OperatorType.Pass)
    //        {
    //            IsTingByPlayPoker = false;
    //            IsLiangXi = false;
    //            for (int i = 0; i < m_LiangXiSelect.Count; ++i)
    //            {
    //                m_LiangXiSelect[i].transform.localPosition = new Vector3(m_LiangXiSelect[i].transform.localPosition.x, m_LiangXiSelect[i].transform.localPosition.y, 0f);
    //            }
    //            m_LiangXiSelect.Clear();
    //            m_LiangXiPoker.Clear();

    //            List<MaJiangCtrl> lstHand = MahJongManager.Instance.GetHand(RoomMaJiangProxy.Instance.PlayerSeat.Pos);
    //            for (int i = 0; i < lstHand.Count; ++i)
    //            {
    //                lstHand[i].isGray = false;
    //            }
    //        }
    //        else if (type == OperatorType.LiangXi)
    //        {
    //#if !IS_LUALU
    //            if (m_CurrentSelect != null)
    //            {
    //                m_CurrentSelect.transform.localPosition = new Vector3(m_CurrentSelect.transform.localPosition.x, m_CurrentSelect.transform.localPosition.y, 0f);
    //                m_CurrentSelect = null;
    //            }
    //            if (RoomMaJiangProxy.Instance.AllCanLiangXi.Count == 3)
    //            {
    //                List<Poker> pokers = new List<Poker>();
    //                for (int i = 0; i < RoomMaJiangProxy.Instance.AllCanLiangXi.Count; ++i)
    //                {
    //                    pokers.Add(RoomMaJiangProxy.Instance.AllCanLiangXi[i][0]);
    //                }
    //                ClientSendLiangXi(pokers);
    //            }
    //            else
    //            {
    //                IsLiangXi = true;

    //                List<MaJiangCtrl> lstHand = MahJongManager.Instance.GetHand(RoomMaJiangProxy.Instance.PlayerSeat.Pos);

    //                List<MaJiangCtrl> lstLiangXi = new List<MaJiangCtrl>();

    //                for (int i = 0; i < lstHand.Count; ++i)
    //                {
    //                    for (int j = 0; j < RoomMaJiangProxy.Instance.AllCanLiangXi.Count; ++j)
    //                    {
    //                        if (MahJongHelper.ContainPoker(lstHand[i].Poker, RoomMaJiangProxy.Instance.AllCanLiangXi[j]))
    //                        {
    //                            bool isExists = false;
    //                            for (int k = 0; k < lstLiangXi.Count; ++k)
    //                            {
    //                                if (lstLiangXi[k].Poker.index == lstHand[i].Poker.index)
    //                                {
    //                                    isExists = true;
    //                                    break;
    //                                }
    //                            }
    //                            if (!isExists)
    //                            {
    //                                lstLiangXi.Add(lstHand[i]);
    //                            }
    //                        }
    //                    }
    //                }

    //                for (int i = 0; i < lstHand.Count; ++i)
    //                {
    //                    bool isExists = false;
    //                    for (int j = 0; j < lstLiangXi.Count; ++j)
    //                    {
    //                        if (lstLiangXi[j].Poker.index == lstHand[i].Poker.index)
    //                        {
    //                            isExists = true;
    //                            break;
    //                        }
    //                    }
    //                    if (!isExists)
    //                    {
    //                        lstHand[i].isGray = true;
    //                    }
    //                }
    //            }
    //            return;
    //#endif
    //        }
    //        else if (type == OperatorType.Ok)
    //        {
    //            if (IsLiangXi)
    //            {
    //                IsLiangXi = false;
    //                for (int i = 0; i < m_LiangXiSelect.Count; ++i)
    //                {
    //                    m_LiangXiSelect[i].transform.localPosition = new Vector3(m_LiangXiSelect[i].transform.localPosition.x, m_LiangXiSelect[i].transform.localPosition.y, 0f);
    //                }
    //                ClientSendLiangXi(m_LiangXiPoker);
    //                m_LiangXiSelect.Clear();
    //                m_LiangXiPoker.Clear();

    //                List<MaJiangCtrl> lstHand = MahJongManager.Instance.GetHand(RoomMaJiangProxy.Instance.PlayerSeat.Pos);
    //                for (int i = 0; i < lstHand.Count; ++i)
    //                {
    //                    lstHand[i].isGray = false;
    //                }
    //            }
    //            return;
    //        }

    //        List<Poker> lstPoker = obj[1] == null ? null : (obj[1] as List<Poker>);

    //        ClientSendOperate(type, lstPoker);
    //    }
    //    #endregion

    #region OnHeadClick 头像点击
    /// <summary>
    /// 头像点击
    /// </summary>
    /// <param name="seatPos"></param>
    private void OnHeadClick(object[] param)
    {
        int seatPos = (int)param[0];
        SeatEntity seat = RoomPaoDeKuaiProxy.Instance.GetSeatBySeatPos(seatPos);

        if (seat == RoomPaoDeKuaiProxy.Instance.PlayerSeat && AccountProxy.Instance.CurrentAccountEntity.identity > 0)
        {
            UIViewManager.Instance.OpenWindow(UIWindowType.PlayerInfo);
            return;
        }

        List<SeatEntityBase> lstSeat = new List<SeatEntityBase>();
        for (int i = 0; i < RoomPaoDeKuaiProxy.Instance.CurrentRoom.SeatList.Count; ++i)
        {
            if (RoomPaoDeKuaiProxy.Instance.CurrentRoom.SeatList[i].Pos != seatPos && RoomPaoDeKuaiProxy.Instance.CurrentRoom.SeatList[i].PlayerId > 0)
            {
                lstSeat.Add(RoomPaoDeKuaiProxy.Instance.CurrentRoom.SeatList[i]);
            }
        }

        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.SeatInfo, (GameObject go) =>
        {
            m_UISeatInfoView = go.GetComponent<UISeatInfoView>();
            m_UISeatInfoView.SetUI(seat, lstSeat);

                m_UISeatInfoView.SetEmoji(cfg_interactiveExpressionDBModel.Instance.GetList(), OnBtnInteractiveExpressionClick);
            
        });
    }
    #endregion

    #region OnBtnInteractiveExpressionClick 互动表情点击
    /// <summary>
    /// 互动表情点击
    /// </summary>
    /// <param name="id"></param>
    private void OnBtnInteractiveExpressionClick(int seatPos, int id)
    {
        //m_UISeatInfoView.Close();
        //cfg_interactiveExpressionEntity entity = cfg_interactiveExpressionDBModel.Instance.Get(id);
        //ChatCtrl.Instance.OnInteractiveClick(ENUM_PLAYER_MESSAGE.ANIMATION, entity.code, RoomMaJiangProxy.Instance.GetSeatBySeatId(seatPos).PlayerId, entity.sound);
    }
    #endregion

    #region OnBtnChangeSeatClick 换座位按钮点击
    /// <summary>
    /// 换座位按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnChangeSeatClick(object[] obj)
    {
        //if (m_isChangingSeat) return;
        //int seatIndex = (int)obj[0];

        //SeatEntity seat = RoomMaJiangProxy.Instance.GetSeatBySeatIndex(seatIndex);
        //if (seat == null) return;

        //int pos = seat.Pos;
        //if (seat.Pos == 3 && RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count == 2)
        //{
        //    pos = 2;
        //}
        //ClientSendChangeSeat(pos);
    }
    #endregion

    #region OnBtnSettleViewReadyClick 结算界面准备按钮点击
    /// <summary>
    /// 结算界面准备按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnSettleViewReadyClick(object[] obj)
    {
        ClientSendReady();
        m_UISettleView.Close();


        RoomPaoDeKuaiProxy.Instance.NextLoopGame();

        //清空桌面
        if (PaoDeKuaiSceneCtrl.Instance != null)
        {
            PaoDeKuaiSceneCtrl.Instance.NextLoopGame();

        }

    }
    #endregion

    #region OnBtnMaJiangViewReadyClick 麻将场景UI准备按钮点击
    /// <summary>
    /// 麻将场景UI准备按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPDKViewReadyClick(object[] obj)
    {
        ClientSendReady();
    }
    #endregion

    //    #region OnBtnMaJiangViewCancelReadyClick 麻将场景取消准备按钮点击
    //    /// <summary>
    //    /// 麻将场景取消准备按钮点击
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private void OnBtnMaJiangViewCancelReadyClick(object[] obj)
    //    {
    //        ClientSendCancelReady();
    //    }
    //    #endregion


    #region OnBtnPDKViewClick 场景UI出牌按钮点击
    /// <summary>
    /// 场景UI出牌按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPDKViewChuPaiClick(object[] obj)
    {
        //PaoDeKuaiSceneCtrl

        if (RoomPaoDeKuaiProxy.Instance.CheckCanPlayPoker(PaoDeKuaiSceneCtrl.Instance.CurrLiftUpPoker))
        {
            ClientSendPlayPoker(PaoDeKuaiSceneCtrl.Instance.CurrLiftUpPoker);
        }
        else
        {
            if (PaoDeKuaiSceneCtrl.Instance != null)
            {
                PaoDeKuaiSceneCtrl.Instance.OperateFeedback(OperateFeedbackType.PokerTypeError);
            }
        }
    }
    #endregion


    #region OnBtnPDKViewJiPaiQiClick 场景UI记牌器按钮点击
    /// <summary>
    /// 场景UI记牌器按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPDKViewJiPaiQiClick(object[] obj)
    {
        OpenView(UIWindowType.JiPaiQi);
    }
    #endregion





    /// <summary>
    ///  不出按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPDKViewBuChuClick(object[] obj)
    {
        NetWorkSocket.Instance.Send(null, PDK_PASS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }

    /// <summary>
    /// 提示点击
    /// </summary>
    private void OnBtnPDKViewTiShiClick(object[] obj)
    {
        //Debug.Log("还未实现提示");

      List<Poker> hint =  RoomPaoDeKuaiProxy.Instance.CheakHint();

        if (PaoDeKuaiSceneCtrl.Instance != null)
        {
            PaoDeKuaiSceneCtrl.Instance.Hint(hint);
        }
    }

    /// <summary>
    /// 抢关点击
    /// </summary>
    private void OnBtnPDKViewQianGuanClick(object[] obj)
    {
        Debug.Log("还未实现抢关");
    }
    /// <summary>
    /// 不抢关点击
    /// </summary>
    private void OnBtnPDKViewBuQiangGuanClick(object[] obj)
    {
        Debug.Log("还未实现不抢关");
        //NetWorkSocket.Instance.Send(null, .CODE, GameCtrl.Instance.SocketHandle);
    }


    #region OnBtnMaJiangViewShareClick 分享按钮点击
    /// <summary>
    /// 分享按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPDKViewShareClick(object[] obj)
    {
        ShareCtrl.Instance.ShareURL(ShareType.InGame);
    }
    #endregion

    #region OnBtnMaJiangViewAutoClick 托管按钮点击
    /// <summary>
    /// 托管按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPDKViewAutoClick(object[] obj)
    {
        if (RoomPaoDeKuaiProxy .Instance.CurrentRoom.matchId > 0)
        {
            if (RoomPaoDeKuaiProxy.Instance.PlayerSeat.IsTrustee) return;
            ClientSendTrustee(true);
        }
        else
        {
            RoomPaoDeKuaiProxy.Instance.Trustee(RoomPaoDeKuaiProxy.Instance.PlayerSeat.PlayerId, true);
        }

    }
    #endregion

    //    #region OnBtnMaJiangViewCancelAutoClick 取消托管按钮点击
    //    /// <summary>
    //    /// 取消托管按钮点击
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private void OnBtnMaJiangViewCancelAutoClick(object[] obj)
    //    {
    //        if (RoomMaJiangProxy.Instance.CurrentRoom.matchId > 0)
    //        {
    //            if (!RoomMaJiangProxy.Instance.PlayerSeat.IsTrustee) return;
    //            ClientSendTrustee(false);
    //        }
    //        else
    //        {
    //            RoomMaJiangProxy.Instance.Trustee(RoomMaJiangProxy.Instance.PlayerSeat.PlayerId, false);
    //        }
    //    }
    //    #endregion

    #region OnBtnSettleViewResultClick 牌局结算界面查看结果按钮点击
    /// <summary>
    /// 牌局结算界面查看结果按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnSettleViewResultClick(object[] obj)
    {
        m_UISettleView.Close();
        SeeResult();
    }
    #endregion

    //    #region OnBtnResultViewReplayOverClick 牌局结果界面回放结束按钮点击
    //    /// <summary>
    //    /// 牌局结果界面回放结束按钮点击
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private void OnBtnResultViewReplayOverClick(object[] obj)
    //    {
    //        ExitGame();
    //    }
    //    #endregion

    #region OnBtnResultViewBack 牌局结果界面返回按钮点击
    /// <summary>
    /// 牌局结果界面返回按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnResultViewBack(object[] obj)
    {
        if (RoomPaoDeKuaiProxy.Instance.CurrentRoom.matchId > 0)
        {
            m_UIResultView.Close();
            MatchCtrl.Instance.StartNext();
        }
        else
        {
            ExitGame();
        }
    }
    #endregion

    #region OnBtnResultViewShareClick 牌局结果界面分享按钮点击
    /// <summary>
    /// 牌局结果界面分享按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnResultViewShareClick(object[] obj)
    {
        if (m_UIResultView != null)
        {
            m_UIResultView.StartCoroutine(ShareCtrl.Instance.ScreenCapture(OnScreenCaptureComplete));
        }
        else if (m_UISettleView != null)
        {
            AppDebug.Log("开始分享了啊");
            m_UISettleView.StartCoroutine(ShareCtrl.Instance.ScreenCapture(OnScreenCaptureComplete));
        }
    }
    #endregion

    #region OnBtnDisbandViewRefuse 解散房间界面拒绝按钮点击
    /// <summary>
    /// 解散房间界面拒绝按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnDisbandViewRefuse(object[] obj)
    {
        ClientSendAnswerDimiss(false);
    }
    #endregion

    #region OnBtnDisbandViewAgree 解散房间界面同意按钮点击
    /// <summary>
    /// 解散房间界面同意按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnDisbandViewAgree(object[] obj)
    {
        ClientSendAnswerDimiss(true);
    }
    #endregion
    //    #endregion

    //    #region 客户端发送消息
    #region ClientSendCreateRoom 客户端发送创建房间消息
    /// <summary>
    /// 客户端发送创建房间消息
    /// </summary>
    private void ClientSendCreateRoom(int groupId, List<int> settingIds)
    {
        PDK_CREATE_ROOM_GET proto = new PDK_CREATE_ROOM_GET();

        for (int i = 0; i < settingIds.Count; ++i)
        {
            proto.addSettingId(settingIds[i]);
        }

        //List<cfg_settingEntity> lst = cfg_settingDBModel.Instance.GetOptionsByGameId(GameCtrl.Instance.CurrentGameId);
        //for (int i = 0; i < lst.Count; ++i)
        //{
        //    if (lst[i].status == 1 && lst[i].init == 1)
        //    {
        //        proto.addSettingId(lst[i].id);
        //    }
        //}
        //proto.clubId = groupId;
        Debug.Log("发送创建房间");
        NetWorkSocket.Instance.Send(proto.encode(), PDK_CREATE_ROOM_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendJoinRoom 客户端发送加入房间
    /// <summary>
    /// 客户端发送加入房间
    /// </summary>
    private void ClientSendJoinRoom(int roomId)
    {
        PDK_ENTER_ROOM_GET proto = new PDK_ENTER_ROOM_GET();
        proto.roomId = roomId;
        NetWorkSocket.Instance.Send(proto.encode(), PDK_ENTER_ROOM_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendRebuild 客户端发送重建房间
    /// <summary>
    /// 客户端发送重建房间
    /// </summary>
    private void ClientSendRebuild()
    {
        NetWorkSocket.Instance.Send(null, PDK_RECREATE.CODE, GameCtrl.Instance.SocketHandle);
        ClientSendFocus(true);
    }
    #endregion

    #region ClientSendLeaveRoom 客户端发送离开房间
    /// <summary>
    /// 客户端发送离开房间
    /// </summary>
    private void ClientSendLeaveRoom()
    {
        //NetWorkSocket.Instance.Send(null, OP_ROOM_LEAVE.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendTrustee 客户端发送托管消息
    /// <summary>
    /// 客户端发送托管消息
    /// </summary>
    private void ClientSendTrustee(bool isTrustee)
    {
        //OP_ROOM_TRUSTEE_GET proto = new OP_ROOM_TRUSTEE_GET();
        //proto.isTrustee = isTrustee;
        //NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_TRUSTEE_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendReady 客户端发送准备消息
    /// <summary>
    /// 客户端发送准备消息
    /// </summary>
    public void ClientSendReady()
    {
        NetWorkSocket.Instance.Send(null, PDK_READY_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    //    #region ClientSendCancelReady 客户端发送取消准备消息
    //    /// <summary>
    //    /// 客户端发送取消准备消息
    //    /// </summary>
    //    private void ClientSendCancelReady()
    //    {
    //        NetWorkSocket.Instance.Send(null, OP_ROOM_UNREADY_GET.CODE, GameCtrl.Instance.SocketHandle);
    //    }
    //    #endregion






    #region ClientSendPlayPoker 客户端发送出牌消息
    /// <summary>
    /// 客户端发送出牌消息
    /// </summary>
    /// <param name="poker">出的牌</param>
    public void ClientSendPlayPoker(List< Poker> pokers)
    {
        if (pokers == null || pokers.Count==0) return;

        PDK_OPERATE_GET proto = new PDK_OPERATE_GET();

        for (int i = 0; i < pokers.Count; ++i)
        {
            POKER_INFO prPoker = new POKER_INFO();
            prPoker.index = pokers[i].index;
            prPoker.size = pokers[i].size;
            prPoker.color = pokers[i].color;
            proto.addPokerInfo(prPoker);
        }
        NetWorkSocket.Instance.Send(proto.encode(), PDK_OPERATE_GET.CODE, GameCtrl.Instance.SocketHandle);


        //        if (RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Ready) return;
        //        if (RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Settle) return;
        //        if (RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Replay) return;
        //#if !IS_LEPING && !IS_TAILAI && !IS_LUALU
        //        if (RoomMaJiangProxy.Instance.CurrentOperator != RoomMaJiangProxy.Instance.PlayerSeat) return;
        //#endif
        //#if IS_LONGGANG
        //            if (MahJongHelper.CheckUniversal(poker, RoomMaJiangProxy.Instance.PlayerSeat.UniversalList)) return;
        //#endif

        //#if IS_LEPING
        //            ClientSendPass();
        //#endif
        //        if (IsTingByPlayPoker)
        //        {
        //            ClientSendPass();
        //        }


        //        OP_ROOM_OPERATE_GET proto = new OP_ROOM_OPERATE_GET();
        //        proto.index = poker.index;
        //        proto.isListen = IsTingByPlayPoker;
        //        AppDebug.Log("客户端出牌 索引:" + poker.index + "   花色:" + poker.color + "  大小:" + poker.size);
        //        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_OPERATE.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendPass 客户端发送过消息
    /// <summary>
    /// 客户端发送过消息
    /// </summary>
    public void ClientSendPass()
    {
        NetWorkSocket.Instance.Send(null, PDK_PASS.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion








    #region ClientSendApplyDisbandRoom_New 客户端发送申请解散房间
    /// <summary>
    /// 客户端发送申请解散房间
    /// </summary>
    private void ClientSendApplyDisbandRoom()
    {
        PDK_APPLY_DISMISS_GET proto = new PDK_APPLY_DISMISS_GET();
        //proto.isDismiss = true;
        NetWorkSocket.Instance.Send(null, PDK_APPLY_DISMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendAgreeDisbandRoom 客户端发送同意解散房间
    /// <summary>
    /// 客户端发送同意解散房间
    /// </summary>
    private void ClientSendAgreeDisbandRoom()
    {
        PDK_ANSWER_DIMISS_GET proto = new PDK_ANSWER_DIMISS_GET();
        proto.dismiss_status = DISMISS_STATUS.DISSMISS_AGENT ;
        NetWorkSocket.Instance.Send(proto.encode(), PDK_ANSWER_DIMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendRefuseDisbandRoom 客户端发送拒绝解散房间
    /// <summary>
    /// 客户端发送拒绝解散房间
    /// </summary>
    private void ClientSendRefuseDisbandRoom()
    {
        PDK_ANSWER_DIMISS_GET proto = new PDK_ANSWER_DIMISS_GET();
        proto.dismiss_status = DISMISS_STATUS.DISSMISS_REJECT;
        NetWorkSocket.Instance.Send(proto.encode(), PDK_ANSWER_DIMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendDisbandRoom_New 客户端发送答复解散房间(新)
    /// <summary>
    /// 客户端发送解散房间
    /// </summary>
    /// <param name="isAgree"></param>
    private void ClientSendAnswerDimiss(bool isAgree)
    {
        PDK_ANSWER_DIMISS_GET proto = new PDK_ANSWER_DIMISS_GET();
        proto.dismiss_status = isAgree? DISMISS_STATUS.DISSMISS_AGENT: DISMISS_STATUS.DISSMISS_REJECT;
        NetWorkSocket.Instance.Send(proto.encode(), PDK_ANSWER_DIMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendFocus 客户端发送焦点切换消息
    /// <summary>
    /// 客户端发送焦点切换消息
    /// </summary>
    /// <param name="isFocus"></param>
    public void ClientSendFocus(bool isFocus)
    {
        //OP_ROOM_AFK_GET proto = new OP_ROOM_AFK_GET();
        //proto.isAfk = !isFocus;

        //NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_AFK_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    //    #region ClientSendChangeSeat 客户端发送换座位消息
    //    /// <summary>
    //    /// 客户端发送换座位消息
    //    /// </summary>
    //    /// <param name="seatPos"></param>
    //    private void ClientSendChangeSeat(int seatPos)
    //    {
    //        OP_ROOM_SITDOWN_GET proto = new OP_ROOM_SITDOWN_GET();
    //        proto.pos = seatPos;
    //        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_SITDOWN_GET.CODE, GameCtrl.Instance.SocketHandle);
    //    }
    //    #endregion
    //    #endregion

    //    #region 服务器返回消息
    #region OnServerReturnCreateRoom 服务器返回创建房间消息
    /// <summary>
    /// 服务器返回创建房间消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnCreateRoom(byte[] obj)
    {
        CommandQueue.Clear();
        PDK_CREATE_ROOM proto = PDK_CREATE_ROOM.decode(obj);
        UIViewManager.Instance.CloseWait();




        //IGameCommand command = new CreateRoomCommand(CurrentRoom);
        //CommandQueue.Enqueue(command);

        RoomPaoDeKuaiProxy.Instance.InitRoom(proto.roomInfo);

        SceneMgr.Instance.LoadScene(SceneType.PaoDeKuai2D);
    }
    #endregion

    #region OnServerBroadcastReady 服务器广播玩家准备消息
    /// <summary>
    /// 服务器广播玩家准备消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastReady(byte[] obj)
    {
        PDK_READY proto = PDK_READY.decode(obj);
        IGameCommand command = new ReadyCommand(proto);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastCancelReady 服务器广播取消准备
    /// <summary>
    /// 服务器广播取消准备
    /// </summary>
    /// <param name="data"></param>
    private void OnServerBroadcastCancelReady(byte[] data)
    {
        //OP_ROOM_UNREADY proto = OP_ROOM_UNREADY.decode(data);
        //RoomMaJiangProxy.Instance.CancelReady(proto.playerId);
    }
    #endregion

    #region OnServerBroadcastEnter 服务器广播玩家进入消息
    /// <summary>
    /// 服务器广播玩家进入消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastEnter(byte[] obj)
    {

        PDK_ENTER_ROOM proto = PDK_ENTER_ROOM.decode(obj);

        IGameCommand command = new EnterRoomCommand(proto);
        CommandQueue.Enqueue(command);
        
    }
    #endregion

    #region OnServerBroadcastLeave 服务器广播玩家离开消息
    /// <summary>
    /// 服务器广播玩家离开消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastLeave(byte[] obj)
    {
        PDK_LEAVE proto = PDK_LEAVE.decode(obj);
        IGameCommand command = new QuitRoomCommand(proto);
        CommandQueue.Enqueue(command);
        
    }
    #endregion

    //    #region OnServerBroadcastWaiver 服务器广播玩家弃权消息
    //    /// <summary>
    //    /// 服务器广播玩家弃权消息
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private void OnServerBroadcastWaiver(byte[] obj)
    //    {
    //        OP_MATCH_WAIVER proto = new OP_MATCH_WAIVER();
    //        RoomMaJiangProxy.Instance.Waiver(proto.playerId);
    //    }
    //    #endregion

    #region OnServerBroadcastBegin 服务器广播开局
    /// <summary>
    /// 服务器广播开局
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastBegin(byte[] obj)
    {
        PDK_BEGIN proto = PDK_BEGIN.decode(obj);
        IGameCommand command = new PaoDeKuai.BeginCommand(proto);
        CommandQueue.Enqueue(command);

        //RoomPaoDeKuaiProxy.Instance.SetCountDown(0);

    }
    #endregion

    //    #region OnServerBroadcastDrawPoker 服务器广播摸牌消息
    //    /// <summary>
    //    /// 服务器广播摸牌消息
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private void OnServerBroadcastDrawPoker(byte[] obj)
    //    {
    //        OP_ROOM_GET_POKER proto = OP_ROOM_GET_POKER.decode(obj);

    //        try
    //        {
    //            if (proto.playerId == AccountProxy.Instance.CurrentAccountEntity.passportId && proto.color == 0)
    //            {
    //                AppDebug.ThrowError("服务器广播玩家摸了一张空牌");
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            AppDebug.LogError(e.ToString());
    //            RebuildRoom();
    //            return;
    //        }
    //        IGameCommand command = new DrawPokerCommand(proto.playerId, new Poker(proto.index, proto.color, proto.size, 0), proto.isLast, proto.isBuhua, proto.countdown);
    //        CommandQueue.Enqueue(command);
    //    }
    //    #endregion

    #region OnServerBroadcastOperate 服务器广播出牌消息
    /// <summary>
    /// 服务器广播出牌消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastPlayPoker(byte[] obj)
    {
        PDK_OPERATE proto = PDK_OPERATE.decode(obj);

        IGameCommand command = new PaoDeKuai.PlayPokerCommand(proto);
        CommandQueue.Enqueue(command);



        //OP_ROOM_OPERATE proto = OP_ROOM_OPERATE.decode(obj);
        //IsTingByPlayPoker = false;


        //IGameCommand command = new PlayPokerCommand(proto.playerId, new Poker(proto.index, proto.color, proto.size), proto.isListen);
        //CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastSeatOperate 服务器广播通知座位操作
    /// <summary>
    /// 服务器广播通知座位操作
    /// </summary>
    /// <param name="obj"></param>
    public void OnServerBroadcastSeatOperate(byte[] obj)
    {
        PDK_NEXT_PLAYER proto = PDK_NEXT_PLAYER.decode(obj);
        IGameCommand command = new PaoDeKuai.NoticeSeatOperateCommand(proto);
        CommandQueue.Enqueue(command);

    }

    #endregion

    #region OnServerBroadcastPass 服务器广播过
    /// <summary>
    /// 服务器广播过
    /// </summary>
    /// <param name="obj"></param>
    public void OnServerBroadcastPass(byte[] obj)
    {
        PDK_PASS proto = PDK_PASS.decode(obj);
        IGameCommand command = new PaoDeKuai.PassCommand(proto);
        CommandQueue.Enqueue(command);
    }
    #endregion


    #region OnServerBroadcastSeatOperate 服务器广播通知本局结算
    /// <summary>
    /// 服务器广播通知本局结算
    /// </summary>
    /// <param name="obj"></param>
    public void OnServerBroadcastSettle(byte[] obj)
    {
        PDK_GAME_OVER proto = PDK_GAME_OVER.decode(obj);
        m_Result = proto;
        IGameCommand command = new PaoDeKuai.SettleCommand(proto);
        CommandQueue.Enqueue(command);

    }
    #endregion
    public void OnServerBroadcastBombGoldChange(byte[] obj)
    {
        PDK_BOMB proto = PDK_BOMB.decode(obj);
        SeatEntity seat = RoomPaoDeKuaiProxy.Instance.GetSeatBySeatPos(proto.pos);
        if (seat != null)
        {
           int gain = proto.gold;
            int debuffs = proto.gold / (RoomPaoDeKuaiProxy.Instance.CurrentRoom.SeatList.Count - 1);
            List<SeatEntity> seatlist = RoomPaoDeKuaiProxy.Instance.CurrentRoom.SeatList;
            for (int i = 0; i < seatlist.Count; i++)
            {
                RoomPaoDeKuaiProxy.Instance.SetGold(seatlist[i].PlayerId, seatlist[i] == seat ? gain : debuffs);
            }


        }
    }




    //    #region OnServerPushAskFight 服务器询问是否吃碰杠胡
    //    /// <summary>
    //    /// 服务器询问是否吃碰杠胡
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private void OnServerPushAskFight(byte[] obj)
    //    {
    //        OP_ROOM_ASK_FIGHT proto = OP_ROOM_ASK_FIGHT.decode(obj);
    //        List<OP_POKER_GROUP> AskPokerGroup = null;
    //        int askLeng = proto.askPokerGroupCount();
    //        if (askLeng > 0)
    //        {
    //            AskPokerGroup = new List<OP_POKER_GROUP>();
    //            for (int i = 0; i < askLeng; ++i)
    //            {
    //                AskPokerGroup.Add(proto.getAskPokerGroup(i));
    //            }
    //        }

    //        IGameCommand command = new AskOperateCommand(AskPokerGroup, proto.countdown);
    //        CommandQueue.Enqueue(command);
    //    }
    //    #endregion

    //    #region OnServerBroadcastFight 服务器广播吃碰杠胡
    //    /// <summary>
    //    /// 服务器广播吃碰杠胡
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private void OnServerBroadcastFight(byte[] obj)
    //    {
    //        OP_ROOM_FIGHT proto = OP_ROOM_FIGHT.decode(obj);
    //        AppDebug.Log(string.Format("服务器广播{0} {1}了", proto.playerId, (OperatorType)proto.typeId));

    //        List<Poker> lst = new List<Poker>(proto.pokerCount());
    //        for (int i = 0; i < proto.pokerCount(); ++i)
    //        {
    //            OP_POKER op_poker = proto.getPoker(i);
    //            lst.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
    //        }

    //        IGameCommand command = new OperateCommand(proto.playerId, (OperatorType)proto.typeId, (int)proto.subTypeId, lst, proto.countdown);
    //        CommandQueue.Enqueue(command);
    //    }
    //    #endregion

    //    #region OnServerReturnPass 服务器返回过
    //    /// <summary>
    //    /// 服务器返回过
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private void OnServerReturnPass(byte[] obj)
    //    {
    //        AppDebug.Log("服务器返回Pass");
    //        IGameCommand command = new OperateCommand(RoomMaJiangProxy.Instance.PlayerSeat.PlayerId, OperatorType.Pass, 0, null, 0);
    //        CommandQueue.Enqueue(command);
    //    }
    //    #endregion

    //    #region OnServerReturnOperateWait 服务器返回吃碰杠胡等待
    //    /// <summary>
    //    /// 服务器返回吃碰杠胡等待
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private void OnServerReturnOperateWait(byte[] obj)
    //    {
    //        AppDebug.Log("服务器返回吃碰杠胡等待");
    //        UIItemOperator.Instance.Close();
    //    }
    //    #endregion

    //    #region OnServerBroadcastTrustee 服务器广播托管消息
    //    /// <summary>
    //    /// 服务器广播托管消息
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private void OnServerBroadcastTrustee(byte[] obj)
    //    {
    //        OP_ROOM_TRUSTEE proto = OP_ROOM_TRUSTEE.decode(obj);
    //        RoomMaJiangProxy.Instance.Trustee(proto.playerId, proto.isTrustee);
    //    }
    //    #endregion

    //    #region OnServerBroadcastSettle 服务器广播结算
    //    /// <summary>
    //    /// 服务器广播结算
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private void OnServerBroadcastSettle(byte[] obj)
    //    {
    //        AppDebug.Log("服务器广播结算");
    //        OP_ROOM_SETTLE proto = OP_ROOM_SETTLE.decode(obj);

    //        IGameCommand command = new SettleCommand(proto);
    //        CommandQueue.Enqueue(command);
    //    }
    //    #endregion

    //    #region OnServerReturnResult 服务器返回结果消息
    //    /// <summary>
    //    /// 服务器返回结果消息
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private void OnServerReturnResult(byte[] obj)
    //    {
    //        OP_ROOM_RESULT proto = OP_ROOM_RESULT.decode(obj);
    //        m_Result = proto;
    //    }
    //    #endregion

    #region OnServerReturnRebuild 服务器返回重建房间
    /// <summary>
    /// 服务器返回重建房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnRebuild(byte[] obj)
    {
        CommandQueue.Clear();

        PDK_RECREATE proto = PDK_RECREATE.decode(obj);

        RoomPaoDeKuaiProxy.Instance.InitRoom(proto.roomInfo);

        SceneMgr.Instance.LoadScene(SceneType.PaoDeKuai2D);


    }
    #endregion

    #region OnServerBroadcastApplyDisband 服务器广播解散房间
    /// <summary>
    /// 服务器广播解散房间(新版)
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastApplyDisband(byte[] obj)
    {
        PDK_APPLY_DISMISS proto = PDK_APPLY_DISMISS.decode(obj);

        //RoomPaoDeKuaiProxy.Instance.SetSeatDisbandState(proto.playerId, DisbandState.Apply , proto.dismissTime,proto.dismissMaxTime);
        //if (m_UIDisbandView == null)
        //{
        //    OpenDisbandView();
        //}

        SeatEntity seat = RoomPaoDeKuaiProxy.Instance.GetSeatByPlayerId(proto.playerId);
        string nickName = "有人";
        if (seat != null)
        {
            nickName = seat.Nickname;
        }
        if (seat == RoomPaoDeKuaiProxy.Instance.PlayerSeat)
        {
            UIViewManager.Instance.ShowMessage("提示", "你已发起解散房间，请等待解散结果", MessageViewType.None);
        }
        else
        {
        UIViewManager.Instance.ShowMessage("提示", string.Format("{0}发起解散房间，是否同意", nickName), MessageViewType.OkAndCancel, ClientSendAgreeDisbandRoom, ClientSendRefuseDisbandRoom, 10f, AutoClickType.Cancel);

        }



        ////是否是申请解散
        //if (proto.hasIsSucceed())
        //{
        //    if (proto.isSucceed)
        //    {
        //        UIViewManager.Instance.ShowMessage("提示", "房间已解散", MessageViewType.Ok, SeeResult);
        //    }
        //    else
        //    {
        //        string nickName = "有人";
        //        for (int i = 0; i < RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count; ++i)
        //        {
        //            if (RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i].DisbandState == DisbandState.Refuse)
        //            {
        //                nickName = RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i].Nickname;
        //            }
        //        }
        //        UIViewManager.Instance.ShowMessage("提示", string.Format("{0}不同意解散房间", nickName), MessageViewType.Ok);
        //        for (int i = 0; i < RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count; ++i)
        //        {
        //            RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i].DisbandState = DisbandState.Wait;
        //        }
        //    }
        //    if (m_UIDisbandView != null)
        //    {
        //        m_UIDisbandView.Close();
        //    }
        //}
        //else
        //{
        //    RoomMaJiangProxy.Instance.SetSeatDisbandState(proto.playerId, (DisbandState)proto.dismiss, proto.dismissTime);
        //    if (m_UIDisbandView == null)
        //    {
        //        OpenDisbandView();
        //    }
        //    else
        //    {
        //        m_UIDisbandView.SetUI(RoomMaJiangProxy.Instance.CurrentRoom.SeatList, RoomMaJiangProxy.Instance.PlayerSeat, 0f, RoomMaJiangProxy.Instance.CurrentRoom.DisbandTimeMax / 1000);
        //    }

        //}
    }
    #endregion

    #region OnServerBroadcastAnswerDimiss 服务器广播答复解散房间
    private void OnServerBroadcastAnswerDimiss(byte[] obj)
    {
        PDK_ANSWER_DIMISS proto = PDK_ANSWER_DIMISS.decode(obj);
        RoomPaoDeKuaiProxy.Instance.SetSeatDisbandState(proto.playerId, proto.dismiss_status == DISMISS_STATUS.DISSMISS_AGENT ? DisbandState.Agree : DisbandState.Refuse);
        //if (m_UIDisbandView != null)
        //    m_UIDisbandView.SetUI(RoomPaoDeKuaiProxy.Instance.CurrentRoom.SeatList, RoomPaoDeKuaiProxy.Instance.PlayerSeat, 0f, RoomPaoDeKuaiProxy.Instance.CurrentRoom.DisbandTimeMax / 1000);
        if (proto.playerId == RoomPaoDeKuaiProxy.Instance.PlayerSeat.PlayerId)
        {
            UIViewManager.Instance.ShowMessage("提示", "请等待解散结果", MessageViewType.None);
        }
    }
    #endregion

    #region OnServerBroadcastApplyDisband_New 服务器广播解散房间(新版)
    /// <summary>
    /// 服务器广播解散房间(新版)
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastApplyDisband_New(byte[] obj)
    {
        PDK_APPLY_DISMISS proto = PDK_APPLY_DISMISS.decode(obj);

        RoomPaoDeKuaiProxy.Instance.SetSeatDisbandState(proto.playerId, DisbandState.Apply, proto.dismissTime, proto.dismissMaxTime);
        if (m_UIDisbandView == null)
        {
            OpenDisbandView();
        }


    }
    #endregion


    #region OnServerBroadcastAnswerDimiss 服务器广播答复解散房间
    private void OnServerBroadcastAnswerDimiss_New(byte[] obj)
    {
        PDK_ANSWER_DIMISS proto = PDK_ANSWER_DIMISS.decode(obj);
        RoomPaoDeKuaiProxy.Instance.SetSeatDisbandState(proto.playerId, proto.dismiss_status == DISMISS_STATUS.DISSMISS_AGENT ? DisbandState.Agree : DisbandState.Refuse);
        if (m_UIDisbandView != null)
            m_UIDisbandView.SetUI(RoomPaoDeKuaiProxy.Instance.CurrentRoom.SeatList, RoomPaoDeKuaiProxy.Instance.PlayerSeat, 0f, RoomPaoDeKuaiProxy.Instance.CurrentRoom.DisbandTimeMax / 1000);


    }
    #endregion


    #region OnServerBroadcastDimissSucceed 服务器广播答复解散房间成功
    private void OnServerBroadcastDimissSucceed(byte[] obj)
    {
        UIViewManager.Instance.ShowMessage("提示", "房间已解散", MessageViewType.Ok, SeeResult);
        if (m_UIDisbandView != null)
        {
            m_UIDisbandView.Close();
        }

    }
    #endregion

    #region OnServerBroadcastDimissDefeat 服务器广播答复解散房间失败
    private void OnServerBroadcastDimissDefeat(byte[] obj)
    {
        UIViewManager.Instance.ShowMessage("提示", "解散房间失败", MessageViewType.Ok);
    }
    #endregion


    //    #region OnServerBroadcastOffLine 服务器广播离线
    //    /// <summary>
    //    /// 服务器广播离线
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private void OnServerBroadcastOffLine(byte[] obj)
    //    {
    //        if (RoomMaJiangProxy.Instance == null) return;

    //        OP_PLAYER_STATUS proto = OP_PLAYER_STATUS.decode(obj);
    //        RoomMaJiangProxy.Instance.SetOnLine(proto.playerId, proto.online > 0);
    //    }
    //    #endregion

    //    #region OnServerBroadcastFocus 服务器广播焦点
    //    /// <summary>
    //    /// 服务器广播焦点
    //    /// </summary>
    //    /// <param name="buffer"></param>
    //    private void OnServerBroadcastFocus(byte[] buffer)
    //    {
    //        OP_ROOM_AFK proto = OP_ROOM_AFK.decode(buffer);
    //        RoomMaJiangProxy.Instance.SetFocus(proto.playerId, !proto.isAfk);
    //    }
    //    #endregion

    //    #region OnServerBroadcastStatus 服务器广播状态
    //    /// <summary>
    //    /// 服务器广播状态
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private void OnServerBroadcastStatus(byte[] obj)
    //    {
    //        OP_ROOM_STATUS proto = OP_ROOM_STATUS.decode(obj);

    //        IGameCommand command = new ChangeStatusCommand((RoomEntity.RoomStatus)proto.status);
    //        CommandQueue.Enqueue(command);
    //    }
    //    #endregion

    //#region OnServerBroadcastGoldChanged 服务器广播金币变化
    ///// <summary>
    ///// 服务器广播金币变化
    ///// </summary>
    ///// <param name="obj"></param>
    //private void OnServerBroadcastGoldChanged(byte[] obj)
    //{
    //    OP_ROOM_FIGHT_GOLD proto = OP_ROOM_FIGHT_GOLD.decode(obj);

    //    for (int i = 0; i < proto.seatCount(); ++i)
    //    {
    //        RoomMaJiangProxy.Instance.SetGold(proto.getSeat(i).playerId, proto.getSeat(i).gold);
    //    }
    //}
    //#endregion

    //    #region OnServerBroadcastDouble 服务器广播翻倍
    //    /// <summary>
    //    /// 服务器广播翻倍
    //    /// </summary>
    //    /// <param name="bytes"></param>
    //    private void OnServerBroadcastDouble(byte[] bytes)
    //    {
    //        OP_ROOM_DOUBLE proto = OP_ROOM_DOUBLE.decode(bytes);

    //        RoomMaJiangProxy.Instance.SetDouble(proto.playerId, proto.isDouble);
    //    }
    //    #endregion

    //    #region OnServerBroadcastChangeSeat 服务器广播换座位消息
    //    /// <summary>
    //    /// 服务器广播换座位消息
    //    /// </summary>
    //    /// <param name="bytes"></param>
    //    private void OnServerBroadcastChangeSeat(byte[] bytes)
    //    {
    //        OP_ROOM_SITDOWN proto = OP_ROOM_SITDOWN.decode(bytes);

    //        if (RoomMaJiangProxy.Instance.SetPos(proto.playerId, proto.pos))
    //        {
    //            if (proto.playerId == RoomMaJiangProxy.Instance.PlayerSeat.PlayerId)
    //            {
    //                m_isChangingSeat = true;
    //                CameraCtrl.Instance.SetPos(RoomMaJiangProxy.Instance.PlayerSeat.Pos, () =>
    //                {
    //                    m_isChangingSeat = false;
    //                });
    //            }
    //        }
    //    }
    //    #endregion

    //    #region OnServerBroadcastCheck 服务器广播检测消息
    //    /// <summary>
    //    /// 服务器广播检测消息
    //    /// </summary>
    //    /// <param name="bytes"></param>
    //    private void OnServerBroadcastCheck(byte[] bytes)
    //    {
    //        OP_ROOM_RECHECK proto = OP_ROOM_RECHECK.decode(bytes);

    //        List<Poker> lst = new List<Poker>();
    //        for (int i = 0; i < proto.pokerCount(); ++i)
    //        {
    //            OP_POKER op_poker = proto.getPoker(i);
    //            lst.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
    //        }

    //        IGameCommand command = new CheckCommand(lst);
    //        CommandQueue.Enqueue(command);
    //    }
    //    #endregion
    //    #endregion


    #region MockBroadcast 模拟消息通知

    #region 
    /// <summary>
    /// 模拟通知某座位操作
    /// </summary>
    public void MockBroadcastSeatOperate(int pos)
    {
        SeatEntity seat = RoomPaoDeKuaiProxy.Instance.GetSeatBySeatPos(pos);
        if (seat != null)
        {

            PDK_NEXT_PLAYER data = new PDK_NEXT_PLAYER();
            data.pos = seat.Pos;
            NoticeSeatOperateCommand operateCommand = new NoticeSeatOperateCommand(data);
            operateCommand.Execute();

        }
    }
    #endregion

    #region MockBroadcastPlayPoker 模拟某座位出牌
    /// <summary>
    /// 模拟某座位出牌
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="pokerList"></param>
    public void MockBroadcastPlayPoker(int pos, List<Poker> pokerList)
    {
        SeatEntity seat = RoomPaoDeKuaiProxy.Instance.GetSeatBySeatPos(pos);
        if (seat == null || pokerList == null|| pokerList.Count==0) return;

        PDK_OPERATE data = new PDK_OPERATE();
        data.pos = pos;

        List<POKER_INFO> prPokerList = data.getPokerInfoList();

        for (int i = 0; i < pokerList.Count; ++i)
        {
            POKER_INFO prPoker = new POKER_INFO();
            prPoker.index = pokerList[i].index;
            prPoker.size = pokerList[i].size;
            prPoker.color = pokerList[i].color;
            //prPoker.type = pokerList[i].t;
            prPokerList.Add(prPoker);
        }

        IGameCommand command = new PaoDeKuai.PlayPokerCommand(data);
        command.Execute();

    }
    #endregion

    #endregion






}
