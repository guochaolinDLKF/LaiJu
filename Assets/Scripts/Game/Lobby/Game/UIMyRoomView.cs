//===================================================
//Author      : DRB
//CreateTime  ：7/6/2017 11:05:35 AM
//Description ：我的房间窗口
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;


public class UIMyRoomView : UIWindowViewBase 
{
    [SerializeField]
    private Transform m_Container;
    [SerializeField]
    private UIItemMyRoomPlayer m_PlayerView;

    public Action<int> onJoinClick;

    public Action<int> onInviteClick;

    public Action<int,Transform> onPlayerClick;

    public Action onUpdate;


    private float m_Timer;

    private List<UIItemMyRoom> m_Cache = new List<UIItemMyRoom>();

    protected override void OnAwake()
    {
        base.OnAwake();
        m_PlayerView.gameObject.SetActive(false);
    }
    

    private const float UPDATE_SPACE = 10f;

    private void Update()
    {
        if (Time.time > m_Timer + UPDATE_SPACE)
        {
            m_Timer = Time.time;
            if (onUpdate != null)
            {
                onUpdate();
            }
        }
    }


    public void SetUI(List<MyRoomEntity> lst, bool canInvite)
    {
        for (int i = 0; i < m_Cache.Count; ++i)
        {
            m_Cache[i].gameObject.SetActive(false);
        }

        UIViewManager.Instance.LoadItemAsync("uiitemmyroom", (GameObject go) =>
         {
             for (int i = 0; i < lst.Count; ++i)
             {
                 MyRoomEntity entity = lst[i];
                 UIItemMyRoom item = null;
                 if (i < m_Cache.Count)
                 {
                     item = m_Cache[i];
                     item.gameObject.SetActive(true);
                 }
                 else
                 {
                     go = Instantiate(go);
                     go.SetParent(m_Container);
                     item = go.GetComponent<UIItemMyRoom>();
                     m_Cache.Add(item);
                 }
                 item.SetUI(entity.roomId, entity.gameName, entity.loop, entity.maxLoop, entity.player, entity.maxPlayer, entity.ownerName, entity.payment, onJoinClick, onInviteClick, onPlayerClick, canInvite);
             }
         });
    }

    public void SetPlayerView(List<MyRoomPlayerEntity> lst,Transform container,int maxPlayerCount)
    {
        m_PlayerView.gameObject.SetActive(lst.Count > 0);
        m_PlayerView.transform.position = container.position;
        m_PlayerView.transform.localPosition += new Vector3(80f,0f,0f);
        m_PlayerView.SetUI(lst, maxPlayerCount);
    }
}
