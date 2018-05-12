//===================================================
//Author      : DRB
//CreateTime  ：9/1/2017 11:43:58 AM
//Description ：
//===================================================
using System.Collections.Generic;
using UnityEngine;
using proto.common;

public class NetWorkSocket : SingletonMono<NetWorkSocket>
{
    private Dictionary<int, SocketClient> m_Dic = new Dictionary<int, SocketClient>();

    private List<SocketClient> m_List = new List<SocketClient>();

    private int m_Seek = 0;

    private void Update()
    {
        for (int i = 0; i < m_List.Count; ++i)
        {
            m_List[i].OnUpdate();
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        for (int i = 0; i < m_List.Count; ++i)
        {
            m_List[i].Close();
        }
        m_Dic.Clear();
        m_List.Clear();
        m_Dic = null;
        m_List = null;
    }

    public SocketClient GetSocket(int handle)
    {
        if (!m_Dic.ContainsKey(handle))
        {
            m_Dic[handle] = new SocketClient(handle);
            m_List.Add(m_Dic[handle]);
        }
        return m_Dic[handle];
    }


    public bool Connected(int handle)
    {
        return (m_Dic.ContainsKey(handle) && null != m_Dic[handle] && m_Dic[handle].Connected);
    }

    public int BeginConnect(string ip, int port, SocketClient.SocketConnectd onConnectedCallBack)
    {
        int handle = ++m_Seek;

        if (!m_Dic.ContainsKey(handle))
        {
            m_Dic[handle] = new SocketClient(handle);
            m_List.Add(m_Dic[handle]);
        }
        m_Dic[handle].BeginConnect(ip, port, onConnectedCallBack);

        return handle;
    }



    public void Send(byte[] data, int protoCode, int handle)
    {
        if (!m_Dic.ContainsKey(handle)) return;
        m_Dic[handle].Send(data, protoCode);
    }

    public void SafeClose(int handle)
    {
        if (!m_Dic.ContainsKey(handle)) return;
        if (m_Dic[handle] != null && m_Dic[handle].Connected)
        {
            Send(null, OP_PLAYER_CLOSE.CODE, handle);
            return;
        }
        Close(handle);
    }

    public void Close(int handle)
    {
        if (!m_Dic.ContainsKey(handle)) return;
        m_Dic[handle].Close(true);
        m_List.Remove(m_Dic[handle]);
        m_Dic.Remove(handle);
    }

}
