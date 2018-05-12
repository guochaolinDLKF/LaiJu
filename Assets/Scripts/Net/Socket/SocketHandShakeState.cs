//===================================================
//Author      : DRB
//CreateTime  ：8/7/2017 11:44:39 AM
//Description ：
//===================================================
using proto.common;
using UnityEngine;


public class SocketHandShakeState : SocketStateBase
{
    private long m_SendHandShakeClientTime;//握手消息发送时间

    private const int HAND_SHAKE_TIME_OUT = 5000;//握手超时时间


    public SocketHandShakeState(SocketClient socketClient) : base(socketClient)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        NetDispatcher.Instance.AddEventListener(OP_PLAYER_CONNECT.CODE, OnServerReturnHandShake);
        m_SocketClient.ReceiveMessage();
        ClientSendHandShake();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (TimeUtil.GetTimestampMS() - m_SendHandShakeClientTime > HAND_SHAKE_TIME_OUT)//握手超时
        {
            Debug.Log("握手超时");
            m_SocketClient.Close(false);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        NetDispatcher.Instance.RemoveEventListener(OP_PLAYER_CONNECT.CODE, OnServerReturnHandShake);
    }



    #region ClientSendHandShake 客户端发送握手消息
    /// <summary>
    /// 客户端发送握手消息
    /// </summary>
    private void ClientSendHandShake()
    {
        OP_PLAYER_CONNECT_GET proto = new OP_PLAYER_CONNECT_GET();
        proto.passportId = AccountProxy.Instance.CurrentAccountEntity.passportId;
        proto.token = AccountProxy.Instance.CurrentAccountEntity.token;
        proto.latitude = LPSManager.Instance.Latitude;
        proto.longitude = LPSManager.Instance.Longitude;

        m_SendHandShakeClientTime = TimeUtil.GetTimestampMS();
        m_SocketClient.Send(proto.encode(), OP_PLAYER_CONNECT_GET.CODE);

        Debug.Log("客户端发送握手消息");
    }
    #endregion

    #region OnServerReturnHandShake 服务器返回握手消息
    /// <summary>
    /// 服务器返回握手消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnHandShake(byte[] obj)
    {
        OP_PLAYER_CONNECT proto = OP_PLAYER_CONNECT.decode(obj);
        long serverTime = proto.unixtime;
        int handShakePing = (int)((TimeUtil.GetTimestampMS() - m_SendHandShakeClientTime) / 2);
        AppDebug.Log("fps=" + handShakePing + "ms");
        long ServerCurrentTime = proto.unixtime + handShakePing;
        GlobalInit.Instance.TimeDistance = TimeUtil.GetTimestampMS() - ServerCurrentTime;
        AppDebug.Log("服务器时间和客户端时间差值" + GlobalInit.Instance.TimeDistance + "ms");
        if (m_SocketClient.onConnectComplete != null)
        {
            m_SocketClient.onConnectComplete(true);
            m_SocketClient.onConnectComplete = null;
        }

        ChangeState(SocketClient.SocketState.Communicate);
    }
    #endregion
}
