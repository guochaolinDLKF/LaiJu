//===================================================
//Author      : DRB
//CreateTime  ：1/3/2018 6:24:41 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAgreeMentWindow : UIWindowViewBase
{
    [SerializeField]
    private Text m_AgreeMentText;



    public void SetUI(string agreeMent)
    {
        if (m_AgreeMentText == null) return;
        m_AgreeMentText.text = agreeMent;
    }

}
