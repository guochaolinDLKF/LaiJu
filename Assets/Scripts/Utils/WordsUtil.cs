//===================================================
//Author      : DRB
//CreateTime  ：1/19/2018 10:17:32 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;
public class WordsUtil
{


    private static string digit = "零一二三四五六七八九";
    //private static string dom = "千百十万千百十亿千百十万千百十元角分里";
    private static string dom = "千百十万千百十亿千百十万千百十    ";


    /// <summary>
    /// 数值转换为中文
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ConvertToChinese(double value)
    {
        string valueStr = value.ToString("0000000000000000.000");
        valueStr = valueStr.Remove(valueStr.IndexOf('.', 1));

        StringBuilder chBuilder = new StringBuilder();

        for (int i = 0; i < valueStr.Length; ++i)
        {
            if (valueStr[i] != '0')
            {
                chBuilder.Append(digit[valueStr[i] - '0']);
                chBuilder.Append(dom[i]);
            }
            else if(i > 0 && valueStr[i - 1] != '0')
            {
                chBuilder.Append(digit[valueStr[i] - '0']);
            }

        }
        string ret= chBuilder.ToString().Trim("零".ToCharArray());
        return ret.Trim();
    }

}
