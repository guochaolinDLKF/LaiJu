//===================================================
//Author      : DRB
//CreateTime  ：5/26/2017 10:31:34 AM
//Description ：
//===================================================
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UISceneLoadingView : UISceneViewBase
{
    //[SerializeField]
    //private Slider m_Progress;
    [SerializeField]
    private Text m_UILabel;

    //public void SetProgressValue(float value)
    //{
    //    if (m_Progress == null || m_UILabel == null)
    //    {
    //        return;
    //    }
    //    m_Progress.value = value;
    //    m_UILabel.text = string.Format("{0}%", (int)(value * 100));
    //}

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        //m_Progress = null;
        m_UILabel = null;
    }
}
