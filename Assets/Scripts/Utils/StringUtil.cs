
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

/// <summary>
/// 
/// </summary>
public static class StringUtil 
{
    /// <summary>
    /// 把string类型转换成int
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int ToInt(this string str)
    {
        int temp = 0;
        int.TryParse(str, out temp);
        return temp;
    }

    /// <summary>
    /// 把string类型转换成long
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static long ToLong(this string str)
    {
        long temp = 0;
        long.TryParse(str, out temp);
        return temp;
    }

    /// <summary>
    /// 把string类型转换成float
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static float ToFloat(this string str)
    {
        float temp = 0;
        float.TryParse(str, out temp);
        return temp;
    }


    public static bool ToBool(this string str)
    {
        if (string.IsNullOrEmpty(str)) return false;
        bool temp = false;
        bool.TryParse(str,out temp);
        if (!temp)
        {
            if (str.Trim().Equals("1"))
            {
                temp = true;
            }
        }
        return temp;
    }

    public static byte[] ToBytes(this string str)
    {
        return System.Text.Encoding.UTF8.GetBytes(str);
    }

    public static string ToUnicodeString(this byte[] bytes)
    {
        return System.Text.Encoding.Unicode.GetString(bytes);
    }

    public static string ToUTF8String(this byte[] bytes)
    {
        return System.Text.Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// 工具string转vector3
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static Vector3 ToVector3(this string str)
    {
        string[] arr = str.Split('_');
        if (arr.Length < 3)
        {
            return Vector3.zero;
        }

        float x, y, z;
        float.TryParse(arr[0], out x);
        float.TryParse(arr[1], out y);
        float.TryParse(arr[2], out z);
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// 正则
    /// </summary>
    /// <param name="str"></param>
    /// <param name="pattener"></param>
    /// <returns></returns>
    public static bool Regex(this string str,string pattener)
    {
        if (string.IsNullOrEmpty(str)) return false;

        Regex reg = new Regex(pattener);
        Match match = reg.Match(str);
        return match.Success;
    }

    public static string FormatWith(this string str,params object[] obj)
    {
        return string.Format(str,obj);
    }
}