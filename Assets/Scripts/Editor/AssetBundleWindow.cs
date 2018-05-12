//===================================================
//Author      : DRB
//CreateTime  ：7/5/2016 7:26:23 PM
//Description ：AssetBundle工具
//===================================================
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
/// <summary>
/// AssetBundleEditorWindow
/// </summary>
public class AssetBundleWindow : EditorWindow
{
    private AssetBundleDAL m_DAL;

    private List<AssetBundleEntity> m_List;

    private Dictionary<string, bool> m_Dic;

    private string[] arrTag = { "ALL", "Scene", "Prefab", "Audio","UI","Effect", "None" };//资源标签
    private int tagIndex = 0;//标签的索引
    private int selectTagIndex = -1;//选择的标签的索引

    private string[] arrBuildTarget = { "Windows", "Android", "IOS" };//目标平台
    private int selectBuildTargetIndex = -1;//选择的打包平台索引
#if UNITY_STANDALONE_WIN
    private BuildTarget target = BuildTarget.StandaloneWindows;

    private int buildTargetIndex = 0;//打包平台索引
#elif UNITY_ANDROID
     private BuildTarget target = BuildTarget.Android;

    private int buildTargetIndex = 1;
#elif UNITY_IPHONE
    private BuildTarget target = BuildTarget.iOS;

    private int buildTargetIndex = 2;
#elif UNITY_STANDALONE_OSX
    private BuildTarget target = BuildTarget.StandaloneWindows;

    private int buildTargetIndex = 0;
#endif

    private string[] arrGame = 
        {
        "all",
        "wangque",
        "shuangliao",
        "tailai",
        "longgang",
        "leping",
        "paijiu",
        "honghu",
        "lualu",
        "baodi",
        "wuanjun",
        "laogui" ,
        "juyou",
        "dazhong",
        "wangpai",
        "haopai",
        "gongxian",
        "zhangjiakou",
        "cangzhou",
        "baoding",
        "gugeng",
        "baodingqipai",
        "chuantongpaijiu",
        "laiju",
        "zhizunwanpai",
        "zhenjiang",
        };//所有游戏
    private int selectGameIndex = -1;//选择的游戏索引
    private int gameIndex = 0;//游戏的索引

    private string m_ToPath = "D:/svndata/assetbundles/";



    private Vector2 pos;

    void OnEnable()
    {
        string xmlPath = Application.dataPath + @"/Editor/AssetBundle/AssetBundleConfig.xml";
        m_DAL = new AssetBundleDAL(xmlPath);
        m_List = m_DAL.GetList();
        m_Dic = new Dictionary<string, bool>();

        if (m_List == null)
        {
            m_List = new List<AssetBundleEntity>();
        }

        for (int i = 0; i < m_List.Count; ++i)
        {
            if (m_List[i].Game.Equals(arrGame[gameIndex], StringComparison.CurrentCultureIgnoreCase) ||
                m_List[i].Game.Equals("common", StringComparison.CurrentCultureIgnoreCase))
            {
                m_Dic[m_List[i].Key] = true;
            }
            else
            {
                m_Dic[m_List[i].Key] = false;
            }
        }
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal("Box");

        selectTagIndex = EditorGUILayout.Popup(tagIndex,arrTag,GUILayout.Width(100));
        if (selectTagIndex != tagIndex)
        {
            tagIndex = selectTagIndex;
            EditorApplication.delayCall = OnSelectTagCallBack;
        }

        selectBuildTargetIndex = EditorGUILayout.Popup(buildTargetIndex,arrBuildTarget,GUILayout.Width(100));
        if (selectBuildTargetIndex != buildTargetIndex)
        {
            buildTargetIndex = selectBuildTargetIndex;
            EditorApplication.delayCall = OnSelectBuildTargetCallBack;
        }

        selectGameIndex = EditorGUILayout.Popup(gameIndex, arrGame,GUILayout.Width(100));
        if (selectGameIndex != gameIndex)
        {
            gameIndex = selectGameIndex;
            EditorApplication.delayCall = OnSelectGameCallBack;
        }

        if (GUILayout.Button("保存设置", GUILayout.Width(100)))
        {
            EditorApplication.delayCall = OnSaveAssetBundleCallBack;
        }

        if (GUILayout.Button("打包", GUILayout.Width(100)))
        {
            EditorApplication.delayCall = OnBuildAssetBundleCallBack;
        }

        if (GUILayout.Button("清空", GUILayout.Width(100)))
        {
            EditorApplication.delayCall = OnClearAssetBundleCallBack;
        }

        if (GUILayout.Button("拷贝数据表", GUILayout.Width(100)))
        {
            EditorApplication.delayCall = OnCopyDataTableCallBack;
        }

        if (GUILayout.Button("生成版本文件", GUILayout.Width(100)))
        {
            EditorApplication.delayCall = OnCreateVersionInfoCallBack;
        }


        GUILayout.EndHorizontal();



        GUILayout.BeginHorizontal("Box");

        GUILayout.Label("包名");
        GUILayout.Label("所属游戏", GUILayout.Width(100));
        GUILayout.Label("标记",GUILayout.Width(100));
        GUILayout.Label("是否文件夹",GUILayout.Width(100));
        GUILayout.Label("是否初始资源",GUILayout.Width(100));

        GUILayout.EndHorizontal();



        GUILayout.BeginVertical();

        if (m_List == null) return;
        pos = EditorGUILayout.BeginScrollView(pos);

        for (int i = 0; i < m_List.Count; ++i)
        {
            AssetBundleEntity entity = m_List[i];
            GUILayout.BeginHorizontal();

            m_Dic[entity.Key] = GUILayout.Toggle(m_Dic[entity.Key],"",GUILayout.Width(100));

            GUILayout.Label(entity.Name);
            GUILayout.Label(entity.Game, GUILayout.Width(100));
            GUILayout.Label(entity.Tag, GUILayout.Width(100));
            GUILayout.Label(entity.IsFolder.ToString(), GUILayout.Width(100));
            GUILayout.Label(entity.IsFirstData.ToString(), GUILayout.Width(100));
            GUILayout.EndHorizontal();

            foreach (string path in entity.PathList)
            {
                GUILayout.BeginHorizontal("box");
                GUILayout.Space(40);

                GUILayout.Label(path);
                GUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();
    }
    

    /// <summary>
    /// 保存设置回调
    /// </summary>
    private void OnSaveAssetBundleCallBack()
    {
        List<AssetBundleEntity> listNeedBuild = new List<AssetBundleEntity>();

        foreach (AssetBundleEntity entity in m_List)
        {
            if (m_Dic[entity.Key])
            {
                entity.IsChecked = true;
                listNeedBuild.Add(entity);
            }
            else
            {
                entity.IsChecked = false;
                listNeedBuild.Add(entity);
            }
        }
        for (int i = 0; i < listNeedBuild.Count; ++i)
        {
            AssetBundleEntity entity = listNeedBuild[i];
            if (entity.IsFolder)
            {
                string[] folderArr = new string[entity.PathList.Count];
                for (int j = 0; j < folderArr.Length; ++j)
                {
                    folderArr[j] = Application.dataPath + "/" + entity.PathList[j];
                }
                SaveFolderSettings(folderArr,!entity.IsChecked);
            }
            else
            {
                string[] FileArr = new string[entity.PathList.Count];
                for (int j = 0; j < FileArr.Length; ++j)
                {
                    FileArr[j] = Application.dataPath + "/" + entity.PathList[j];
                    SaveFileBundleNameAndVariant(FileArr[j], !entity.IsChecked);
                }
            }
        }
        this.ShowNotification(new GUIContent("保存设置完成"));
        Debug.Log("保存设置完成");
    }

    /// <summary>
    /// 保存文件夹下所有资源包名和后缀名
    /// </summary>
    /// <param name="arrFolder"></param>
    /// <param name="isSetNull"></param>
    private void SaveFolderSettings(string[] arrFolder,bool isSetNull)
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
                        SaveFileBundleNameAndVariant(filePath, isSetNull);
                    }
                }

                string[] subFolders = Directory.GetDirectories(folderPath);
                SaveFolderSettings(subFolders, isSetNull);
            }
        }
    }

    /// <summary>
    /// 保存资源包名和后缀名
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="isSetNull"></param>
    private void SaveFileBundleNameAndVariant(string filePath,bool isSetNull)
    {
        if (!File.Exists(filePath)) return;
        FileInfo file = new FileInfo(filePath);
        if (!file.Extension.Equals(".meta",StringComparison.CurrentCultureIgnoreCase))
        {
            int index = filePath.IndexOf("Assets/",StringComparison.CurrentCultureIgnoreCase);

            string newPath = filePath.Substring(index);

            string fileName = newPath.Replace("Assets/","").Replace(file.Extension,"");
            fileName = fileName.Replace("CommonAsset",arrGame[gameIndex]);
            fileName = fileName.Replace('\\', '/');
            string variant = "drb";

            AssetImporter importer = AssetImporter.GetAtPath(newPath);
            if (importer == null) return;
            if (isSetNull)
            {
                if (string.IsNullOrEmpty(importer.assetBundleName))
                {
                    return;
                }
                importer.SetAssetBundleNameAndVariant("", "");
            }
            else
            {
                if ((filePath.IndexOf("/uisource/",StringComparison.CurrentCultureIgnoreCase) > -1 || filePath.IndexOf("\\uisource\\", StringComparison.CurrentCultureIgnoreCase) > -1) && file.Extension.Equals(".png",StringComparison.CurrentCultureIgnoreCase))
                {
                    string bundleName = fileName.Substring(0, fileName.LastIndexOf('/'));
                    if (importer.assetBundleName.Equals(bundleName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return;
                    }
                    importer.SetAssetBundleNameAndVariant(bundleName, variant);
                    TextureImporter txtImporter = importer as TextureImporter;
                    string tag = string.Empty;
                    if (fileName.IndexOf("/gameuisource/",StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        tag = "dynamic/";
                    }
                    else
                    {
                        tag = "static/";
                    }
                    int startIndex = bundleName.LastIndexOf('/') + 1;
                    txtImporter.spritePackingTag = tag + fileName.Substring(startIndex, bundleName.Length - startIndex).ToLower();
                }
                else
                {
                    if (importer.assetBundleName.Equals(fileName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return;
                    }
                    importer.SetAssetBundleNameAndVariant(fileName, variant);
                }
            }
            importer.SaveAndReimport();
        }
    }

    /// <summary>
    /// 选择平台回调
    /// </summary>
    private void OnSelectBuildTargetCallBack()
    {
        switch (buildTargetIndex)
        {
            case 0:
                target = BuildTarget.StandaloneWindows;
                break;
            case 1:
                target = BuildTarget.Android;
                break;
            case 2:
                target = BuildTarget.iOS;
                break;
        }
    }

    /// <summary>
    /// 选择游戏回调
    /// </summary>
    private void OnSelectGameCallBack()
    {
        //if (gameIndex == 0)
        //{
        //    foreach (AssetBundleEntity entity in m_List)
        //    {
        //        m_Dic[entity.Key] = (entity.Tag.Equals(arrTag[tagIndex], StringComparison.CurrentCultureIgnoreCase) || tagIndex == 0);
        //    }
        //    return;
        //}

        foreach (AssetBundleEntity entity in m_List)
        {
            m_Dic[entity.Key] = (entity.Game.Equals(arrGame[gameIndex], StringComparison.CurrentCultureIgnoreCase) 
                || entity.Game.Equals("Common", StringComparison.CurrentCultureIgnoreCase)) 
                && (entity.Tag.Equals(arrTag[tagIndex], StringComparison.CurrentCultureIgnoreCase) || tagIndex == 0);
        }
    }

    /// <summary>
    /// 选择标签回调
    /// </summary>
    private void OnSelectTagCallBack()
    {
        if (tagIndex == 0)
        {
            foreach (AssetBundleEntity entity in m_List)
            {
                m_Dic[entity.Key] = entity.Game.Equals(arrGame[gameIndex], StringComparison.CurrentCultureIgnoreCase)
                    || entity.Game.Equals("Common", StringComparison.CurrentCultureIgnoreCase);
            }
        }
        else
        {
            foreach (AssetBundleEntity entity in m_List)
            {
                m_Dic[entity.Key] = (entity.Game.Equals(arrGame[gameIndex], StringComparison.CurrentCultureIgnoreCase)
                    || entity.Game.Equals("Common", StringComparison.CurrentCultureIgnoreCase))
                    && entity.Tag.Equals(arrTag[tagIndex], StringComparison.CurrentCultureIgnoreCase);
            }
        }
    }

    /// <summary>
    /// 选择平台回调
    /// </summary>
    private void OnSelectTargetCallBack()
    {
        switch (buildTargetIndex)
        {
            case 0:
                target = BuildTarget.StandaloneWindows;
                break;
            case 1:
                target = BuildTarget.Android;
                break;
            case 2:
                target = BuildTarget.iOS;
                break;
        }
    }

    /// <summary>
    /// 打包按钮回调
    /// </summary>
    private void OnBuildAssetBundleCallBack()
    {
        string toPath = m_ToPath + arrGame[gameIndex] + "/" + arrBuildTarget[buildTargetIndex];
        //string toPath = Application.dataPath + "/../AssetBundles/" + arrBuildTarget[buildTargetIndex];
        if (!Directory.Exists(toPath))
        {
            Directory.CreateDirectory(toPath);
        }

        BuildPipeline.BuildAssetBundles(toPath,BuildAssetBundleOptions.None,target);

        this.ShowNotification(new GUIContent("打包完成"));
        Debug.Log("打包完成");
    }

    /// <summary>
    /// 清空按钮回调
    /// </summary>
    private void OnClearAssetBundleCallBack()
    {
        string path = Application.dataPath + "/../AssetBundles/" + arrBuildTarget[buildTargetIndex];

        if (Directory.Exists(path))
        {
            Directory.Delete(path,true);
        }
        this.ShowNotification(new GUIContent("清空完毕"));
        Debug.Log("清空完毕");
    }

    /// <summary>
    /// 复制数据表回调
    /// </summary>
    private void OnCopyDataTableCallBack()
    {
        string srcPath = Application.dataPath + "/Download/" + arrGame[gameIndex] + "/BinaryData/";
        string destPath = m_ToPath + arrGame[gameIndex] + "/" + arrBuildTarget[buildTargetIndex] + "/download/binarydata/";
        IOUtil.CopyDirectory(srcPath, destPath);
        this.ShowNotification(new GUIContent("拷贝数据表完毕"));
        Debug.Log("拷贝数据表完毕");
    }

    //private void OnCreatePackageInfoCallBack()
    //{
    //    Version v = null;
    //    //游戏名称和版本号
    //    string content = IOUtil.GetFileText(strVersionInfoPath);
    //    if (!string.IsNullOrEmpty(content))
    //    {
    //        string[] arr = content.Split('\n');
    //        string[] arr1 = arr[0].Split(' ');
    //        if (arr1.Length == 2)
    //        {
    //            string gameName = arr1[0];
    //            v = new Version(arr1[1]);
    //            v.ModifiedVersion += 1;
    //            sb.Append(gameName + " ");
    //            sb.AppendLine(v.ToString());
    //        }
    //        else
    //        {
    //            v = ConstDefine.VERSION;
    //            sb.AppendLine(ConstDefine.GAME_CHINESE_NAME + " " + ConstDefine.VERSION.ToString());
    //        }
    //    }
    //    else
    //    {
    //        v = ConstDefine.VERSION;
    //        sb.AppendLine(ConstDefine.GAME_CHINESE_NAME + " " + ConstDefine.VERSION.ToString());
    //    }

    //    IOUtil.DeleteFile(strVersionInfoPath);
    //    //APK版本信息
    //    FileInfo[] apk = directory.GetFiles(ConstDefine.GAME_NAME + ".apk", SearchOption.TopDirectoryOnly);
    //    if (apk.Length == 1)
    //    {
    //        string fullName = apk[0].FullName.Substring(0, apk[0].FullName.LastIndexOf('\\') + 1) + ConstDefine.GAME_NAME + v.ToString() + ".apk";
    //        string name = fullName.Substring(fullName.IndexOf(arrBuildTarget[buildTargetIndex]) + arrBuildTarget[buildTargetIndex].Length + 1);
    //        string size = Math.Ceiling(apk[0].Length / 1024f).ToString();
    //        IOUtil.ReName(apk[0].FullName, fullName);
    //        string md5 = EncryptUtil.GetFileMD5(fullName);
    //        string strLine = string.Format("{0} {1} {2} {3}", name, md5, size, "0");
    //        sb.AppendLine(strLine);
    //    }
    //    else if (apk == null || apk.Length == 0)
    //    {
    //        Version prevVersion = new Version(v.ToString());
    //        prevVersion.ModifiedVersion -= 1;
    //        FileInfo[] apk1 = directory.GetFiles("*.apk", SearchOption.TopDirectoryOnly);
    //        FileInfo apk2 = null;
    //        Version maxVersion = null;
    //        for (int i = 0; i < apk1.Length; ++i)
    //        {
    //            const string regex = @"\d.\d{1,4}.\d{1,4}";
    //            Regex reg = new Regex(regex);
    //            MatchCollection match = reg.Matches(apk1[i].Name);
    //            foreach (Match m in match)
    //            {
    //                Version currentVersion = new Version(m.Value);
    //                if (maxVersion == null || currentVersion > maxVersion)
    //                {
    //                    maxVersion = currentVersion;
    //                    apk2 = apk1[i];
    //                }
    //            }
    //        }
    //        if (apk2 != null)
    //        {
    //            string name = apk2.Name;
    //            string size = Math.Ceiling(apk2.Length / 1024f).ToString();
    //            string md5 = EncryptUtil.GetFileMD5(apk2.FullName);
    //            string strLine = string.Format("{0} {1} {2} {3}", name, md5, size, "0");
    //            sb.AppendLine(strLine);
    //        }
    //        else
    //        {
    //            sb.AppendLine("null null null null");
    //        }
    //    }
    //}

    /// <summary>
    /// 生成版本信息文件回调
    /// </summary>
    private void OnCreateVersionInfoCallBack()
    {
        string path = m_ToPath + arrGame[gameIndex] + "/" + arrBuildTarget[buildTargetIndex];
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string strVersionInfoPath = path + "/VersionInfo.txt";
        DirectoryInfo directory = new DirectoryInfo(path);
        StringBuilder sb = new StringBuilder();
        //资源版本信息
        FileInfo[] arrFiles = directory.GetFiles("*",SearchOption.AllDirectories);
        for (int i = 0; i < arrFiles.Length; ++i)
        {
            if (arrFiles[i].Extension.Equals(".txt", StringComparison.CurrentCultureIgnoreCase)) continue;
            if (arrFiles[i].Extension.Equals(".apk", StringComparison.CurrentCultureIgnoreCase)) continue;
            FileInfo file = arrFiles[i];
            string fullName = file.FullName.Replace('\\','/');
            string name = fullName.Substring(fullName.IndexOf(arrBuildTarget[buildTargetIndex]) + arrBuildTarget[buildTargetIndex].Length + 1);

            //if (name.Equals(arrBuildTarget[buildTargetIndex], StringComparison.CurrentCultureIgnoreCase))
            //{
            //    continue;
            //}

            string md5 = EncryptUtil.GetFileMD5(fullName);
            if (string.IsNullOrEmpty(md5)) continue;

            string size = Math.Ceiling(file.Length / 1024f).ToString();

            bool isFirstData = false;
            bool isBreak = false;
            
            for (int j = 0; j < m_List.Count; ++j)
            {
                foreach (string xmlPath in m_List[j].PathList)
                {
                    string tempPath = xmlPath;
                    if (xmlPath.IndexOf(".") != -1)
                    {
                        tempPath = xmlPath.Substring(0,xmlPath.IndexOf("."));
                    }
                    tempPath = tempPath.Replace("CommonAsset", arrGame[gameIndex]);
                    //tempPath = tempPath.Replace("/", @"\");
                    if (name.IndexOf(tempPath, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        isFirstData = m_List[j].IsFirstData;
                        isBreak = true;
                        break;
                    }
                }
                if (isBreak) break;
            }

            if (name.IndexOf("binarydata",StringComparison.CurrentCultureIgnoreCase) != -1)
            {
                isFirstData = true;
            }
            if (name.Equals(arrBuildTarget[buildTargetIndex], StringComparison.CurrentCultureIgnoreCase))
            {
                isFirstData = true;
            }
            if (name.Equals(arrBuildTarget[buildTargetIndex] + ".manifest", StringComparison.CurrentCultureIgnoreCase))
            {
                isFirstData = true;
            }

            string strLine = string.Format("{0};{1};{2};{3}", name, md5, size, isFirstData ? 1 : 0);
            sb.AppendLine(strLine);
        }

        IOUtil.CreateTextFile(strVersionInfoPath, sb.ToString());
        this.ShowNotification(new GUIContent("创建版本信息文件完毕"));
        Debug.Log("创建版本信息文件完毕");
    }
}
