//===================================================
//Author      : DRB
//CreateTime  ：7/6/2017 12:14:34 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemMyRoomPlayer : UIItemBase 
{
    [SerializeField]
    private RectTransform m_BG;
    [SerializeField]
    private Transform m_Container;
    [SerializeField]
    private Button m_ButtonMask;

    private List<UIItemMyRoomPlayerName> m_Cache = new List<UIItemMyRoomPlayerName>();

    private bool m_isTop = true;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_ButtonMask.onClick.AddListener(OnMaskClick);
    }

    private void OnMaskClick()
    {
        gameObject.SetActive(false);
    }

    public void SetUI(List<MyRoomPlayerEntity> lst, int maxPlayerCount)
    {
        float localBottomPointY = GetComponent<RectTransform>().anchoredPosition.y - maxPlayerCount * 70f;
        float canvasBottomPointY = -UIViewManager.Instance.CurrentUIScene.CurrentCanvas.GetComponent<CanvasScaler>().referenceResolution.y / 2;
        if (localBottomPointY < canvasBottomPointY)
        {
            m_BG.pivot = new Vector2(0.5f, 0f);
            if (m_isTop)
            {
                m_BG.transform.localPosition = new Vector3(m_BG.transform.localPosition.x, -m_BG.transform.localPosition.y, m_BG.transform.localPosition.z);
                m_isTop = !m_isTop;
            }
        }
        else
        {
            m_BG.pivot = new Vector2(0.5f, 1f);
            if (!m_isTop)
            {
                m_BG.transform.localPosition = new Vector3(m_BG.transform.localPosition.x, -m_BG.transform.localPosition.y, m_BG.transform.localPosition.z);
                m_isTop = !m_isTop;
            }
        }


        m_BG.sizeDelta = new Vector2(209f, maxPlayerCount * 70f);


        for (int i = 0; i < m_Cache.Count; ++i)
        {
            m_Cache[i].gameObject.SetActive(false);
        }
        UIViewManager.Instance.LoadItemAsync("uiitemmyroomplayername", (GameObject go) => 
        {
            for (int i = 0; i < lst.Count; ++i)
            {
                UIItemMyRoomPlayerName item = null;
                if (i < m_Cache.Count)
                {
                    item = m_Cache[i];
                    item.gameObject.SetActive(true);
                }
                else
                {
                    go = Instantiate(go);
                    go.SetParent(m_Container);
                    item = go.GetComponent<UIItemMyRoomPlayerName>();
                    m_Cache.Add(item);
                }
                item.SetUI(lst[i].nickname);
            }
        });


    }

}
