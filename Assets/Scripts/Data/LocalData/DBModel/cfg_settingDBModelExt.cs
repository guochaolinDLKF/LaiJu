//===================================================
//Author      : DRB
//CreateTime  ：5/5/2017 10:43:02 AM
//Description ：
//===================================================
using System.Collections.Generic;
using UnityEngine;


public partial class cfg_settingDBModel
{


    public List<cfg_settingEntity> CurrentSetting;

    private List<cfg_settingEntity> m_AllGame;

    public cfg_settingDBModel():base()
    {
        string str = PlayerPrefs.GetString("SettingCache");
        if (!string.IsNullOrEmpty(str))
        {
            string[] arr = str.Split(',');
            for (int i = 0; i < arr.Length; ++i)
            {
                string[] arr1 = arr[i].Split(':');
                if (arr1.Length == 3)
                {
                    int id = arr1[0].ToInt();
                    int init = arr1[2].ToInt();

                    for (int j = 0; j < m_List.Count; ++j)
                    {
                        if (m_List[j].id == id)
                        {
                            m_List[j].init = init;
                        }
                    }
                }
                if (arr1.Length == 2)
                {
                    int id = arr1[0].ToInt();
                    int init = arr1[1].ToInt();

                    for (int j = 0; j < m_List.Count; ++j)
                    {
                        if (m_List[j].id == id)
                        {
                            m_List[j].init = init;
                        }
                    }
                }
            }
        }

        CurrentSetting = new List<cfg_settingEntity>(m_List);

        for (int i = 0; i < CurrentSetting.Count; ++i)
        {
            cfg_settingEntity entity = CurrentSetting[i];
            List<SettingBind> lstBind = null;
            if (entity.init == 1 && entity.bindSelect != null)
            {
                lstBind = entity.bindSelect;
            }
            else if (entity.init == 0 && entity.bindUnSelect != null)
            {
                lstBind = entity.bindUnSelect;
            }

            if (lstBind != null)
            {

                foreach (SettingBind bind in lstBind)
                {
                    //Debug.Log(bind.status);
                    cfg_settingEntity optionEntity = Get(bind.id);
                    if (optionEntity == null) continue;
                    optionEntity.status = bind.status;
                    //SetOptionSelect(optionEntity, bind.init);
                    //optionEntity.init = bind.init;
                    //if (optionEntity.mode == 1)
                    //{
                    //    List<cfg_settingEntity> lst = GetOptionsByRuleNameAndGameId(optionEntity.gameId, optionEntity.label);
                    //    for (int i = 0; i < lst.Count; ++i)
                    //    {
                    //        SetOptionSelect(lst[i], 0);
                    //    }
                    //    SetOptionSelect(optionEntity, 1);
                    //}
                    //else
                    //{
                    //    SetOptionSelect(optionEntity, bind.init);
                    //}
                }
            }
        }
    }

    public List<cfg_settingEntity> GetAllOptions()
    {
        return CurrentSetting;
    }

    public List<cfg_settingEntity> GetAllGame()
    {
        if (m_AllGame == null)
        {
            m_AllGame = new List<cfg_settingEntity>();
            List<int> lstGameId = new List<int>();
            for (int i = 0; i < CurrentSetting.Count; ++i)
            {
                
                if (lstGameId.Contains(CurrentSetting[i].gameId) || CurrentSetting[i].status == 0) continue;
                lstGameId.Add(CurrentSetting[i].gameId);
                m_AllGame.Add(CurrentSetting[i]);
            }
        }
        
        return m_AllGame;
    }


    public List<cfg_settingEntity> GetOptionsByGameId(int gameId)
    {
        List<cfg_settingEntity> lst = new List<cfg_settingEntity>();
        for (int i = 0; i < CurrentSetting.Count; ++i)
        {
            if (CurrentSetting[i].gameId != gameId || CurrentSetting[i].status == 0) continue;
            lst.Add(CurrentSetting[i]);
        }
        return lst;
    }

    public List<cfg_settingEntity> GetRuleByGameId(int gameId)
    {
        List<cfg_settingEntity> lst = new List<cfg_settingEntity>();
        List<string> lstRuleName = new List<string>();
        for (int i = 0; i < CurrentSetting.Count; ++i)
        {
            if (CurrentSetting[i].gameId != gameId || CurrentSetting[i].status == 0) continue;
            if (lstRuleName.Contains(CurrentSetting[i].label)) continue;
            lst.Add(CurrentSetting[i]);
            lstRuleName.Add(CurrentSetting[i].label);
        }
        return lst;
    }

    public List<cfg_settingEntity> GetOptionsByRuleNameAndGameId(int gameId,string ruleName)
    {
        List<cfg_settingEntity> lst = new List<cfg_settingEntity>();
        for (int i = 0; i < CurrentSetting.Count; ++i)
        {
            if (CurrentSetting[i].gameId != gameId || CurrentSetting[i].status == 0 || !CurrentSetting[i].label.Equals(ruleName)) continue;
            lst.Add(CurrentSetting[i]);
        }
        return lst;
    }

    #region SelectOption 选择选项
    /// <summary>
    /// 选择选项
    /// </summary>
    /// <param name="optionName"></param>
    public void SelectOption(int id)
    {
        cfg_settingEntity entity = null;
        for (int i = 0; i < CurrentSetting.Count; ++i)
        {
            if (CurrentSetting[i].id == id)
            {
                entity = CurrentSetting[i];
                break;
            }
        }
        if (entity != null)
        {
            if (entity.mode == 1)
            {
                List<cfg_settingEntity> lstEntity = GetOptionsByRuleNameAndGameId(entity.gameId, entity.label);
                for (int i = 0; i < lstEntity.Count; ++i)
                {
                    SetOptionSelect(lstEntity[i], 0);
                }
                SetOptionSelect(entity, 1);
            }
            else
            {
                SetOptionSelect(entity, entity.init == 1?0:1);
            }


            List<SettingBind> lstBind = null;
            if (entity.init == 1 && entity.bindSelect != null)
            {
                lstBind = entity.bindSelect;
            }
            else if (entity.init == 0 && entity.bindUnSelect != null)
            {
                lstBind = entity.bindUnSelect;
            }

            if (lstBind != null)
            {
                
                foreach (SettingBind bind in lstBind)
                {
                    //Debug.Log(bind.status);
                    cfg_settingEntity optionEntity = Get(bind.id);
                    if (optionEntity == null) continue;
                    optionEntity.status = bind.status;
                    SetOptionSelect(optionEntity, bind.init);
                    //optionEntity.init = bind.init;
                    //if (optionEntity.mode == 1)
                    //{
                    //    List<cfg_settingEntity> lst = GetOptionsByRuleNameAndGameId(optionEntity.gameId, optionEntity.label);
                    //    for (int i = 0; i < lst.Count; ++i)
                    //    {
                    //        SetOptionSelect(lst[i], 0);
                    //    }
                    //    SetOptionSelect(optionEntity, 1);
                    //}
                    //else
                    //{
                    //    SetOptionSelect(optionEntity, bind.init);
                    //}
                }
            }
            //PlayerPrefs.SetString("RoomSettings", LitJson.JsonMapper.ToJson(m_AllSetting));
        }
    }
    #endregion


    #region SetOptionSelect 设置选项是否选中
    /// <summary>
    /// 设置选项是否选中
    /// </summary>
    /// <param name="option"></param>
    /// <param name="isSelect"></param>
    private void SetOptionSelect(cfg_settingEntity option, int isSelect)
    {
        option.init = isSelect;

        TransferData data = new TransferData();

        data.SetValue("IsOn", option.init);
        data.SetValue("Id",option.id);
        ModelDispatcher.Instance.Dispatch("OnSettingRuleOptionSelectedChange", data);

        SaveSetting();
    }
    #endregion

    public cfg_settingEntity GetOptionByGroup(string groupName)
    {
        for (int i = 0; i < CurrentSetting.Count; ++i)
        {
            if (groupName == CurrentSetting[i].tags && CurrentSetting[i].status == 1 && CurrentSetting[i].init == 1)
            {
                return CurrentSetting[i];
            }
        }
        return null;
    }


    public void SaveSetting()
    {
        string str = string.Empty;
        for (int i = 0; i < CurrentSetting.Count; ++i)
        {
            str += CurrentSetting[i].id + ":" + CurrentSetting[i].init;
            if (i <= CurrentSetting.Count - 1)
            {
                str += ",";
            }
        }

        PlayerPrefs.SetString("SettingCache", str);
    }
}
