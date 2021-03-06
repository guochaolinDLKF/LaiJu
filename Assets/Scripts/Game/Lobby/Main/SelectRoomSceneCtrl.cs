//===================================================
//Author      : DRB
//CreateTime  ：3/22/2017 3:42:28 PM
//Description ：选择房间场景控制器
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using com.oegame.mahjong.protobuf;
using UnityEngine;

public class SelectRoomSceneCtrl : SceneCtrlBase
{
    private UISceneMainView m_UISceneMainView;

    private static List<UIWindowType> m_windowNames = new List<UIWindowType> { UIWindowType.AgreeMent, UIWindowType.Mail, UIWindowType.WelfareActivities };

    private const string m_AutoWindowName = "player";


    #region MonoBehaviour
    protected override void OnAwake()
    {

        UIDispatcher.Instance.AddEventListener(ConstDefine.BtnMyRoom, OnBtnMyRoomClick);//主界面我的房间按钮点击
        UIDispatcher.Instance.AddEventListener(ConstDefine.BtnSelectRoomViewRefresh, OnBtnSelectRoomViewRefreshClick);//主界面刷新按钮点击
        DelegateDefine.Instance.OnAutoJoinRoom += OnAutoJoinRoomCallBack;//自动加入房间回调


    }

    protected override void BeforeOnDestroy()
    {
        UIDispatcher.Instance.RemoveEventListener(ConstDefine.BtnMyRoom, OnBtnMyRoomClick);//主界面刷新按钮点击
        UIDispatcher.Instance.RemoveEventListener(ConstDefine.BtnSelectRoomViewRefresh, OnBtnSelectRoomViewRefreshClick);//主界面刷新按钮点击
        DelegateDefine.Instance.OnAutoJoinRoom -= OnAutoJoinRoomCallBack;//自动加入房间回调
    }

    protected override void OnStart()
    {
        base.OnStart();
        AudioBackGroundManager.Instance.Play("bgm_main");
        if (DelegateDefine.Instance.OnSceneLoadComplete != null)
        {
            DelegateDefine.Instance.OnSceneLoadComplete();
        }
        GameObject go = UIViewManager.Instance.LoadSceneUIFromAssetBundle(UIViewManager.SceneUIType.Main);
#if IS_TAILAI || IS_DAZHONG || IS_GUGENG || IS_BAODING
        UIViewManager.Instance.OpenWindow(UIWindowType.Ranking);
#endif
        m_UISceneMainView = go.GetComponent<UISceneMainView>();
        List<cfg_gameEntity> lstEntity = cfg_gameDBModel.Instance.GetList();
        if (lstEntity != null)
        {
            m_UISceneMainView.SetUI(lstEntity, OnGameClick);
            RequestNoticeThree();
        }

        if (GlobalInit.Instance.IsAutoJoin)
        {
            OnAutoJoinRoomCallBack(GlobalInit.Instance.InviteRoomId, GlobalInit.Instance.ParentId);
        }
        else
        {
            GameCtrl.Instance.RequestRebuildRoom();
        }

        AccountCtrl.Instance.RequestCards();

        StartCoroutine(BeginRequestInform());

        OnAutoOpenWindow();//自动打开窗口

        RequestIcon();
        
    }
    #endregion

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AccountCtrl.Instance.ChangeUser();
        }
    }

    /// <summary>
    /// 自动打开窗口
    /// </summary>
    private void OnAutoOpenWindow()
    {
        if (m_windowNames.Count == 0) return;
        int rule = PlayerPrefs.GetInt(m_AutoWindowName);
        int index = rule == 0 ? 0 : 1;        
        UIViewManager.Instance.OpenWindow(m_windowNames[index]);
        UIViewManager.Instance.OnWindowClose += OnWindowClose;
    }

    private void OnWindowClose(string windowName)
    {        
        for (int i = 0; i < m_windowNames.Count; i++)
        {
            if (windowName.Equals(m_windowNames[i].ToString().ToLower()))
            {
                if (i < m_windowNames.Count - 1)
                {
                    UIViewManager.Instance.OpenWindow(m_windowNames[i + 1]);
                }
                else
                {
                    UIViewManager.Instance.OnWindowClose -= OnWindowClose;
                    PlayerPrefs.SetInt(m_AutoWindowName, 1);
                    m_windowNames.Clear();
                }
               // m_windowNames.Remove(m_windowNames[i]);
            }
        }
    }


    /// <summary>
    /// 游戏图标点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnGameClick(int gameId)
    {
        GameCtrl.Instance.OpenCreateRoomView(gameId);
    }

    #region OnAutoJoinRoomCallBack 自动加入房间回调
    /// <summary>
    /// 自动加入房间回调
    /// </summary>
    /// <param name="roomId"></param>
    private void OnAutoJoinRoomCallBack(int roomId, int parentId)
    {
        Debug.Log("自动进入房间" + roomId);
        GlobalInit.Instance.IsAutoJoin = false;

        GameCtrl.Instance.RequestJoinRoom(roomId);

        if (parentId > 0)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
            dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
            dic["parentId"] = parentId;

            NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrUrlBind, null, true, ConstDefine.HTTPFuncUrlBind, dic);
        }
    }
    #endregion

    #region OnBtnSelectRoomViewRefreshClick 主界面刷新按钮点击
    /// <summary>
    /// 主界面刷新按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnSelectRoomViewRefreshClick(object[] obj)
    {
        AccountCtrl.Instance.RequestCards();
    }
    #endregion

    #region OnBtnMyRoomClick 我的房间按钮点击
    /// <summary>
    /// 我的房间按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnMyRoomClick(object[] obj)
    {
        UIViewManager.Instance.OpenWindow(UIWindowType.MyRoom);
    }
    #endregion


    #region BeginRequestInform 请求消息
    /// <summary>
    /// 请求消息
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeginRequestInform()
    {
        if (AccountProxy.Instance.CurrentAccountEntity == null) yield break;
        yield return new WaitForSeconds(1.0f);
        while (true)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            AccountEntity entity = AccountProxy.Instance.CurrentAccountEntity;
            dic["passportId"] = entity.passportId;
            dic["token"] = entity.token;
            NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrInform, OnRequestInformCallBack, true, ConstDefine.HTTPFuncInform, dic);
            yield return new WaitForSeconds(10.0f);
        }
    }
    #endregion

    #region OnRequestInformCallBack 请求消息回调
    /// <summary>
    /// 请求消息回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestInformCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {

        }
        else
        {
            if (args.Value.code < 0)
            {
                return;
            }

            InformEntity entity = LitJson.JsonMapper.ToObject<InformEntity>(args.Value.data.ToJson());
            InformProxy.Instance.SetUnreadMailCount(entity.mailCount);

            if (!string.IsNullOrEmpty(entity.inform))
            {
                NoticeCtrl.Instance.ShowInform(entity.inform);
            }
        }
    }
    #endregion

    #region RequestIcon 请求游戏图标
    /// <summary>
    /// 请求游戏图标
    /// </summary>
    private void RequestIcon()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/service/", OnRequestIconCallBack, true, "service", dic);
    }
    #endregion

    #region OnRequestIconCallBack 请求游戏图标回调
    /// <summary>
    /// 请求游戏图标回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestIconCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            UIViewManager.Instance.ShowMessage("提示", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                UIViewManager.Instance.ShowMessage("提示", args.Value.msg);
                return;
            }
            if (args.Value.data == null || args.Value.data.Count == 0) return;
            string logo = args.Value.data["logo"].ToString();
            m_UISceneMainView.SetIcon(logo);
        }
    }
    #endregion


    #region RequestNoticeThree 请求主界面的公告
    /// <summary>
    /// 请求主界面的公告
    /// </summary>
    public void RequestNoticeThree()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
        dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/noticeThree/", OnRequestNoticeThreeCallBack, true, "noticeThree", dic);
    }
    #endregion

    #region OnRequestNoticeThreeCallBack 请求主界面公告回调
    /// <summary>
    /// 请求主界面公告回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestNoticeThreeCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            UIViewManager.Instance.ShowMessage("提示", "网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                UIViewManager.Instance.ShowMessage("错误", args.Value.msg);
                return;
            }
            List<NoticeThreeEntity> lst = LitJson.JsonMapper.ToObject<List<NoticeThreeEntity>>(args.Value.data.ToJson());
            Debug.Log(args.Value.data.ToJson()+"++++++++++++++++++++++");
            
            m_UISceneMainView.setNoticeThree(lst);
           
        }
    }

    #endregion


    private int m_ClickCount = 0;

    private long m_PrevClickTimeStamp;

    private const int CLICK_SPACE = 400;

    private const int CLICK_COUNT = 10;

    protected override void OnPlayerClick()
    {
        base.OnPlayerClick();
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null) return;
            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name.Equals("healthgame", StringComparison.CurrentCultureIgnoreCase))
            {
                if (TimeUtil.GetTimestampMS() - m_PrevClickTimeStamp > CLICK_SPACE)
                {
                    m_ClickCount = 1;
                }
                else
                {
                    ++m_ClickCount;
                    if (m_ClickCount == CLICK_COUNT)
                    {
                        m_ClickCount = 0;
                        SystemProxy.Instance.IsTestMode = !SystemProxy.Instance.IsTestMode;
                        UIViewManager.Instance.ShowTip(SystemProxy.Instance.IsTestMode ? "您已进入测试模式" : "您已退出测试模式");
                    }
                }

                m_PrevClickTimeStamp = TimeUtil.GetTimestampMS();
            }
        }
    }
}
