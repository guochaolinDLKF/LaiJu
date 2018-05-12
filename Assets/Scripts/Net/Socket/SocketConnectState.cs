//===================================================
//Author      : DRB
//CreateTime  ：8/7/2017 10:48:01 AM
//Description ：
//===================================================
using System;
using System.Net;
using System.Net.Sockets;
using proto.common;
using UnityEngine;


public class SocketConnectState : SocketStateBase
{
    private bool m_isConnectSuccess = false;

    private bool m_isConnectComplete = false;

    public SocketConnectState(SocketClient socketClient) : base(socketClient) { }

    public override void OnEnter()
    {
        base.OnEnter();

        m_SocketClient.CurrentNetType = Application.internetReachability;
        m_isConnectSuccess = false;
        m_isConnectComplete = false;
        m_SocketClient.ClearMessageQueue();
        BeginConnect();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_isConnectComplete)
        {
            if (m_isConnectSuccess)
            {
                //if (m_SocketClient.onConnectComplete != null)
                //{
                //    m_SocketClient.onConnectComplete(true);
                //}
                //m_SocketClient.ReceiveMessage();
                ChangeState(SocketClient.SocketState.HandShake);
            }
            else
            {
                m_SocketClient.Close(false);
            }
            m_isConnectComplete = false;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }






    #region BeginConnect 异步连接
    /// <summary>
    /// 异步连接
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    /// <param name="onConnectedCallBack"></param>
    public void BeginConnect()
    {
        if (m_SocketClient.Socket != null && m_SocketClient.Socket.Connected)
        {
            Debug.Log("重复连接？？？？？？？？？");
            m_SocketClient.Close(false);
            return;
        }

        string ip = m_SocketClient.CurrentHost;
        AddressFamily addressFamily = AddressFamily.InterNetwork;

        IPAddress[] ips = null;
        try
        {
            ips = Dns.GetHostAddresses(m_SocketClient.CurrentHost);
        }
        catch
        {
            m_isConnectComplete = true;
            m_isConnectSuccess = false;
            return;
        }

        if (ips != null && ips.Length > 0)
        {
            string rawIp = ips[0].ToString();
            GetIP(rawIp, out ip, out addressFamily);
            m_SocketClient.Socket = new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);
            for (int i = 0; i < ips.Length; ++i)
            {
                Debug.Log("DNS解析的地址有：" + ips[i].ToString() + "网络类型:" + ips[i].AddressFamily.ToString());

                //if (ips[i].AddressFamily == AddressFamily.InterNetwork)
                //{
                //    string rawIp = ips[i].ToString();
                //    GetIP(rawIp, out ip, out addressFamily);
                //    m_SocketClient.Socket = new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);
                //    break;
                //}
            }
        }
        else
        {
            m_SocketClient.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        try
        {
            AppDebug.Log(string.Format("连接{0}:{1}", ip, m_SocketClient.CurrentPort.ToString()));
            m_SocketClient.Socket.BeginConnect(ip, m_SocketClient.CurrentPort, ConnectCallBack, m_SocketClient.Socket);
        }
        catch
        {
            m_isConnectComplete = true;
            m_isConnectSuccess = false;
        }
    }
    #endregion

    #region ConnectCallBack 连接回调
    /// <summary>
    /// 连接回调
    /// </summary>
    /// <param name="ar"></param>
    private void ConnectCallBack(IAsyncResult ar)
    {
        Socket client = (Socket)ar.AsyncState;
        try
        {
            client.EndConnect(ar);
            m_SocketClient.ClearMessageQueue();
            m_isConnectSuccess = true;
            LogSystem.Log("Socket连接成功");
        }
        catch (Exception e)
        {
            m_isConnectSuccess = false;
            LogSystem.Log("Socket连接失败!" + e.Message);
        }
        finally
        {
            m_isConnectComplete = true;
        }
    }
    #endregion


    private void GetIP(string ip, out string oIp, out AddressFamily oAddressFamily)
    {
        oIp = string.Empty;
        oAddressFamily = AddressFamily.InterNetwork;
        string newIp = SDK.Instance.GetIpV6(ip);
        string[] split = newIp.Split('&');
        if (split.Length == 2)
        {
            oIp = split[0];
            oAddressFamily = split[1].Equals("ipv6", StringComparison.CurrentCultureIgnoreCase) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
        }
    }
}
