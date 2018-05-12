//===================================================
//Author      : DRB
//CreateTime  ：3/28/2017 1:40:26 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISceneInitView : UISceneViewBase
{
    [SerializeField]
    private Text m_Text;
    [SerializeField]
    private Slider m_Slider;

    [SerializeField]
    private Text m_LocalVersion;
    [SerializeField]
    private Text m_ServerVersion;

    protected override void OnStart()
    {
        base.OnStart();
        UIViewManager.Instance.CurrentUIScene = this;
    }

    public void SetUI(int currentCount,int totalCount,int currentSize,int totalSize)
    {
        m_Text.SafeSetText(string.Format("已完成{0}/{1}个文件，{2}/{3}KB", currentCount, totalCount, currentSize, totalSize));
        m_Slider.SafeSetSliderValue((float)currentSize / totalSize);
    }

    public void SetVersionInfo(string localVersion,string serverVersion)
    {
        m_LocalVersion.SafeSetText("客户端版本:" + localVersion);
        m_ServerVersion.SafeSetText("服务器版本" + serverVersion);
    }
}
