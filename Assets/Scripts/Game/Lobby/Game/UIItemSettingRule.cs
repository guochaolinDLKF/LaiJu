//===================================================
//Author      : DRB
//CreateTime  ：4/12/2017 11:09:05 AM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSettingRule : MonoBehaviour
{
    [SerializeField]
    private Text m_TextRuleName;
    [SerializeField]
    private ToggleGroup m_ToggleGroup;

    private List<UIItemSettingOption> m_Cache = new List<UIItemSettingOption>();



    public void SetUI(string ruleName,bool isRadio,List<cfg_settingEntity> options,Action<int> onToggle,bool isAA,int payment,int playerCount)
    {
        m_TextRuleName.SafeSetText(ruleName);

        if (string.IsNullOrEmpty(ruleName.Trim()))
        {
            m_TextRuleName.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            m_TextRuleName.transform.parent.gameObject.SetActive(true);
        }

        for (int i = 0; i < m_Cache.Count; ++i)
        {
            m_Cache[i].gameObject.SetActive(false);
        }

        UIViewManager.Instance.LoadItemAsync("UIItemSettingOption",(GameObject prefab)=> 
        {
            for (int i = 0; i < options.Count; ++i)
            {
                cfg_settingEntity entity = options[i];
                UIItemSettingOption item = null;
                if (i < m_Cache.Count)
                {
                    item = m_Cache[i];
                    item.gameObject.SetActive(true);
                }
                else
                {
                    GameObject go = Instantiate(prefab);
                    go.SetParent(m_ToggleGroup.transform);
                    item = go.GetComponent<UIItemSettingOption>();
                    m_Cache.Add(item);
                }
                item.SetUI(entity.id, isRadio, entity.name, entity.value, entity.init == 1, onToggle,entity.tags,isAA, payment,playerCount, entity.cost);
            }
        });
    }
}
