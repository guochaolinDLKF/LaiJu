//===================================================
//Author      : DRB
//CreateTime  ：4/7/2017 5:46:42 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopWindow : UIWindowViewBase
{
    [SerializeField]
    private Transform m_ShopContainer;
    [SerializeField]
    private Text m_TxtBindCode;
    [SerializeField]
    private Image m_ImgAddition;
    [SerializeField]
    private GameObject m_Bind;
    [SerializeField]
    private Text m_TextAgent;
    [SerializeField]
    private Button m_ButtonCopy;
    [SerializeField]
    private string m_strAgent;


    private Action<string> m_OnClick;

    

    protected override void OnAwake()
    {
        base.OnAwake();
        m_ButtonCopy.onClick.AddListener(OnBtnClick);
    }

    public void SetUI(TransferData data,Action<int> onClick, Action<string> onClickCopy) //value 表示要复制的内容
    {
        int bindCode = data.GetValue<int>("BindCode");

        m_TxtBindCode.SafeSetText(bindCode.ToString());

        if (m_Bind != null)
        {
            m_Bind.SetActive(bindCode > 0);
        }
        if (m_ImgAddition != null)
        {
            m_ImgAddition.gameObject.SetActive(bindCode > 0);
        }       
        if (onClickCopy != null)
        {
            m_OnClick = onClickCopy;
        }
        if (m_TextAgent != null)
        {
            m_TextAgent.SafeSetText(m_strAgent);
        } 
        bool isBind = true;
#if IS_LAOGUI
        isBind = bindCode > 0;
#endif
        List<TransferData> lstShop = data.GetValue<List<TransferData>>("ShopList");
        for (int i = 0; i < lstShop.Count; ++i)
        {
            TransferData shopData = lstShop[i];
            int id = shopData.GetValue<int>("ShopId");
            string name = shopData.GetValue<string>("ShopName");
            string iosCode = shopData.GetValue<string>("IosCode");
            int money = shopData.GetValue<int>("Money");
            int amount = shopData.GetValue<int>("Amount");
            int giveCount = shopData.GetValue<int>("GiveCount");
            bool isHot = shopData.GetValue<bool>("IsHot");
            string icon = shopData.GetValue<string>("Icon");
            UIItemShop shop = UIPoolManager.Instance.Spawn("UIItemShop").GetComponent<UIItemShop>();
            shop.gameObject.SetParent(m_ShopContainer);
            shop.SetUI(icon, id, money, amount, giveCount, isHot, onClick, isBind);
        }
    }


    private void OnBtnClick()
    {
        if (m_OnClick != null)
        {
            m_OnClick(m_strAgent);
        }
    }
}
