//===================================================
//Author      : DRB
//CreateTime  ：7/4/2016 1:23:24 AM
//Description ：本地文件管理器
//===================================================
using UnityEngine;
using System.Collections;
using System.IO;


public class LocalFileManager : Singleton<LocalFileManager>
{
    #region Variation
    public readonly string LocalFilePath = Application.persistentDataPath + "/";
    #endregion

    #region Public Function
    /// <summary>
    /// 读取本地文件到byte数组
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public byte[] GetBuffer(string path)
    {
        byte[] buffer = null;
        using (FileStream fs = new FileStream(path,FileMode.Open))
        {
            buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
        }
        return buffer;
    }
    #endregion
}
