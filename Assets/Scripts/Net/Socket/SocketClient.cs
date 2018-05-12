//===================================================
//Author      : DRB
//CreateTime  ：8/7/2017 10:54:17 AM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using proto.common;

public class SocketClient 
{
    public class SocketArgs
    {
        public SocketState state;
    }


    public enum SocketState
    {
        /// <summary>
        /// 空闲
        /// </summary>
        Idle,
        /// <summary>
        /// 连接
        /// </summary>
        Connect,
        /// <summary>
        /// 握手
        /// </summary>
        HandShake,
        /// <summary>
        /// 通讯
        /// </summary>
        Communicate,
        /// <summary>
        /// 重连
        /// </summary>
        Reconnect,
        /// <summary>
        /// 关闭
        /// </summary>
        Close,
    }

    #region Event 事件

    public delegate void SocketConnectd(bool isSuccess);
    public delegate void SocketDisconnect(bool isActive);
    public delegate void SocketReconnect();
    /// <summary>
    /// 建立连接完成
    /// </summary>
    public SocketConnectd onConnectComplete;
    /// <summary>
    /// 断开连接
    /// </summary>
    public SocketDisconnect OnDisConnect;
    /// <summary>
    /// 重新连接
    /// </summary>
    public SocketReconnect OnReconnect;



    #endregion

    #region 字段或属性
    private byte[] m_ReceiveBuffer = new byte[2048];//接收缓冲区
    private MemoryStreamExt m_ReceiveMemoryStream = new MemoryStreamExt();//接收消息内存流
    private Queue<byte[]> m_ReceiveQueue = new Queue<byte[]>();//接受消息队列
    private int m_ReceiveCount = 0;//接收长度
    private byte[] m_HeadData = new byte[4];//头数据
    private Queue<byte[]> m_QueueSendData = new Queue<byte[]>();//发送消息队列
    public Socket Socket = null;//客户端套接字
    private const int DATA_HEAD_LENGTH = 4;//头长度
    private const int COMPRESS_LENGTH = 20480;//压缩长度
    public NetworkReachability CurrentNetType = NetworkReachability.NotReachable;//当前网络状态
    private bool m_isClientClose = false;//是否是客户端主动断开连接
    private SocketState m_CurrentState = SocketState.Idle;//当前套接字状态
    public bool isActiveClose;
    public bool isReconnect;

    public OP_SYS_HEART lastHeart;


    public int Handle;
    
    /// <summary>
    /// 当前连接的域名
    /// </summary>
    public string CurrentHost;
    /// <summary>
    /// 当前连接的端口
    /// </summary>
    public int CurrentPort;
    /// <summary>
    /// 是否处于连接状态
    /// </summary>
    public bool Connected
    {
        get
        {
            return (m_CurrentState == SocketState.Communicate || m_CurrentState == SocketState.HandShake);
        }
    }
    #endregion

    private Dictionary<SocketState, SocketStateBase> m_SocketStateDic;

    public SocketClient(int handle)
    {
        Handle = handle;
        m_SocketStateDic = new Dictionary<SocketState, SocketStateBase>();
        m_SocketStateDic.Add(SocketState.Idle, new SocketIdleState(this));
        m_SocketStateDic.Add(SocketState.Connect, new SocketConnectState(this));
        m_SocketStateDic.Add(SocketState.HandShake, new SocketHandShakeState(this));
        m_SocketStateDic.Add(SocketState.Communicate, new SocketCommunicateState(this));
        m_SocketStateDic.Add(SocketState.Reconnect, new SocketReconnectState(this));
        m_SocketStateDic.Add(SocketState.Close, new SocketCloseState(this));
    }

    /// <summary>
    /// 开始连接
    /// </summary>
    /// <param name="host">域名</param>
    /// <param name="port">端口</param>
    /// <param name="onConnectedCallBack">连接完成回调</param>
    public void BeginConnect(string host, int port, SocketConnectd onConnectCompleteCallBack)
    {
        if (m_CurrentState != SocketState.Idle)
        {
            if (onConnectCompleteCallBack != null)
            {
                onConnectCompleteCallBack(false);
            }
            return;
        }
        CurrentHost = host;
        CurrentPort = port;
        onConnectComplete = onConnectCompleteCallBack;
        m_isClientClose = false;
        ChangeState(SocketState.Connect);
    }


    public void OnUpdate()
    {
        while (true)
        {
            if (m_ReceiveCount < 5)
            {
                ++m_ReceiveCount;
                lock (m_ReceiveQueue)
                {
                    if (m_ReceiveQueue.Count > 0)
                    {
                        byte[] buffer = m_ReceiveQueue.Dequeue();
                        byte[] protocodeBuffer = new byte[4];
                        byte[] protoContent = new byte[buffer.Length - 5];
                        bool isCompress = false;
                        using (MemoryStreamExt ms = new MemoryStreamExt(buffer))
                        {
                            isCompress = ms.ReadBool();
                            ms.Read(protocodeBuffer, 0, protocodeBuffer.Length);
                            Array.Reverse(protocodeBuffer);
                            int protoCode = BitConverter.ToInt32(protocodeBuffer, 0);
                            LogSystem.Log("=============================================================收到消息" + protoCode);
                            ms.Read(protoContent, 0, protoContent.Length);
                            if (isCompress)
                            {
                                protoContent = GZipCompress.DeCompress(protoContent);
                            }
                            if (protoCode == OP_PLAYER_CLOSE.CODE)
                            {
                                LogSystem.LogSpecial("服务器返回关闭消息，网络连接断开");
                                Close(true);
                            }
                            else if (protoCode == OP_SYS_HEART.CODE)
                            {
                                lastHeart = OP_SYS_HEART.decode(protoContent);
                            }
                            else
                            {
                                //return;
                                NetDispatcher.Instance.Dispatch(protoCode, protoContent);
                            }
                        }

                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                m_ReceiveCount = 0;
                break;
            }
        }
        m_SocketStateDic[m_CurrentState].OnUpdate();
    }

    public void ChangeState(SocketState state)
    {
        if (m_CurrentState == state) return;

        if (m_SocketStateDic.ContainsKey(state))
        {
            if (m_CurrentState != SocketState.Idle)
            {
                m_SocketStateDic[m_CurrentState].OnExit();
            }
            LogSystem.Log("网络状态切换到" + state.ToString());
            m_CurrentState = state;

            m_SocketStateDic[m_CurrentState].OnEnter();
        }
    }






    

    #region OnCheckSendMassageQueueCallBack 检查队列回调
    /// <summary>
    /// 检查队列回调
    /// </summary>
    private void OnCheckSendMassageQueueCallBack()
    {
        lock (m_QueueSendData)
        {
            if (m_QueueSendData.Count > 0)
            {
                lock (m_QueueSendData)
                {
                    SendMsg(m_QueueSendData.Dequeue());
                }
            }
        }
    }
    #endregion

    #region SendMsg 发送信息
    /// <summary>
    /// 真正的发送信息
    /// </summary>
    /// <param name="data"></param>
    private void SendMsg(byte[] data)
    {
        if (Socket == null) return;
        try
        {
            IAsyncResult res = this.Socket.BeginSend(data, 0, data.Length, SocketFlags.None, SendMsgCallBack, this.Socket);
        }
        catch
        {

        }
        //bool success = res.AsyncWaitHandle.WaitOne(1000, true);
        //AppDebug.Log(success ? "发送成功" : "发送失败");
        //if (!success)
        //{
        //    Debug.Log("发送消息失败，断开连接");
        //    Close(false);
        //}
    }
    #endregion

    #region SendMsgCallBack 发送信息回调
    /// <summary>
    /// 发送信息回调
    /// </summary>
    /// <param name="ar"></param>
    private void SendMsgCallBack(IAsyncResult ar)
    {
        this.Socket.EndSend(ar);
        OnCheckSendMassageQueueCallBack();
    }
    #endregion

    private byte[] m_LastData;

    #region Send 客户端调用发送（加入到发送队列）
    /// <summary>
    /// 客户端调用发送（加入到发送队列）
    /// </summary>
    /// <param name="data"></param>
    public void Send(byte[] data, int protoCode)
    {
        if (this.Socket == null || !this.Socket.Connected)
        {
            return;
        }

        LogSystem.LogCore("============================================客户端发送" + protoCode.ToString());

        byte[] pSendMassage = MakeDatas(data, protoCode);
        lock (m_QueueSendData)
        {
            m_QueueSendData.Enqueue(pSendMassage);
        }
        OnCheckSendMassageQueueCallBack();
    }
    #endregion

    #region MakeDatas 封装数据包
    /// <summary>
    /// 封装数据包
    /// </summary>
    /// <param name="data">要发送的数据</param>
    /// <returns>封装好的数据</returns>
    private byte[] MakeDatas(byte[] data, int protoCode)
    {
        int leng = 0;
        bool isCompress;
        if (data == null)
        {
            leng = 5;
            isCompress = false;
        }
        else
        {
            data = EncryptUtil.NetEncrypt(data, SystemProxy.Instance.NetKey, SystemProxy.Instance.NetCorrected);
            isCompress = data.Length >= COMPRESS_LENGTH;
            if (isCompress)
            {
                data = GZipCompress.Compress(data);
            }
            leng = data.Length + 5;
        }
        byte[] head = new byte[4];
        head[0] = (byte)((leng & 0xFF000000) >> 24);
        head[1] = (byte)((leng & 0x00FF0000) >> 16);
        head[2] = (byte)((leng & 0x0000FF00) >> 8);
        head[3] = (byte)((leng & 0x000000FF));

        byte[] protoCodeBuffer = BitConverter.GetBytes(protoCode);
        Array.Reverse(protoCodeBuffer);

        using (MemoryStreamExt ms = new MemoryStreamExt())
        {
            ms.Write(head, 0, 4);
            ms.WriteBool(isCompress);
            ms.Write(protoCodeBuffer, 0, protoCodeBuffer.Length);
            if (data != null)
            {
                ms.Write(data, 0, data.Length);
            }
            return ms.ToArray();
        }
    }
    #endregion



    #region ReceiveMassage 接收数据
    /// <summary>
    /// 接收数据
    /// </summary>
    public void ReceiveMessage()
    {
        Socket.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, Socket);
    }
    #endregion

    #region ReceiveCallBack 接收数据回调
    /// <summary>
    /// 接收数据回调
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCallBack(IAsyncResult ar)
    {
        if (Socket == null || !Socket.Connected)
        {
            //Debug.Log("接收数据回调返回");
            return;
        }
        try
        {
            int nLength = Socket.EndReceive(ar);
            //Debug.Log("从套接字缓冲区读取了" + nLength.ToString() + "长度的数据");
            if (nLength > 0)
            {
                m_ReceiveMemoryStream.Position = m_ReceiveMemoryStream.Length;
                m_ReceiveMemoryStream.Write(m_ReceiveBuffer, 0, nLength);



                if (m_ReceiveMemoryStream.Length > DATA_HEAD_LENGTH)
                {
                    while (true)
                    {
                        m_ReceiveMemoryStream.Position = 0;

                        m_ReceiveMemoryStream.Read(m_HeadData, 0, DATA_HEAD_LENGTH);
                        int currentMsgLen = ((m_HeadData[0] & 0xff) << 24) + ((m_HeadData[1] & 0xff) << 16) + ((m_HeadData[2] & 0xff) << 8) + (m_HeadData[3] & 0xff);
                        int currentFullMsgLen = DATA_HEAD_LENGTH + currentMsgLen;
                        if (m_ReceiveMemoryStream.Length >= currentFullMsgLen) // 说明至少接收到了一个整包
                        {
                            AppDebug.Log("客户端收到了一个" + currentFullMsgLen + "长度的消息");
                            byte[] buffer = new byte[currentMsgLen];
                            m_ReceiveMemoryStream.Position = DATA_HEAD_LENGTH;
                            m_ReceiveMemoryStream.Read(buffer, 0, currentMsgLen);
                            buffer = EncryptUtil.NetEncrypt(buffer, SystemProxy.Instance.NetKey, SystemProxy.Instance.NetCorrected);
                            m_ReceiveQueue.Enqueue(buffer);


                            //===============处理剩余字节数组======================

                            int remainLen = (int)m_ReceiveMemoryStream.Length - currentFullMsgLen;
                            if (remainLen > 0)
                            {
                                m_ReceiveMemoryStream.Position = currentFullMsgLen;
                                byte[] remainBuffer = new byte[remainLen];
                                m_ReceiveMemoryStream.Read(remainBuffer, 0, remainLen);
                                m_ReceiveMemoryStream.Position = 0;
                                m_ReceiveMemoryStream.SetLength(0);

                                m_ReceiveMemoryStream.Write(remainBuffer, 0, remainBuffer.Length);
                                remainBuffer = null;
                            }
                            else // 没有剩余字节
                            {
                                m_ReceiveMemoryStream.Position = 0;
                                m_ReceiveMemoryStream.SetLength(0);
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                ReceiveMessage();
            }
            else
            {
                AppDebug.LogWarning(string.Format("{0}断开连接", m_isClientClose ? "客户端断的" : "服务器断的"));
                Close(false);
            }
        }
        catch (Exception ex)
        {
            AppDebug.LogWarning(string.Format("{0}断开连接,{1}", m_isClientClose ? "客户端断的" : "服务器断的", ex));
            Close(false);
        }
    }
    #endregion

    #region Close 关闭套接字
    /// <summary>
    /// 关闭套接字
    /// </summary>
    public void Close(bool isActive = true)
    {
        isActiveClose = isActive;
        ChangeState(SocketState.Close);
    }
    #endregion

    #region ClearMessageQueue 清空消息队列
    /// <summary>
    /// 清空消息队列
    /// </summary>
    public void ClearMessageQueue()
    {
        m_QueueSendData.Clear();
        m_ReceiveQueue.Clear();
        m_ReceiveMemoryStream.Position = 0;
        m_ReceiveMemoryStream.SetLength(0);
    }
    #endregion
}
