//===================================================
//Author      : DRB
//CreateTime  ：7/6/2017 4:47:41 PM
//Description ：邀请码窗口
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIInviteView : UIWindowViewBase 
{
    [SerializeField]
    private InputField m_InputInviteCode;

    [SerializeField]
    private Text m_TextBindCode;

    [SerializeField]
    private GameObject m_BindPage;
    [SerializeField]
    private GameObject m_UnBindPage;


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnInviteViewOk":
                SendNotification("btnInviteViewOk", m_InputInviteCode.text.ToInt());
                break;
        }
    }


    public void SetUI(int bindCode)
    {
        if (bindCode > 0)
        {
            m_TextBindCode.SafeSetText(bindCode.ToString());
        }
        m_BindPage.SetActive(bindCode > 0);
        m_UnBindPage.SetActive(bindCode == 0);
    }
}
