//===================================================
//Author      : DRB
//CreateTime  ：4/18/2017 11:56:09 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMicroView : UIWindowViewBase
{
    [SerializeField]
    private Image m_ImageLevel;

    public void SetMicroVolumeLevel(int level)
    {
        m_ImageLevel.fillAmount = level / 4.0f;
    }
}
