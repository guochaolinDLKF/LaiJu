//===================================================
//Author      : DRB
//CreateTime  ：7/5/2016 3:56:17 PM
//Description ：
//===================================================
using UnityEngine;
using System.Collections;

public class MacroItem
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public bool IsDebug { get; set; }
    public bool IsRelease { get; set; }

    public MacroItem(string name,string displayName,bool isDebug,bool isRelease)
    {
        Name = name;
        DisplayName = displayName;
        IsDebug = isDebug;
        IsRelease = isRelease;
    }
}
