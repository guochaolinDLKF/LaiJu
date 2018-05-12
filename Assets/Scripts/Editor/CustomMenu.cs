//===================================================
//Author      : DRB
//CreateTime  ：7/5/2016 7:16:22 PM
//Description ：自定义菜单
//===================================================
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;
using UnityEngine.UI;

public class CustomMenu
{
    [MenuItem("DRB/AssetBundle/资源打包管理")]
    public static void OpenAssetBundleTool()
    {
        AssetBundleWindow win = EditorWindow.GetWindow<AssetBundleWindow>();
        win.titleContent.text = "AssetBundle工具";
        win.Show();
    }

    [MenuItem("DRB/AssetBundle/初始资源拷贝到StreamingAssets")]
    public static void AssetBundleCopyToStreamingAssets()
    {
        string toPath = Application.streamingAssetsPath + "/AssetBundles/";

        if (Directory.Exists(toPath))
        {
            Directory.Delete(toPath, true);
        }
        Directory.CreateDirectory(toPath);

        IOUtil.CopyDirectory(Application.persistentDataPath, toPath);
        AssetDatabase.Refresh();
        Debug.Log("拷贝完毕");
    }

    [MenuItem("DRB/宏设置")]
    public static void OpenSettingTool()
    {
        SettingWindow win = EditorWindow.GetWindow<SettingWindow>();
        win.titleContent.text = "宏设置";
        win.minSize = new Vector2(635, 200);
        win.maxSize = new Vector2(635, 630);
        win.Show();
    }


    [MenuItem("DRB/清除缓存")]
    public static void ClearCache()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("DRB/运行游戏")]
    public static void RunGame()
    {
        EditorApplication.OpenScene(string.Format("Assets/Scenes/{0}/Scene_Init.unity", ConstDefine.GAME_NAME));
        EditorApplication.isPlaying = true;
    }

    [MenuItem("DRB/字体设置")]
    public static void OpenFontTool()
    {
        FontWindow win = EditorWindow.GetWindow<FontWindow>();
        win.titleContent.text = "字体设置";
        win.minSize = new Vector2(635, 200);
        win.maxSize = new Vector2(635, 630);
        win.Show();
    }


    private static int count = 0;

    [MenuItem("DRB/计算代码行数")]
    public static void CalculateCount()
    {
        count = 0;

        string[] path = new string[1] { "D:/svndata/client/common/Assets/Scripts/" };
        CalculateLine(path);
        
        EditorWindow.focusedWindow.ShowNotification(new GUIContent(string.Format("谁这么无聊写了{0}行代码",count.ToString())));
    }



    /// <summary>
    /// 保存文件夹下所有资源包名和后缀名
    /// </summary>
    /// <param name="arrFolder"></param>
    /// <param name="isSetNull"></param>
    private static void CalculateLine(string[] arrFolder)
    {
        if (arrFolder != null && arrFolder.Length > 0)
        {
            foreach (string folderPath in arrFolder)
            {
                if (!Directory.Exists(folderPath)) continue;
                string[] arrFile = Directory.GetFiles(folderPath);
                if (arrFile != null && arrFile.Length > 0)
                {
                    foreach (string filePath in arrFile)
                    {
                        if (filePath.IndexOf(".meta") > -1) continue;
                        //if (filePath.IndexOf(@"\Proto\") > -1) continue;
                        if (filePath.IndexOf("/protobuf\\") > -1) continue;
                        string content = IOUtil.GetFileText(filePath);
                        string[] result = content.Split(new string[1] { "\n"},StringSplitOptions.None);
                        count += result.Length;
                    }
                }

                string[] subFolders = Directory.GetDirectories(folderPath);
                CalculateLine(subFolders);
            }
        }
    }

}
