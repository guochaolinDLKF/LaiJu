//===================================================
//Author      : DRB
//CreateTime  ：8/7/2017 11:17:44 AM
//Description ：
//===================================================
using proto.common;
using UnityEngine;


public class SocketCommunicateState : SocketStateBase
{
    private float m_PrevSendHeartTime = 0.0f;//上一次发送心跳包时间
    private const float SEND_HEART_BEAT_SPACE = 5f;//发送心跳间隔
    private float m_PrevReceiveHeartTime = 0.0f;//上一次接收心跳包时间
    private const float HEART_BEAT_OVER_TIME = 15f;//心跳超时时间

    public SocketCommunicateState(SocketClient socketClient) : base(socketClient)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        UIViewManager.Instance.CloseReconnectView();

        if (m_SocketClient.isReconnect)
        {
            if (m_SocketClient.OnReconnect != null)
            {
                m_SocketClient.OnReconnect();
            }
            m_SocketClient.isReconnect = false;
        }
        m_SocketClient.lastHeart = null;
        m_PrevSendHeartTime = 0f;
        m_PrevReceiveHeartTime = Time.realtimeSinceStartup;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (Time.realtimeSinceStartup - m_PrevSendHeartTime > SEND_HEART_BEAT_SPACE)
        {
            m_PrevSendHeartTime = Time.realtimeSinceStartup;
            ClientSendHeart();
        }

        if (Time.realtimeSinceStartup - m_PrevReceiveHeartTime > HEART_BEAT_OVER_TIME)
        {
            Debug.LogWarning("心跳超时，断开连接");
            ChangeState(SocketClient.SocketState.Reconnect);
        }

        if (m_SocketClient.lastHeart != null)
        {
            long sendTime = m_SocketClient.lastHeart.cli_time;
            long serverTime = m_SocketClient.lastHeart.svr_time;
            m_PrevReceiveHeartTime = Time.realtimeSinceStartup;

            long localTime = TimeUtil.GetTimestampMS();

            long fps = (localTime - sendTime) / 2;

            serverTime = serverTime + fps;

            if (m_SocketClient.Handle == GameCtrl.Instance.SocketHandle)
            {
                GlobalInit.Instance.TimeDistance = localTime - serverTime;
            }
            LogSystem.Log("FPS:" + fps.ToString() + "毫秒");
            m_SocketClient.lastHeart = null;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        m_SocketClient.lastHeart = null;
    }

    #region ClientSendHeart 客户端发送心跳包
    /// <summary>
    /// 客户端发送心跳包
    /// </summary>
    private void ClientSendHeart()
    {
        OP_SYS_HEART_GET proto = new OP_SYS_HEART_GET();
        proto.cli_time = TimeUtil.GetTimestampMS();
        m_SocketClient.Send(proto.encode(), OP_SYS_HEART_GET.CODE);
    }
    #endregion
}
