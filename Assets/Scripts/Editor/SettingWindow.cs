//===================================================
//Author      : DRB
//CreateTime  ：7/5/2016 3:51:58 PM
//Description ：宏定义工具
//===================================================
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class SettingWindow :  EditorWindow
{
    private Vector2 pos;
    private List<MacroItem> m_ListMacroItem = new List<MacroItem>();
    private string m_strMacro = null;

    private Dictionary<string, bool> m_Dic = new Dictionary<string, bool>();

    private GUIStyle m_Style = new GUIStyle();
    void OnEnable()
    {
        m_Style.alignment = TextAnchor.MiddleCenter;

        m_strMacro = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
        Debug.Log(m_strMacro);

        m_ListMacroItem.Clear();
        m_ListMacroItem.Add(new MacroItem("DEBUG_MODE","调试模式",true,false));
        m_ListMacroItem.Add(new MacroItem("DEBUG_LOG", "打印日志", true, false));
        m_ListMacroItem.Add(new MacroItem("STAT_TD", "开启统计", false, true));
        m_ListMacroItem.Add(new MacroItem("OUTER_NET", "是否使用外网", false, true));
        m_ListMacroItem.Add(new MacroItem("IS_SHUANGLIAO", "双辽", false,false));
        m_ListMacroItem.Add(new MacroItem("IS_LONGGANG", "龙港", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_TAILAI", "泰来", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_LEPING", "乐平", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_HONGHU", "鸿鹄", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_LUALU", "撸啊撸", false, false));
        //m_ListMacroItem.Add(new MacroItem("IS_BAODI", "宝坻", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_PAIJIU", "牌九", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_WUANJUN", "武安郡", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_LAOGUI", "老鬼", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_JUYOU", "聚友", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_DAZHONG", "大众", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_WANGPAI", "王牌", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_HAOPAI", "好牌", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_GONGXIAN", "珙县", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_ZHANGJIAKOU", "张家口", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_CANGZHOU", "沧州", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_WANGQUE", "网雀", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_BAODING", "保定", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_GUGENG", "古耿", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_BAODINGQIPAI", "保定棋牌", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_CHUANTONGPAIJIU","传统牌九",false,false));
        m_ListMacroItem.Add(new MacroItem("IS_LAIJU", "涞聚棋牌", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_ZHIZUNWANPAI", "至尊玩牌", false, false));
        m_ListMacroItem.Add(new MacroItem("IS_ZHENJIANG", "镇江麻将", false, false));
        m_ListMacroItem.Add(new MacroItem("DISABLE_ASSETBUNDLE", "禁用AssetBundle", false, false));

        for (int i = 0; i < m_ListMacroItem.Count; ++i)
        {
            m_Dic[m_ListMacroItem[i].Name] = false;
        }
        string[] macro = m_strMacro.Split(';');
        for (int j = 0; j < macro.Length; ++j)
        {
            for (int i = 0; i < m_ListMacroItem.Count; ++i)
            {
                if (macro[j].Equals(m_ListMacroItem[i].Name))
                {
                    m_Dic[m_ListMacroItem[i].Name] = true;
                }
            }
        }

    }
    

    void OnGUI()
    {
        GUILayout.BeginHorizontal("Box");

        GUILayout.Label("当前是否启用宏", GUILayout.Width(200));
        GUILayout.Space(10);
        GUILayout.Label("Debug环境是否启用", m_Style, GUILayout.Width(200));
        GUILayout.Space(10);
        GUILayout.Label("Release环境是否启用", m_Style, GUILayout.Width(200));

        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();

        for (int i = 0; i < m_ListMacroItem.Count; ++i)
        {
            GUILayout.BeginHorizontal();
            m_Dic[m_ListMacroItem[i].Name] = GUILayout.Toggle(m_Dic[m_ListMacroItem[i].Name], m_ListMacroItem[i].DisplayName, GUILayout.Width(200));
            GUILayout.Space(10);
            GUILayout.Label(m_ListMacroItem[i].IsDebug.ToString(), m_Style, GUILayout.Width(200));
            GUILayout.Space(10);
            GUILayout.Label(m_ListMacroItem[i].IsRelease.ToString(), m_Style, GUILayout.Width(200));
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("保存", GUILayout.Width(200)))
        {
            SaveMacro();
        }
        GUILayout.Space(10);
        if (GUILayout.Button("切换调试版本", GUILayout.Width(200)))
        {
            for (int i = 0; i < m_ListMacroItem.Count; ++i)
            {
                m_Dic[m_ListMacroItem[i].Name] = m_ListMacroItem[i].IsDebug;
            }
            SaveMacro();
        }
        GUILayout.Space(10);
        if (GUILayout.Button("切换发布版本", GUILayout.Width(200)))
        {
            for (int i = 0; i < m_ListMacroItem.Count; ++i)
            {
                m_Dic[m_ListMacroItem[i].Name] = m_ListMacroItem[i].IsRelease;
            }
            SaveMacro();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void SaveMacro()
    {
        m_strMacro = string.Empty;

        string strGameName = string.Empty;
        foreach (var item in m_Dic)
        {
            if (item.Value)
            {
                if (item.Key.IndexOf("IS_", System.StringComparison.CurrentCultureIgnoreCase) > -1)
                {
                    strGameName = item.Key;
                    break;
                }

            }
        }
        
        bool isSelect = false;
        foreach (var item in m_Dic)
        {
            if (item.Value && item.Key.IndexOf("IS_") > -1)
            {
                string gameName = item.Key.Substring(3, item.Key.Length - 3);
                Debug.Log("当前选择游戏:" + gameName);
                isSelect = true;
                EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
                for (int i = 0; i < scenes.Length; ++i)
                {
                    if (scenes[i].path.IndexOf(gameName, System.StringComparison.CurrentCultureIgnoreCase) > -1 
                        || scenes[i].path.IndexOf("commonasset", System.StringComparison.CurrentCultureIgnoreCase) > -1)
                    {
                        Debug.Log("启用场景 : " + scenes[i].path);
                        scenes[i].enabled = true;
                    }
                    else
                    {
                        scenes[i].enabled = false;
                    }
                }
                EditorBuildSettings.scenes = scenes;
            }
        }
        if (!isSelect)
        {
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            Debug.Log("当前选择游戏: WANGQUE");
            for (int i = 0; i < scenes.Length; ++i)
            {
                if (scenes[i].path.IndexOf("wangque", System.StringComparison.CurrentCultureIgnoreCase) > -1
                    || scenes[i].path.IndexOf("commonasset", System.StringComparison.CurrentCultureIgnoreCase) > -1)
                {
                    Debug.Log("启用场景 : " + scenes[i].path);
                    scenes[i].enabled = true;
                }
                else
                {
                    scenes[i].enabled = false;
                }
            }
            EditorBuildSettings.scenes = scenes;
        }


        foreach (var item in m_Dic)
        {
            if (item.Value)
            {
                m_strMacro += string.Format("{0};", item.Key);
            }
            if (item.Key.Equals("DISABLE_ASSETBUNDLE"))
            {
                EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
                for (int i = 0; i < scenes.Length; ++i)
                {
                    if (scenes[i].path.IndexOf("download/", System.StringComparison.CurrentCultureIgnoreCase) > -1)
                    {
                        if (scenes[i].enabled)
                        {
                            scenes[i].enabled = item.Value;
                        }
                    }
                }
                EditorBuildSettings.scenes = scenes;
            }
        }




        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, m_strMacro);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, m_strMacro);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, m_strMacro);
    }
    
}
