//===================================================
//Author      : DRB
//CreateTime  ：3/22/2017 3:55:31 PM
//Description ：主场景视图
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISceneMainView : UISceneViewBase
{
    #region Variable
    [SerializeField]
    private RawImage m_Avatar;

    [SerializeField]
    private Text m_TextNickname;

    [SerializeField]
    private Text m_TextPassportId;

    [SerializeField]
    private Text m_TextRoomCardCount;

    [SerializeField]
    private UIItemCountTip m_MailCount;
    [SerializeField]
    private Button m_BtnShare;

    [SerializeField]
    private Button m_BtnPresent;

    [SerializeField]
    private Button m_BtnInvite;

    [SerializeField]
    private Transform m_GameContainer;

    [SerializeField]
    private Image m_ImgRechargeDouble;
    [SerializeField]
    private Image m_ImgBoy;
    [SerializeField]
    private Image m_ImgGirl;
    [SerializeField]
    private RawImage m_ImgIcon;

    private List<UIItemGame> m_CacheGame = new List<UIItemGame>();

    [SerializeField]
    private Transform m_imgContent;
    [SerializeField]
    private Transform m_pointContent;
    private List<UIItemNoticeThree> m_noticethree = new List<UIItemNoticeThree>();
    Dictionary<int, string> WeChatNumber = new Dictionary<int, string>();
    private List<NoticeThreeEntity> m_notictthreelst;
    [SerializeField]
    private PageView m_pageview;

    private string m_linkUrl = null;
    #endregion

    #region DicNotificationInterests 注册事件
    /// <summary>
    /// 注册事件
    /// </summary>
    /// <returns></returns>
    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, ModelDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add("OnCardCountChanged", OnCardCountChanged);
        dic.Add("OnMailCountChange", OnMailCountChange);
        return dic;
    }
    #endregion

    #region Override
    protected override void OnStart()
    {
        base.OnStart();

        m_TextNickname.SafeSetText(AccountProxy.Instance.CurrentAccountEntity.nickname);
        m_TextPassportId.SafeSetText(AccountProxy.Instance.CurrentAccountEntity.passportId.ToString());
        m_TextRoomCardCount.SafeSetText(AccountProxy.Instance.CurrentAccountEntity.cards.ToString());
        TextureManager.Instance.LoadHead(AccountProxy.Instance.CurrentAccountEntity.avatar, OnLoadAvatarFinishCallBack);

        if (m_MailCount != null)
        {
            m_MailCount.SetUI(InformProxy.Instance.UnreadMailCount);
        }

        if (SystemProxy.Instance.IsOpenWXLogin)
        {
            if (!SystemProxy.Instance.IsInstallWeChat)
            {
                m_BtnShare.gameObject.SetActive(false);
            }
        }
        else
        {
            m_BtnShare.gameObject.SetActive(false);
        }

        if (m_BtnInvite != null)
        {
            m_BtnInvite.gameObject.SetActive(SystemProxy.Instance.IsOpenInvite);
        }

        if (m_BtnPresent != null)
        {
            m_BtnPresent.gameObject.SetActive(SystemProxy.Instance.IsOpenPresent);
        }

        if (m_ImgRechargeDouble != null)
        {
            m_ImgRechargeDouble.gameObject.SetActive(AccountProxy.Instance.CurrentAccountEntity.first_pay == 0);
        }

        if (m_ImgBoy != null)
        {
            m_ImgBoy.gameObject.SetActive(AccountProxy.Instance.CurrentAccountEntity.gender == 1);
        }

        if (m_ImgGirl != null)
        {
            m_ImgGirl.gameObject.SetActive(AccountProxy.Instance.CurrentAccountEntity.gender == 0);
        }
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case ConstDefine.BtnRule:
                UIViewManager.Instance.OpenWindow(UIWindowType.Rule);
                break;
            case ConstDefine.BtnShare:
                UIViewManager.Instance.OpenWindow(UIWindowType.Share);
                break;
            case ConstDefine.BtnSevice:
                UIViewManager.Instance.OpenWindow(UIWindowType.Service);
                break;
            case ConstDefine.BtnSetting:
                UIViewManager.Instance.OpenWindow(UIWindowType.AudioSetting);
                break;
            case ConstDefine.BtnShop:
                UIViewManager.Instance.OpenWindow(UIWindowType.AgentService);
                break;
            case ConstDefine.BtnRecord:
                UIViewManager.Instance.OpenWindow(UIWindowType.Record);
                break;
            case ConstDefine.BtnGame:
                UIViewManager.Instance.OpenWindow(UIWindowType.Match);
                break;
            case ConstDefine.BtnPresent:
                UIViewManager.Instance.OpenWindow(UIWindowType.Present);
                break;
            case ConstDefine.BtnInvite:
                UIViewManager.Instance.OpenWindow(UIWindowType.Invite);
                break;
            case ConstDefine.BtnMessage:
                UIViewManager.Instance.OpenWindow(UIWindowType.Mail);
                break;
            case ConstDefine.BtnSelectRoomViewCreateRoom:
                UIViewManager.Instance.OpenWindow(UIWindowType.CreateRoom);
                break;
            case ConstDefine.BtnSelectRoomViewJoinRoom:
                UIViewManager.Instance.OpenWindow(UIWindowType.JoinRoom);
                break;
            case ConstDefine.BtnSelectRoomViewRefresh:
                SendNotification(ConstDefine.BtnSelectRoomViewRefresh);
                break;
            case ConstDefine.BtnMatch:
                UIViewManager.Instance.OpenWindow(UIWindowType.Match);
                break;
            case "btnMainViewHead":
                SendNotification("btnMainViewHead");
                break;
            case "btn_MyRoom":
                SendNotification("btn_MyRoom");
                break;
            case "btn_feedback":
                UIViewManager.Instance.OpenWindow(UIWindowType.Retroaction);
                break;
            case "btn_notice":
                UIViewManager.Instance.OpenWindow(UIWindowType.Notice);
                break;
            case "btn_chatGroup":
                UIViewManager.Instance.OpenWindow(UIWindowType.ChatGroup);
                break;
            case "btn_welfareActivities":
                UIViewManager.Instance.OpenWindow(UIWindowType.WelfareActivities);
                break;
            case "btn_integral":
                UIViewManager.Instance.OpenWindow(UIWindowType.Integral);
                break;
            case ConstDefine.BtnRealName:
                SendNotification(ConstDefine.BtnRealName);
                break;
            //case "imgJoin":
            //    SendNotification("imgJoin", m_linkUrl);
            //    break;
        }
    }
    #endregion

    #region OnMailCountChange 未读邮件数量变更
    /// <summary>
    /// 未读邮件数量变更
    /// </summary>
    /// <param name="data"></param>
    private void OnMailCountChange(TransferData data)
    {
        int mailCount = data.GetValue<int>("MailCount");
        if (m_MailCount != null)
        {
            m_MailCount.SetUI(mailCount);
        }
    }
    #endregion

    #region OnCardCountChanged 房卡数量变更
    /// <summary>
    /// 房卡数量变更
    /// </summary>
    /// <param name="cardCount"></param>
    private void OnCardCountChanged(TransferData data)
    {
        int cardCount = data.GetValue<int>("CardCount");
        m_TextRoomCardCount.SafeSetText(cardCount.ToString());
    }
    #endregion

    #region OnLoadAvatarFinishCallBack 加载头像完成回调
    /// <summary>
    /// 加载头像完成回调
    /// </summary>
    /// <param name="tex"></param>
    private void OnLoadAvatarFinishCallBack(Texture2D tex)
    {
        if (m_Avatar != null)
        {
            m_Avatar.texture = tex;
        }
    }
    #endregion



    public void SetUI(List<cfg_gameEntity> lst, Action<int> onGameClick)
    {
        if (lst == null) return;
        if (m_GameContainer == null) return;

        for (int i = 0; i < m_CacheGame.Count; ++i)
        {
            UIPoolManager.Instance.Despawn(m_CacheGame[i].transform);
        }
        m_CacheGame.Clear();

        for (int i = 0; i < lst.Count; ++i)
        {
            if (string.IsNullOrEmpty(lst[i].Icon)) continue;
            GameObject go = UIPoolManager.Instance.Spawn("UIItemGame").gameObject;
            go.SetParent(m_GameContainer);
            UIItemGame item = go.GetComponent<UIItemGame>();
            item.SetUI(lst[i].id, lst[i].Icon, onGameClick);
            m_CacheGame.Add(item);
        }
    }

    #region SetIcon 设置图标
    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="url"></param>
    public void SetIcon(string url)
    {
        TextureManager.Instance.LoadHead(url, (Texture2D tex) =>
         {
             if (m_ImgIcon != null && tex != null)
             {
                 m_ImgIcon.texture = tex;
             }
         });
    }
    #endregion


    #region  SetNoticeThree 主界面公告
    public void setNoticeThree(List<NoticeThreeEntity> lst)
    {
        if (lst == null) return;
        if (m_imgContent == null) return;
        for (int i = 0; i < m_noticethree.Count; ++i)
        {
            UIPoolManager.Instance.Despawn(m_noticethree[i].transform);
        }
        for (int i = 0; i < m_pageview.toggleArray.Count; i++)
        {
            UIPoolManager.Instance.Despawn(m_pageview.toggleArray[i].transform);
        }
        m_pageview.toggleArray.Clear();
        m_noticethree.Clear();
        UIItemNoticeThree page = null;
        Toggle itemToggle = null;
        for (int i = 0; i < lst.Count; ++i)
        {
            GameObject imgItem = UIPoolManager.Instance.Spawn("UIItemNoticeThree").gameObject;
            GameObject pointItem = UIPoolManager.Instance.Spawn("UIItemNoticeThreePoint").gameObject;
            imgItem.SetParent(m_imgContent);
            pointItem.SetParent(m_pointContent);
            page = imgItem.GetComponent<UIItemNoticeThree>();
            itemToggle = pointItem.GetComponent<Toggle>();
            itemToggle.group = m_pointContent.gameObject.GetComponent<ToggleGroup>();
            if(i==0)
            {
                itemToggle.isOn = true;
            }
            else
            {
                itemToggle.isOn = false;
            }
            //第五步添加回调参数
            page.SetUI(lst[i].id,lst[i].link_url, lst[i].img_url, lst[i].content, OnClickCallBack, OnClickTextureCallBack);
            if (!WeChatNumber.ContainsKey(lst[i].id))
            {
                WeChatNumber.Add(lst[i].id, lst[i].content);
            }
            
            m_noticethree.Add(page);
            m_pageview.toggleArray.Add(itemToggle);
           
        }
        m_pageview.Init(m_noticethree);
    }


    #endregion

    void OnClickTextureCallBack(string linkurl)
    {
        if (linkurl!=null)
        {
            System.Diagnostics.Process.Start(linkurl);//打开外部链接的API函数
        }
       
    }



    /// <summary>
    /// 点击复制按钮后的回调函数，将按钮所在的脚本id传过来
    /// </summary>
    /// <param name="id"></param>
    void OnClickCallBack(int id)
    {
        string wechatNum;
       
        WeChatNumber.TryGetValue(id, out wechatNum);
        SDK.Instance.CopyTextToClipboard(wechatNum);
        UIViewManager.Instance.ShowTip("已复制到剪贴板");
    }
}
