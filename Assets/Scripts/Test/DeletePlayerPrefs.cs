//===================================================
//Author      : DRB
//CreateTime  ：3/8/2017 5:32:08 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DeletePlayerPrefs : MonoBehaviour
{

#if UNITY_EDITOR
    void Update ()
    {
        if (Application.isPlaying) return;
        PlayerPrefs.DeleteAll();
        Debug.Log("清除缓存成功！！！！");
	}
#endif
}
