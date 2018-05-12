using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

/// <summary>
/// HTTP通讯管理
/// </summary>
public class NetWorkHttp : MonoBehaviour {

    #region 单例
    private static NetWorkHttp instance;
    public static NetWorkHttp Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "NetWorkHttp";
                DontDestroyOnLoad(go);
                instance = go.AddComponent<NetWorkHttp>();
            }
            return instance;
        }
    }
    #endregion

    public delegate void CallBack(CallBackArgs args);

    public CallBack OnTokenError;

    private Queue<HttpSendPackage> m_SendQueue = new Queue<HttpSendPackage>(); 

    /// <summary>
    /// 请求超时时间
    /// </summary>
    private const float TIME_OUT = 30f;

    /// <summary>
    /// 最大超时次数
    /// </summary>
    private const int MAX_TIME_OUT_COUNT = 3;


    #region Get请求
    /// <summary>
    /// Get请求
    /// </summary>
    /// <param name="package"></param>
    private void GetUrl(HttpSendPackage package)
    {
        WWW www = new WWW(package.url);
        StartCoroutine(Request(www, package));
    }
    #endregion

    #region Request 请求服务器
    /// <summary>
    /// 请求服务器
    /// </summary>
    /// <param name="www"></param>
    /// <returns></returns>
    private IEnumerator Request(WWW www,HttpSendPackage package)
    {
        LogSystem.Log(www.url);
        CallBackArgs args = new CallBackArgs();
        float timeOut = Time.time;
        float progress = www.progress;
        while (www != null && !www.isDone)
        {
            if (progress < www.progress)
            {
                timeOut = Time.time;
                progress = www.progress;
            }

            if (Time.time - timeOut > TIME_OUT)
            {
                www.Dispose();
                AppDebug.LogWarning("HTTP超时");
                ++package.timeOutCount;
                if (package.timeOutCount >= MAX_TIME_OUT_COUNT)
                {
                    if (package.callBack != null)
                    {
                        args.HasError = true;
                        args.ErrorMsg = "请求超时";
                        package.callBack(args);
                    }
                }
                else
                {
                    if (package.isPost)
                    {
                        PostUrl(package);
                    }
                    else
                    {
                        GetUrl(package);
                    }
                }
                
                yield break;
            }
            yield return null;
        }

        yield return www;
        if (www.error == null)
        {
            AppDebug.Log(www.text);
            if (www.text.Equals("null",StringComparison.OrdinalIgnoreCase))
            {
                if (package.callBack != null)
                {
                    args.HasError = true;
                    args.ErrorMsg = "未请求到数据";
                    package.callBack(args);
                }
            }
            else
            {
                if (package.callBack != null)
                {
                    try
                    {
                        args.HasError = false;
                        LitJson.JsonData jsonData = LitJson.JsonMapper.ToObject(www.text);
                        args.Value = new Ret()
                        {
                            code = jsonData["code"].ToString().ToInt(),
                            data = jsonData["data"],
                            msg = jsonData["msg"].ToString()
                        };

                    }
                    catch
                    {
                        AppDebug.Log(www.text);
                        args.HasError = true;
                        args.ErrorMsg = "数据异常";
                    }
                    finally
                    {
                        package.callBack(args);
                        if (args.Value != null && (args.Value.code == -91017 || args.Value.code == -91018))
                        {
                            if (OnTokenError != null)
                            {
                                OnTokenError(args);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (package.callBack != null)
            {
                args.HasError = true;
                args.ErrorMsg = "网络异常";
                package.callBack(args);
            }
            AppDebug.Log("连接失败" + www.error);
        }
        www.Dispose();
    }
    #endregion

    #region Post请求
    /// <summary>
    /// Post请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="json"></param>
    private void PostUrl(HttpSendPackage packeg)
    {
        WWWForm form = new WWWForm();
        StringBuilder signContent = new StringBuilder();
        foreach (KeyValuePair<string, object> pair in packeg.data)
        {
            string content = pair.Value.ToString();
            signContent.Append(content);
            form.AddField(pair.Key, content);
        }
        //web加密
        string stamp = TimeUtil.GetTimestampMS().ToString();
        string sign = EncryptUtil.Md5(string.Format("{0}{1}{2}{3}", packeg.functionName, signContent.ToString(), stamp,ConstDefine.SIGN));
        form.AddField("sign", sign);
        
        WWW www = new WWW(packeg.url + stamp + "/" + GlobalInit.Instance.CurrentVersion.ToString(), form);
        StartCoroutine(Request(www, packeg));
    }
    #endregion

    #region 客户端调用发送
    /// <summary>
    /// 发送Web数据
    /// </summary>
    /// <param name="url"></param>
    /// <param name="json"></param>
    /// <param name="callback"></param>
    /// <param name="isPost"></param>
    public void SendData(string url,CallBack callback, bool isPost = false, string functionName = "", Dictionary<string,object> dic = null)
    {
        HttpSendPackage package = new HttpSendPackage();
        package.url = url;
        package.callBack = callback;
        package.isPost = isPost;
        package.data = dic;
        package.functionName = functionName;
        m_SendQueue.Enqueue(package);
    }
    #endregion

    private void Update()
    {
        if (m_SendQueue.Count > 0)
        {
            HttpSendPackage package = m_SendQueue.Dequeue();
            if (!package.isPost)
            {
                GetUrl(package);
            }
            else
            {

                PostUrl(package);
            }
        }
    }

    public class HttpSendPackage
    {
        public string url;
        public CallBack callBack;
        public bool isPost;
        public string functionName;
        public Dictionary<string, object> data;
        public int timeOutCount;
    }

    #region Web回调数据
    /// <summary>
    /// Web回调数据
    /// </summary>
    public class CallBackArgs:EventArgs
    {
        /// <summary>
        /// 是否有错
        /// </summary>
        public bool HasError;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg;

        /// <summary>
        /// Json数据
        /// </summary>
        public Ret Value;
    }
    #endregion
}