//===================================================
//Author      : DRB
//CreateTime  ：5/5/2017 1:42:59 PM
//Description ：
//===================================================
using System.Collections.Generic;
using UnityEngine;

public class SettingBind
{
    public int id;

    public int status;

    public int init;
}


public partial class cfg_settingEntity 
{


    private List<SettingBind> m_BindSelect;
    public List<SettingBind> bindSelect
    {
        get
        {
            if (m_BindSelect == null)
            {
                if (string.IsNullOrEmpty(selected)) return null;
                m_BindSelect = new List<SettingBind>();
                string[] arr = selected.Split(',');
                for (int i = 0; i < arr.Length; ++i)
                {
                    string[] arr1 = arr[i].Split(':');
                    SettingBind bind = new SettingBind();
                    if (arr1.Length == 2)
                    {
                        bind.id = arr1[0].ToInt();
                        bind.init = arr1[1].ToInt();
                        bind.status = 1;
                    }
                    else if(arr1.Length == 3)
                    {
                        bind.id = arr1[0].ToInt();
                        bind.status = arr1[1].ToInt();
                        bind.init = arr1[2].ToInt();
                    }
                    m_BindSelect.Add(bind);
                }
            }
            return m_BindSelect;
        }
    }

    private List<SettingBind> m_BindUnSelect;
    public List<SettingBind> bindUnSelect
    {
        get
        {
            if (m_BindUnSelect == null)
            {
                if (string.IsNullOrEmpty(unselect)) return null;
                m_BindUnSelect = new List<SettingBind>();
                string[] arr = unselect.Split(',');
                for (int i = 0; i < arr.Length; ++i)
                {
                    string[] arr1 = arr[i].Split(':');
                    SettingBind bind = new SettingBind();
                    if (arr1.Length == 2)
                    {
                        bind.id = arr1[0].ToInt();
                        bind.init = arr1[1].ToInt();
                        bind.status = 1;
                    }
                    else if (arr1.Length == 3)
                    {
                        bind.id = arr1[0].ToInt();
                        bind.status = arr1[1].ToInt();
                        bind.init = arr1[2].ToInt();
                    }
                    m_BindUnSelect.Add(bind);
                }
            }
            return m_BindUnSelect;
        }
    }

}
