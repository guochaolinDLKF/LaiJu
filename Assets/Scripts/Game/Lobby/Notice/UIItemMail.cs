//===================================================
//Author      : DRB
//CreateTime  ：5/12/2017 11:38:15 AM
//Description ：邮件UI项
//===================================================
using UnityEngine;
using UnityEngine.UI;


public class UIItemMail : MonoBehaviour 
{
    [SerializeField]
    private Text m_TextDateTime;

    [SerializeField]
    private Text m_TextContent;

    public void SetUI(string content,string dateTime)
    {
        m_TextContent.SafeSetText(content);
        m_TextDateTime.SafeSetText(dateTime);
    }
}
