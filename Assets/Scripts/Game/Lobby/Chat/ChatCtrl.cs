//===================================================
//Author      : DRB
//CreateTime  ：4/15/2017 6:27:21 PM
//Description ：
//===================================================
using proto.common;
using UnityEngine;

public class ChatCtrl : SystemCtrlBase<ChatCtrl>, ISystemCtrl
{
    private UIChatView m_UIChatView;

    private UIMicroView m_UIMicroView;

    public bool isPlayExternalAudio = true;

    public ChatCtrl()
    {
        NetDispatcher.Instance.AddEventListener(OP_PLAYER_MESSAGE.CODE, OnServerBroadcastMessage);//服务器广播聊天消息
    }

    public override void Dispose()
    {
        base.Dispose();
        NetDispatcher.Instance.RemoveEventListener(OP_PLAYER_MESSAGE.CODE, OnServerBroadcastMessage);//服务器广播聊天消息
    }

    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.Chat:
                OpenChatView();
                break;
            case UIWindowType.Micro:
                OpenMicroView();
                break;
        }
    }

    #region OpenChatView 打开聊天界面
    /// <summary>
    /// 打开聊天界面
    /// </summary>
    private void OpenChatView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Chat, (GameObject go) =>
        {
            m_UIChatView = go.GetComponent<UIChatView>();
            m_UIChatView.SetUI(cfg_commonMessageDBModel.Instance.GetListByGameId(GameCtrl.Instance.CurrentGameId), cfg_chatExpressionDBModel.Instance.GetList(), OnCommonMessageClick, OnExpressionClick);
        });
    }
    #endregion

    #region OpenMicroView 打开录音界面
    /// <summary>
    /// 打开录音界面
    /// </summary>
    private void OpenMicroView()
    {
        MicroPhoneManager.Instance.RequestMicroPermission(OnRequestMicroAuthorityCallBack);
    }
    #endregion

    #region OnCommonMessageClick 通用聊天语句点击
    /// <summary>
    /// 通用聊天语句点击
    /// </summary>
    /// <param name="message"></param>
    private void OnCommonMessageClick(string message)
    {
        string audioName = string.Empty;
        cfg_commonMessageEntity commonMessageEntity = cfg_commonMessageDBModel.Instance.GetEntityByMessage(message);
        if (commonMessageEntity != null)
        {
            audioName = commonMessageEntity.AudioName;
        }
        GameCtrl.Instance.OnReceiveMessage(ChatType.Message, AccountProxy.Instance.CurrentAccountEntity.passportId, message, audioName, 0);

        ClientSendMessage(ENUM_PLAYER_MESSAGE.STRING, message, 0);
        m_UIChatView.Close();
    }
    #endregion

    #region OnExpressionClick 表情点击
    /// <summary>
    /// 表情点击
    /// </summary>
    /// <param name="expressionId"></param>
    private void OnExpressionClick(int expressionId)
    {
        cfg_chatExpressionEntity entity = cfg_chatExpressionDBModel.Instance.Get(expressionId);

        GameCtrl.Instance.OnReceiveMessage(ChatType.Expression, AccountProxy.Instance.CurrentAccountEntity.passportId, entity.image, entity.sound, 0);

        ClientSendMessage(ENUM_PLAYER_MESSAGE.FACE, entity.code, 0);
        m_UIChatView.Close();
    }
    #endregion

    #region OnInteractiveClick 互动表情点击
    /// <summary>
    /// 互动表情点击
    /// </summary>
    /// <param name="type"></param>
    /// <param name="message"></param>
    /// <param name="toPlayerId"></param>
    public void OnInteractiveClick(ENUM_PLAYER_MESSAGE type, string message, int toPlayerId, string audioName)
    {
        GameCtrl.Instance.OnReceiveMessage(ChatType.InteractiveExpression, AccountProxy.Instance.CurrentAccountEntity.passportId, message, audioName, toPlayerId);
        ClientSendMessage(type, message, toPlayerId);
    }
    #endregion

    #region ClientSendMessage 客户端发送消息
    /// <summary>
    /// 客户端发送消息
    /// </summary>
    /// <param name="message"></param>
    private void ClientSendMessage(ENUM_PLAYER_MESSAGE type, string message, int toPlayerId)
    {
        OP_PLAYER_MESSAGE_GET proto = new OP_PLAYER_MESSAGE_GET();
        proto.typeId = type;
        proto.content = System.Text.Encoding.UTF8.GetBytes(message);
        proto.toId = toPlayerId;

        NetWorkSocket.Instance.Send(proto.encode(), OP_PLAYER_MESSAGE_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendMicro 客户端发送语音
    /// <summary>
    /// 客户端发送语音
    /// </summary>
    /// <param name="bytes"></param>
    private void ClientSendMicro(byte[] bytes)
    {
        OP_PLAYER_MESSAGE_GET proto = new OP_PLAYER_MESSAGE_GET();
        proto.typeId = ENUM_PLAYER_MESSAGE.BYTES;
        proto.content = bytes;
        NetWorkSocket.Instance.Send(proto.encode(), OP_PLAYER_MESSAGE_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region OnRequestMicroAuthorityCallBack 请求麦克风权限回调
    /// <summary>
    /// 请求麦克风权限回调
    /// </summary>
    /// <param name="isUse"></param>
    private void OnRequestMicroAuthorityCallBack(bool isUse)
    {
        if (isUse)
        {
            MicroPhoneManager.Instance.StartRecord(OnMicroVolumeChangedCallBack, OnMicroStopCallBack);
            UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Micro, (GameObject go) =>
             {
                 m_UIMicroView = go.GetComponent<UIMicroView>();
             });
        }
        else
        {
            ShowMessage("错误", "找不到声音输入设备或没有权限");
        }
    }
    #endregion

    #region OnMicroVolumeChangedCallBack 麦克风音量等级改变回调
    /// <summary>
    /// 麦克风音量等级改变回调
    /// </summary>
    /// <param name="level">音量等级</param>
    private void OnMicroVolumeChangedCallBack(int level)
    {
        if (m_UIMicroView == null) return;
        m_UIMicroView.SetMicroVolumeLevel(level);
    }
    #endregion

    #region OnMicroStopCallBack 麦克风录音关闭回调
    /// <summary>
    /// 麦克风录音关闭回调
    /// </summary>
    private void OnMicroStopCallBack(bool isValid)
    {
        if (!isValid)
        {
            ShowMessage("错误提示", "录制太长或太短");
        }
        if (m_UIMicroView == null) return;
        m_UIMicroView.Close();
    }
    #endregion

    #region SendMicro 发送录音
    /// <summary>
    /// 发送录音
    /// </summary>
    public void SendMicro()
    {
        if (!MicroPhoneManager.Instance.isRecoding) return;
        if (!MicroPhoneManager.Instance.StopRecord()) return;
        byte[] bytes = MicroPhoneManager.Instance.GetClipData();
        Debug.Log("原始录音数据大小" + bytes.Length);
        MicroPhoneManager.Instance.PlayExternalAudio(bytes);
        GameCtrl.Instance.OnReceiveMessage(ChatType.MicroPhone, AccountProxy.Instance.CurrentAccountEntity.passportId, string.Empty, string.Empty, 0);

        ClientSendMicro(bytes);
    }
    #endregion

    #region CancelMicro 取消录音
    /// <summary>
    /// 取消录音
    /// </summary>
    public void CancelMicro()
    {
        if (!MicroPhoneManager.Instance.isRecoding) return;
        if (!MicroPhoneManager.Instance.StopRecord())
        {
            ShowMessage("错误", "找不到声音输入设备");
        }
    }
    #endregion



    #region OnServerBroadcastMessage 服务器广播聊天消息
    /// <summary>
    /// 服务器广播聊天消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastMessage(byte[] obj)
    {
        OP_PLAYER_MESSAGE proto = OP_PLAYER_MESSAGE.decode(obj);

        if (proto.typeId == ENUM_PLAYER_MESSAGE.STRING)
        {
            string message = System.Text.Encoding.UTF8.GetString(proto.content);
            cfg_commonMessageEntity commonMessageEntity = cfg_commonMessageDBModel.Instance.GetEntityByMessage(message);
            string audioName = string.Empty;
            if (commonMessageEntity != null)
            {
                audioName = commonMessageEntity.AudioName;
            }
            GameCtrl.Instance.OnReceiveMessage(ChatType.Message, proto.playerId, message, audioName, proto.toId);
        }
        else if (proto.typeId == ENUM_PLAYER_MESSAGE.ANIMATION)
        {
            string message = System.Text.Encoding.UTF8.GetString(proto.content);
            cfg_interactiveExpressionEntity entity = cfg_interactiveExpressionDBModel.Instance.GetEntityByCode(message);
            if (entity != null)
            {
                GameCtrl.Instance.OnReceiveMessage(ChatType.InteractiveExpression, proto.playerId, entity.animation, entity.sound, proto.toId);
            }
        }
        else if (proto.typeId == ENUM_PLAYER_MESSAGE.FACE)
        {
            string message = System.Text.Encoding.UTF8.GetString(proto.content);
            cfg_chatExpressionEntity entity = cfg_chatExpressionDBModel.Instance.GetEntityIdByCode(message);
            if (entity != null)
            {
                GameCtrl.Instance.OnReceiveMessage(ChatType.Expression, proto.playerId, entity.image, entity.sound, proto.toId);
            }
        }
        else if (proto.typeId == ENUM_PLAYER_MESSAGE.BYTES)
        {
            if (isPlayExternalAudio)
            {
                MicroPhoneManager.Instance.PlayExternalAudio(proto.content);
                GameCtrl.Instance.OnReceiveMessage(ChatType.MicroPhone, proto.playerId, string.Empty, string.Empty, proto.toId);
            }
        }
    }
    #endregion
}
