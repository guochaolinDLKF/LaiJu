//===================================================
//Author      : DRB
//CreateTime  ：6/23/2017 10:30:07 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIZhaJHItemJSInfo : MonoBehaviour {
     
    [SerializeField]
    private Text m_IDtext;
    [SerializeField]
    private Text m_Gold;

    public void Assignment(string id,float gold)
    {
        if (m_IDtext!=null) m_IDtext.text = id;
        if (m_Gold!=null) m_Gold.text = gold.ToString();
    }

   

}
