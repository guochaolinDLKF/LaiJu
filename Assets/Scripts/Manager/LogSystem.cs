//===================================================
//Author      : DRB
//CreateTime  ：9/20/2017 11:43:07 AM
//Description ：日志系统
//===================================================
using proto.common;
using UnityEngine;

/// <summary>
/// 日志系统
/// </summary>
public static class LogSystem
{
    #region Enum
    #region LogMode 日志状态（部署模式）
    /// <summary>
    /// 日志状态（部署模式）
    /// </summary>
    public enum LogMode
    {
        Develop,      //开发模式（输出所有日志内容）
        Special,     //特殊输出模式
        Deploy,       //部署模式（只输出最核心日志信息）
        Stop          //停止输出模式（不输出任何日志信息）
    };
    #endregion

    #region Level 调试信息的等级
    /// <summary>
    /// 调试信息的等级
    /// </summary>
    public enum Level
    {
        /// <summary>
        /// 普通
        /// </summary>
        Normal,
        /// <summary>
        /// 警告
        /// </summary>
        Warning,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 特殊日志
        /// </summary>
        Speacial,
        /// <summary>
        /// 核心
        /// </summary>
        Core,
    }
    #endregion
    #endregion

    #region Members
    /// <summary>
    /// 当前日志模式
    /// </summary>
    public static LogMode CurrentLogMode { get; set; }
    #endregion

    #region Static Constructor
    static LogSystem()
    {
#if DEBUG_MODE
        CurrentLogMode = LogMode.Develop;
#else
        CurrentLogMode = LogMode.Deploy;
#endif

#if !DEBUG_LOG
        CurrentLogMode = LogMode.Stop;
#endif
    }
    #endregion

    #region Log 打印日志
    /// <summary>
    /// 打印日志
    /// </summary>
    /// <param name="message"></param>
    public static void Log(string message)
    {
        Log(message, Level.Normal);
    }
    #endregion

    #region LogSpecial 打印特殊日志
    /// <summary>
    /// 打印特殊日志
    /// </summary>
    /// <param name="message"></param>
    public static void LogSpecial(string message)
    {
        Log(message, Level.Speacial);
    }
    #endregion

    #region LogCore 打印核心日志
    /// <summary>
    /// 打印核心日志
    /// </summary>
    /// <param name="message"></param>
    public static void LogCore(string message)
    {
        Log(message, Level.Core);
    }
    #endregion

    #region LogWarning 打印警告日志
    /// <summary>
    /// 打印警告日志
    /// </summary>
    /// <param name="message"></param>
    public static void LogWarning(string message)
    {
        Log(message, Level.Warning);
    }
    #endregion

    #region LogError 打印错误日志
    /// <summary>
    /// 打印错误日志
    /// </summary>
    /// <param name="message"></param>
    public static void LogError(string message)
    {
        Log(message, Level.Error);
    }
    #endregion

    #region Log 处理日志
    /// <summary>
    /// 处理日志
    /// </summary>
    /// <param name="message"></param>
    /// <param name="level"></param>
    public static void Log(string message, Level level)
    {
#if !DEBUG_LOG
        return;
#endif
        switch (CurrentLogMode)
        {
            case LogMode.Develop:

                switch (level)
                {
                    case Level.Error:
                        Debug.LogError(message);
                        SendLog(message);
                        break;
                    case Level.Warning:
                        Debug.LogWarning(message);
                        break;
                    default:
                        Debug.Log(message);
                        break;
                }
                break;
            case LogMode.Special:

                switch (level)
                {
                    case Level.Speacial:
                        Debug.LogWarning(message);
                        SendLog(message);
                        break;
                    case Level.Error:
                        Debug.LogError(message);
                        SendLog(message);
                        break;
                }

                break;
            case LogMode.Deploy:
                switch (level)
                {
                    case Level.Core:
                        Debug.Log(message);
                        SendLog(message);
                        break;
                    case Level.Error:
                        Debug.LogError(message);
                        SendLog(message);
                        break;
                }
                break;
            case LogMode.Stop:
                break;
            default:
                break;
        }
    }
    #endregion

    #region SendLog 发送日志到服务器
    /// <summary>
    /// 发送日志到服务器
    /// </summary>
    /// <param name="message">日志内容</param>
    private static void SendLog(string message)
    {
        OP_SYS_LOG_GET proto = new OP_SYS_LOG_GET();
        proto.msg = message.ToString();
        NetWorkSocket.Instance.Send(proto.encode(), OP_SYS_LOG_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion
}
