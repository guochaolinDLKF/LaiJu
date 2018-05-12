//===================================================
//Author      : DRB
//CreateTime  ：3/10/2017 9:36:11 AM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICreateRoomView : UIWindowViewBase
{
    [SerializeField]
    private Transform m_MenuContainer;
    [SerializeField]
    private Transform m_ContentContainer;


    private List<UIItemSettingRule> m_ListRule = new List<UIItemSettingRule>();


    private List<UIItemSettingMenu> m_ListMenu = new List<UIItemSettingMenu>();


    public Action<int> OnSettingMenuClick;

    public Action<int> OnOptionClick;
    [SerializeField]
    private Toggle[] m_ArrToggle;

    private List<int> m_SelectId = new List<int>();
    [SerializeField]
    private Toggle m_CanLiangXi;
    [SerializeField]
    private Toggle m_HongzhongLiangxi;
    [SerializeField]
    private Toggle m_ZhongFaBaiLiangxi;

    protected override void OnAwake()
    {
        base.OnAwake();
        if (m_CanLiangXi != null)
        {
            m_CanLiangXi.onValueChanged.AddListener(OnLiangxiValueChanged);
        }

        string setting = PlayerPrefs.GetString("TaiLaiRule", string.Empty);
        if (!string.IsNullOrEmpty(setting))
        {
            string[] arr = setting.Split(';');
            for (int i = 0; i < arr.Length; ++i)
            {
                string[] arr1 = arr[i].Split(':');
                if (arr1.Length == 2)
                {
                    for (int j = 0; j < m_ArrToggle.Length; ++j)
                    {
                        if (arr1[0].Equals(m_ArrToggle[j].name))
                        {
                            m_ArrToggle[j].isOn = arr1[1].ToBool();
                        }
                    }
                }
            }
        }

        for (int i = 0; i < m_ArrToggle.Length; ++i)
        {
            m_ArrToggle[i].onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    private void OnToggleValueChanged(bool arg0)
    {
        string str = string.Empty;
        for (int i = 0; i < m_ArrToggle.Length; ++i)
        {
            str += string.Format("{0}:{1};",m_ArrToggle[i].name,m_ArrToggle[i].isOn?1:0);
        }

        PlayerPrefs.SetString("TaiLaiRule", str);
    }

    private void OnLiangxiValueChanged(bool isSelect)
    {
        if (m_HongzhongLiangxi != null)
        {
            m_HongzhongLiangxi.interactable = isSelect;
        }
        if (m_ZhongFaBaiLiangxi != null)
        {
            m_ZhongFaBaiLiangxi.interactable = isSelect;
        }
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case ConstDefine.BtnCreateRoomViewCreate:
                m_SelectId.Clear();
                for (int i = 0; i < m_ArrToggle.Length; ++i)
                {
                    if (m_ArrToggle[i].isOn && m_ArrToggle[i].gameObject.activeInHierarchy)
                    {
                        m_SelectId.Add(m_ArrToggle[i].name.ToInt());
                    }
                }
                SendNotification(ConstDefine.BtnCreateRoomViewCreate, m_SelectId);
                break;
            case ConstDefine.BtnCreateRoomViewBack:
                SendNotification(ConstDefine.BtnCreateRoomViewBack);
                break;
        }
    }


    public void CreateMenu(List<cfg_gameEntity> lst)
    {
        m_ListMenu.Clear();

        UIViewManager.Instance.LoadItemAsync("UIItemSettingMenu", (GameObject prefab) => 
        {
            for (int i = 0; i < lst.Count; ++i)
            {
                GameObject go = Instantiate(prefab);
                go.SetParent(m_MenuContainer);
                UIItemSettingMenu menu = go.GetComponent<UIItemSettingMenu>();
                menu.SetUI(lst[i].id, lst[i].GameName, OnSettingMenuClick);
                m_ListMenu.Add(menu);
            }
        });
        if (lst.Count < 2)
        {
            m_MenuContainer.gameObject.SetActive(false);
        }
    }

    #region SetContent 设置选项内容
    /// <summary>
    /// 设置选项内容
    /// </summary>
    /// <param name="rules"></param>
    public void SetContent(List<cfg_settingEntity> rules,int payment,bool isAA,int playerCount)
    {
        if (m_ContentContainer == null) return;
#if IS_HONGHU || IS_LAOGUI || IS_BAODING
        GridLayoutGroup grid = m_ContentContainer.GetComponent<GridLayoutGroup>();

        grid.spacing = new Vector2(0,30);

        float cellSizeY = (680 - rules.Count * grid.spacing.y) / rules.Count;
        grid.cellSize = new Vector2(100, cellSizeY);

#endif
        for (int i = 0; i < m_ListMenu.Count; ++i)
        {
            m_ListMenu[i].SetState(m_ListMenu[i].GameId == rules[0].gameId);
        }

        for (int i = 0; i < m_ListRule.Count; ++i)
        {
            m_ListRule[i].gameObject.SetActive(false);
        }
       
        UIViewManager.Instance.LoadItemAsync("UIItemSettingRule", (GameObject prefab) =>
        {
            for (int i = 0; i < rules.Count; ++i)
            {
                UIItemSettingRule item = null;
                if (i < m_ListRule.Count)
                {
                    item = m_ListRule[i];
                    item.gameObject.SetActive(true);
                }
                else
                {
                    GameObject go2 = Instantiate(prefab);
                    go2.SetParent(m_ContentContainer);
                    item = go2.GetComponent<UIItemSettingRule>();
                    m_ListRule.Add(item);
                }
                item.SetUI(rules[i].label, rules[i].mode == 1, cfg_settingDBModel.Instance.GetOptionsByRuleNameAndGameId(rules[i].gameId, rules[i].label), OnOptionClick,isAA, payment,playerCount);
            }
        });
    }
    #endregion
}
