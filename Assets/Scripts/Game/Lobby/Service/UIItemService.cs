//===================================================
//Author      : DRB
//CreateTime  ：5/20/2017 7:10:04 PM
//Description ：
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIItemService : UIItemBase 
{
    [SerializeField]
    private Text m_TextKey;
    [SerializeField]
    private Text m_TextValue;
    [SerializeField]
    private Button btnCopy;

    private string m_Content;

    private Action<string> m_OnClick;

    protected override void OnAwake()
    {
        base.OnAwake();
        btnCopy.gameObject.GetOrCreatComponent<Button>().onClick.AddListener(OnBtnClick);
    }

    private void OnBtnClick()
    {
        if (m_OnClick != null)
        {
            m_OnClick(m_Content);
        }
    }

    public void SetUI(string key,string value,Action<string> onClick)
    {
        m_TextKey.SafeSetText(key);
        //m_TextValue.SafeSetText(value + "<size=20>(点击复制)</size>");
        m_TextValue.SafeSetText(value);
        m_Content = value;
        m_OnClick = onClick;
    }
}
