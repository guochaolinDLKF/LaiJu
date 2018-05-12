//===================================================
//Author      : DRB
//CreateTime  ：11/15/2017 8:16:20 PM
//Description ：
//===================================================
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System;

public class FontWindow : EditorWindow
{

    private Font m_SelectFont;

    void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("目标字体:");
        m_SelectFont = (Font)EditorGUILayout.ObjectField(m_SelectFont, typeof(Font), true, GUILayout.MinWidth(100f));
        GUILayout.Space(10);
        if (GUILayout.Button("确认修改"))
        {
            OnModifyClick(m_SelectFont);
        }
    }

    private void OnModifyClick(Font font)
    {
        string[] subFolders = Directory.GetDirectories(Application.dataPath + "/download/");
        SaveFolderSettings(subFolders, font);
    }


    /// <summary>
    /// 保存文件夹下所有资源包名和后缀名
    /// </summary>
    /// <param name="arrFolder"></param>
    /// <param name="isSetNull"></param>
    private void SaveFolderSettings(string[] arrFolder, Font font)
    {
        if (arrFolder != null && arrFolder.Length > 0)
        {
            foreach (string folderPath in arrFolder)
            {
                string[] arrFile = Directory.GetFiles(folderPath);
                if (arrFile != null && arrFile.Length > 0)
                {
                    foreach (string filePath in arrFile)
                    {
                        SaveFileBundleNameAndVariant(filePath, font);
                    }
                }

                string[] subFolders = Directory.GetDirectories(folderPath);
                SaveFolderSettings(subFolders, font);
            }
        }
    }


    /// <summary>
    /// 保存资源包名和后缀名
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="isSetNull"></param>
    private void SaveFileBundleNameAndVariant(string filePath, Font font)
    {
        FileInfo file = new FileInfo(filePath);
        if (!file.Extension.Equals(".meta", StringComparison.CurrentCultureIgnoreCase))
        {
            int index = filePath.IndexOf("Assets/", StringComparison.CurrentCultureIgnoreCase);

            string newPath = filePath.Substring(index);

            GameObject go = AssetDatabase.LoadAssetAtPath(newPath, typeof(GameObject)) as GameObject;
            if (go == null) return;

            ChangeChildText(go, font);
        }
    }

    private void ChangeChildText(GameObject root, Font font)
    {
        GameObject go = root;
        for (int i = 0; i < root.transform.childCount; ++i)
        {
            go = root.transform.GetChild(i).gameObject;
            Text text = go.GetComponent<Text>();
            if (text != null)
            {
                text.font = font;
                EditorUtility.SetDirty(text);
            }
            ChangeChildText(go, font);
        }
    }

}
