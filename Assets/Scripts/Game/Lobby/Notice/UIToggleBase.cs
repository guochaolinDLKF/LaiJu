//===================================================
//Author      : DRB
//CreateTime  ：12/13/2017 10:33:17 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleBase : MonoBehaviour {


    void Start()
    {
        EventTriggerListener.Get(gameObject).onClick = OnChangedTag;
    }

    protected virtual void OnChangedTag(GameObject go)
    {

    }
}
