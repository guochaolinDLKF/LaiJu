//===================================================
//Author      : DRB
//CreateTime  ：1/4/2018 11:20:36 AM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIItemNoticeThree : UIItemBase
{

    [SerializeField]
    private RawImage m_BgImage; //背景图片
    [SerializeField]
    private Text m_WechatNumber;//微信号
    [SerializeField]
    private GameObject m_CopyBtn;//复制按钮

  
    private int m_OnclickId;
    private Action<int> m_Onclick;
    //第一步添加图片点击回调并声明变量存储地址、声明图片按钮
    private Action<string> m_OnTextureClick; //图片点击回调
    private string m_link_url; //声明变量存储地址
    [SerializeField]
    private GameObject m_TextureBtn;//声明图片按钮
    public int OnclickId
    {
        get
        {
            return m_OnclickId;
        }

        set
        {
            m_OnclickId = value;
        }
    }

    void Awake()
    {
        EventTriggerListener.Get(m_CopyBtn).onClick = OnClickBtnEvent;   
        //第二步    注册图片按钮回调
        EventTriggerListener.Get(m_TextureBtn).onClick = OnTextureBtnEvent;         
    }
    void OnClickBtnEvent(GameObject go)
    {
        if (m_Onclick != null)
        {
            m_Onclick(OnclickId);
        }    
    }
    //第三步当点击按钮时调用回调
    void OnTextureBtnEvent(GameObject go)
    {
        if (m_OnTextureClick != null)
        {
            m_OnTextureClick(m_link_url);
        }
    }

    public void SetUI(int OnclickId, string imgUrl,string linkUrl, string WechatNumber, Action<int> onClick,Action<string> onTexClick ) 
    { 
        Debug.Log(WechatNumber + "================================"); 
        TextureManager.Instance.LoadHead(imgUrl, (Texture2D tex) =>
        {
            if(tex!=null)
            {
                m_BgImage.texture = tex;
            }
        },true);
        this.OnclickId = OnclickId;
       
        m_WechatNumber.text = WechatNumber;
        m_Onclick = onClick;
        //第四步在参数中添加回调并且赋值回调
        m_link_url = linkUrl;
        m_OnTextureClick = onTexClick;
    }

 
}
