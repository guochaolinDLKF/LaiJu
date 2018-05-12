using UnityEngine;
using proto.common;
public class AppDebug
{
    public static void Log(object message)
    {
#if DEBUG_LOG
        LogSystem.Log(message.ToString());
#endif
    }
    public static void ThrowError(object message)
    {

        throw new System.Exception(message.ToString());
    }

    public static void LogError(string message)
    {
#if DEBUG_LOG
        Debug.LogError(message);
#endif
        OP_SYS_LOG_GET proto = new OP_SYS_LOG_GET();
        proto.msg = message.ToString();
        NetWorkSocket.Instance.Send(proto.encode(), OP_SYS_LOG_GET.CODE, GameCtrl.Instance.SocketHandle);
    }

    public static void LogWarning(object message)
    {
#if DEBUG_LOG
        Debug.LogWarning(message);
#endif
    }
}
