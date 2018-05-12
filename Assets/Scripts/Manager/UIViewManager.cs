//===================================================
//Author      : DRB
//CreateTime  ：7/4/2016 1:23:24 AM
//Description ：UI视图管理器
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIViewManager : Singleton<UIViewManager>
{
    #region SceneUIType 场景UI类型
    /// <summary>
    /// 场景UI类型
    /// </summary>
    public enum SceneUIType
    {
        None,
        /// <summary>
        /// 初始化
        /// </summary>
        Init,
        /// <summary>
        /// 登录
        /// </summary>
        Login,
        /// <summary>
        /// 加载
        /// </summary>
        Loading,
        /// <summary>
        /// 选择房间
        /// </summary>
        Main,
        /// <summary>
        /// 麻将
        /// </summary>
        MaJiang3D,
        /// <summary>
        /// 牛牛
        /// </summary>
        NiuNiu2D,
        /// <summary>
        /// 斗地主
        /// </summary>
        DLandlord,
        /// <summary>
        /// 炸金花
        /// </summary>
        ZhaJH,
        /// <summary>
        /// 牌九
        /// </summary>
        PaiJiu3D,
        /// <summary>
        /// 斗地主
        /// </summary>
        DouDiZhu,
        /// <summary>
        /// 聚友挤旮拉
        /// </summary>
        JuYou3D,
        /// <summary>
        /// 骨牌
        /// </summary>
        GuPaiJiu,
        /// <summary>
        /// 跑得快
        /// </summary>
        PaoDeKuai2D,
        /// <summary>
        /// 十三张
        /// </summary>
        ShiSanZhang,
    }
    #endregion

    #region Delegate
    /// <summary>
    /// 加载完毕委托原型
    /// </summary>
    /// <param name="go"></param>
    public delegate void OnLoadComplete(GameObject go);
    /// <summary>
    /// 初始化完毕委托原型
    /// </summary>
    public delegate void OnInitComplete();

    public delegate void OnWindowCloseHandler(string windowName);
    public OnWindowCloseHandler OnWindowClose;
    #endregion

    #region Members
    /// <summary>
    /// 当前场景UI
    /// </summary>
    public UISceneViewBase CurrentUIScene;
    //模块控制器字典
    private Dictionary<UIWindowType, ISystemCtrl> m_SystemCtrlDic = new Dictionary<UIWindowType, ISystemCtrl>();
    //等待界面
    private UIWaitView m_UIWaitView;
    //消息窗口
    private UIMessageView m_MessageView;
    //断线重连界面
    private UIReconnectView m_UIReconnectView;
    //提示窗口
    private UITipView m_UITipView;
    //场景UI缓存列表
    private Dictionary<SceneUIType,UISceneViewBase> m_UISceneDic = new Dictionary<SceneUIType, UISceneViewBase>();

    #endregion

    #region Constructor
    public UIViewManager()
    {
        m_SystemCtrlDic.Add(UIWindowType.Login,AccountCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.Bind, AccountCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.PlayerInfo, AccountCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.Invite, AccountCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.CreateRoom, GameCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.JoinRoom, GameCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.MyRoom, GameCtrl.Instance);

        m_SystemCtrlDic.Add(UIWindowType.Rule, RuleCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.Service, ServiceCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.Setting, SettingCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.Share, ShareCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.Record, RecordCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.OwnerRecord, RecordCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.Shop, ShopCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.Present, PresentCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.Chat, ChatCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.Micro, ChatCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.ChatGroup, ChatGroupCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.AudioSetting, AudioSettingCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.Mail,NoticeCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.Notice, NoticeCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.AgreeMent, AgreeMentCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.AgentService, AgentServiceCtrl.Instance);

        m_SystemCtrlDic.Add(UIWindowType.Retroaction, RetroactionCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.Match, MatchCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.MatchWait, MatchCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.MatchDetail, MatchCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.MatchTip, MatchCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.MatchRankList, MatchCtrl.Instance);

        m_SystemCtrlDic.Add(UIWindowType.Ranking, RankingCtrl.Instance);

        m_SystemCtrlDic.Add(UIWindowType.Settle, MaJiangGameCtrl.Instance);

        m_SystemCtrlDic.Add(UIWindowType.WelfareActivities, WelfareActivitiesCtrl.Instance);
        m_SystemCtrlDic.Add(UIWindowType.Integral, IntegralCtrl.Instance);

        m_SystemCtrlDic.Add(UIWindowType.RealName,RealNameCtrl.Instance);
    }
    #endregion

    #region Public Function
    #region OpenWindow 打开窗口
    /// <summary>
    /// 打开窗口
    /// </summary>
    /// <param name="type"></param>
    public void OpenWindow(UIWindowType type)
    {
        AppDebug.Log("打开窗口" + type.ToString());
        m_SystemCtrlDic[type].OpenView(type);
    }
    #endregion

    #region LoadSceneUIFromResouces 同步加载场景UI(Resources)
    /// <summary>
    /// 同步加载场景UI(Resources)
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject LoadSceneUIFromResouces(SceneUIType type, OnInitComplete onInitComplete = null)
    {
        GameObject obj = ResourcesManager.Instance.Load(ResourceType.UIScene, string.Format("UI_Root_{0}", type));
        CurrentUIScene = obj.GetComponent<UISceneViewBase>();
        CurrentUIScene.OnInitComplete = onInitComplete;
        return obj;
    }
    #endregion

    #region LoadSceneUIFromAssetBundle 同步加载场景UI(Assetbundle)
    /// <summary>
    /// 同步加载场景UI(Assetbundle)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="onInitComplete"></param>
    /// <returns></returns>
    public GameObject LoadSceneUIFromAssetBundle(SceneUIType type, OnInitComplete onInitComplete = null)
    {
        foreach (KeyValuePair<SceneUIType, UISceneViewBase> pair in m_UISceneDic)
        {
            if (pair.Value != null)
            {
                pair.Value.gameObject.SetActive(false);
            }
        }

        GameObject ret = null;
        if (m_UISceneDic.ContainsKey(type) && m_UISceneDic[type] != null)
        {
            CurrentUIScene = m_UISceneDic[type];
            CurrentUIScene.gameObject.SetActive(true);
            ret = CurrentUIScene.gameObject;
        }
        else
        {
            string strUIName = string.Format("ui_root_{0}", type.ToString().ToLower());
            string path = string.Format("download/{0}/prefab/uiprefab/uiscenes/{1}.drb", ConstDefine.GAME_NAME, strUIName);
            Debug.Log("打开场景UI: " + strUIName);
            float time = Time.realtimeSinceStartup;
            ret = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(path, strUIName);
            Debug.Log("加载场景UI资源耗时" + (Time.realtimeSinceStartup - time).ToString() + "秒");
            ret = UnityEngine.Object.Instantiate(ret);
            CurrentUIScene = ret.GetComponent<UISceneViewBase>();
            if (CurrentUIScene.persistenceType == UIScenePersistenceType.LoadSceneHide)
            {
                UnityEngine.Object.DontDestroyOnLoad(ret);
            }
            
            m_UISceneDic[type] = CurrentUIScene;
            CurrentUIScene.OnInitComplete = onInitComplete;
        }
        return ret;
    }
    #endregion

    #region LoadSceneUIAsync 异步加载场景UI(Assetbundle)
    /// <summary>
    /// 异步加载场景UI(Assetbundle)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="onLoadComplete">加载完毕回调</param>
    /// <param name="onInitComplete">场景UI初始化完毕回调</param>
    public void LoadSceneUIAsync(SceneUIType type, OnLoadComplete onLoadComplete,OnInitComplete onInitComplete = null)
    {
        string strUIName = string.Format("ui_root_{0}", type.ToString().ToLower());
        string path = string.Format("download/{0}/prefab/uiprefab/uiscenes/{1}.drb", ConstDefine.GAME_NAME, strUIName);
        Debug.Log("打开场景UI: " + strUIName);

        AssetBundleManager.Instance.LoadOrDownload(path, strUIName, (GameObject go) =>
         {
             Debug.Log("场景UI资源加载完毕");
             go = UnityEngine.Object.Instantiate(go);
             CurrentUIScene = go.GetComponent<UISceneViewBase>();
             CurrentUIScene.OnInitComplete = onInitComplete;
             if (onLoadComplete != null)
             {
                 onLoadComplete(go);
             }
         });
    }
    #endregion

    //public GameObject LuaLoadSceneUI(string prefabName)
    //{
    //    string strUIName = prefabName.ToString().ToLower();
    //    string path = string.Format("download/{0}/prefab/xluauiprefab/{1}.drb", ConstDefine.GAME_NAME, strUIName);
    //    GameObject go = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(path, strUIName);
    //    go = UnityEngine.Object.Instantiate(go);
    //    go.AddComponent<LuaViewBehaviour>();
    //    CurrentUIScene = go.GetOrCreatComponent<UISceneViewBase>();
    //    return go;
    //}

    #region LoadItemAsync 异步加载UI项预制体资源
    /// <summary>
    /// 异步加载UI项预制体资源
    /// </summary>
    /// <param name="uiItemName"></param>
    /// <param name="onLoadComplete"></param>
    public void LoadItemAsync(string uiItemName, OnLoadComplete onLoadComplete)
    {
        uiItemName = uiItemName.ToLower();
        string path = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb",ConstDefine.GAME_NAME,uiItemName);        
        AssetBundleManager.Instance.LoadOrDownload(path, uiItemName, (GameObject go)=> 
        {
            if (onLoadComplete != null)
            {
                onLoadComplete(go);
            }
        });
    }

    #region LoadItem 同步加载UI项预制体资源
    /// <summary>
    /// 同步加载UI项预制体资源
    /// </summary>
    /// <param name="uiItemName"></param>
    /// <returns></returns>
    public GameObject LoadItem(string uiItemName)
    {
        uiItemName = uiItemName.ToLower();
        string path = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, uiItemName);
        return AssetBundleManager.Instance.LoadAssetBundle<GameObject>(path,uiItemName);
    }
    #endregion
    #endregion

    #region ShowWait 显示等待界面
    /// <summary>
    /// 显示等待界面
    /// </summary>
    public void ShowWait()
    {
        if (m_UIWaitView == null)
        {
            GameObject go = ResourcesManager.Instance.Load(ResourceType.UIWindow, "pan_Wait", true, true);
            m_UIWaitView = go.GetComponent<UIWaitView>();
            m_UIWaitView.gameObject.SetParent(UIViewManager.Instance.CurrentUIScene.Container_Center);
            Canvas canvas = m_UIWaitView.GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 1111;
        }
        else
        {
            m_UIWaitView.gameObject.SetActive(true);
        }
    }
    #endregion

    #region CloseWait 关闭等待界面
    /// <summary>
    /// 关闭等待界面
    /// </summary>
    public void CloseWait()
    {
        if (m_UIWaitView != null)
        {
            m_UIWaitView.gameObject.SetActive(false);
        }
    }
    #endregion

    #region ShowMessage 显示消息窗口
    /// <summary>
    /// 显示消息窗口
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="message">内容</param>
    /// <param name="type">类型</param>
    /// <param name="okAction">确定回调</param>
    /// <param name="cancelAction">取消回调</param>
    public void ShowMessage(string title, string message, MessageViewType type = MessageViewType.Ok, Action okAction = null, Action cancelAction = null, float countDown = 0.0f, AutoClickType autoType = AutoClickType.None)
    {
        if (m_MessageView == null)
        {
            GameObject go = ResourcesManager.Instance.Load(ResourceType.UIWindow, "pan_Message");
            m_MessageView = go.GetOrCreatComponent<UIMessageView>();
        }
        m_MessageView.gameObject.SetParent(CurrentUIScene.Container_Center, true);
        m_MessageView.gameObject.transform.localPosition = Vector3.zero;
        m_MessageView.gameObject.transform.localScale = Vector3.one;
        m_MessageView.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        Canvas canvas = m_MessageView.GetComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 1000;
        LogSystem.Log("显示消息窗口:" + message);
        m_MessageView.Show(title, message, countDown, autoType, type, okAction, cancelAction);
    }
    #endregion

    #region ShowReconnectView 显示断线重连界面
    /// <summary>
    /// 显示断线重连界面
    /// </summary>
    public void ShowReconnectView()
    {
        if (m_UIReconnectView == null)
        {
            GameObject go = ResourcesManager.Instance.Load(ResourceType.UIWindow,"pan_Reconnect");
            go.SetParent(CurrentUIScene.Container_Center,true);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

            Canvas canvas = go.GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 888;

            m_UIReconnectView = go.GetOrCreatComponent<UIReconnectView>();
        }
        m_UIReconnectView.Show();
    }
    #endregion

    #region CloseReconnectView 关闭断线重连界面
    /// <summary>
    /// 关闭断线重连界面
    /// </summary>
    public void CloseReconnectView()
    {
        if (m_UIReconnectView == null) return;

        m_UIReconnectView.Close();
    }
    #endregion

    #region ClearWindow 清空窗口
    /// <summary>
    /// 清空窗口
    /// </summary>
    public void ClearWindows()
    {
        UIViewUtil.Instance.ClearWindows();
    }
    #endregion

    #region ShowTip 显示提示
    /// <summary>
    /// 显示提示
    /// </summary>
    /// <param name="content"></param>
    public void ShowTip(string content)
    {
        if (m_UITipView == null)
        {
            GameObject go = ResourcesManager.Instance.Load(ResourceType.UIWindow, "pan_Tip");
            go.SetParent(CurrentUIScene.Container_Center, true);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

            Canvas canvas = go.GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 1999;

            m_UITipView = go.GetOrCreatComponent<UITipView>();
        }

        m_UITipView.ShowTip(content);
    }
    #endregion
    #endregion
}