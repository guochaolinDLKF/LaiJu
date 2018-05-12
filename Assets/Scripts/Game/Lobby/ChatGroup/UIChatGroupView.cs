//===================================================
//Author      : DRB
//CreateTime  ：9/23/2017 2:16:04 PM
//Description ：群友会窗口
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DRBPool;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 群友会窗口
/// </summary>
public class UIChatGroupView : UIWindowViewBase
{
    [SerializeField]
    private Transform m_ChatGroupContainer;//群挂载点
    [SerializeField]
    private Transform m_RoomContainer;//房间挂载点
    [SerializeField]
    private Transform m_MemberContainer;//成员挂载点
    [SerializeField]
    private Transform m_RecordContainer;//战绩挂载点
    [SerializeField]
    private Transform m_NoneGroupPage;//没有群页面
    [SerializeField]
    private Transform m_DefaultPageRight;//右边默认页面
    [SerializeField]
    private Transform m_ExistGroup;//存在群页面
    [SerializeField]
    private Transform m_MemberListPage;//成员列表页面
    [SerializeField]
    private Transform m_AddGroupPage;//添加群页面
    [SerializeField]
    private Transform m_GroupInfoPage;//群详情页面
    [SerializeField]
    private Transform m_GroupRoomPage;//群房间页面
    [SerializeField]
    private Transform m_GroupMessagePage;//群消息页面
    [SerializeField]
    private Transform m_GroupRecordPage;//群战绩页面
    [SerializeField]
    private Transform m_GroupManagePage;//群管理页面
    [SerializeField]
    private InputField m_InputGroupId;//输入的要加入的群号
    [SerializeField]
    private InputField m_InputGroupName;//输入的要创建的群名
    [SerializeField]
    private Image m_ImgSelectHead;//选中的头像
    [SerializeField]
    private ToggleGroup m_ToggleHead;
    [SerializeField]
    private List<Toggle> m_ListHead;//可选择的头像
    [SerializeField]
    private Button m_BtnDisband;//解散按钮
    [SerializeField]
    private Button m_BtnManage;//管理按钮
    //[SerializeField]
    //private Button m_BtnExit;//退群按钮
    //[SerializeField]
    //private Button m_BtnApply;//申请按钮
    //[SerializeField]
    //private GameObject m_NewApply;//新申请提示
    [SerializeField]
    private GameObject m_NewMessageTip;//新消息提示
    [SerializeField]
    private GameObject m_NewRecordTip;//新战绩提示
    [SerializeField]
    private GameObject m_NewManageTip;//新管理提示
    [SerializeField]
    private ScrollRect m_GroupScroll;//群拖拽视图
    [SerializeField]
    private Transform m_MessageContainer;//聊天消息挂载点
    [SerializeField]
    private InputField m_InputMessage;//输入的消息
    [SerializeField]
    private ScrollViewExt m_ScrollViewMessage;//消息滚动视图
    [SerializeField]
    private UIItemChatGroup m_CurrentGroup;//当前选中的群
    [SerializeField]
    private UIItemChatGroupMemberOption m_MemberOption;//成员选项
    [SerializeField]
    private Text m_TxtCards;

    private int m_SelectHeadIndex;//选中的头像索引

    private List<UIItemChatGroup> m_GroupList = new List<UIItemChatGroup>();//群列表

    private List<UIItemChatMember> m_MemberList = new List<UIItemChatMember>();//成员列表

    private List<UIItemChatRoom> m_RoomList = new List<UIItemChatRoom>();//房间列表

    private List<UIItemChatRoomPlayer> m_RoomPlayerList = new List<UIItemChatRoomPlayer>();//房间玩家列表

    private List<UIItemChatGroupMessage> m_MessageList = new List<UIItemChatGroupMessage>();//群消息列表

    private List<UIItemChatGroupRecord> m_RecordList = new List<UIItemChatGroupRecord>();//群战绩列表

    private Action<int> OnDragTop;



    private int m_GroupId;
    [SerializeField]
    private Text m_TxtGroupName;//群名
    [SerializeField]
    private Text m_TxtGroupId;//群Id
    [SerializeField]
    private Text m_TxtPlayerCount;//玩家数量
    [SerializeField]
    private Text m_TxtWaitingRoomCount;//等待房间数量
    [SerializeField]
    private Text m_TxtGamingRoomCount;//游戏房间数量

    [SerializeField]
    private Transform m_Container;//申请项挂载点

    private List<UIItemChatGroupApply> m_PlayerList = new List<UIItemChatGroupApply>();//申请项

    protected override void OnAwake()
    {
        base.OnAwake();

        for (int i = 0; i < m_ListHead.Count; ++i)
        {
            m_ListHead[i].onValueChanged.AddListener(OnToggle);
        }

        m_ScrollViewMessage.onDropTop += OnMessageDropTop;
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        if (m_ScrollViewMessage != null)
        {
            m_ScrollViewMessage.onDropTop -= OnMessageDropTop;
        }
    }

    private void OnMessageDropTop()
    {
        SendNotification("OnMessageScrollViewTop", m_GroupId);
    }

    private void OnToggle(bool arg0)
    {
        for (int i = 0; i < m_ListHead.Count; ++i)
        {
            if (m_ListHead[i].isOn)
            {
                m_SelectHeadIndex = i;
                m_ImgSelectHead.overrideSprite = m_ListHead[i].GetComponent<Image>().overrideSprite;
                break;
            }
        }
    }

    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();

        dic.Add(ChatGroupProxy.ON_ADD_GROUP, OnAddGroup);
        dic.Add(ChatGroupProxy.ON_REMOVE_GROUP, OnRemoveGroup);
        dic.Add(ChatGroupProxy.ON_ADD_MEMBER, OnAddMember);
        dic.Add(ChatGroupProxy.ON_REMOVE_MEMBER, OnRemoveMember);
        dic.Add(ChatGroupProxy.ON_ADD_ROOM, OnAddRoom);
        dic.Add(ChatGroupProxy.ON_REMOVE_ROOM, OnRemoveRoom);
        dic.Add(ChatGroupProxy.ON_ENTER_ROOM, OnEnterRoom);
        dic.Add(ChatGroupProxy.ON_LEAVE_ROOM, OnLeaveRoom);
        dic.Add(ChatGroupProxy.ON_GROUP_NEW_APPLY, OnGroupNewApply);
        dic.Add(ChatGroupProxy.ON_GROUP_INFO_CHANGED, OnGroupInfoChanged);
        dic.Add(ChatGroupProxy.ON_ROOM_INFO_CHANGED, OnRoomInfoChanged);
        dic.Add(ChatGroupProxy.ON_PLAYER_INFO_CHANGED, OnPlayerInfoChanged);
        dic.Add(ChatGroupProxy.ON_ADD_MESSAGE, OnAddMessage);
        dic.Add(ChatGroupProxy.ON_ADD_RECORD, OnAddRecord);
        dic.Add(ChatGroupProxy.ON_REMOVE_RECORD, OnRemoveRecord);

        dic.Add(ChatGroupProxy.ON_GROUP_ADD_APPLY, OnAddApply);
        dic.Add(ChatGroupProxy.ON_GROUP_REMOVE_APPLY, OnRemoveApply);
        dic.Add(ChatGroupProxy.ON_GROUP_REFRESH_APPLY, OnRefreshApply); 
        return dic;
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);

        switch (go.name)
        {
            case "btnChatGroupViewAddGroup"://添加群
                ShowAddGroup();
                break;
            case "btnChatGroupViewCreatGroup"://创建群
                SendNotification("btnChatGroupViewCreatGroup", m_InputGroupName.text, m_SelectHeadIndex);
                break;
            case "btnChatGroupViewJoinGroup"://加入群
                SendNotification("btnChatGroupViewJoinGroup", m_InputGroupId.text.ToInt());
                break;
            case "btnChatGroupViewDisband"://解散群
                SendNotification("btnChatGroupViewDisband", m_GroupId);
                break;
            case "btnChatGroupViewClearGroupId"://清空加入群浩
                m_InputGroupId.text = string.Empty;
                break;
            case "btnChatGroupViewCreatRoom"://创建房间
                SendNotification("btnChatGroupViewCreatRoom", m_GroupId);
                break;
            //case "btnChatGroupViewApply"://申请
            //    SendNotification("btnChatGroupViewApply", m_GroupId);
            //    break;
            case "btnChatGroupViewChat"://聊天
                m_GroupRoomPage.gameObject.SetActive(false);
                m_GroupMessagePage.gameObject.SetActive(true);
                m_GroupRecordPage.gameObject.SetActive(false);
                m_GroupManagePage.gameObject.SetActive(false);
                m_NewMessageTip.SetActive(false);
                break;
            case "btnChatGroupViewRoom"://房间
                m_GroupRoomPage.gameObject.SetActive(true);
                m_GroupMessagePage.gameObject.SetActive(false);
                m_GroupRecordPage.gameObject.SetActive(false);
                m_GroupManagePage.gameObject.SetActive(false);
                break;
            case "btnChatGroupViewRecord"://战绩
                m_GroupRoomPage.gameObject.SetActive(false);
                m_GroupMessagePage.gameObject.SetActive(false);
                m_GroupRecordPage.gameObject.SetActive(true);
                m_GroupManagePage.gameObject.SetActive(false);
                m_NewRecordTip.SetActive(false);
                break;
            case "btnChatGroupViewManage"://管理
                m_GroupRoomPage.gameObject.SetActive(false);
                m_GroupMessagePage.gameObject.SetActive(false);
                m_GroupRecordPage.gameObject.SetActive(false);
                m_NewManageTip.SetActive(false);
                if (!m_GroupManagePage.gameObject.activeInHierarchy)
                {
                    SendNotification("btnChatGroupViewApply", m_GroupId);
                }
                m_GroupManagePage.gameObject.SetActive(true);
                break;
            case "btnChatGroupViewSendMessage"://发送消息
                SendNotification("btnChatGroupViewSendMessage", m_GroupId, m_InputMessage.text);
                m_InputMessage.text = string.Empty;
                break;
            case "btnChatGroupViewSettingCards":
                SendNotification("btnChatGroupViewSettingCards", m_GroupId);
                break;
            case "btnChatGroupViewExit":
                SendNotification("btnChatGroupViewExit", m_GroupId);
                break;
            case "btnChatGroupViewMemberBack":
                m_MemberListPage.gameObject.SetActive(false);
                break;
        }
    }

    #region ShowGroupList 显示群列表
    /// <summary>
    /// 显示群列表
    /// </summary>
    /// <param name="lst"></param>
    public void ShowGroupList(List<ChatGroupEntity> lst)
    {
        SetPageActive(lst.Count > 0 ? m_ExistGroup : m_NoneGroupPage);
        if (lst == null || lst.Count == 0) return;

        for (int i = 0; i < m_GroupList.Count; ++i)
        {
            UIItemChatGroup item = m_GroupList[i];
            UIPoolManager.Instance.Despawn(item.transform);
        }
        m_GroupList.Clear();

        for (int i = 0; i < lst.Count; ++i)
        {
            ChatGroupEntity group = lst[i];
            UIItemChatGroup item = UIPoolManager.Instance.Spawn("UIItemChatGroup").GetComponent<UIItemChatGroup>();
            item.gameObject.SetParent(m_ChatGroupContainer);
            item.SetUI(group.id, group.name, group.avatar/*GetChatGroupHead(group.avatarIndex)*/, group.currMemberCount, group.maxMemberCount, group.roomCount);
            item.SetSelect(false);
            item.SetNewTip(group.hasNewApply);
            m_GroupList.Add(item);
        }
    }
    #endregion

    #region ShowGroup 显示群信息
    /// <summary>
    /// 显示群信息
    /// </summary>
    /// <param name="lst"></param>
    public void ShowGroup(TransferData data)
    {
        int groupId = data.GetValue<int>("GroupId");
        bool playerIsOwner = data.GetValue<bool>("IsOwner");
        bool playerIsManager = data.GetValue<bool>("PlayerIsManager");
        bool hasNewApply = data.GetValue<bool>("HasNewApply");
        string groupName = data.GetValue<string>("GroupName");
        int memberCount = data.GetValue<int>("MemberCount");
        int maxMemberCount = data.GetValue<int>("MaxMemberCount");
        int ownerId = data.GetValue<int>("OwnerId");
        int avatarIndex = data.GetValue<int>("AvatarIndex");
        int cards = data.GetValue<int>("CardCount");
        int waitingCount = data.GetValue<int>("WaitingRoomCount");
        int gamingCount = data.GetValue<int>("GamingRoomCount");
        string groupAvatar = data.GetValue<string>("GroupAvatar");
        List<TransferData> lstRoom = data.GetValue<List<TransferData>>("Room");
        List<TransferData> lstMember = data.GetValue<List<TransferData>>("Member");
        List<TransferData> lstMessage = data.GetValue<List<TransferData>>("Message");
        List<TransferData> lstRecord = data.GetValue<List<TransferData>>("Record");

        m_GroupId = groupId;
        SetPageActive(m_GroupList.Count > 0 ? m_ExistGroup : m_NoneGroupPage);
        m_DefaultPageRight.gameObject.SetActive(false);
        m_MemberListPage.gameObject.SetActive(true);
        m_GroupInfoPage.gameObject.SetActive(true);
        m_BtnDisband.gameObject.SetActive(playerIsOwner);
        //m_BtnExit.gameObject.SetActive(!playerIsOwner);
        //m_BtnApply.gameObject.SetActive(playerIsOwner);
        //m_NewApply.SetActive(hasNewApply);
        m_BtnManage.gameObject.SetActive(playerIsOwner|| playerIsManager);
        m_NewManageTip.SetActive(hasNewApply);
        m_NewMessageTip.SetActive(false);
        m_NewRecordTip.SetActive(false);
        m_TxtGroupName.SafeSetText(groupName);
        m_TxtGroupId.SafeSetText(string.Format("(ID:{0})",groupId.ToString()));
        m_TxtPlayerCount.SafeSetText(string.Format("{0}人", memberCount));
        m_TxtCards.SafeSetText(cards.ToString());
        m_TxtWaitingRoomCount.SafeSetText(waitingCount.ToString());
        m_TxtGamingRoomCount.SafeSetText(gamingCount.ToString());

        for (int i = 0; i < m_GroupList.Count; ++i)
        {
            if (m_GroupList[i].GroupId == groupId)
            {
                m_GroupList[i].SetNewTip(false);
                break;
            }
        }

        for (int i = 0; i < m_MemberList.Count; ++i)
        {
            UIPoolManager.Instance.Despawn(m_MemberList[i].transform);
        }
        m_MemberList.Clear();
        for (int i = 0; i < m_RoomList.Count; ++i)
        {
            UIPoolManager.Instance.Despawn(m_RoomList[i].transform);
        }
        m_RoomList.Clear();
        for (int i = 0; i < m_RoomPlayerList.Count; ++i)
        {
            UIPoolManager.Instance.Despawn(m_RoomPlayerList[i].transform);
        }
        m_RoomPlayerList.Clear();
        for (int i = 0; i < m_MessageList.Count; ++i)
        {
            UIPoolManager.Instance.Despawn(m_MessageList[i].transform);
        }
        m_MessageList.Clear();
        for (int i = 0; i < m_RecordList.Count; ++i)
        {
            UIPoolManager.Instance.Despawn(m_RecordList[i].transform);
        }
        m_RecordList.Clear();
        for (int i = 0; i < lstMember.Count; ++i)
        {
            TransferData memberData = lstMember[i];
            int playerId = memberData.GetValue<int>("PlayerId");
            string nickname = memberData.GetValue<string>("Nickname");
            bool isOnline = memberData.GetValue<bool>("IsOnline");
            string avatar = memberData.GetValue<string>("Avatar");
            bool isOwner = memberData.GetValue<bool>("IsOwner");
            bool isManager = memberData.GetValue<bool>("IsManager");
            UIItemChatMember item = UIPoolManager.Instance.Spawn("UIItemChatMember").GetComponent<UIItemChatMember>();
            item.SetUI(groupId, playerId, nickname, isOnline, avatar, ownerId == playerId, isManager, playerIsOwner, playerIsManager);
            item.gameObject.SetParent(m_MemberContainer, true);
            m_MemberList.Add(item);
        }
        for (int i = 0; i < lstMessage.Count; ++i)
        {
            TransferData messageData = lstMessage[i];
            string avatar = messageData.GetValue<string>("Avatar");
            byte[] message = messageData.GetValue<byte[]>("Message");
            bool isPlayer = messageData.GetValue<bool>("IsPlayer");

            UIItemChatGroupMessage item = UIPoolManager.Instance.Spawn("UIItemChatGroupMessage").GetComponent<UIItemChatGroupMessage>();
            item.SetUI(avatar, message.ToUTF8String(), isPlayer);
            item.gameObject.SetParent(m_MessageContainer);
            m_MessageList.Add(item);
        }
        for (int i = 0; i < lstRoom.Count; ++i)
        {
            TransferData roomData = lstRoom[i];
            int roomId = roomData.GetValue<int>("RoomId");
            string gameName = roomData.GetValue<string>("GameName");
            RoomEntityBase.RoomStatus roomStatus = roomData.GetValue<RoomEntityBase.RoomStatus>("RoomStatus");
            List<TransferData> lstRoomPlayer = roomData.GetValue<List<TransferData>>("Player");
            UIItemChatRoom item = UIPoolManager.Instance.Spawn("UIItemChatRoom").GetComponent<UIItemChatRoom>();
            List<UIItemChatRoomPlayer> lst = new List<UIItemChatRoomPlayer>();
            for (int j = 0; j < lstRoomPlayer.Count; ++j)
            {
                TransferData playerData = lstRoomPlayer[j];
                int playerId = playerData.GetValue<int>("PlayerId");
                string avatar = playerData.GetValue<string>("Avatar");
                UIItemChatRoomPlayer itemPlayer = UIPoolManager.Instance.Spawn("UIItemChatRoomPlayer").GetComponent<UIItemChatRoomPlayer>();
                if (playerData == null)
                {
                    itemPlayer.SetUI(groupId, roomId, 0, string.Empty, false);
                }
                else
                {
                    itemPlayer.SetUI(groupId, roomId, playerId, avatar, playerIsOwner);
                }
                lst.Add(itemPlayer);
                m_RoomPlayerList.Add(itemPlayer);
            }
            item.SetUI(groupId, roomId, roomStatus == RoomEntityBase.RoomStatus.Gaming, gameName);
            item.SetPlayer(lst);
            item.gameObject.SetParent(m_RoomContainer, true);
            m_RoomList.Add(item);
        }

        for (int i = 0; i < lstRecord.Count; ++i)
        {
            TransferData recordData = lstRecord[i];
            UIItemChatGroupRecord item = UIPoolManager.Instance.Spawn("UIItemChatGroupRecord").GetComponent<UIItemChatGroupRecord>();
            item.SetUI(recordData);
            item.gameObject.SetParent(m_RecordContainer);
            m_RecordList.Add(item);
        }

        m_CurrentGroup.SetSelect(true);
        m_CurrentGroup.SetNewTip(false);
        m_CurrentGroup.SetUI(groupId, groupName, groupAvatar, memberCount, maxMemberCount, lstRoom.Count);


       

        if (m_GroupManagePage.gameObject.activeInHierarchy)
        {
            if (!m_BtnManage.gameObject.activeInHierarchy)
            {
                m_GroupManagePage.gameObject.SetActive(false);
                m_GroupRoomPage.gameObject.SetActive(true);
            }
            else
            {
                SendNotification("btnChatGroupViewApply", m_GroupId);
            }
        }
    }
    #endregion

    #region ShowAddGroup 显示添加群界面
    /// <summary>
    /// 显示添加群界面
    /// </summary>
    private void ShowAddGroup()
    {
        SetPageActive(m_AddGroupPage);
        m_DefaultPageRight.gameObject.SetActive(false);
        m_GroupInfoPage.gameObject.SetActive(false);
        m_GroupId = 0;
    }
    #endregion

    #region ShowMemberOption 显示成员选项界面
    /// <summary>
    /// 显示成员选项界面
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="canAppoint"></param>
    /// <param name="canDimission"></param>
    /// <param name="canKick"></param>
    public void ShowMemberOption(int playerId, bool canAppoint, bool canDimission, bool canKick)
    {
        if (!canAppoint && !canDimission && !canKick) return;
        for (int i = 0; i < m_MemberList.Count; ++i)
        {
            if (m_MemberList[i].PlayerId == playerId)
            {
                m_MemberOption.gameObject.SetActive(true);
                m_MemberOption.SetUI(m_GroupId, playerId, canAppoint, canDimission, canKick);
                m_MemberOption.transform.position = m_MemberList[i].OptionContainer.position;
                break;
            }
        }
    }
    #endregion



    #region OnGroupInfoChanged 群信息变更
    /// <summary>
    /// 群信息变更
    /// </summary>
    /// <param name="obj"></param>
    private void OnGroupInfoChanged(TransferData data)
    {
        ChatGroupEntity group = data.GetValue<ChatGroupEntity>("GroupEntity");
        if (group == null) return;
        for (int i = 0; i < m_GroupList.Count; ++i)
        {
            if (group.id == m_GroupList[i].GroupId)
            {
                m_GroupList[i].SetUI(group.id, group.name, group.avatar , group.currMemberCount, group.maxMemberCount, group.roomCount);
                break;
            }
        }

        if (group.id == m_GroupId)
        {
            m_CurrentGroup.SetUI(group.id,group.name, group.avatar , group.currMemberCount,group.maxMemberCount,group.rooms.Count);
        }

        m_TxtPlayerCount.SafeSetText(string.Format("{0}人", group.currMemberCount, group.maxMemberCount));

        m_TxtCards.SafeSetText(group.cards.ToString());

        m_TxtWaitingRoomCount.SafeSetText(group.WaitingRoomCount.ToString());
        m_TxtGamingRoomCount.SafeSetText(group.GamingRoomCount.ToString());
    }
    #endregion

    #region OnRoomInfoChanged 房间信息变更
    /// <summary>
    /// 房间信息变更
    /// </summary>
    /// <param name="obj"></param>
    private void OnRoomInfoChanged(TransferData data)
    {
        ChatGroupEntity group = data.GetValue<ChatGroupEntity>("GroupEntity");
        if (group == null) return;
        RoomEntityBase room = data.GetValue<RoomEntityBase>("RoomEntity");
        if (room == null) return;

        for (int i = 0; i < m_RoomList.Count; ++i)
        {
            if (m_RoomList[i].RoomId == room.roomId)
            {
                m_RoomList[i].SetUI(group.id, room.roomId, room.roomStatus == RoomEntityBase.RoomStatus.Gaming, room.GameName);
                break;
            }
        }
    }
    #endregion

    #region OnPlayerInfoChanged 玩家信息变更
    /// <summary>
    /// 玩家信息变更
    /// </summary>
    /// <param name="obj"></param>
    private void OnPlayerInfoChanged(TransferData data)
    {
        ChatGroupEntity group = data.GetValue<ChatGroupEntity>("GroupEntity");
        if (group == null) return;
        if (group.id != m_GroupId) return;
        PlayerEntity player = data.GetValue<PlayerEntity>("PlayerEntity");
        PlayerEntity oneselfEntity = data.GetValue<PlayerEntity>("OneselfEntity");
        if (player == null) return;

        int index = data.GetValue<int>("Index");

        for (int i = 0; i < m_MemberList.Count; ++i)
        {
            if (m_MemberList[i].PlayerId == player.id)
            {
                m_MemberList[i].SetUI(group.id, player.id, player.nickname, player.online > 0, player.avatar, player.isOwner, player.isManager, oneselfEntity.isOwner, oneselfEntity.isManager);
                m_MemberList[i].transform.SetSiblingIndex(index);
                break;
            }
        }

        bool isPlayer = data.GetValue<bool>("IsPlayer");
        if (isPlayer)
        {
            m_BtnManage.gameObject.SetActive(player.isManager|| player.isOwner);
            for(int i = 0; i < m_RoomPlayerList.Count; ++i)
            {
                m_RoomPlayerList[i].SetAuthority(player.isManager);
            }
        }
    }
    #endregion

    #region OnAddGroup 添加新群
    /// <summary>
    /// 添加新群
    /// </summary>
    /// <param name="data"></param>
    private void OnAddGroup(TransferData data)
    {
        SetPageActive(m_ExistGroup);
        ChatGroupEntity group = data.GetValue<ChatGroupEntity>("ChatGroup");
        UIItemChatGroup item = UIPoolManager.Instance.Spawn("UIItemChatGroup").GetComponent<UIItemChatGroup>();
        item.gameObject.SetParent(m_ChatGroupContainer);
        item.SetUI(group.id, group.name, group.avatar/*GetChatGroupHead(group.avatarIndex)*/, group.currMemberCount, group.maxMemberCount, group.roomCount);
        item.SetSelect(false);
        item.SetNewTip(group.hasNewApply);
        m_GroupList.Add(item);
    }
    #endregion

    #region OnRemoveGroup 移除群
    /// <summary>
    /// 移除群
    /// </summary>
    /// <param name="data"></param>
    private void OnRemoveGroup(TransferData data)
    {
        ChatGroupEntity group = data.GetValue<ChatGroupEntity>("ChatGroup");

        for (int i = 0; i < m_GroupList.Count; ++i)
        {
            if (group.id == m_GroupList[i].GroupId)
            {
                UIItemChatGroup item = m_GroupList[i];
                UIPoolManager.Instance.Despawn(item.transform);
                m_GroupList.Remove(item);
                break;
            }
        }

        if (group.id == m_GroupId)
        {
            for (int i = 0; i < m_MemberList.Count; ++i)
            {
                UIPoolManager.Instance.Despawn(m_MemberList[i].transform);
            }
            m_MemberList.Clear();

            for (int i = 0; i < m_RoomList.Count; ++i)
            {
                UIPoolManager.Instance.Despawn(m_RoomList[i].transform);
            }
            m_RoomList.Clear();

            for (int i = 0; i < m_RecordList.Count; ++i)
            {
                UIPoolManager.Instance.Despawn(m_RecordList[i].transform);
            }
            m_RecordList.Clear();

            for (int i = 0; i < m_RoomPlayerList.Count; ++i)
            {
                UIPoolManager.Instance.Despawn(m_RoomPlayerList[i].transform);
            }
            m_RoomPlayerList.Clear();

            for (int i = 0; i < m_MessageList.Count; ++i)
            {
                UIPoolManager.Instance.Despawn(m_MessageList[i].transform);
            }
            m_MessageList.Clear();
        }
     

        if (m_GroupList.Count == 0)
        {
            m_NoneGroupPage.gameObject.SetActive(true);
        }

        if (m_GroupId == group.id)
        {
            m_GroupId = 0;
            m_MemberListPage.gameObject.SetActive(false);
            m_GroupInfoPage.gameObject.SetActive(false);
            m_DefaultPageRight.gameObject.SetActive(true);
        }
        SetPageActive(m_GroupList.Count > 0 ? m_ExistGroup : m_NoneGroupPage);
    }
    #endregion

    #region OnAddMember 添加成员
    /// <summary>
    /// 添加成员
    /// </summary>
    /// <param name="obj"></param>
    private void OnAddMember(TransferData data)
    {
        ChatGroupEntity group = data.GetValue<ChatGroupEntity>("GroupEntity");
        if (group.id != m_GroupId) return;

        PlayerEntity playerEntity = data.GetValue<PlayerEntity>("PlayerEntity");
        PlayerEntity oneselfEntity = data.GetValue<PlayerEntity>("OneselfEntity");
        int index = data.GetValue<int>("Index");
        UIItemChatMember item = UIPoolManager.Instance.Spawn("UIItemChatMember").GetComponent<UIItemChatMember>();
        item.SetUI(group.id, playerEntity.id, playerEntity.nickname, playerEntity.online > 0, playerEntity.avatar, playerEntity.isOwner,playerEntity.isManager, oneselfEntity.isOwner, oneselfEntity.isManager);
        item.gameObject.SetParent(m_MemberContainer, true);
        item.transform.SetSiblingIndex(index);
        m_MemberList.Add(item);

        bool isPlayer = data.GetValue<bool>("IsPlayer");
        if (isPlayer)
        {
            m_BtnManage.gameObject.SetActive(playerEntity.isManager||playerEntity.isOwner);
        }
    }
    #endregion

    #region OnRemoveMember 移除成员
    /// <summary>
    /// 移除成员
    /// </summary>
    /// <param name="data"></param>
    private void OnRemoveMember(TransferData data)
    {
        int groupId = data.GetValue<int>("GroupId");
        if (groupId != m_GroupId) return;
        int playerId = data.GetValue<int>("PlayerId");
        for (int i = 0; i < m_MemberList.Count; ++i)
        {
            if (playerId == m_MemberList[i].PlayerId)
            {
                UIItemChatMember item = m_MemberList[i];
                UIPoolManager.Instance.Despawn(item.transform);
                m_MemberList.Remove(item);
                break;
            }
        }
    }
    #endregion

    #region OnAddRoom 添加房间
    /// <summary>
    /// 添加房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnAddRoom(TransferData data)
    {
        ChatGroupEntity group = data.GetValue<ChatGroupEntity>("GroupEntity");
        if (group.id != m_GroupId) return;

        RoomEntityBase roomEntity = data.GetValue<RoomEntityBase>("RoomEntity");
        UIItemChatRoom item = UIPoolManager.Instance.Spawn("UIItemChatRoom").GetComponent<UIItemChatRoom>();
        List<UIItemChatRoomPlayer> lst = new List<UIItemChatRoomPlayer>();
        for (int j = 0; j < roomEntity.players.Count; ++j)
        {
            PlayerEntity playerEntity = roomEntity.players[j];
            UIItemChatRoomPlayer itemPlayer = UIPoolManager.Instance.Spawn("UIItemChatRoomPlayer").GetComponent<UIItemChatRoomPlayer>();
            if (playerEntity == null)
            {
                itemPlayer.SetUI(group.id, roomEntity.roomId, 0, string.Empty, false);
            }
            else
            {
                itemPlayer.SetUI(group.id, roomEntity.roomId, playerEntity.id, playerEntity.avatar, group.isOwner);
            }
            lst.Add(itemPlayer);
            m_RoomPlayerList.Add(itemPlayer);
        }
        item.SetUI(group.id, roomEntity.roomId, roomEntity.roomStatus == RoomEntityBase.RoomStatus.Gaming, roomEntity.GameName);
        item.SetPlayer(lst);
        item.gameObject.SetParent(m_RoomContainer, true);
        m_RoomList.Add(item);
        if (roomEntity.roomStatus == RoomEntityBase.RoomStatus.Gaming)
        {
            m_TxtGamingRoomCount.SafeSetText((m_TxtGamingRoomCount.text.ToInt() + 1).ToString());
        }
        else if (roomEntity.roomStatus == RoomEntityBase.RoomStatus.Waiting)
        {
            m_TxtWaitingRoomCount.SafeSetText((m_TxtWaitingRoomCount.text.ToInt() + 1).ToString());
        }
    }
    #endregion

    #region OnRemoveRoom 移除房间
    /// <summary>
    /// 移除房间
    /// </summary>
    /// <param name="data"></param>
    private void OnRemoveRoom(TransferData data)
    {
        int groupId = data.GetValue<int>("GroupId");
        if (groupId != m_GroupId) return;
        int roomId = data.GetValue<int>("RoomId");
        for (int i = 0; i < m_RoomList.Count; ++i)
        {
            if (roomId == m_RoomList[i].RoomId)
            {
                UIItemChatRoom item = m_RoomList[i];
                UIPoolManager.Instance.Despawn(item.transform);
                m_RoomList.Remove(item);

                for (int j = 0; j < item.PlayerList.Count; ++j)
                {
                    UIPoolManager.Instance.Despawn(item.PlayerList[j].transform);
                    m_RoomPlayerList.Remove(item.PlayerList[j]);
                }
                break;
            }
        }


    }
    #endregion

    #region OnEnterRoom 进入房间
    /// <summary>
    /// 进入房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnEnterRoom(TransferData data)
    {
        ChatGroupEntity group = data.GetValue<ChatGroupEntity>("GroupEntity");
        if (group.id != m_GroupId) return;
        int roomId = data.GetValue<int>("RoomId");
        PlayerEntity player = data.GetValue<PlayerEntity>("PlayerEntity");
        for (int i = 0; i < m_RoomList.Count; ++i)
        {
            if (m_RoomList[i].RoomId != roomId) continue;
            UIItemChatRoom room = m_RoomList[i];
            for (int j = 0; j < room.PlayerList.Count; ++j)
            {
                if (room.PlayerList[j].PlayerId == 0)
                {
                    room.PlayerList[j].SetUI(group.id, roomId, player.id, player.avatar, group.isOwner);
                    break;
                }
            }
        }
    }
    #endregion

    #region OnLeaveRoom 离开房间
    /// <summary>
    /// 离开房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnLeaveRoom(TransferData data)
    {
        int groupId = data.GetValue<int>("GroupId");
        if (groupId != m_GroupId) return;
        int roomId = data.GetValue<int>("RoomId");
        int playerId = data.GetValue<int>("PlayerId");

        for (int i = 0; i < m_RoomList.Count; ++i)
        {
            if (m_RoomList[i].RoomId != roomId) continue;
            UIItemChatRoom room = m_RoomList[i];
            if (room.PlayerList == null)
            {
                Debug.LogError("?????");
                break;
            }
            for (int j = 0; j < room.PlayerList.Count; ++j)
            {
                if (room.PlayerList[j].PlayerId == playerId)
                {
                    room.PlayerList[j].SetUI(groupId, roomId, 0, string.Empty, false);
                    break;
                }
            }
        }
    }
    #endregion

    #region OnGroupNewApply 新申请进群
    /// <summary>
    /// 新申请进群
    /// </summary>
    /// <param name="obj"></param>
    private void OnGroupNewApply(TransferData data)
    {
        int groupId = data.GetValue<int>("GroupId");
        bool hasNewApply = data.GetValue<bool>("HasNewApply");
        if (groupId != m_GroupId)
        {
            if (hasNewApply)
            {
                for (int i = 0; i < m_GroupList.Count; ++i)
                {
                    if (m_GroupList[i].GroupId == groupId)
                    {
                        m_GroupList[i].SetNewTip(true);
                    }
                }
            }
        }
        else
        {
            if (!m_GroupManagePage.gameObject.activeInHierarchy)
            {
                m_NewManageTip.gameObject.SetActive(hasNewApply);
            }
            else
            {
                SendNotification("btnChatGroupViewApply", m_GroupId);
            }
        }
    }
    #endregion

    #region
    /// <summary>
    /// 添加申请
    /// </summary>
    /// <param name="data"></param>
    private void OnAddApply(TransferData data)
    {
        int groupId = data.GetValue<int>("GroupId");
        if (groupId != m_GroupId) return;
        PlayerEntity player = data.GetValue<PlayerEntity>("PlayerEntity");

        UIItemChatGroupApply item = UIPoolManager.Instance.Spawn("UIItemChatGroupApply").GetComponent<UIItemChatGroupApply>();
        item.SetUI(groupId, player.id, player.nickname, player.avatar);
        item.gameObject.SetParent(m_Container);
        m_PlayerList.Add(item);
    }

    /// <summary>
    /// 移除申请
    /// </summary>
    /// <param name="obj"></param>
    private void OnRemoveApply(TransferData data)
    {

        int groupId = data.GetValue<int>("GroupId");
        if (groupId != m_GroupId) return;
        int playerId = data.GetValue<int>("PlayerId");
        bool hasNewApply = data.GetValue<bool>("HasNewApply");
        m_NewManageTip.SetActive(hasNewApply);
        for (int i = 0; i < m_PlayerList.Count; ++i)
        {
            if (m_PlayerList[i].PlayerId == playerId)
            {
                UIPoolManager.Instance.Despawn(m_PlayerList[i].transform);
                m_PlayerList.Remove(m_PlayerList[i]);
                break;
            }
        }
    }

    /// <summary>
    /// 刷新申请
    /// </summary>
    /// <param name="data"></param>
    private void OnRefreshApply(TransferData data)
    {
        int groupId = data.GetValue<int>("GroupId");
        if (groupId != m_GroupId) return;
        List<PlayerEntity> applyList= data.GetValue<List<PlayerEntity>>("ApplyList");
        for (int i = m_PlayerList.Count - 1; i >= 0; --i)
        {
            UIPoolManager.Instance.Despawn(m_PlayerList[i].transform);
            m_PlayerList.Remove(m_PlayerList[i]);
        }
        for (int i = 0; i < applyList.Count; ++i)
        {
            PlayerEntity player = applyList[i];
            UIItemChatGroupApply item = UIPoolManager.Instance.Spawn("UIItemChatGroupApply").GetComponent<UIItemChatGroupApply>();
            item.SetUI(groupId, player.id, player.nickname, player.avatar);
            item.gameObject.SetParent(m_Container);
            m_PlayerList.Add(item);
        }

    }


    #endregion



    #region OnAddMessage 添加聊天消息
    /// <summary>
    /// 添加聊天消息
    /// </summary>
    /// <param name="data"></param>
    private void OnAddMessage(TransferData data)
    {
        ChatGroupEntity group = data.GetValue<ChatGroupEntity>("GroupEntity");
        if (group == null) return;
        MessageEntity message = data.GetValue<MessageEntity>("Message");
        if (message == null) return;

        if (group.id != m_GroupId)
        {
            for (int i = 0; i < m_GroupList.Count; ++i)
            {
                if (m_GroupList[i].GroupId == group.id)
                {
                    m_GroupList[i].SetNewTip(true);
                }
            }
        }
        else
        {
            int index = data.GetValue<int>("Index");
            UIItemChatGroupMessage item = UIPoolManager.Instance.Spawn("UIItemChatGroupMessage").GetComponent<UIItemChatGroupMessage>();
            item.SetUI(message.sendPlayer.avatar, message.message.ToUTF8String(), message.isPlayer);
            item.gameObject.SetParent(m_MessageContainer);
            m_MessageList.Add(item);
            item.transform.SetSiblingIndex(index);
            if (index > 0)
            {
                m_ScrollViewMessage.scrollView.DOVerticalNormalizedPos(0f, 0.2f);
                if (!m_MessageContainer.gameObject.activeInHierarchy)
                {
                    m_NewMessageTip.SetActive(true);
                }
            }
            else
            {
                m_ScrollViewMessage.scrollView.DOVerticalNormalizedPos(0.99f, 0.2f);
            }
        }
    }
    #endregion

    #region OnAddRecord 添加战绩
    /// <summary>
    /// 添加战绩
    /// </summary>
    /// <param name="obj"></param>
    private void OnAddRecord(TransferData data)
    {
        int groupId = data.GetValue<int>("GroupId");

        if (groupId == m_GroupId)
        {
            UIItemChatGroupRecord item = UIPoolManager.Instance.Spawn("UIItemChatGroupRecord").GetComponent<UIItemChatGroupRecord>();
            item.SetUI(data);
            item.gameObject.SetParent(m_RecordContainer);
            item.transform.SetSiblingIndex(0);
            m_RecordList.Add(item);
        }
        else
        {
            m_NewRecordTip.SetActive(true);
        }
    }
    #endregion

    #region OnRemoveRecord 移除战绩
    /// <summary>
    /// 移除战绩
    /// </summary>
    /// <param name="obj"></param>
    private void OnRemoveRecord(TransferData data)
    {
        int groupId = data.GetValue<int>("GroupId");
        if (groupId != m_GroupId) return;
        int battleId = data.GetValue<int>("BattleId");

        for (int i = 0; i < m_RecordList.Count; ++i)
        {
            if (m_RecordList[i].BattleId == battleId)
            {
                UIPoolManager.Instance.Despawn(m_RecordList[i].transform);
                m_RecordList.RemoveAt(i);
                break;
            }
        }
    }
    #endregion

    private Sprite GetChatGroupHead(int avatarIndex)
    {
        return m_ListHead[avatarIndex].GetComponent<Image>().overrideSprite;
    }


    private void SetPageActive(Transform go)
    {
        if (go == null) return;
        if (m_NoneGroupPage != null) m_NoneGroupPage.gameObject.SetActive(m_NoneGroupPage == go);
        if (m_ExistGroup != null) m_ExistGroup.gameObject.SetActive(m_ExistGroup == go);
        if (m_AddGroupPage != null) m_AddGroupPage.gameObject.SetActive(m_AddGroupPage == go);
    }

}
