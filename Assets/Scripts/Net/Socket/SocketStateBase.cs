//===================================================
//Author      : DRB
//CreateTime  ：8/7/2017 10:48:41 AM
//Description ：套接字状态基类
//===================================================
using UnityEngine;


public class SocketStateBase 
{

    protected SocketClient m_SocketClient;

    public SocketStateBase(SocketClient socketClient)
    {
        m_SocketClient = socketClient;
    }


    /// <summary>
    /// 状态进入
    /// </summary>
    public virtual void OnEnter() { }

    /// <summary>
    /// 状态更新
    /// </summary>
    public virtual void OnUpdate() { }

    /// <summary>
    /// 状态离开
    /// </summary>
    public virtual void OnExit() { }

    protected void ChangeState(SocketClient.SocketState state)
    {
        m_SocketClient.ChangeState(state);
    }

}
