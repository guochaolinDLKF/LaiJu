//===================================================
//Author      : DRB
//CreateTime  ：4/11/2017 1:55:31 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using DRB.MahJong;
using UnityEngine;
using UnityEngine.UI;

public class UIItemMahJongRoomInfo : UIItemRoomInfoBase
{
    public static UIItemMahJongRoomInfo Instance;

    [SerializeField]
    private Text m_TextOverplusWallCount;

    [SerializeField]
    private Image m_TextLuckPoker;
    [SerializeField]
    private GameObject m_UniversalPrefab;
    [SerializeField]
    private Transform m_UniversalContainer;

    private List<GameObject> m_UniversalList = new List<GameObject>();

    protected override void OnAwake()
    {
        base.OnAwake();
        Instance = this;
        ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_ROOM_INFO_CHANGED, OnRoomInfoChanged);
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_ROOM_INFO_CHANGED, OnRoomInfoChanged);
    }

    /// <summary>
    /// 房间信息变更
    /// </summary>
    /// <param name="obj"></param>
    private void OnRoomInfoChanged(TransferData data)
    {
        RoomEntity room = data.GetValue<RoomEntity>("Room");
        SetLuckPoker(room.LuckPoker);
        m_TextOverplusWallCount.SafeSetText(room.PokerAmount.ToString());
        if (m_TextBaseScore.text.Contains("底"))
        {
            m_TextBaseScore.SafeSetText("底    分:" + room.BaseScore.ToString());
        }
        base.ShowLoop(room.currentLoop, room.maxLoop, room.isQuan);
    }

    public void SetLuckPoker(Poker poker)
    {
        if (m_TextLuckPoker == null) return;
        if (poker == null || (poker.index == 0 && poker.color == 0))
        {
            m_TextLuckPoker.gameObject.SetActive(false);
        }
        else
        {
            m_TextLuckPoker.gameObject.SetActive(true);


            m_TextLuckPoker.overrideSprite = MahJongManager.Instance.LoadPokerSprite(poker);
        }
    }

    public void SetUniversal(List<Poker> pokers)
    {
        if (m_UniversalContainer == null) return;
        if (m_UniversalPrefab == null) return;

        for (int i = 0; i < m_UniversalList.Count; ++i)
        {
            Destroy(m_UniversalList[i]);
        }
        m_UniversalList.Clear();

        if (pokers == null || pokers.Count == 0) return;

        for (int i = 0; i < pokers.Count; ++i)
        {
            GameObject go = Instantiate(m_UniversalPrefab.gameObject);
            go.gameObject.SetActive(true);
            go.SetParent(m_UniversalContainer);
            Image img = go.GetComponent<Image>();
            img.overrideSprite = MahJongManager.Instance.LoadPokerSprite(pokers[i]);
            m_UniversalList.Add(go);
        }
    }
}
