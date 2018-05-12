//===================================================
//Author      : DRB
//CreateTime  ：6/19/2017 3:04:54 PM
//Description ：系统模块外观
//===================================================
using System.Collections.Generic;
/// <summary>
/// 系统模块外观
/// </summary>
public class SystemFacade : Singleton<SystemFacade> 
{

    private Dictionary<UIWindowType, ISystemCtrl> m_SystemCtrlDic = new Dictionary<UIWindowType, ISystemCtrl>();    //窗口 模块字典

    private AccountCtrl m_AccountCtrl;//账户系统
    private GameCtrl m_SelectGameCtrl;//选择游戏系统
    private RuleCtrl m_RuleCtrl;//规则系统
    private ServiceCtrl m_ServiceCtrl;//客服系统
    private SettingCtrl m_SettingCtrl;//设置系统
    private ShareCtrl m_ShareCtrl;//分享系统
    private RecordCtrl m_RecordCtrl;//战绩系统
    private ShopCtrl m_ShopCtrl;//商店系统
    private PresentCtrl m_PresentCtrl;//送礼系统
    private MaJiangGameCtrl m_MahJongCtrl;//麻将系统
    private ChatCtrl m_ChatCtrl;//聊天系统
    private AudioSettingCtrl m_AudioSettingCtrl;//声音系统
    private NoticeCtrl m_NoticeCtrl;//通知系统
    private MatchCtrl m_MatchCtrl;//比赛系统

    public SystemFacade()
    {
        m_AccountCtrl = new AccountCtrl();
        m_SelectGameCtrl = new GameCtrl();
        m_RuleCtrl = new RuleCtrl();
        m_ServiceCtrl = new ServiceCtrl();
        m_SettingCtrl = new SettingCtrl();
        m_ShareCtrl = new ShareCtrl();
        m_RecordCtrl = new RecordCtrl();
        m_ShopCtrl = new ShopCtrl();
        m_PresentCtrl = new PresentCtrl();
        m_MahJongCtrl = new MaJiangGameCtrl();
        m_ChatCtrl = new ChatCtrl();
        m_AudioSettingCtrl = new AudioSettingCtrl();
        m_NoticeCtrl = new NoticeCtrl();
        m_MatchCtrl = new MatchCtrl();
        RegisterWindow();
    }

    #region RegisterWindow 注册窗口
    /// <summary>
    /// 注册窗口
    /// </summary>
    private void RegisterWindow()
    {
        m_SystemCtrlDic.Add(UIWindowType.Login, m_AccountCtrl);
        m_SystemCtrlDic.Add(UIWindowType.Bind, m_AccountCtrl);
        m_SystemCtrlDic.Add(UIWindowType.PlayerInfo, m_AccountCtrl);
        m_SystemCtrlDic.Add(UIWindowType.CreateRoom, m_SelectGameCtrl);
        m_SystemCtrlDic.Add(UIWindowType.JoinRoom, m_SelectGameCtrl);

        m_SystemCtrlDic.Add(UIWindowType.Rule, m_RuleCtrl);
        m_SystemCtrlDic.Add(UIWindowType.Service, m_ServiceCtrl);
        m_SystemCtrlDic.Add(UIWindowType.Setting, m_SettingCtrl);
        m_SystemCtrlDic.Add(UIWindowType.Share, m_ShareCtrl);
        m_SystemCtrlDic.Add(UIWindowType.Record, m_RecordCtrl);
        m_SystemCtrlDic.Add(UIWindowType.Shop, m_ShopCtrl);
        m_SystemCtrlDic.Add(UIWindowType.Present, m_PresentCtrl);
        m_SystemCtrlDic.Add(UIWindowType.Settle, m_MahJongCtrl);
        m_SystemCtrlDic.Add(UIWindowType.Result, m_MahJongCtrl);
        m_SystemCtrlDic.Add(UIWindowType.Chat, m_ChatCtrl);
        m_SystemCtrlDic.Add(UIWindowType.Micro, m_ChatCtrl);
        m_SystemCtrlDic.Add(UIWindowType.AudioSetting, m_AudioSettingCtrl);
        m_SystemCtrlDic.Add(UIWindowType.Mail, m_NoticeCtrl);
        m_SystemCtrlDic.Add(UIWindowType.Notice, m_NoticeCtrl);

        m_SystemCtrlDic.Add(UIWindowType.Match, m_MatchCtrl);
        m_SystemCtrlDic.Add(UIWindowType.MatchWait, m_MatchCtrl);
        m_SystemCtrlDic.Add(UIWindowType.MatchDetail, m_MatchCtrl);
        m_SystemCtrlDic.Add(UIWindowType.MatchTip, m_MatchCtrl);
        m_SystemCtrlDic.Add(UIWindowType.MatchRankList, m_MatchCtrl);
    }
    #endregion


    public void OpenWindow(UIWindowType type)
    {
        m_SystemCtrlDic[type].OpenView(type);
    }












}
